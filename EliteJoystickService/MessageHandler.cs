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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CommonCommunication.Client Client { get; set; }
        public Action ConnectJoysticks { get; set; }
        public Action ConnectArduino { get; set; }

        public async void HandleMessage(
            string rawMessage, 
            Controllers.EliteSharedState sharedState, 
            ArduinoCommunication.Arduino arduino)
        {
            var message = JsonConvert.DeserializeObject<CommonCommunication.Message>(rawMessage);

            log.Debug($"Message Type received: {message.Type}");

            switch (message?.Type)
            {
                case "client_ready":
                    await Task.Run(() => Client.CreateConnection(message.Data));
                    break;
                case "connect_joysticks":
                    await Task.Run(ConnectJoysticks);
                    break;
                case "connect_arduino":
                    await Task.Run(ConnectArduino);
                    break;
                case "keyboard_output":
                    log.Debug($"Arduino: testing");
                    await Task.Run(() => ArduinoCommunication.Utils.TypeFullString(arduino, message.Data, null));
                    break;
                default:
                    //Unknown message
                    break;
            }
        }
    }
}
