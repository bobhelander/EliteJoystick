using DDJSB2;
using DDJSB2.Controls;
using EliteJoystick.Common;
using EliteJoystick.Common.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace vJoyMapping.Pioneer.ddjsb2.Mapping
{
    public static class DdjSb2UtilsHandler
    {
        public static void OculusASWOff(PioneerDDJSB2 ddjsb2, Leds.Deck deck, Leds.PadGroup group, Led led, IState state)
        {
            var button = state.Control as Button;
            var on = button.NoteValue > 0;

            // Light the LED when pressed
            ddjsb2.PadLedControl(deck, group, led, false, on);

            if (on)
            {
                EliteJoystick.Common.Utils.OculusASWOff().Wait();
            }
        }

        public static void ExplorerLog(PioneerDDJSB2 ddjsb2, Leds.Deck deck, Leds.PadGroup group, Led led, IState state)
        {
            var button = state.Control as Button;
            var on = button.NoteValue > 0;

            // Light the LED when pressed
            ddjsb2.PadLedControl(deck, group, led, false, on);

            if (on)
            {
                EliteJoystick.Common.Utils.ExplorerLog().Wait();
            }
        }

        public static void ShutUpVoiceAttack(Controller controller, IState state)
        {
            var encoder = state.Control as DDJSB2.Controls.Encoder;
            var on = state.Value > 0;

            if (encoder.NoteNumber == state.Number && on)
            {
                // Send "Shut Up" to Voice Attack  F7
                //System.Threading.Tasks.Task.Run(async () => await controller.SendKeyCombo(new byte[] { }, 0xC8).ConfigureAwait(false));

                controller.PressKey(KeyMap.KeyNameMap["KEY_F7"].Code);
            }
        }

        public static int subsystemValue, hostileValue = 0;

        public static void TargetSubsystem(Controller controller, IState state)
        {
            var encoder = state.Control as DDJSB2.Controls.Encoder;

            if (encoder.NoteNumber == state.Number && state.Value > 0)
            {
                // reset the subsystem value
                subsystemValue = 0;
            }

            if (encoder.ControlNumber == state.Number)
            {
                subsystemValue += (state.Value - encoder.Cutoff);

                // Every ten values
                if (Math.Abs(subsystemValue) > 100)
                {
                    subsystemValue = 0;

                    if (state.Value > encoder.Cutoff)
                        controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CycleSubsystem, 100);
                    else if (state.Value < (encoder.Cutoff))
                        controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CycleNextSubsystem, 100);
                }
            }
        }

        public static void TargetHostile(Controller controller, IState state)
        {
            var encoder = state.Control as DDJSB2.Controls.Encoder;

            if (encoder.NoteNumber == state.Number && state.Value > 0)
            {
                // reset the hostile value
                hostileValue = 0;
            }

            if (encoder.ControlNumber == state.Number)
            {
                hostileValue += (state.Value - encoder.Cutoff);

                // Every ten values
                if (Math.Abs(hostileValue) > 100)
                {
                    hostileValue = 0;

                    if (state.Value > encoder.Cutoff)
                        controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CycleNextHostile, 100);
                    else if (state.Value < (encoder.Cutoff))
                        controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.CyclePrevHostile, 100);
                }
            }
        }

        public static void MenuSelection(Controller controller, IState state)
        {
            var encoder = state.Control as DDJSB2.Controls.Encoder;

            if (encoder.ControlNumber == state.Number)
            {
                if (state.Value < encoder.Cutoff)
                {
                    controller.CallActivateButton(vJoyTypes.StickAndPedals, MappedButtons.ForceFeedback2HatDown, 100);
                }
                else if (state.Value > encoder.Cutoff)
                {
                    controller.CallActivateButton(vJoyTypes.StickAndPedals, MappedButtons.ForceFeedback2HatUp, 100);
                }
            }

            if (encoder.NoteNumber == state.Number)
            {
                if (state.Value > 0)
                {
                    controller.CallActivateButton(vJoyTypes.StickAndPedals, MappedButtons.ForceFeedback2Trigger, 100);
                }
            }
        }

        public static void PanelSelection(Controller controller, IState state, uint pressButton)
        {
            var button = state.Control as Button;
            var on = button.NoteValue > 0;

            if (button.NoteNumber == state.Number && on)
            {
                controller.CallActivateButton(vJoyTypes.StickAndPedals, pressButton, 200);
            }
        }
    }
}
