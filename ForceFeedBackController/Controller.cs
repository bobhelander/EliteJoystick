using EliteJoystick.Common.Interfaces;
using ForceFeedBackController.Handlers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForceFeedBackController
{
    public class Controller : IForceFeedbackController
    {
        private readonly EliteAPI.Abstractions.IEliteDangerousApi eliteDangerousApi;
        private readonly EliteAPI.Status.Ship.Abstractions.IShip ship;
        private ForceFeedbackSharpDx.ForceFeedbackController msffb2;

        private List<IDisposable> Disposables { get; } = new List<IDisposable>();

        private List<SharpDX.DirectInput.Effect> centerSpringEffects;
        private List<SharpDX.DirectInput.Effect> damperEffects;
        private List<SharpDX.DirectInput.Effect> vibrationEffects;

        public ILogger Logger { get; set; }

        public Controller(
            EliteAPI.Abstractions.IEliteDangerousApi eliteDangerousApi,
            EliteAPI.Status.Ship.Abstractions.IShip ship,
            ILogger<Controller> log)
        {
            this.eliteDangerousApi = eliteDangerousApi;
            this.ship = ship;
            Logger = log;

            Initialize();
        }

        public Controller() { }

        public bool CenterSpring
        {
            get => EffectPlaying(centerSpringEffects);
            set => SetEffect(centerSpringEffects, value);
        }

        public bool Damper
        {
            get => EffectPlaying(damperEffects);
            set => SetEffect(damperEffects, value);
        }

        public bool Vibration
        {
            get => EffectPlaying(vibrationEffects);
            set => SetEffect(vibrationEffects, value);
        }

        public void StopAllEffects() => msffb2.StopAllEffects();
        public void PlayFileEffect(string fileName, int duration) => msffb2.PlayFileEffect(fileName, duration);

        public void Initialize()
        {
            msffb2 = new ForceFeedbackSharpDx.ForceFeedbackController() { Logger = Logger };

            Disposables.Add(msffb2);

            msffb2.Initialize(
                "001b045e-0000-0000-0000-504944564944",
                "SideWinder Force Feedback 2 Joystick",
                @".\Forces",
                false,
                10000);

            // Load the continuous effects
            centerSpringEffects = msffb2.GetEffectFromFile("CenterSpringXY.ffe");
            damperEffects = msffb2.GetEffectFromFile("Damper.ffe");
            vibrationEffects = msffb2.GetEffectFromFile("Vibrate.ffe");

            eliteDangerousApi.Events.AllEvent += (sender, eventArgs) =>
                EffectHandler.Process(this, eventArgs.ToString(), Logger);

            ship.Docked.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.Docked:{eventArgs}", Logger);
            ship.Landed.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.Landed:{eventArgs}", Logger);
            ship.Gear.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.Gear:{eventArgs}", Logger);
            ship.Shields.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.Shields:{eventArgs}", Logger);
            ship.Supercruise.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.Supercruise:{eventArgs}", Logger);
            ship.FlightAssist.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.FlightAssist:{eventArgs}", Logger);
            ship.Hardpoints.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.Hardpoints:{eventArgs}", Logger);
            ship.Winging.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.Winging:{eventArgs}", Logger);
            ship.Lights.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.Lights:{eventArgs}", Logger);
            ship.CargoScoop.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.CargoScoop:{eventArgs}", Logger);
            ship.SilentRunning.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.SilentRunning:{eventArgs}", Logger);
            ship.Scooping.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.Scooping:{eventArgs}", Logger);
            ship.SrvHandbreak.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.SrvHandbreak:{eventArgs}", Logger);
            ship.SrvTurrent.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.SrvTurrent:{eventArgs}", Logger);
            ship.SrvNearShip.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.SrvNearShip:{eventArgs}", Logger);
            ship.SrvDriveAssist.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.SrvDriveAssist:{eventArgs}", Logger);
            ship.MassLocked.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.MassLocked:{eventArgs}", Logger);
            ship.FsdCharging.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.FsdCharging:{eventArgs}", Logger);
            ship.FsdCooldown.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.FsdCooldown:{eventArgs}", Logger);
            ship.LowFuel.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.LowFuel:{eventArgs}", Logger);
            ship.Overheating.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.Overheating:{eventArgs}", Logger);
            ship.HasLatLong.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.HasLatLong:{eventArgs}", Logger);
            ship.InDanger.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.InDanger:{eventArgs}", Logger);
            ship.InInterdiction.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.InInterdiction:{eventArgs}", Logger);
            ship.InMothership.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.InMothership:{eventArgs}", Logger);
            ship.InFighter.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.InFighter:{eventArgs}", Logger);
            ship.InSrv.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.InSrv:{eventArgs}", Logger);
            ship.AnalysisMode.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.AnalysisMode:{eventArgs}", Logger);
            ship.NightVision.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.NightVision:{eventArgs}", Logger);
            ship.AltitudeFromAverageRadius.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.AltitudeFromAverageRadius:{eventArgs}", Logger);
            ship.FsdJump.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.FsdJump:{eventArgs}", Logger);
            ship.SrvHighBeam.OnChange += (obj, eventArgs) => EffectHandler.Process(this, $"Status.SrvHighBeam:{eventArgs}", Logger);
        }

        private void SetEffect(List<SharpDX.DirectInput.Effect> effects, bool on)
        {
            var playing = EffectPlaying(effects);

            if (playing != on)
            {
                if (on) { msffb2.PlayEffects(effects); }
                else { msffb2.StopEffects(effects, false); }
            }
        }

        private static bool EffectPlaying(List<SharpDX.DirectInput.Effect> effects)
        {
            try
            {
                return effects[0].Status == SharpDX.DirectInput.EffectStatus.Playing;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Dispose()
        {
            if (Disposables != null)
            {
                foreach (var item in Disposables)
                    item?.Dispose();
            }
            if (centerSpringEffects != null)
            {
                foreach (var effect in centerSpringEffects)
                    effect.Dispose();
            }

            if (damperEffects != null)
            {
                foreach (var effect in damperEffects)
                    effect.Dispose();
            }

            if (vibrationEffects != null)
            {
                foreach (var effect in vibrationEffects)
                    effect.Dispose();
            }
        }
    }
}
