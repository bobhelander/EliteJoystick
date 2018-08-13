using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faz.SideWinderSC.Logic;
using vJoyInterfaceWrap;

namespace Controllers.GearHead.Keypad
{
    public class GhKpController : Controller
    {
        static readonly string Name = "Key Pad";

        public class ModifierChangedArgs {};

        public event EventHandler<ModifierChangedArgs> BeforeModifierChanged;
        public event EventHandler<ModifierChangedArgs> AfterModifierChanged;

        public static GhKpController Create(EliteSharedState sharedState, vJoy vjoy, vJoyMapper vJoyMapper)
        {
            return new GhKpController()
            {
                Controller = Faz.SideWinderSC.Logic.KeypadController.RetrieveAll()?.First(),
                SharedState = sharedState,
                vJoy = vjoy,
                vJoyMapper = vJoyMapper,
                //VisualState = new VisualState { Name = Name }
            };
        }

        public override void Initialize()
        {
            controller?.Initialize();
            CycleLights();

        }

        private EliteSharedState sharedState;
        public EliteSharedState SharedState
        {
            get { return sharedState; }
            set
            {
                if (null != value)
                {
                    //value.MuteChanged += Value_MuteChanged;
                    //value.GearChanged += Value_GearChanged;
                }

                sharedState = value;
            }
        }

        public List<Button> Buttons { get; set; }
        public List<StateHandler> StateHandlers { get; set; }

        private Faz.SideWinderSC.Logic.KeypadController controller;

        public Faz.SideWinderSC.Logic.KeypadController Controller
        {
            get { return controller; }
            set
            {
                controller = value;
                if (null != controller)
                {
                    StateHandlers = new List<StateHandler>
                    {
                        new GhKpButtonsStateHandler { GhKpController = this },
                        new GhKpUtilCommandsStateHandler { GhKpController = this }
                    };
                }
            }
        }

        private void CycleLights()
        {
        }
    }
}

