using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SharpPcap;
using SharpPcap.LibPcap;
using PacketDotNet;

namespace BepopProtocolAnalyzer
{
    public class PacketReader
    {
        private ushort sendPort;
        private ushort recvPort;
        private IPAddress droneIp;
        private IPAddress controlIp;

        private const ushort MaxFragmentSize = 1000; //max video fragment size from json
        private const ushort MaxFragmentNum = 128; //max video fragment num from json

        private RingBuffer[] buffers = new RingBuffer[2];

        // Video buffering
        private byte[] videoBuffer = new byte[MaxFragmentNum * MaxFragmentSize];
        private ushort currentFrameNum = 0;
        private int currentFrameSize = 0;

        public string Filename { get; private set; }

        public event EventHandler<FrameReceivedEventArgs> OnFrameReceived;
        public event EventHandler<VideoFrameReceived> OnVideoFrameReceived;
        public event EventHandler<StreamFinishedEventArgs> OnStreamFinished;

        private ICaptureDevice device;
        private DateTime firstPacket = DateTime.MinValue;
        private bool dumpVideo;

        public PacketReader(string filename, bool dumpVideo)
        {
            Filename = filename;
            this.dumpVideo = dumpVideo;
            buffers[0] = new RingBuffer(Frame.FrameDirection.ToDrone);
            buffers[1] = new RingBuffer(Frame.FrameDirection.ToController);
        }

        public void Open()
        {
            if (device == null)
            {   
                device = new CaptureFileReaderDevice(Filename);
                device.Open();
                device.Filter = string.Format("tcp dst port {0} or tcp src port {0}", 44444);
            }
        }

        public void Start()
        {
            if(device == null)
                Open();

            RawCapture p;
            while ((p = device.GetNextPacket()) != null)
            {
                var packet = PacketDotNet.Packet.ParsePacket(LinkLayers.Ethernet, p.Data);
                var ethernetPacket = (EthernetPacket)packet;

                var tcpPacket = (TcpPacket)ethernetPacket.PayloadPacket.PayloadPacket;
                var ipv4 = (IPv4Packet) ethernetPacket.PayloadPacket;
                if (tcpPacket.PayloadData.Length > 0)
                {
                    var json = Encoding.ASCII.GetString(tcpPacket.PayloadData).TrimEnd();
                    var obj = JObject.Parse(json);
                    if (obj["d2c_port"] != null)
                    {
                        recvPort = (ushort) obj["d2c_port"];
                        controlIp = ipv4.SourceAddress;
                        Debug.WriteLine("Control at {0}:{1}", ipv4.SourceAddress, recvPort);
                    }
                    else if (obj["c2d_port"] != null)
                    {
                        sendPort = (ushort)obj["c2d_port"];
                        droneIp = ipv4.SourceAddress;
                        Debug.WriteLine("Drone at {0}:{1}", ipv4.SourceAddress, sendPort);
                    }
                    else
                    {
                        Debug.WriteLine("Invalid TCP packet on port 44444");
                    }
                }
                if (recvPort != 0 && sendPort != 0)
                    break; //We found
            }

            // Found location
            device.Filter = string.Format("udp dst port {0} or udp dst port {1}", sendPort, recvPort);
            device.OnPacketArrival += device_OnPacketArrival;

            device.Capture();
            device.Close();

            var e = OnStreamFinished;
            if (e != null)
                OnStreamFinished(this, new StreamFinishedEventArgs(p != null));
        }

        void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            if (e.Packet.LinkLayerType == LinkLayers.Ethernet)
            {
                if (firstPacket == DateTime.MinValue)
                {
                    firstPacket = e.Packet.Timeval.Date;
                }
                var packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
                var ethernetPacket = (EthernetPacket)packet;
                var ipv4 = (IPv4Packet) packet.PayloadPacket;
                var udpPacket = (UdpPacket)ethernetPacket.PayloadPacket.PayloadPacket;

                var num = ethernetPacket.DestinationHwAddress;

                //0 = toDrone, 1 = toControl
                var buffer = buffers[ipv4.SourceAddress.Equals(controlIp) ? 0 : 1];
                buffer.AddData(udpPacket.PayloadData);
                Frame f = null;
                var numPackets = 0;
                do
                {
                    f = buffer.ReadFrame();
                    if (f != null)
                    {
                        numPackets++;
                        f.Time = (e.Packet.Timeval.Date - firstPacket).TotalSeconds;
                        if (dumpVideo && f.Id == 125)
                        {
                            // Process video data
                            ProcessVideoFrame(f);
                        }

                        var ev = OnFrameReceived;
                        if (ev != null)
                        {
                            OnFrameReceived(this, new FrameReceivedEventArgs(f));
                        }
                    }
                } while (f != null);
                if (numPackets != 1 && buffer.BytesRemaining != 0)
                {
                    Debug.WriteLine("Multiple/no packet detected.");
                }
            }

        }

        private void ProcessVideoFrame(Frame f)
        {
            try
            {
                ushort frameNum = 0;
                frameNum = f.Data[0];
                frameNum |= (ushort) (f.Data[1] << 8);

                if (frameNum != currentFrameNum)
                {
                    // Try to flush the previous frame
                    if (currentFrameSize > 0)
                    {
                        var data = new byte[currentFrameSize];
                        Buffer.BlockCopy(videoBuffer, 0, data, 0, currentFrameSize);
                        var ev = OnVideoFrameReceived;
                        if (ev != null)
                        {
                            ev(this, new VideoFrameReceived()
                            {
                                Data = data,
                                FrameNum = currentFrameNum
                            });
                        }
                    }
                    currentFrameNum = frameNum;
                    currentFrameSize = 0;
                }

                var flags = f.Data[2];
                var fragNum = f.Data[3];
                var fragmentsPerFrame = f.Data[4];
                var flushFrame = (flags & 1) == 1;
                Debug.WriteLine("FrameNum={0}, FragmentNum={1}, # Fragments={2}", frameNum, fragmentsPerFrame, fragNum);

                var offset = fragNum*MaxFragmentSize;
                var dataLen = f.Data.Length - 5;
                if (fragNum == fragmentsPerFrame - 1)
                {
                    currentFrameSize = (fragmentsPerFrame - 1)*MaxFragmentSize + dataLen;
                    Debug.WriteLine("Final frame, most likely not full size.");
                }
                else
                {
                    if (dataLen != MaxFragmentSize)
                    {
                         Debug.WriteLine("Received non-full packet in between stream.");
                    }
                }
                Buffer.BlockCopy(f.Data, 5, videoBuffer, offset, dataLen);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
