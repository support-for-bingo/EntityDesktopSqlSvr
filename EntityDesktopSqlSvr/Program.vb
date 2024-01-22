Public Class Program
    Inherits LastException

    <STAThread()>
    Shared Sub Main()
        'Windowsフォームアプリケーション用集約例外ハンドラーの定義
        AddHandler Application.ThreadException, AddressOf Application_ThreadException

        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New Form1())
    End Sub

    '集約例外イベントプロシージャー
    Shared Sub Application_ThreadException(sender As Object, e As Threading.ThreadExceptionEventArgs)
        '例外の内容やトレース内容をLogに出力したい場合やユーザーに画面出力したい場合にここへ書きます。

        _LastExcepTitle = e.Exception.GetType().ToString()
        _LastExcepPlace = Reflection.MethodBase.GetCurrentMethod().Name
        _LastExcepParam = ""
        _LastExcepMessage = e.Exception.Message
        _LastExcepTrace = e.Exception.StackTrace

        '例外ログ書き込み
        LogWrite()

        'メッセージダイアログ
        MessageBox.Show("アプリケーションエラーが起きました。アプリケーションを終了します。" & vbCrLf &
                "メッセージ：" & e.Exception.GetType.ToString & vbCrLf & "スタックトレース：" & e.Exception.StackTrace)

        'アプリケーションの終了
        Application.Exit()
    End Sub

End Class