using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystickConsole
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (false)
            {
                //var test = EdsmConnector.Connector.GetSystem("Maia").Result;
                //var test = EdsmConnector.Connector.GetSystem("Sol").Result;
                var test = EdsmConnector.Connector.GetSystem("Hypoae Aewsy CG-F d11-4").Result;

                //EliteGameStatus.Exploration.EliteActions.OutputValuableSystems(test);

                var test2 = test.name;
            }
            if (false)
            {
                /*
                var chrome = new GoogleChrome.Chrome("http://localhost:9222");

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
                //chrome.NavigateTo(sessions[0], "http://eddb.io");
                chrome.NavigateTo(sessions[0], uri);
                */
            }
            if (true)
            {
                using (ILoggerFactory loggerFactory =
                LoggerFactory.Create(builder => builder.AddSimpleConsole(options =>
                    { options.SingleLine = true; options.TimestampFormat = "hh:mm:ss "; }).SetMinimumLevel(LogLevel.Debug)))
                {
                    ILogger<EliteJoystickClient.Client> logger = loggerFactory.CreateLogger<EliteJoystickClient.Client>();

                    var client = new EliteJoystickClient.Client { Name = "elite_joystick_client", Logger = logger };

                    client.Initialize().Wait();

                    client.ConnectArduino().Wait();
                    Console.WriteLine("connected");
                    //Console.ReadKey();
                    //client.PasteClipboard().Wait();

                    var task = Task.Run(async () => await client.ConnectJoysticks().ConfigureAwait(false))
                        .ContinueWith(t => Console.WriteLine($"ConnectJoysticks Exception: {t.Exception}"),
                        TaskContinuationOptions.OnlyOnFaulted);

                    Console.ReadKey();

                    var task2 = Task.Run(async () => await client.DisconnectJoysticks().ConfigureAwait(false))
                        .ContinueWith(t => Console.WriteLine($"DisconnectJoysticks Exception: {t.Exception}"),
                        TaskContinuationOptions.OnlyOnFaulted);

                }
            }
            if (false)
            {
//                var started = Voicemeeter.Remote.Initialize( Voicemeeter.RunVoicemeeterParam.VoicemeeterBanana).Result;
  //              VoiceMeeter.Remote.SetParameter("Bus[1].Mute", 1);
            }

            while (true)
            {
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
