using System;
using System.ServiceProcess;
using Microsoft.Extensions.Logging;
using EliteDesktopApp.Services;

namespace EliteDesktopApp
{
    public partial class JoystickService //: ServiceBase
    {
        private ILogger Log { get; }

        private readonly MessageHandlingService messageHandlingService;
        private readonly IpcService ipcService;

        /// <summary>
        /// Starts the JoystickService.
        /// IpcService Starts the interprocess communication layer.
        /// MessageHandlingService processes the messages that come across IPC and connects the joysticks when it receives the message.
        /// </summary>
        public JoystickService(            
            MessageHandlingService messageHandlingService,
            IpcService ipcService,
            ILogger<JoystickService> log)
        {
            //InitializeComponent();
            this.Log = log;
            this.messageHandlingService = messageHandlingService;
            this.ipcService = ipcService;
        }

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            System.Threading.Thread.Sleep(60000);
            Console.ReadLine();
            this.OnStop();
        }

        public void Start()
        {
            this.OnStart(Array.Empty<string>());
        }

        public void OnStart(string[] args)
        {
            Log?.LogDebug("Starting Service");
            StartService(args);
        }

        public void OnStop()
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
