using System;

namespace BepopProtocolAnalyzer
{
    public class Packet
    {
        

        public byte[] Data { get; private set; }
        public PacketType Project { get; private set; }
        public byte Class { get; private set; }
        public ushort Command { get; private set; }

        public Packet(PacketType project, byte clas, ushort command, byte[] data)
        {
            Project = project;
            Class = clas;
            Command = command;
            Data = data;
        }

        public static string GetPacketClass(Packet packet)
        {
            switch (packet.Project)
            {
                case PacketType.COMMON:
                    return ((CommonPacketClass)packet.Class).ToString();
                case PacketType.ARDRONE3:
                    return ((Ardrone3PacketClass)packet.Class).ToString();
                case PacketType.ARDRONE3DEBUG:
                    return ((Ardrone3DebugClass)packet.Class).ToString();
                default:
                    return packet.Class.ToString();
            }
        }

        public static Packet Parse(byte[] data)
        {
            byte p = data[0];
            byte c = data[1];
            ushort command = 0;
            command = data[2];
            command |= (ushort)(data[3] << 8);

            byte[] payload = new byte[data.Length - 4];
            Buffer.BlockCopy(data, 4, payload, 0, data.Length - 4);
            return new Packet((PacketType)p, c, command, payload);
        }
    }
}
