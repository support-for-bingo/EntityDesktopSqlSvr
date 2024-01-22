Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

''' <summary>Plain Old CLR Object(POCO)永続化クラス</summary>
''' <remarks></remarks>
Public Class ShohinEntity

#Region "フィールド"
    Private _NumId As Integer
    Private _ShohinCode As Short
    Private _ShohinName As String
    Private _EditDate As Decimal
    Private _EditTime As Decimal
    Private _Note As String
#End Region

#Region "定数"
    Private Const INT As String = "int"
    Private Const SMALLINT As String = "smallint"
    Private Const CHAR2 As String = "char"
    Private Const VARCHAR As String = "varchar"
    Private Const DECIMAL2 As String = "decimal"
    Private Const NUMERIC As String = "numeric"
#End Region

#Region "プロパティ"
    <Key>
    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    <Column("NumId", Order:=0, TypeName:="int")>
    Public Property NumId() As Integer
        Get
            Return Me._NumId
        End Get
        Set(ByVal value As Integer)
            Me._NumId = value
        End Set
    End Property

    <Display(Name:="商品番号")>
    <Column("ShohinNum", Order:=1, TypeName:=SMALLINT)>
    Public Property ShohinCode() As Short
        Get
            Return Me._ShohinCode
        End Get
        Set(ByVal value As Short)
            Me._ShohinCode = value
        End Set
    End Property

    <Display(Name:="商品名")>
    <StringLength(50)>
    <Column("ShohinName", Order:=2, TypeName:=CHAR2)>
    Public Property ShohinName() As String
        Get
            Return Me._ShohinName
        End Get
        Set(ByVal value As String)
            Me._ShohinName = value
        End Set
    End Property

    <DefaultValue("0")> '文字列のデフォルト値は'シングルクォーテーションでくくる。例"'a'"
    <Display(Name:="編集日付")>
    <Required>
    <Range(0, 29991231)>
    <Column("EditDate", Order:=3, TypeName:=DECIMAL2)>
    Public Property EditDate As Decimal '?はNull許容
        Get
            Return Me._EditDate
        End Get
        Set(ByVal value As Decimal)
            Me._EditDate = value
        End Set
    End Property

    <DefaultValue("0")> '文字列のデフォルト値は'シングルクォーテーションでくくる。例"'a'"
    <Display(Name:="編集時刻")>
    <Required>
    <Range(0, 235959)>
    <Column("EditTime", Order:=4, TypeName:=NUMERIC)>
    Public Property EditTime() As Decimal '?はNull許容
        Get
            Return Me._EditTime
        End Get
        Set(ByVal value As Decimal)
            Me._EditTime = value
        End Set
    End Property

    <Display(Name:="備考")>
    <StringLength(255)>
    <Column("Note", Order:=5, TypeName:=VARCHAR)>
    Public Property Note() As String
        Get
            Return Me._Note
        End Get
        Set(ByVal value As String)
            Me._Note = value
        End Set
    End Property
#End Region

End Class