using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopProtocolAnalyzer
{
    public class RingBuffer
    {
        private const int BufferSize = 64 * 1024;

        public int BytesRemaining
        {
            get { return blen - bstart; }
        }

        private byte[] buffer = new byte[BufferSize];
        private int bstart = 0;
        private int blen = 0;

        private Frame.FrameDirection direction;

        public RingBuffer(Frame.FrameDirection direction)
        {
            this.direction = direction;
        }

        public void AddData(byte[] data)
        {
            // Ring buffer
            var len = data.Length;
            Buffer.BlockCopy(data, 0, buffer, bstart, len);
            blen += len;
        }

       

        public Frame ReadFrame()
        {
            if (blen > 7)
            {
                int frameLen = 0;
                frameLen = buffer[bstart + 3];
                frameLen |= (buffer[bstart + 4] << 8);
                frameLen |= (buffer[bstart + 5] << 16);
                frameLen |= (buffer[bstart + 6] << 24);

                if (blen >= frameLen)
                {
                    // Process packet
                    var payload = new byte[frameLen - 7];
                    Buffer.BlockCopy(buffer, bstart + 7, payload, 0, frameLen - 7);
                    var type = buffer[bstart];
                    var id = buffer[bstart + 1];
                    var seq = buffer[bstart + 2];

                    var frame = new Frame(direction, (FrameType) type, id, seq, payload);
                    bstart += frameLen;
                    blen -= frameLen;

                    if (blen == 0)
                    {
                        bstart = 0;
                    }
                    else if (bstart > 0 && (bstart + blen) >= buffer.Length)
                    {
                        Buffer.BlockCopy(buffer, bstart, buffer, 0, blen);
                        bstart = 0;
                    }

                    return frame;
                }
            }
            return null;
        }
    }
}
