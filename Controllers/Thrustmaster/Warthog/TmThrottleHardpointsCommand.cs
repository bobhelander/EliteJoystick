using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers.Thrustmaster.Warthog
{
    public class TmThrottleHardpointsCommand : StateHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static UInt32 button19 = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button19;

        public Faz.SideWinderSC.Logic.TmThrottleButton ButtonId { get; set; }

        private TmThrottleController tmThrottleController;

        public TmThrottleController TmThrottleController
        {
            get { return tmThrottleController; }
            set
            {
                tmThrottleController = value;
                if (null != tmThrottleController)
                {
                    tmThrottleController.Controller.SwitchState += Controller_SwitchState;
                }
            }
        }

        private void Controller_SwitchState(object sender, Faz.SideWinderSC.Logic.TmThrottleSwitchEventArgs e)
        {
            if (tmThrottleController.SharedState.HardpointsDeployed == false &&
                tmThrottleController.TestButtonPressed(e.PreviousButtons, e.Buttons, button19))
            {
                log.Debug($"Hardpoints out {tmThrottleController.SharedState.HardpointsDeployed} {e.PreviousButtons} {e.Buttons}");
                tmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.HardpointsToggle, 200);
                tmThrottleController.SharedState.HardpointsDeployed = true;
            }

            if (tmThrottleController.SharedState.HardpointsDeployed == true &&
                tmThrottleController.TestButtonReleased(e.PreviousButtons, e.Buttons, button19))
            {
                log.Debug($"Hardpoints in {tmThrottleController.SharedState.HardpointsDeployed} {e.PreviousButtons} {e.Buttons}");
                tmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.HardpointsToggle, 200);
                tmThrottleController.SharedState.HardpointsDeployed = false;
            }
        }
    }
}
