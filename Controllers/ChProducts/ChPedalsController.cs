using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faz.SideWinderSC.Logic;

namespace Controllers.ChProducts
{
    public class ChPedalsController : Controller
    {
        static readonly string Name = "CH Pedals";

        public static ChPedalsController Create(EliteSharedState sharedState, vJoyInterfaceWrap.vJoy vjoy, vJoyMapper vJoyMapper)
        {
            return new ChPedalsController()
            {
                Controller = Faz.SideWinderSC.Logic.CHPedalsController.RetrieveAll()?.First(),
                SharedState = sharedState,
                vJoy = vjoy,
                vJoyMapper = vJoyMapper,
            };
        }

        public override void Initialize()
        {
            controller?.Initialize();
        }

        public List<Axis> Axis { get; set; }

        private CHPedalsController controller;

        public CHPedalsController Controller
        {
            get { return controller; }
            set
            {
                controller = value;
                if (null != controller)
                {
                    Axis = new List<Axis>
                    {
                        new ChPedalsXYcombined { Name="XY", ChPedalsController = this },
                        new ChPedalsRJoystick { Name="Z", ChPedalsController = this },
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

