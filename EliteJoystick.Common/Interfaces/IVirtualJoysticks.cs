using System;

namespace EliteJoystick.Common.Interfaces
{
    public interface IVirtualJoysticks
    {
        void Initialize();

        void Release() ;

        void UpdateAll();

        void SetJoystickButton(bool down, uint vButton, uint vJoyId);

        bool GetJoystickButton(uint vButton, uint vJoyId);

        void SetJoystickButtons(UInt32 buttons, uint vJoyId, UInt32 mask = 0xFFFFFFFF);

        void SetJoystickAxis(int value, int usage, uint vJoyId);

        void SetJoystickHat(int value, uint hatNumber, uint vJoyId);

    }
}
