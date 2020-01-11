using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Common
{
    public static class Utils
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, IntPtr lParam);

        [DllImport("vJoyInstall.dll", EntryPoint = "refresh_vjoy_specific")]
        private static extern void refresh_vjoy_specific(ushort Revision);

        [DllImport("vJoyInterface.dll", EntryPoint = "GetvJoyVersion")]
        private static extern short _GetvJoyVersion();

        [DllImport("vJoyInstall.dll", EntryPoint = "enable")]
        private static extern bool enable(ushort Revision);

        [DllImport("vJoyInstall.dll", EntryPoint = "disable")]
        private static extern bool disable(ushort Revision);

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

        public static void RefreshvJoySpecific(ushort Revision) =>
            refresh_vjoy_specific(Revision);

        public static short GetvJoyVersion() =>
            _GetvJoyVersion();

        public static bool Disable(ushort Revision) =>
            disable(Revision);

        public static Process Launch(string filename) =>
            Process.Start(filename);

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

        public static bool ProcessRunning(string name)
        {
            var processes = Process.GetProcesses().ToList();
            return processes.Any(x => x.ProcessName == name);
        }

        private const int WM_APPCOMMAND = 0x319;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;

        public static void SendVKeyToProcess(string name, int vkey)
        {
            var processes = Process.GetProcesses().ToList();
            foreach (var p in Process.GetProcesses().Where(x => x.ProcessName == name && x.MainWindowHandle.ToInt64() != 0))
            {
                SendMessage(p.MainWindowHandle, WM_APPCOMMAND, p.MainWindowHandle, (IntPtr)((int)vkey << 16));
                return;
            }
        }

        public static async Task SendKeyToProcess(string name, int key)
        {
            await Task.Run(() =>
            {
                foreach (var p in Process.GetProcesses().Where(x => x.ProcessName == name && x.MainWindowHandle.ToInt64() != 0))
                {
                    SendMessage(p.MainWindowHandle, WM_KEYDOWN, key, IntPtr.Zero);
                    System.Threading.Thread.Sleep(50);
                    SendMessage(p.MainWindowHandle, WM_KEYUP, key, IntPtr.Zero);
                    return;
                }
            }).ConfigureAwait(false);
        }
    }
}
