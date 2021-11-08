using EliteJoystick.Common;
using EliteJoystick.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystickService
{
    public class MessageHandler
    {
        public CommonCommunication.Client Client { get; set; }
        public Action ConnectJoysticks { get; set; }
        public Action DisconnectJoysticks { get; set; }
        public Action ConnectArduino { get; set; }
        public Action DisconnectArduino { get; set; }
        public Action ReconnectArduino { get; set; }
        public Action<string> KeyPress { get; set; }
        public ILogger Logger { get; set; }

        public async Task HandleMessage(
            string rawMessage)
        {
            var message = JsonConvert.DeserializeObject<CommonCommunication.Message>(rawMessage);

            Logger?.LogDebug($"Message Type received: {message.Type}");

            switch (message?.Type)
            {
                case "client_ready":
                    await Task.Run(() => Client.CreateConnection(message.Data)).ConfigureAwait(false);
                    break;
                case "connect_joysticks":
                    await Task.Run(ConnectJoysticks).ConfigureAwait(false);
                    break;
                case "disconnect_joysticks":
                    await Task.Run(DisconnectJoysticks).ConfigureAwait(false);
                    break;
                case "connect_arduino":
                    await Task.Run(ConnectArduino).ConfigureAwait(false);
                    break;
                case "disconnect_arduino":
                    await Task.Run(DisconnectArduino).ConfigureAwait(false);
                    break;
                case "reconnect_arduino":
                    await Task.Run(ReconnectArduino).ConfigureAwait(false);
                    break;
                //case "keypress":
                //    await Task.Run(() => KeyPress(message.Data)).ConfigureAwait(false);
                //    break;
                //case "keyboard_output":
                //    await ArduinoCommunication.Utils.TypeFullString(arduino, message.Data, Logger).ConfigureAwait(false);
                //    break;
                default:
                    //Unknown message
                    break;
            }
        }
    }
}
