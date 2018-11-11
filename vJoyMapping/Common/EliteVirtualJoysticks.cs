using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vJoyInterfaceWrap;

namespace vJoyMapping.Common
{
    /// <summary>
    /// Container for all of the EliteVirtualJoystick controllers 
    /// </summary>
    public class EliteVirtualJoysticks
    {
        public vJoy Joystick { get; set; } = new vJoy();
        public EliteVirtualJoystick[] Controllers { get; set; } = new EliteVirtualJoystick[4] {
                        new EliteVirtualJoystick(), new EliteVirtualJoystick(), new EliteVirtualJoystick(), new EliteVirtualJoystick() };

        public vJoy.JoystickState[] States { get; set; } = new vJoy.JoystickState[4] {
                        new vJoy.JoystickState(), new vJoy.JoystickState(), new vJoy.JoystickState(), new vJoy.JoystickState() };

        public void Initialize()
        {
            uint vJoyId = 1;
            foreach (var controller in Controllers)
            {
                controller.Joystick = Joystick;
                controller.JoystickId = vJoyId;
                controller.Aquire();
                vJoyId++;
            }
        }

        public void Release()
        {
            foreach (var controller in Controllers)
                controller.Release();
        }

        public void SetJoystickButton(bool down, uint vButton, uint vJoyId)
        {
            //States[vJoyId-1].
            Joystick.SetBtn(down, vJoyId, vButton);
        }

        public void SetJoystickAxis(int value, HID_USAGES usage, uint vJoyId)
        {
            Joystick.SetAxis(value, vJoyId, usage);
        }

        public void SetJoystickHat(int value, uint hatNumber, uint vJoyId)
        {
            Joystick.SetDiscPov(value, vJoyId, hatNumber);
        }
    }
}
