using DDJSB2;
using DDJSB2.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace vJoyMapping.Pioneer.ddjsb2.Mapping
{
    public static class DdjSb2VoicemeeterHandler
    {
        public static void SliderNotifiedVM(PioneerDDJSB2 ddjsb2, string parameter, IState state)
        {
            var slider = state.Control as Slider;

            // VoiceMeter   DDJ-SB2
            //-60 to +12    0 - 127

            //(0-72) - 60
            const float divisor = 72.0f / 127.0f;
            const float adjust = 60f;

            float value = ((float)slider.Controls[0].ControlValue * divisor) - adjust;

            VoiceMeeter.Remote.SetParameter(parameter, value);
        }

        public static void DialWithDetentNotifiedVM(PioneerDDJSB2 ddjsb2, string parameter, IState state)
        {
            var dial = state.Control as Dial;
            SetDetentValueVM(ddjsb2, parameter, dial.Controls[0].ControlValue);
        }

        public static void SliderWithDetentNotifiedVM(PioneerDDJSB2 ddjsb2, string parameter, IState state)
        {
            var slider = state.Control as Slider;

            // The slider goes low to high.
            byte adjusted = (byte)((byte)0x7F - slider.Controls[0].ControlValue);

            SetDetentValueVM(ddjsb2, parameter, adjusted);
        }

        public static void SetDetentValueVM(PioneerDDJSB2 ddjsb2, string parameter, byte inputValue)
        {
            // Middle of dial/slider has detent.  

            // VoiceMeter   DDJ-SB2
            //-60 to +12    0 - 127

            //(0-72) - 60
            const byte middle = 0x40;   // Middle Dial Value

            const float adjust = 60f;
            const float lowerDivisor = 60.0f / (float)middle;
            const float upperDivisor = 12.0f / (float)middle;

            float value = (((float)inputValue) * lowerDivisor) - adjust;

            // Are we above the middle
            if (inputValue > middle)
            {
                value = (((float)inputValue) * upperDivisor) - 11.8f;  // Doesn't quite get to +12
            }
            if (inputValue == middle)
            {
                value = 0f;
            }

            VoiceMeeter.Remote.SetParameter(parameter, value);
        }

        public static void PatchControl(string patchIdentifier, IState state)
        {
            var button = state.Control as Button;
            var on = button.NoteValue > 0;

            if (on)
            {
                // Switch On/Off
                var a1Patch = VoiceMeeter.Remote.GetParameter(patchIdentifier);
                VoiceMeeter.Remote.SetParameter(patchIdentifier, a1Patch > 0 ? 0f : 1f);
            }
        }

        public static void RestartAudio(PioneerDDJSB2 ddjsb2, Leds.Deck deck, Leds.PadGroup group, Led led, IState state)
        {
            var button = state.Control as Button;
            var on = button.NoteValue > 0;

            ddjsb2.PadLedControl(deck, group, led, false, on);

            if (on)
            {
                // Restart Audio
                VoiceMeeter.Remote.Restart();
            }
        }
    }
}
