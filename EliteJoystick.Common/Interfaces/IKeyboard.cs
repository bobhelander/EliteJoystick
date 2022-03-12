using EliteJoystick.Common.Logic;
using System.Threading.Tasks;

namespace EliteJoystick.Common.Interfaces
{
    public interface IKeyboard
    {
        /// <summary>
        /// Press a key
        /// </summary>
        /// <param name="code">keyboard code for the key</param>
        /// <param name="modifiers">an array of modifiers also pressed</param>
        /// <param name="duration">Milliseconds before releasing key. -1 to leave pressed until it is released</param>
        /// <returns></returns>
        Task PressKey(byte code, KeyCode[] modifiers = null, int duration = 50);

        /// <summary>
        /// Release the key using the keyboard code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task ReleaseKey(byte code, KeyCode[] modifiers = null);

        /// <summary>
        /// Look up the keyboard code value for the value given. Press that key.  It will also press modifiers if the key is a shifted value. 
        /// </summary>
        /// <param name="value">Character to press</param>
        /// <param name="modifiers">an array of modifiers also pressed</param>
        /// <param name="duration">Milliseconds before releasing key. -1 to leave pressed until it is released</param>
        /// <returns></returns>        
        Task PressKey(string value, KeyCode[] modifiers = null, int duration = 50);

        /// <summary>
        /// Look up the keyboard code value for the value given. Release that key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Task ReleaseKey(string value, KeyCode[] modifiers = null);

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
    }
}
