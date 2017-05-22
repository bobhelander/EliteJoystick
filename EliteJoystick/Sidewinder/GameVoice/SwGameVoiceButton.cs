using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vJoyInterfaceWrap;

namespace EliteJoystick.Sidewinder.GameVoice
{
    public class SwGameVoiceButton : Button
    {        
        public SwgvButton ButtonId { get; set; }
        //public SwscLight ButtonLight { get; set; }

        private SwGvController swGvController;

        public SwGvController SwGvController
        {
            get { return swGvController; }
            set
            {
                swGvController = value;
                if (null != swGvController)
                {
                    swGvController.Controller.ButtonDown += Controller_ButtonDown;
                    swGvController.Controller.ButtonUp += Controller_ButtonUp;
                    //swGvController.BeforeModifierChanged += ScController_BeforeModifierChanged;
                    //swGvController.AfterModifierChanged += ScController_AfterModifierChanged;
                }
            }
        }        

        //public virtual void ScController_BeforeModifierChanged(object sender, ScController.ModifierChangedArgs e)
        //{
        //}

        //public virtual void ScController_AfterModifierChanged(object sender, ScController.ModifierChangedArgs e)
        //{
        //}

        public virtual void Controller_ButtonUp(object sender, SwgvButtonEventArgs e)
        {            
        }

        public virtual void Controller_ButtonDown(object sender, SwgvButtonEventArgs e)
        {
        }
    }
}
