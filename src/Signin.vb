Imports System.Data.OleDb

' Sign-in screen. Teachers sign in with their Teacher ID + password (teachersdetail table).
' The admin tab uses the single admin password (adminpassword table).
' After Db.MaxFailedLogins wrong passwords an account is locked for Db.LockMinutes minutes.
Public Class signin

    Private adminpassword As String = Nothing     ' stored value (hash, or an old plain-text password)
    Private adminRowExists As Boolean = False
    Private adminAttempts As Integer = 0
    Private adminLockedUntil As Object = DBNull.Value

    Private Sub signin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.AcceptButton = sib
        Dim ready As Boolean = False
        Try
            DbSchema.EnsureSchema()      'creates / adds any missing tables and columns (safe to repeat)
            ready = True
        Catch ex As Exception
            Db.ShowError(ex)
        End Try
        If ready Then
            LoadAdminPassword()
        Else
            TabControl1.Enabled = False      'the database could not be prepared: close and fix it first
        End If
    End Sub

    Private Sub signin_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        'when the user re-opens this page, all the text input will be cleared.
        untb.Text = ""
        pwtx.Text = ""
        tbsp.Text = ""
    End Sub

    Private Sub LoadAdminPassword()
        adminpassword = Nothing
        adminRowExists = False
        adminAttempts = 0
        adminLockedUntil = DBNull.Value
        Try
            Using con As OleDbConnection = Db.OpenConnection()
                Using cmd As New OleDbCommand("SELECT PasswordHash, FailedAttempts, LockedUntil FROM adminpassword", con)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            adminRowExists = True
                            If Not IsDBNull(reader(0)) Then
                                adminpassword = reader(0).ToString()
                            End If
                            adminAttempts = Db.ToInt(reader(1))
                            adminLockedUntil = reader(2)
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Db.ShowError(ex)
        End Try
    End Sub

    ' Saves a new admin password (hashed). Plain UPDATE/INSERT because the table has no primary key.
    Private Sub SaveAdminPassword(ByVal newPassword As String)
        Dim hashed As String = Db.HashPassword(newPassword)
        Using con As OleDbConnection = Db.OpenConnection()
            Dim sql As String
            If adminRowExists Then
                sql = "UPDATE adminpassword SET PasswordHash = ?"
            Else
                sql = "INSERT INTO adminpassword (PasswordHash) VALUES (?)"
            End If
            Using cmd As New OleDbCommand(sql, con)
                cmd.Parameters.AddWithValue("@pw", hashed)
                cmd.ExecuteNonQuery()
            End Using
        End Using
        adminpassword = hashed
        adminRowExists = True
    End Sub

    ' ---------------- Teacher sign-in ----------------

    Private Sub sib_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sib.Click
        Dim teacherId As Integer
        If Not Db.TryParseTeacherId(untb.Text, teacherId) OrElse pwtx.Text.Length = 0 Then
            MessageBox.Show("Please enter your Teacher ID (for example T7) and your password.", "Sign in", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim fullName As String = ""
        Dim loggedIn As Boolean = False
        Dim lockedMinutes As Integer = 0

        Try
            Using con As OleDbConnection = Db.OpenConnection()
                Dim stored As String = Nothing
                Dim found As Boolean = False
                Dim attempts As Integer = 0
                Dim lockedUntil As Object = DBNull.Value

                Using cmd As New OleDbCommand("SELECT FirstName, SecondName, PasswordHash, FailedAttempts, LockedUntil FROM teachersdetail WHERE TeacherID = ?", con)
                    cmd.Parameters.AddWithValue("@id", teacherId)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            found = True
                            fullName = (reader("FirstName").ToString() & " " & reader("SecondName").ToString()).Trim()
                            If Not IsDBNull(reader("PasswordHash")) Then
                                stored = reader("PasswordHash").ToString()
                            End If
                            attempts = Db.ToInt(reader("FailedAttempts"))
                            lockedUntil = reader("LockedUntil")
                        End If
                    End Using
                End Using

                If found Then
                    lockedMinutes = Db.MinutesLocked(lockedUntil)
                    If lockedMinutes = 0 Then
                        Dim needsUpgrade As Boolean = False
                        If Db.VerifyPassword(pwtx.Text, stored, needsUpgrade) Then
                            loggedIn = True
                            If needsUpgrade Then
                                'old plain-text password: replace it with a hashed one
                                Db.Exec(con, Nothing, "UPDATE teachersdetail SET PasswordHash = ? WHERE TeacherID = ?", Db.HashPassword(pwtx.Text), teacherId)
                            End If
                            If attempts > 0 OrElse Not IsDBNull(lockedUntil) Then
                                Db.ClearFailures(con, "teachersdetail", "TeacherID", teacherId)
                            End If
                        ElseIf Db.RecordFailure(con, "teachersdetail", "TeacherID", teacherId, attempts) Then
                            lockedMinutes = Db.LockMinutes
                        End If
                    End If
                End If
            End Using
        Catch ex As Exception
            Db.ShowError(ex)
            Return
        End Try

        If lockedMinutes > 0 Then
            MessageBox.Show("Too many wrong attempts. This account is locked for about " & lockedMinutes & " minute(s).", "Account locked", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            pwtx.Clear()
            Return
        End If

        If Not loggedIn Then
            'same message whether the ID or the password was wrong, so nothing is given away
            MessageBox.Show("Teacher ID or password is incorrect.", "Sign in", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            pwtx.Clear()
            Return
        End If

        teacherhomepage.TeacherName = fullName
        teacherhomepage.TeacherId = teacherId
        teacherhomepage.Show()
        Me.Close()
    End Sub

    ' ---------------- Admin ----------------

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        If TabControl1.SelectedTab Is TabPageA Then
            Me.AcceptButton = btali      'Enter key = admin Log in
            If String.IsNullOrEmpty(adminpassword) Then
                SetFirstAdminPassword()
            End If
        Else
            Me.AcceptButton = sib        'Enter key = teacher Sign in
        End If
    End Sub

    ' First run: no admin password exists yet, so ask for one.
    Private Sub SetFirstAdminPassword()
        Dim pw As String = PasswordPrompt.Ask("No admin password has been set yet." & vbNewLine & _
                           "Choose one (at least " & Db.MinPasswordLength & " characters):", "Set admin password", Me)
        Dim ok As Boolean = False

        If pw Is Nothing Then
            'cancelled
        ElseIf pw.Length < Db.MinPasswordLength Then
            MessageBox.Show("The password must be at least " & Db.MinPasswordLength & " characters.", "Set admin password")
        Else
            Dim again As String = PasswordPrompt.Ask("Type the password again to confirm:", "Set admin password", Me)
            If again <> pw Then
                MessageBox.Show("The passwords did not match.", "Set admin password")
            Else
                Try
                    SaveAdminPassword(pw)
                    ok = True
                Catch ex As Exception
                    Db.ShowError(ex)
                End Try
            End If
        End If

        If Not ok Then
            TabControl1.SelectedTab = TabPageR     'go back to the teacher tab
        End If
    End Sub

    Private Sub btali_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btali.Click
        Dim minutes As Integer = Db.MinutesLocked(adminLockedUntil)
        If minutes > 0 Then
            MessageBox.Show("Too many wrong attempts. Admin login is locked for about " & minutes & " minute(s).", "Account locked", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            tbsp.Clear()
            Return
        End If

        Dim needsUpgrade As Boolean = False
        If Not Db.VerifyPassword(tbsp.Text, adminpassword, needsUpgrade) Then
            Dim nowLocked As Boolean = False
            Try
                If adminRowExists Then
                    Using con As OleDbConnection = Db.OpenConnection()
                        nowLocked = Db.RecordFailure(con, "adminpassword", "", 0, adminAttempts)
                    End Using
                    If nowLocked Then
                        adminAttempts = 0
                        adminLockedUntil = Date.Now.AddMinutes(Db.LockMinutes)
                    Else
                        adminAttempts += 1
                    End If
                End If
            Catch ex As Exception
                Db.ShowError(ex)
            End Try
            If nowLocked Then
                MessageBox.Show("Too many wrong attempts. Admin login is locked for " & Db.LockMinutes & " minutes.", "Account locked", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Else
                MessageBox.Show("Incorrect password", "Admin", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
            tbsp.Clear()
            Return
        End If

        Try
            If needsUpgrade Then
                SaveAdminPassword(tbsp.Text)
            End If
            If adminAttempts > 0 OrElse Not IsDBNull(adminLockedUntil) Then
                Using con As OleDbConnection = Db.OpenConnection()
                    Db.ClearFailures(con, "adminpassword", "", 0)
                End Using
            End If
        Catch ex As Exception
            Db.ShowError(ex)
            Return
        End Try

        Adminhome.Show()
        Me.Close()
    End Sub

    Private Sub lbcp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbcp.Click
        'admin clicked the "change password" label
        Dim needsUpgrade As Boolean = False
        Dim oldpw As String = PasswordPrompt.Ask("Enter the current admin password:", "Change admin password", Me)
        If oldpw Is Nothing Then
            Return
        End If
        If Not Db.VerifyPassword(oldpw, adminpassword, needsUpgrade) Then
            MessageBox.Show("Wrong password", "Change admin password")
            Return
        End If

        Dim np As String = PasswordPrompt.Ask("Enter the new password (at least " & Db.MinPasswordLength & " characters):", "Change admin password", Me)
        If np Is Nothing OrElse np.Length < Db.MinPasswordLength Then
            MessageBox.Show("Password not changed (it must be at least " & Db.MinPasswordLength & " characters).", "Change admin password")
            Return
        End If
        Dim again As String = PasswordPrompt.Ask("Type the new password again to confirm:", "Change admin password", Me)
        If again <> np Then
            MessageBox.Show("The passwords did not match. Password not changed.", "Change admin password")
            Return
        End If

        Try
            SaveAdminPassword(np)
            MessageBox.Show("Admin password changed.", "Change admin password")
        Catch ex As Exception
            Db.ShowError(ex)
        End Try
    End Sub

End Class
