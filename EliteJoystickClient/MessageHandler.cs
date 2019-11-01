using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystickClient
{
    public class MessageHandler
    {
        private static readonly log4net.ILog log = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CommonCommunication.Client Client { get; set; }

        public void HandleMessage(string rawMessage)
        {
            try
            {
                var message = JsonConvert.DeserializeObject<CommonCommunication.Message>(rawMessage);

                log.Debug($"Message Type: {message.Type}");

                switch (message?.Type)
                {
                    case "clipboard":
                        if (System.Windows.Clipboard.ContainsText())
                        {
                            var data = Utils.CallClipboard();

                            var outputMessage = JsonConvert.SerializeObject(
                                new CommonCommunication.Message { Type = "keyboard_output", Data = data });

                            log.Debug($"Sending Clipboard Contents: {data}");

                            Client.SendMessageAsync(outputMessage).Wait();
                        }
                        break;

                    case "focus":
                        Utils.FocusWindow("EliteDangerous64");
                        break;

                    case "kill":
                        Utils.KillProcess("EliteDangerous64").Wait();
                        break;

                    case "navigate_url":
                        Utils.NavigateUrl(message.Data);
                        break;

                    case "client_information":
                        log.Info(message.Data);
                        break;

                    default:
                        //Unknown message
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Debug($"Error: {ex}");
            }
        }
    }
}
