using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vJoyMapping.Common;

namespace EliteJoystickService
{
    /// <summary>
    /// Settings Object to save and load the settings from disk
    /// </summary>
    public class Settings
    {
        public Settings()
        {
            vJoyMapper = new vJoyMapper();
        }

        public vJoyMapper vJoyMapper { get; set; }
        public String ArduinoCommPort { get; set; }

        public void Save()
        {
            var pathName = String.Format(@"{0}\EliteJoystick\",
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));

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
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));

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
                    vJoyMapper = new vJoyMapper { vJoyMap = vJoyMapper.Initialize() }
                };
            }
        }
    }
}
