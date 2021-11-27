using System;
using EliteAPI.Event.Models;
using EliteJoystick.Common.Interfaces;
using EliteJoystick.Common.Models;
using Microsoft.Extensions.Logging;

namespace EliteGameStatus.Exploration
{
    public static class EliteActions
    {
        public static void OutputValuableSystems(StarSystem starSystem, ILogger logger) // "InGame" Logger
        {
            logger.LogInformation($"-----------------------------------------------------------------------------------------------------------------");
            logger.LogInformation($"System: {starSystem.name}");

            foreach (var body in starSystem.bodies)
            {
                var starScan = new EliteGameStatus.Exploration.ScanBody();
                if (body.bodyType == "Planet" && Exploration.EliteStarTypes.Bodies.ContainsKey(body.subType))
                    starScan = Exploration.EliteStarTypes.Bodies[body.subType];
                if (body.bodyType == "Star" && Exploration.EliteStarTypes.Stars.ContainsKey(body.subType))
                    starScan = Exploration.EliteStarTypes.Stars[body.subType];

                var bodyText = string.Format("{0,-8}{1,-30}{2,-40}{3,-26}{4,9}", 
                    body.distanceToArrival, 
                    body.subType.Substring(0, Math.Min(body.subType.Length, 27)), 
                    body.name, 
                    body.terraformingState, 
                    starScan.value);

                logger.LogInformation(bodyText);
            }
        }

        internal static void OutputValuableBody(ScanEvent scanInfoEvent, ILogger logger) // "InGame" Logger
        {
            if (null != scanInfoEvent)
            {
                var output = String.Empty;
                if (false == string.IsNullOrEmpty(scanInfoEvent.TerraformState))
                {
                    output = $"Body {scanInfoEvent.BodyName} is {scanInfoEvent.TerraformState}";
                    logger.LogInformation(output);
                }
            }
        }

        public static string HttpOutputValuableSystems(StarSystem starSystem)
        {
            var page = new Explore(starSystem);
            String pageContent = page.TransformText();

            /*
            var bodyReport = new StringBuilder();

            bodyReport.AppendFormat(Page.Title, starSystem.name, starSystem.distancefromhome);

            foreach (var body in starSystem.bodies)
            {
                if (body.type == "Star" && new List<string> { "H", "N", "D" }.Any(item => item == body.stellarclass))
                {
                    bodyReport.Append($"Stellar Class: {body.stellarclass}  Distance: {body.distance} </br>");
                }
                else if (null != body.planettype &&
                    Exploration.StarTypes.Bodies.ContainsKey(body.planettype) &&
                    Exploration.StarTypes.Bodies[body.planettype].scan)
                {
                    bodyReport.Append(String.Format(
                        Page.Entry, body.name, body.planettype, body.distance, body.terraformstate));
                }
            }
            */

            var fileName = $"{Environment.GetEnvironmentVariable("LocalAppData")}\\EliteJoystick\\Pages\\explore.html";

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                file.Write(pageContent.ToString());
            }

            return fileName;
        }
    }
}
