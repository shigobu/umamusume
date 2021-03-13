# umamusumeAutoSize
ウィンドウが横と縦に切り替わるときに、ウィンドウサイズが初期状態に戻ってしまうアプリで、前のウィンドウサイズを記憶してそのサイズに戻すアプリです。  

なんとなく動作するようになったと思います。

PC版ウマ娘で動作確認をしています。

# ダウンロード方法
右のReleasesからzipでダウンロードできます。適当なフォルダに展開してください。

削除する場合は、展開したフォルダを削除してください。レジストリ等は操作していません。

# 使い方
常駐アプリとして機能します。起動するとタスクトレイに格納されます。終了する場合は、タスクトレイのアイコンを右クリックして終了を選択してください。

このアプリを起動中に、対象アプリ（ウマ娘）を起動します。

対象アプリの縦画面のサイズと位置をいい感じに調整します。

次に、対象アプリを(ライブを見る等で)横画面にします。この時点で、直前の縦画面のサイズと位置が記憶されます。横画面のサイズと位置をいい感じに調整します。

横画面が終了し縦画面になったら、前回記憶した縦画面のサイズと位置に自動で移動します。この時点で、直前の横画面のサイズと位置が記憶されます。

再び、横画面になったら、前回記憶した横画面のサイズと位置に自動で移動します。

このアプリが終了するときに、最後に記録した縦画面のサイズと位置・横画面のサイズと位置がPCに保存されます。次回起動時に読み込まれ、前回の設定が復元されます。

# ウィンドウサイズ変更と移動タイミング
PC版ウマ娘は、ロードが完了したタイミングでウィンドウサイズを初期状態にするようです。なので、ウィンドウサイズ変更等は、ロードが終了するまで待機する必要があります。しかし、外部からロードが完了したかどうかは正確に判断できません。

そこで、音情報を使用するようにしました。無音状態を検知し、次に音が鳴ったタイミングでウィンドウサイズを変更するようにしました。したがって、ウィンドウの縦・横が変わってから、ウィンドウのサイズが変更されるまでに時間がかかります。特に、アプリ起動時は、「サイゲームス」って言うまでウィンドウサイズの変更は行われません。

また、こういった仕様のため、ゲームオプションから音量を変更している場合、正常に動作しないことがあるかもしれません。Windowsの音量ミキサーを変更しても正常に動作することを確認しています。音量調整は、Windows標準機能の音量ミキサーを使用してください。

# 実装予定
ライブを縦画面で開始した場合のサイズ設定。

# 開発環境
VisualStudio 2017  
.NET Framework 4.7  
WPF

# ビルド方法
 [Hardcodet NotifyIcon for WPF](https://github.com/hardcodet/wpf-notifyicon) と [NAudio](https://github.com/naudio/NAudio)を使用しています。NuGetで取得するようにしていますが、参照できていない場合NuGetパッケージを再インストールしてみてください。  

 正常にNuGetパッケージがインストールされたら、ビルドができるはずです。
