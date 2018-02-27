using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick_Launcher
{
    public static class MyConvert
    {
        public static float ToGigabyte(long x)
        {
            float ans = System.Convert.ToSingle(x) / 1024 / 1024 / 1024;
            ans = ans - (ans % (float)0.01);
            return ans;
        }

        public static float ToMebibyte(long x)
        {
            float ans = System.Convert.ToSingle(x) / 1024 / 1024;
            ans = ans - (ans % (float)0.01);
            return ans;
        }

        public static float ToKilobyte(long x)
        {
            float ans = System.Convert.ToSingle(x) / 1024;
            ans = ans - (ans % (float)0.01);
            return ans;
        }

        public static string Convert(long size)
        {
            string ans = string.Empty;
            if (size > 1024 * 1024 * 1024)
            {
                ans = ToGigabyte(size) + "GB";
            }
            else if (size > 1024 * 1024)
            {
                ans = ToMebibyte(size) + "MB";
            }
            else
            {
                ans = ToKilobyte(size) + "KB";
            }
            return ans;
        }

        public static string GetTotalFreeSpace(this DriveInfo drive)
        {
            return Convert(drive.TotalFreeSpace);
        }

        public static string GetTotalSize(this DriveInfo drive)
        {
            return Convert(drive.TotalSize); ;
        }

    }
}
