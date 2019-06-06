Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices

Public Class topForm

    Public Class dgvRowHeaderCell

        'DataGridViewRowHeaderCell を継承
        Inherits DataGridViewRowHeaderCell

        'DataGridViewHeaderCell.Paint をオーバーライドして行ヘッダーを描画
        Protected Overrides Sub Paint(ByVal graphics As Graphics, ByVal clipBounds As Rectangle,
           ByVal cellBounds As Rectangle, ByVal rowIndex As Integer, ByVal cellState As DataGridViewElementStates,
           ByVal value As Object, ByVal formattedValue As Object, ByVal errorText As String,
           ByVal cellStyle As DataGridViewCellStyle, ByVal advancedBorderStyle As DataGridViewAdvancedBorderStyle,
           ByVal paintParts As DataGridViewPaintParts)
            '標準セルの描画からセル内容の背景だけ除いた物を描画(-5)
            MyBase.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value,
                     formattedValue, errorText, cellStyle, advancedBorderStyle,
                     Not DataGridViewPaintParts.ContentBackground)
        End Sub

    End Class

    'データベースのパス
    Public dbFilePath As String = My.Application.Info.DirectoryPath & "\Part.mdb"
    Public DB_Part As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & dbFilePath

    'エクセルのパス
    Public excelFilePass As String = My.Application.Info.DirectoryPath & "\Part.xls"

    '.iniファイルのパス
    Public iniFilePath As String = My.Application.Info.DirectoryPath & "\Part.ini"

    '曜日
    Private dayArray() As String = {"日", "月", "火", "水", "木", "金", "土"}

    '時間
    Private timeArray() As String = {"16:30～0:30", "0:30～8:30", "8:30～12:00", "8:30～12:30", "9:00～13:00", "8:30～15:00", "8:30～15:30", "8:30～16:00", "8:30～17:00", "13:00～17:00"}

    '休憩、勤務時間対応
    Private timeDic As New Dictionary(Of String, String()) From {{"16:30～0:30", {"1.0", "7.0"}}, {"0:30～8:30", {"1.0", "7.0"}}, {"8:30～12:00", {"", "3.5"}}, {"8:30～12:30", {"", "4.0"}}, {"9:00～13:00", {"", "4.0"}}, {"8:30～15:00", {"1.0", "5.5"}}, {"8:30～15:30", {"1.0", "6.0"}}, {"8:30～16:00", {"1.0", "6.5"}}, {"8:30～17:00", {"1.0", "7.5"}}, {"13:00～17:00", {"", "4.0"}}}

    ''' <summary>
    ''' loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub topForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'データベース、エクセル、構成ファイルの存在チェック
        If Not System.IO.File.Exists(dbFilePath) Then
            MsgBox("データベースファイルが存在しません。ファイルを配置して下さい。")
            Me.Close()
            Exit Sub
        End If

        If Not System.IO.File.Exists(excelFilePass) Then
            MsgBox("エクセルファイルが存在しません。ファイルを配置して下さい。")
            Me.Close()
            Exit Sub
        End If

        If Not System.IO.File.Exists(iniFilePath) Then
            MsgBox("構成ファイルが存在しません。ファイルを配置して下さい。")
            Me.Close()
            Exit Sub
        End If

        Me.WindowState = FormWindowState.Maximized

        '印刷ラジオボタン初期値設定
        initPrintState()

        '時間コンボボックス初期設定
        initTimeBox()

        '氏名ボックス設定
        loadNamBox()

        '氏名リスト設定
        loadNamList()

        'データグリッドビュー初期設定
        initDgvPart()

        '曜日設定
        settingYoubi()

        '初期フォーカス
        namBox.Focus()
    End Sub

    ''' <summary>
    ''' 印刷ラジオボタン初期値設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub initPrintState()
        Dim state As String = Util.getIniString("System", "Printer", iniFilePath)
        If state = "Y" Then
            rbtnPrint.Checked = True
        Else
            rbtnPreview.Checked = True
        End If
    End Sub

    ''' <summary>
    ''' ﾌﾟﾚﾋﾞｭｰラジオボタン値変更イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub rbtnPreview_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbtnPreview.CheckedChanged
        If rbtnPreview.Checked = True Then
            Util.putIniString("System", "Printer", "N", iniFilePath)
        End If
    End Sub

    ''' <summary>
    ''' 印刷ラジオボタン値変更イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub rbtnPrint_CheckedChanged(sender As Object, e As System.EventArgs) Handles rbtnPrint.CheckedChanged
        If rbtnPrint.Checked = True Then
            Util.putIniString("System", "Printer", "Y", iniFilePath)
        End If
    End Sub

    ''' <summary>
    ''' 時間コンボボックス初期設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub initTimeBox()
        timeBox.Items.AddRange(timeArray)
    End Sub

    ''' <summary>
    ''' 氏名ボックス設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub loadNamBox()
        'クリア
        namBox.Items.Clear()

        '先月
        Dim ym As String = ymBox.getADymStr()
        Dim year As Integer = CInt(ym.Split("/")(0))
        Dim month As Integer = CInt(ym.Split("/")(1))
        Dim dt As New DateTime(year, month, 1)
        Dim prevYm As String = dt.AddMonths(-1).ToString("yyyy/MM")

        'データ取得
        Dim cn As New ADODB.Connection()
        cn.Open(DB_Part)
        Dim sql As String = "select distinct Nam from SvsD where Ymd Like '%" & prevYm & "%'"
        Dim rs As New ADODB.Recordset
        rs.Open(sql, cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockOptimistic)
        While Not rs.EOF
            Dim nam As String = Util.checkDBNullValue(rs.Fields("Nam").Value)
            namBox.Items.Add(nam)
            rs.MoveNext()
        End While
        rs.Close()
        cn.Close()
    End Sub

    ''' <summary>
    ''' 氏名リスト設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub loadNamList()
        'クリア
        namListBox.Items.Clear()

        '当月
        Dim ym As String = ymBox.getADymStr()

        'データ取得
        Dim cn As New ADODB.Connection()
        cn.Open(DB_Part)
        Dim sql As String = "select distinct Nam from SvsD where Ymd Like '%" & ym & "%'"
        Dim rs As New ADODB.Recordset
        rs.Open(sql, cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockOptimistic)
        While Not rs.EOF
            Dim nam As String = Util.checkDBNullValue(rs.Fields("Nam").Value)
            namListBox.Items.Add(nam)
            rs.MoveNext()
        End While
        rs.Close()
        cn.Close()

        '"＊すべて"を先頭に追加
        If namListBox.Items.Count > 0 Then
            namListBox.Items.Insert(0, "＊すべて")
        End If

    End Sub

    ''' <summary>
    ''' 入力内容クリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub clearInput()
        'データグリッドビュー
        For i As Integer = 0 To 30
            dgvPart("Bgn", i).Value = ""
            dgvPart("Los", i).Value = ""
            dgvPart("Fin", i).Value = ""
            dgvPart("Svs", i).Value = ""
        Next

        '調整
        cyoBox.Text = ""

        '合計時間
        timeLabel.Text = ""

        '氏名
        namBox.Text = ""

        '時間
        timeBox.Text = ""
    End Sub

    ''' <summary>
    ''' 年月ボックスエンターキーイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ymBox_keyDownEnter(sender As Object, e As System.EventArgs) Handles ymBox.keyDownEnter
        clearInput()
        loadNamBox()
        loadNamList()
        settingYoubi()
        namBox.Focus()
    End Sub

    ''' <summary>
    ''' データグリッドビュー初期設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub initDgvPart()
        Util.EnableDoubleBuffering(dgvPart)

        With dgvPart
            .AllowUserToAddRows = False '行追加禁止
            .AllowUserToResizeColumns = False '列の幅をユーザーが変更できないようにする
            .AllowUserToResizeRows = False '行の高さをユーザーが変更できないようにする
            .AllowUserToDeleteRows = False '行削除禁止
            .BorderStyle = BorderStyle.FixedSingle
            .MultiSelect = False
            .RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            .ColumnHeadersHeight = 20
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .RowTemplate.Height = 18
            .RowHeadersWidth = 30
            .RowTemplate.HeaderCell = New dgvRowHeaderCell() '行ヘッダの三角マークを非表示に
            .BackgroundColor = Color.FromKnownColor(KnownColor.Control)
            .DefaultCellStyle.SelectionBackColor = Color.White
            .DefaultCellStyle.SelectionForeColor = Color.Black
            .ShowCellToolTips = False
            .EnableHeadersVisualStyles = False
            .DefaultCellStyle.Font = New Font("ＭＳ Ｐゴシック", 11)
            .ImeMode = Windows.Forms.ImeMode.Disable
        End With

        '列追加、空の行追加
        Dim dt As New DataTable()
        dt.Columns.Add("Youbi", Type.GetType("System.String"))
        dt.Columns.Add("Bgn", Type.GetType("System.String"))
        dt.Columns.Add("Los", Type.GetType("System.String"))
        dt.Columns.Add("Fin", Type.GetType("System.String"))
        dt.Columns.Add("Svs", Type.GetType("System.String"))
        For i = 0 To 30
            Dim row As DataRow = dt.NewRow()
            dt.Rows.Add(row)
        Next

        '表示
        dgvPart.DataSource = dt

        '幅設定等
        With dgvPart
            With .Columns("Youbi")
                .HeaderText = "曜"
                .Width = 30
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .ReadOnly = True
            End With
            With .Columns("Bgn")
                .HeaderText = "出勤"
                .Width = 70
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
            With .Columns("Los")
                .HeaderText = "休憩"
                .Width = 70
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
            With .Columns("Fin")
                .HeaderText = "退勤"
                .Width = 70
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
            With .Columns("Svs")
                .HeaderText = "勤務"
                .Width = 70
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
        End With
    End Sub

    ''' <summary>
    ''' CellPainting
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgv_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles dgvPart.CellPainting
        '行ヘッダーかどうか調べる
        If e.ColumnIndex < 0 AndAlso e.RowIndex >= 0 AndAlso dgvPart("Youbi", e.RowIndex).Value <> "" Then
            'セルを描画する
            e.Paint(e.ClipBounds, DataGridViewPaintParts.All)

            '行番号を描画する範囲を決定する
            'e.AdvancedBorderStyleやe.CellStyle.Paddingは無視しています
            Dim indexRect As Rectangle = e.CellBounds
            indexRect.Inflate(-2, -2)

            '行番号を描画する
            TextRenderer.DrawText(e.Graphics,
                (e.RowIndex + 1).ToString(),
                e.CellStyle.Font,
                indexRect,
                e.CellStyle.ForeColor,
                TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
            '描画が完了したことを知らせる
            e.Handled = True
        End If

        '選択したセルに枠を付ける
        If e.ColumnIndex >= 0 AndAlso e.RowIndex >= 0 AndAlso (e.PaintParts And DataGridViewPaintParts.Background) = DataGridViewPaintParts.Background Then
            e.Graphics.FillRectangle(New SolidBrush(e.CellStyle.BackColor), e.CellBounds)

            If (e.PaintParts And DataGridViewPaintParts.SelectionBackground) = DataGridViewPaintParts.SelectionBackground AndAlso (e.State And DataGridViewElementStates.Selected) = DataGridViewElementStates.Selected Then
                e.Graphics.DrawRectangle(New Pen(Color.Black, 2I), e.CellBounds.X + 1I, e.CellBounds.Y + 1I, e.CellBounds.Width - 3I, e.CellBounds.Height - 3I)
            End If

            Dim pParts As DataGridViewPaintParts
            pParts = e.PaintParts And Not DataGridViewPaintParts.Background
            e.Paint(e.ClipBounds, pParts)
            e.Handled = True
        End If
    End Sub

    ''' <summary>
    ''' 設定年月の曜日を表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub settingYoubi()
        'クリア
        For i As Integer = 0 To 30
            dgvPart("Youbi", i).Value = ""
        Next

        '設定年月
        Dim ym As String = ymBox.getADymStr()
        Dim year As Integer = CInt(ym.Split("/")(0))
        Dim month As Integer = CInt(ym.Split("/")(1))

        '初日の曜日、月の日数取得
        Dim dt As New DateTime(year, month, 1)
        Dim firstDayOfWeek As Integer = dt.DayOfWeek '最初の曜日
        Dim lastDay As Integer = DateTime.DaysInMonth(year, month) '日数

        '曜日設定
        For i As Integer = 1 To lastDay
            dgvPart("Youbi", i - 1).Value = dayArray((firstDayOfWeek + (i - 1)) Mod 7)
        Next
    End Sub

    ''' <summary>
    ''' セル編集終了イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvPart_CellEndEdit(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvPart.CellEndEdit
        '入力値
        Dim inputStr As String = Util.checkDBNullValue(dgvPart.CurrentCell.Value)
        Dim inputLength As Integer = inputStr.Length
        If inputStr = "" Then
            '勤務時間合計計算
            calcWorkTime()
            Return
        End If

        '列名
        Dim columnName As String = dgvPart.Columns(dgvPart.CurrentCell.ColumnIndex).Name

        '入力チェック等
        If (columnName = "Bgn" OrElse columnName = "Fin") Then
            '時間形式の文字列かどうか
            Dim isMatch As Boolean = System.Text.RegularExpressions.Regex.IsMatch(inputStr, "^\d\d?:\d\d")

            If Not isMatch AndAlso inputLength <> 3 AndAlso inputLength <> 4 Then
                Dim colStr As String = If(columnName = "Bgn", "出勤", "退勤")
                MsgBox(colStr & "の桁数が不正です。数値3桁か4桁を入力して下さい。", MsgBoxStyle.Exclamation)
                Return
            End If
            If Not isMatch Then
                dgvPart.CurrentCell.Value = inputStr.Insert(inputStr.Length - 2, ":")
            End If
        ElseIf columnName = "Los" Then
            If Not System.Text.RegularExpressions.Regex.IsMatch(inputStr, "^\d\.\d$") Then
                MsgBox("休憩の入力が不正です。#.# のように入力して下さい", MsgBoxStyle.Exclamation)
                Return
            End If
        End If

        '勤務時間合計計算
        calcWorkTime()
    End Sub

    ''' <summary>
    ''' 勤務時間計算
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub calcWorkTime()
        Dim totalTime As Double = 0
        For i As Integer = 0 To 30
            Dim timeStr As String = Util.checkDBNullValue(dgvPart("Svs", i).Value)
            If System.Text.RegularExpressions.Regex.IsMatch(timeStr, "^\d+(\.\d+)?$") Then
                Dim time As Double = Math.Round(CDbl(timeStr), 1, MidpointRounding.AwayFromZero)
                totalTime += time
            End If
        Next
        If totalTime <> 0 Then
            timeLabel.Text = totalTime.ToString("#.0")
            If System.Text.RegularExpressions.Regex.IsMatch(cyoBox.Text, "^\d+(\.\d)?$") Then
                Dim tyoNum As Double = CDbl(cyoBox.Text)
                timeLabel.Text = (CDbl(timeLabel.Text) + tyoNum).ToString("#.0")
            End If
        End If
    End Sub

    ''' <summary>
    ''' 時間ボックス値変更イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub timeBox_SelectedValueChanged(sender As Object, e As System.EventArgs) Handles timeBox.SelectedValueChanged
        Dim bgn As String = timeBox.Text.Split("～")(0) '出勤時間
        Dim fin As String = timeBox.Text.Split("～")(1) '退勤時間
        Dim los As String = If(timeDic.ContainsKey(timeBox.Text), timeDic(timeBox.Text)(0), "") '休憩時間
        Dim svs As String = If(timeDic.ContainsKey(timeBox.Text), timeDic(timeBox.Text)(1), "") '勤務時間

        '値セット
        For i As Integer = 0 To 30
            Dim youbi As String = Util.checkDBNullValue(dgvPart("Youbi", i).Value)
            If youbi <> "" AndAlso youbi <> "土" AndAlso youbi <> "日" Then
                dgvPart("Bgn", i).Value = bgn
                dgvPart("Fin", i).Value = fin
                dgvPart("Los", i).Value = los
                dgvPart("Svs", i).Value = svs
            End If
        Next

        '勤務時間合計表示
        calcWorkTime()
    End Sub

    ''' <summary>
    ''' 調整ボックスKeyDownイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tyoBox_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles cyoBox.KeyDown
        If e.KeyCode = Keys.Enter Then
            calcWorkTime()
            btnRegist.Focus()
        End If
    End Sub

    ''' <summary>
    ''' 登録ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnRegist_Click(sender As System.Object, e As System.EventArgs) Handles btnRegist.Click
        '氏名
        Dim nam As String = namBox.Text
        If nam = "" OrElse nam = "＊すべて" Then
            MsgBox("氏名を入力して下さい。", MsgBoxStyle.Exclamation)
            namBox.Focus()
            Return
        End If

        '調整
        Dim cyo As String = cyoBox.Text
        If cyo <> "" AndAlso Not System.Text.RegularExpressions.Regex.IsMatch(cyo, "^\d+(\.\d)?$") Then
            MsgBox("調整は数値(#.#)を入力して下さい。", MsgBoxStyle.Exclamation)
            cyoBox.Focus()
            Return
        End If

        '既存データ削除
        Dim ym As String = ymBox.getADymStr()
        Dim cnn As New ADODB.Connection
        cnn.Open(DB_Part)
        Dim cmd As New ADODB.Command()
        cmd.ActiveConnection = cnn
        cmd.CommandText = "delete from SvsD where Ymd Like '%" & ym & "%' and Nam = '" & nam & "'"
        cmd.Execute()

        '登録
        Dim registIndex As Integer = 30
        For i As Integer = 0 To 30
            If Util.checkDBNullValue(dgvPart("Youbi", i).Value) = "" Then
                registIndex = i - 1
            End If
        Next
        Dim rs As New ADODB.Recordset
        rs.Open("SvsD", cnn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
        For i As Integer = 0 To registIndex
            rs.AddNew()
            If i = 0 Then
                rs.Fields("Cyo").Value = cyoBox.Text
            End If
            rs.Fields("Nam").Value = nam
            rs.Fields("Ymd").Value = ym & "/" & If(i + 1 < 10, "0" & i + 1, i + 1)
            rs.Fields("Bgn").Value = Util.checkDBNullValue(dgvPart("Bgn", i).Value)
            rs.Fields("Fin").Value = Util.checkDBNullValue(dgvPart("Fin", i).Value)
            rs.Fields("Los").Value = Util.checkDBNullValue(dgvPart("Los", i).Value)
            rs.Fields("Svs").Value = Util.checkDBNullValue(dgvPart("Svs", i).Value)
        Next
        rs.Update()
        rs.Close()
        cnn.Close()

        'クリア
        clearInput()
        loadNamList()
        namBox.Focus()

    End Sub

    ''' <summary>
    ''' 削除ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnDelete.Click
        '氏名
        Dim nam As String = namBox.Text
        '年月
        Dim ym As String = ymBox.getADymStr()

        '削除
        Dim cnn As New ADODB.Connection
        cnn.Open(DB_Part)
        Dim sql As String = "select * from SvsD where Nam = '" & nam & "' and Ymd Like '%" & ym & "%'"
        Dim rs As New ADODB.Recordset
        rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
        If rs.RecordCount <= 0 Then
            MsgBox("登録されていません。", MsgBoxStyle.Exclamation)
            rs.Close()
            cnn.Close()
        Else
            Dim result As DialogResult = MessageBox.Show("削除してよろしいですか？", "削除", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = Windows.Forms.DialogResult.Yes Then
                Dim cmd As New ADODB.Command()
                cmd.ActiveConnection = cnn
                cmd.CommandText = "delete from SvsD where Ymd Like '%" & ym & "%' and Nam = '" & nam & "'"
                cmd.Execute()
                rs.Close()
                cnn.Close()

                'クリア
                clearInput()
                loadNamList()
                namBox.Focus()
            Else
                rs.Close()
                cnn.Close()
            End If
        End If
    End Sub

    ''' <summary>
    ''' 印刷ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrint_Click(sender As System.Object, e As System.EventArgs) Handles btnPrint.Click
        '氏名
        Dim selectedNam As String = namBox.Text
        If selectedNam = "" Then
            MsgBox("氏名を選択して下さい。", MsgBoxStyle.Exclamation)
            namBox.Focus()
            Return
        End If
        '年月
        Dim ym As String = ymBox.getADymStr()
        '年月文字列
        Dim ymStr As String = ym.Split("/")(0) & " 年 " & ym.Split("/")(1) & " 月"

        'データ取得
        Dim sql As String
        If selectedNam = "＊すべて" Then
            sql = "select * from SvsD where Ymd Like '%" & ym & "%' order by Nam, Ymd"
        Else
            sql = "select * from SvsD where Nam = '" & selectedNam & "' and Ymd Like '%" & ym & "%' order by Ymd"
        End If
        Dim cnn As New ADODB.Connection
        cnn.Open(DB_Part)
        Dim rs As New ADODB.Recordset
        rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
        If rs.RecordCount <= 0 Then
            MsgBox(selectedNam & "　は登録されてません。", MsgBoxStyle.Exclamation)
            rs.Close()
            cnn.Close()
            Return
        End If

        '印刷データ作成
        Dim lastDate As Integer = 31
        Dim youbiArray(30) As String
        For i As Integer = 0 To 30
            Dim youbi As String = Util.checkDBNullValue(dgvPart("Youbi", i).Value)
            If youbi = "" Then
                lastDate = i
                Exit For
            Else
                youbiArray(i) = youbi
            End If
        Next
        Dim dataList As New List(Of String(,))
        Dim dataArray(34, 9) As String
        Dim rowIndex As Integer = 0
        Dim tmpNam As String = Util.checkDBNullValue(rs.Fields("Nam").Value)
        '初期値設定
        dataArray(0, 0) = ymStr '日付
        dataArray(0, 3) = tmpNam '氏名
        dataArray(1, 0) = "日付"
        dataArray(1, 1) = "曜日"
        dataArray(1, 2) = "出勤"
        dataArray(1, 3) = "休憩"
        dataArray(1, 4) = "退勤"
        dataArray(1, 5) = "勤務"
        dataArray(1, 6) = "備考"
        dataArray(1, 9) = "印鑑"
        dataArray(33, 0) = "調整"
        dataArray(33, 3) = "合計"
        For i As Integer = 0 To lastDate - 1
            dataArray(2 + i, 0) = i + 1
            dataArray(2 + i, 1) = youbiArray(i)
        Next
        While Not rs.EOF
            Dim nam As String = Util.checkDBNullValue(rs.Fields("Nam").Value)
            If nam <> tmpNam Then
                '勤務合計時間計算
                dataArray(33, 5) = calcWorkTime4Print(dataArray, lastDate, dataArray(33, 2))

                '配列追加、クリア
                dataList.Add(dataArray.Clone())
                Array.Clear(dataArray, 0, dataArray.Length)

                '初期値設定
                dataArray(0, 0) = ymStr '日付
                dataArray(0, 3) = nam '氏名
                dataArray(1, 0) = "日付"
                dataArray(1, 1) = "曜日"
                dataArray(1, 2) = "出勤"
                dataArray(1, 3) = "休憩"
                dataArray(1, 4) = "退勤"
                dataArray(1, 5) = "勤務"
                dataArray(1, 6) = "備考"
                dataArray(1, 9) = "印鑑"
                dataArray(33, 0) = "調整"
                dataArray(33, 3) = "合計"
                For i As Integer = 0 To lastDate - 1
                    dataArray(2 + i, 0) = i + 1
                    dataArray(2 + i, 1) = youbiArray(i)
                Next

                '更新
                tmpNam = nam
                rowIndex = 0
            End If
            dataArray(2 + rowIndex, 2) = Util.checkDBNullValue(rs.Fields("Bgn").Value) '出勤
            dataArray(2 + rowIndex, 3) = Util.checkDBNullValue(rs.Fields("Los").Value) '休憩
            dataArray(2 + rowIndex, 4) = Util.checkDBNullValue(rs.Fields("Fin").Value) '退勤
            dataArray(2 + rowIndex, 5) = Util.checkDBNullValue(rs.Fields("Svs").Value) '勤務
            If rowIndex = 0 Then
                dataArray(33, 2) = Util.checkDBNullValue(rs.Fields("Cyo").Value) '調整
            End If

            rowIndex += 1
            rs.MoveNext()
        End While
        '勤務合計時間計算
        dataArray(33, 5) = calcWorkTime4Print(dataArray, lastDate, dataArray(33, 2))
        dataList.Add(dataArray.Clone())

        'エクセル
        Dim objExcel As Excel.Application = CreateObject("Excel.Application")
        Dim objWorkBooks As Excel.Workbooks = objExcel.Workbooks
        Dim objWorkBook As Excel.Workbook = objWorkBooks.Open(excelFilePass)
        Dim oSheet As Excel.Worksheet = objWorkBook.Worksheets("勤務時間改")
        objExcel.Calculation = Excel.XlCalculation.xlCalculationManual
        objExcel.ScreenUpdating = False

        '必要枚数コピペ
        For i As Integer = 0 To dataList.Count - 2
            Dim xlPasteRange As Excel.Range = oSheet.Range("A" & (39 + (38 * i))) 'ペースト先
            oSheet.Rows("1:38").copy(xlPasteRange)
            oSheet.HPageBreaks.Add(oSheet.Range("A" & (39 + (38 * i)))) '改ページ
        Next

        'データ貼り付け
        For i As Integer = 0 To dataList.Count - 1
            oSheet.Range("B" & (1 + 38 * i), "K" & (35 + 38 * i)).Value = dataList(i)
        Next

        objExcel.Calculation = Excel.XlCalculation.xlCalculationAutomatic
        objExcel.ScreenUpdating = True

        '変更保存確認ダイアログ非表示
        objExcel.DisplayAlerts = False

        '印刷
        If rbtnPrint.Checked = True Then
            oSheet.PrintOut()
        ElseIf rbtnPreview.Checked = True Then
            objExcel.Visible = True
            oSheet.PrintPreview(1)
        End If

        ' EXCEL解放
        objExcel.Quit()
        Marshal.ReleaseComObject(objWorkBook)
        Marshal.ReleaseComObject(objExcel)
        oSheet = Nothing
        objWorkBook = Nothing
        objExcel = Nothing
    End Sub

    ''' <summary>
    ''' 勤務時間合計計算（印刷用）
    ''' </summary>
    ''' <param name="dataArray">書き込みデータ配列</param>
    ''' <param name="lastDate">月の日数</param>
    ''' <param name="cyoTime">調整時間</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function calcWorkTime4Print(dataArray As String(,), lastDate As Integer, cyoTime As String) As String
        Dim totalTime As Double = 0
        For i As Integer = 2 To lastDate - 1 + 2
            If System.Text.RegularExpressions.Regex.IsMatch(dataArray(i, 5), "^\d+(\.\d)?$") Then
                Dim time As Double = CDbl(dataArray(i, 5))
                totalTime += time
            End If
        Next
        If totalTime <> 0 AndAlso System.Text.RegularExpressions.Regex.IsMatch(cyoTime, "^\d+(\.\d)?$") Then
            totalTime += CDbl(cyoTime)
        End If

        Return If(totalTime = 0, "", totalTime.ToString("#.0"))
    End Function

    ''' <summary>
    ''' 前月コピーボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCopy_Click(sender As System.Object, e As System.EventArgs) Handles btnCopy.Click
        '当月
        Dim ym As String = ymBox.getADymStr()
        Dim yyyy As Integer = CInt(ym.Split("/")(0))
        Dim MM As Integer = CInt(ym.Split("/")(1))

        '前月
        Dim prevYm As String = New DateTime(yyyy, MM, 1).AddMonths(-1).ToString("yyyy/MM")

        '前月のデータ存在チェック
        Dim cnn As New ADODB.Connection
        cnn.Open(DB_Part)
        Dim sql As String = "select * from SvsD where Ymd Like '%" & prevYm & "%' order by Ymd, Nam"
        Dim rsPrev As New ADODB.Recordset
        rsPrev.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
        If rsPrev.RecordCount <= 0 Then
            MsgBox(prevYm & "　分がありません。", MsgBoxStyle.Exclamation)
            rsPrev.Close()
            cnn.Close()
            Return
        End If

        'コピー確認
        Dim result As DialogResult = MessageBox.Show(prevYm & "　から　" & ym & "　を生成しますか？", "コピー", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = Windows.Forms.DialogResult.Yes Then
            sql = "select * from SvsD where Ymd Like '%" & ym & "%'"
            Dim rs As New ADODB.Recordset
            rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
            If rs.RecordCount > 0 Then
                '既存データ削除確認
                Dim deleteResult As DialogResult = MessageBox.Show(ym & "　分が既に存在します" & Environment.NewLine & "削除してよろしいですか？", "コピー", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If deleteResult = Windows.Forms.DialogResult.Yes Then
                    '削除
                    Dim cmd As New ADODB.Command()
                    cmd.ActiveConnection = cnn
                    cmd.CommandText = "delete from SvsD where Ymd Like '%" & ym & "%'"
                    cmd.Execute()
                Else
                    rsPrev.Close()
                    rs.Close()
                    cnn.Close()
                    Return
                End If
            End If

            '前月コピー
            While Not rsPrev.EOF
                rs.AddNew()

                '日付を1ヶ月後に
                Dim ymd As String = Util.checkDBNullValue(rsPrev.Fields("Ymd").Value)
                Dim dt As New DateTime(CInt(ymd.Split("/")(0)), CInt(ymd.Split("/")(1)), CInt(ymd.Split("/")(2)))
                rs.Fields("Ymd").Value = dt.AddMonths(1).ToString("yyyy/MM/dd")

                '氏名だけコピー
                rs.Fields("Nam").Value = Util.checkDBNullValue(rsPrev.Fields("Nam").Value)
                rs.Fields("Bgn").Value = ""
                rs.Fields("Fin").Value = ""
                rs.Fields("Los").Value = ""
                rs.Fields("Svs").Value = ""
                rs.Fields("Cyo").Value = ""
                rsPrev.MoveNext()
            End While
            rs.Update()
            rs.Close()
            rsPrev.Close()
            cnn.Close()

            '再表示
            clearInput()
            namBox.Focus()
            loadNamList()

        End If
    End Sub

    ''' <summary>
    ''' 設定年月の選択氏名のデータ表示
    ''' </summary>
    ''' <param name="nam">氏名</param>
    ''' <param name="ym">年月(yyyy/MM)</param>
    ''' <remarks></remarks>
    Private Sub displayDgvData(nam As String, ym As String)
        'クリア
        clearInput()

        '氏名ボックスへセット
        namBox.Text = nam

        'データ取得、表示
        Dim cnn As New ADODB.Connection
        cnn.Open(DB_Part)
        Dim sql As String = "select * from SvsD where Nam = '" & nam & "' and Ymd Like '%" & ym & "%' order by Ymd"
        Dim rs As New ADODB.Recordset
        Dim rowIndex As Integer = 0
        rs.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic)
        While Not rs.EOF
            If rowIndex = 0 Then
                cyoBox.Text = Util.checkDBNullValue(rs.Fields("Cyo").Value)
            End If
            dgvPart("Bgn", rowIndex).Value = Util.checkDBNullValue(rs.Fields("Bgn").Value)
            dgvPart("Fin", rowIndex).Value = Util.checkDBNullValue(rs.Fields("Fin").Value)
            dgvPart("Los", rowIndex).Value = Util.checkDBNullValue(rs.Fields("Los").Value)
            dgvPart("Svs", rowIndex).Value = Util.checkDBNullValue(rs.Fields("Svs").Value)

            rowIndex += 1
            rs.MoveNext()
        End While
        rs.Close()
        cnn.Close()

        '勤務時間計算
        calcWorkTime()

        'フォーカス
        dgvPart.Focus()
    End Sub

    ''' <summary>
    ''' 氏名リスト値変更イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub namListBox_SelectedValueChanged(sender As Object, e As System.EventArgs) Handles namListBox.SelectedValueChanged
        '選択氏名
        Dim nam As String = namListBox.Text
        '年月
        Dim ym As String = ymBox.getADymStr()
        'データ表示
        displayDgvData(nam, ym)
    End Sub
End Class
