using Microsoft.Extensions.Logging;

namespace EliteDesktopApp.Services
{
    public class IpcService : IDisposable
    {
        private readonly CommonCommunication.Server server;
        private readonly CommonCommunication.Server serverKeyboard;

        private Task IpcProcessingTask;
        private Task IpcProcessingTask2;

        /// <summary>
        /// This service connects and forwards messages on the IPC message bus.
        /// </summary>
        /// <param name="messageHandler"></param>
        /// <param name="log"></param>
        public IpcService(
            MessageHandlingService messageHandler,
            ILogger<IpcService> log)
        {
            log?.LogDebug("Starting IPC services");
            server = new CommonCommunication.Server { ContinueListening = true, Logger = log };
            serverKeyboard = new CommonCommunication.Server { ContinueListening = true };

            IpcProcessingTask = Task.Factory.StartNew(() => server.StartListening("elite_joystick", messageHandler.ReceiveMessage),
                CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                .ContinueWith(t => log?.LogError($"IPC Service Exception: {t.Exception}"), TaskContinuationOptions.OnlyOnFaulted);

            IpcProcessingTask2 = Task.Factory.StartNew(() => serverKeyboard.StartListening("elite_keyboard", messageHandler.ReceiveMessage),
                CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                .ContinueWith(t => log?.LogError($"IPC Service Exception: {t.Exception}"), TaskContinuationOptions.OnlyOnFaulted);
        }

        public void Dispose()
        {
            if (server != null)
                server.ContinueListening = false;
            if (serverKeyboard != null)
                serverKeyboard.ContinueListening = false;
        }
    }
}
