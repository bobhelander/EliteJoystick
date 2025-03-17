using CommonCommunication;
using EliteJoystick.Common;
using EliteJoystick.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Reactive.Linq;
using vJoyMapping.Common;

namespace EliteDesktopApp.Services
{
    public class ControllersService : IDisposable
    {
        private readonly ILogger? Log;
        private readonly IDisposable virtualControllerUpdater;
        private List<Controller> Controllers { get; set; } = [];

        /// <summary>
        /// This service defines all of the current joysticks being connected.  Each of these controllers are created and injected
        /// into this service.  When this service goes out of scope the controllers are disposed and disconnected.
        /// </summary>
        /// <param name="forceFeedbackController"></param>
        /// <param name="ffb2"></param>
        /// <param name="swsc"></param>
        /// <param name="warthog"></param>
        /// <param name="proPedals"></param>
        /// <param name="bbi32"></param>
        /// <param name="ddjsb2"></param>
        /// <param name="eliteSharedState"></param>
        /// <param name="virtualJoysticks"></param>
        /// <param name="log"></param>
        public ControllersService(
            IForceFeedbackController forceFeedbackController,
            vJoyMapping.Microsoft.Sidewinder.ForceFeedback2.Controller ffb2,
            vJoyMapping.Microsoft.Sidewinder.StrategicCommander.Controller swsc,
            vJoyMapping.Thrustmaster.Warthog.Throttle.Controller warthog,
            vJoyMapping.CHProducts.ProPedals.Controller proPedals,
            vJoyMapping.LeoBodnar.BBI32.Controller bbi32,
            vJoyMapping.Pioneer.ddjsb2.Controller ddjsb2,
            EliteSharedState eliteSharedState,
            IVirtualJoysticks virtualJoysticks,
            ILogger<ControllersService> log)
        {
            // Holder to aggregate the joystick services
            Log = log;

            // Hold on to the created services
            Controllers = [ffb2, swsc, warthog, proPedals, bbi32, ddjsb2];

            // State Handlers
            var subscription = eliteSharedState.GearChanged.Subscribe(
                _ => ffb2.CallActivateButton(vJoyTypes.Virtual, MappedButtons.LandingGearToggle, 200));

            virtualControllerUpdater = StartUpdateData(virtualJoysticks, 300);

            // Tell the listening client that we are connected
            ClientActions.ClientInformationAction(this, "Controllers Ready");

            Log?.LogDebug("Controllers Ready");
        }

        public void Dispose()
        {
            virtualControllerUpdater?.Dispose();

            Controllers = [];

            Log?.LogDebug("Controllers Unloaded");
        }

        // Create an Observable that sends an update message every x milliseconds
        private static IDisposable StartUpdateData(IVirtualJoysticks eliteVirtualJoysticks, int updateFrequency = 100) =>
            Observable.Interval(TimeSpan.FromMilliseconds(updateFrequency)).Subscribe(_ => eliteVirtualJoysticks.UpdateAll());
    }
}
