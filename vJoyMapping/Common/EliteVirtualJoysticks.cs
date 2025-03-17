using EliteJoystick.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vJoy.Wrapper;

namespace vJoyMapping.Common
{
    /// <summary>
    /// Container for all of the EliteVirtualJoystick controllers 
    /// </summary>
    public class EliteVirtualJoysticks : IVirtualJoysticks, IDisposable
    {
        private List<VirtualJoystick> Joysticks { get; }
            = new List<VirtualJoystick>(
                new VirtualJoystick[4] {
                    new VirtualJoystick(1),
                    new VirtualJoystick(2),
                    new VirtualJoystick(3),
                    new VirtualJoystick(4) });

        public EliteVirtualJoysticks()
        {
            Initialize();
        }

        public void Initialize() =>
            Joysticks.ForEach(joystick => joystick.Aquire());

        public void Release() =>
            Joysticks.ForEach(joystick => joystick.Release());

        public void UpdateAll() =>
            Joysticks.ForEach(joystick => joystick.Update());

        public void SetJoystickButton(bool down, uint vButton, uint vJoyId) =>
            Joysticks[(int)(vJoyId-1)].SetJoystickButton(down, vButton);

        public bool GetJoystickButton(uint vButton, uint vJoyId) =>
            Joysticks[(int)(vJoyId-1)].GetJoystickButton(vButton);

        public void SetJoystickButtons(UInt32 buttons, uint vJoyId, UInt32 mask = 0xFFFFFFFF) =>
            Joysticks[(int)(vJoyId-1)].SetJoystickButtons(buttons, mask);

        public void SetJoystickAxis(int value, int usage, uint vJoyId) =>
            Joysticks[(int)(vJoyId-1)].SetJoystickAxis(value, (Axis)usage);

        public void SetJoystickHat(int value, uint hatNumber, uint vJoyId) =>
            Joysticks[(int)(vJoyId-1)].SetJoystickHat(value, (Hats)hatNumber);

        public void Dispose()
        {
            Release();
        }
    }
}
