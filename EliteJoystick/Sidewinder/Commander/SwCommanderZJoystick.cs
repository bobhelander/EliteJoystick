using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Sidewinder.Commander
{
    public class SwCommanderZJoystick : Axis 
    {
        private ScController scController;

        public ScController ScController
        {
            get { return scController; }
            set
            {
                scController = value;
                if (null != scController)
                {
                    scController.Controller.Rotate += Controller_Rotate;
                }
            }
        }

        // x = -512 to 511
        // y = -512 to 511
        // z = -512 to 511

        private void Controller_Rotate(object sender, RotateEventArgs e)
        {
            int z = ((e.R * -1) + 511) * 32;
            scController.SetJoystickAxis(z, HID_USAGES.HID_USAGE_Z, vJoyTypes.Commander);

            this.scController.VisualState.UpdateAxis3((UInt32)z);
        }
    }
}
