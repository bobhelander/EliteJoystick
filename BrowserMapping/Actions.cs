using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BrowserMapping
{
    static public class Actions
    {
        public static void OpenPage(string url)
        {
            Controller.Navigate(url);
        }

        public static void BestSellCommodity(string commodity = "350") // Void Opals
        {
            // https://eddb.io/commodity/350
            var url = $"https://eddb.io/commodity/{commodity}";
            Controller.NewChromeTab(url, 800);
        }

        public static void MaterialTradersEdsm(string systemName)
        {
            // https://www.edsm.net/en/search/stations/index/service/71/cmdrPosition/Hydrae+Sector+EW-W+b1-4/sortBy/distanceCMDR
            var url = $"https://www.edsm.net/en/search/stations/index/service/71/cmdrPosition/{Uri.EscapeUriString(systemName)}/sortBy/distanceCMDR";
            Controller.NewChromeTab(url, 700);
        }

        public static void TechnologyBrokersEdsm(string systemName)
        {
            // https://www.edsm.net/en/search/stations/index/service/70/cmdrPosition/Hydrae+Sector+EW-W+b1-4/sortBy/distanceCMDR
            var url = $"https://www.edsm.net/en/search/stations/index/service/70/cmdrPosition/{Uri.EscapeUriString(systemName)}/sortBy/distanceCMDR";
            Controller.NewChromeTab(url, 700);
        }

        public static void HighGradeEmissionsEdsm(string systemName)
        {
            // https://www.edsm.net/en/search/systems/index/cmdrPosition/Hydrae+Sector+EW-W+b1-4/onlyPopulated/1/radius/250/sortBy/distanceCMDR/ussDrop/85
            var url = $"https://www.edsm.net/en/search/systems/index/cmdrPosition/{Uri.EscapeUriString(systemName)}/onlyPopulated/1/radius/250/sortBy/distanceCMDR/ussDrop/85";
            Controller.NewChromeTab(url, 700);
        }

        public static void InterstellarFactorsContactEdsm(string systemName)
        {
            // https://www.edsm.net/en/search/stations/index/cmdrPosition/Hydrae+Sector+EW-W+b1-4/service/39/sortBy/distanceCMDR
            var url = $"https://www.edsm.net/en/search/stations/index/cmdrPosition/{Uri.EscapeUriString(systemName)}/service/39/sortBy/distanceCMDR";
            Controller.NewChromeTab(url, 700);
        }

        public static string GetEdsmSystemRow(int row)
        {
            // F12 to evaluate Chrome DOM

            // If this returns "System" use systemDom
            var typeJson = Controller.ChromeCommand($"document.getElementsByClassName('table table-hover')[0].rows[0].cells[1].innerText");

            dynamic tableType = JObject.Parse(typeJson);
            var value = (string)tableType.result.result.value;

            string json;
            if (string.IsNullOrEmpty(value))
            {
                // Station Table
                json = Controller.ChromeCommand($"document.getElementsByClassName('table table-hover')[0].rows[{row}].cells[1].childNodes[4].childNodes[0].childNodes[0].textContent");
            }
            else
            {
                // System Table
                json = Controller.ChromeCommand($"document.getElementsByClassName('table table-hover')[0].rows[{row}].cells[1].childNodes[1].childNodes[0].childNodes[0].textContent");
            }

            dynamic d = JObject.Parse(json);
            var systemName = (string)d.result.result.value;

            return systemName;
        }

        public static void MaterialTradersEddb(string EDDBID, string type)
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

            string url;
            if (economy > 0)
                url = $"https://eddb.io/station?h=has_material_trader&i=1&r={EDDBID}&e={economy}";
            else
                url = $"https://eddb.io/station?h=has_material_trader&i=1&r={EDDBID}";

            Controller.NewChromeTab(url, 800);
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

        private static readonly Dictionary<string, int> EngineerNames = new Dictionary<string, int>
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

        public static void EngineerByName(string engineerName)
        {
            var engineerId = EngineerNames[engineerName];
            EngineerById($"{engineerId}");
        }

        public static void EngineerById(string engineerId)
        {
            // https://www.edsm.net/en/engineers/details/id/17/name/Lei+Cheung
            var url = $"https://www.edsm.net/en/engineers/details/id/{engineerId}";
            Controller.NewChromeTab(url, 0);
        }
    }
}
