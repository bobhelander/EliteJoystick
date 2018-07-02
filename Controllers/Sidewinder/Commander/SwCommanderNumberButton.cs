using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Sidewinder.Commander
{
    class SwCommanderNumberButton : SwCommanderButton
    {
        public uint vButtonId { get; set; }

        public override void ScController_BeforeModifierChanged(object sender, ScController.ModifierChangedArgs e)
        {
            if (State)
            {
                ScController.SetJoystickButton(false, ScController.ModifiedButton(vButtonId), vJoyTypes.Commander);
            }
        }

        public override void ScController_AfterModifierChanged(object sender, ScController.ModifierChangedArgs e)
        {
            if (State)
            {
                ScController.SetJoystickButton(State, ScController.ModifiedButton(vButtonId), vJoyTypes.Commander);
            }
        }

        public override void Controller_ButtonUp(object sender, ButtonEventArgs e)
        {
            if (e.Button == ButtonId)
            {
                State = false;
                ScController.SetJoystickButton(State, ScController.ModifiedButton(vButtonId), vJoyTypes.Commander);
            }
        }

        public override void Controller_ButtonDown(object sender, ButtonEventArgs e)
        {
            if (e.Button == ButtonId)
            {
                State = true;
                if (ScController.Profile != 1)
                    ScController.SetJoystickButton(State, ScController.ModifiedButton(vButtonId), vJoyTypes.Commander);
                else
                    ScController.ProgramButtonPressed(vButtonId);
            }
        }
    }
}
