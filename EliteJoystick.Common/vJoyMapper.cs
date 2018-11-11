using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace EliteJoystick.Common
{
    /// <summary>
    /// Allow dynamic changing of the virtual joystick output identifiers 
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

        public event PropertyChangedEventHandler PropertyChanged;

        public uint GetJoystickId(string vJoyType)
        {
            return vJoyMap[vJoyType];
        }
    }
}
