using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Quick_Launcher
{
    /// <summary>
    /// Countdown.xaml 的交互逻辑
    /// </summary>
    public partial class Countdown : UserControl
    {
        private DateTime destDateMaster = new DateTime(2018, 6, 17);
        private DateTime destDateSlave = new DateTime(2018, 5, 11).AddHours(6).AddMinutes(30);
        private DispatcherTimer timeCountDown = new DispatcherTimer();

        private SolidColorBrush whiteBrush = new SolidColorBrush(Colors.White);
        private SolidColorBrush orangeRedBrush = new SolidColorBrush(Colors.OrangeRed);
        private SolidColorBrush indianRedBrush = new SolidColorBrush(Colors.IndianRed);
        private SolidColorBrush darkRedBrush = new SolidColorBrush(Colors.DarkRed);

        public Countdown()
        {
            InitializeComponent();
            orangeRedBrush.Opacity = 0.8;
            indianRedBrush.Opacity = 0.8;
            darkRedBrush.Opacity = 0.8;
            timeCountDown.Interval = new TimeSpan(0, 0, 1);
            timeCountDown.Tick += new EventHandler(setRemain);
            timeCountDown.Start();
        }

        private void checkValid()
        {
            int val = 2;
            if (destDateMaster < DateTime.Now)
            {
                lbMaster.Visibility = System.Windows.Visibility.Hidden;
                lbCountDownMaster.Visibility = System.Windows.Visibility.Hidden;
                val--;
            }
            if (destDateSlave < DateTime.Now)
            {
                lbSlave.Visibility = System.Windows.Visibility.Hidden;
                lbCountDownSlave.Visibility = System.Windows.Visibility.Hidden;
                val--;
                this.Height = 120;
            }
            else lbSlave.Visibility = System.Windows.Visibility.Visible;
            if (val == 0)
            {
                this.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void checkColor(ref TimeSpan tsMaster, ref TimeSpan tsSlave)
        {
            if (tsMaster.Days < 40)
            {
                if (!(tsMaster.Days < 30)) gridCountdown.Background = orangeRedBrush;
                lbCountDownMaster.Foreground = whiteBrush;
                lbCountDownSlave.Foreground = whiteBrush;
                lbMaster.Foreground = whiteBrush;
                lbSlave.Foreground = whiteBrush;
            }
            if (tsMaster.Days < 30)
            {
                if (!(tsMaster.Days < 15)) gridCountdown.Background = indianRedBrush;
                gridCountdown.Background = indianRedBrush;
            }
            if (tsMaster.Days < 15)
            {
                gridCountdown.Background = darkRedBrush;
            }
            if (tsSlave.Days < 3 && tsSlave.Days > 0 && tsMaster.Days >= 30)
            {
                gridCountdown.Background = orangeRedBrush;
            }
        }

        private void setRemain(object sender, EventArgs e)
        {
            TimeSpan tsMaster = destDateMaster - DateTime.Now;
            TimeSpan tsSlave = destDateSlave - DateTime.Now;
            lbCountDownMaster.Content = tsMaster.Days + "天 " + tsMaster.Hours + "小时 " + tsMaster.Minutes + "分钟 " + tsMaster.Seconds + "秒";
            lbCountDownSlave.Content = tsSlave.Days + "天 " + tsSlave.Hours + "小时 " + tsSlave.Minutes + "分钟 " + tsSlave.Seconds + "秒";
            checkColor(ref tsMaster, ref tsSlave);
            checkValid();
        }
    }
}
