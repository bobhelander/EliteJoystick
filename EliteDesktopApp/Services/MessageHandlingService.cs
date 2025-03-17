using EliteGameStatus.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EliteDesktopApp.Services
{
    /// <summary>
    /// This service is the glue between messages received on the IPC message bus and the handler of those messages.  The service
    /// also contains the startup code to connect to the controllers.  When the ControllersService is requested, each of the controllers
    /// injected into that service are started and connect to the virtual joysticks.  This is done through the construtor code of 
    /// each controller.
    /// </summary>
    public class MessageHandlingService : MessageHandler, IDisposable
    {
        private readonly CommonCommunication.Client client;
        private IServiceScope? controllerScope;
        private ControllersService? controllersService;

        public MessageHandlingService(
            IServiceProvider serviceProvider,
            GameService gameService,
            ILogger<MessageHandlingService> log)
        {
            CommonCommunication.ClientActions.ClientAction += ClientActions_ClientAction;
            client = new CommonCommunication.Client()
            {
                Logger = log
            };

            Client = client;

            // Connect the messages with the handlers
            ConnectJoysticks = () => HandleStartControllers(serviceProvider, gameService, log);
            DisconnectJoysticks = () => HandleStopControllers(log);
            ConnectArduino = () => HandleConnectArduino(log);
            DisconnectArduino = () => HandleDisconnectArduino(log);
            ReconnectArduino = () => HandleReconnectArduino(log);
            Logger = log;
            //KeyPress = (string data) => KeyPress(data);
        }

        public void ReceiveMessage(string message)
        {
            Logger?.LogDebug("Message: {message}", message);
            if (string.IsNullOrEmpty(message) == false)
            {
                Task.Run(async () => await HandleMessage(message).ConfigureAwait(false))
                    .ContinueWith(t => Logger?.LogError("Message Exception: {exception}", t.Exception), TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        private void HandleStartControllers(
            IServiceProvider serviceProvider,
            GameService gameService,
            ILogger log)
        {
            log.LogDebug("Connecting to Elite Game");

            gameService.Initialize();

            log.LogDebug("Connected to Elite Game");

            log.LogDebug("Connecting to Controllers");

            // Create a service scope so we can release the joystick services when we receive the disconnect message.
            controllerScope = serviceProvider.CreateScope();

            // Get the service from that service scope
            controllersService = controllerScope.ServiceProvider.GetRequiredService<ControllersService>();
        }

        private void HandleStopControllers(ILogger log)
        {
            controllerScope?.Dispose();
            controllersService = null;
        }

        private void HandleConnectArduino(ILogger log)
        {
        }

        private void HandleDisconnectArduino(ILogger log)
        {
        }

        private void HandleReconnectArduino(ILogger log)
        {
            DisconnectArduino();
            ConnectArduino();
        }

        /*
        private void KeyPress(string data)
        {
            log.LogDebug($"Keypress: {data}");
            var message = JsonConvert.DeserializeObject<EliteJoystick.Common.Messages.KeyboardMessage>(data);
            KeyboardController.Notify(message);
        }
        */

        private void ClientActions_ClientAction(object? sender, CommonCommunication.ClientActions.ClientEventArgs e)
        {
            var task = Task.Run(async() => await client.SendMessageAsync(e.Message))
                .ContinueWith(t => Logger?.LogError("Send Message Exception: {exception}", t.Exception), TaskContinuationOptions.OnlyOnFaulted);
        }

        public void Dispose()
        {
        }
    }
}
