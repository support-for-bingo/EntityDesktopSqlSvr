Imports System.Text.RegularExpressions
Public Class Form1
    Friend WithEvents LabelDic As New Dictionary(Of String, Label)()
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents RichTextBox1 As RichTextBox
    Friend WithEvents LabelNumId As Label
    Friend WithEvents LabelFoot As Label
    Friend WithEvents TextBoxShohinNum As TextBox
    Friend WithEvents TextBoxShohinName As TextBox
    Friend WithEvents TextBoxNote As TextBox
    Friend WithEvents ButtonQuery As Button
    Friend WithEvents ButtonInsert As Button
    Friend WithEvents ButtonUpdate As Button
    Friend WithEvents ButtonDelete As Button

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

        Call FormDesignSetting()

    End Sub

    ''' <summary>商品を全件表示します。</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ButtonQuery_Click(sender As Object, e As EventArgs) Handles ButtonQuery.Click

        'AppConfigを使わずDbConnectionを使うやり方
        'Dim connection = Common.DbProviderFactories.GetFactory("System.Data.SqlClient").CreateConnection()
        'connection.ConnectionString = "Server=lpc:(local)\SQLEXPRESS;Initial Catalog=EntitySample;Integrated Security=True"

        ''AppConfigを使わずSystem.Data.Entity.DatabaseクラスのDefaultConnectionFactoryプロパティに登録するやり方
        'Dim sb As New SqlClient.SqlConnectionStringBuilder()
        'Dim factory As New System.Data.Entity.Infrastructure.SqlConnectionFactory(sb.ConnectionString)
        'System.Data.Entity.Database.DefaultConnectionFactory = factory

        Using context As New ShohinContext()
            If context.ShohinData.Any() = False Then
                ' すでにデータが存在するなら追加しない。
                Call InitialData(context)
            End If

            If TextBoxShohinNum.Text = "" Then
                'For Each data As SyohinEntity In context.SyohinData 'DataReaderみたいな物？　データベースが存在しなければ初回ここで時間が掛かる
                '    RichTextBox1.AppendText(data.SyohinCode & data.SyohinName & data.EditDate & data.EditTime & data.Note & vbCrLf)
                'Next
                DataGridView1.DataSource = context.ShohinData.ToList()
                RichTextBox1.AppendText("全件表示しました。" & vbCrLf)
            Else
                'For Each data As SyohinEntity In context.SyohinData.Where(Function(x) x.SyohinCode = TextBoxSyohinCode.Text).ToArray()
                '    RichTextBox1.AppendText(data.SyohinCode & data.SyohinName & data.EditDate & data.EditTime & data.Note & vbCrLf)
                'Next
                DataGridView1.DataSource = context.ShohinData.Where(Function(x) x.ShohinCode = TextBoxShohinNum.Text).ToList()
                RichTextBox1.AppendText("商品コードの条件による抽出を行いました。" & vbCrLf)
            End If

            Call DataGridSetting()
        End Using

        Call TextBoxClear()

    End Sub

    ''' <summary>テキストボックスによる内容で商品を追加します。</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ButtonInsert_Click(sender As Object, e As EventArgs) Handles ButtonInsert.Click

        If Regex.IsMatch(TextBoxShohinNum.Text, "^[0-9]{1,4}$") = False Then
            MessageBox.Show("商品番号は半角数値の0～9999でなければなりません。", "メッセージ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Using context As New ShohinContext()
            context.ShohinData.Add(New ShohinEntity With {
                    .ShohinCode = TextBoxShohinNum.Text,
                    .ShohinName = TextBoxShohinName.Text,
                    .EditDate = Format(Now, "yyyyMMdd"),
                    .EditTime = Format(Now, "HHmmss"),
                    .Note = TextBoxNote.Text})
            context.SaveChanges()
        End Using
        RichTextBox1.AppendText("商品を追加しました。" & vbCrLf)

    End Sub

    ''' <summary>商品ID(NumId)による商品の更新を行います。</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ButtonUpdate_Click(sender As Object, e As EventArgs) Handles ButtonUpdate.Click

        If DataGridView1.Rows.Count <= 0 Or LabelNumId.Text = "" Then
            MessageBox.Show("削除する商品行が選択がされていません", "商品IDなし", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Regex.IsMatch(TextBoxShohinNum.Text, "^[0-9]{1,4}$") = False Then
            MessageBox.Show("商品番号は半角数値の0～9999でなければなりません。", "メッセージ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Using context As New ShohinContext()
            Dim data = context.ShohinData.Single(Function(x) x.NumId = LabelNumId.Text)
            data.ShohinCode = TextBoxShohinNum.Text
            data.ShohinName = TextBoxShohinName.Text
            data.EditDate = Format(Now, "yyyyMMdd")
            data.EditTime = Format(Now, "HHmmss")
            data.Note = TextBoxNote.Text
            context.SaveChanges()
        End Using
        RichTextBox1.AppendText("商品ID：" & LabelNumId.Text & "のレコードを更新しました。" & vbCrLf)

    End Sub

    ''' <summary>商品ID(NumId)による商品を削除します。</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ButtonDelete_Click(sender As Object, e As EventArgs) Handles ButtonDelete.Click

        If DataGridView1.Rows.Count <= 0 Or LabelNumId.Text = "" Then
            MessageBox.Show("削除する商品行が選択がされていません", "商品IDなし", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Regex.IsMatch(TextBoxShohinNum.Text, "^[0-9]{1,4}$") = False Then
            MessageBox.Show("商品番号は半角数値の0～9999でなければなりません。", "メッセージ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Using context As New ShohinContext()
            Dim data = context.ShohinData.Single(Function(x) x.NumId = LabelNumId.Text)
            context.ShohinData.Remove(data)
            context.SaveChanges()
        End Using
        RichTextBox1.AppendText("商品ID：" & LabelNumId.Text & "のレコードを削除しました。" & vbCrLf)

    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        LabelNumId.Text = Integer.Parse(DataGridView1.CurrentRow.Cells("NumId").Value)
        TextBoxShohinNum.Text = DataGridView1.CurrentRow.Cells("ShohinCode").Value
        TextBoxShohinName.Text = DataGridView1.CurrentRow.Cells("ShohinName").Value
        TextBoxNote.Text = DataGridView1.CurrentRow.Cells("Note").Value

    End Sub

    Private Sub InitialData(ByVal context As ShohinContext)

        context.ShohinData.Add(New ShohinEntity() With {
                .ShohinCode = 7500,
                .ShohinName = "ｾﾄｳﾁﾚﾓﾝ",
                .EditDate = 20190417,
                .EditTime = 203145,
                .Note = "瀬戸内レモンです"})
        context.ShohinData.Add(New ShohinEntity() With {
                .ShohinCode = 2840,
                .ShohinName = "ﾘﾝｺﾞｼﾞｭｰｽ",
                .EditDate = 20050923,
                .EditTime = 102532,
                .Note = "果汁100%の炭酸飲料です"})
        context.ShohinData.Add(New ShohinEntity With {
                .ShohinCode = 1580,
                .ShohinName = "ｶﾌｪｵﾚ",
                .EditDate = 20160716,
                .EditTime = 91103,
                .Note = "200ml増量です"})
        context.ShohinData.Add(New ShohinEntity With {
                .ShohinCode = 270,
                .ShohinName = "ｳﾒｵﾆｷﾞﾘ",
                .EditDate = 20080825,
                .EditTime = 141520,
                .Note = "none"})
        context.SaveChanges()

    End Sub

    Private Function LabelsSetting(name As String, txt As String, x As Integer, y As Integer, w As Integer, h As Integer) As Label

        Dim label As New Label()
        label.Name = name
        label.AutoSize = False
        label.Text = txt
        label.Location = New Point(x, y)
        label.Size = New Size(w, h)
        LabelDic.Add(label.Name, label)
        Controls.Add(label)

        Return label

    End Function

    ''' <summary>
    ''' 与えられたControlクラスオブジェクトのロケーション、サイズを設定しフォームに追加しControlオブジェクトで戻します
    ''' </summary>
    ''' <param name="ctl">System.Windows.Forms.Control</param>
    ''' <param name="name">オブジェクト名</param>
    ''' <param name="x">ロケーションX</param>
    ''' <param name="y">ロケーションY</param>
    ''' <param name="w">コントロールの横サイズ</param>
    ''' <param name="h">コントロールの縦サイズ</param>
    ''' <returns>System.Windows.Forms.Control</returns>
    Private Function ControlsSetting(ctl As Control, name As String, x As Integer, y As Integer, w As Integer, h As Integer) As Control

        ctl.Name = name
        ctl.Location = New Point(x, y)
        ctl.Size = New Size(w, h)
        Controls.Add(ctl)

        Return ctl

    End Function

    Private Sub FormDesignSetting()

        Me.Name = "Form1"
        Me.Text = "EntityFramework +デスクトップアプリ + SQL Server"
        Me.Location = New Point(500, 200)
        Me.Size = New Size(800, 600)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.ResumeLayout(False)
        Me.PerformLayout()

        DataGridView1 = New DataGridView()
        DataGridView1 = CType(ControlsSetting(DataGridView1, "DataGridView1", 25, 25, 730, 200), DataGridView)
        CType(DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()

        RichTextBox1 = New RichTextBox()
        RichTextBox1.ReadOnly = True
        RichTextBox1 = CType(ControlsSetting(RichTextBox1, "RichTextBox1", 25, 235, 350, 200), RichTextBox)

        LabelsSetting("Label1", "商品ID：", 385, 250, 75, 25)
        LabelsSetting("Label2", "商品番号：", 385, 300, 75, 25)
        LabelsSetting("Label3", "商品名：", 385, 350, 75, 25)
        LabelsSetting("Label4", "備考：", 385, 400, 60, 25)

        LabelNumId = New Label()
        LabelNumId.AutoSize = False
        LabelNumId.Text = ""
        LabelNumId.TextAlign = ContentAlignment.TopRight
        LabelNumId = CType(ControlsSetting(LabelNumId, "LabelNumId", 690, 250, 60, 19), Label)

        LabelFoot = New Label()
        LabelFoot.AutoSize = False
        LabelFoot.Text = "Copyright (c)  2021-2024  support-for-bingo"
        LabelFoot = CType(ControlsSetting(LabelFoot, "LabelFoot", 30, 535, 300, 19), Label)

        TextBoxShohinNum = New TextBox()
        TextBoxShohinNum.TabIndex = 0
        TextBoxShohinNum = CType(ControlsSetting(TextBoxShohinNum, "TextBoxShohinNum", 600, 300, 150, 19), TextBox)

        TextBoxShohinName = New TextBox()
        TextBoxShohinName.TabIndex = 1
        TextBoxShohinName = CType(ControlsSetting(TextBoxShohinName, "TextBoxShohinName", 550, 350, 200, 19), TextBox)

        TextBoxNote = New TextBox()
        TextBoxNote.TabIndex = 2
        TextBoxNote = CType(ControlsSetting(TextBoxNote, "TextBoxNote", 450, 400, 300, 19), TextBox)

        ButtonQuery = New Button()
        ButtonQuery.Text = "抽出"
        ButtonQuery.TabIndex = 3
        ButtonQuery.UseVisualStyleBackColor = True
        ButtonQuery = CType(ControlsSetting(ButtonQuery, "ButtonQuery", 50, 470, 150, 50), Button)

        ButtonInsert = New Button()
        ButtonInsert.Text = "追加"
        ButtonInsert.TabIndex = 4
        ButtonInsert.UseVisualStyleBackColor = True
        ButtonInsert = CType(ControlsSetting(ButtonInsert, "ButtonInsert", 230, 470, 150, 50), Button)

        ButtonUpdate = New Button()
        ButtonUpdate.Text = "更新"
        ButtonUpdate.TabIndex = 5
        ButtonUpdate.UseVisualStyleBackColor = True
        ButtonUpdate = CType(ControlsSetting(ButtonUpdate, "ButtonUpdate", 410, 470, 150, 50), Button)

        ButtonDelete = New Button()
        ButtonDelete.Text = "削除"
        ButtonDelete.TabIndex = 6
        ButtonDelete.UseVisualStyleBackColor = True
        ButtonDelete = CType(ControlsSetting(ButtonDelete, "ButtonDelete", 590, 470, 150, 50), Button)

    End Sub

    Private Sub DataGridSetting()

        DataGridView1.Columns("NumId").HeaderText = "商品ID"
        DataGridView1.Columns("ShohinCode").HeaderText = "商品番号"
        DataGridView1.Columns("ShohinName").HeaderText = "商品名"
        DataGridView1.Columns("EditDate").HeaderText = "編集日付"
        DataGridView1.Columns("EditTime").HeaderText = "編集時刻"
        DataGridView1.Columns("Note").HeaderText = "備考"
        DataGridView1.Columns("NumId").Width = 70
        DataGridView1.Columns("Note").Width = 250
        DataGridView1.Columns("EditDate").DefaultCellStyle.Format = "0000/00/00"
        DataGridView1.Columns("EditTime").DefaultCellStyle.Format = "00:00:00"
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.RowHeadersVisible = False
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.ReadOnly = True

    End Sub

    Private Sub TextBoxClear()

        TextBoxShohinNum.Text = ""
        TextBoxShohinName.Text = ""
        TextBoxNote.Text = ""

    End Sub
End Class
