namespace UmamusumeAutoSize.Properties {
    
    
    // このクラスでは設定クラスでの特定のイベントを処理することができます:
    //  SettingChanging イベントは、設定値が変更される前に発生します。
    //  PropertyChanged イベントは、設定値が変更された後に発生します。
    //  SettingsLoaded イベントは、設定値が読み込まれた後に発生します。
    //  SettingsSaving イベントは、設定値が保存される前に発生します。
    internal sealed partial class Settings {
        
        public Settings() {
            // // 設定の保存と変更のイベント ハンドラーを追加するには、以下の行のコメントを解除します:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }

        /// <summary>
        /// 前の縦画面の矩形
        /// </summary>
        [System.Configuration.UserScopedSetting()]
        public RECT BeforeVerticalRECT
        {
            get
            {
                return (RECT)(this[nameof(BeforeVerticalRECT)]);
            }
            set
            {
                this[nameof(BeforeVerticalRECT)] = value;
            }
        }

        /// <summary>
        /// 前の横画面の矩形
        /// </summary>
        [System.Configuration.UserScopedSetting()]
        public RECT BeforeHorizontalRECT
        {
            get
            {
                return (RECT)(this[nameof(BeforeHorizontalRECT)]);
            }
            set
            {
                this[nameof(BeforeHorizontalRECT)] = value;
            }
        }

        /// <summary>
        /// 初期横画面の矩形
        /// </summary>
        [System.Configuration.UserScopedSetting()]
        public RECT DefaultHorizontalRECT
        {
            get
            {
                return (RECT)(this[nameof(DefaultHorizontalRECT)]);
            }
            set
            {
                this[nameof(DefaultHorizontalRECT)] = value;
            }
        }

        /// <summary>
        /// 初期縦画面の矩形
        /// </summary>
        [System.Configuration.UserScopedSetting()]
        public RECT DefaultVerticalRECT
        {
            get
            {
                return (RECT)(this[nameof(DefaultVerticalRECT)]);
            }
            set
            {
                this[nameof(DefaultVerticalRECT)] = value;
            }
        }

        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            // SettingChangingEvent イベントを処理するコードをここに追加してください。
        }
        
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            // SettingsSaving イベントを処理するコードをここに追加してください。
        }
    }
}
