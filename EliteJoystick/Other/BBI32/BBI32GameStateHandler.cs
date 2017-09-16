using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EliteJoystick.Other.BBI32
{
    public class BBI32GameStateHandler : StateHandler
    {
        private ButtonBoxController buttonBoxController;

        public ButtonBoxController ButtonBoxController
        {
            get { return buttonBoxController; }
            set
            {
                buttonBoxController = value;
                if (null != buttonBoxController)
                {
                    buttonBoxController.Controller.ButtonsChanged += Controller_ButtonsChanged;
                }
            }
        }

        private void Controller_ButtonsChanged(object sender, Faz.SideWinderSC.Logic.ButtonStateEventArgs e)
        {
            // Buttons 21 - 32 On the combined controller
            uint buttonIndex = 21;
            foreach (BBI32Button value in Enum.GetValues(typeof(BBI32Button)))
            {
                bool pressed = ButtonBoxController.TestButtonDown(e.ButtonsState, (UInt32)value);                
                ButtonBoxController.SetJoystickButton(pressed, buttonIndex, vJoyTypes.StickAndPedals);
                buttonIndex++;
            }
            ButtonBoxController.VisualState.UpdateButtons(e.ButtonsState);
        }
    }
}
