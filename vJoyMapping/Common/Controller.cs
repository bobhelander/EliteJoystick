using EliteJoystick.Common;
using EliteJoystick.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usb.GameControllers.Common;
using vJoy.Wrapper;
//using vJoyInterfaceWrap;

namespace vJoyMapping.Common
{
    public class Controller : IDisposable
    {
        public static string GetDevicePath(int vendorId, int productId) =>
            Usb.Hid.Connection.Devices.RetrieveAllDevicePath(vendorId, productId).FirstOrDefault();

        protected List<IDisposable> Disposables { get; set; }

        public ILogger Logger { get; set; }

        public String Name { get; set; }

        public EliteVirtualJoysticks VirtualJoysticks { get; set; }

        public Settings Settings { get; set; }

        public IArduino Arduino { get; set; }

        public EliteSharedState SharedState { get; set; }

        public void Dispose() =>
            Disposables.ForEach(disposable => { try { disposable?.Dispose(); } catch (Exception) {;} });

        #region  Virtual Joystick Actions

        public bool GetJoystickButton(uint vButton, string vJoyType) =>
            VirtualJoysticks.GetJoystickButton(vButton, Settings.vJoyMapper.GetJoystickId(vJoyType));

        public void SetJoystickButton(bool down, uint vButton, string vJoyType) =>
            VirtualJoysticks.SetJoystickButton(down, vButton, Settings.vJoyMapper.GetJoystickId(vJoyType));

        public void SetJoystickButtons(uint buttons, string vJoyType, uint mask) =>
            VirtualJoysticks.SetJoystickButtons(buttons, Settings.vJoyMapper.GetJoystickId(vJoyType), mask);

        public void SetJoystickAxis(int value, Axis usage, string vJoyType) =>
            VirtualJoysticks.SetJoystickAxis(value, usage, Settings.vJoyMapper.GetJoystickId(vJoyType));

        public void SetJoystickHat(int value, string vJoyType, uint hatNumber) =>
            VirtualJoysticks.SetJoystickHat(value, hatNumber, Settings.vJoyMapper.GetJoystickId(vJoyType));

        #endregion

        #region  Arduino keyboard actions

        public void DepressKey(byte key) =>
            Arduino?.DepressKey(key).Wait();

        public void ReleaseKey(byte key) =>
            Arduino?.ReleaseKey(key).Wait();

        public void ReleaseAllKeys() =>
            Arduino?.ReleaseAll().Wait();

        public async Task TypeFullString(String text) =>
            await Arduino.TypeFullString(text).ContinueWith(t => Utils.LogTaskResult(t, "Controller:TypeFullString", Logger)).ConfigureAwait(false);

        public async Task TypeFromClipboard() =>
            await Arduino.TypeFromClipboard().ContinueWith(t => Utils.LogTaskResult(t, "Controller:TypeFromClipboard", Logger)).ConfigureAwait(false);

        /// <summary>
        /// Press keyboard keys. Modifier array are the keys to press first and then the key is pressed.
        /// https://www.arduino.cc/en/Reference/KeyboardModifiers
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task SendKeyCombo(byte[] modifier, byte key) =>
            await Arduino.KeyCombo(modifier, key).ContinueWith(t => Utils.LogTaskResult(t, "Controller:SendKeyCombo", Logger)).ConfigureAwait(false);

        #endregion

        #region Asynchronous Actions

        public void CallActivateButton(string vJoyType, uint vButton, int delay)
        {
            uint joyId = Settings.vJoyMapper.GetJoystickId(vJoyType);
            uint modJoyId = Settings.vJoyMapper.GetJoystickId(vJoyTypes.StickAndPedals);

            Task.Run(async () =>
            {
                //VirtualJoysticks.SetJoystickButton(true, 27, modJoyId);
                VirtualJoysticks.SetJoystickButton(true, vButton, joyId);
                await Task.Delay(delay).ConfigureAwait(false);
                VirtualJoysticks.SetJoystickButton(false, vButton, joyId);
                //VirtualJoysticks.SetJoystickButton(false, 27, modJoyId);
            }).ContinueWith(t =>
            {
                if (t.IsCanceled) Logger.LogError("CallActivateButton Canceled");
                else if (t.IsFaulted) Logger.LogError($"CallActivateButton Exception: {t.Exception}");
                else Logger.LogDebug($"Press {vJoyType} Button {vButton}");
            });
        }

        #endregion

        #region Helper Functions

        public bool TestButtonDown(UInt32 state, UInt32 button) =>
            (state & button) == button;

        public bool TestButtonPressed(UInt32 previousState, UInt32 state, UInt32 button) =>
            ((previousState & button) == 0) && ((state & button) == button);

        public bool TestButtonReleased(UInt32 previousState, UInt32 state, UInt32 button) =>
            ((previousState & button) == button) && ((state & button) == 0);

        public bool TestButtonPressedOrReleased(UInt32 previousState, UInt32 state, UInt32 button) =>
            TestButtonReleased(previousState, state, button) || TestButtonPressed(previousState, state, button);

        /// <summary>
        /// Used with Tri-state switches.  This tests if the switch was moved to the middle state
        /// </summary>
        /// <param name="previousState"></param>
        /// <param name="state"></param>
        /// <param name="switchValue"></param>
        /// <returns></returns>
        public bool TestMultiSwitchStateOff(UInt32 previousState, UInt32 state, UInt32 switchValue) =>
            ((previousState & switchValue) != 0) && ((state & switchValue) == 0);

        /// <summary>
        /// Used with Tri-state switches.  This tests if the switch was moved from the middle state
        /// </summary>
        /// <param name="previousState"></param>
        /// <param name="state"></param>
        /// <param name="switchValue"></param>
        /// <returns></returns>
        public bool TestMultiSwitchStateOn(UInt32 previousState, UInt32 state, UInt32 switchValue) =>
            ((previousState & switchValue) == 0) && ((state & switchValue) != 0);

        #endregion
    }
}
