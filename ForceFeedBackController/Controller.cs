using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ForceFeedBackController
{
    public class Controller : IDisposable
    {
        private ForceFeedbackSharpDx.ForceFeedbackController msffb2;

        List<IDisposable> Disposables { get; set; } = new List<IDisposable>();

        private List<SharpDX.DirectInput.Effect> centerSpringEffects;
        private List<SharpDX.DirectInput.Effect> damperEffects;
        private List<SharpDX.DirectInput.Effect> vibrationEffects;

        public ILogger Logger { get; set; }

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

        public void Initialize(EliteJoystickService.GameService gameService)
        {
            msffb2 = new ForceFeedbackSharpDx.ForceFeedbackController() { Logger = Logger };
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

            Disposables.AddRange(new List<IDisposable> {
                gameService.GameStatusObservable.Subscribe(x => ForceFeedBackController.Handlers.EffectHandler.Process(this, x, Logger)),
            });
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
            foreach (var effect in centerSpringEffects)
                effect.Dispose();
            foreach (var effect in damperEffects)
                effect.Dispose();
            foreach (var effect in vibrationEffects)
                effect.Dispose();

            foreach (var item in Disposables)
                item?.Dispose();
        }
    }
}
