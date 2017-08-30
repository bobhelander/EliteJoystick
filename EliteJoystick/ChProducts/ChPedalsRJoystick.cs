using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.ChProducts
{
    public class ChPedalsRJoystick : Axis
    {
        private ChPedalsController chPedals;

        public ChPedalsController ChPedalsController
        {
            get { return chPedals; }
            set
            {
                chPedals = value;
                if (null != chPedals)
                {
                    chPedals.Controller.Rotate += Controller_Rotate;
                }
            }
        }

        // x = -512 to 511
        // y = -512 to 511
        // z = -512 to 511
        // z = 0 to 255

        private void Controller_Rotate(object sender, RotateEventArgs e)
        {
            bool button3 = e.R < 64;
            bool button4 = e.R > 128+64;

            int z = e.R * 128;
            chPedals.SetJoystickAxis(z, HID_USAGES.HID_USAGE_RZ, vJoyTypes.StickAndPedals);

            //ChPedalsController.SetJoystickButton(button3, 23, vJoyTypes.StickAndPedals);
            //ChPedalsController.SetJoystickButton(button4, 24, vJoyTypes.StickAndPedals);

            this.ChPedalsController.VisualState.UpdateAxis3((UInt32)z);
        }
    }
}
