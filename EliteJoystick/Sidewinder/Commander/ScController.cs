﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faz.SideWinderSC.Logic;
using vJoyInterfaceWrap;

namespace EliteJoystick.Sidewinder.Commander
{
    public class ScController : Controller
    {
        static String Name = "Game Commander";

        public class ModifierChangedArgs {};

        public event EventHandler<ModifierChangedArgs> BeforeModifierChanged;
        public event EventHandler<ModifierChangedArgs> AfterModifierChanged;

        public static ScController Create(EliteSharedState sharedState, vJoy vjoy, vJoyMapper vJoyMapper)
        {
            return new ScController()
            {
                Controller = SwscController.RetrieveAll()?.First(),
                SharedState = sharedState,
                vJoy = vjoy,
                vJoyMapper = vJoyMapper,
                VisualState = new VisualState { Name = Name }
            };
        }

        public override void Initialize()
        {
            controller?.Initialize();
            CycleLights();
        }

        public List<Button> Buttons { get; set; }
        public List<Axis> Axis { get; set; }
        public List<StateHandler> StateHandlers { get; set; }

        private SwscController controller;

        public SwscController Controller
        {
            get { return controller; }
            set
            {
                controller = value;
                if (null != controller)
                {
                    Buttons = new List<Button> {
                        new SwCommanderNumberButton { vButtonId = 1, ButtonId = SwscButton.Button1, ButtonLight = SwscLight.Button1, ScController = this},
                        new SwCommanderNumberButton { vButtonId = 2, ButtonId = SwscButton.Button2, ButtonLight = SwscLight.Button2, ScController = this},
                        new SwCommanderNumberButton { vButtonId = 3, ButtonId = SwscButton.Button3, ButtonLight = SwscLight.Button3, ScController = this},
                        new SwCommanderNumberButton { vButtonId = 4, ButtonId = SwscButton.Button4, ButtonLight = SwscLight.Button4, ScController = this},
                        new SwCommanderNumberButton { vButtonId = 5, ButtonId = SwscButton.Button5, ButtonLight = SwscLight.Button5, ScController = this},
                        new SwCommanderNumberButton { vButtonId = 6, ButtonId = SwscButton.Button6, ButtonLight = SwscLight.Button6, ScController = this},
                        new SwCommanderModifierButton { ButtonId = SwscButton.Shift1, ScController = this},
                        new SwCommanderModifierButton { ButtonId = SwscButton.Shift2, ScController = this},
                        new SwCommanderModifierButton { ButtonId = SwscButton.Shift3, ScController = this},
                        new SwCommanderNumberButton { vButtonId = 7, ButtonId = SwscButton.ZoomIn, ScController = this},
                        new SwCommanderNumberButton { vButtonId = 8, ButtonId = SwscButton.ZoomOut, ScController = this},
                        new SwCommanderSingleNumberButton { vButtonId = 32, ButtonId = SwscButton.Record, ButtonLight = SwscLight.Record, ScController = this},
                    };

                    Axis = new List<Axis>
                    {
                        new SwCommanderXYJoystick { Name="XY", ScController = this },
                        new SwCommanderZJoystick { Name="Z", ScController = this },
                    };
                    StateHandlers = new List<StateHandler>
                    {
                        new SwCommanderButtonsStateHandler { ScController = this },
                        new SwCommanderShiftHandler { ScController = this },
                        new SwCommanderProfileHandler { ScController = this },
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
                sharedState.ModeChanged += SharedState_ModeChanged;
                sharedState.ThrottleShiftChanged += SharedState_ThrottleShiftChanged;
            }
        }

        private void SharedState_ThrottleShiftChanged(object sender, EliteSharedState.ThrottleShiftChangedEventArgs e)
        {
            if (e.ThrottleShiftState == EliteSharedState.ThrottleShiftState.Shift1)
                Controller.SetLights(new SwscLight[] { SwscLight.Button2Flash });
        }

        private void SharedState_ModeChanged(object sender, EliteSharedState.ModeChangedEventArgs e)
        {
            if (e.Mode == EliteSharedState.Mode.Travel)
                Controller.SetLights(new SwscLight[] { SwscLight.Button2 });
            if (e.Mode == EliteSharedState.Mode.Fighting)
                Controller.SetLights(new SwscLight[] { SwscLight.Button1 });
            if (e.Mode == EliteSharedState.Mode.Mining)
                Controller.SetLights(new SwscLight[] { SwscLight.Button3 });
        }

        public void OnBeforeModifierChange()
        {
            if (null != BeforeModifierChanged)
                BeforeModifierChanged(this, new ModifierChangedArgs());
        }

        public void OnAfterModifierChange()
        {
            if (null != AfterModifierChanged)
                AfterModifierChanged(this, new ModifierChangedArgs());
        }

        public void UpdateLights()
        {
            //var lights = Buttons.Where(x => x.State && 0 != x.ButtonLight);
            //controller.SetLights(lights.Select(x => x.ButtonLight).ToArray());
        }

        public uint ModifiedButton(uint button)
        {
            if (Buttons[6].State)
                return button + 8;
            if (Buttons[7].State)
                return button + 16;
            if (Buttons[8].State)
                return button + 24;
            return button;
        }

        private void CycleLights()
        {
            var lights = new List<SwscLight>();
            controller.SetLights(lights.ToArray());
            System.Threading.Thread.Sleep(200);
            lights.Add(SwscLight.Button1);
            controller.SetLights(lights.ToArray());
            System.Threading.Thread.Sleep(200);
            lights.Add(SwscLight.Button2);
            controller.SetLights(lights.ToArray());
            System.Threading.Thread.Sleep(200);
            lights.Add(SwscLight.Button3);
            controller.SetLights(lights.ToArray());
            System.Threading.Thread.Sleep(200);
            lights.Add(SwscLight.Button4);
            controller.SetLights(lights.ToArray());
            System.Threading.Thread.Sleep(200);
            lights.Add(SwscLight.Button5);
            controller.SetLights(lights.ToArray());
            System.Threading.Thread.Sleep(200);
            lights.Add(SwscLight.Button6);
            controller.SetLights(lights.ToArray());
            System.Threading.Thread.Sleep(200);
            lights.Add(SwscLight.Record);
            controller.SetLights(lights.ToArray());
            System.Threading.Thread.Sleep(200);
            lights = new List<SwscLight>();
            controller.SetLights(lights.ToArray());
        }
    }
}

