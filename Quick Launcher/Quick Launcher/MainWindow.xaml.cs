using System.IO;
using System.Windows;
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
                //OnPropertyChanged("Settings");
            }
        }
        public MainWindow()
        {
            #region 初始化config
            string path = System.Environment.CurrentDirectory + @"\configuration.xml";
            if (File.Exists(path))
            {
                using (var stream = File.OpenRead(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
                    Settings = (Configuration)serializer.Deserialize(stream);
                }
            }
            else
            {
                Settings = new Configuration();
                Settings.Background = System.Environment.CurrentDirectory + @"\Background.png";
                MessageBox.Show("configuration.xml缺失...请联系系统管理员(网管)\n     (如果你们班网管没有参与本项目开fa那我也没办法了-callG");
            }
            #endregion
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new About().Show();
        }
    }
}
