Public Class topForm

    'データベースのパス
    Public dbFilePath As String = My.Application.Info.DirectoryPath & "\Part.mdb"
    Public DB_Part As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & dbFilePath

    'エクセルのパス
    Public excelFilePass As String = My.Application.Info.DirectoryPath & "\Part.xls"

    '.iniファイルのパス
    Public iniFilePath As String = My.Application.Info.DirectoryPath & "\Part.ini"

    '時間
    Private timeArray() As String = {"16:30～0:30", "0:30～8:30", "8:30～12:00", "8:30～12:30", "9:00～13:00", "8:30～15:00", "8:30～15:30", "8:30～16:00", "8:30～17:00", "13:00～17:00"}

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
    ''' 氏名リスト読み込み
    ''' </summary>
    ''' <param name="ymStr"></param>
    ''' <remarks></remarks>
    Private Sub loadNamList(ymStr As String)
        'クリア
        namListBox.Items.Clear()

        '
        'Dim cn As New ADODB.Connection()
        'cn.Open(DB_Part)
        'Dim sql As String = "select Nam from SvsD where Ymd"
        'Dim rs As New ADODB.Recordset
        'rs.Open(sql, cn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockOptimistic)
        'While Not rs.EOF
        '    Dim nam As String = Util.checkDBNullValue(rs.Fields("Nam").Value)
        '    namListBox.Items.Add(nam)
        '    rs.MoveNext()
        'End While
        'rs.Close()
        'cn.Close()
    End Sub
End Class
