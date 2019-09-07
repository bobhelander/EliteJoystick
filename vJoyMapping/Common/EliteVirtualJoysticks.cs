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

        public void UpdateAll()
        {
            uint vJoyId = 1;
            foreach (var controller in Controllers)
            {
                Joystick.UpdateVJD(vJoyId, ref States[vJoyId - 1]);
                vJoyId++;
            }
        }

        public void SetJoystickButton(bool down, uint vButton, uint vJoyId)
        {
            // Offset by one
            vButton = vButton - 1;

            // Set the position or don't
            var buttons = down ? (uint)0x1 << (int)vButton : 0;

            // Build a mask for that position
            var mask = (uint)0x1 << (int)vButton;

            // Clear just that position
            var holdValues = States[vJoyId - 1].Buttons & ~mask;

            // Set the new value
            States[vJoyId - 1].Buttons = holdValues | buttons;

            // Update
            Joystick.UpdateVJD(vJoyId, ref States[vJoyId - 1]);
        }

        public void SetJoystickButtons(UInt32 buttons, uint vJoyId, UInt32 mask = 0xFFFFFFFF)
        {
            // Clear the buttons we are assigning
            var holdValues = States[vJoyId - 1].Buttons & ~mask;
            States[vJoyId - 1].Buttons = holdValues | buttons;
            Joystick.UpdateVJD(vJoyId, ref States[vJoyId - 1]);
        }

        public void SetJoystickAxis(int value, HID_USAGES usage, uint vJoyId)
        {
            //Joystick.SetAxis(value, vJoyId, usage);

            switch (usage)
            {
                case HID_USAGES.HID_USAGE_X:
                    States[vJoyId - 1].AxisX = value;
                    break;
                case HID_USAGES.HID_USAGE_Y:
                    States[vJoyId - 1].AxisY = value;
                    break;
                case HID_USAGES.HID_USAGE_Z:
                    States[vJoyId - 1].AxisZ = value;
                    break;
                case HID_USAGES.HID_USAGE_RX:
                    States[vJoyId - 1].AxisXRot = value;
                    break;
                case HID_USAGES.HID_USAGE_RY:
                    States[vJoyId - 1].AxisYRot = value;
                    break;
                case HID_USAGES.HID_USAGE_RZ:
                    States[vJoyId - 1].AxisZRot = value;
                    break;
                case HID_USAGES.HID_USAGE_SL0:
                    States[vJoyId - 1].Slider = value;
                    break;
                case HID_USAGES.HID_USAGE_SL1:
                    States[vJoyId - 1].Dial = value;
                    break;
                case HID_USAGES.HID_USAGE_WHL:
                    States[vJoyId - 1].Wheel = value;
                    break;
                case HID_USAGES.HID_USAGE_POV:
                    //States[vJoyId - 1]. = value;
                    break;
            }

            Joystick.UpdateVJD(vJoyId, ref States[vJoyId - 1]);
        }

        public void SetJoystickHat(int value, uint hatNumber, uint vJoyId)
        {
            //Joystick.SetDiscPov(value, vJoyId, hatNumber);

            switch (hatNumber)
            {
                case 1:
                    States[vJoyId - 1].bHats = (byte)value;
                    break;
                case 2:
                    States[vJoyId - 1].bHatsEx1 = (byte)value;
                    break;
                case 3:
                    States[vJoyId - 1].bHatsEx2 = (byte)value;
                    break;
                case 4:
                    States[vJoyId - 1].bHatsEx3 = (byte)value;
                    break;
            }
            Joystick.UpdateVJD(vJoyId, ref States[vJoyId - 1]);
        }
    }
}
