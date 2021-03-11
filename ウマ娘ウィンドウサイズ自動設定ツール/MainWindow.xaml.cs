using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Threading;

namespace ウマ娘ウィンドウサイズ自動設定ツール
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        
        /// <summary>
        /// ウマ娘ウィンドウハンドル
        /// </summary>
        IntPtr umemusumeWindowHandle = IntPtr.Zero;

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            //インターバルを100ミリ秒に
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //ウマ娘のウィンドウハンドル取得

        }
    }
}
