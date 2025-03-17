using System.Threading.Tasks;

namespace EliteJoystick.Common.Interfaces
{
    public interface IArduino_bak
    {
        Task DepressKey(byte key);
        Task ReleaseKey(byte key);
        Task ReleaseAll();
        Task TypeFullString(string text);
        Task TypeFromClipboard();
        Task KeyCombo(byte[] modifier, byte key);

        Task PressKey(byte key, int duration = 30);

        void Close();
    }
}
