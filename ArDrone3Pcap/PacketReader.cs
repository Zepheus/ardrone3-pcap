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

namespace ArDrone3Pcap
{
    public class PacketReader : IDisposable
    {
        private const ushort MaxFragmentSize = 1000; //max video fragment size, can be parsed from json
        private const ushort MaxFragmentNum = 128;

        private ushort _sendPort;
        private ushort _recvPort;
        private IPAddress _droneIp;
        private IPAddress _controlIp;

        private readonly RingBuffer[] _buffers = new RingBuffer[2];

        // Video buffering
        private readonly byte[] videoBuffer = new byte[MaxFragmentNum * MaxFragmentSize];
        private ushort _currentFrameNum = 0;
        private int _currentFrameSize = 0;

        public event EventHandler<FrameReceivedEventArgs> OnFrameReceived;
        public event EventHandler<VideoFrameReceived> OnVideoFrameReceived;
        public event EventHandler<StreamFinishedEventArgs> OnStreamFinished;

        private ICaptureDevice _device;
        private DateTime _firstPacket = DateTime.MinValue;
        private readonly bool dumpVideo;

        private bool _isFile;
        private bool _hadDiscovery;


        public PacketReader(string filename, bool dumpVideo)
            : this(new CaptureFileReaderDevice(filename), dumpVideo)
        {
            _isFile = true;
        }

        public PacketReader(ICaptureDevice device, bool dumpVideo)
        {
            _device = device;
            this.dumpVideo = dumpVideo;
            _buffers[0] = new RingBuffer(Frame.FrameDirection.ToDrone);
            _buffers[1] = new RingBuffer(Frame.FrameDirection.ToController);
        }

        public void Open()
        {
            if (_isFile)
                _device.Open();
            else
                _device.Open(DeviceMode.Promiscuous, 1);
        }

        public void Start()
        {
            _device.Filter = string.Format("tcp dst port {0} or tcp src port {0}", 44444);
            _device.OnPacketArrival += device_OnPacketArrival;

            if (_isFile)
            {
                _device.Capture(); //blocks infinitely (unless file)
                var e = OnStreamFinished;
                if (e != null)
                    e(this, new StreamFinishedEventArgs(_hadDiscovery));

                _device.Close();
            }
            else
            {
                _device.StartCapture();
            }
        }

        private void ParseDataFrame(RawCapture raw)
        {
           
            var packet = PacketDotNet.Packet.ParsePacket(raw.LinkLayerType, raw.Data);
            var ethernetPacket = (EthernetPacket)packet;
            var ipv4 = (IPv4Packet)packet.PayloadPacket;

            var udpPacket = ethernetPacket.PayloadPacket.PayloadPacket as UdpPacket;
            if (udpPacket == null)
                return;

            if (_firstPacket == DateTime.MinValue)
            {
                _firstPacket = raw.Timeval.Date;
            }

            var num = ethernetPacket.DestinationHwAddress;

            //0 = toDrone, 1 = toControl
            var buffer = _buffers[ipv4.SourceAddress.Equals(_controlIp) ? 0 : 1];
            buffer.AddData(udpPacket.PayloadData);
            Frame f = null;
            var numPackets = 0;
            do
            {
                f = buffer.ReadFrame();
                if (f != null)
                {
                    numPackets++;
                    f.Time = (raw.Timeval.Date - _firstPacket).TotalSeconds;
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
            if (numPackets == 1 && buffer.BytesRemaining != 0)
            {
                Debug.WriteLine("Warning: larger packet found than MTU.");
            }
        }

        private void ParseDiscovery(RawCapture raw)
        {
            var packet = PacketDotNet.Packet.ParsePacket(LinkLayers.Ethernet, raw.Data);
            var ethernetPacket = (EthernetPacket)packet;
            var ipv4 = (IPv4Packet)ethernetPacket.PayloadPacket;

            var tcpPacket = ethernetPacket.PayloadPacket.PayloadPacket as TcpPacket;
            if (tcpPacket == null)
                return;

            if (tcpPacket.PayloadData.Length > 0)
            {
                var json = Encoding.ASCII.GetString(tcpPacket.PayloadData).TrimEnd();
                var obj = JObject.Parse(json);
                if (obj["d2c_port"] != null)
                {
                    _recvPort = (ushort)obj["d2c_port"];
                    _controlIp = ipv4.SourceAddress;
                    Debug.WriteLine("Control at {0}:{1}", ipv4.SourceAddress, _recvPort);
                }
                else if (obj["c2d_port"] != null)
                {
                    _sendPort = (ushort)obj["c2d_port"];
                    _droneIp = ipv4.SourceAddress;
                    Debug.WriteLine("Drone at {0}:{1}", ipv4.SourceAddress, _sendPort);
                }
                else
                {
                    Debug.WriteLine("Invalid TCP packet on port 44444");
                }
            }

            if (_recvPort != 0 && _sendPort != 0)
            {
                _device.Filter = string.Format("udp dst port {0} or udp dst port {1}", _sendPort, _recvPort);
                _hadDiscovery = true;
            }
        }

        void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            if (e.Packet.LinkLayerType == LinkLayers.Ethernet)
            {
                if (_hadDiscovery)
                    ParseDataFrame(e.Packet);
                else
                    ParseDiscovery(e.Packet);
            }
            else
            {
                Debug.WriteLine("Other linklayer protocol: {0}", e.Packet.LinkLayerType);
            }
        }

        public void Stop()
        {
            try
            {
                _device.StopCapture();
            }
            catch
            {
                // ignored
            }
        }

        public void Close()
        {
            _device.Close();
        }

        private void ProcessVideoFrame(Frame f)
        {
            try
            {
                ushort frameNum = f.Data[0];
                frameNum |= (ushort)(f.Data[1] << 8);

                if (frameNum != _currentFrameNum)
                {
                    // Try to flush the previous frame
                    if (_currentFrameSize > 0)
                    {
                        var data = new byte[_currentFrameSize];
                        Buffer.BlockCopy(videoBuffer, 0, data, 0, _currentFrameSize);
                        var ev = OnVideoFrameReceived;
                        if (ev != null)
                        {
                            ev(this, new VideoFrameReceived()
                            {
                                Data = data,
                                FrameNum = _currentFrameNum
                            });
                        }
                    }
                    _currentFrameNum = frameNum;
                    _currentFrameSize = 0;
                }

                var flags = f.Data[2];
                var fragNum = f.Data[3];
                var fragmentsPerFrame = f.Data[4];
                var flushFrame = (flags & 1) == 1; //when flush, dump frame already instead of assembly?
                Debug.WriteLine("FrameNum={0}, FragmentNum={1}, # Fragments={2}", frameNum, fragmentsPerFrame, fragNum);

                var offset = fragNum * MaxFragmentSize;
                var dataLen = f.Data.Length - 5;
                if (fragNum == fragmentsPerFrame - 1)
                {
                    _currentFrameSize = (fragmentsPerFrame - 1) * MaxFragmentSize + dataLen;
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

        public void Dispose()
        {
            if (_device != null)
            {
                _device.Close();
            }
        }
    }
}
