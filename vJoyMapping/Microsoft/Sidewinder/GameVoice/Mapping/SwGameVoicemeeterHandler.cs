using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Microsoft.Sidewinder.GameVoice.Mapping
{
    public static class SwGameVoicemeeterHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Process(States value, Controller controller)
        {
            // Turn Off Command Button
            if ((value.Current.Buttons & (byte)Button.CommandButton) == (byte)Button.CommandButton)
            {
                // Turn it off.  Previous state is stored in the device
                controller.SetLights((byte)0);
                return;
            }

            var button1 = (value.Current.Buttons & (byte)Button.Button1) == (byte)Button.Button1;
            var button2 = (value.Current.Buttons & (byte)Button.Button2) == (byte)Button.Button2;
            var button3 = (value.Current.Buttons & (byte)Button.Button3) == (byte)Button.Button3;
            var button4 = (value.Current.Buttons & (byte)Button.Button4) == (byte)Button.Button4;

            var all = (value.Current.Buttons & (byte)Button.ButtonAll) == (byte)Button.ButtonAll;
            var team = (value.Current.Buttons & (byte)Button.ButtonTeam) == (byte)Button.ButtonTeam;
            var mute = (value.Current.Buttons & (byte)Button.MuteButton) == (byte)Button.MuteButton;

            if (!all && !team && !mute)
            {
                VoiceMeeter.Remote.SetParameter("Strip[0].Mute", button1 ? 0 : 1);
                VoiceMeeter.Remote.SetParameter("Strip[1].Mute", button2 ? 0 : 1);
                VoiceMeeter.Remote.SetParameter("Bus[1].Mute", button3 ? 0 : 1);
                VoiceMeeter.Remote.SetParameter("Bus[2].Mute", button4 ? 0 : 1);
            }

            if (mute)
            {
                VoiceMeeter.Remote.SetParameter("Strip[0].Mute", 1);
                VoiceMeeter.Remote.SetParameter("Strip[1].Mute", 1);
                VoiceMeeter.Remote.SetParameter("Bus[1].Mute", 1);
                VoiceMeeter.Remote.SetParameter("Bus[2].Mute", 1);
            }
        }
    }
}
