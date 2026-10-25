Imports System.Drawing
Imports System.Windows.Forms

' A small "type a password" box that hides the characters (InputBox cannot do that).
' Use:  Dim pw As String = PasswordPrompt.Ask("Enter the password:", "Title", Me)
' It returns Nothing if the user cancels.
Public Class PasswordPrompt
    Inherits Form

    Private tb As TextBox

    Private Sub New(ByVal prompt As String, ByVal title As String)
        Me.Text = title
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.StartPosition = FormStartPosition.CenterParent
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.ShowInTaskbar = False
        Me.ClientSize = New Size(340, 130)

        Dim lb As New Label
        lb.Text = prompt
        lb.Location = New Point(12, 12)
        lb.Size = New Size(316, 40)

        tb = New TextBox
        tb.UseSystemPasswordChar = True
        tb.Location = New Point(12, 58)
        tb.Size = New Size(316, 20)

        Dim ok As New Button
        ok.Text = "OK"
        ok.DialogResult = System.Windows.Forms.DialogResult.OK
        ok.Location = New Point(172, 92)
        ok.Size = New Size(75, 25)

        Dim cancel As New Button
        cancel.Text = "Cancel"
        cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        cancel.Location = New Point(253, 92)
        cancel.Size = New Size(75, 25)

        Me.AcceptButton = ok
        Me.CancelButton = cancel
        Me.Controls.Add(lb)
        Me.Controls.Add(tb)
        Me.Controls.Add(ok)
        Me.Controls.Add(cancel)
    End Sub

    Public Shared Function Ask(ByVal prompt As String, ByVal title As String, ByVal owner As IWin32Window) As String
        Using f As New PasswordPrompt(prompt, title)
            If f.ShowDialog(owner) = System.Windows.Forms.DialogResult.OK Then
                Return f.tb.Text
            End If
        End Using
        Return Nothing
    End Function

End Class
