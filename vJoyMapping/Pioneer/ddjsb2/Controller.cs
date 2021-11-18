using System;
using System.Collections.Generic;
using System.Text;
using DDJSB2;
using System.Linq;
using vJoyMapping.Pioneer.ddjsb2.Mapping;
using DDJSB2.Controls;
using EliteJoystick.Common;
using Microsoft.Extensions.Logging;
using EliteJoystick.Common.Interfaces;
using EliteJoystickService;

namespace vJoyMapping.Pioneer.ddjsb2
{
    public class Controller : Common.Controller
    {
        public override String Name { get; } = "DDJSB2";

        // Voicemeeter Settings
        private const string WindowsDefault = "Strip[5].Gain";
        private const string EliteDangerous = "Strip[7].Gain";
        private const string VoiceAttack = "Strip[1].Gain";
        private const string Spotify = "Strip[2].Gain";
        private const string Discord = "Strip[3].Gain";
        private const string Microphone = "Strip[4].Gain";

        private const string BassShaker = "Bus[1].Gain";  //A2 Output
        private const string Headphones = "Bus[2].Gain";  //A3 Output

        private const string SpotifyA1Patch = "Strip[2].A1";  // Spotify A1 Patch
        private const string SpotifyA2Patch = "Strip[2].A2";  // Spotify A2 Patch
        private const string SpotifyA3Patch = "Strip[2].A3";  // Spotify A3 Patch

        private const string EliteDangerousEqGain1 = "Strip[7].EQGain1";
        private const string EliteDangerousEqGain2 = "Strip[7].EQGain2";
        private const string EliteDangerousEqGain3 = "Strip[7].EQGain3";

        private IForceFeedbackController msffb2;

        private IDisposable skippingSubscription = null;
        private DateTime lastPause = DateTime.UtcNow;
        private bool playLed = false;

        public EliteJoystickService.GameService GameService { get; set; }

        public Controller(
            IKeyboard arduino,
            GameService gameService,
            EliteSharedState eliteSharedState,
            ISettings settings,
            IVirtualJoysticks virtualJoysticks,
            IForceFeedbackController ForceFeedBackController,
            IVoiceMeeterService voicemeeter,
            ILogger<Controller> log)
        {
            Arduino = arduino;
            SharedState = eliteSharedState;
            Settings = settings;
            VirtualJoysticks = virtualJoysticks;
            GameService = gameService;
            Logger = log;

            Initialize(ForceFeedBackController);

            Logger?.LogDebug($"Added {Name}");
        }


        public void Initialize(IForceFeedbackController msffb2)
        {
            this.msffb2 = msffb2;

            Disposables = new List<IDisposable>();

            var midiController = DDJSB2.PioneerDDJSB2.CreateInstance();
            Disposables.Add(midiController);

            MapControls(midiController);
            MapLights(midiController);
            midiController.Initialize();
        }

        public void MapControls(DDJSB2.PioneerDDJSB2 ddjsb2)
        {
            // Add in the mappings
            Disposables.AddRange(new List<IDisposable> {
                // Deck 1 Channel Fader:  VoiceAttack Gain
                (ddjsb2.ChannelControls[1].First(x => x.Name == "Channel Fader") as Slider)
                    .Subscribe(x => DdjSb2VoicemeeterHandler.SliderNotifiedVM(ddjsb2, VoiceAttack, x), ex => Logger.LogError($"Exception : {ex}")),

                // Deck 2 Channel Fader:  Spotify Gain
                (ddjsb2.ChannelControls[2].First(x => x.Name == "Channel Fader") as Slider)
                    .Subscribe(x => DdjSb2VoicemeeterHandler.SliderNotifiedVM(ddjsb2, Spotify, x), ex => Logger.LogError($"Exception : {ex}")),

                // Deck 2 Play/Pause:  Spotify Play/Pause/Launch
                (ddjsb2.ChannelControls[2].First(x => x.Name == "Play") as Button)
                    .Subscribe(x => DdjSb2SpotifyHandler.SpotifyPlayPause(ddjsb2, x), ex => Logger.LogError($"Exception : {ex}")),

                // Deck 2 Cue:  Spotify Previous Track
                (ddjsb2.ChannelControls[2].First(x => x.Name == "Cue") as Button)
                    .Subscribe(x => DdjSb2SpotifyHandler.SpotifyTrackChange(ddjsb2, DdjSb2SpotifyHandler.MediaPrevious, Leds.Deck.Deck2, Leds.CueLed, x), ex => Logger.LogError($"Exception : {ex}")),

                // Deck 2 Sync:  Spotify Next Track
                (ddjsb2.ChannelControls[2].First(x => x.Name == "Sync") as Button)
                    .Subscribe(x => DdjSb2SpotifyHandler.SpotifyTrackChange(ddjsb2, DdjSb2SpotifyHandler.MediaNext, Leds.Deck.Deck2, Leds.SyncLed, x), ex => Logger.LogError($"Exception : {ex}")),

                // Middle Headphones Mix: Headphones Gain
                (ddjsb2.ChannelControls[7].First(x => x.Name == "Headphones Mix") as Dial)
                    .Subscribe(x => DdjSb2VoicemeeterHandler.DialWithDetentNotifiedVM(ddjsb2, Headphones, x), ex => Logger.LogError($"Exception : {ex}")),

                // Deck 1 Tempo Slider: Windows Gain
                (ddjsb2.ChannelControls[1].First(x => x.Name == "Tempo") as Slider)
                    .Subscribe(x => DdjSb2VoicemeeterHandler.SliderWithDetentNotifiedVM(ddjsb2, WindowsDefault, x), ex => Logger.LogError($"Exception : {ex}")),

                // Deck 2 Tempo Slider: EliteDangerous Gain
                (ddjsb2.ChannelControls[2].First(x => x.Name == "Tempo") as Slider)
                    .Subscribe(x => DdjSb2VoicemeeterHandler.SliderWithDetentNotifiedVM(ddjsb2, EliteDangerous, x), ex => Logger.LogError($"Exception : {ex}")),

                // Deck 2 Low Dial: EliteDangerous EqGain1
                (ddjsb2.ChannelControls[2].First(x => x.Name == "Low") as Dial)
                    .Subscribe(x => DdjSb2VoicemeeterHandler.EqDialNotifiedVM(ddjsb2, EliteDangerousEqGain1, x), ex => Logger.LogError($"Exception : {ex}")),
                // Deck 2 Mid Dial: EliteDangerous EqGain2
                (ddjsb2.ChannelControls[2].First(x => x.Name == "Mid") as Dial)
                    .Subscribe(x => DdjSb2VoicemeeterHandler.EqDialNotifiedVM(ddjsb2, EliteDangerousEqGain2, x), ex => Logger.LogError($"Exception : {ex}")),
                // Deck 2 Hi Dial: EliteDangerous EqGain3
                (ddjsb2.ChannelControls[2].First(x => x.Name == "Hi") as Dial)
                    .Subscribe(x => DdjSb2VoicemeeterHandler.EqDialNotifiedVM(ddjsb2, EliteDangerousEqGain3, x), ex => Logger.LogError($"Exception : {ex}")),

                //  Deck 2 1/2X (Hot Que): Focus Elite Dangerous
                (ddjsb2.ChannelControls[9].First(x => x.Name == "1/2X (Hot Que)") as Button)
                    .Subscribe(x => FocusWindowHandler(ddjsb2, "EliteDangerous64", Leds.Deck.Deck1, x), ex => Logger.LogError($"Exception : {ex}")),

                // Deck 2 Headphone Cue: A1 Patch (Main Speakers) Enable/Disable
                (ddjsb2.ChannelControls[2].First(x => x.Name == "Headphone Cue") as Button)
                    .Subscribe(x => DdjSb2VoicemeeterHandler.PatchControl(SpotifyA1Patch, x), ex => Logger.LogError($"Exception : {ex}")),

                // Deck 2 In (Hot Que): Spotify Skip Commerical (Headphones - A3)
                (ddjsb2.ChannelControls[9].First(x => x.Name == "In (Hot Que)") as Button)
                    .Subscribe(x => DdjSb2SpotifyHandler.SpotifySkipCommerical(ddjsb2, SpotifyA3Patch, skippingSubscription, Leds.Deck.Deck2, Leds.PadGroup.hotCue, Leds.InLed, x, lastPause),
                    ex => Logger.LogError($"Exception : {ex}")),

                // Deck 2 Out (Hot Que): Voicemeeter Restart Audio
                (ddjsb2.ChannelControls[9].First(x => x.Name == "Out (Hot Que)") as Button)
                    .Subscribe(x => DdjSb2VoicemeeterHandler.RestartAudio(ddjsb2, Leds.Deck.Deck2, Leds.PadGroup.hotCue, Leds.OutLed, x),
                    ex => Logger.LogError($"Exception : {ex}")),

                //// Deck 2 Exit (Hot Que): Explorer Log File  Note: Moved to keyboard media key
                //(ddjsb2.ChannelControls[9].First(x => x.Name == "Exit (Hot Que)") as Button)
                //    .Subscribe(x => DdjSb2UtilsHandler.ExplorerLog(ddjsb2, Leds.Deck.Deck2, Leds.PadGroup.hotCue, Leds.ExitLed, x),
                //    ex => Logger.LogError($"Exception : {ex}")),

                // Deck 1 In (Hot Que): Oculus Automatic Space Warp Off
                (ddjsb2.ChannelControls[8].First(x => x.Name == "In (Hot Que)") as Button)
                    .Subscribe(x => DdjSb2UtilsHandler.OculusASWOff(ddjsb2, Leds.Deck.Deck1, Leds.PadGroup.hotCue, Leds.InLed, x),
                    ex => Logger.LogError($"Exception : {ex}")),

                // Deck 1 Out (Hot Que): Feedback Center Spring
                (ddjsb2.ChannelControls[8].First(x => x.Name == "Out (Hot Que)") as Button)
                    .Subscribe(x => HandleForceEffects(ddjsb2, Leds.Deck.Deck1, Leds.PadGroup.hotCue, Leds.OutLed, msffb2, prop => prop.CenterSpring, x),
                    ex => Logger.LogError($"Exception : {ex}")),

                // Deck 1 Exit (Hot Que): Feedback Damper
                (ddjsb2.ChannelControls[8].First(x => x.Name == "Exit (Hot Que)") as Button)
                    .Subscribe(x => HandleForceEffects(ddjsb2, Leds.Deck.Deck1, Leds.PadGroup.hotCue, Leds.ExitLed, msffb2, prop => prop.Damper, x),
                    ex => Logger.LogError($"Exception : {ex}")),

                // Deck 1 1/2X (Hot Que): Stop All Effects
                (ddjsb2.ChannelControls[8].First(x => x.Name == "1/2X (Hot Que)") as Button)
                    .Subscribe(x => StopAllEffects(ddjsb2, Leds.Deck.Deck1, Leds.PadGroup.hotCue, Leds.Pad2xLed, x),
                    ex => Logger.LogError($"Exception : {ex}")),

                // Deck 1 Touch Jog Wheel: Shut up Voice Attack
                (ddjsb2.ChannelControls[1].First(x => x.Name == "Jog Dial") as DDJSB2.Controls.Encoder)
                    .Subscribe(x => DdjSb2UtilsHandler.ShutUpVoiceAttack(this, x),
                    ex => Logger.LogError($"Exception : {ex}")),

                // Deck 2 Jog Wheel: Target subsystem
                (ddjsb2.ChannelControls[2].First(x => x.Name == "Jog Dial") as DDJSB2.Controls.Encoder)
                    .Subscribe(x => DdjSb2UtilsHandler.TargetSubsystem(this, x),
                    ex => Logger.LogError($"Exception : {ex}")),

                // Center Browse: Menu Selection
                (ddjsb2.ChannelControls[7].First(x => x.Name == "Browse") as DDJSB2.Controls.Encoder)
                    .Subscribe(x => DdjSb2UtilsHandler.MenuSelection(this, x),
                    ex => Logger.LogError($"Exception : {ex}")),

                // Center Left Load: Panel Selection
                (ddjsb2.ChannelControls[7].First(x => x.Name == "Left Load") as DDJSB2.Controls.Button)
                    .Subscribe(x => DdjSb2UtilsHandler.PanelSelection(this, x, MappedButtons.ForceFeedback2Button3),
                    ex => Logger.LogError($"Exception : {ex}")),

                // Center Right Load: Panel Selection
                (ddjsb2.ChannelControls[7].First(x => x.Name == "Right Load") as DDJSB2.Controls.Button)
                    .Subscribe(x => DdjSb2UtilsHandler.PanelSelection(this, x, MappedButtons.ForceFeedback2Button4),
                    ex => Logger.LogError($"Exception : {ex}")),
            });
        }

        public void MapLights(DDJSB2.PioneerDDJSB2 ddjsb2)
        {
            var channels = new Voicemeeter.Levels.Channel[] {
                    new Voicemeeter.Levels.Channel {    // Check if Spotify is outputting audio
                        LevelType = Voicemeeter.LevelType.PreFaderInput,
                        ChannelNumber = 4  // Strip #3 - Left Audio
                    },
                    new Voicemeeter.Levels.Channel {    // Spotify Audio Level
                        LevelType = Voicemeeter.LevelType.PostFaderInput,
                        ChannelNumber = 4  // Strip #3 - Left Audio
                    },
                    new Voicemeeter.Levels.Channel {    // VoiceAttack Audio Level
                        LevelType = Voicemeeter.LevelType.PostFaderInput,
                        ChannelNumber = 2  // Strip #2 - Left Audio
                    }
                };

            // Subscribe to the levels from Voicemeeter
            var levels = new Voicemeeter.Levels(channels, 20);  // Update every 20 miliseconds
            Disposables.Add(levels.Subscribe(x => LevelsUpdate(ddjsb2, x)));

            // Watch for changes
            var parameters = new Voicemeeter.Parameters();
            Disposables.Add(parameters.Subscribe(x => ParametersUpdate(ddjsb2, x)));

            Disposables.Add(GameService.GameStatusObservable.Subscribe(x => Process(ddjsb2, x)));

            // Init 
            //ddjsb2.LedControl(Leds.Deck.Deck1, Leds.PlayLed, false, GameService.GameStatusObservable.EliteAPI.Status.IsRunning);
        }

        private void LevelsUpdate(PioneerDDJSB2 ddjsb2, float[] levels)
        {
            // Voicemeeter    DDJSB2
            // 0 - 1.1        0 - 127

            const float divisor = 127.0f / 1.1f;

            float deck1Value = levels[2] * divisor;
            float deck2Value = levels[1] * divisor;

            ddjsb2.LevelLedControl(Leds.Deck.Deck1, Leds.Level, (byte)deck1Value);
            ddjsb2.LevelLedControl(Leds.Deck.Deck2, Leds.Level, (byte)deck2Value);

            // Is something playing on the Spotify channel?
            if (levels[0] > 0.005f && playLed == false)
            {
                playLed = true;
                ddjsb2.LedControl(Leds.Deck.Deck2, Leds.PlayLed, false, true);
            }
            if (levels[0] < 0.005f && playLed)
            {
                playLed = false;
                ddjsb2.LedControl(Leds.Deck.Deck2, Leds.PlayLed, false, false);
                lastPause = DateTime.UtcNow;
            }

            //ddjsb2.LedControl(Leds.Deck.Deck1, Leds.PlayLed, false, GameService.GameStatusObservable.EliteAPI.Status.IsRunning);
        }

        private void ParametersUpdate(PioneerDDJSB2 ddjsb2, int changeEvent)
        {
            // Deck 2 Headphone Cue hooked to SpotifyA1Patch
            ddjsb2.LedControl(
                Leds.Deck.Deck2,
                Leds.HeadphoneCueLed,
                false,
                VoiceMeeter.Remote.GetParameter(SpotifyA1Patch) > 0);
        }

        private void FocusWindowHandler(PioneerDDJSB2 ddjsb2, string process, Leds.Deck deck, IState state)
        {
            Logger.LogDebug($"FocusWindowHandler: {state.Control.Name}");

            var button = state.Control as Button;
            var on = button.NoteValue > 0;

            // Light the LED when pressed
            ddjsb2.LedControl(deck, Leds.CueLed, false, on);

            if (on)
            {
                Utils.FocusWindow(process);
            }
        }

        private void Process(DDJSB2.PioneerDDJSB2 ddjsb2, EliteAPI.Events.IEvent statusEvent)
        {
            Logger.LogDebug($"{statusEvent.GetType()}");

            ddjsb2.LedControl(Leds.Deck.Deck1, Leds.PlayLed, false, GameService.GameStatusObservable.EliteAPI.Status.IsRunning);
        }

        // Use a LINQ Expression to send the boolean property to set
        private void HandleForceEffects<T>(
            PioneerDDJSB2 ddjsb2,
            Leds.Deck deck,
            Leds.PadGroup group,
            Led led,
            T targetObject,
            System.Linq.Expressions.Expression<Func<T, bool>> targetExpr,
            IState state)
        {
            var button = state.Control as Button;
            var on = button.NoteValue > 0;

            // Get the property
            var expr = (System.Linq.Expressions.MemberExpression)targetExpr.Body;
            var prop = (System.Reflection.PropertyInfo)expr.Member;

            // Get the value of the property
            bool effectState = (bool)prop.GetValue(targetObject);

            if (on)
                prop.SetValue(targetObject, !effectState);

            // Light the LED when on
            ddjsb2.PadLedControl(deck, group, led, false, effectState || on);
        }

        private void StopAllEffects(PioneerDDJSB2 ddjsb2, Leds.Deck deck, Leds.PadGroup group, Led led, IState state)
        {
            var button = state.Control as Button;
            var on = button.NoteValue > 0;

            // Light the LED when on
            ddjsb2.PadLedControl(deck, group, led, false, on);

            if (on)
                msffb2.StopAllEffects();
        }
    }
}
