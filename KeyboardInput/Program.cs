using Linearstar.Windows.RawInput;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyboardInput
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // To begin catching inputs, first make a window that listens WM_INPUT.
            var window = new RawInputReceiverWindow();

            // Build the IPC Pipe
            var Client = new CommonCommunication.Client();

            // Create the event handler
            window.Input += async (sender, e) =>
            {
                // ECVILLA Device
                if (e.Data.Device.DevicePath == @"\\?\HID#VID_0C45&PID_760A&MI_00#b&22a65cf9&0&0000#{884b96c3-56ef-11d1-bc8c-00a0c91405dd}")
                {
                    if (false == Client.IsConnected())
                    {
                        // Connect to IPC service
                        Console.WriteLine($"Connecting to elite_keyboard");

                        await Client.CreateConnection("elite_keyboard").ConfigureAwait(false);

                        Console.WriteLine($"Connected to elite_keyboard");
                    }

                    if (e.Data is RawInputKeyboardData)
                    {
                        var keyboardData = e.Data as RawInputKeyboardData;

                        var messageBody = new EliteJoystick.Common.Messages.KeyboardMessage
                        {
                            VirutalKey = keyboardData.Keyboard.VirutalKey,
                            ScanCode = keyboardData.Keyboard.ScanCode,
                            Flags = (int)keyboardData.Keyboard.Flags
                        };

                        var message = new CommonCommunication.Message
                        {
                            Type = "keypress",
                            Data = JsonConvert.SerializeObject(messageBody)
                        };

                        // Send the message to the IPC channel
                        await Client.SendMessageAsync(JsonConvert.SerializeObject(message)).ConfigureAwait(false);

                        Console.WriteLine($"{e.Data.Device.DevicePath}: {e.Data}");
                    }
                }
            };

            try
            {
                // Register the HidUsageAndPage to watch any device.
                RawInputDevice.RegisterDevice(HidUsageAndPage.Keyboard, RawInputDeviceFlags.ExInputSink | RawInputDeviceFlags.NoLegacy, window.Handle);

                Console.WriteLine($"Ready");

                // Run the listener
                Application.Run();
            }
            finally
            {
                RawInputDevice.UnregisterDevice(HidUsageAndPage.Keyboard);
            }
        }
    }
}
