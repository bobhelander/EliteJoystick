using DDJSB2;
using DDJSB2.Controls;
using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace vJoyMapping.Pioneer.ddjsb2.Mapping
{
    public static class DdjSb2SpotifyHandler
    {
        // Media vKeys
        public const int MediaNext = 11;
        public const int MediaPrevious = 12;
        public const int MediaPlayPause = 14;

        public static void SpotifyPlayPause(PioneerDDJSB2 ddjsb2, IState state)
        {
            var button = state.Control as Button;
            var on = button.NoteValue > 0;

            if (on)
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
        }

        public static void SpotifyTrackChange(PioneerDDJSB2 ddjsb2, int mediaVkey, Leds.Deck deck, Led led, IState state)
        {
            var button = state.Control as Button;
            var on = button.NoteValue > 0;

            // Light the LED when pressed
            ddjsb2.LedControl(deck, led, false, on);

            if (on)
            {
                Utils.SendVKeyToProcess("Spotify", mediaVkey);
            }
        }

        public static void SpotifySkipCommerical(
            PioneerDDJSB2 ddjsb2,
            string patchParameter,
            IDisposable skippingSubscription,
            Leds.Deck deck,
            Leds.PadGroup group,
            Led led,
            IState state,
            DateTime lastPause)
        {
            var button = state.Control as Button;
            var on = button.NoteValue > 0;

            if (on)
            {
                // Switch On/Off
                var a1Patch = VoiceMeeter.Remote.GetParameter(patchParameter);
                VoiceMeeter.Remote.SetParameter(patchParameter, a1Patch > 0 ? 0f : 1f);

                var pauseAmount = DateTime.UtcNow - lastPause;
                var muteAmount = TimeSpan.FromSeconds(30) - pauseAmount;

                if (muteAmount > TimeSpan.FromSeconds(1))
                {
                    ddjsb2.PadLedControl(deck, group, led, false, true);
                    var skipping = System.Reactive.Linq.Observable.Timer(muteAmount);
                    skippingSubscription = skipping.Subscribe(x => ReenableSound(ddjsb2, patchParameter, skippingSubscription, deck, group, led, skipping));
                }
            }
        }

        private static void ReenableSound(
            PioneerDDJSB2 ddjsb2,
            string patchParameter,
            IDisposable skippingSubscription,
            Leds.Deck deck,
            Leds.PadGroup group,
            Led led,
            IObservable<long> observable)
        {
            VoiceMeeter.Remote.SetParameter(patchParameter, 1f);

            ddjsb2.PadLedControl(deck, group, led, false, false);
            skippingSubscription?.Dispose();
        }
    }
}
