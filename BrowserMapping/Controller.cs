using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BrowserMapping
{
    static public class Controller
    {
        private static GoogleChrome.Chrome chrome;

        private static GoogleChrome.Chrome GetChromeController()
        {
            if (null == chrome)
                chrome = new GoogleChrome.Chrome("http://localhost:9222");

            return chrome;
        }

        public static void Navigate(string url)
        {
            var chrome = GetChromeController();

            var sessions = chrome.GetAvailableSessions();
            if (sessions?.Count > 0)
            {
                chrome.ActivateTab(sessions[0]);
                chrome.NavigateTo(sessions[0], url);
            }
        }

        public static void Scroll(int distance)
        {
            var chrome = GetChromeController();

            if (null != chrome.CurrentSession)
            {
                chrome.Scroll(chrome.CurrentSession, distance);
            }
        }

        public static void ChangeTab(int number) =>
            GetChromeController().ChangeTab(number);

        public static string ChromeCommand(string command) =>
            GetChromeController().Eval(GetChromeController().CurrentSession, command);

        public static void NewChromeTab(string url, int scrollDistance = 0)
        {
            var chrome = GetChromeController();
            var session = chrome.OpenNewTab(url);
            Thread.Sleep(1500);
            chrome.Scroll(session, scrollDistance);
        }
    }
}
