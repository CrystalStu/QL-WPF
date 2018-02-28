using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Xml.Serialization;

namespace Quick_Launcher
{
    public partial class MainWindow
    {
        #region WndProc_prep
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            this.win_SourceInitialized(this, e);
        }

        void win_SourceInitialized(object sender, EventArgs e)
        {
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            if (hwndSource != null)
            {
                hwndSource.AddHook(new HwndSourceHook(WndProc));
            }
        }
        #endregion

        #region ScanDrives
        public const int WM_DEVICECHANGE = 0x219;
        public const int DBT_DEVICEARRIVAL = 0x8000;    //如果m.Msg的值为0x8000那么表示有U盘插入
        public const int DBT_CONFIGCHANGECANCELED = 0x0019;
        public const int DBT_CONFIGCHANGED = 0x0018;
        public const int DBT_CUSTOMEVENT = 0x8006;
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;
        public const int DBT_DEVICEQUERYREMOVEFAILED = 0x8002;
        public const int DBT_DEVICEREMOVECOMPLETE = 0X8004;
        public const int DBT_DEVICEREMOVEPENDING = 0x8003;
        public const int DBT_DEVICETYPESPECIFIC = 0x8005;
        public const int DBT_DEVNODES_CHANGED = 0x0007;
        public const int DBT_QUERYCHANGECONFIG = 0x0017;
        public const int DBT_USERDEFINED = 0xFFFF;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            try
            {
                if (msg == WM_DEVICECHANGE)
                {
                    switch (wParam.ToInt32())
                    {
                        case DBT_DEVICEARRIVAL:
                            DriveInfo[] drives = DriveInfo.GetDrives();
                            foreach (DriveInfo drive in drives)
                            {
                                if ((drive.DriveType == DriveType.Removable))
                                {
                                    USBList.Items.Clear();
                                    bt_usb_browse.IsEnabled = false;
                                    bt_usb_eject.IsEnabled = false;
                                    bt_open_all.IsEnabled = false;
                                    ListUSB();
                                }
                            }
                            break;
                        case DBT_DEVICEQUERYREMOVE:
                            USBList.Items.Clear();
                            ListUSB();
                            break;
                        case DBT_DEVICEREMOVECOMPLETE:
                            USBList.Items.Clear();
                            bt_usb_browse.IsEnabled = false;
                            bt_usb_eject.IsEnabled = false;
                            bt_open_all.IsEnabled = false;
                            ListUSB();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return IntPtr.Zero;
        }

        private void ListUSB()
        {
            DriveInfo[] disk = DriveInfo.GetDrives();
            foreach (DriveInfo drive in disk)
            {
                if (drive.DriveType.GetHashCode() == 2)
                {
                    USBList.Items.Add(drive.Name.ToString()[0] + ":  " + drive.VolumeLabel + "  " + drive.GetTotalFreeSpace() + "/" + drive.GetTotalSize());
                    KillMyDocument(drive.Name.ToString()[0] + ":\\");
                }
            }
#if DEBUG
#else
            Restart_Explorer();
#endif
        }

        private void PleaseSelect()
        {
            System.Windows.Forms.MessageBox.Show("请选择一项！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        #endregion

        private void Restart_Explorer()
        {
            ProcessStartInfo killer = new ProcessStartInfo("taskkill.exe");
            killer.UseShellExecute = false;
            killer.CreateNoWindow = true;
            killer.Arguments = "/f /im explorer.exe";
            Process.Start(killer);
            System.Threading.Thread.Sleep(500);
            ProcessStartInfo starter = new ProcessStartInfo("explorer.exe");
            Process.Start(starter);
        }

        private void time4refreshDesktop_time(object sender, EventArgs e)
        {
            UseDesktop();
        }

        private void KillMyDocument(string Path)
        {
            try
            {
                string[] FileNames = Directory.GetDirectories(Path);  //获取该文件夹下面的所有文件名
                foreach (string FileName in FileNames)
                {
                    if (Directory.Exists(path: FileName))
                    {
                        string DestinationFile = FileName + ".exe";
                        if (File.Exists(DestinationFile))
                        {
                            FileInfo DestinationInfo = new FileInfo(DestinationFile);
                            DestinationInfo.Attributes = FileAttributes.Normal;
                            File.Delete(DestinationFile);
                            FileInfo RealFile_Info = new FileInfo(FileName);
                            RealFile_Info.Attributes = FileAttributes.Normal;
                        }
                        DestinationFile = FileName + ".lnk";
                        if (File.Exists(DestinationFile))
                        {
                            FileInfo DestinationInfo = new FileInfo(DestinationFile);
                            DestinationInfo.Attributes = FileAttributes.Normal;
                            File.Delete(DestinationFile);
                            FileInfo RealFile_Info = new FileInfo(FileName);
                            RealFile_Info.Attributes = FileAttributes.Normal;
                        }
                    }
                    else if (File.Exists(path: FileName))
                    {
                        string DestinationFile = FileName + ".exe";
                        if (File.Exists(DestinationFile))
                        {
                            FileInfo DestinationInfo = new FileInfo(DestinationFile);
                            DestinationInfo.Attributes = FileAttributes.Normal;
                            File.Delete(DestinationFile);
                            FileInfo RealFile_Info = new FileInfo(FileName);
                            RealFile_Info.Attributes = FileAttributes.Normal;
                        }
                        DestinationFile = FileName + ".lnk";
                        if (File.Exists(DestinationFile))
                        {
                            FileInfo DestinationInfo = new FileInfo(DestinationFile);
                            DestinationInfo.Attributes = FileAttributes.Normal;
                            File.Delete(DestinationFile);
                            FileInfo RealFile_Info = new FileInfo(FileName);
                            RealFile_Info.Attributes = FileAttributes.Normal;
                        }
                    }
                }
                if (File.Exists(path: Path + "\\MyDocument.exe"))
                {
                    FileInfo Info = new FileInfo(fileName: Path + "\\MyDocument.exe");
                    Info.Attributes = FileAttributes.Normal;
                    File.Delete(Path + "\\MyDocument.exe");
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "杀毒错误");
            }
        }
    }
}
