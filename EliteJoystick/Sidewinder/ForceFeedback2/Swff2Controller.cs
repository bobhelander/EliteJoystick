using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faz.SideWinderSC.Logic;
using vJoyInterfaceWrap;

namespace EliteJoystick.Sidewinder.ForceFeedback2
{
    public class Swff2Controller : Controller
    {
        static String Name = "Force Feedback 2";

        public static Swff2Controller Create(EliteSharedState sharedState, vJoy vjoy,
            vJoyMapper vJoyMapper, ArduinoCommunication.Arduino arduino)
        {
            return new Swff2Controller()
            {                
                Controller = Faz.SideWinderSC.Logic.Swff2Controller.RetrieveAll()?.First(),
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
        }

        public List<Axis> Axis { get; set; }
        public List<Swff2Hat> POVHats { get; set; }
        public List<StateHandler> StateHandlers { get; set; }

        private Faz.SideWinderSC.Logic.Swff2Controller controller;

        public Faz.SideWinderSC.Logic.Swff2Controller Controller
        {
            get { return controller; }
            set
            {
                controller = value;
                if (null != controller)
                {
                    Axis = new List<Axis>
                    {
                        new Swff2XYJoystick { Name="XY", Swff2Controller = this },
                        new Swff2ZJoystick { Name="Z", Swff2Controller = this },
                        new Swff2SliderJoystick { Name="S0", Swff2Controller = this },
                    };

                    POVHats = new List<Swff2Hat>
                    {
                        new Swff2Hat {
                            Swff2Controller = this,
                            vButtons = new List<uint> { 9, 10, 11, 12, 13 }
                        },
                    };

                    StateHandlers = new List<StateHandler>
                    {
                        new Swff2ButtonsStateHandler { Swff2Controller = this },
                        new Swff2ClipboardStateHandler { Swff2Controller = this },
                        new Swff2UtilCommandsStateHandler { Swff2Controller = this },
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
    }
}

