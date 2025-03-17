using EliteJoystick.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace EliteDesktopApp.Services
{
    public class VoiceMeeterService : IVoiceMeeterService, IDisposable
    {
        private IDisposable voiceMeeterDisposable;

        public VoiceMeeterService(ILogger<VoiceMeeterService> logger)
        {
            retry:

            try
            {
                // Connect to Voicemeeter. Each remote is a handle across the entire service
                voiceMeeterDisposable = VoiceMeeter.Remote.Initialize(Voicemeeter.RunVoicemeeterParam.VoicemeeterBanana).Result;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                goto retry;
            }
        }

        public void Dispose()
        {
            //TODO: Implement Logout()
            voiceMeeterDisposable?.Dispose();
        }
    }
}
