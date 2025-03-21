﻿using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EliteDesktopApp
{
    public class MessageHandler
    {
        public required CommonCommunication.Client Client { get; set; }
        public required Action ConnectJoysticks { get; set; }
        public required Action DisconnectJoysticks { get; set; }
        public required Action ConnectArduino { get; set; }
        public required Action DisconnectArduino { get; set; }
        public required Action ReconnectArduino { get; set; }
        public required Action<string> KeyPress { get; set; }
        public ILogger? Logger { get; set; }

        public async Task HandleMessage(
            string rawMessage)
        {
            var message = JsonConvert.DeserializeObject<CommonCommunication.Message>(rawMessage);

            Logger?.LogDebug("Message Type received: {messageType}", message?.Type);

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
