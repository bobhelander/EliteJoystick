using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faz.SideWinderSC.Logic;
using vJoyInterfaceWrap;

namespace Controllers.Other.BBI32
{
    public class ButtonBoxController : Controller
    {
        static String Name = "BBI32";

        public class ModifierChangedArgs {};

        public static ButtonBoxController Create(
            EliteSharedState sharedState, vJoy vjoy, EliteVirtualJoysticks virtualJoysticks, vJoyMapper vJoyMapper, ArduinoCommunication.Arduino arduino)
        {
            return new ButtonBoxController()
            {
                Controller = BBI32Controller.RetrieveAll()?.First(),
                SharedState = sharedState,
                //vJoy = vjoy,
                VirtualJoysticks = virtualJoysticks,
                vJoyMapper = vJoyMapper,
                Arduino = arduino,
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
                }

                sharedState = value;
            }
        }

        public List<Button> Buttons { get; set; }
        public List<StateHandler> StateHandlers { get; set; }

        private BBI32Controller controller;

        public BBI32Controller Controller
        {
            get { return controller; }
            set
            {
                controller = value;
                if (null != controller)
                {
                    Buttons = new List<Button> {};

                    StateHandlers = new List<StateHandler>
                    {
                        new BBI32GameStateHandler { ButtonBoxController = this },
                        new BBI32UtilCommandsStateHandler { ButtonBoxController = this },
                    };
                }
            }
        }

        private void CycleLights()
        {
        }
    }
}

