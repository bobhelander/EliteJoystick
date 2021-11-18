using EliteJoystick.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystickService.Services
{
    public class VoiceMeeterService : IVoiceMeeterService, IDisposable
    {
        private IDisposable voiceMeeterDisposable;

        public VoiceMeeterService()
        {
            // Connect to Voicemeeter. Each remote is a handle across the entire service
            voiceMeeterDisposable = VoiceMeeter.Remote.Initialize(Voicemeeter.RunVoicemeeterParam.VoicemeeterBanana).Result;
        }

        public void Dispose()
        {
            //TODO: Implement Logout()
            voiceMeeterDisposable?.Dispose();
        }
    }
}
