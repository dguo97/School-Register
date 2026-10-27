' Admin home page. The admin can do everything: manage pupils, teachers, subjects and clubs,
' edit the timetable, assign pupils to clubs/subjects, and see the uniform and school bus lists. (The admin password is changed
' from the sign-in screen.)
Public Class Adminhome

    Private Sub OpenManage(ByVal kind As ManageKind, ByVal canResetPassword As Boolean)
        Using f As New ManageForm
            f.Kind = kind
            f.CanAdd = True
            f.CanEdit = True
            f.CanDelete = True
            f.CanResetPassword = canResetPassword
            f.ShowDialog(Me)
        End Using
    End Sub

    Private Sub btpupils_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btpupils.Click
        OpenManage(ManageKind.Pupils, False)
    End Sub

    Private Sub btteachers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btteachers.Click
        OpenManage(ManageKind.Teachers, True)
    End Sub

    Private Sub btsubjects_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btsubjects.Click
        OpenManage(ManageKind.Subjects, False)
    End Sub

    Private Sub btclubs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btclubs.Click
        OpenManage(ManageKind.Clubs, False)
    End Sub

    Private Sub bttimetable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttimetable.Click
        Using f As New TimetableForm
            f.CanEdit = True
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

    Private Sub btlogout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btlogout.Click
        signin.Show()
        Me.Close()
    End Sub

End Class
