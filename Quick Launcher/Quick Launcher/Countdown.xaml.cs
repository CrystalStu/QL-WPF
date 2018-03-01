using System;
//using System.Threading;
using System.Windows.Controls;
using System.Timers;
using System.Windows.Threading;

namespace Quick_Launcher
{
    /// <summary>
    /// Countdown.xaml 的交互逻辑
    /// </summary>
    public partial class Countdown : UserControl
    {
        //private int TimeCountdown;
        //private DateTime Now;
        //private Thread threadTimer;
        private DateTime destDate = new DateTime(2018, 6, 17);
        private DispatcherTimer timeCountDown = new DispatcherTimer();
        public Countdown()
        {
            InitializeComponent();
            timeCountDown.Interval = new TimeSpan(0, 0, 1);
            timeCountDown.Tick += new EventHandler(setRemain);
            timeCountDown.Start();
            /*Now = DateTime.Now;
            TimeCountdown = new DateTime(2018, 6, 17).Subtract(Now).Days + 1;
            CountdownLabel.Content = TimeCountdown.ToString();
            Timer = new Thread( () =>
            {
                if (DateTime.Now != Now)
                {
                    Now = DateTime.Now;
                    TimeCountdown = new DateTime(2018, 6, 17).Subtract(Now).Days + 1;
                }
                Thread.Sleep(30000);    //半分钟一次
            });*/
        }

        private void setRemain(object sender, EventArgs e)
        {
            TimeSpan ts = destDate - DateTime.Now;
            lbCountDown.Content = ts.Days + "天 " + ts.Hours + "小时 " + ts.Minutes + "分钟 " + ts.Seconds + "秒";
        }
    }
}
