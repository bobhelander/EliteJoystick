using System;
using System.Collections.Generic;
using System.Text;

namespace EliteJoystick.Common.Interfaces
{
    public interface ISettings
    {
        vJoyMapper vJoyMapper { get; set; }
        String ArduinoCommPort { get; set; }

        void Save();
    }
}
