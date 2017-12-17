using System;
using System.Windows;
using System.Windows.Controls;

namespace Quick_Launcher
{
    /// <summary>
    /// Start.xaml 的交互逻辑
    /// </summary>
    public partial class Start : Window
    {
        public Start()
        {
            InitializeComponent();
            try
            {
                Updater.CheckUpdate();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            new MainWindow().Show();
            Close();
        }
        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Position = ((MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1));
        }
    }
}
