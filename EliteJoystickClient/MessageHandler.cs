using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace EliteJoystickClient
{
    public class MessageHandler
    {
        public ILogger Logger { get; set; }
        public CommonCommunication.Client Client { get; set; }

        public void HandleMessage(string rawMessage)
        {
            try
            {
                var message = JsonConvert.DeserializeObject<CommonCommunication.Message>(rawMessage);

                Logger?.LogDebug($"Message Type: {message.Type}");

                switch (message?.Type)
                {
                    case "clipboard":
                        if (System.Windows.Clipboard.ContainsText())
                        {
                            var data = Utils.CallClipboard();

                            var outputMessage = JsonConvert.SerializeObject(
                                new CommonCommunication.Message { Type = "keyboard_output", Data = data });

                            Logger?.LogDebug($"Sending Clipboard Contents: {data}");

                            Client.SendMessageAsync(outputMessage).Wait();
                        }
                        break;

                    case "focus":
                        Utils.FocusWindow("EliteDangerous64");
                        break;

                    case "kill":
                        Utils.KillProcess("EliteDangerous64").Wait();
                        break;

                    //case "navigate_url":
                    //    Utils.NavigateUrl(message.Data);
                    //    break;

                    case "client_information":
                        Logger?.LogInformation(message.Data);
                        break;

                    default:
                        //Unknown message
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger?.LogDebug($"Error: {ex}");
            }
        }
    }
}
