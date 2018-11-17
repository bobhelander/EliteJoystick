﻿using EliteJoystick.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Usb.GameControllers.Common;
using Usb.GameControllers.Thrustmaster.Warthog.Throttle.Models;
using vJoyMapping.Common;

namespace vJoyMapping.Thrustmaster.Warthog.Throttle.Mapping
{
    public static class TmThrottleClearMessages
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static uint pinkyBack = (UInt32)Button.Button14;

        public static void Process(States value, Controller controller)
        {
            if (Reactive.ButtonPressed(value, pinkyBack))
            {
                controller.CallActivateButton(vJoyTypes.Virtual, MappedButtons.TextMessageEntry, 200);
                controller.TypeFullString("/clear");
                log.Debug("Clear Message Log");
            }
        }
    }
}
