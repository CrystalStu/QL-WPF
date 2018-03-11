using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Quick_Launcher
{
    /// <summary>
    /// Countdown.xaml 的交互逻辑
    /// </summary>
    public partial class Countdown : UserControl
    {
        private DateTime destDateZK = new DateTime(2018, 6, 17);
        private DateTime destDateYM = new DateTime(2018, 4, 13);
        private DispatcherTimer timeCountDown = new DispatcherTimer();
        public Countdown()
        {
            InitializeComponent();
            checkValid();
            timeCountDown.Interval = new TimeSpan(0, 0, 1);
            timeCountDown.Tick += new EventHandler(setRemain);
            timeCountDown.Start();
        }

        private void checkValid()
        {
            int val = 2;
            if (destDateZK < DateTime.Now)
            {
                lbZK.Visibility = System.Windows.Visibility.Hidden;
                lbCountDownZK.Visibility = System.Windows.Visibility.Hidden;
                val--;
            }
            if(destDateYM < DateTime.Now)
            {
                lbYM.Visibility = System.Windows.Visibility.Hidden;
                lbCountDownYM.Visibility = System.Windows.Visibility.Hidden;
                val--;
            }
            if(val == 0)
            {
                this.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void setRemain(object sender, EventArgs e)
        {
            TimeSpan tsZK = destDateZK - DateTime.Now;
            TimeSpan tsYM = destDateYM - DateTime.Now;
            lbCountDownZK.Content = tsZK.Days + "天 " + tsZK.Hours + "小时 " + tsZK.Minutes + "分钟 " + tsZK.Seconds + "秒";
            lbCountDownYM.Content = tsYM.Days + "天 " + tsYM.Hours + "小时 " + tsYM.Minutes + "分钟 " + tsYM.Seconds + "秒";
        }
    }
}
