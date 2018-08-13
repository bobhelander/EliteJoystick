using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers.Sidewinder.GameVoice
{
    public class SwGameStateHandler : StateHandler
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
                    swGvController.Controller.ButtonsChanged += async (s, e) =>
                        await Task.Run(() => ControllerButtonsChanged(s, e));
                }
            }
        }

        private void ControllerButtonsChanged(object sender, Faz.SideWinderSC.Logic.SwgvButtonStateEventArgs e)
        {
            uint buttonIndex = 1;
            foreach (SwgvButton value in Enum.GetValues(typeof(SwgvButton)))
            {
                bool pressed = ((e.ButtonsState & (byte)value) == (byte)value);
                SwGvController.SetJoystickButton(pressed, buttonIndex, vJoyTypes.Voice);
                buttonIndex++;
            }
            //this.swGvController.VisualState.UpdateButtons(e.ButtonsState);
        }
    }
}
