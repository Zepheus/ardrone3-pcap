using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopProtocolAnalyzer
{
    public class Frame
    {
        public enum FrameDirection
        {
            ToDrone,
            ToController
        }

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
