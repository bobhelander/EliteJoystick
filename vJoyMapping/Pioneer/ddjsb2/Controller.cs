using System;
using System.Collections.Generic;
using System.Text;
using DDJSB2;
using System.Linq;
using vJoyMapping.Pioneer.ddjsb2.Mapping;
using DDJSB2.Controls;

namespace vJoyMapping.Pioneer.ddjsb2
{
    public class Controller : Common.Controller
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Voicemeeter Settings
        private const string WindowsDefault = "Strip[0].Gain";
        private const string EliteDangerous = "Strip[7].Gain";
        private const string VoiceAttack = "Strip[1].Gain";
        private const string Spotify = "Strip[2].Gain";
        private const string Discord = "Strip[3].Gain";
        private const string Microphone = "Strip[4].Gain";

        private const string BassShaker = "Bus[1].Gain";  //A2 Output
        private const string Headphones = "Bus[2].Gain";  //A3 Output

        private const string SpotifyA1Patch = "Strip[2].A1";  // Spotify A1 Patch


        private IDisposable skippingSubscription = null;
        private DateTime lastPause = DateTime.UtcNow;
        private bool playLed = false;

        public EliteJoystickService.GameService GameService { get; set; }

        public void Initialize()
        {
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
                    .Subscribe(x => DdjSb2VoicemeeterHandler.SliderNotifiedVM(ddjsb2, VoiceAttack, x), ex => log.Error($"Exception : {ex}")),

                // Deck 2 Channel Fader:  Spotify Gain
                (ddjsb2.ChannelControls[2].First(x => x.Name == "Channel Fader") as Slider)
                    .Subscribe(x => DdjSb2VoicemeeterHandler.SliderNotifiedVM(ddjsb2, Spotify, x), ex => log.Error($"Exception : {ex}")),

                // Deck 2 Play/Pause:  Spotify Play/Pause/Launch
                (ddjsb2.ChannelControls[2].First(x => x.Name == "Play") as Button)
                    .Subscribe(x => DdjSb2SpotifyHandler.SpotifyPlayPause(ddjsb2, x), ex => log.Error($"Exception : {ex}")),

                // Deck 2 Cue:  Spotify Previous Track
                (ddjsb2.ChannelControls[2].First(x => x.Name == "Cue") as Button)
                    .Subscribe(x => DdjSb2SpotifyHandler.SpotifyTrackChange(ddjsb2, DdjSb2SpotifyHandler.MediaPrevious, Leds.Deck.Deck2, Leds.CueLed, x), ex => log.Error($"Exception : {ex}")),

                // Deck 2 Sync:  Spotify Next Track
                (ddjsb2.ChannelControls[2].First(x => x.Name == "Sync") as Button)
                    .Subscribe(x => DdjSb2SpotifyHandler.SpotifyTrackChange(ddjsb2, DdjSb2SpotifyHandler.MediaNext, Leds.Deck.Deck2, Leds.SyncLed, x), ex => log.Error($"Exception : {ex}")),

                // Middle Headphones Mix: Headphones Gain
                (ddjsb2.ChannelControls[7].First(x => x.Name == "Headphones Mix") as Dial)
                    .Subscribe(x => DdjSb2VoicemeeterHandler.DialWithDetentNotifiedVM(ddjsb2, Headphones, x), ex => log.Error($"Exception : {ex}")),

                // Deck 1 Tempo Slider: Windows Gain
                (ddjsb2.ChannelControls[1].First(x => x.Name == "Tempo") as Slider)
                    .Subscribe(x => DdjSb2VoicemeeterHandler.SliderWithDetentNotifiedVM(ddjsb2, WindowsDefault, x), ex => log.Error($"Exception : {ex}")),

                // Deck 2 Tempo Slider: EliteDangerous Gain
                (ddjsb2.ChannelControls[2].First(x => x.Name == "Tempo") as Slider)
                    .Subscribe(x => DdjSb2VoicemeeterHandler.SliderWithDetentNotifiedVM(ddjsb2, EliteDangerous, x), ex => log.Error($"Exception : {ex}")),

                //  Deck 2 1/2X (Hot Que): Focus Elite Dangerous
//                (ddjsb2.ChannelControls[1].First(x => x.Name == "1/2X (Hot Que)") as Button)
//                    .Subscribe(x => FocusWindowHandler(ddjsb2, "EliteDangerous64", Leds.Deck.Deck1, x), ex => log.Error($"Exception : {ex}")),

                // Deck 2 Headphone Cue: A1 Patch (Main Speakers) Enable/Disable
                (ddjsb2.ChannelControls[2].First(x => x.Name == "Headphone Cue") as Button)
                    .Subscribe(x => DdjSb2VoicemeeterHandler.PatchControl(SpotifyA1Patch, x), ex => log.Error($"Exception : {ex}")),

                // Deck 2 In (Hot Que): Spotify Skip Commerical
                (ddjsb2.ChannelControls[9].First(x => x.Name == "In (Hot Que)") as Button)
                    .Subscribe(x => DdjSb2SpotifyHandler.SpotifySkipCommerical(ddjsb2, SpotifyA1Patch, skippingSubscription, Leds.Deck.Deck2, Leds.PadGroup.hotCue, Leds.InLed, x, lastPause), 
                    ex => log.Error($"Exception : {ex}")),

                // Deck 2 Out (Hot Que): Voicemeeter Restart Audio
                (ddjsb2.ChannelControls[9].First(x => x.Name == "Out (Hot Que)") as Button)
                    .Subscribe(x => DdjSb2VoicemeeterHandler.RestartAudio(ddjsb2, Leds.Deck.Deck2, Leds.PadGroup.hotCue, Leds.OutLed, x),
                    ex => log.Error($"Exception : {ex}")),
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
        private void Process(DDJSB2.PioneerDDJSB2 ddjsb2, EliteAPI.Events.IEvent statusEvent)
        {
            log.Debug($"{statusEvent.GetType().ToString()}");

            ddjsb2.LedControl(Leds.Deck.Deck1, Leds.PlayLed, false, GameService.GameStatusObservable.EliteAPI.Status.IsRunning);
        }
    }
}
