using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers.Thrustmaster.Warthog
{
    public class TmThrottle75Command : StateHandler
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static UInt32 rdrAlt = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button25;

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
            if (TmThrottleController.TestButtonReleased(e.PreviousButtons, e.Buttons, rdrAlt))
            {
                TmThrottleController.CallActivateButton(vJoyTypes.Virtual, MappedButtons.Throttle75, 200);
                log.Debug("75% throttle");
            }
        }
    }
}
