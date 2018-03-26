using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Quick_Launcher
{
    /// <summary>
    /// Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        private static string stat = String.Empty;
        private static bool complete = false;

        public Start()
        {
            InitializeComponent();
            Thread cte_thr = new Thread(changeTextEve);
            cte_thr.Start();
#if DEBUG
            Hide();
            ShowMW();
#else
            new Thread(Update).Start();
#endif
        }

        #region Event Triggers
        private void ShowMW()
        {
            Process instance = GetRunningInstance();
            if (instance != null) HandleRunningInstance(instance);
            new MainWindow().Show();
        }

        private void Update()
        {
            try
            {
                Updater.CheckUpdate();
            }
            catch (Exception e)
            {
                changeText(e.Message);
                Thread.Sleep(1000);
            }
            this.Dispatcher.Invoke((Action)(() =>
            {   // This refer to the window in the WPF application.
                complete = true;
                Hide();
                ShowMW();
            }));
        }

        private void changeTextEve()
        {
            while (!complete)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {   // This refer to the window in the WPF application.
                    loadAnim.Visibility = Visibility.Visible;
                    lb_stat.Content = stat;
                }));
            }
        }
        #endregion

        #region UI
        public static void changeText(string text)
        {
            stat = text;
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Position = ((MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1));
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        #endregion

        #region Check
        private static Process GetRunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            // Foreach the same process name.
            foreach (Process process in processes)
            {
                // Ignore current process if an instance exists.
                if (process.Id != current.Id)
                {
                    // Same directory of the running instance.
                    if (Environment.CurrentDirectory.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        return process;
                    }
                }
            }
            return null;
        }

        private static void HandleRunningInstance(Process Instance)
        {
            ShowWindowAsync(Instance.MainWindowHandle, 1); // Display the window normally.
            SetForegroundWindow(Instance.MainWindowHandle); // Set the window topmost.
            Environment.Exit(0);
        }

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(System.IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(System.IntPtr hWnd);
        #endregion
    }
}
