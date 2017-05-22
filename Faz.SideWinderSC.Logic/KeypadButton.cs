using System;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Specifies the special buttons of the keypad.
    /// </summary>
    [Serializable]
    public enum KeypadButton : byte
    {
        /// <summary>
        /// The All button.
        /// </summary>
        ButtonHtml = 0x20,

        /// <summary>
        /// The Team button.
        /// </summary>
        ButtonEmail = 0x08,

        /// <summary>
        /// The 1 button.
        /// </summary>
        ButtonCalc = 0x10,
    }
}
