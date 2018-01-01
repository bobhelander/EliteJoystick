using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Sidewinder.ForceFeedback2
{
    public class Swff2Hat
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
                    swff2Controller.Controller.Hat += Controller_Hat;
                }
            }
        }

        public List<uint> vButtons { get; set; }

        private void Controller_Hat(object sender, Faz.SideWinderSC.Logic.HatEventArgs e)
        {
            int pov = e.Hat / 2;
            pov = pov == 4 ? -1 : pov;
            swff2Controller.vJoy.SetDiscPov(pov, 1, 1);

            foreach (var vButton in vButtons)
            {
                swff2Controller.SetJoystickButton(false, vButton, vJoyTypes.StickAndPedals);
            }

            switch (e.Hat)
            {
                case 0: // up
                    swff2Controller.SetJoystickButton(true, vButtons[0], vJoyTypes.StickAndPedals);
                    break;
                case 1: // up - right
                    swff2Controller.SetJoystickButton(true, vButtons[0], vJoyTypes.StickAndPedals);
                    swff2Controller.SetJoystickButton(true, vButtons[1], vJoyTypes.StickAndPedals);
                    break;
                case 2: // right
                    swff2Controller.SetJoystickButton(true, vButtons[1], vJoyTypes.StickAndPedals);
                    break;
                case 3: // right down
                    swff2Controller.SetJoystickButton(true, vButtons[1], vJoyTypes.StickAndPedals);
                    swff2Controller.SetJoystickButton(true, vButtons[2], vJoyTypes.StickAndPedals);
                    break;
                case 4: // down
                    swff2Controller.SetJoystickButton(true, vButtons[2], vJoyTypes.StickAndPedals);
                    break;
                case 5: // left down
                    swff2Controller.SetJoystickButton(true, vButtons[2], vJoyTypes.StickAndPedals);
                    swff2Controller.SetJoystickButton(true, vButtons[3], vJoyTypes.StickAndPedals);
                    break;
                case 6: // left
                    swff2Controller.SetJoystickButton(true, vButtons[3], vJoyTypes.StickAndPedals);
                    break;
                case 7: // left up
                    swff2Controller.SetJoystickButton(true, vButtons[3], vJoyTypes.StickAndPedals);
                    swff2Controller.SetJoystickButton(true, vButtons[0], vJoyTypes.StickAndPedals);
                    break;
                case 8: // none
                    swff2Controller.SetJoystickButton(true, vButtons[4], vJoyTypes.StickAndPedals);
                    break;
            }   
        }
    }
}
