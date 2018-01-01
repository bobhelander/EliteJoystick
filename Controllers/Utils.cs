using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public static class Utils
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hwnd);

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

        public static void RefreshvJoySpecific(ushort Revision)
        {
            refresh_vjoy_specific(Revision);
        }

        public static short GetvJoyVersion()
        {
            return _GetvJoyVersion();
        }

        public static bool Disable(ushort Revision)
        {
            return disable(Revision);
        }

        public static Process Launch(string filename)
        {
            return Process.Start(filename);
        }
    }
}
