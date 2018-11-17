using System;
using System.Collections.Generic;
using System.Threading;
using EliteJoystick.Common;
using EliteJoystickService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vJoyMapping.Common;

namespace EliteJoystickServiceTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                List<Controller> Controllers = new List<Controller>();
                EliteVirtualJoysticks eliteVirtualJoysticks = new EliteVirtualJoysticks();
                vJoyMapper vJoyMapper = new vJoyMapper();
                EliteSharedState SharedState = new EliteSharedState { OrbitLines = true, HeadsUpDisplay = true };
                ArduinoCommunication.Arduino arduino = null;

                Settings settings = Settings.Load();
 
                var ffb2 = new vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Controller
                {
                    Arduino = arduino,
                    Name = "Force Feedback 2",
                    SharedState = SharedState,
                    Settings = settings,
                    VirtualJoysticks = eliteVirtualJoysticks
                };

                Controllers.Add(ffb2);

                var swgv = new vJoyMapping.Microsoft.Sidewinder.GameVoice.Controller
                {
                    Arduino = arduino,
                    Name = "Game Voice",
                    SharedState = SharedState,
                    Settings = settings,
                    VirtualJoysticks = eliteVirtualJoysticks
                };

                Controllers.Add(swgv);

                var swsc = new vJoyMapping.Microsoft.Sidewinder.StrategicCommander.Controller
                {
                    Arduino = arduino,
                    Name = "Strategic Commander",
                    SharedState = SharedState,
                    Settings = settings,
                    VirtualJoysticks = eliteVirtualJoysticks
                };

                Controllers.Add(swsc);

                var warthog = new vJoyMapping.Thrustmaster.Warthog.Throttle.Controller
                {
                    Arduino = arduino,
                    Name = "Warthog Throttle",
                    SharedState = SharedState,
                    Settings = settings,
                    VirtualJoysticks = eliteVirtualJoysticks
                };

                Controllers.Add(warthog);

                var pedals = new vJoyMapping.CHProducts.ProPedals.Controller
                {
                    Arduino = arduino,
                    Name = "Pro Pedals",
                    SharedState = SharedState,
                    Settings = settings,
                    VirtualJoysticks = eliteVirtualJoysticks
                };

                Controllers.Add(pedals);

                var bbi32 = new vJoyMapping.LeoBodnar.BBI32.Controller
                {
                    Arduino = arduino,
                    Name = "BBI32",
                    SharedState = SharedState,
                    Settings = settings,
                    VirtualJoysticks = eliteVirtualJoysticks
                };

                Controllers.Add(bbi32);

                // State Handlers
                var subscription = SharedState.GearChanged.Subscribe(
                    x => ffb2.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LandingGearToggle, 200));

                

                while (true)
                    Thread.Sleep(1000);
            }
            catch(Exception)
            {
                ;
            }
        }
    }
}
