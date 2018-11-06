using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Common.Interfaces
{
    public interface IArduino
    {
        Task DepressKey(byte key);
        Task ReleaseKey(byte key);
        Task ReleaseAll();
        Task TypeFullString(string text);
        Task TypeFromClipboard();
        Task KeyCombo(byte[] modifier, byte key);
    }
}
