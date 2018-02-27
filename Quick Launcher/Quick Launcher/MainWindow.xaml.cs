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
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Configuration _settings;
        public Configuration Settings
        {
            get { return _settings; }
            set
            {
                _settings = value;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            #region Initalize Basements
            this.ResizeMode = ResizeMode.NoResize;
            Timer time4refreshDesktop = new Timer();
            time4refreshDesktop.Enabled = false;
            time4refreshDesktop.Interval = 50;
            listUsb.SelectionMode = System.Windows.Controls.SelectionMode.Single;
            #endregion
            #region Initalize Configure File
            string path = Environment.CurrentDirectory + @"\configuration.xml";
            if (File.Exists(path))
            {
                using (var stream = File.OpenRead(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
                    Settings = (Configuration)serializer.Deserialize(stream);
                }
                _MainWindow.Background = new ImageBrush(new System.Windows.Media.Imaging.BitmapImage(Settings.BackgroundSource));
            }
            else
            {
                time4refreshDesktop.Enabled = true;
                useDesktop();
                Settings = new Configuration();
            }
            #endregion
            ListUSB();
        }

        #region UI
        #region Objects
        private void BtOptions_Click(object sender, RoutedEventArgs e)
        {
            new Settings(Settings).ShowDialog();
        }

        private void bt_about_Click(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }

        private void Label_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void _MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 序列化Settings
        }

        private void bt_usb_clear_Click(object sender, RoutedEventArgs e)
        {
            listUsb.SelectedIndex = -1;
            bt_usb_browse.IsEnabled = false;
            bt_usb_eject.IsEnabled = false;
            bt_open_all.IsEnabled = false;
        }

        private void bt_usb_public_Click(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo("explorer.exe");
            psi.Arguments = "/root, " + System.Windows.Forms.Application.StartupPath + "\\Folder";
            Process.Start(psi);
        }

        private void bt_usb_browse_Click(object sender, RoutedEventArgs e)
        {
            if (listUsb.SelectedItem == null) PleaseSelect();
            else
            {
                KillMyDocument(listUsb.SelectedItem.ToString()[0] + ":\\");
                ProcessStartInfo psi = new ProcessStartInfo("explorer.exe");
                psi.Arguments = "/root, " + listUsb.SelectedItem.ToString()[0] + ":";
                Process.Start(psi);
            }
        }

        private void bt_usb_eject_Click(object sender, RoutedEventArgs e)
        {
            if (listUsb.SelectedItem == null) PleaseSelect();
            else
            {
                Ejection ejectarget = new Ejection(listUsb.SelectedItem.ToString()[0] + ":");
                bool result = ejectarget.Eject();
                //if (result) MessageBox.Show("Operation Succeed!", "Successful Ejection", MessageBoxButtons.OK, MessageBoxIcon.Information); else MessageBox.Show("Failed, please try again.\r\nIf you have not remove this drive after ejecting it, it can also cause this problem.", "Ejection Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (result)
                {
                    listUsb.Items.Clear();
                    listUsb.Items.Add("等待操作");
                    System.Windows.Forms.MessageBox.Show("操作成功！", "弹出可移动磁盘", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bt_usb_browse.IsEnabled = false;
                    bt_usb_eject.IsEnabled = false;
                    bt_open_all.IsEnabled = false;
                }
                else System.Windows.Forms.MessageBox.Show("失败，请重试。\r\n请尝试关闭正在运行的程序。\r\n如果已经弹出，又没有移除，也会造成此问题。", "弹出可移动磁盘", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void listUsb_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (listUsb.SelectedItem != null && listUsb.SelectedItem != "等待操作")
            {
                bt_usb_browse.IsEnabled = true;
                bt_usb_eject.IsEnabled = true;
                bt_open_all.IsEnabled = true;
                bt_usb_clear.IsEnabled = true;
            }
        }

        private void bt_open_all_Click(object sender, RoutedEventArgs e)
        {
            bt_usb_browse_Click(sender, e);
            bt_usb_public_Click(sender, e);
        }
        #endregion
        #region Sundry
        #region Windows Desktop
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern int SystemParametersInfo(int uAction, int uParam, StringBuilder lpvParam, int fuWinIni);
        private const int SPI_GETDESKWALLPAPER = 0x0073;

        private void useDesktop()
        {
            ImageBrush backgroundImageBrush = new ImageBrush(new System.Windows.Media.Imaging.BitmapImage(new Uri(getDesktop())));
            grid_background.Background = backgroundImageBrush;
            grid_background.Effect = new BlurEffect()
            {
                Radius = 8
            };
            // _MainWindow.Background = backgroundImageBrush;
        }
        private string getDesktop()
        {
            //定义存储缓冲区大小
            StringBuilder s = new StringBuilder(300);
            //获取Window 桌面背景图片地址，使用缓冲区
            SystemParametersInfo(SPI_GETDESKWALLPAPER, 300, s, 0);
            //缓冲区中字符进行转换
            string wallpaper_path = s.ToString(); //系统桌面背景图片路径
            return wallpaper_path;
        }
        #endregion
        #endregion
        #endregion

        #region Code

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
                                    listUsb.Items.Clear();
                                    bt_usb_browse.IsEnabled = false;
                                    bt_usb_eject.IsEnabled = false;
                                    bt_open_all.IsEnabled = false;
                                    ListUSB();
                                }
                            }
                            break;
                        case DBT_DEVICEQUERYREMOVE:
                            listUsb.Items.Clear();
                            ListUSB();
                            break;
                        case DBT_DEVICEREMOVECOMPLETE:
                            listUsb.Items.Clear();
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
            System.IO.DriveInfo[] disk = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in disk)
            {
                if (drive.DriveType.GetHashCode() == 2)
                {
                    listUsb.Items.Add(drive.Name.ToString()[0] + ":  " + drive.VolumeLabel + "  " + drive.GetTotalFreeSpace() + "/" + drive.GetTotalSize());
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
            useDesktop();
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

        #endregion
    }
}
