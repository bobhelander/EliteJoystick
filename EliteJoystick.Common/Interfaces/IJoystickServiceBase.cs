using System;
using System.Collections.Generic;
using System.Text;

namespace EliteJoystick.Common.Interfaces
{
    public interface IJoystickServiceBase: IDisposable
    {
        void Initialize();
    }
}
