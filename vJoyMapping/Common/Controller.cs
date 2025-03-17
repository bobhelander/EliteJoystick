using EliteJoystick.Common;
using EliteJoystick.Common.Interfaces;
//using EliteJoystick.Common.Logic;
//using EliteJoystick.Common.Logic;
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

        public virtual String Name { get; }

        public IVirtualJoysticks VirtualJoysticks { get; set; }

        public ISettings Settings { get; set; }

        public IKeyboard Keyboard { get; set; }

        public EliteSharedState SharedState { get; set; }

        public void Dispose()
        {
            ReleaseAllKeys();
            Disposables.ForEach(disposable => { try { disposable?.Dispose(); disposable = null; } catch (Exception) {; } });
        }

        #region  Virtual Joystick Actions

        public bool GetJoystickButton(uint vButton, string vJoyType) =>
            VirtualJoysticks.GetJoystickButton(vButton, Settings.vJoyMapper.GetJoystickId(vJoyType));

        public void SetJoystickButton(bool down, uint vButton, string vJoyType) =>
            VirtualJoysticks.SetJoystickButton(down, vButton, Settings.vJoyMapper.GetJoystickId(vJoyType));

        public void SetJoystickButtons(uint buttons, string vJoyType, uint mask) =>
            VirtualJoysticks.SetJoystickButtons(buttons, Settings.vJoyMapper.GetJoystickId(vJoyType), mask);

        public void SetJoystickAxis(int value, int usage, string vJoyType) =>
            VirtualJoysticks.SetJoystickAxis(value, usage, Settings.vJoyMapper.GetJoystickId(vJoyType));

        public void SetJoystickHat(int value, string vJoyType, uint hatNumber) =>
            VirtualJoysticks.SetJoystickHat(value, hatNumber, Settings.vJoyMapper.GetJoystickId(vJoyType));

        #endregion

        #region  Keyboard actions

        public async Task TypeFullString(String text) =>
            await Keyboard.TypeFullString(text).ContinueWith(t => Utils.LogTaskResult(t, "Controller:TypeFullString", Logger)).ConfigureAwait(false);

        public async Task TypeFromClipboard() =>
            await Keyboard.TypeFromClipboard().ContinueWith(t => Utils.LogTaskResult(t, "Controller:TypeFromClipboard", Logger)).ConfigureAwait(false);

        /// <summary>
        /// Press key. Do not wait.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modifiers"></param>
        /// <param name="duration"></param>
        public void PressKey(byte key, EliteJoystick.Common.Logic.KeyCode[] modifiers = null, int duration = 50) =>
            Task.Run(async () => await Keyboard.PressKey(key, modifiers, duration).ConfigureAwait(false))
            .ContinueWith(t =>
            {
                if (t.IsCanceled) Logger.LogError("PressKey Canceled");
                else if (t.IsFaulted) Logger.LogError($"PressKey Exception: {t.Exception}");
                else Logger.LogDebug($"PressKey: 0x{key:X}");
            }).ConfigureAwait(false);

        /// <summary>
        /// Press key asynchronous.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modifiers"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public Task PressKeyAsync(byte key, EliteJoystick.Common.Logic.KeyCode[] modifiers = null, int duration = 50) =>
            Keyboard.PressKey(key, modifiers, duration).ContinueWith(t => Utils.LogTaskResult(t, "Controller:PressKey", Logger));

        /// <summary>
        /// Release key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modifiers"></param>
        public void ReleaseKey(byte key, EliteJoystick.Common.Logic.KeyCode[] modifiers = null) =>
            Keyboard?.ReleaseKey(key, modifiers).Wait();

        /// <summary>
        /// Clear all keys.
        /// </summary>
        public void ReleaseAllKeys() =>
            Keyboard?.ReleaseAll().Wait();

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
