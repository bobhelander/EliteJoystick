using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.LeoBodnar.BBI32.Models;
using vJoyMapping.Common;

namespace vJoyMapping.LeoBodnar.BBI32.Mapping
{
    public static class BBI32VoicemeeterHandler
    {
        private const string WindowsDefault = "Strip[0].Gain";
        private const string EliteDangerous = "Strip[7].Gain";
        private const string VoiceAttack = "Strip[1].Gain";
        private const string Spotify = "Strip[2].Gain";
        private const string Discord = "Strip[3].Gain";
        private const string BassShaker = "Bus[1].Gain";
        private const string Microphone = "Strip[4].Gain";

        private static float WindowsGain = 0;
        private static float EliteDangerousGain = 0;
        private static float VoiceAttackGain = 0;
        private static float SpotifyGain = 0;
        private static float DiscordGain = 0;

        // Media vKeys
        private const int MediaNext = 11;
        private const int MediaPrevious = 12;
        private const int MediaPlayPause = 14;

        // Keyboard
        private const int Space = 32;
        private const int LeftArrow = 37;
        private const int RightArrow = 39;

        public static void Process(States value, Controller controller)
        {
            // Check Mode Switch 
            if (Reactive.ButtonDown(value, (uint)BBI32Button.Button3))
            {
                PrimaryProcess(value, controller);
            }
            else if (Reactive.ButtonDown(value, (uint)BBI32Button.Button4))
            {
                AuxiliaryProcess(value, controller);
            }
            else
            {
                NormalProcess(value, controller);
            }
        }

        public static void NormalProcess(States value, Controller controller)
        {
            // Volume 1 Base Up
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button23))
            {
                EliteDangerousGain += (float)1.0;
                VoiceMeeter.Remote.SetParameter(EliteDangerous, EliteDangerousGain);
            }

            // Volume 1 Base Down
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button24))
            {
                EliteDangerousGain -= (float)1.0;
                VoiceMeeter.Remote.SetParameter(EliteDangerous, EliteDangerousGain);
            }

            // Volume 1 Tip Up
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button26))
            {
                VoiceAttackGain += (float)1.0;
                VoiceMeeter.Remote.SetParameter(VoiceAttack, VoiceAttackGain);
            }

            // Volume 1 Tip Down
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button25))
            {
                VoiceAttackGain -= (float)1.0;
                VoiceMeeter.Remote.SetParameter(VoiceAttack, VoiceAttackGain);
            }

            // Volume 2 Base Up
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button20))
            {
                SpotifyGain += (float)1.0;
                VoiceMeeter.Remote.SetParameter(Spotify, SpotifyGain);
            }

            // Volume 2 Base Down
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button19))
            {
                SpotifyGain -= (float)1.0;
                VoiceMeeter.Remote.SetParameter(Spotify, SpotifyGain);
            }

            // Volume 2 Tip Up
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button17))
            {
                DiscordGain += (float)1.0;
                VoiceMeeter.Remote.SetParameter(Discord, DiscordGain);
            }

            // Volume 2 Tip Down
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button18))
            {
                DiscordGain -= (float)1.0;
                VoiceMeeter.Remote.SetParameter(Discord, DiscordGain);
            }

            // Volume 2 Button  Spotify Play/Pause
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button21))
            {
                // Launch if not running
                if (Utils.ProcessRunning("Spotify"))
                {
                    Utils.SendVKeyToProcess("Spotify", MediaPlayPause);
                }
                else
                {
                    Utils.Launch("spotify:");
                }
            }

            // Volume 1 Button  Restart Audio Engine
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button22))
            {
                VoiceMeeter.Remote.Restart();
            }
        }

        public static void AuxiliaryProcess(States value, Controller controller)
        {
        }

        public static void PrimaryProcess(States value, Controller controller)
        {
            // Volume 1 Tip Up - Windows Volume
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button26))
            {
                WindowsGain += (float)1.0;
                VoiceMeeter.Remote.SetParameter(WindowsDefault, WindowsGain);
            }

            // Volume 1 Tip Down - Windows Volume
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button25))
            {
                WindowsGain -= (float)1.0;
                VoiceMeeter.Remote.SetParameter(WindowsDefault, WindowsGain);
            }

            // Volume 2 Base Up - Next Spotify Track
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button20))
            {
                Utils.SendVKeyToProcess("Spotify", MediaNext);
            }

            // Volume 2 Base Down - Previous Spotify Track
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button19))
            {
                Utils.SendVKeyToProcess("Spotify", MediaPrevious);
            }

            // Volume 2 Tip Up - Amazon Fast Forward 10 sec
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button17))
            {
                Utils.SendKeyToProcess("firefox", RightArrow).Wait();
            }

            // Volume 2 Tip Down - Amazon Rewind 10 sec
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button18))
            {
                Utils.SendKeyToProcess("firefox", LeftArrow).Wait();
            }

            // Volume 2 Button  Amazon Play/Pause
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button21))
            {
                Utils.SendKeyToProcess("firefox", Space).Wait();
            }

            // Volume 1 Button
            if (Reactive.ButtonPressed(value, (uint)BBI32Button.Button22))
            {
            }
        }
    }
}
