Imports System.Drawing
Imports System.Windows.Forms

' A small add/edit box with one text box per label. The first field is required.
' Use:  Dim v() As String = RecordDialog.Edit(Me, "Subject", labels, currentValues)
' It returns Nothing if the user cancels.
Public Class RecordDialog
    Inherits Form

    Private boxes() As TextBox

    Private Sub New(ByVal title As String, ByVal labels() As String, ByVal values() As String)
        Me.Text = title
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.StartPosition = FormStartPosition.CenterParent
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.ShowInTaskbar = False

        ReDim boxes(labels.Length - 1)
        For i As Integer = 0 To labels.Length - 1
            Dim lb As New Label
            lb.Text = labels(i)
            lb.Location = New Point(12, 19 + i * 30)
            lb.Size = New Size(105, 18)

            Dim tb As New TextBox
            tb.Location = New Point(122, 16 + i * 30)
            tb.Size = New Size(220, 20)
            tb.MaxLength = 50
            tb.Text = values(i)
            boxes(i) = tb

            Me.Controls.Add(lb)
            Me.Controls.Add(tb)
        Next

        Dim y As Integer = 26 + labels.Length * 30
        Dim ok As New Button
        ok.Text = "Save"
        ok.Location = New Point(186, y)
        ok.Size = New Size(75, 25)
        AddHandler ok.Click, AddressOf OkClicked

        Dim cancel As New Button
        cancel.Text = "Cancel"
        cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        cancel.Location = New Point(267, y)
        cancel.Size = New Size(75, 25)

        Me.Controls.Add(ok)
        Me.Controls.Add(cancel)
        Me.AcceptButton = ok
        Me.CancelButton = cancel
        Me.ClientSize = New Size(360, y + 38)
    End Sub

    Private Sub OkClicked(ByVal sender As Object, ByVal e As EventArgs)
        If boxes(0).Text.Trim().Length = 0 Then
            MessageBox.Show("The first box can't be empty.", "Missing information", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            boxes(0).Focus()
            Return
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    Public Shared Function Edit(ByVal owner As IWin32Window, ByVal title As String, ByVal labels() As String, ByVal values() As String) As String()
        Using f As New RecordDialog(title, labels, values)
            If f.ShowDialog(owner) = System.Windows.Forms.DialogResult.OK Then
                Dim result(labels.Length - 1) As String
                For i As Integer = 0 To labels.Length - 1
                    result(i) = f.boxes(i).Text.Trim()
                Next
                Return result
            End If
        End Using
        Return Nothing
    End Function

End Class
