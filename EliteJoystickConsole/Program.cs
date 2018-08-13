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
            if (false)
            {
                var chrome = new Chrome("http://localhost:9222");

                foreach (var session in chrome.GetAvailableSessions())
                {
                    Console.WriteLine(session.title);
                }

                var sessions = chrome.GetAvailableSessions();

                var sessionWSEndpoint = sessions[0].webSocketDebuggerUrl;
                chrome.SetActiveSession(sessionWSEndpoint);
                var uri = @"http://localhost:8080/explore.html";

                //chrome.NavigateTo("http://eddb.io");

                chrome.NavigateTo(uri);
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

                var task = Task.Run(async () => await client.ConnectJoysticks())
                    .ContinueWith(t => { log.Error($"ConnectJoysticks Exception: {t.Exception}"); }, 
                    TaskContinuationOptions.OnlyOnFaulted);
            }
            Console.ReadKey();
        }
    }
}
