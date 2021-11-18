using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace vKeyboard
{
    public static class WinApi
    {
        public static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        [DllImport("SetupApi.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, int Flags);

        [DllImport("Setupapi.dll", CharSet = CharSet.Auto)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, uint memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]//, CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean SetupDiGetDeviceInterfaceDetail(
            IntPtr hDevInfo,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            IntPtr deviceInterfaceDetailData,
            UInt32 deviceInterfaceDetailDataSize,
            ref UInt32 requiredSize,
            ref SP_DEVINFO_DATA deviceInfoData);

        [DllImport("HID.dll", CharSet = CharSet.Auto)]
        public static extern void HidD_GetHidGuid(out Guid ClassGuid);

        [DllImport("HID.dll", CharSet = CharSet.Auto)]
        public static extern bool HidD_GetAttributes(SafeFileHandle HidDeviceObject, ref HIDD_ATTRIBUTES Attributes);

        [DllImport("HID.dll", CharSet = CharSet.Auto)]
        public static extern bool HidD_GetNumInputBuffers(SafeFileHandle HidDeviceObject, ref uint NumberBuffers);

        [DllImport("hid.dll", CharSet = CharSet.Auto)]
        public static extern bool HidD_SetNumInputBuffers(SafeFileHandle HidDeviceObject, uint BufferLength);

        [DllImport("hid.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool HidD_SetFeature(SafeFileHandle HidDeviceObject, byte[] Buffer, uint BufferLength);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
            //UInt32 fileAccess,
            [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            //[MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
            uint flagsAndAttributes,
            IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool ReadFileEx(
            SafeFileHandle hFile,
            [Out] byte[] lpbuffer,
            [In] uint nNumberOfBytesToRead,
            [In, Out] ref NativeOverlapped lpOverlapped,
            IntPtr lpCompletionRoutine);


        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public uint cbSize;
            public Guid interfaceClassGuid;
            public Int32 flags;
            private IntPtr reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        public struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public uint cbSize;
            public char devicePath;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVINFO_DATA
        {
            public uint cbSize;
            public Guid ClassGuid;
            public uint DevInst;
            public IntPtr Reserved;
        }

        public struct HIDD_ATTRIBUTES
        {
            public Int32 Size;
            public UInt16 VendorID;
            public UInt16 ProductID;
            public UInt16 VersionNumber;
        }

        public class UnmanagedMemory : IDisposable
        {
            public IntPtr Ptr { get; set; }

            public UnmanagedMemory(uint needed)
            {
                Ptr = Marshal.AllocHGlobal((int)needed);
            }

            public void Dispose()
            {
                Marshal.FreeHGlobal(Ptr);
            }
        }

    }
}
