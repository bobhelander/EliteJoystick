using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faz.SideWinderSC.Logic;
using vJoyInterfaceWrap;

namespace Controllers.Sidewinder.GameVoice
{
    public class SwGvController : Controller
    {
        static readonly string Name = "Game Voice";

        public class ModifierChangedArgs {};

        public event EventHandler<ModifierChangedArgs> BeforeModifierChanged;
        public event EventHandler<ModifierChangedArgs> AfterModifierChanged;

        public static SwGvController Create(EliteSharedState sharedState, vJoy vjoy, vJoyMapper vJoyMapper)
        {
            return new SwGvController()
            {
                Controller = SwgvController.RetrieveAll()?.First(),
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

            // Start with gear deployed
            controller.Lights = (byte)SwgvButton.Button1;
        }

        private EliteSharedState sharedState;
        public EliteSharedState SharedState
        {
            get { return sharedState; }
            set
            {
                if (null != value)
                {
                    value.MuteChanged += Value_MuteChanged;
                    value.GearChanged += Value_GearChanged;
                }

                sharedState = value;
            }
        }

        private void Value_GearChanged(object sender, EliteSharedState.GearDeployedEventArgs e)
        {
            byte state = controller.Lights;
            if (e.Deployed)
                controller.Lights = (byte)(state | (byte)SwgvButton.Button1);
            else
                controller.Lights = (byte)(state & ~(byte)SwgvButton.Button1);
        }

        private byte oldState = 0;

        private void Value_MuteChanged(object sender, EliteSharedState.MuteChangedEventArgs e)
        {
            if (e.MuteState)
            {
                oldState = controller.Lights;
                controller.Lights = (byte)SwgvButton.MuteButton;
            }
            else
            {
                controller.Lights = oldState;
            }
        }

        public List<Button> Buttons { get; set; }
        public List<StateHandler> StateHandlers { get; set; }

        private SwgvController controller;

        public SwgvController Controller
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
                        new SwGameStateHandler { SwGvController = this },
                        new SwGameLandingGearHandler { SwGvController = this },
                        //new SwGameResetHandler { Delay=500, SwGvController = this },
                    };
                }
            }
        }

        private void CycleLights()
        {
            controller.Lights = (byte)SwgvButton.Button1;
            System.Threading.Thread.Sleep(200);
            controller.Lights =
                (byte)(SwgvButton.Button1 |
                       SwgvButton.Button2);
            System.Threading.Thread.Sleep(200);
            controller.Lights =
                (byte)(SwgvButton.Button1 |
                       SwgvButton.Button2 |
                       SwgvButton.Button3);
            System.Threading.Thread.Sleep(200);
            controller.Lights =
                (byte)(SwgvButton.Button1 |
                       SwgvButton.Button2 |
                       SwgvButton.Button3 |
                       SwgvButton.Button4);
            System.Threading.Thread.Sleep(200);
            controller.Lights = (byte)(SwgvButton.ButtonAll);
            System.Threading.Thread.Sleep(200);
            controller.Lights = (byte)(SwgvButton.ButtonTeam);
            System.Threading.Thread.Sleep(200);
            controller.Lights = (byte)(SwgvButton.MuteButton);
            System.Threading.Thread.Sleep(200);
            controller.Lights = (byte)(SwgvButton.CommandButton);
            System.Threading.Thread.Sleep(200);
            controller.Lights = 0;
        }
    }
}

