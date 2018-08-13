using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controllers.Thrustmaster.Warthog
{
    public class TmThrottleClearMessages: StateHandler
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public long Delay { get; set; }
        public uint vButton { get; set; }

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
            var button14 = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button14;
            if (((e.Buttons & button14) != 0) &&
                ((e.PreviousButtons & button14) == 0)) 
            {
                TmThrottleController.CallActivateButton(vJoyTypes.Virtual, vButton, 200);
                tmThrottleController.TypeFullString("/clear");
                log.Debug("Clear Message Log");
            }
        }
    }
}
