using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using vJoyInterfaceWrap;

namespace EliteJoystick
{
    /// <summary>
    /// Base class for the mapping of the events from a physical joystick to the virtual joysticks or actions
    /// </summary>
    public class Controller 
    {
        public virtual void Initialize() { }

        public vJoy vJoy { get; set; }

        public vJoyMapper vJoyMapper { get; set; }

        public VisualState VisualState { get; set; }

        public bool Use { get; set; }

        public ArduinoCommunication.Arduino Arduino { get; set; }

        #region  Virtual Joystick Actions

        public void SetJoystickButton(bool down, uint vButton, string vJoyType)
        {
            vJoy.SetBtn(down, vJoyMapper.GetJoystickId(vJoyType), vButton);
        }

        public void SetJoystickAxis(int value, HID_USAGES usage, string vJoyType)
        {
            vJoy.SetAxis(value, vJoyMapper.GetJoystickId(vJoyType), usage);
        }

        public void SetJoystickHat(int value, string vJoyType, uint hatNumber)
        {
            vJoy.SetDiscPov(value, vJoyMapper.GetJoystickId(vJoyType), hatNumber);
        }

        #endregion

        #region  Arduino keyboard actions

        public void DepressKey(byte key)
        {
            Arduino?.DepressKey(key);
        }

        public void ReleaseKey(byte key)
        {
            Arduino?.ReleaseKey(key);
        }

        public void ReleaseAllKeys()
        {
            Arduino?.ReleaseAll();
        }

        public void TypeFullString(String text, System.Threading.EventWaitHandle finishedEvent)
        {
            ArduinoCommunication.Utils.TypeFullString(Arduino, text, finishedEvent);
        }

        public void SendKeyCombo(byte[] modifier, byte key)
        {
            ArduinoCommunication.Utils.KeyCombo(Arduino, modifier, key);
        }

        #endregion

        #region Asynchronous Actions

        public class ActivateButtonClass
        {
            public Timer Timer { get; set; }
            public bool Pressed { get; set; }
            public vJoy vJoy { get; set; }
            public uint vJoyId { get; set; }            
            public uint vButton { get; set; }
            public long Delay { get; set; }
        }

        public void CallActivateButton(string vJoyType, uint vButton, long delay)
        {
            uint joyId = vJoyMapper.GetJoystickId(vJoyType);

            var activateButton = new ActivateButtonClass
            {
                vJoy = vJoy,
                vJoyId = joyId,
                vButton = vButton,
                Delay = delay
            };

            activateButton.Timer = new Timer(new TimerCallback(Action), activateButton, 0, Timeout.Infinite);
        }

        public void Disable(ActivateButtonClass activateButton)
        {
            if (null != activateButton.Timer)
            {
                var temp = activateButton.Timer;
                activateButton.Timer = null;
                temp.Dispose();
                activateButton.vJoy.SetBtn(false, activateButton.vJoyId, activateButton.vButton);
                activateButton.Pressed = false;
            }
        }

        public virtual void Action(object o)
        {
            var activateButton = o as ActivateButtonClass;
            activateButton.Pressed = !activateButton.Pressed;
            activateButton.vJoy.SetBtn(activateButton.Pressed, activateButton.vJoyId, activateButton.vButton);

            if (activateButton.Pressed)
            {
                activateButton.Timer.Change(activateButton.Delay, Timeout.Infinite);
                return;
            }

            Disable(activateButton);
        }

        #endregion

        #region Helper Functions

        public bool TestButtonDown(UInt32 state, UInt32 button)
        {
            return (state & button) == button;
        }

        public bool TestButtonPressed(UInt32 previousState, UInt32 state, UInt32 button)
        {
            return ((previousState & button) == 0 && (state & button) == button);
        }

        public bool TestButtonReleased(UInt32 previousState, UInt32 state, UInt32 button)
        {
            return ((previousState & button) == button && (state & button) == 0);
        }

        public bool TestButtonPressedOrReleased(UInt32 previousState, UInt32 state, UInt32 button)
        {
            return (TestButtonReleased(previousState, state, button) || TestButtonPressed(previousState, state, button));
        }

        /// <summary>
        /// Used with Tri-state switches.  This tests if the switch was moved to the middle state
        /// </summary>
        /// <param name="previousState"></param>
        /// <param name="state"></param>
        /// <param name="switchValue"></param>
        /// <returns></returns>
        public bool TestMultiSwitchStateOff(UInt32 previousState, UInt32 state, UInt32 switchValue)
        {
            return ((previousState & switchValue) != 0 && (state & switchValue) == 0);
        }

        /// <summary>
        /// Used with Tri-state switches.  This tests if the switch was moved from the middle state
        /// </summary>
        /// <param name="previousState"></param>
        /// <param name="state"></param>
        /// <param name="switchValue"></param>
        /// <returns></returns>
        public bool TestMultiSwitchStateOn(UInt32 previousState, UInt32 state, UInt32 switchValue)
        {
            return ((previousState & switchValue) == 0 && (state & switchValue) != 0);
        }

        #endregion
    }
}
