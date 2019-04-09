using EddiDataDefinitions;
using EddiDataProviderService;
using EddiEvents;
using EliteJoystickClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EddiJoystickResponder
{
    public static class EventHandlers
    {
        public static void DockedEvent(Client client, DockedEvent actionEvent)
        {
            
        }

        public static void JumpedEvent(Client client, JumpedEvent actionEvent)
        {
            // We have entered a new system
            StarSystem starSystem = StarSystemSqLiteRepository.Instance.GetOrCreateStarSystem(actionEvent.system, true);
            var page = Exploration.Actions.OutputValuableSystems(client, starSystem);

            var local_files_base = "http://127.0.0.1:8080";
            var httpPage = $"{local_files_base}/{Path.GetFileName(page)}";

            client.Navigate(httpPage);
        }

        public static void EddiAction(Client client, JoystickActionEvent actionEvent, Dictionary<string, string> environmentVars)
        {
            switch (actionEvent.command)
            {
                case "best_sell_commodity":
                    BestSellCommodity(client, actionEvent.var1);
                    break;
                case "material_traders_edsm":
                    MaterialTradersEdsm(client, actionEvent.var1);
                    break;
                case "material_traders_eddb":
                    MaterialTradersEddb(client, actionEvent.var1, actionEvent.var2);
                    break;
                case "scroll":
                    Scroll(client, actionEvent.var1);
                    break;
                case "change_tab":
                    ChangeTab(client, actionEvent.var1);
                    break;
                case "engineer_by_name":
                    EngineerByName(client, actionEvent.var1);
                    break;
                case "engineer_by_id":
                    EngineerById(client, actionEvent.var1);
                    break;
            }
        }

        public static void BestSellCommodity(Client client, string commodity = "350") // Void Opals
        {
            // https://eddb.io/commodity/350
            var url = $"https://eddb.io/commodity/{commodity}";
            client.NewChromeTab(url, 800);
        }

        public static void MaterialTradersEdsm(Client client, string systemName)
        {
            // https://www.edsm.net/en/search/stations/index/service/71/cmdrPosition/Hydrae+Sector+EW-W+b1-4/sortBy/distanceCMDR
            var url = $"https://www.edsm.net/en/search/stations/index/service/71/cmdrPosition/{Uri.EscapeUriString(systemName)}/sortBy/distanceCMDR";
            client.NewChromeTab(url, 700);
        }

        public static void MaterialTradersEddb(Client client, string systemName, string type)
        {
            // https://eddb.io/station?h=has_material_trader&i=1&r=10340&e=4   Manufactured
            // https://eddb.io/station?h=has_material_trader&i=1&r=10340&e=2   Raw
            // https://eddb.io/station?h=has_material_trader&i=1&r=10340&e=3   Data

            var economy = 0;

            if ("manufactured".CompareTo(type) == 0)
                economy = 4;
            if ("raw".CompareTo(type) == 0)
                economy = 2;
            if ("data".CompareTo(type) == 0)
                economy = 3;

            StarSystem starSystem = StarSystemSqLiteRepository.Instance.GetOrCreateStarSystem(systemName, true);

            var url = string.Empty;

            if (economy > 0)
                url = $"https://eddb.io/station?h=has_material_trader&i=1&r={starSystem.EDDBID}&e={economy}";
            else
                url = $"https://eddb.io/station?h=has_material_trader&i=1&r={starSystem.EDDBID}";

            client.NewChromeTab(url, 800);
        }

            /*
            https://www.edsm.net/en/engineers/details/id/6/name/Felicity+Farseer
            https://www.edsm.net/en/engineers/details/id/14/name/Juri+Ishmaak
            https://www.edsm.net/en/engineers/details/id/11/name/Colonel+Bris+Dekker
            https://www.edsm.net/en/engineers/details/id/16/name/The+Sarge
            https://www.edsm.net/en/engineers/details/id/12/name/Elvira+Martuuk
            https://www.edsm.net/en/engineers/details/id/7/name/Marco+Qwent
            https://www.edsm.net/en/engineers/details/id/8/name/Professor+Palin
            https://www.edsm.net/en/engineers/details/id/13/name/Lori+Jameson
            https://www.edsm.net/en/engineers/details/id/15/name/Zacariah+Nemo
            https://www.edsm.net/en/engineers/details/id/23/name/Mel+Brandon
            https://www.edsm.net/en/engineers/details/id/10/name/The+Dweller
            https://www.edsm.net/en/engineers/details/id/17/name/Lei+Cheung
            https://www.edsm.net/en/engineers/details/id/18/name/Ram+Tah
            https://www.edsm.net/en/engineers/details/id/22/name/Marsha+Hicks
            https://www.edsm.net/en/engineers/details/id/4/name/Tod+%27The+Blaster%27+McQuinn
            https://www.edsm.net/en/engineers/details/id/5/name/Selene+Jean
            https://www.edsm.net/en/engineers/details/id/9/name/Didi+Vatermann
            https://www.edsm.net/en/engineers/details/id/19/name/Bill+Turner
            https://www.edsm.net/en/engineers/details/id/21/name/Petra+Olmanova
            https://www.edsm.net/en/engineers/details/id/2/name/Liz+Ryder
            https://www.edsm.net/en/engineers/details/id/1/name/Hera+Tani
            https://www.edsm.net/en/engineers/details/id/3/name/Broo+Tarquin
            https://www.edsm.net/en/engineers/details/id/20/name/Tiana+Fortune
            https://www.edsm.net/en/engineers/details/id/24/name/Etienne+Dorn
            */

        static readonly Dictionary<string, int> EngineerNames = new Dictionary<string, int>
        {
            { "Felicity Farseer", 6},
            { "Juri Ishmaak", 14},
            { "Colonel Bris Dekker", 11},
            { "The Sarge", 16},
            { "Elvira Martuuk", 12},
            { "Marco Qwent", 7},
            { "Professor Palin", 8},
            { "Lori Jameson", 13},
            { "Zacariah Nemo", 15},
            { "Mel Brandon", 23},
            { "The Dweller", 10},
            { "Lei Cheung", 17},
            { "Ram Tah", 18},
            { "Marsha Hicks", 22},
            { "Tod The Blaster McQuinn", 4},
            { "Selene Jean", 5},
            { "Didi Vatermann", 9},
            { "Bill Turner", 19},
            { "Petra Olmanova", 21},
            { "Liz Ryder", 2},
            { "Hera Tani", 1},
            { "Broo Tarquin", 3},
            { "Tiana Fortune", 20},
            { "Etienne Dorn", 24},
        };

        public static void EngineerByName(Client client, string engineerName)
        {
            var engineerId = EngineerNames[engineerName];
            EngineerById(client, $"{engineerId}");
        }

        public static void EngineerById(Client client, string engineerId)
        {
            // https://www.edsm.net/en/engineers/details/id/17/name/Lei+Cheung
            var url = $"https://www.edsm.net/en/engineers/details/id/{engineerId}";
            client.NewChromeTab(url, 0);
        }

        public static void Scroll(Client client, string amount)
        {
            if (int.TryParse(amount, out var number ))
            {
                client.Scroll(number);
            }
        }

        public static void ChangeTab(Client client, string amount)
        {
            if (int.TryParse(amount, out var number))
            {
                client.ChangeTab(number);
            }
        }
    }
}
