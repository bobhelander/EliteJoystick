using Newtonsoft.Json;

namespace EliteJoystick.Common.EliteGame
{
    public class GameStatus
    {
        // http://hosting.zaonce.net/community/journal/v15/Journal_Manual_v15.pdf

        // { "timestamp": "2017-12-07T10:31:37Z", "event": "Status", "Flags": 16842765, "Pips": [2,8,2], "FireGroup": 0, "Fuel": { "FuelMain": 15.146626, "FuelReservoir": 0.382796 }, "GuiFocus": 5 }
        // { "timestamp":"2019-07-09T04:46:50Z", "event":"Status", "Flags":69730568, "Pips":[4,0,8], "FireGroup":0, "GuiFocus":0, "Fuel":{ "FuelMain":0.000000, "FuelReservoir":0.390963 }, "Cargo":1.000000, "LegalState":"Clean", "Latitude":-34.827103, "Longitude":132.886551, "Heading":260, "Altitude":0, "BodyName":"Synuefe NL-N c23-4 B 3", "PlanetRadius":2051975.625000 }

        [JsonProperty(PropertyName = "timestamp")]
        public string Timestamp { get; set; }
        [JsonProperty(PropertyName = "event")]
        public string EventType { get; set; }
        public int Flags { get; set; }
        public int[] Pips { get; set; }
        public int FireGroup { get; set; }
        public Fuel Fuel { get; set; }
        public int GuiFocus { get; set; }
        public double Cargo { get; set; }
        public string LegalState { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Heading { get; set; }
        public int Altitude { get; set; }
        public string BodyName { get; set; }
        public double PlanetRadius { get; set; }
    }
}
