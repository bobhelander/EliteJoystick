using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace EliteJoystick.Common
{
    /// <summary>
    /// Allow dynamic changing of the virtual joystick output identifiers.  With multiple vJoy devices
    /// games get confused what device number is being fed by the vJoy actions.  This allows the service to map
    /// the output to the different vJoy devices.
    /// </summary>
    public class vJoyMapper
    {
        public Dictionary<string, uint> vJoyMap { get; set; } =
            new Dictionary<string, uint> {
                {vJoyTypes.StickAndPedals, 1 },
                {vJoyTypes.Throttle, 2 },
                {vJoyTypes.Commander, 3 },
                {vJoyTypes.Virtual, 4 }
            };

        public uint GetJoystickId(string vJoyType) =>
            vJoyMap[vJoyType];
    }
}
