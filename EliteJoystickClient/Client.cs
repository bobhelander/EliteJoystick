using Microsoft.Extensions.Logging;
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
        public ILogger Logger { get; set; }
        public string Name { get; set; }
        public CommonCommunication.Server Server { get; set; }
        public MessageHandler MessageHandler { get; set; }
        //public GoogleChrome.Chrome Chrome { get; set; }
        private Task ServerListeningTask { get; set; }

        public async Task Initialize()
        {
            // Add the standard message handler
            MessageHandler = new MessageHandler() { Logger = Logger };

            // Create the client server pipe
            Server = new CommonCommunication.Server { ContinueListening = true };

            ServerListeningTask = Task.Factory.StartNew(() => Server.StartListening(Name, MessageHandler.HandleMessage),
                CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                .ContinueWith(t => { Logger?.LogError($"Server Listening Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);

            // Contact the Service Pipe
            MessageHandler.Client = new CommonCommunication.Client() { Logger = Logger };

            await MessageHandler.Client.CreateConnection("elite_joystick").ConfigureAwait(false);

            // Begin two way communication
            var message = JsonConvert.SerializeObject(
                new CommonCommunication.Message { Type = "client_ready", Data = Name });

            await MessageHandler.Client.SendMessageAsync(message).ConfigureAwait(false);

            //Chrome = new GoogleChrome.Chrome("http://localhost:9222");
        }

        public async Task Shutdown()
        {
            await DisconnectJoysticks().ConfigureAwait(false);
            await DisconnectArduino().ConfigureAwait(false);

            await Task.Delay(500).ConfigureAwait(false);

            if (Server != null)
                Server.ContinueListening = false;
        }

        public async Task HandleCommand(string command, Dictionary<string, string> environmentVars)
        {
            Logger?.LogDebug($"Receieved command: {command}");
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

        public async Task ConnectJoysticks() =>
            await MessageHandler.Client.SendMessageAsync(
                JsonConvert.SerializeObject(new CommonCommunication.Message { Type = "connect_joysticks" })).ConfigureAwait(false);

        public async Task DisconnectJoysticks() =>
            await MessageHandler.Client.SendMessageAsync(
                JsonConvert.SerializeObject(new CommonCommunication.Message { Type = "disconnect_joysticks" })).ConfigureAwait(false);

        public async Task ConnectArduino() =>
            await MessageHandler.Client.SendMessageAsync(
                JsonConvert.SerializeObject(new CommonCommunication.Message { Type = "connect_arduino" })).ConfigureAwait(false);

        public async Task DisconnectArduino() =>
            await MessageHandler.Client.SendMessageAsync(
                JsonConvert.SerializeObject(new CommonCommunication.Message { Type = "disconnect_arduino" })).ConfigureAwait(false);

        public async Task SendKeyPress(string data) =>
            await MessageHandler.Client.SendMessageAsync(
                JsonConvert.SerializeObject(new CommonCommunication.Message {
                    Type = "keypress",
                    Data = data
                })).ConfigureAwait(false);

        public async Task PasteClipboard()
        {
            if (System.Windows.Clipboard.ContainsText())
            {
                var data = Utils.CallClipboard();

                var outputMessage = JsonConvert.SerializeObject(
                    new CommonCommunication.Message { Type = "keyboard_output", Data = data });

                Logger?.LogDebug($"Sending Clipboard Contents: {data}");

                await MessageHandler.Client.SendMessageAsync(outputMessage).ConfigureAwait(false);
            }
        }

        public void CopyToClipboard(string text) =>
            Utils.SetClipboardText(text);

        /*
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
            if (Chrome.CurrentSession != null)
            {
                Chrome.Scroll(Chrome.CurrentSession, distance);
            }
        }

        public void ChangeTab(int number) =>
            Chrome.ChangeTab(number);

        public string ChromeCommand(string command) =>
            Chrome.Eval(Chrome.CurrentSession, command);

        public void NewChromeTab(string url, int scrollDistance = 0)
        {
            var session = Chrome.OpenNewTab(url);
            Thread.Sleep(1500);
            Chrome.Scroll(session, scrollDistance);
        }
        */
    }
}
