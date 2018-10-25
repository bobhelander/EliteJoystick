using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using vJoyInterfaceWrap;

namespace Controllers
{
    /// <summary>
    /// Base class for the mapping of the events from a physical joystick to the virtual joysticks or actions
    /// </summary>
    public class Controller 
    {
        private static readonly log4net.ILog log = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public virtual void Initialize() { }

        //public vJoy vJoy { get; set; }
        public EliteVirtualJoysticks VirtualJoysticks { get; set; }

        public vJoyMapper vJoyMapper { get; set; }

        public bool Use { get; set; }

        public ArduinoCommunication.Arduino Arduino { get; set; }

        #region  Virtual Joystick Actions

        public void SetJoystickButton(bool down, uint vButton, string vJoyType)
        {
            VirtualJoysticks.SetJoystickButton(down, vButton, vJoyMapper.GetJoystickId(vJoyType));
            log.Debug($"{vJoyType}: {vButton}: {down}");
        }

        public void SetJoystickAxis(int value, HID_USAGES usage, string vJoyType)
        {
            VirtualJoysticks.SetJoystickAxis(value, usage, vJoyMapper.GetJoystickId(vJoyType));
            //vJoy.SetAxis(value, vJoyMapper.GetJoystickId(vJoyType), usage);
        }

        public void SetJoystickHat(int value, string vJoyType, uint hatNumber)
        {
            VirtualJoysticks.SetJoystickHat(value, hatNumber, vJoyMapper.GetJoystickId(vJoyType));
            //vJoy.SetDiscPov(value, vJoyMapper.GetJoystickId(vJoyType), hatNumber);
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

        public void TypeFullString(String text)
        {
            Task.Run(async () => await ArduinoCommunication.Utils.TypeFullString(Arduino, text))
             .ContinueWith(t => { log.Error($"SendKeyCombo Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);
        }

        public void TypeFromClipboard()
        {
            Task.Run(async () => await ArduinoCommunication.Utils.TypeFromClipboard(Arduino))
             .ContinueWith(t => { log.Error($"TypeFromClipboard Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);
        }

        public void SendKeyCombo(byte[] modifier, byte key)
        {
            Task.Run(async () => await ArduinoCommunication.Utils.KeyCombo(Arduino, modifier, key))
             .ContinueWith(t => { log.Error($"SendKeyCombo Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);
        }

        #endregion

        #region Asynchronous Actions

        public void CallActivateButton(string vJoyType, uint vButton, int delay)
        {
            uint joyId = vJoyMapper.GetJoystickId(vJoyType);

            Task.Run(async () => {
                VirtualJoysticks.SetJoystickButton(true, vButton, joyId);
                await Task.Delay(delay);
                VirtualJoysticks.SetJoystickButton(false, vButton, joyId);
                log.Debug($"Button {vJoyType} Button {vButton}");
            }).ContinueWith(t => { log.Error($"CallActivateButton Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);
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
