using System;
using System.Collections.Generic;
using System.Text;

namespace EliteJoystick.Common.Messages
{
    public class KeyboardMessage
    {
        public int VirutalKey { get; set; }
        public int ScanCode { get; set; }
        public int Flags { get; set; }
    }
}
