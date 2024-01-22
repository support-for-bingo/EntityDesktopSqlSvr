''' <summary>最新の例外格納クラス</summary>
''' <remarks></remarks>
Public MustInherit Class LastException

#Region "フィールド"
    Protected Shared _LastExcepTitle As String
    Protected Shared _LastExcepPlace As String
    Protected Shared _LastExcepParam As String
    Protected Shared _LastExcepMessage As String
    Protected Shared _LastExcepTrace As String
#End Region

#Region "メソッド"
    ''' <summary>最新の例外情報をセットします</summary>
    ''' <param name="method">例外のメソッド名</param>
    ''' <param name="param">例外の発生したメソッドが引き受けた引数、参考値など</param>
    ''' <param name="ex">Exceptionまたは派生クラス</param>
    Public Shared Sub SetLastException(ByVal method As String, ByVal param As String, ByVal ex As Exception)

        _LastExcepTitle = ex.GetType().ToString()
        _LastExcepPlace = method
        _LastExcepParam = param
        _LastExcepMessage = ex.Message
        _LastExcepTrace = ex.StackTrace

    End Sub

    ''' <summary>例外ログの書き込み</summary>
    ''' <remarks></remarks>
    Public Shared Sub LogWrite()

        Dim LogStr = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & ":" & _LastExcepTitle + vbCrLf
        LogStr += "例外メッセージ：" & _LastExcepMessage + vbCrLf
        LogStr += "スタックトレース：" & _LastExcepTrace & vbCrLf
        IO.File.AppendAllText("Exceptionlog.txt", LogStr, Text.Encoding.Default)

    End Sub
#End Region

#Region "プロパティ"
    ''' <summary>最新の例外のクラス名が格納されます</summary>
    ''' <returns></returns>
    Public Shared Property LastExcepTitle As String
        Get
            Return _LastExcepTitle
        End Get
        Set(ByVal value As String)
            _LastExcepTitle = value
        End Set
    End Property

    ''' <summary>最新の例外のメソッド名が格納されます</summary>
    ''' <returns></returns>
    Public Shared Property LastExcepPlace As String
        Get
            Return _LastExcepPlace
        End Get
        Set(ByVal value As String)
            _LastExcepPlace = value
        End Set
    End Property

    ''' <summary>最新の例外のパラメータが格納されます
    ''' <para>(メソッドに与えた引数や参考になる情報など)</para></summary>
    ''' <returns></returns>
    Public Shared Property LastExcepParam As String
        Get
            Return _LastExcepParam
        End Get
        Set(ByVal value As String)
            _LastExcepParam = value
        End Set
    End Property

    ''' <summary>最新の例外のメッセージが格納されます</summary>
    ''' <returns></returns>
    Public Shared Property LastExcepMessage As String
        Get
            Return _LastExcepMessage
        End Get
        Set(ByVal value As String)
            _LastExcepMessage = value
        End Set
    End Property

    ''' <summary>最新の例外のスタックトレースが格納されます</summary>
    ''' <returns></returns>
    Public Shared Property LastExcepTrace As String
        Get
            Return _LastExcepTrace
        End Get
        Set(ByVal value As String)
            _LastExcepTrace = value
        End Set
    End Property
#End Region

End Class