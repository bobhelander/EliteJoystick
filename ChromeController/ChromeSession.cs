using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeController
{
    public class ChromeSession
    {
        public string description { get; set; }
        public string devtoolsFrontendUrl { get; set; }
        public string faviconUrl { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string webSocketDebuggerUrl { get; set; }
    }
}
