using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EliteJoystickClient
{
    public class Client
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Name { get; set; }
        public CommonCommunication.Server Server { get; set; }
        public MessageHandler MessageHandler { get; set; }
        public ChromeController.Chrome Chrome { get; set; }

        public async Task Initialize()
        {
            // Add the standard message handler
            MessageHandler = new MessageHandler();

            // Create the client server pipe
            Server = new CommonCommunication.Server { ContinueListening = true };
            //Task.Run(() => { Server.StartListening(Name, MessageHandler.HandleMessage); }).Start();
            Task.Run(async () => { await Server.StartListening(Name, MessageHandler.HandleMessage); });

            // Contact the Service Pipe
            MessageHandler.Client = new CommonCommunication.Client();
            MessageHandler.Client.CreateConnection("elite_joystick");

            // Begin two way communication
            var message = JsonConvert.SerializeObject(
                new CommonCommunication.Message { Type = "client_ready", Data = Name });

            await MessageHandler.Client.SendMessageAsync(message);

            Chrome = new ChromeController.Chrome("http://localhost:9222");
        }

        public async Task HandleCommand(string command, Dictionary<string, string> environmentVars)
        {
            log.Debug($"Receieved command: {command}");
            switch (command)
            {
                case "connect_joysticks":                    
                    await ConnectJoysticks();
                    break;
                case "connect_arduino":
                    await ConnectArduino();
                    break;
                case "paste_clipboard":
                    await PasteClipboard();
                    break;
            }
        }

        public async Task ConnectJoysticks()
        {
            var message = JsonConvert.SerializeObject(new CommonCommunication.Message { Type = "connect_joysticks" });
            await MessageHandler.Client.SendMessageAsync(message);
        }

        public async Task ConnectArduino()
        {
            var message = JsonConvert.SerializeObject(new CommonCommunication.Message { Type = "connect_arduino" });
            await MessageHandler.Client.SendMessageAsync(message);
        }

        public async Task PasteClipboard()
        {
            if (System.Windows.Clipboard.ContainsText())
            {
                var data = Utils.CallClipboard();

                var outputMessage = JsonConvert.SerializeObject(
                    new CommonCommunication.Message { Type = "keyboard_output", Data = data });

                log.Debug($"Sending Clipboard Contents: {data}");

                await MessageHandler.Client.SendMessageAsync(outputMessage);
            }
        }

        public void Navigate(string url)
        {
            var sessions = Chrome.GetAvailableSessions();
            if (sessions?.Count > 0)
            {
                var sessionWSEndpoint = sessions[0].webSocketDebuggerUrl;
                Chrome.SetActiveSession(sessionWSEndpoint);

                Chrome.NavigateTo(url);
            }
        }
    }
}
