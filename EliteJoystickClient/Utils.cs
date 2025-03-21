﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace EliteJoystickClient
{
    public static class Utils
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hwnd);

        static public string CallClipboard()
        {
            object returnValue = null;
            var t = new Thread((ThreadStart)(() =>
                returnValue = System.Windows.Clipboard.GetText()));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            return returnValue as String;
        }

        static public void SetClipboardText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var t = new Thread((ThreadStart)(() =>
                System.Windows.Clipboard.SetText(text)));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }

        private const int SW_SHOWNORMAL = 1;

        public static string FocusWindow(string name)
        {
            Process[] processes = Process.GetProcessesByName(name);

            foreach (Process p in Process.GetProcesses().Where(x => x.ProcessName == name))
            {
                ShowWindow(p.MainWindowHandle, SW_SHOWNORMAL);
                SetForegroundWindow(p.MainWindowHandle);
                return p.ProcessName;
            }
            return String.Empty;
        }

        public static async Task KillProcess(string name)
        {
            await Task.Run(() =>
            {
                foreach (var p in Process.GetProcesses().Where(x => x.ProcessName == name))
                {
                    p.Kill();
                }
            }).ConfigureAwait(false);
        }

        /*
        public static void NavigateUrl(string url)
        {
            try
            {
                var chrome = new GoogleChrome.Chrome("http://localhost:9222");
                var sessions = chrome.GetAvailableSessions();
                chrome.ActivateTab(sessions[0]);
                chrome.NavigateTo(sessions[0], url);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        */
    }
}
