Public Class ExDataGridView
    Inherits DataGridView

    Protected Overrides Function ProcessDialogKey(keyData As System.Windows.Forms.Keys) As Boolean
        Dim columnName As String = Me.Columns(CurrentCell.ColumnIndex).Name '選択列名
        If keyData = Keys.Enter Then
            If columnName = "Svs" Then
                Me.ProcessTabKey(keyData)
            End If
            Return Me.ProcessTabKey(keyData)
        Else
            Return MyBase.ProcessDialogKey(keyData)
        End If
    End Function

    Protected Overrides Function ProcessDataGridViewKey(e As System.Windows.Forms.KeyEventArgs) As Boolean
        Dim columnName As String = Me.Columns(CurrentCell.ColumnIndex).Name '選択列名
        If e.KeyCode = Keys.Enter Then
            If columnName = "Svs" Then
                Me.ProcessTabKey(e.KeyCode)
            End If
            Me.ProcessTabKey(e.KeyCode)
            BeginEdit(True)
            Return False
        End If

        Dim tb As DataGridViewTextBoxEditingControl = CType(Me.EditingControl, DataGridViewTextBoxEditingControl)
        If Not IsNothing(tb) AndAlso ((e.KeyCode = Keys.Left AndAlso tb.SelectionStart = 0) OrElse (e.KeyCode = Keys.Right AndAlso tb.SelectionStart = tb.TextLength)) Then
            Return False
        Else
            Return MyBase.ProcessDataGridViewKey(e)
        End If
    End Function
End Class
