using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vJoyInterfaceWrap;

namespace Controllers
{
    /// <summary>
    /// Container for all of the EliteVirtualJoystick controllers 
    /// </summary>
    public class EliteVirtualJoysticks
    {
        public EliteVirtualJoysticks()
        {
            Joystick = new vJoy();
            Controllers = new List<EliteVirtualJoystick>();
        }

        public vJoy Joystick { get; set; }
        public List<EliteVirtualJoystick> Controllers { get; set; }

        public void Initialize()
        {
            foreach (var controller in Controllers)
            {
                controller.Aquire();
            }
        }

        public void Release()
        {
            foreach (var controller in Controllers)
                controller.Release();
        }
    }
}
