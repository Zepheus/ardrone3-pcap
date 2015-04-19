namespace ArDrone3Pcap
{
    public class Frame
    {
        public enum FrameDirection
        {
            ToDrone,
            ToController
        }

        public double Time { get; set; }
        public FrameDirection Direction { get; private set; }
        public FrameType Type { get; private set; }
        public byte Id { get; private set; }
        public byte Seq { get; private set; }

        public byte[] Data { get; private set; }

        public Frame(FrameDirection direction, FrameType type, byte id, byte seq, byte[] data)
        {
            this.Direction = direction;
            this.Type = type;
            this.Id = id;
            this.Seq = seq;

            this.Data = data;
        }
    }
}
