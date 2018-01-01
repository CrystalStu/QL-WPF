using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
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
            #region 初始化config
            string path = Environment.CurrentDirectory + @"\configuration.xml";
            if (File.Exists(path))
            {
                using (var stream = File.OpenRead(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
                    Settings = (Configuration)serializer.Deserialize(stream);
                }
                if (Settings.BackgroundSourceString == "none") ;
                else if (File.Exists(Settings.BackgroundSourceString))
                {
                    _MainWindow.Background = new ImageBrush(new System.Windows.Media.Imaging.BitmapImage(Settings.BackgroundSource));
                    MainGird.Children.Remove(Chara);
                }
                else
                {
                    MessageBox.Show("背景图片出错。。。\n       将恢复默认。");
                    Settings.BackgroundSourceString = "none";
                }
            }
            else
            {
                Settings = new Configuration();
                MessageBox.Show("configuration.xml缺失...请联系系统管理员(网管)\n     (如果你们班网管没有参与本项目那我也没办法了-callG");
            }
#endregion
        }

        #region UI
        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            new About().Show();
        }
        private void SettingsButton_Cilck(object sender, RoutedEventArgs e)
        {
            new Settings(Settings).Show();
        }
        private void _MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //序列化Settings
        }
        #endregion

        #region code
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
                MessageBox.Show(e.Message, "杀毒错误");
            }
        }
        #endregion
    }
}
