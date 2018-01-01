using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers.Sidewinder.GameVoice
{
    public class SwGameLandingGearHandler : StateHandler
    {
        private SwGvController swGvController;
        public SwGvController SwGvController
        {
            get { return swGvController; }
            set
            {
                swGvController = value;
                if (null != swGvController)
                {
                    swGvController.Controller.ButtonsChanged += Controller_ButtonsChanged;
                }
            }
        }

        private void Controller_ButtonsChanged(object sender, Faz.SideWinderSC.Logic.SwgvButtonStateEventArgs e)
        {
            byte button1 = (byte)SwgvButton.Button1;

            if (0 == (e.PreviousButtonsState & button1) &&
                (e.ButtonsState & button1) == button1)
            {
                // On
                swGvController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LandingGearToggle, 200);
            }
            else if (button1 == (e.PreviousButtonsState & button1) &&
                     0 == (e.ButtonsState & button1))
            {
                // Off
                swGvController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LandingGearToggle, 200);
            }

        }
    }
}
