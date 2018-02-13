using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Thrustmaster.Warthog
{
    public class TmThrottleSwitchHandler : StateHandler
    {
        public Dictionary<Faz.SideWinderSC.Logic.TmThrottleSwitches.AllSwitches, uint> StateGroup { get; set; }

        private TmThrottleController tmThrottleController;

        public TmThrottleController TmThrottleController
        {
            get { return tmThrottleController; }
            set
            {
                tmThrottleController = value;
                if (null != tmThrottleController)
                {
                    tmThrottleController.Controller.SwitchState += async (s, e) =>
                        await Task.Run(() => ControllerSwitchState(s, e));
                }
            }
        }

        private void ControllerSwitchState(object sender, Faz.SideWinderSC.Logic.TmThrottleSwitchEventArgs e)
        {
        }
    }
}
