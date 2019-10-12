using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace VoiceMeeter
{
    public static class Remote
    {
        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_Login", CallingConvention = CallingConvention.Cdecl)]
        public static extern int LoginVoicemeeter();

        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_Logout")]
        public static extern int Logout();

        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_RunVoicemeeter")]
        public static extern int RunVoicemeeter(int _);

        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_GetParameterFloat")]
        public static extern int GetParameter(string szParamName, ref float value);
        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_SetParameterFloat")]
        public static extern int SetParameter(string szParamName, float value);

        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_GetParameterStringW", CharSet = CharSet.Unicode)]
        public static extern int GetParameter(string szParamName, ref string value);

        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_IsParametersDirty")]
        public static extern int IsParametersDirty();

        // Load the DLL to pull it into the running process
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        // Don't keep loading the DLL
        private static IntPtr? handle;

        public static async Task<IDisposable> Initialize()
        {
            if (handle.HasValue == false)
            {
                // Find current version from the registry
                const string key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                const string uninstKey = "VB:Voicemeeter {17359A74-1236-5467}";
                var voicemeeter = Registry.GetValue($"{key}\\{uninstKey}", "UninstallString", null);
                if (voicemeeter == null)
                {
                    throw new Exception("Voicemeeter not installed");
                }

                handle = LoadLibrary(
                    System.IO.Path.Combine(System.IO.Path.GetDirectoryName(voicemeeter.ToString()), "VoicemeeterRemote.dll"));
            }

            if (await Login().ConfigureAwait(false))
            {
                return new Voicemeeter.VoicemeeterClient();
            }

            return null;
        }

        public static async Task<bool> Login(bool retry = true)
        {
            switch ((VoiceMeeter.LoginResponse)LoginVoicemeeter())
            {
                case VoiceMeeter.LoginResponse.OK:
                case VoiceMeeter.LoginResponse.AlreadyLoggedIn:
                    return true;

                case VoiceMeeter.LoginResponse.VoicemeeterNotRunning:
                    if (retry)
                    {
                        // Run voicemeeter, not sure what "2" is
                        RunVoicemeeter(2);
                        await Task.Delay(2000).ConfigureAwait(false);
                        return await Login(false).ConfigureAwait(false);
                    }
                    break;
            }
            return false;

        }
    }
}