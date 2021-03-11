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
using System.Diagnostics;

namespace ウマ娘ウィンドウサイズ自動設定ツール
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
		string umamusumeProcessName = "umamusume";

		DispatcherTimer timer;

		bool IsUmamusumeAppRunning = false;

		/// <summary>
		/// 前のウィンドウサイズ
		/// </summary>
		RECT beforeRECT = new RECT();
		/// <summary>
		/// 前の縦長ウィンドウサイズ
		/// </summary>
		RECT beforeVerticalRECT = new RECT();
		/// <summary>
		/// 前の横長ウィンドウサイズ
		/// </summary>
		RECT beforeHorizontalRECT = new RECT();

		RECT DefaultHorizontalRECT { get; } = new RECT(306, 157, 1613, 922);
		RECT DefaultVerticalRECT { get; } = new RECT(748,157, 1172, 922);

		const double tolerance = 1e-6;

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            //インターバルを100ミリ秒に
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Timer_Tick;
			timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
			//ウマ娘のウィンドウハンドル取得
			Process[] process = Process.GetProcessesByName(umamusumeProcessName);

			//アプリ起動中で無い時は、フラグクリアして終了
			if (process.Length == 0)
			{
				IsUmamusumeAppRunning = false;
				return;
			}

			Process umamusumeProcess = process.First();

			//起動を検知したら、フラグセット。
			//Windowの大きさを設定して終了。
			if (!IsUmamusumeAppRunning)
			{
				IsUmamusumeAppRunning = true;
				MoveUmamusumeWindow(umamusumeProcess.MainWindowHandle, beforeVerticalRECT);
				return;
			}

			bool error = Win32api.GetWindowRect(umamusumeProcess.MainWindowHandle, out RECT rect);

			//ウィンドウの縦横比が変わったら、大きさ変更
			double aspectRatioCurrent = (double)rect.Height / rect.Width;
			double aspectRatioBefore = (double)beforeRECT.Height / beforeRECT.Width;

			//浮動小数点の比較は、誤差があるので差の絶対値を求めて、誤差以上の場合等しくない。
			if (Math.Abs(aspectRatioCurrent - aspectRatioBefore) > tolerance)
			{
				//横画面になった時
				if (aspectRatioCurrent < 1 && aspectRatioBefore > 1)
				{
					beforeVerticalRECT = beforeRECT;
					MoveUmamusumeWindow(umamusumeProcess.MainWindowHandle, beforeHorizontalRECT);
				}
				//縦長になった時
				else if(aspectRatioBefore < 1 && aspectRatioCurrent > 1)
				{
					beforeHorizontalRECT = beforeRECT;
					MoveUmamusumeWindow(umamusumeProcess.MainWindowHandle, beforeVerticalRECT);
				}
			}
			else
			{
				//デフォルトサイズに戻ってしまった場合、前のサイズに戻す
				if (rect == DefaultVerticalRECT && beforeRECT != DefaultVerticalRECT
					|| rect == DefaultHorizontalRECT && beforeRECT != DefaultHorizontalRECT)
				{
					MoveUmamusumeWindow(umamusumeProcess.MainWindowHandle, beforeRECT);
				}
			}


			beforeRECT = rect;
		}

		/// <summary>
		/// 指定のウィンドウの移動とサイズ変更を行います。
		/// </summary>
		/// <param name="windowHandle">ウィンドウハンドル</param>
		/// <param name="rect">RECTオブジェクト</param>
		private void MoveUmamusumeWindow(IntPtr windowHandle, RECT rect)
		{
			if (rect == new RECT())
			{
				return;
			}
			System.Threading.Thread.Sleep(200);
			Win32api.MoveWindow(windowHandle, rect.X, rect.Y, rect.Width, rect.Height, false);
		}

	}
}
