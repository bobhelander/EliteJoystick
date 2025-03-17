using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using vKeyboard.Models;

namespace vKeyboard
{
    public partial class Keyboard
    {
        private const uint TIMEOUT = 10000;  //approx ten seconds

        private async Task SendAsync(Byte modifiers, Byte padding, Byte key0, Byte key1, Byte key2, Byte key3, Byte key4, Byte key5)
        {
            // The virtual keyboard takes SetFeature messages to press the keys
            var featureMessage = GetFeatureMessage(modifiers, padding, key0, key1, key2, key3, key4, key5);

            if (Controller != null)
                Controller.Feature = featureMessage;
            else if (device != null)
                await device.SetFeature(featureMessage).ConfigureAwait(false);
        }

        public Task Key(byte modifier, byte key, bool press)
        {
            // Set the values
            Modifier(modifier, press);
            Key(key, press);

            return Task.CompletedTask;
        }

        private static byte[] GetFeatureMessage(Byte modifiers, Byte padding, Byte key0, Byte key1, Byte key2, Byte key3, Byte key4, Byte key5)
        {
            var keyboardData = new SetFeatureKeyboard
            {
                ReportID = 1,
                CommandCode = 2,
                Timeout = TIMEOUT / 5, //5 because we count in blocks of 5 in the driver
                Modifier = modifiers,
                Padding = padding, //padding should always be zero.
                Key0 = key0,
                Key1 = key1,
                Key2 = key2,
                Key3 = key3,
                Key4 = key4,
                Key5 = key5
            };

            return GetMessageByteArray(keyboardData, Marshal.SizeOf(keyboardData));
        }

        //for converting a struct to byte array
        private static byte[] GetMessageByteArray(SetFeatureKeyboard sfj, int size)
        {
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(sfj, ptr, false);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
    }
}
