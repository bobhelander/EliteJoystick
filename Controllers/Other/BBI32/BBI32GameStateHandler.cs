using Faz.SideWinderSC.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers.Other.BBI32
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
                    buttonBoxController.Controller.ButtonsChanged += async (s, e) =>
                        await Task.Run(() => ControllerButtonsChanged(s, e));
                }
            }
        }

        private void ControllerButtonsChanged(object sender, ButtonStateEventArgs e)
        {
            // Buttons 21 - 32 On the combined controller
            uint buttonIndex = 21;
            foreach (BBI32Button value in Enum.GetValues(typeof(BBI32Button)))
            {
                bool pressed = ButtonBoxController.TestButtonDown(e.ButtonsState, (UInt32)value);                
                ButtonBoxController.SetJoystickButton(pressed, buttonIndex, vJoyTypes.StickAndPedals);
                buttonIndex++;
            }
        }
    }
}
