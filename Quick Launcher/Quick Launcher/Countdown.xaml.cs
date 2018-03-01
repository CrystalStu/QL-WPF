using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Quick_Launcher
{
    /// <summary>
    /// Countdown.xaml 的交互逻辑
    /// </summary>
    public partial class Countdown : UserControl
    {
        private int TimeCountdown;
        private DateTime Now;
        private Thread Timer;
        public Countdown()
        {
            InitializeComponent();
            Now = DateTime.Now;
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
            });
        }
    }
}
