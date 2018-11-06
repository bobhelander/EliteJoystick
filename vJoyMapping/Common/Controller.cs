using EliteJoystick.Common;
using EliteJoystick.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usb.GameControllers.Common;
using vJoyInterfaceWrap;

namespace vJoyMapping.Common
{
    public class Controller
    {
        public static string GetDevicePath(int vendorId, int productId)
        {
            return Usb.Hid.Connection.Devices.RetrieveAllDevicePath(vendorId, productId).FirstOrDefault();
        }

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public virtual void Initialize() { }

        public String Name { get; set; }

        public EliteVirtualJoysticks VirtualJoysticks { get; set; }

        public vJoyMapper vJoyMapper { get; set; }

        public IArduino Arduino { get; set; }

        public EliteSharedState SharedState { get; set; }

        #region  Virtual Joystick Actions

        public void SetJoystickButton(bool down, uint vButton, string vJoyType)
        {
            VirtualJoysticks.SetJoystickButton(down, vButton, vJoyMapper.GetJoystickId(vJoyType));
            log.Debug($"{vJoyType}: {vButton}: {down}");
        }

        public void SetJoystickAxis(int value, HID_USAGES usage, string vJoyType)
        {
            VirtualJoysticks.SetJoystickAxis(value, usage, vJoyMapper.GetJoystickId(vJoyType));
        }

        public void SetJoystickHat(int value, string vJoyType, uint hatNumber)
        {
            VirtualJoysticks.SetJoystickHat(value, hatNumber, vJoyMapper.GetJoystickId(vJoyType));
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
            Task.Run(async () => await Arduino.TypeFullString(text))
             .ContinueWith(t => { log.Error($"SendKeyCombo Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);
        }

        public void TypeFromClipboard()
        {
            Task.Run(async () => await Arduino.TypeFromClipboard())
             .ContinueWith(t => { log.Error($"TypeFromClipboard Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);
        }

        public void SendKeyCombo(byte[] modifier, byte key)
        {
            Task.Run(async () => await Arduino.KeyCombo(modifier, key)
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
