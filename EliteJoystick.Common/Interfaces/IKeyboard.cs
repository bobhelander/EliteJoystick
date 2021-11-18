using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Common.Interfaces
{
    public interface IKeyboard
    {
        /// <summary>
        /// Send Keys to keyboard.  This action sends the exact keys to the virtual keyboard. To release keys send 0x00 to the code or modifiers
        /// </summary>
        /// <param name="modifiers">bitmask of the modifiers pressed</param>
        /// <param name="code0">Key 1</param>
        /// <param name="code1">Key 2</param>
        /// <param name="code2">Key 3</param>
        /// <param name="code3">Key 4</param>
        /// <param name="code4">Key 5</param>
        /// <param name="code5">Key 6</param>
        /// <returns></returns>
        Task KeyAction(byte modifiers, byte code0, byte code1 = 0x00, byte code2 = 0x00, byte code3 = 0x00, byte code4 = 0x00, byte code5 = 0x00);

        /// <summary>
        /// Press the key using the keyboard code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task DepressKey(byte code);

        /// <summary>
        /// Release the key using the keyboard code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task ReleaseKey(byte code);

        /// <summary>
        /// Look up the keyboard code value for the value given. Press that key.  It will also press modifiers if the key is a shifted value. 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task DepressKey(string value);

        /// <summary>
        /// Look up the keyboard code value for the value given. Release that key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task ReleaseKey(string value);

        /// <summary>
        /// Release all keys
        /// </summary>
        /// <returns></returns>
        Task ReleaseAll();

        /// <summary>
        /// Type the given text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        Task TypeFullString(string text);

        /// <summary>
        /// Read the text clipboard and type that text.
        /// </summary>
        /// <returns></returns>
        Task TypeFromClipboard();

        /// <summary>
        /// Use the KeyAction() instead
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task KeyCombo(byte[] modifier, byte key);

        /// <summary>
        /// Press and release a key using the keyboard code.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        Task PressKey(byte code, int duration = 30);

        /// <summary>
        /// Look up the keyboard code value for the value given. Press that key.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        Task PressKey(string value, int duration = 30);

        /// <summary>
        /// Press and release a key using the keyboard code and modifiers.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        Task PressKey(byte modifiers, byte code, int duration = 50);
    }
}
