using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Common
{
    /// <summary>
    /// Settings Object to save and load the settings from disk
    /// </summary>
    public class Settings
    {
        public vJoyMapper vJoyMapper { get; set; } = new vJoyMapper();
        public String ArduinoCommPort { get; set; } = "COM6";

        public void Save()
        {
            var pathName = String.Format(@"{0}\EliteJoystick\",
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));

            (new FileInfo(pathName)).Directory.Create();

            var fileName = String.Format("{0}{1}", pathName, "settings.json");

            File.WriteAllText(fileName, JsonConvert.SerializeObject(this));
        }

        public static Settings Load()
        {
            try
            {
                var pathName = String.Format(@"{0}\EliteJoystick\",
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));

                var fileName = String.Format("{0}{1}", pathName, "settings.json");

                return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(fileName));
            }
            catch (Exception)
            {
                return new Settings();
            }
        }
    }
}
