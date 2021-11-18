using Microsoft.Extensions.Logging;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using vKeyboard.Models;

namespace vKeyboard.Logic
{
    public class Device : IDisposable
    {
        private String devicePathName { get; set; }
        private SafeFileHandle deviceFileHandle { get; set; }

        public ushort ProductID { get; set; }
        public ushort VendorID { get; set; }

        public Device(IntPtr hDeviceInfoSet, WinApi.SP_DEVICE_INTERFACE_DATA deviceInterfaceData, ILogger log)
        {
            WinApi.SP_DEVINFO_DATA deviceInfoData = new WinApi.SP_DEVINFO_DATA();
            deviceInfoData.cbSize = (uint)Marshal.SizeOf(deviceInfoData);

            uint needed = 0;

            bool result = WinApi.SetupDiGetDeviceInterfaceDetail(
                hDeviceInfoSet, ref deviceInterfaceData, IntPtr.Zero, 0, ref needed, ref deviceInfoData);

            //it's supposed to give an error 122 as we just only retrieved the data size needed, so this is as designed
            if (result == false && Marshal.GetLastWin32Error() == 122)
            {
                Connect(hDeviceInfoSet, deviceInterfaceData, needed, log);
            }
        }

        void Connect(IntPtr hDeviceInfoSet, WinApi.SP_DEVICE_INTERFACE_DATA deviceInterfaceData, uint sizeNeeded, ILogger log)
        {
            using (var deviceInterfaceDetailData = new WinApi.UnmanagedMemory(sizeNeeded))
            {
                IntPtr deviceInterfaceDetailData2 = Marshal.AllocHGlobal((int)sizeNeeded);

                uint size = sizeNeeded;
                Marshal.WriteInt32(deviceInterfaceDetailData.Ptr, IntPtr.Size == 8 ? 8 : 6);

                WinApi.SP_DEVINFO_DATA DevInfoData = new WinApi.SP_DEVINFO_DATA();
                DevInfoData.cbSize = (uint)Marshal.SizeOf(DevInfoData);

                uint needed = 0;

                if (WinApi.SetupDiGetDeviceInterfaceDetail(hDeviceInfoSet, ref deviceInterfaceData, deviceInterfaceDetailData.Ptr, size, ref needed, ref DevInfoData) == false)
                {
                    int errorNumber = Marshal.GetLastWin32Error();
                    log?.LogDebug("SetupDiGetDeviceInterfaceDetail {errorNumber}", errorNumber);
                    return;
                }

                IntPtr pDevicePathName = new IntPtr(deviceInterfaceDetailData.Ptr.ToInt32() + 4);
                devicePathName = Marshal.PtrToStringAuto(pDevicePathName);

                var fileHandle = OpenFileHandle(devicePathName);

                if (fileHandle.IsInvalid == false)
                {
                    //this device has readwrite access, could it be the device we are looking for?
                    WinApi.HIDD_ATTRIBUTES HIDAttributes = new WinApi.HIDD_ATTRIBUTES();
                    HIDAttributes.Size = Marshal.SizeOf(HIDAttributes);
                    if (WinApi.HidD_GetAttributes(fileHandle, ref HIDAttributes))
                    {
                        ProductID = HIDAttributes.ProductID;
                        VendorID = HIDAttributes.VendorID;
                    }

                    fileHandle.Close();
                }
            }
        }

        public void Open()
        {
            deviceFileHandle = OpenFileHandle(devicePathName);
        }

        private static SafeFileHandle OpenFileHandle(string devicePathName)
        {
            //see if this driver has readwrite access
            var fileHandle = WinApi.CreateFile(
                devicePathName,
                System.IO.FileAccess.ReadWrite,
                System.IO.FileShare.ReadWrite,
                IntPtr.Zero,
                System.IO.FileMode.Open,
                0,
                IntPtr.Zero);

            if (fileHandle.IsInvalid)
            {
                fileHandle = WinApi.CreateFile(
                    devicePathName,
                    0,
                    System.IO.FileShare.ReadWrite,
                    IntPtr.Zero,
                    System.IO.FileMode.Open,
                    0,
                    IntPtr.Zero);
            }

            return fileHandle;
        }

        public async Task SetFeature(byte[] featureMessage) =>
            await SetFeatureAsync(deviceFileHandle, featureMessage, (uint)featureMessage.Length);


        public static Task<bool> SetFeatureAsync(SafeFileHandle fileHandle, byte[] Buffer, uint BufferLength)
        {
            if (fileHandle.IsInvalid == false)
                return Task.FromResult(WinApi.HidD_SetFeature(fileHandle, Buffer, BufferLength + 1));

            return Task.FromResult(false);
        }

        //read data from the driver.  This ideally should be in a thread
        //in this example, we oversimplified the possible effects of overlapped data reads since readfilex is asynchronous
        public Boolean ReadData(SafeFileHandle fileHandle, byte[] Buffer, uint BufferLength)
        {
            bool res = false;
            if (fileHandle.IsInvalid == false)
            {
                var overlapped = new NativeOverlapped();
                overlapped.EventHandle = IntPtr.Zero;
                bool res1 = WinApi.ReadFileEx(fileHandle, Buffer, BufferLength, ref overlapped, IntPtr.Zero);
                res = true;
            }
            return res;
        }

        public void Dispose()
        {
            deviceFileHandle?.Close();
        }
    }
}
