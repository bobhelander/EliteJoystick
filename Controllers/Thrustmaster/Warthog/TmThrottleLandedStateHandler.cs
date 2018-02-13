using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Thrustmaster.Warthog
{
    public class TmThrottleLandedStateHandler : StateHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static UInt32 leftThrottleParked = (UInt32)Faz.SideWinderSC.Logic.TmThrottleButton.Button30;

        public TmThrottleZJoystick ZAxis { get; set; }

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
            if (TmThrottleController.TestButtonPressed(e.PreviousButtons, e.Buttons, leftThrottleParked))
            {
                ZAxis.LeftEnabled = false;
                tmThrottleController.SetJoystickAxis(16*1024, HID_USAGES.HID_USAGE_RZ, vJoyTypes.Throttle);
                log.Debug($"Left Throttle Parked");
            }
            else if (TmThrottleController.TestButtonReleased(e.PreviousButtons, e.Buttons, leftThrottleParked))
            {
                ZAxis.LeftEnabled = true;
                tmThrottleController.SetJoystickAxis(32*1024, HID_USAGES.HID_USAGE_RZ, vJoyTypes.Throttle);
                log.Debug($"Left Throttle Activated");
            }
        }
    }
}
