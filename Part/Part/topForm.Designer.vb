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
        Me.components = New System.ComponentModel.Container()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
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
        Me.cyoBox = New System.Windows.Forms.TextBox()
        Me.ymBox = New ADBox.adBox()
        Me.timeLabel = New System.Windows.Forms.Label()
        Me.dgvPart = New Part.ExDataGridView(Me.components)
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
        'namListBox
        '
        Me.namListBox.BackColor = System.Drawing.SystemColors.Control
        Me.namListBox.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.namListBox.FormattingEnabled = True
        Me.namListBox.ItemHeight = 15
        Me.namListBox.Location = New System.Drawing.Point(449, 87)
        Me.namListBox.Name = "namListBox"
        Me.namListBox.Size = New System.Drawing.Size(118, 379)
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
        Me.Label3.Location = New System.Drawing.Point(164, 643)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 15)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "調整"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label4.Location = New System.Drawing.Point(288, 643)
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
        Me.namBox.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.namBox.FormattingEnabled = True
        Me.namBox.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.namBox.Location = New System.Drawing.Point(289, 17)
        Me.namBox.Name = "namBox"
        Me.namBox.Size = New System.Drawing.Size(145, 23)
        Me.namBox.TabIndex = 14
        '
        'timeBox
        '
        Me.timeBox.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.timeBox.FormattingEnabled = True
        Me.timeBox.Location = New System.Drawing.Point(445, 17)
        Me.timeBox.Name = "timeBox"
        Me.timeBox.Size = New System.Drawing.Size(122, 23)
        Me.timeBox.TabIndex = 15
        '
        'cyoBox
        '
        Me.cyoBox.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cyoBox.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.cyoBox.Location = New System.Drawing.Point(213, 640)
        Me.cyoBox.Name = "cyoBox"
        Me.cyoBox.Size = New System.Drawing.Size(56, 22)
        Me.cyoBox.TabIndex = 16
        Me.cyoBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ymBox
        '
        Me.ymBox.dateText = "06"
        Me.ymBox.Location = New System.Drawing.Point(122, 9)
        Me.ymBox.Mode = 1
        Me.ymBox.monthText = "06"
        Me.ymBox.Name = "ymBox"
        Me.ymBox.Size = New System.Drawing.Size(105, 35)
        Me.ymBox.TabIndex = 17
        Me.ymBox.yearText = "2019"
        '
        'timeLabel
        '
        Me.timeLabel.AutoSize = True
        Me.timeLabel.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.timeLabel.Location = New System.Drawing.Point(358, 644)
        Me.timeLabel.Name = "timeLabel"
        Me.timeLabel.Size = New System.Drawing.Size(0, 15)
        Me.timeLabel.TabIndex = 19
        Me.timeLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'dgvPart
        '
        Me.dgvPart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPart.Location = New System.Drawing.Point(78, 55)
        Me.dgvPart.Name = "dgvPart"
        Me.dgvPart.RowTemplate.Height = 21
        Me.dgvPart.Size = New System.Drawing.Size(342, 580)
        Me.dgvPart.TabIndex = 18
        '
        'topForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(810, 687)
        Me.Controls.Add(Me.timeLabel)
        Me.Controls.Add(Me.dgvPart)
        Me.Controls.Add(Me.ymBox)
        Me.Controls.Add(Me.cyoBox)
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
    Friend WithEvents cyoBox As System.Windows.Forms.TextBox
    Friend WithEvents ymBox As ADBox.adBox
    Friend WithEvents dgvPart As Part.ExDataGridView
    Friend WithEvents timeLabel As System.Windows.Forms.Label

End Class
