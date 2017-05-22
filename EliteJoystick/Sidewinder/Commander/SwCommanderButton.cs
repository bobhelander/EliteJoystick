using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vJoyInterfaceWrap;

namespace EliteJoystick.Sidewinder.Commander
{
    public class SwCommanderButton : Button
    {        
        public SwscButton ButtonId { get; set; }
        public SwscLight ButtonLight { get; set; }

        private ScController scController;

        public ScController ScController
        {
            get { return scController; }
            set
            {
                scController = value;
                if (null != scController)
                {
                    scController.Controller.ButtonDown += Controller_ButtonDown;
                    scController.Controller.ButtonUp += Controller_ButtonUp;
                    scController.BeforeModifierChanged += ScController_BeforeModifierChanged;
                    scController.AfterModifierChanged += ScController_AfterModifierChanged;
                }
            }
        }        

        public virtual void ScController_BeforeModifierChanged(object sender, ScController.ModifierChangedArgs e)
        {
        }

        public virtual void ScController_AfterModifierChanged(object sender, ScController.ModifierChangedArgs e)
        {
        }

        public virtual void Controller_ButtonUp(object sender, ButtonEventArgs e)
        {            
        }

        public virtual void Controller_ButtonDown(object sender, ButtonEventArgs e)
        {
        }
    }
}
