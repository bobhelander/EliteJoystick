using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vJoyInterfaceWrap;

namespace EliteJoystick
{
    public class EliteVirtualJoystick
    {
        public vJoy Joystick { get; set; }
        public VisualState VisualState { get; set; }
        public uint JoystickId { get; set; }

        public void Aquire()
        {
            Joystick.AcquireVJD(JoystickId);
            Joystick.ResetVJD(JoystickId);
        }
    }
}
