using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voicemeeter
{
    public class VoicemeeterClient : IDisposable
    {
        public void Dispose()
        {
            VoiceMeeter.Remote.Logout();
        }
    }
}
