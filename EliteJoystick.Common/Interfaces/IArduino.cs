using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Common.Interfaces
{
    public interface IArduino
    {
        void DepressKey(byte key);
        void ReleaseKey(byte key);
        void ReleaseAll();
        Task TypeFullString(string text);
        Task TypeFromClipboard();
        Task KeyCombo(byte[] modifier, byte key);
    }
}
