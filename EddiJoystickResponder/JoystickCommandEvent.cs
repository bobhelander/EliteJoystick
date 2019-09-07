using EddiEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EddiJoystickResponder
{
    public class JoystickCommandEvent : Event
    {
        public const string NAME = "Joystick Command";
        public const string DESCRIPTION = "Triggered when a message for the joystick is requested";
        //public const string SAMPLE = "{\"timestamp\":\"2016-06-10T14:32:03Z\",\"event\":\"ClearSaveGame\",\"Name\":\"HRC1\"}";
        public static Dictionary<string, string> VARIABLES = new Dictionary<string, string>();

        static JoystickCommandEvent()
        {
            VARIABLES.Add("name", "A command for the joystick has been requested");
        }

        [JsonProperty("command")]
        public string command { get; private set; }

        public JoystickCommandEvent(DateTime timestamp, string command) : base(timestamp, NAME)
        {
            this.command = command;
        }
    }
}
