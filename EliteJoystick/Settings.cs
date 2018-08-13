using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick
{
    /// <summary>
    /// Settings Object to save and load the settings from disk
    /// </summary>
    public class Settings
    {
        public Settings()
        {
            vJoyMapper = new vJoyMapper();
            StartUpApplications = new List<StartUpApplication>();
        }

        public vJoyMapper vJoyMapper { get; set; }
        public List<StartUpApplication> StartUpApplications { get; set; }
        public String ArduinoCommPort { get; set; }

        public void Save()
        {
            var pathName = String.Format(@"{0}\EliteJoystick\",
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            (new FileInfo(pathName)).Directory.Create();

            var fileName = String.Format(@"{0}{1}", pathName, "settings1.json");

            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, this);
                }
            }
        }

        public static Settings Load()
        {
            try
            {
                var pathName = String.Format(@"{0}\EliteJoystick\",
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

                var fileName = String.Format(@"{0}{1}", pathName, "settings1.json");

                using (StreamReader file = File.OpenText(fileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var settings = (Settings)serializer.Deserialize(file, typeof(Settings));

                    // Upgrade if necessary
                    if (String.IsNullOrEmpty(settings.ArduinoCommPort))
                        settings.ArduinoCommPort = "COM6";

                    return settings;
                }
            }
            catch(Exception)
            {
                return new Settings
                {
                    StartUpApplications = Initialize(),
                    vJoyMapper = new vJoyMapper { vJoyMap = vJoyMapper.Initialize() }
                };
            }
        }

        static List<StartUpApplication> Initialize()
        {
            return new List<StartUpApplication> {
                new StartUpApplication { Name = "VoiceMeeter", Command = @"C:\Program Files (x86)\VB\Voicemeeter\voicemeeterpro.exe" },
                new StartUpApplication { Name = "Elite Launcher", Command = @"H:\Steam\steamapps\common\Elite Dangerous\EDLaunch.exe" },
                new StartUpApplication { Name = "EDDI", Command = @"C:\Program Files (x86)\VoiceAttack\Apps\EDDI\EDDI.exe" },
                new StartUpApplication { Name = "ED Profiler", Command = @"C:\Users\Bob\Desktop\EDProfiler.appref-ms" },
            };
        }
    }
}
