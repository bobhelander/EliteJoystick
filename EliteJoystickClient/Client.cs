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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            var message = JsonConvert.SerializeObject(
                new CommonCommunication.Message { Type = "client_ready", Data = Name });

            MessageHandler.Client.SendMessageAsync(message);
        }

        public void HandleCommand(string command)
        {
            log.Debug($"Receieved command: {command}");
            switch (command)
            {
                case "connect_joysticks":                    
                    ConnectJoysticks();
                    break;
                case "connect_arduino":
                    ConnectArduino();
                    break;
                case "paste_clipboard":
                    PasteClipboard();
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

        public void PasteClipboard()
        {
            if (System.Windows.Clipboard.ContainsText())
            {
                var data = Utils.CallClipboard();

                var outputMessage = JsonConvert.SerializeObject(
                    new CommonCommunication.Message { Type = "keyboard_output", Data = data });

                log.Debug($"Sending Clipboard Contents: {data}");

                MessageHandler.Client.SendMessageAsync(outputMessage);
            }
        }
    }
}
