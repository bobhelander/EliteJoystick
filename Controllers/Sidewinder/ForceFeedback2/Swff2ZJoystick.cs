using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Sidewinder.ForceFeedback2
{
    public class Swff2ZJoystick : Axis 
    {
        private Swff2Controller swff2Controller;

        public Swff2Controller Swff2Controller
        {
            get { return swff2Controller; }
            set
            {
                swff2Controller = value;
                if (null != swff2Controller)
                {
                    swff2Controller.Controller.Rotate += Controller_Rotate;
                }
            }
        }

       // twist = -32 to 31

        private void Controller_Rotate(object sender, RotateEventArgs e)
        {
            int z = ((e.R * -1) + 32) * 16 * 32;

            //this.swff2Controller.VisualState.UpdateAxis3((UInt32)z);

            z = Curves.Calculate(z - (16 * 1024), (16 * 1024), .4) + 16 * 1024;

            swff2Controller.SetJoystickAxis(z, HID_USAGES.HID_USAGE_Z, vJoyTypes.StickAndPedals);
        }
    }
}
