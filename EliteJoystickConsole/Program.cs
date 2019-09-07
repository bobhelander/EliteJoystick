using ChromeController;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystickConsole
{
    public static class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            if (false)
            {
                var chrome = new Chrome("http://localhost:9222");

                foreach (var session in chrome.GetAvailableSessions())
                {
                    Console.WriteLine(session.title);
                }

                //                var sessionEddb = chrome.OpenNewTab("https://eddb.io/commodity/350");

                var sessionEddb = chrome.OpenNewTab("https://www.edsm.net/en/search/systems/index/cmdrPosition/Hydrae+Sector+EW-W+b1-4/onlyPopulated/1/radius/250/sortBy/distanceCMDR/ussDrop/85");

                var json1 = chrome.Eval(chrome.CurrentSession, $"document.getElementsByClassName('table table-hover')[0].rows[1].cells[1].childNodes[1].childNodes[0].childNodes[0].textContent");

                var json2 = chrome.Eval(chrome.CurrentSession, $"document.getElementsByClassName('table table-hover')[0].rows[2].cells[1].childNodes[1].childNodes[0].childNodes[0].textContent");

                var google = chrome.OpenNewTab("https://google.com");

                chrome.Scroll(sessionEddb, 800);

                chrome.ActivateTab(google);

                var sessions = chrome.GetAvailableSessions();

                var uri = "http://localhost:8080/explore.html";

                chrome.ActivateTab(sessions[0]);
                chrome.NavigateTo(sessions[0], "http://eddb.io");
                //chrome.NavigateTo(sessions[0], uri);
            }
            if (true)
            {
                var client = new EliteJoystickClient.Client { Name = "elite_joystick_client" };

                client.Initialize().Wait();

                Console.ReadKey();
                client.ConnectArduino().Wait();
                Console.WriteLine("connected");
                //Console.ReadKey();
                //client.PasteClipboard().Wait();

                var task = Task.Run(async () => await client.ConnectJoysticks().ConfigureAwait(false))
                    .ContinueWith(t => log.Error($"ConnectJoysticks Exception: {t.Exception}"),
                    TaskContinuationOptions.OnlyOnFaulted);
            }
            Console.ReadKey();
        }
    }
}
