Imports System.Data.Entity

''' <summary>商品データ/コンテキスト</summary>
''' <remarks>NuGetパッケージ: Install-Package EntityFramework -Version 6.4.4</remarks>
Public Class ShohinContext
    Inherits DbContext

#Region "フィールド"
    Private _ShohinData As DbSet(Of ShohinEntity)
#End Region

#Region "コンストラクタ"
    ''' <summary>App.configやWeb.configの接続文字列により接続する方法</summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New("ShohinContext")
        '自動マイグレーション
        Database.SetInitializer(New MigrateDatabaseToLatestVersion(Of ShohinContext, Configuration))
    End Sub

    ''' <summary>App.configやWeb.configを使わずこのコンストラクタの呼び出し側から接続文字列を指定する方法
    ''' (DbConnectionクラスのConnectionStringプロパティに接続文字列を代入したものを引数としてください。)</summary>
    ''' <param name="Cnstring"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal Cnstring As Common.DbConnection)

        MyBase.New(Cnstring, True)

    End Sub
#End Region

#Region "デストラクタ"
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
#End Region

#Region "メソッド"
    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)

        'Fluent API
        modelBuilder.HasDefaultSchema("dbo") 'デフォルトスキーマ
        'このバージョンでのDecimal、Numeric型のColumn属性では桁数は同時に指定できない。指定しないと桁数が自動的に(18,2)となってしまうためここで設定
        modelBuilder.Entity(Of ShohinEntity)().Property(Function(x) x.EditDate).HasPrecision(8, 0)
        modelBuilder.Entity(Of ShohinEntity)().Property(Function(x) x.EditTime).HasPrecision(6, 0)
        'modelBuilder.Conventions.Remove()
        'modelBuilder.Conventions.Add(New ModelConfiguration.Conventions.DecimalPropertyConvention(8, 0))
        MyBase.OnModelCreating(modelBuilder)

    End Sub
#End Region

#Region "プロパティ"
    Public Property ShohinData() As DbSet(Of ShohinEntity)
        Get
            Return Me._ShohinData
        End Get
        Set(ByVal value As DbSet(Of ShohinEntity))
            Me._ShohinData = value
        End Set
    End Property
#End Region

End Class

''' <summary>データベース自動マイグレーションのための設定クラス</summary>
''' <remarks></remarks>
Friend NotInheritable Class Configuration
    Inherits Migrations.DbMigrationsConfiguration(Of ShohinContext)

#Region "コンストラクタ"
    Public Sub New()

        Dim GetMyApplication As String = My.Application.Info.AssemblyName
        Dim GetMyClass As String = Me.[GetType]().Name

        AutomaticMigrationsEnabled = True
        AutomaticMigrationDataLossAllowed = True
        ContextKey = GetMyApplication & "." & GetMyClass

    End Sub
#End Region

End Class