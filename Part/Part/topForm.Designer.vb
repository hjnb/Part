<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class topForm
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dgvPart = New System.Windows.Forms.DataGridView()
        Me.namListBox = New System.Windows.Forms.ListBox()
        Me.btnRegist = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnCopy = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.rbtnPrint = New System.Windows.Forms.RadioButton()
        Me.rbtnPreview = New System.Windows.Forms.RadioButton()
        Me.namBox = New System.Windows.Forms.ComboBox()
        Me.timeBox = New System.Windows.Forms.ComboBox()
        Me.tyoBox = New System.Windows.Forms.TextBox()
        Me.AdBox1 = New ADBox.adBox()
        CType(Me.dgvPart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.Location = New System.Drawing.Point(54, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "対象年月"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label2.Location = New System.Drawing.Point(247, 21)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 15)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "氏名"
        '
        'dgvPart
        '
        Me.dgvPart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPart.Location = New System.Drawing.Point(78, 60)
        Me.dgvPart.Name = "dgvPart"
        Me.dgvPart.RowTemplate.Height = 21
        Me.dgvPart.Size = New System.Drawing.Size(341, 573)
        Me.dgvPart.TabIndex = 4
        '
        'namListBox
        '
        Me.namListBox.BackColor = System.Drawing.SystemColors.Control
        Me.namListBox.FormattingEnabled = True
        Me.namListBox.ItemHeight = 12
        Me.namListBox.Location = New System.Drawing.Point(449, 87)
        Me.namListBox.Name = "namListBox"
        Me.namListBox.Size = New System.Drawing.Size(104, 388)
        Me.namListBox.TabIndex = 5
        '
        'btnRegist
        '
        Me.btnRegist.Location = New System.Drawing.Point(586, 87)
        Me.btnRegist.Name = "btnRegist"
        Me.btnRegist.Size = New System.Drawing.Size(75, 36)
        Me.btnRegist.TabIndex = 6
        Me.btnRegist.Text = "登録"
        Me.btnRegist.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(586, 129)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(75, 36)
        Me.btnDelete.TabIndex = 7
        Me.btnDelete.Text = "削除"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(586, 171)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(75, 36)
        Me.btnPrint.TabIndex = 8
        Me.btnPrint.Text = "印刷"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnCopy
        '
        Me.btnCopy.Location = New System.Drawing.Point(586, 261)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(75, 36)
        Me.btnCopy.TabIndex = 9
        Me.btnCopy.Text = "前月コピー"
        Me.btnCopy.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label3.Location = New System.Drawing.Point(166, 638)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 15)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "調整"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label4.Location = New System.Drawing.Point(282, 638)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 15)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "合　計"
        '
        'rbtnPrint
        '
        Me.rbtnPrint.AutoSize = True
        Me.rbtnPrint.Location = New System.Drawing.Point(598, 238)
        Me.rbtnPrint.Name = "rbtnPrint"
        Me.rbtnPrint.Size = New System.Drawing.Size(47, 16)
        Me.rbtnPrint.TabIndex = 13
        Me.rbtnPrint.TabStop = True
        Me.rbtnPrint.Text = "印刷"
        Me.rbtnPrint.UseVisualStyleBackColor = True
        '
        'rbtnPreview
        '
        Me.rbtnPreview.AutoSize = True
        Me.rbtnPreview.Location = New System.Drawing.Point(598, 215)
        Me.rbtnPreview.Name = "rbtnPreview"
        Me.rbtnPreview.Size = New System.Drawing.Size(63, 16)
        Me.rbtnPreview.TabIndex = 12
        Me.rbtnPreview.TabStop = True
        Me.rbtnPreview.Text = "ﾌﾟﾚﾋﾞｭｰ"
        Me.rbtnPreview.UseVisualStyleBackColor = True
        '
        'namBox
        '
        Me.namBox.FormattingEnabled = True
        Me.namBox.Location = New System.Drawing.Point(289, 19)
        Me.namBox.Name = "namBox"
        Me.namBox.Size = New System.Drawing.Size(145, 20)
        Me.namBox.TabIndex = 14
        '
        'timeBox
        '
        Me.timeBox.FormattingEnabled = True
        Me.timeBox.Location = New System.Drawing.Point(445, 19)
        Me.timeBox.Name = "timeBox"
        Me.timeBox.Size = New System.Drawing.Size(122, 20)
        Me.timeBox.TabIndex = 15
        '
        'tyoBox
        '
        Me.tyoBox.Location = New System.Drawing.Point(215, 636)
        Me.tyoBox.Name = "tyoBox"
        Me.tyoBox.Size = New System.Drawing.Size(56, 19)
        Me.tyoBox.TabIndex = 16
        '
        'AdBox1
        '
        Me.AdBox1.dateText = "04"
        Me.AdBox1.Location = New System.Drawing.Point(122, 9)
        Me.AdBox1.Mode = 1
        Me.AdBox1.monthText = "06"
        Me.AdBox1.Name = "AdBox1"
        Me.AdBox1.Size = New System.Drawing.Size(105, 35)
        Me.AdBox1.TabIndex = 17
        Me.AdBox1.yearText = "2019"
        '
        'topForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(810, 687)
        Me.Controls.Add(Me.AdBox1)
        Me.Controls.Add(Me.tyoBox)
        Me.Controls.Add(Me.timeBox)
        Me.Controls.Add(Me.namBox)
        Me.Controls.Add(Me.rbtnPrint)
        Me.Controls.Add(Me.rbtnPreview)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnCopy)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnRegist)
        Me.Controls.Add(Me.namListBox)
        Me.Controls.Add(Me.dgvPart)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "topForm"
        Me.Text = "Part 勤務データ"
        CType(Me.dgvPart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dgvPart As System.Windows.Forms.DataGridView
    Friend WithEvents namListBox As System.Windows.Forms.ListBox
    Friend WithEvents btnRegist As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnCopy As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents rbtnPrint As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnPreview As System.Windows.Forms.RadioButton
    Friend WithEvents namBox As System.Windows.Forms.ComboBox
    Friend WithEvents timeBox As System.Windows.Forms.ComboBox
    Friend WithEvents tyoBox As System.Windows.Forms.TextBox
    Friend WithEvents AdBox1 As ADBox.adBox

End Class
