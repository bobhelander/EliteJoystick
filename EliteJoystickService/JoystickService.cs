using System;
using System.ServiceProcess;
using Microsoft.Extensions.Logging;
using EliteJoystickService.Services;

namespace EliteJoystickService
{
    public partial class JoystickService : ServiceBase
    {
        private ILogger Log { get; }

        private readonly MessageHandlingService messageHandlingService;
        private readonly IpcService ipcService;

        /// <summary>
        /// Starts the JoystickService.
        /// IpcService Starts the interprocess communication layer.
        /// MessageHandlingService processes the messages that come across IPC and connects the joysticks when it receives the message.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="messageHandlingService"></param>
        /// <param name="ipcService"></param>
        public JoystickService(
            ILogger<JoystickService> log,
            MessageHandlingService messageHandlingService,
            IpcService ipcService)
        {
            InitializeComponent();
            this.Log = log;
            this.messageHandlingService = messageHandlingService;
            this.ipcService = ipcService;
        }

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }

        public void Start()
        {
            this.OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            Log?.LogDebug("Starting Service");
            StartService(args);
        }

        protected override void OnStop()
        {
            Log?.LogDebug("Stopping Service");
            StopService();
        }

        private void StartService(string[] args)
        {
        }

        private void StopService()
        {
        }
    }
}
