using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BepopProtocolAnalyzer
{
    public class BepopServer
    {
        private const string DiscoveryResponse =
            "{ \"status\": 0, \"c2d_port\": 54321, \"arstream_fragment_size\": 1000," +
            " \"arstream_fragment_maximum_number\": 128, \"arstream_max_ack_interval\": 0," +
            " \"c2d_update_port\": 51, \"c2d_user_port\": 21 }";

        private TcpListener l;
        private bool listening;

        private RingBuffer ring;

        public event EventHandler<FrameReceivedEventArgs> OnFrameReceived;

        public BepopServer(ushort discoveryPort)
        {
            l = new TcpListener(new IPEndPoint(IPAddress.Any, discoveryPort));
            ring = new RingBuffer(Frame.FrameDirection.ToDrone); 
        }

        private async void StartUdpServer()
        {
            var udpListener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpListener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            // Important to specify a timeout value, otherwise the socket ReceiveFrom() 
            // will block indefinitely if no packets are received and the thread will never terminate
            udpListener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 100);
            udpListener.Bind(new IPEndPoint(IPAddress.Any, 54321));

            var buffer = new byte[1500];
            EndPoint sender = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                var result = await Task.Factory.FromAsync(
               (iar, s) =>
                   udpListener.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref sender, iar,
                       null),
               iar => udpListener.EndReceiveFrom(iar, ref sender), null);
                if (result > 0)
                {
                    var data = new byte[result];
                    Buffer.BlockCopy(buffer, 0, data, 0, result);
                    ring.AddData(data);

                    Frame f = null;
                    do
                    {
                        f = ring.ReadFrame();
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

        public void Start()
        {
            if (!listening)
            {
                listening = true;
                l.Start();
                Task.Factory.StartNew(StartUdpServer);
                Task.Factory.StartNew(StartAccept);
            }
        }

        public void Stop()
        {
            if (listening)
            {
                listening = false;
                l.Stop(); //TODO: cancellationtoken etc
            }
        }

        private async void StartAccept()
        {
            while (listening)
            {
                var client = await l.AcceptTcpClientAsync();
                Console.WriteLine("Client connected");
                StartClientListening(client);
            }
        }

        private async void StartClientListening(TcpClient client)
        {
            var buffer = new byte[2048];

            var data = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
            if (data > 0)
            {
                Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, data));
                // Send packet back
                byte[] resp = Encoding.ASCII.GetBytes(DiscoveryResponse);
                Buffer.BlockCopy(resp, 0, buffer, 0, resp.Length);
                buffer[resp.Length] = 0; //null terminate
                await client.GetStream().WriteAsync(buffer, 0, resp.Length + 1);
                await client.GetStream().FlushAsync();
              //  client.Close();
            }
        }
    }
}
