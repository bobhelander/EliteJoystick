using System;
using System.Threading;
using Controllers;
using EliteJoystickService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                EliteControllers eliteControllers = new EliteControllers();
                EliteVirtualJoysticks eliteVirtualJoysticks = new EliteVirtualJoysticks();
                vJoyMapper vJoyMapper = new vJoyMapper();
                EliteSharedState sharedState = new EliteSharedState { OrbitLines = true, HeadsUpDisplay = true };
                ArduinoCommunication.Arduino arduino = null;

                Settings settings = Settings.Load();

                //for (uint joyId = 1; joyId <= 6; joyId++)
                //{
                //    eliteVirtualJoysticks.Controllers.Add(new EliteVirtualJoystick
                //    {
                //        Joystick = eliteVirtualJoysticks.Joystick,
                //        JoystickId = joyId,
                //    });
                //}

                eliteVirtualJoysticks.Initialize();

                eliteControllers.Controllers.Add(
                        Controllers.Sidewinder.ForceFeedback2.Swff2Controller.Create(
                            sharedState,
                            eliteVirtualJoysticks.Joystick,
                            eliteVirtualJoysticks,
                            vJoyMapper,
                            arduino));

                eliteControllers.Controllers.Add(
                    Controllers.Thrustmaster.Warthog.TmThrottleController.Create(
                        sharedState,
                        eliteVirtualJoysticks.Joystick,
                        eliteVirtualJoysticks,
                        vJoyMapper,
                        arduino));

                eliteControllers.Controllers.Add(
                    Controllers.ChProducts.ChPedalsController.Create(
                        sharedState,
                        eliteVirtualJoysticks.Joystick,
                        eliteVirtualJoysticks,
                        vJoyMapper));

                eliteControllers.Controllers.Add(
                    Controllers.Sidewinder.Commander.ScController.Create(
                        sharedState,
                        eliteVirtualJoysticks.Joystick,
                        eliteVirtualJoysticks,
                        vJoyMapper));

                eliteControllers.Controllers.Add(
                    Controllers.Sidewinder.GameVoice.SwGvController.Create(
                        sharedState,
                        eliteVirtualJoysticks.Joystick,
                        eliteVirtualJoysticks,
                        vJoyMapper));

                eliteControllers.Controllers.Add(
                    Controllers.Other.BBI32.ButtonBoxController.Create(
                        sharedState,
                        eliteVirtualJoysticks.Joystick,
                        eliteVirtualJoysticks,
                        vJoyMapper,
                        arduino));

                eliteControllers.Initialize();

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
