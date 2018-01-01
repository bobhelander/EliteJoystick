using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Thrustmaster.Warthog
{
    public class TmThrottleHat
    {
        private TmThrottleController tmThrottleController;

        public TmThrottleController TmThrottleController
        {
            get { return tmThrottleController; }
            set
            {
                tmThrottleController = value;
                if (null != tmThrottleController)
                {
                    tmThrottleController.Controller.Hat += Controller_Hat;
                }
            }
        }

        public List<uint> vButtons { get; set; }

        private void Controller_Hat(object sender, Faz.SideWinderSC.Logic.HatEventArgs e)
        {
            int index = 0;
            foreach (Faz.SideWinderSC.Logic.TmThrottleHatSwitch value in Enum.GetValues(typeof(Faz.SideWinderSC.Logic.TmThrottleHatSwitch)))
            {
                bool pressed = (e.HatSwitch & (int)value) != 0;
                tmThrottleController.SetJoystickButton(pressed, vButtons[index], vJoyTypes.Virtual);
                index++;
            }

            int pov = e.Hat / 2;
            pov = pov == 4 ? -1 : pov;
            tmThrottleController.SetJoystickHat(pov, vJoyTypes.Throttle, 1);
        }
    }
}
