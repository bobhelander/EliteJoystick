using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystickClient
{
    public class Client
    {
        public string Name { get; set; }
        public CommonCommunication.Server Server { get; set; }
        public MessageHandler MessageHandler { get; set; }

        public void Initialize()
        {
            // Add the standard message handler
            MessageHandler = new MessageHandler();

            // Create the client server pipe
            Server = new CommonCommunication.Server { ContinueListening = true };
            Task.Run(() => Server.StartListeningAsync(Name, MessageHandler.HandleMessage));

            // Contact the Service Pipe
            MessageHandler.Client = new CommonCommunication.Client();
            MessageHandler.Client.CreateConnection("elite_joystick");

            // Begin two way communication
            var message = JsonConvert.SerializeObject(new CommonCommunication.Message { Type = "client_ready", Data = Name });
            MessageHandler.Client.SendMessageAsync(message);
        }

        public void HandleCommand(string command)
        {
            switch (command)
            {
                case "connect_joysticks":
                    ConnectJoysticks();
                    break;
                case "connect_arduino":
                    ConnectArduino();
                    break;
            }
        }

        public void ConnectJoysticks()
        {
            var message = JsonConvert.SerializeObject(new CommonCommunication.Message { Type = "connect_joysticks" });
            MessageHandler.Client.SendMessageAsync(message);
        }

        public void ConnectArduino()
        {
            var message = JsonConvert.SerializeObject(new CommonCommunication.Message { Type = "connect_arduino" });
            MessageHandler.Client.SendMessageAsync(message);
        }
    }
}
