using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace EliteJoystick
{
    /// <summary>
    /// Allow dynamic changing of the virtual joystick output identifiers 
    /// </summary>
    public class vJoyMapper : INotifyPropertyChanged
    {
        public class vJoyMapItem
        {
            public vJoyMapper vJoyMapper { get; set; }
            public String Key { get; set; }
            public List<String> Values { get { return new List<string> { "1", "2", "3", "4", "5", "6" }; } }
            public String Selected
            {
                get { return vJoyMapper.vJoyMap[Key].ToString(); }
                set { vJoyMapper.vJoyMap[Key] = UInt32.Parse(value); }
            }
        }

        public vJoyMapper()
        {
            vJoyMap = Initialize();

            try
            {
                Load();
            }
            catch(Exception)
            {
                ;
            }
        }

        static public Dictionary<string, uint> Initialize()
        {
            return new Dictionary<string, uint> {
                {vJoyTypes.StickAndPedals, 1 },
                {vJoyTypes.Throttle, 2 },
                {vJoyTypes.Commander, 3 },
                {vJoyTypes.Voice, 4 },
                {vJoyTypes.Virtual, 5 },
                {vJoyTypes.Buttons, 6 },
            };
        }

        [JsonIgnore]
        public List<vJoyMapItem> Values
        {
            get
            {
                return new List<vJoyMapItem>
                {
                    new vJoyMapItem { vJoyMapper = this, Key = vJoyTypes.StickAndPedals },
                    new vJoyMapItem { vJoyMapper = this, Key = vJoyTypes.Throttle },
                    new vJoyMapItem { vJoyMapper = this, Key = vJoyTypes.Commander },
                    new vJoyMapItem { vJoyMapper = this, Key = vJoyTypes.Voice },
                    new vJoyMapItem { vJoyMapper = this, Key = vJoyTypes.Virtual },
                    new vJoyMapItem { vJoyMapper = this, Key = vJoyTypes.Buttons }
                };
            }
        }

        public Dictionary<string, uint> vJoyMap { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public uint GetJoystickId(string vJoyType)
        {
            return vJoyMap[vJoyType];
        }

        public void Save()
        {
            var pathName = String.Format(@"{0}\EliteJoystick\",
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            (new FileInfo(pathName)).Directory.Create();

            var fileName = String.Format(@"{0}{1}",
                pathName,
                "settings.json");

            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, vJoyMap);
                }
            }
        }

        public void Load()
        {
            var pathName = String.Format(@"{0}\EliteJoystick\",
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            var fileName = String.Format(@"{0}{1}",
                pathName,
                "settings.json");

            using (StreamReader file = File.OpenText(fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                vJoyMap = (Dictionary<string, uint>)serializer.Deserialize(file, typeof(Dictionary<string, uint>));
            }

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(null));
        }
    }
}
