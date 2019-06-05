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

    '
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
        settingYoubi()
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
        If nam = "" Then
            MsgBox("氏名を入力して下さい。", MsgBoxStyle.Exclamation)
            namBox.Focus()
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

    End Sub

    ''' <summary>
    ''' 削除ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnDelete.Click

    End Sub

    ''' <summary>
    ''' 印刷ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrint_Click(sender As System.Object, e As System.EventArgs) Handles btnPrint.Click

    End Sub

    ''' <summary>
    ''' 前月コピーボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCopy_Click(sender As System.Object, e As System.EventArgs) Handles btnCopy.Click

    End Sub
End Class
