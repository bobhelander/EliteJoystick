﻿using System;
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
using vJoyMapping.Common;
using EliteJoystick.Common;
using CommonCommunication;
using System.Reactive.Linq;

namespace EliteJoystickService
{
    public partial class JoystickService : ServiceBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private EliteVirtualJoysticks eliteVirtualJoysticks = null;
        private EliteSharedState SharedState { get; set; } = new EliteSharedState();
        private ArduinoCommunication.Arduino arduino = null;
        private CommonCommunication.Server server = null;
        private CommonCommunication.Client client = null;
        private Settings settings;
        private MessageHandler messageHandler = null;
        private Task IpcProcessingTask;
        private List<Controller> Controllers { get; set; } = new List<Controller>();
        private IDisposable virtualControllerUpdater = null;

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
                eliteVirtualJoysticks.Initialize();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }

            return eliteVirtualJoysticks;
        }

        private void StartControllers(
            Settings settings,
            EliteVirtualJoysticks eliteVirtualJoysticks,
            EliteSharedState sharedState)
        {
            log.Debug("Connecting to Controllers");   

            try
            {
                var ffb2 = new vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Controller
                {
                    Arduino = arduino,
                    Name = "Force Feedback 2",
                    SharedState = SharedState,
                    Settings = settings,
                    VirtualJoysticks = eliteVirtualJoysticks
                };

                ffb2.Initialize(Controller.GetDevicePath(
                    Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Joystick.VendorId,
                    Usb.GameControllers.Microsoft.Sidewinder.ForceFeedback2.Joystick.ProductId));

                Controllers.Add(ffb2);


                var swgv = new vJoyMapping.Microsoft.Sidewinder.GameVoice.Controller
                {
                    Arduino = arduino,
                    Name = "Game Voice",
                    SharedState = SharedState,
                    Settings = settings,
                    VirtualJoysticks = eliteVirtualJoysticks
                };

                swgv.Initialize(Controller.GetDevicePath(
                    Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Joystick.VendorId,
                    Usb.GameControllers.Microsoft.Sidewinder.GameVoice.Joystick.ProductId));

                Controllers.Add(swgv);

                var swsc = new vJoyMapping.Microsoft.Sidewinder.StrategicCommander.Controller
                {
                    Arduino = arduino,
                    Name = "Strategic Commander",
                    SharedState = SharedState,
                    Settings = settings,
                    VirtualJoysticks = eliteVirtualJoysticks
                };

                swsc.Initialize(Controller.GetDevicePath(
                    Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Joystick.VendorId,
                    Usb.GameControllers.Microsoft.Sidewinder.StrategicCommander.Joystick.ProductId));

                Controllers.Add(swsc);

                var warthog = new vJoyMapping.Thrustmaster.Warthog.Throttle.Controller
                {
                    Arduino = arduino,
                    Name = "Warthog Throttle",
                    SharedState = SharedState,
                    Settings = settings,
                    VirtualJoysticks = eliteVirtualJoysticks
                };

                warthog.Initialize(Controller.GetDevicePath(
                    Usb.GameControllers.Thrustmaster.Warthog.Throttle.Joystick.VendorId,
                    Usb.GameControllers.Thrustmaster.Warthog.Throttle.Joystick.ProductId));

                Controllers.Add(warthog);

                var pedals = new vJoyMapping.CHProducts.ProPedals.Controller
                {
                    Arduino = arduino,
                    Name = "Pro Pedals",
                    SharedState = SharedState,
                    Settings = settings,
                    VirtualJoysticks = eliteVirtualJoysticks
                };

                pedals.Initialize(Controller.GetDevicePath(
                    Usb.GameControllers.CHProducts.ProPedals.Joystick.VendorId,
                    Usb.GameControllers.CHProducts.ProPedals.Joystick.ProductId));

                Controllers.Add(pedals);

                var bbi32 = new vJoyMapping.LeoBodnar.BBI32.Controller
                {
                    Arduino = arduino,
                    Name = "BBI32",
                    SharedState = SharedState,
                    Settings = settings,
                    VirtualJoysticks = eliteVirtualJoysticks
                };

                bbi32.Initialize(Controller.GetDevicePath(
                    Usb.GameControllers.LeoBodnar.BBI32.Joystick.VendorId,
                    Usb.GameControllers.LeoBodnar.BBI32.Joystick.ProductId));

                Controllers.Add(bbi32);

                // State Handlers
                var subscription = SharedState.GearChanged.Subscribe(
                    x => ffb2.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LandingGearToggle, 200));

                virtualControllerUpdater = StartUpdateData(eliteVirtualJoysticks, 300);

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

        private IDisposable StartUpdateData(EliteVirtualJoysticks eliteVirtualJoysticks, int updateFrequency = 100)
        {
            // Send an update message every x milliseconds
            return Observable.Interval(TimeSpan.FromMilliseconds(updateFrequency)).Subscribe(x => eliteVirtualJoysticks.UpdateAll());
        }

        private void ReceiveMessage(string message)
        {
            log.Debug($"Message: {message}");
            if (false == String.IsNullOrEmpty(message))
            {
                var task = Task.Run(async () => await messageHandler.HandleMessage(message, SharedState, arduino))
                    .ContinueWith(t => { log.Error($"Message Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        private void StartService(string[] args)
        {
            try
            {
                settings = Settings.Load();
                ClientActions.ClientAction += ClientActions_ClientAction;                
                client = new CommonCommunication.Client();
                messageHandler = new MessageHandler
                {                    
                    Client = client,
                    ConnectJoysticks = () => {
                        eliteVirtualJoysticks = StartVirtualJoysticks();
                        StartControllers(settings, eliteVirtualJoysticks, SharedState);
                    },
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
            settings.Save();
            eliteVirtualJoysticks?.Release();
        }        
    }
}
