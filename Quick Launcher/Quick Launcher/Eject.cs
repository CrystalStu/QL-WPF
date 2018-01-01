﻿using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Quick_Launcher
{
    /// <summary>
    /// 弹出U盘，调用方法：
    ///
    /// EjectUsb eju = new EjectUsb("H:");
    /// bool result = eju.Eject();
    /// if (result)
    /// {
    ///    MessageBox.Show("success");
    /// }
    /// else
    /// {
    ///    MessageBox.Show("fail");
    /// }
    /// </summary>
    class Ejection
    {
        //返回设备句柄
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr CreateFile(
             string lpFileName,
             uint dwDesiredAccess,
             uint dwShareMode,
             IntPtr SecurityAttributes,
             uint dwCreationDisposition,
             uint dwFlagsAndAttributes,
             IntPtr hTemplateFile
        );
        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            IntPtr lpOutBuffer,
            uint nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped
        );
        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            byte[] lpInBuffer,
            uint nInBufferSize,
            IntPtr lpOutBuffer,
            uint nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped
        );
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);
        private IntPtr handle = IntPtr.Zero;
        const uint GENERIC_READ = 0x80000000;
        const int GENERIC_WRITE = 0x40000000;
        const int FILE_SHARE_READ = 0x1;
        const int FILE_SHARE_WRITE = 0x2;
        const int FSCTL_LOCK_VOLUME = 0x00090018;
        const int FSCTL_DISMOUNT_VOLUME = 0x00090020;
        const int IOCTL_STORAGE_EJECT_MEDIA = 0x2D4808;
        const int IOCTL_STORAGE_MEDIA_REMOVAL = 0x002D4804;
        /// <summary>
        /// 初始化设备句柄
        /// </summary>
        /// <param name="driveLetter">设备名如: F:/, C:/..</param>
        public Ejection(string driveLetter)
        {
            string filename = @"\\.\" + driveLetter[0] + ":";
            handle = CreateFile(filename, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, 0x3, 0, IntPtr.Zero);
        }
        /// <summary>
        /// 卸载u盘
        /// </summary>
        /// <returns>正确返回true,错误返回false</returns>
        public bool Eject()
        {
            if (LockVolume(handle) && DismountVolume(handle))
            {
                PreventRemovalOfVolume(handle, false);
                return AutoEjectVolume(handle);
            }
            return false;
        }
        private bool LockVolume(IntPtr handle)
        {
            uint byteReturned;
            for (int i = 0; i < 10; i++)
            {
                if (DeviceIoControl(handle, FSCTL_LOCK_VOLUME, IntPtr.Zero, 0, IntPtr.Zero, 0, out byteReturned, IntPtr.Zero))
                {
                    return true;
                }
                Thread.Sleep(500);
            }
            return false;
        }
        private bool PreventRemovalOfVolume(IntPtr handle, bool prevent)
        {
            byte[] buf = new byte[1];
            uint retVal;
            buf[0] = (prevent) ? (byte)1 : (byte)0;
            return DeviceIoControl(handle, IOCTL_STORAGE_MEDIA_REMOVAL, buf, 1, IntPtr.Zero, 0, out retVal, IntPtr.Zero);
        }
        private bool DismountVolume(IntPtr handle)
        {
            uint byteReturned;
            return DeviceIoControl(handle, FSCTL_DISMOUNT_VOLUME, IntPtr.Zero, 0, IntPtr.Zero, 0, out byteReturned, IntPtr.Zero);
        }
        private bool AutoEjectVolume(IntPtr handle)
        {
            uint byteReturned;
            return DeviceIoControl(handle, IOCTL_STORAGE_EJECT_MEDIA, IntPtr.Zero, 0, IntPtr.Zero, 0, out byteReturned, IntPtr.Zero);
        }
        public bool CloseVolume(IntPtr handle)
        {
            return CloseHandle(handle);
        }
    }
}
