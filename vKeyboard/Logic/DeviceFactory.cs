using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace vKeyboard.Logic
{
    public static class DeviceFactory
    {
        enum DiGetClassFlags : uint
        {
            DIGCF_PRESENT = 0x00000002,
            DIGCF_DEVICEINTERFACE = 0x00000010,
        }

        public static IEnumerable<Device> Devices(ILogger log)
        {
            // Get the HID Class GUID 
            WinApi.HidD_GetHidGuid(out var hidGuid);

            IntPtr hDeviceInfoSet = WinApi.SetupDiGetClassDevs(ref hidGuid, IntPtr.Zero, IntPtr.Zero, (int)(DiGetClassFlags.DIGCF_PRESENT | DiGetClassFlags.DIGCF_DEVICEINTERFACE));
            if (hDeviceInfoSet != (IntPtr)WinApi.INVALID_HANDLE_VALUE)
            {
                var foundDevice = true;
                var index = 0;

                while (foundDevice)
                {
                    WinApi.SP_DEVICE_INTERFACE_DATA deviceInterfaceData = new WinApi.SP_DEVICE_INTERFACE_DATA();
                    deviceInterfaceData.cbSize = (uint)Marshal.SizeOf(deviceInterfaceData);

                    foundDevice = WinApi.SetupDiEnumDeviceInterfaces(hDeviceInfoSet, IntPtr.Zero, ref hidGuid, (uint)index++, ref deviceInterfaceData);

                    if (foundDevice)
                        yield return new Device(hDeviceInfoSet, deviceInterfaceData, log);
                }
            }
            else
            {
                log?.LogDebug("Connect: SetupDiGetClassDevs failed.");
            }
        }
    }
}
