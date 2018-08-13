using Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EliteJoystickService
{
    public partial class JoystickService : ServiceBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private EliteControllers eliteControllers = null;
        private EliteVirtualJoysticks eliteVirtualJoysticks = null;
        private vJoyMapper vJoyMapper;
        private EliteSharedState sharedState;
        private ArduinoCommunication.Arduino arduino = null;
        private CommonCommunication.Server server = null;
        private CommonCommunication.Client client = null;
        private Settings settings;
        private MessageHandler messageHandler = null;
        private Task IpcProcessingTask;

        public JoystickService()
        {
            InitializeComponent();
        }

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            ClientActions.Action(this, ClientActions.ClipboardAction());
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
                for (uint joyId = 1; joyId <= 6; joyId++)
                {
                    eliteVirtualJoysticks.Controllers.Add(new EliteVirtualJoystick
                    {
                        Joystick = eliteVirtualJoysticks.Joystick,
                        JoystickId = joyId,
                    });
                }

                eliteVirtualJoysticks.Initialize();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            return eliteVirtualJoysticks;
        }

        private void StartControllers(
            vJoyMapper vJoyMapper, 
            EliteVirtualJoysticks eliteVirtualJoysticks,
            EliteSharedState sharedState)
        {
            log.Debug("Connecting to Controllers");

            eliteControllers = new EliteControllers();

            try
            {
                eliteControllers.Controllers.Add(
                    Controllers.Sidewinder.ForceFeedback2.Swff2Controller.Create(
                        sharedState,
                        eliteVirtualJoysticks.Joystick,
                        vJoyMapper,
                        arduino));

                eliteControllers.Controllers.Add(
                    Controllers.Thrustmaster.Warthog.TmThrottleController.Create(
                        sharedState,
                        eliteVirtualJoysticks.Joystick,
                        vJoyMapper,
                        arduino));

                eliteControllers.Controllers.Add(
                    Controllers.ChProducts.ChPedalsController.Create(
                        sharedState,
                        eliteVirtualJoysticks.Joystick,
                        vJoyMapper));

                eliteControllers.Controllers.Add(
                    Controllers.Sidewinder.Commander.ScController.Create(
                        sharedState,
                        eliteVirtualJoysticks.Joystick,
                        vJoyMapper));

                eliteControllers.Controllers.Add(
                    Controllers.Sidewinder.GameVoice.SwGvController.Create(
                        sharedState,
                        eliteVirtualJoysticks.Joystick,
                        vJoyMapper));

                eliteControllers.Controllers.Add(
                    Controllers.Other.BBI32.ButtonBoxController.Create(
                        sharedState,
                        eliteVirtualJoysticks.Joystick,
                        vJoyMapper,
                        arduino));

                //eliteControllers.Controllers.Add(
                //    Controllers.GearHead.Keypad.GhKpController.Create(
                //        sharedState,
                //        eliteVirtualJoysticks.Joystick,
                //        vJoyMapper));

                eliteControllers.Initialize();
                ClientActions.ClientInformationAction(this, "Controllers Ready");
                log.Debug("Controllers Ready");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ClientActions.ClientInformationAction(this, $"Controller Error: {ex.Message}");
            }
        }

        private void ConnectArduino()
        {
            arduino = new ArduinoCommunication.Arduino(settings.ArduinoCommPort);
            ClientActions.ClientInformationAction(this, "Arduino Ready");
        }

        private void KeyboardOutput(string data)
        {
            arduino = new ArduinoCommunication.Arduino(settings.ArduinoCommPort);
            ClientActions.ClientInformationAction(this, "Arduino Ready");
        }

        private void ClientActions_ClientAction(object sender, ClientActions.ClientEventArgs e)
        {
            Task.Run(() => client.SendMessageAsync(e.Message))
                .ContinueWith(t => { log.Error($"Send Message Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);
        }

        private void StartIpcServer()
        {
            log.Debug("Starting IPC services");
            server = new CommonCommunication.Server { ContinueListening = true };
            
            IpcProcessingTask = Task.Factory.StartNew(() => server.StartListening("elite_joystick", this.ReceiveMessage),
                CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                .ContinueWith(t => { log.Error($"IPC Service Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);
        }

        private void ReceiveMessage(string message)
        {
            log.Debug($"Message: {message}");
            if (false == String.IsNullOrEmpty(message))
            {
                var task = Task.Run(async () => await messageHandler.HandleMessage(message, sharedState, arduino))
                    .ContinueWith(t => { log.Error($"Message Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        private void StartService(string[] args)
        {
            try
            {
                sharedState = new EliteSharedState { OrbitLines = true, HeadsUpDisplay = true };
                settings = Settings.Load();
                //vJoyMapper = new vJoyMapper();
                ClientActions.ClientAction += ClientActions_ClientAction;
                eliteVirtualJoysticks = StartVirtualJoysticks();
                client = new CommonCommunication.Client();
                messageHandler = new MessageHandler
                {
                    Client = client,
                    ConnectJoysticks = () => StartControllers(settings.vJoyMapper, eliteVirtualJoysticks, sharedState),
                    ConnectArduino = () => ConnectArduino(),
                };

                StartIpcServer();
            }
            catch (Exception ex)
            {
                log.Error($"Error starting service: {ex.Message}");
            }
        }

        private void StopService()
        {
            server.ContinueListening = false;
            //vJoyMapper.Save();
            settings.Save();
            eliteVirtualJoysticks?.Release();
        }        
    }
}
