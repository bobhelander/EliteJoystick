using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vJoyInterfaceWrap;

namespace EliteJoystick.Thrustmaster.Warthog
{
    public class TmThrottleController : Controller
    {
        static String Name = "Warthog";

        public static TmThrottleController Create(EliteSharedState sharedState, vJoy vjoy, vJoyMapper vJoyMapper, ArduinoCommunication.Arduino arduino)
        {
            return new TmThrottleController()
            {                
                Controller = Faz.SideWinderSC.Logic.TMWartHogThrottleController.RetrieveAll()?.First(),
                SharedState = sharedState,
                vJoy = vjoy,
                vJoyMapper = vJoyMapper,
                Arduino = arduino,
                VisualState = new VisualState { Name = Name }
            };
        }

        public override void Initialize()
        {
            controller?.Initialize();

            CycleLights();

            VisualState.Initialized = true;
            VisualState.Message = "Initialized";
        }

        public List<Axis> Axis { get; set; }
        public List<Button> Buttons { get; set; }
        public List<TmThrottleHat> POVHats { get; set; }
        public List<StateHandler> StateHandlers { get; set; }

        private Faz.SideWinderSC.Logic.TMWartHogThrottleController controller;
        public Faz.SideWinderSC.Logic.TMWartHogThrottleController Controller
        {
            get { return controller; }
            set
            {
                controller = value;
                if (null != controller)
                {
                    var zAxis = new TmThrottleZJoystick { Name = "Z", LeftEnabled = true, RightEnabled = true, TmThrottleController = this };
                    Axis = new List<Axis>
                    {
                        zAxis,
                        new TmThrottleXYJoystick { Name="XY", TmThrottleController = this },
                        new TmThrottleSliderJoystick { Name="S0", TmThrottleController = this },
                    };

                    Buttons = new List<Button> {};

                    POVHats = new List<TmThrottleHat>
                        {
                            new TmThrottleHat { TmThrottleController = this,
                                vButtons = new List<uint> {
                                    MappedButtons.ThrottleHatUp,
                                    MappedButtons.ThrottleHatRight,
                                    MappedButtons.ThrottleHatDown,
                                    MappedButtons.ThrottleHatLeft }},
                        };

                    StateHandlers = new List<StateHandler>
                    {
                        new TmThrottleButtonStateHandler { TmThrottleController = this },
                        new TmThrottleStateModifier { TmThrottleController = this },
                        new TmThrottleCycleCommand { vButton = MappedButtons.CycleSubsystem, Delay=250, TmThrottleController = this },
                        new TmThrottleSecondaryFireCommand { vButton = MappedButtons.SecondaryFire, TmThrottleController = this },
                        new TmThrottleClearMessages { vButton = MappedButtons.TextMessageEntry, Delay=30, TmThrottleController = this },
                        new TmThrottleHardpointsCommand { TmThrottleController = this },
                        new TmThrottleLandedStateHandler { ZAxis = zAxis, TmThrottleController = this },
                        new TmThrottleLandingGearCommand { vButton = MappedButtons.LandingGearToggle, Delay = 400, TmThrottleController = this },
                        new TmThrottle75Command { TmThrottleController = this },
                        new TmThrottleVoiceCommandHandler { TmThrottleController = this },
                        new TmThrottleCameraCommand { TmThrottleController = this },
                        new TmThrottleLightsCommand { TmThrottleController = this },
                        new TmThrottleSilentCommand { TmThrottleController = this },
                    };
                }
            }
        }

        private EliteSharedState sharedState;
        public EliteSharedState SharedState
        {
            get { return sharedState; }
            set
            {
                sharedState = value;
                //sharedState.ModeChanged += SharedState_ModeChanged;
            }
        }

        private void SharedState_ModeChanged(object sender, EliteSharedState.ModeChangedEventArgs e)
        {
        }


        private void CycleLights()
        {
            controller.LightIntensity = 5;
            System.Threading.Thread.Sleep(100);
            controller.Lights = (byte)Faz.SideWinderSC.Logic.TmThrottleLights.LEDBacklight;
            //System.Threading.Thread.Sleep(500);
            //controller.Lights =
            //    (byte)(Faz.SideWinderSC.Logic.TmThrottleLights.LEDBacklight |
            //           Faz.SideWinderSC.Logic.TmThrottleLights.LED1);
            //System.Threading.Thread.Sleep(500);
            //controller.Lights =
            //    (byte)(Faz.SideWinderSC.Logic.TmThrottleLights.LEDBacklight |
            //           Faz.SideWinderSC.Logic.TmThrottleLights.LED1 |
            //           Faz.SideWinderSC.Logic.TmThrottleLights.LED2);
            //System.Threading.Thread.Sleep(500);
            //controller.Lights =
            //    (byte)(Faz.SideWinderSC.Logic.TmThrottleLights.LEDBacklight |
            //           Faz.SideWinderSC.Logic.TmThrottleLights.LED1 |
            //           Faz.SideWinderSC.Logic.TmThrottleLights.LED2 |
            //           Faz.SideWinderSC.Logic.TmThrottleLights.LED3);
            //System.Threading.Thread.Sleep(500);
            //controller.Lights =
            //    (byte)(Faz.SideWinderSC.Logic.TmThrottleLights.LEDBacklight |
            //           Faz.SideWinderSC.Logic.TmThrottleLights.LED1 |
            //           Faz.SideWinderSC.Logic.TmThrottleLights.LED2 |
            //           Faz.SideWinderSC.Logic.TmThrottleLights.LED3 |
            //           Faz.SideWinderSC.Logic.TmThrottleLights.LED4);
            //System.Threading.Thread.Sleep(500);
            //controller.LightIntensity = 1;
            //controller.Lights =
            //    (byte)(Faz.SideWinderSC.Logic.TmThrottleLights.LEDBacklight |
            //           Faz.SideWinderSC.Logic.TmThrottleLights.LED1 |
            //           Faz.SideWinderSC.Logic.TmThrottleLights.LED2 |
            //           Faz.SideWinderSC.Logic.TmThrottleLights.LED3 |
            //           Faz.SideWinderSC.Logic.TmThrottleLights.LED4 |
            //           Faz.SideWinderSC.Logic.TmThrottleLights.LED5);
            //System.Threading.Thread.Sleep(500);
            //controller.LightIntensity = 3;
            //System.Threading.Thread.Sleep(500);
            //controller.LightIntensity = 5;

            //System.Threading.Thread.Sleep(500);
            
            //controller.Lights =
            //    (byte)(Faz.SideWinderSC.Logic.TmThrottleLights.LEDBacklight);
            //controller.LightIntensity = 5;
        }
    }
}

