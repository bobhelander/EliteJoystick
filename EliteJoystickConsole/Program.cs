using ChromeController;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystickConsole
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            if (true)
            {
                var chrome = new Chrome("http://localhost:9222");

                foreach (var session in chrome.GetAvailableSessions())
                {
                    Console.WriteLine(session.title);
                }

                var sessionEddb = chrome.OpenNewTab("https://eddb.io/commodity/350");
                var google = chrome.OpenNewTab("https://google.com");

                chrome.Scroll(sessionEddb, 800);

                chrome.ActivateTab(google);

                var sessions = chrome.GetAvailableSessions();

                var uri = @"http://localhost:8080/explore.html";

                chrome.ActivateTab(sessions[0]);
                chrome.NavigateTo(sessions[0], "http://eddb.io");
                //chrome.NavigateTo(sessions[0], uri);
            }
            if (false)
            {
                var client = new EliteJoystickClient.Client { Name = "elite_joystick_client" };

                client.Initialize().Wait();

                Console.ReadKey();
                client.ConnectArduino().Wait();
                Console.WriteLine("connected");
                //Console.ReadKey();
                //client.PasteClipboard().Wait();

                var task = Task.Run(async () => await client.ConnectJoysticks())
                    .ContinueWith(t => { log.Error($"ConnectJoysticks Exception: {t.Exception}"); }, 
                    TaskContinuationOptions.OnlyOnFaulted);
            }
            Console.ReadKey();
        }
    }
}
