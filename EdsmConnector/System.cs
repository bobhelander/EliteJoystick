using Newtonsoft.Json;
using System;

namespace EdsmConnector
{
    public class System
    {
        public int? id { get; set; }
        public long? id64 { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public int? bodyCount { get; set; }
        public Body[] bodies { get; set; }
    }

    public class Body
    {
        public int? id { get; set; }
        public long? id64 { get; set; }
        public int? bodyId { get; set; }
        public string name { get; set; }
        public Discovery discovery { get; set; }
        [JsonProperty("type")]
        public string bodyType { get; set; }
        public string subType { get; set; }
        public Parent[] parents { get; set; }
        public int? distanceToArrival { get; set; }
        public bool isMainStar { get; set; }
        public bool isScoopable { get; set; }
        public int? age { get; set; }
        public string spectralClass { get; set; }
        public string luminosity { get; set; }
        public float? absoluteMagnitude { get; set; }
        public float? solarMasses { get; set; }
        public float? solarRadius { get; set; }
        public int? surfaceTemperature { get; set; }
        public float? orbitalPeriod { get; set; }
        public float? semiMajorAxis { get; set; }
        public float? orbitalEccentricity { get; set; }
        public float? orbitalInclination { get; set; }
        public float? argOfPeriapsis { get; set; }
        public float? rotationalPeriod { get; set; }
        public bool rotationalPeriodTidallyLocked { get; set; }
        public float? axialTilt { get; set; }
        public Belt[] belts { get; set; }
        public string updateTime { get; set; }
        public bool isLandable { get; set; }
        public float? gravity { get; set; }
        public float? earthMasses { get; set; }
        public float? radius { get; set; }
        public float? surfacePressure { get; set; }
        public string volcanismType { get; set; }
        public string atmosphereType { get; set; }
        public Atmospherecomposition atmosphereComposition { get; set; }
        public Solidcomposition solidComposition { get; set; }
        public string terraformingState { get; set; }
        public Ring[] rings { get; set; }
        public string reserveLevel { get; set; }
        public Materials materials { get; set; }
    }

    public class Discovery
    {
        public string commander { get; set; }
        public string date { get; set; }
    }

    public class Atmospherecomposition
    {
        public float? Argon { get; set; }
        public float? Hydrogen { get; set; }
        public float? Helium { get; set; }
        public float? Silicates { get; set; }
        public float? Carbondioxide { get; set; }
        public float? Water { get; set; }
        public float? Sulphurdioxide { get; set; }
        public float? Oxygen { get; set; }
        public float? Methane { get; set; }
        public float? Nitrogen { get; set; }
        public float? Ammonia { get; set; }
        public float? Neon { get; set; }
    }

    public class Solidcomposition
    {
        public float? Metal { get; set; }
        public float? Ice { get; set; }
        public float? Rock { get; set; }
    }

    public class Materials
    {
        public float? Iron { get; set; }
        public float? Nickel { get; set; }
        public float? Chromium { get; set; }
        public float? Zinc { get; set; }
        public float? Zirconium { get; set; }
        public float? Niobium { get; set; }
        public float? Molybdenum { get; set; }
        public float? Technetium { get; set; }
        public float? Selenium { get; set; }
        public float? Tin { get; set; }
        public float? Sulphur { get; set; }
        public float? Carbon { get; set; }
        public float? Phosphorus { get; set; }
        public float? Manganese { get; set; }
        public float? Yttrium { get; set; }
        public float? Mercury { get; set; }
        public float? Tellurium { get; set; }
        public float? Tungsten { get; set; }
        public float? Antimony { get; set; }
        public float? Germanium { get; set; }
        public float? Vanadium { get; set; }
        public float? Cadmium { get; set; }
        public float? Ruthenium { get; set; }
        public float? Arsenic { get; set; }
        public float? Polonium { get; set; }
    }

    public class Parent
    {
        public int Null { get; set; }
        public int Star { get; set; }
        public int Planet { get; set; }
    }

    public class Belt
    {
        public string name { get; set; }
        public string type { get; set; }
        public long? mass { get; set; }
        public decimal? innerRadius { get; set; }
        public decimal? outerRadius { get; set; }
    }

    public class Ring
    {
        public string name { get; set; }
        public string type { get; set; }
        public long? mass { get; set; }
        public decimal? innerRadius { get; set; }
        public decimal? outerRadius { get; set; }
    }
}
