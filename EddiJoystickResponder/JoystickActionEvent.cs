using EddiEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EddiJoystickResponder
{
    public class JoystickActionEvent : Event
    {
        public const string NAME = "Joystick EDDI Action";
        public const string DESCRIPTION = "Triggered when an EDDI action for the joystick is requested";
        //public const string SAMPLE = "{\"timestamp\":\"2016-06-10T14:32:03Z\",\"event\":\"ClearSaveGame\",\"Name\":\"HRC1\"}";
        public static Dictionary<string, string> VARIABLES = new Dictionary<string, string>();

        static JoystickActionEvent()
        {
            VARIABLES.Add("name", "An EDDI action for the joystick has been requested");
        }

        [JsonProperty("command")]
        public string command { get; private set; }

        [JsonProperty("var1")]
        public string var1 { get; set; }

        [JsonProperty("var2")]
        public string var2 { get; set; }

        [JsonProperty("var3")]
        public string var3 { get; set; }

        public JoystickActionEvent(DateTime timestamp, string command) : base(timestamp, NAME)
        {
            this.command = command;
        }
    }
}
