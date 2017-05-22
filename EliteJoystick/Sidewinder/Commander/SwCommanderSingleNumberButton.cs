using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Sidewinder.Commander
{
    class SwCommanderSingleNumberButton : SwCommanderButton
    {
        public uint vButtonId { get; set; }

        public override void Controller_ButtonUp(object sender, ButtonEventArgs e)
        {
            if (e.Button == ButtonId)
            {
                State = false;
                ScController.SetJoystickButton(State, vButtonId, vJoyTypes.Virtual);
            }
        }

        public override void Controller_ButtonDown(object sender, ButtonEventArgs e)
        {
            if (e.Button == ButtonId)
            {
                State = true;
                ScController.SetJoystickButton(State, vButtonId, vJoyTypes.Virtual);
            }
        }
    }
}
