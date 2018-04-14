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
        private DateTime destDateMaster = new DateTime(2018, 6, 17);
        private DateTime destDateSlave = new DateTime(2018, 5, 5).AddHours(8);
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
            if (destDateMaster < DateTime.Now)
            {
                lbMaster.Visibility = System.Windows.Visibility.Hidden;
                lbCountDownMaster.Visibility = System.Windows.Visibility.Hidden;
                val--;
            }
            if(destDateSlave < DateTime.Now)
            {
                lbSlave.Visibility = System.Windows.Visibility.Hidden;
                lbCountDownSlave.Visibility = System.Windows.Visibility.Hidden;
                val--;
            }
            if(val == 0)
            {
                this.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void setRemain(object sender, EventArgs e)
        {
            TimeSpan tsZK = destDateMaster - DateTime.Now;
            TimeSpan tsYM = destDateSlave - DateTime.Now;
            lbCountDownMaster.Content = tsZK.Days + "天 " + tsZK.Hours + "小时 " + tsZK.Minutes + "分钟 " + tsZK.Seconds + "秒";
            lbCountDownSlave.Content = tsYM.Days + "天 " + tsYM.Hours + "小时 " + tsYM.Minutes + "分钟 " + tsYM.Seconds + "秒";
        }
    }
}
