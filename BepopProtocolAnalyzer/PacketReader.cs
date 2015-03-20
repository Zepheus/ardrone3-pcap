using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpPcap;
using SharpPcap.LibPcap;
using PacketDotNet;

namespace BepopProtocolAnalyzer
{
    public class PacketReader
    {
        private const ushort SendPort = 43210;
        private const ushort RecvPort = 54321;

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
        public event EventHandler<EventArgs> OnStreamFinished;

        private ICaptureDevice device;
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
                device.Filter = string.Format("udp dst port {0} or udp dst port {1}", SendPort, RecvPort);
                device.OnPacketArrival += device_OnPacketArrival;
            }
        }

        public void Start()
        {
            if(device == null)
                Open();

            device.Capture();
            device.Close();

            var e = OnStreamFinished;
            if (e != null)
                OnStreamFinished(this, EventArgs.Empty);
        }

        void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            if (e.Packet.LinkLayerType == LinkLayers.Ethernet)
            {
                var packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
                var ethernetPacket = (EthernetPacket)packet;

                var udpPacket = (UdpPacket)ethernetPacket.PayloadPacket.PayloadPacket;

                var buffer = buffers[udpPacket.DestinationPort == RecvPort ? 0 : 1];
                buffer.AddData(udpPacket.PayloadData);
                Frame f = null;
                do
                {
                    f = buffer.ReadFrame();
                    if (f != null)
                    {
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
