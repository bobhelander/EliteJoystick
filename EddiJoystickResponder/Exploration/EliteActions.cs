using EddiDataDefinitions;
using EliteJoystickClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EddiJoystickResponder.Exploration
{
    public static class EliteActions
    {
        public static string OutputValuableSystems(Client Client, StarSystem starSystem)
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
