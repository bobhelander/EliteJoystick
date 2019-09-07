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
        private Task ServerListeningTask { get; set; }

        public async Task Initialize()
        {
            // Add the standard message handler
            MessageHandler = new MessageHandler();

            // Create the client server pipe
            Server = new CommonCommunication.Server { ContinueListening = true };

            ServerListeningTask = Task.Factory.StartNew(() => Server.StartListening(Name, MessageHandler.HandleMessage),
                CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                .ContinueWith(t => { log.Error($"Server Listening Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);

            // Contact the Service Pipe
            MessageHandler.Client = new CommonCommunication.Client();

            await MessageHandler.Client.CreateConnection("elite_joystick").ConfigureAwait(false);

            // Begin two way communication
            var message = JsonConvert.SerializeObject(
                new CommonCommunication.Message { Type = "client_ready", Data = Name });

            await MessageHandler.Client.SendMessageAsync(message).ConfigureAwait(false);

            Chrome = new ChromeController.Chrome("http://localhost:9222");
        }

        public async Task HandleCommand(string command, Dictionary<string, string> environmentVars)
        {
            log.Debug($"Receieved command: {command}");
            switch (command)
            {
                case "connect_joysticks":
                    await ConnectJoysticks().ConfigureAwait(false);
                    break;
                case "connect_arduino":
                    await ConnectArduino().ConfigureAwait(false);
                    break;
                case "paste_clipboard":
                    await PasteClipboard().ConfigureAwait(false);
                    break;
            }
        }

        public async Task ConnectJoysticks()
        {
            var message = JsonConvert.SerializeObject(new CommonCommunication.Message { Type = "connect_joysticks" });
            await MessageHandler.Client.SendMessageAsync(message).ConfigureAwait(false);
        }

        public async Task DisconnectJoysticks()
        {
            var message = JsonConvert.SerializeObject(new CommonCommunication.Message { Type = "disconnect_joysticks" });
            await MessageHandler.Client.SendMessageAsync(message).ConfigureAwait(false);
        }

        public async Task ConnectArduino()
        {
            var message = JsonConvert.SerializeObject(new CommonCommunication.Message { Type = "connect_arduino" });
            await MessageHandler.Client.SendMessageAsync(message).ConfigureAwait(false);
        }

        public async Task DisconnectArduino()
        {
            var message = JsonConvert.SerializeObject(new CommonCommunication.Message { Type = "disconnect_arduino" });
            await MessageHandler.Client.SendMessageAsync(message).ConfigureAwait(false);
        }

        public async Task PasteClipboard()
        {
            if (System.Windows.Clipboard.ContainsText())
            {
                var data = Utils.CallClipboard();

                var outputMessage = JsonConvert.SerializeObject(
                    new CommonCommunication.Message { Type = "keyboard_output", Data = data });

                log.Debug($"Sending Clipboard Contents: {data}");

                await MessageHandler.Client.SendMessageAsync(outputMessage).ConfigureAwait(false);
            }
        }

        public void CopyToClipboard(string text)
        {
            Utils.SetClipboardText(text);
        }

        public void Navigate(string url)
        {
            var sessions = Chrome.GetAvailableSessions();
            if (sessions?.Count > 0)
            {
                Chrome.ActivateTab(sessions[0]);
                Chrome.NavigateTo(sessions[0], url);
            }
        }

        public void Scroll(int distance)
        {
            if (null != Chrome.CurrentSession)
            {
                Chrome.Scroll(Chrome.CurrentSession, distance);
            }
        }

        public void ChangeTab(int number)
        {
            Chrome.ChangeTab(number);
        }

        public string ChromeCommand(string command)
        {
            return Chrome.Eval(Chrome.CurrentSession, command);
        }

        public void NewChromeTab(string url, int scrollDistance = 0)
        {
            var session = Chrome.OpenNewTab(url);
            Thread.Sleep(1500);
            Chrome.Scroll(session, scrollDistance);
        }
    }
}
