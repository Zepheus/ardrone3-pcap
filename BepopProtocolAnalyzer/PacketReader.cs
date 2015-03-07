using System;
using System.Collections.Generic;
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

       private RingBuffer[] buffers = new RingBuffer[2];

        public string Filename { get; private set; }

        public event EventHandler<FrameReceivedEventArgs> OnFrameReceived;
        public event EventHandler<EventArgs> OnStreamFinished;

        private ICaptureDevice device;

        public PacketReader(string filename)
        {
            Filename = filename;
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
                        var ev = OnFrameReceived;
                        if (ev != null)
                        {
                            OnFrameReceived(this, new FrameReceivedEventArgs(f));
                        }
                    }
                } while (f != null);
            }

        }
    }
}
