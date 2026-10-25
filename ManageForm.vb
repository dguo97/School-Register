Imports System.Data.OleDb
Imports System.Drawing
Imports System.Windows.Forms

Public Enum ManageKind
    Pupils
    Teachers
    Subjects
    Clubs
    BusList
    Uniform
End Enum

' One screen for looking at, searching, adding, editing and deleting
' pupils, teachers, subjects, clubs, the school bus list or the uniform list.
' Set the properties, then call ShowDialog.
'   Kind                      what to show
'   CanAdd / CanEdit / CanDelete / CanResetPassword   which buttons appear
Public Class ManageForm
    Inherits Form

    Public Kind As ManageKind = ManageKind.Pupils
    Public CanAdd As Boolean = False
    Public CanEdit As Boolean = False
    Public CanDelete As Boolean = False
    Public CanResetPassword As Boolean = False

    Private WithEvents tbsearch As TextBox
    Private WithEvents dgv As DataGridView
    Private WithEvents btadd As Button
    Private WithEvents btedit As Button
    Private WithEvents btdelete As Button
    Private WithEvents btreset As Button
    Private WithEvents bttotals As Button
    Private WithEvents cbyear As ComboBox
    Private WithEvents cbgender As ComboBox
    Private WithEvents cbbus As ComboBox
    Private WithEvents btclose As Button
    Private lbcount As Label
    Private view As DataView

    Public Sub New()
        Me.Text = "List"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.ShowInTaskbar = False
        Me.MinimumSize = New Size(560, 320)
        Me.ClientSize = New Size(780, 440)

        Dim lbs As New Label
        lbs.Text = "Search:"
        lbs.Location = New Point(12, 16)
        lbs.Size = New Size(50, 18)

        tbsearch = New TextBox
        tbsearch.Location = New Point(66, 13)
        tbsearch.Size = New Size(190, 20)

        cbyear = MakeCombo(264, 100)
        cbgender = MakeCombo(370, 90)
        cbbus = MakeCombo(466, 100)

        lbcount = New Label
        lbcount.Location = New Point(580, 16)
        lbcount.Size = New Size(190, 18)

        dgv = New DataGridView
        dgv.Location = New Point(12, 44)
        dgv.Size = New Size(756, 340)
        dgv.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False
        dgv.ReadOnly = True
        dgv.MultiSelect = False
        dgv.RowHeadersVisible = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        btadd = MakeButton("Add", 80)
        btedit = MakeButton("Edit", 80)
        btdelete = MakeButton("Delete", 80)
        btreset = MakeButton("Reset password", 120)
        bttotals = MakeButton("Totals (rows shown)", 140)
        btclose = MakeButton("Close", 80)
        btclose.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btclose.Left = 688
        btclose.DialogResult = System.Windows.Forms.DialogResult.Cancel

        Me.Controls.Add(lbs)
        Me.Controls.Add(tbsearch)
        Me.Controls.Add(cbyear)
        Me.Controls.Add(cbgender)
        Me.Controls.Add(cbbus)
        Me.Controls.Add(lbcount)
        Me.Controls.Add(dgv)
        Me.Controls.Add(btadd)
        Me.Controls.Add(btedit)
        Me.Controls.Add(btdelete)
        Me.Controls.Add(btreset)
        Me.Controls.Add(bttotals)
        Me.Controls.Add(btclose)
        Me.CancelButton = btclose
    End Sub

    Private Function MakeCombo(ByVal left As Integer, ByVal width As Integer) As ComboBox
        Dim c As New ComboBox
        c.DropDownStyle = ComboBoxStyle.DropDownList
        c.Location = New Point(left, 13)
        c.Size = New Size(width, 21)
        Return c
    End Function

    Private Function MakeButton(ByVal text As String, ByVal width As Integer) As Button
        Dim b As New Button
        b.Text = text
        b.Size = New Size(width, 28)
        b.Top = 400
        b.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left
        Return b
    End Function

    Private Sub ManageForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'NOTE: a control's Visible property reads False until the form is shown, so the
        'layout below works from these yes/no values instead of reading Visible back.
        Dim buttons() As Button = New Button() {btadd, btedit, btdelete, btreset, bttotals}
        Dim wanted() As Boolean = New Boolean() {CanAdd, CanEdit, CanDelete, (CanResetPassword AndAlso Kind = ManageKind.Teachers), (Kind = ManageKind.Uniform)}

        'filters: Year / Gender / School bus for the pupil list, Year only for the bus and uniform lists
        cbyear.Items.Add("All years")
        cbyear.Items.AddRange(Db.YearLevels)
        cbyear.SelectedIndex = 0
        cbgender.Items.AddRange(New Object() {"All genders", "Male", "Female"})
        cbgender.SelectedIndex = 0
        cbbus.Items.AddRange(New Object() {"Bus: all", "Uses bus", "No bus"})
        cbbus.SelectedIndex = 0
        cbyear.Visible = ShowsYearFilter()
        cbgender.Visible = ShowsPupilFilters()
        cbbus.Visible = ShowsPupilFilters()

        If Kind = ManageKind.Uniform Then
            btedit.Text = "Edit uniform"
            btedit.Width = 100
        ElseIf Kind = ManageKind.BusList Then
            btedit.Text = "Edit bus details"
            btedit.Width = 120
        End If

        Dim x As Integer = 12
        For i As Integer = 0 To buttons.Length - 1
            buttons(i).Visible = wanted(i)
            If wanted(i) Then
                buttons(i).Left = x
                x += buttons(i).Width + 8
            End If
        Next

        Select Case Kind
            Case ManageKind.Pupils
                Me.Text = "Pupils"
            Case ManageKind.Teachers
                Me.Text = "Teachers"
            Case ManageKind.Subjects
                Me.Text = "Subjects"
            Case ManageKind.BusList
                Me.Text = "School bus list"
            Case ManageKind.Uniform
                Me.Text = "Uniform"
            Case Else
                Me.Text = "Clubs"
        End Select
        LoadData()
    End Sub

    Private Function BuildQuery(ByVal con As OleDbConnection) As String
        Select Case Kind
            Case ManageKind.Pupils
                Return "SELECT StudentID AS RawID, 'S' & StudentID AS ID, FirstName, SecondName, Gender, YearGroup AS [Year], DOB, HouseNumber, PostCode, PhoneNumber, Phone2, Email, Schoolbus " & _
                       "FROM studentsdetail ORDER BY SecondName, FirstName"
            Case ManageKind.Teachers
                Return "SELECT TeacherID AS RawID, 'T' & TeacherID AS ID, FirstName, SecondName, " & Db.TeacherHouseColumn(con) & " AS HouseNumber, PhoneNumber, DOB " & _
                       "FROM teachersdetail ORDER BY SecondName, FirstName"
            Case ManageKind.Subjects
                Dim col As String = DbSchema.SubjectNameColumn(con)
                Return "SELECT SubjectID AS RawID, " & col & " AS Subject, Teacher, Room FROM Subjects ORDER BY " & col
            Case ManageKind.BusList
                Return "SELECT s.StudentID AS RawID, 'S' & s.StudentID AS ID, s.FirstName, s.SecondName, s.YearGroup AS [Year], s.PhoneNumber, b.Address, b.DistanceKm AS [Distance " & Db.DistanceUnit & "] " & _
                       "FROM studentsdetail AS s LEFT JOIN BusList AS b ON s.StudentID = b.StudentID WHERE s.Schoolbus = 'Yes' ORDER BY s.SecondName, s.FirstName"
            Case ManageKind.Uniform
                Dim sb As New System.Text.StringBuilder
                sb.Append("SELECT s.StudentID AS RawID, 'S' & s.StudentID AS ID, s.FirstName, s.SecondName, s.YearGroup AS [Year]")
                For i As Integer = 0 To Db.UniformColumns.Length - 1
                    sb.Append(", IIf(IsNull(u.").Append(Db.UniformColumns(i)).Append("), 0, u.").Append(Db.UniformColumns(i)).Append(") AS [").Append(Db.UniformLabels(i)).Append("]")
                Next
                sb.Append(" FROM studentsdetail AS s LEFT JOIN Uniform AS u ON s.StudentID = u.StudentID ORDER BY s.SecondName, s.FirstName")
                Return sb.ToString()
            Case Else
                Return "SELECT Clubs.Clubs AS RawID, Clubs.ClubName, Clubs.Teacher, Clubs.Location, Clubs.Days FROM Clubs ORDER BY Clubs.ClubName"
        End Select
    End Function

    Private Sub LoadData()
        Try
            Using con As OleDbConnection = Db.OpenConnection()
                Using adp As New OleDbDataAdapter(BuildQuery(con), con)
                    Dim dt As New DataTable
                    adp.Fill(dt)
                    view = New DataView(dt)
                    dgv.DataSource = view
                End Using
            End Using
            If dgv.Columns.Contains("RawID") Then
                dgv.Columns("RawID").Visible = False
            End If
            If dgv.Columns.Contains("DOB") Then
                dgv.Columns("DOB").DefaultCellStyle.Format = "d"
            End If
            ApplyFilter()
        Catch ex As Exception
            Db.ShowError(ex)
        End Try
    End Sub

    Private Sub ApplyFilter()
        If view Is Nothing Then
            Return
        End If
        Dim parts As New List(Of String)
        Dim search As String = Db.BuildRowFilter(view.Table, tbsearch.Text)
        If search.Length > 0 Then
            parts.Add("(" & search & ")")
        End If
        If ShowsYearFilter() AndAlso cbyear.SelectedIndex > 0 Then
            parts.Add("[Year] = '" & cbyear.SelectedItem.ToString().Replace("'", "''") & "'")
        End If
        If ShowsPupilFilters() AndAlso cbgender.SelectedIndex > 0 Then
            parts.Add("[Gender] = '" & If(cbgender.SelectedIndex = 1, "M", "F") & "'")
        End If
        If ShowsPupilFilters() AndAlso cbbus.SelectedIndex > 0 Then
            parts.Add("[Schoolbus] = '" & If(cbbus.SelectedIndex = 1, "Yes", "No") & "'")
        End If
        Try
            view.RowFilter = String.Join(" AND ", parts.ToArray())
        Catch ex As Exception
            view.RowFilter = ""
        End Try
        lbcount.Text = view.Count & " record(s)"
    End Sub

    Private Function ShowsYearFilter() As Boolean
        Return Kind = ManageKind.Pupils OrElse Kind = ManageKind.BusList OrElse Kind = ManageKind.Uniform
    End Function

    Private Function ShowsPupilFilters() As Boolean
        Return Kind = ManageKind.Pupils
    End Function

    Private Sub FilterChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbyear.SelectedIndexChanged, cbgender.SelectedIndexChanged, cbbus.SelectedIndexChanged
        ApplyFilter()
    End Sub

    Private Sub tbsearch_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbsearch.TextChanged
        ApplyFilter()
    End Sub

    Private Function SelectedRow() As DataRowView
        If dgv.CurrentRow Is Nothing Then
            Return Nothing
        End If
        Return TryCast(dgv.CurrentRow.DataBoundItem, DataRowView)
    End Function

    Private Function DescribeRow(ByVal row As DataRowView) As String
        Select Case Kind
            Case ManageKind.Pupils, ManageKind.Teachers
                Return (row("ID").ToString() & " " & row("FirstName").ToString() & " " & row("SecondName").ToString()).Trim()
            Case ManageKind.Subjects
                Return row("Subject").ToString()
            Case ManageKind.BusList, ManageKind.Uniform
                Return (row("ID").ToString() & " " & row("FirstName").ToString() & " " & row("SecondName").ToString()).Trim()
            Case Else
                Return row("ClubName").ToString()
        End Select
    End Function

    ' ---------------- Add / Edit ----------------

    Private Sub btadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btadd.Click
        OpenEditor(0)
    End Sub

    Private Sub btedit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btedit.Click
        EditSelected()
    End Sub

    Private Sub dgv_CellDoubleClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dgv.CellDoubleClick
        If CanEdit AndAlso e.RowIndex >= 0 Then
            EditSelected()
        End If
    End Sub

    Private Sub EditSelected()
        Dim row As DataRowView = SelectedRow()
        If row Is Nothing Then
            MessageBox.Show("Please select a row first.", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        OpenEditor(Convert.ToInt32(row("RawID")))
    End Sub

    ' id = 0 means "add new"
    Private Sub OpenEditor(ByVal id As Integer)
        If Kind = ManageKind.Pupils OrElse Kind = ManageKind.Teachers OrElse Kind = ManageKind.BusList Then
            'the bus list is edited through the pupil's own record
            Using f As New RegisterForm
                f.IsTeacher = (Kind = ManageKind.Teachers)
                f.EditId = id
                If f.ShowDialog(Me) = DialogResult.OK Then
                    LoadData()
                End If
            End Using
            Return
        End If

        If Kind = ManageKind.Uniform Then
            Using f As New UniformForm
                f.StudentId = id
                If f.ShowDialog(Me) = DialogResult.OK Then
                    LoadData()
                End If
            End Using
            Return
        End If

        'subjects and clubs use the simple dialog
        Dim labels() As String
        Dim values() As String
        Dim row As DataRowView = Nothing
        If id > 0 Then
            row = SelectedRow()
        End If

        If Kind = ManageKind.Subjects Then
            labels = New String() {"Subject name", "Teacher", "Room"}
            values = New String() {"", "", ""}
            If row IsNot Nothing Then
                values = New String() {row("Subject").ToString(), row("Teacher").ToString(), row("Room").ToString()}
            End If
        Else
            labels = New String() {"Club name", "Teacher", "Location", "Days"}
            values = New String() {"", "", "", ""}
            If row IsNot Nothing Then
                values = New String() {row("ClubName").ToString(), row("Teacher").ToString(), row("Location").ToString(), row("Days").ToString()}
            End If
        End If

        Dim result() As String = RecordDialog.Edit(Me, Me.Text, labels, values)
        If result Is Nothing Then
            Return
        End If

        Try
            Using con As OleDbConnection = Db.OpenConnection()
                If Kind = ManageKind.Subjects Then
                    Dim col As String = DbSchema.SubjectNameColumn(con)
                    If id = 0 Then
                        Db.Exec(con, Nothing, "INSERT INTO Subjects (" & col & ", Teacher, Room) VALUES (?, ?, ?)", result(0), result(1), result(2))
                    Else
                        Db.Exec(con, Nothing, "UPDATE Subjects SET " & col & " = ?, Teacher = ?, Room = ? WHERE SubjectID = ?", result(0), result(1), result(2), id)
                    End If
                Else
                    If id = 0 Then
                        Db.Exec(con, Nothing, "INSERT INTO Clubs (ClubName, Teacher, Location, Days) VALUES (?, ?, ?, ?)", result(0), result(1), result(2), result(3))
                    Else
                        Db.Exec(con, Nothing, "UPDATE Clubs SET ClubName = ?, Teacher = ?, Location = ?, Days = ? WHERE Clubs.Clubs = ?", result(0), result(1), result(2), result(3), id)
                    End If
                End If
            End Using
            LoadData()
        Catch ex As Exception
            Db.ShowError(ex)
        End Try
    End Sub

    ' ---------------- Delete ----------------

    Private Sub btdelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btdelete.Click
        Dim row As DataRowView = SelectedRow()
        If row Is Nothing Then
            MessageBox.Show("Please select a row first.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim id As Integer = Convert.ToInt32(row("RawID"))
        Dim label As String = DescribeRow(row)

        If MessageBox.Show("Delete " & label & "?" & vbNewLine & "This cannot be undone.", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            Return
        End If

        Try
            Using con As OleDbConnection = Db.OpenConnection()
                Dim tx As OleDbTransaction = con.BeginTransaction()
                Try
                    Select Case Kind
                        Case ManageKind.Pupils
                            Db.Exec(con, tx, "DELETE FROM StudentClubs WHERE StudentID = ?", id)
                            Db.Exec(con, tx, "DELETE FROM StudentSubjects WHERE StudentID = ?", id)
                            Db.Exec(con, tx, "DELETE FROM BusList WHERE StudentID = ?", id)
                            Db.Exec(con, tx, "DELETE FROM Uniform WHERE StudentID = ?", id)
                            Db.Exec(con, tx, "DELETE FROM studentsdetail WHERE StudentID = ?", id)
                        Case ManageKind.Teachers
                            Db.Exec(con, tx, "DELETE FROM teachersdetail WHERE TeacherID = ?", id)
                        Case ManageKind.Subjects
                            Db.Exec(con, tx, "DELETE FROM Timetable WHERE SubjectID = ?", id)
                            Db.Exec(con, tx, "DELETE FROM StudentSubjects WHERE SubjectID = ?", id)
                            Db.Exec(con, tx, "DELETE FROM Subjects WHERE SubjectID = ?", id)
                        Case Else
                            Db.Exec(con, tx, "DELETE FROM StudentClubs WHERE ClubID = ?", id)
                            Db.Exec(con, tx, "DELETE FROM Clubs WHERE Clubs.Clubs = ?", id)
                    End Select
                    tx.Commit()
                Catch
                    tx.Rollback()
                    Throw
                End Try
            End Using
            LoadData()
        Catch ex As Exception
            Db.ShowError(ex)
        End Try
    End Sub

    ' ---------------- Uniform totals ----------------

    ' Adds up each uniform item over the pupils currently shown (so filter by Year first for one year).
    Private Sub bttotals_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttotals.Click
        If view Is Nothing Then
            Return
        End If
        Dim sb As New System.Text.StringBuilder
        sb.Append("Totals for the ").Append(view.Count).Append(" pupil(s) shown:").Append(vbNewLine).Append(vbNewLine)
        For i As Integer = 0 To Db.UniformLabels.Length - 1
            Dim total As Integer = 0
            For Each drv As DataRowView In view
                total += Db.ToInt(drv(Db.UniformLabels(i)))
            Next
            sb.Append(Db.UniformLabels(i)).Append(": ").Append(total).Append(vbNewLine)
        Next
        MessageBox.Show(sb.ToString(), "Uniform totals", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' ---------------- Reset a teacher's password ----------------

    Private Sub btreset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btreset.Click
        Dim row As DataRowView = SelectedRow()
        If row Is Nothing Then
            MessageBox.Show("Please select a teacher first.", "Reset password", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim id As Integer = Convert.ToInt32(row("RawID"))
        Dim label As String = DescribeRow(row)

        Dim pw As String = PasswordPrompt.Ask("New password for " & label & " (at least " & Db.MinPasswordLength & " characters):", "Reset password", Me)
        If pw Is Nothing Then
            Return
        End If
        If pw.Length < Db.MinPasswordLength Then
            MessageBox.Show("The password must be at least " & Db.MinPasswordLength & " characters.", "Reset password")
            Return
        End If
        Dim again As String = PasswordPrompt.Ask("Type the new password again to confirm:", "Reset password", Me)
        If again <> pw Then
            MessageBox.Show("The passwords did not match. Password not changed.", "Reset password")
            Return
        End If

        Try
            Using con As OleDbConnection = Db.OpenConnection()
                Db.Exec(con, Nothing, "UPDATE teachersdetail SET PasswordHash = ? WHERE TeacherID = ?", Db.HashPassword(pw), id)
                Db.ClearFailures(con, "teachersdetail", "TeacherID", id)     'also unlocks the account
            End Using
            MessageBox.Show("Password reset for " & label & ".", "Reset password", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            Db.ShowError(ex)
        End Try
    End Sub

End Class
