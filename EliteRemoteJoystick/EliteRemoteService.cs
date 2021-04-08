using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using vJoyMapping.Common;

namespace EliteRemoteJoystick
{
    partial class EliteRemoteService : ServiceBase
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private EliteVirtualJoysticks eliteVirtualJoysticks = null;

        public EliteRemoteService()
        {
            InitializeComponent();
        }

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            //ClientActions.Action(this, ClientActions.ClipboardAction());
            Console.ReadLine();
            this.OnStop();
        }

        public void Start()
        {
            this.OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            log.Debug("Starting Service");
            StartService(args);
        }

        protected override void OnStop()
        {
            log.Debug("Stopping Service");
            StopService();
        }

        private EliteVirtualJoysticks StartVirtualJoysticks()
        {
            var eliteVirtualJoysticks = new EliteVirtualJoysticks();

            try
            {
                eliteVirtualJoysticks.Initialize();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return eliteVirtualJoysticks;
        }

        private void StartService(string[] args)
        {
            try
            {
                eliteVirtualJoysticks = StartVirtualJoysticks();

                /*
                settings = Settings.Load();
                ClientActions.ClientAction += ClientActions_ClientAction;
                client = new CommonCommunication.Client();
                messageHandler = new MessageHandler
                {
                    Client = client,
                    ConnectJoysticks = () => {
                        eliteVirtualJoysticks = StartVirtualJoysticks();
                        StartControllers(settings, eliteVirtualJoysticks);
                    },
                    DisconnectJoysticks = () => StopControllers(),
                    ConnectArduino = () => ConnectArduino(),
                    DisconnectArduino = () => DisconnectArduino(),
                    ReconnectArduino = () => ReconnectArduino(),
                    KeyPress = (string data) => KeyPress(data),
                };

                StartIpcServer();
                */
            }
            catch (Exception ex)
            {
                log.Error($"Error starting service: {ex.Message}");
            }
        }

        private void StopService()
        {
            /*
            server.ContinueListening = false;
            serverKeyboard.ContinueListening = false;
            settings.Save();
            eliteVirtualJoysticks?.Release();
            DisconnectArduino();
            */
        }
    }
}
