using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using NAudio.CoreAudioApi;
using System.Threading;

namespace UmamusumeAutoSize
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        const string umamusumeProcessName = "umamusume";

        DispatcherTimer timer;

        bool IsUmamusumeAppRunning = false;

        /// <summary>
        /// 前のウィンドウサイズ
        /// </summary>
        RECT beforeRECT = new RECT();

        const double tolerance = 1e-6;

        const int timeOut = 10000;

        /// <summary>
        /// 設定ファイルのファイル名
        /// </summary>
        private string SettingFileName
        {
            get
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                string assemblyPath = assembly.Location;
                string assemblyDirectory = Path.GetDirectoryName(assemblyPath);

                return Path.Combine(assemblyDirectory, "setting.xml");
            }
        }

        /// <summary>
        /// 設定データ
        /// </summary>
        private Setting setting = null;

        public MainWindow()
        {
            InitializeComponent();

            //Xmlから設定読み込み
            if (File.Exists(SettingFileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Setting));
                using (StreamReader streamReader = new StreamReader(SettingFileName, new UTF8Encoding(false)))
                {
                    setting = (Setting)serializer.Deserialize(streamReader);
                }
            }
            else
            {
                setting = new Setting();
            }

            timer = new DispatcherTimer();
            //インターバルを設定
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Tick += Timer_Tick;
            timer.Start();

            //バルーンの表示
            taskIcon.ShowBalloonTip("ウマいサイズ", "タスクトレイに格納されました。", Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);

        }

        /// <summary>
        /// ウィンドウが閉じたときのイベント
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            //XmlSerializerオブジェクトを作成
            XmlSerializer serializer = new XmlSerializer(typeof(Setting));
            using (StreamWriter streamWriter = new StreamWriter(SettingFileName, false, new UTF8Encoding(false)))
            {
                //シリアル化し、XMLファイルに保存する
                serializer.Serialize(streamWriter, setting);
            }
        }

        /// <summary>
        /// タイマーティックイベント
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            //ウマ娘のウィンドウハンドル取得
            Process[] process = Process.GetProcessesByName(umamusumeProcessName);

            //アプリの終了を検知した場合
            if (process.Length == 0)
            {
                if (IsUmamusumeAppRunning)
                {
                    double aspectRatioBefore = (double)beforeRECT.Height / beforeRECT.Width;
                    if (aspectRatioBefore > 1)
                    {
                        //縦
                        setting.BeforeVerticalRECT = beforeRECT;
                    }
                    else
                    {
                        //横
                        setting.BeforeHorizontalRECT = beforeRECT;
                    }
                }
                IsUmamusumeAppRunning = false;
                return;
            }

            Process umamusumeProcess = process.First();

            bool error = Win32api.GetWindowRect(umamusumeProcess.MainWindowHandle, out RECT rect);

            //起動を検知したら、フラグセット。
            //Windowの大きさを設定して終了。
            if (!IsUmamusumeAppRunning)
            {
                WaitUmaWindowAvailable(umamusumeProcess, timeOut);

                IsUmamusumeAppRunning = true;

                MoveUmamusumeWindow(umamusumeProcess.MainWindowHandle, setting.BeforeVerticalRECT);
                return;
            }

            //サイズ変更が行われた
            if (rect != beforeRECT)
            {
                //ウィンドウの縦横比が変わったら、大きさ変更
                double aspectRatioCurrent = (double)rect.Height / rect.Width;
                double aspectRatioBefore = (double)beforeRECT.Height / beforeRECT.Width;

                //浮動小数点の比較は、誤差があるので差の絶対値を求めて、誤差以上の場合等しくない。
                if (Math.Abs(aspectRatioCurrent - aspectRatioBefore) > tolerance)
                {
                    //横画面になった時
                    if (aspectRatioCurrent < 1 && aspectRatioBefore > 1)
                    {
                        setting.BeforeVerticalRECT = beforeRECT;

                        WaitUmaWindowAvailable(umamusumeProcess, timeOut);
                        MoveUmamusumeWindow(umamusumeProcess.MainWindowHandle, setting.BeforeHorizontalRECT);
                    }
                    //縦長になった時
                    else if(aspectRatioBefore < 1 && aspectRatioCurrent > 1)
                    {
                        setting.BeforeHorizontalRECT = beforeRECT;

                        WaitUmaWindowAvailable(umamusumeProcess, timeOut);
                        MoveUmamusumeWindow(umamusumeProcess.MainWindowHandle, setting.BeforeVerticalRECT);
                    }
                }
            }

            beforeRECT = rect;
        }

        /// <summary>
        /// 指定ウィンドウの移動とサイズ変更を行います。
        /// </summary>
        /// <param name="windowHandle">ウィンドウハンドル</param>
        /// <param name="rect">RECTオブジェクト</param>
        private void MoveUmamusumeWindow(IntPtr windowHandle, RECT rect)
        {
            if (rect == new RECT())
            {
                return;
            }
            Win32api.MoveWindow(windowHandle, rect.X, rect.Y, rect.Width, rect.Height, false);
        }

        /// <summary>
        /// 終了メニュークリック時
        /// </summary>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// ウマ娘ウィンドウの音量が０になって再び０以上になるまで待機します。
        /// </summary>
        /// <param name="timeOutMillisecond">タイムアウト時間ミリ秒</param>
        private void WaitUmaWindowAvailable(Process process, int timeOutMillisecond)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            MMDevice device = null;
            try
            {
                //ウマ娘セッションの検索
                AudioSessionControl umamusumeSession = null;
                while (umamusumeSession == null)
                {
                    using (MMDeviceEnumerator DevEnum = new MMDeviceEnumerator())
                    {
                        device?.Dispose();
                        device = DevEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                    }
                    AudioSessionManager sessionManager = device.AudioSessionManager;
                    var sessions = sessionManager.Sessions;
                    for (int j = 0; j < sessions.Count; j++)
                    {
                        if (sessions[j].GetProcessID == process.Id)
                        {
                            umamusumeSession = sessions[j];
                            break;
                        }
                    }
                }

                //0になるまで待機(0以外の間ループ)
                while (umamusumeSession.AudioMeterInformation.MasterPeakValue > 0.005)
                {
                    Thread.Sleep(10);
                    if (stopwatch.ElapsedMilliseconds > timeOutMillisecond)
                    {
                        return;
                    }
                }

                //0以上になるまで待機(0の間ループ)
                while (umamusumeSession.AudioMeterInformation.MasterPeakValue < 0.1)
                {
                    Thread.Sleep(10);
                    if (stopwatch.ElapsedMilliseconds > timeOutMillisecond)
                    {
                        return;
                    }
                }
            }
            finally
            {
                if (device != null)
                {
                    device.Dispose();
                }
            }

        }

    }
}
