using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers.Thrustmaster.Warthog
{
    public class TmThrottleSilentCommand : StateHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Fuel Right : Silent
        static UInt32 button17 = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button17;

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
            if (TmThrottleController.TestButtonPressed(e.PreviousButtons, e.Buttons, button17))
            {
                TmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.SilentRunningToggle, 200);
                log.Debug($"Silent Running Activated");
            }
            if (TmThrottleController.TestButtonReleased(e.PreviousButtons, e.Buttons, button17))
            {
                TmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.SilentRunningToggle, 200);
                log.Debug($"Silent Running Deactivated");
            }
        }
    }
}
