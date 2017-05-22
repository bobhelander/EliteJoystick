using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Sidewinder.Commander
{
    class SwCommanderModifierButton : SwCommanderButton
    {
        public override void Controller_ButtonUp(object sender, ButtonEventArgs e)
        {
            if (e.Button == ButtonId)
            {
                ScController.OnBeforeModifierChange();
                State = false;
                ScController.OnAfterModifierChange();
            }
        }

        public override void Controller_ButtonDown(object sender, ButtonEventArgs e)
        {
            if (e.Button == ButtonId)
            {
                ScController.OnBeforeModifierChange();
                State = true;
                ScController.OnAfterModifierChange();
            }
        }
    }
}
