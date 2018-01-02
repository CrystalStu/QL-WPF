using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Quick_Launcher
{
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : Window
    {
        public Configuration configuration;
        public bool cancel = false;
        public Settings(Configuration _configuration)
        {
            configuration = _configuration;
            InitializeComponent();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            cancel = true;
            Close();
        }

        private void Set(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        bool isImage(string path)
        {
            string[] s = path.Split('.');
            string FileExtension = s[s.Length - 1];
            switch (FileExtension)
            {
                case "png":
                    return true;
                case "jpg":
                    return true;
                case "jepg":
                    return true;
                case "gif":
                    return true;
                default:
                    return false;
            }

        }
        void TextBlock_DragOver(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length == 1)
                    BackgroundPath.Text = isImage(files[0]) ? files[0] : "这不是图像文件";
            foreach (string file in files)
            {
                //dosomething
            }
        }

        /// <summary>
        /// 窗口拖动事件
        /// </summary>
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
