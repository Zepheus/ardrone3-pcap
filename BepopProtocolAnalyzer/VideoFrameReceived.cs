using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopProtocolAnalyzer
{
    public class VideoFrameReceived : EventArgs
    {
        public ushort FrameNum { get; set; }
        public byte[] Data { get; set; }
    }
}
