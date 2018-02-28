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
                UseDesktop();
                Settings = new Configuration();
            }
            #endregion
            ListUSB();
        }
        
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
            if (listUsb.SelectedItem != null && (string)listUsb.SelectedItem != "等待操作")
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
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode, SetLastError = true)]
        protected static extern int SystemParametersInfo(int uAction, int uParam, StringBuilder lpvParam, int fuWinIni);
        private const int SPI_GETDESKWALLPAPER = 0x0073;

        private void UseDesktop()
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

        #region Code

        ///MainWindow.UDiskOperation.cs

        #endregion
    }
}
