using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
#if DEBUG
#else
            try
            {
                Updater.CheckUpdate();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
#endif
            new MainWindow().Show();
            Close();
        }
        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Position = ((MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1));
        }

#region Check
        [STAThread]
        static void Check()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            Process Instance = GetRunningInstance();
            if (Instance != null)
            {
                HandleRunningInstance(Instance: Instance);
            }

            //System.Windows.Forms.Application.Run(new frmMain());
        }
        
        private static Process GetRunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //遍历与当前进程名称相同的进程列表 
            foreach (Process process in processes)
            {
                //如果实例已经存在则忽略当前进程
                if (process.Id != current.Id)
                {
                    //保证要打开的进程同已经存在的进程来自同一文件路径
                    if (System.Environment.CurrentDirectory.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;
        }

        private static void HandleRunningInstance(Process Instance)
        {
            ShowWindowAsync(Instance.MainWindowHandle, 1); //调用api函数，正常显示窗口
            SetForegroundWindow(Instance.MainWindowHandle); //将窗口放置最前端
            Environment.Exit(0);
        }

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(System.IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(System.IntPtr hWnd);
#endregion
    }
}
