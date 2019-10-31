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
    public class EliteVirtualJoysticks
    {
        //private vJoy Joystick { get; set; } = new vJoy();

        private VirtualJoystick[] Joysticks { get; } = new VirtualJoystick[4] {
                                                                new VirtualJoystick(1),
                                                                new VirtualJoystick(2),
                                                                new VirtualJoystick(3),
                                                                new VirtualJoystick(4) };

        public void Initialize()
        {
            foreach (var controller in Joysticks)
            {
                controller.Aquire();
            }
        }

        public void Release()
        {
            foreach (var controller in Joysticks)
                controller.Release();
        }

        public void UpdateAll()
        {
            foreach (var controller in Joysticks)
            {
                controller.Update();
            }
        }

        public void SetJoystickButton(bool down, uint vButton, uint vJoyId)
        {
            Joysticks[vJoyId-1].SetJoystickButton(down, vButton);
        }

        public bool GetJoystickButton(uint vButton, uint vJoyId)
        {
            return Joysticks[vJoyId-1].GetJoystickButton(vButton);
        }

        public void SetJoystickButtons(UInt32 buttons, uint vJoyId, UInt32 mask = 0xFFFFFFFF)
        {
            Joysticks[vJoyId-1].SetJoystickButtons(buttons, mask);
        }

        public void SetJoystickAxis(int value, Axis usage, uint vJoyId)
        {
            Joysticks[vJoyId - 1].SetJoystickAxis(value, usage);
        }

        public void SetJoystickHat(int value, uint hatNumber, uint vJoyId)
        {
            Joysticks[vJoyId - 1].SetJoystickHat(value, (Hats)hatNumber);
        }
    }
}
