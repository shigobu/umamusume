# umamusumeAutoSize
ウィンドウが横と縦に切り替わるときに、ウィンドウサイズが初期状態に戻ってしまうアプリで、前のウィンドウサイズを記憶してそのサイズに戻すアプリです。  

作成途中です。どうさするか

PC板ウマ娘で動作確認をしています。

# 使い方
常駐アプリとして機能します。
画面は無しにするつもりですが、現状ダミー画面を表示しています。

このアプリを起動中に、対象アプリ（ウマ娘）を起動します。対象アプリの縦初期サイズが記憶されます。

対象アプリの縦画面のサイズと位置をいい感じに調整します。

次に、対象アプリを(ライブを見る等で)横画面にします。この時点で、直前の縦画面のサイズと位置が記憶されます。横画面のサイズと位置をいい感じに調整します。

横画面が終了し縦画面になったら、前回記憶した縦画面のサイズと位置に自動で移動します。この時点で、直前の横画面のサイズと位置が記憶されます。

再び、横画面になったら、前回記憶した横画面のサイズと位置に自動でい移動します。

このアプリが終了するときに、最後に記録した縦画面のサイズと位置・横画面のサイズと位置がPCに保存されます。次回起動時に読み込まれ、前回の設定が復元されます。
