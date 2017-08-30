using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.ChProducts
{
    public class ChPedalsXYcombined : Axis
    {
        private ChPedalsController chPedalsController;

        public ChPedalsController ChPedalsController
        {
            get { return chPedalsController; }
            set
            {
                chPedalsController = value;
                if (null != chPedalsController)
                {
                    chPedalsController.Controller.Move += Controller_Move;
                }
            }
        }

        // x = 0 to 255
        // y = 0 to 255
        // z = 0 to 255

        private void Controller_Move(object sender, MoveEventArgs e)
        {
            int y = e.Y * 128;
            int x = e.X * 128;

            bool buttonX = e.X > 128;
            bool buttonY = e.Y > 128;

            this.ChPedalsController.VisualState.UpdateAxis1((UInt32)x);
            this.ChPedalsController.VisualState.UpdateAxis2((UInt32)y);

            int combined = ((128 * 255) / 2) + (e.Y * 64 - e.X * 64);

            combined = Curves.Calculate(combined - (16 * 1024), (16 * 1024), .2) + 16 * 1024;

            //chPedalsController.SetJoystickAxis(x, HID_USAGES.HID_USAGE_X);
            //chPedalsController.SetJoystickAxis(y, HID_USAGES.HID_USAGE_Y);
            chPedalsController.SetJoystickAxis(combined, HID_USAGES.HID_USAGE_RX, vJoyTypes.StickAndPedals);

            //chPedalsController.SetJoystickButton(buttonX, 21, vJoyTypes.StickAndPedals);
            //chPedalsController.SetJoystickButton(buttonY, 22, vJoyTypes.StickAndPedals);
        }
    }
}
