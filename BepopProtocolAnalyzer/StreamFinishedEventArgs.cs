using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepopProtocolAnalyzer
{
    public class StreamFinishedEventArgs : EventArgs
    {
        public bool FoundDiscovery { get; private set; }

        public StreamFinishedEventArgs(bool foundDiscovery)
        {
            FoundDiscovery = foundDiscovery;
        }
    }
}
