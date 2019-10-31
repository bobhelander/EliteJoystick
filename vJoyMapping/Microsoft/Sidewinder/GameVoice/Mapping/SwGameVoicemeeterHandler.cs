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
        private const string EliteDangerous = "Strip[7].Mute";
        private const string VoiceAttack = "Strip[1].Mute";
        private const string Spotify = "Strip[2].Mute";
        private const string BassShaker = "Bus[1].Mute";
        private const string Microphone = "Strip[4].Mute";

        public static void Process(States value, Controller controller)
        {
            // Turn Off Command Button
            if ((value.Current.Buttons & (byte)Button.CommandButton) == (byte)Button.CommandButton)
            {
                // Turn it off.  Previous state is stored in the device
                controller.SetLights((byte)0);
                return;
            }

            // Button 1:  Mute/Unmute Elite Dangerous
            // Button 2:  Mute/Unmute Voice Attack
            // Button 3:  Mute/Unmute Spotify
            // Button 4:  Mute/Unmute Bass Shaker

            // Mute Button: Mute/Unmute Microphone

            var button1 = (value.Current.Buttons & (byte)Button.Button1) == (byte)Button.Button1;
            var button2 = (value.Current.Buttons & (byte)Button.Button2) == (byte)Button.Button2;
            var button3 = (value.Current.Buttons & (byte)Button.Button3) == (byte)Button.Button3;
            var button4 = (value.Current.Buttons & (byte)Button.Button4) == (byte)Button.Button4;

            var all = (value.Current.Buttons & (byte)Button.ButtonAll) == (byte)Button.ButtonAll;
            var team = (value.Current.Buttons & (byte)Button.ButtonTeam) == (byte)Button.ButtonTeam;
            var mute = (value.Current.Buttons & (byte)Button.MuteButton) == (byte)Button.MuteButton;

            if (!all && !team && !mute)
            {
                VoiceMeeter.Remote.SetParameter(EliteDangerous, button1 ? 0 : 1);
                VoiceMeeter.Remote.SetParameter(VoiceAttack, button2 ? 0 : 1);
                VoiceMeeter.Remote.SetParameter(Spotify, button3 ? 0 : 1);
                VoiceMeeter.Remote.SetParameter(BassShaker, button4 ? 0 : 1);

                VoiceMeeter.Remote.SetParameter(Microphone, 0);
            }

            if (mute)
            {
                VoiceMeeter.Remote.SetParameter(Microphone, 1);
            }
        }

        public static void UpdateLights(Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Joystick swgv)
        {
            float button1 = VoiceMeeter.Remote.GetParameter(EliteDangerous);
            float button2 = VoiceMeeter.Remote.GetParameter(VoiceAttack);
            float button3 = VoiceMeeter.Remote.GetParameter(Spotify);
            float button4 = VoiceMeeter.Remote.GetParameter(BassShaker);

            byte lights = 0;

            if (button1 == 0) lights = (byte)(lights | (byte)Button.Button1);
            if (button2 == 0) lights = (byte)(lights | (byte)Button.Button2);
            if (button3 == 0) lights = (byte)(lights | (byte)Button.Button3);
            if (button4 == 0) lights = (byte)(lights | (byte)Button.Button4);

            swgv.Lights = lights;
        }
    }
}
