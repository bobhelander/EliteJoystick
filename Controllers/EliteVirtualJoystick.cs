using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vJoyInterfaceWrap;

namespace Controllers
{
    /// <summary>
    /// A vJoy virtual joystick container
    /// </summary>
    public class EliteVirtualJoystick
    {
        public vJoy Joystick { get; set; }
        public uint JoystickId { get; set; }
        public vJoy.JoystickState State = new vJoy.JoystickState();

        public void Aquire()
        {
            State.bDevice = (byte)JoystickId;
            Joystick.AcquireVJD(JoystickId);
            Joystick.ResetVJD(JoystickId);
        }

        public void Release()
        {
            Joystick.RelinquishVJD(JoystickId);
        }
    }
}
