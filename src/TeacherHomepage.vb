Imports System.Data.OleDb

' Teacher home page. Teachers can add and edit pupils, view subjects, clubs, the timetable, the uniform
' and school bus lists, assign pupils to clubs/subjects, edit their own profile and change their own password.
' (Only the admin can delete records or edit subjects, clubs and the timetable.)
Public Class teacherhomepage

    ' Both are set by the sign-in form before this page is shown.
    Public TeacherName As String = ""
    Public TeacherId As Integer = 0

    Private Sub teacherhomepage_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If TeacherName.Length > 0 Then
            Me.Text = "School Register - " & TeacherName & " (" & Db.FormatTeacherId(TeacherId) & ")"
        End If
    End Sub

    Private Sub btstudents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btstudents.Click
        Using f As New ManageForm
            f.Kind = ManageKind.Pupils
            f.CanAdd = True
            f.CanEdit = True
            f.CanDelete = False
            f.ShowDialog(Me)
        End Using
    End Sub

    Private Sub btsubjects_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btsubjects.Click
        Using f As New ManageForm
            f.Kind = ManageKind.Subjects
            f.ShowDialog(Me)
        End Using
    End Sub

    Private Sub btclubs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btclubs.Click
        Using f As New ManageForm
            f.Kind = ManageKind.Clubs
            f.ShowDialog(Me)
        End Using
    End Sub

    Private Sub bttimetable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttimetable.Click
        Using f As New TimetableForm
            f.CanEdit = False
            f.ShowDialog(Me)
        End Using
    End Sub

    Private Sub btenrol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btenrol.Click
        Using f As New EnrolForm
            f.ShowDialog(Me)
        End Using
    End Sub

    Private Sub btuniform_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btuniform.Click
        Using f As New ManageForm
            f.Kind = ManageKind.Uniform
            f.CanEdit = True
            f.ShowDialog(Me)
        End Using
    End Sub

    Private Sub btbus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btbus.Click
        Using f As New ManageForm
            f.Kind = ManageKind.BusList
            f.CanEdit = True
            f.ShowDialog(Me)
        End Using
    End Sub

    ' Teachers view and change their own details here.
    Private Sub btprofile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btprofile.Click
        If TeacherId <= 0 Then
            Return
        End If
        Using f As New RegisterForm
            f.IsTeacher = True
            f.EditId = TeacherId
            f.ShowDialog(Me)
        End Using
    End Sub

    ' Teachers change their own password here.
    Private Sub btchangepw_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btchangepw.Click
        If TeacherId <= 0 Then
            Return
        End If

        Dim oldpw As String = PasswordPrompt.Ask("Enter your current password:", "Change password", Me)
        If oldpw Is Nothing Then
            Return
        End If

        Dim stored As String = Nothing
        Try
            Using con As OleDbConnection = Db.OpenConnection()
                Using cmd As New OleDbCommand("SELECT PasswordHash FROM teachersdetail WHERE TeacherID = ?", con)
                    cmd.Parameters.AddWithValue("@id", TeacherId)
                    Dim result As Object = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        stored = result.ToString()
                    End If
                End Using
            End Using
        Catch ex As Exception
            Db.ShowError(ex)
            Return
        End Try

        Dim unused As Boolean
        If Not Db.VerifyPassword(oldpw, stored, unused) Then
            MessageBox.Show("That is not your current password.", "Change password", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim np As String = PasswordPrompt.Ask("Enter your new password (at least " & Db.MinPasswordLength & " characters):", "Change password", Me)
        If np Is Nothing OrElse np.Length < Db.MinPasswordLength Then
            MessageBox.Show("Password not changed (it must be at least " & Db.MinPasswordLength & " characters).", "Change password")
            Return
        End If
        Dim again As String = PasswordPrompt.Ask("Type the new password again to confirm:", "Change password", Me)
        If again <> np Then
            MessageBox.Show("The passwords did not match. Password not changed.", "Change password")
            Return
        End If

        Try
            Using con As OleDbConnection = Db.OpenConnection()
                Db.Exec(con, Nothing, "UPDATE teachersdetail SET PasswordHash = ? WHERE TeacherID = ?", Db.HashPassword(np), TeacherId)
            End Using
            MessageBox.Show("Your password has been changed.", "Change password", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            Db.ShowError(ex)
        End Try
    End Sub

    Private Sub btlogout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btlogout.Click
        signin.Show()
        Me.Close()
    End Sub

End Class
