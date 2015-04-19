using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArDrone3Pcap
{
    public class FrameReceivedEventArgs : EventArgs
    {
        public Frame Frame { get; private set; }

        public FrameReceivedEventArgs(Frame frame)
        {
            Frame = frame;
        }
    }
}
