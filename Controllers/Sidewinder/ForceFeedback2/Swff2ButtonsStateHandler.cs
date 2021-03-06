﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Sidewinder.ForceFeedback2
{
    public class Swff2ButtonsStateHandler : StateHandler
    {
        private Swff2Controller swff2Controller;

        public Swff2Controller Swff2Controller
        {
            get { return swff2Controller; }
            set
            {
                swff2Controller = value;
                if (null != swff2Controller)
                {
                    swff2Controller.Controller.ButtonsChanged += async (s, e) => 
                        await Task.Run(() => ControllerButtonsChanged(s, e));
                }
            }
        }

        private void ControllerButtonsChanged(object sender, Faz.SideWinderSC.Logic.ButtonsEventArgs e)
        {
            uint buttonIndex = 1;
            foreach (Faz.SideWinderSC.Logic.Swff2Button value in Enum.GetValues(typeof(Faz.SideWinderSC.Logic.Swff2Button)))
            {
                bool pressed = ((e.Buttons & (int)value) == (int)value);
                swff2Controller.SetJoystickButton(pressed, buttonIndex, vJoyTypes.StickAndPedals);
                buttonIndex++;
            }
        }
    }
}
