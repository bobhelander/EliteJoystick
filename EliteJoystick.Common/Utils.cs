using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick.Common
{
    /// <summary>
    /// DLL Import Methods and various other methods
    /// </summary>
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

        /// <summary>
        /// Start an external process
        /// </summary>
        /// <param name="filename">Full path to executable</param>
        /// <returns></returns>
        public static Process Launch(string filename) =>
            Process.Start(filename);

        /// <summary>
        /// Kill a runnig process
        /// </summary>
        /// <param name="name">Name of the process to kill</param>
        /// <returns></returns>
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

        /// <summary>
        /// Return if a process is running
        /// </summary>
        /// <param name="name">Name of the process</param>
        /// <returns></returns>
        public static bool ProcessRunning(string name)
        {
            var processes = Process.GetProcesses().ToList();
            return processes.Any(x => x.ProcessName == name);
        }

        private const int WM_APPCOMMAND = 0x319;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;

        /// <summary>
        /// Send a virtual keystroke to a process.
        /// </summary>
        /// <param name="name">Name of the process</param>
        /// <param name="vkey">Key</param>
        public static void SendVKeyToProcess(string name, int vkey)
        {
            var processes = Process.GetProcesses().ToList();
            foreach (var p in Process.GetProcesses().Where(x => x.ProcessName == name && x.MainWindowHandle.ToInt64() != 0))
            {
                SendMessage(p.MainWindowHandle, WM_APPCOMMAND, p.MainWindowHandle, (IntPtr)((int)vkey << 16));
                return;
            }
        }

        /// <summary>
        /// Send a key press and a release to a process. 50 millisecond delay on the release.
        /// </summary>
        /// <param name="name">Name of the process</param>
        /// <param name="key">key</param>
        /// <returns></returns>
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

        /// <summary>
        /// Run the script to turn off Oculus's Automatic Spacewarp feature.
        /// </summary>
        /// <returns></returns>
        public static async Task OculusASWOff()
        {
            await Task.Run(() =>
            {
                var currentPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

                const string oculusDebugToolCLI = @"C:\Program Files\Oculus\Support\oculus-diagnostics\OculusDebugToolCLI.exe";
                const string inputFile = @".\scripts\ASW_OFF.txt";

                var processInfo = new ProcessStartInfo(oculusDebugToolCLI, "-f " + inputFile)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    WorkingDirectory = currentPath
                };

                var process = Process.Start(processInfo);
                process.WaitForExit();

                Console.WriteLine($"ASW output: {process.StandardOutput.ReadToEnd()}");
                Console.WriteLine($"ASW error: {process.StandardError.ReadToEnd()}");
                Console.WriteLine($"ASW ExitCode: {process.ExitCode}");
                process.Close();
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Opens the Explorer Log
        /// </summary>
        /// <returns></returns>
        public static async Task ExplorerLog()
        {
            await Task.Run(() =>
            {
                var currentPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

                const string powershell = "powershell.exe";
                const string inputFile = @".\scripts\InGameLogs.ps1";

                var processInfo = new ProcessStartInfo(powershell, inputFile)
                //var processInfo = new ProcessStartInfo(powershell)
                {
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    RedirectStandardError = false,
                    RedirectStandardOutput = false,
                    WorkingDirectory = currentPath
                };

                var process = Process.Start(processInfo);
                process.WaitForExit();

                //Console.WriteLine($"PS output: {process.StandardOutput.ReadToEnd()}");
                //Console.WriteLine($"PS error: {process.StandardError.ReadToEnd()}");
                //Console.WriteLine($"PS ExitCode: {process.ExitCode}");
                //process.Close();
            }).ConfigureAwait(false);
        }

        public static void LogTaskResult(Task t, string methodName, ILogger logger)
        {
            if (t.IsCanceled)
            {
                logger.LogWarning($"{methodName} Canceled");
            }
            else if (t.IsFaulted)
            {
                // Logging and Auditing 

                // Pull the innermost exception
                Exception ex = t.Exception;
                while (ex is AggregateException && ex.InnerException != null)
                    ex = ex.InnerException;

                logger.LogError($"{methodName} Exception: {ex}");
            }
            else if (t.IsCompleted)
            {
                // Enable when debugging TaskCanceledExceptions
                //logger.LogDebug($"{methodName} Completed");
            }
        }
    }
}
