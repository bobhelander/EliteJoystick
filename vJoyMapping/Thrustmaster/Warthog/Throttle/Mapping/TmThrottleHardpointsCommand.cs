using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public class TmThrottleHardpointsCommand : IObserver<States>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 

        public vJoyMapping.Common.Controller Controller { get; set; }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        static UInt32 EORDown = (UInt32)Button.Button19;

        public void OnNext(States value)
        {
            var current = value.Current as State;
            var previous = value.Previous as State;

            if (Controller.SharedState.HardpointsDeployed == false &&
                Controller.TestButtonPressed(previous.buttons, current.buttons, EORDown))
            {
                log.Debug($"Hardpoints out {Controller.SharedState.HardpointsDeployed} {previous.buttons} {current.buttons}");
                Controller.SendKeyCombo(new byte[] { 0x80, 0x82 }, 0x48);
                Controller.SharedState.HardpointsDeployed = true;
            }

            if (Controller.SharedState.HardpointsDeployed == true &&
                Controller.TestButtonReleased(previous.buttons, current.buttons, EORDown))
            {
                log.Debug($"Hardpoints in {Controller.SharedState.HardpointsDeployed} {previous.buttons} {current.buttons}");                
                Controller.SendKeyCombo(new byte[] { 0x80, 0x82 }, 0x48);
                Controller.SharedState.HardpointsDeployed = false;
            }
        }
    }
}
