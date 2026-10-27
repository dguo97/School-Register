Imports System.Data.OleDb
Imports System.Drawing
Imports System.Windows.Forms

' Weekly timetable grid (Monday-Friday, periods 1-6).
' CanEdit = True shows the controls for adding / clearing slots (admin).
' Set CanEdit before calling ShowDialog.
Public Class TimetableForm
    Inherits Form

    Public CanEdit As Boolean = False

    Private Const Periods As Integer = 6
    Private ReadOnly DayNames() As String = New String() {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday"}

    Private WithEvents cbsubject As ComboBox
    Private WithEvents cbday As ComboBox
    Private WithEvents cbperiod As ComboBox
    Private WithEvents btadd As Button
    Private WithEvents btclear As Button
    Private WithEvents btclose As Button
    Private WithEvents dgv As DataGridView
    Private lbhelp As Label
    Private editControls As New List(Of Control)

    Public Sub New()
        Me.Text = "Timetable"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.ShowInTaskbar = False
        Me.MinimumSize = New Size(640, 420)
        Me.ClientSize = New Size(860, 520)

        cbsubject = New ComboBox
        cbsubject.DropDownStyle = ComboBoxStyle.DropDownList
        cbsubject.Location = New Point(12, 12)
        cbsubject.Size = New Size(210, 21)

        cbday = New ComboBox
        cbday.DropDownStyle = ComboBoxStyle.DropDownList
        cbday.Location = New Point(230, 12)
        cbday.Size = New Size(100, 21)
        cbday.Items.AddRange(New Object() {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday"})
        cbday.SelectedIndex = 0

        cbperiod = New ComboBox
        cbperiod.DropDownStyle = ComboBoxStyle.DropDownList
        cbperiod.Location = New Point(338, 12)
        cbperiod.Size = New Size(80, 21)
        For p As Integer = 1 To Periods
            cbperiod.Items.Add("Period " & p)
        Next
        cbperiod.SelectedIndex = 0

        btadd = New Button
        btadd.Text = "Add to timetable"
        btadd.Location = New Point(428, 10)
        btadd.Size = New Size(120, 25)

        btclear = New Button
        btclear.Text = "Clear selected slot"
        btclear.Location = New Point(556, 10)
        btclear.Size = New Size(130, 25)

        editControls.Add(cbsubject)
        editControls.Add(cbday)
        editControls.Add(cbperiod)
        editControls.Add(btadd)
        editControls.Add(btclear)

        dgv = New DataGridView
        dgv.Location = New Point(12, 46)
        dgv.Size = New Size(836, 424)
        dgv.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False
        dgv.AllowUserToResizeRows = False
        dgv.ReadOnly = True
        dgv.MultiSelect = False
        dgv.SelectionMode = DataGridViewSelectionMode.CellSelect
        dgv.RowHeadersWidth = 70
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True

        lbhelp = New Label
        lbhelp.Location = New Point(12, 484)
        lbhelp.Size = New Size(700, 18)
        lbhelp.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left

        btclose = New Button
        btclose.Text = "Close"
        btclose.Size = New Size(80, 28)
        btclose.Location = New Point(768, 480)
        btclose.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btclose.DialogResult = System.Windows.Forms.DialogResult.Cancel

        For Each c As Control In editControls
            Me.Controls.Add(c)
        Next
        Me.Controls.Add(dgv)
        Me.Controls.Add(lbhelp)
        Me.Controls.Add(btclose)
        Me.CancelButton = btclose
    End Sub

    Private Sub TimetableForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        For Each c As Control In editControls
            c.Visible = CanEdit
        Next
        If CanEdit Then
            lbhelp.Text = "To clear a slot, click its cell and press 'Clear selected slot'."
        Else
            lbhelp.Text = "View only."
        End If
        If Not CanEdit Then
            'no edit bar, so use the space for the grid
            dgv.Top = 12
            dgv.Height = 458
        End If

        For d As Integer = 0 To DayNames.Length - 1
            dgv.Columns.Add("day" & d, DayNames(d))
        Next
        dgv.Rows.Add(Periods)
        For r As Integer = 0 To Periods - 1
            dgv.Rows(r).HeaderCell.Value = "Period " & (r + 1)
        Next

        If CanEdit Then
            LoadSubjects()
        End If
        RefreshGrid()
    End Sub

    Private Sub LoadSubjects()
        Try
            Using con As OleDbConnection = Db.OpenConnection()
                Dim col As String = DbSchema.SubjectNameColumn(con)
                Using adp As New OleDbDataAdapter("SELECT SubjectID, " & col & " AS SubjectName FROM Subjects ORDER BY " & col, con)
                    Dim dt As New DataTable
                    adp.Fill(dt)
                    cbsubject.DisplayMember = "SubjectName"
                    cbsubject.ValueMember = "SubjectID"
                    cbsubject.DataSource = dt
                End Using
            End Using
        Catch ex As Exception
            Db.ShowError(ex)
        End Try
    End Sub

    Private Sub RefreshGrid()
        For Each row As DataGridViewRow In dgv.Rows
            For Each cell As DataGridViewCell In row.Cells
                cell.Value = ""
            Next
        Next

        Try
            Using con As OleDbConnection = Db.OpenConnection()
                Dim col As String = DbSchema.SubjectNameColumn(con)
                Dim sql As String = "SELECT t.DayNum, t.PeriodNum, s." & col & " AS SubjectName, s.Teacher, s.Room " & _
                                    "FROM Timetable AS t INNER JOIN Subjects AS s ON t.SubjectID = s.SubjectID " & _
                                    "ORDER BY t.DayNum, t.PeriodNum, s." & col
                Using cmd As New OleDbCommand(sql, con)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        Do While reader.Read()
                            Dim d As Integer = Db.ToInt(reader("DayNum"))
                            Dim p As Integer = Db.ToInt(reader("PeriodNum"))
                            If d >= 1 AndAlso d <= DayNames.Length AndAlso p >= 1 AndAlso p <= Periods Then
                                Dim detail As String = ""
                                Dim teacher As String = reader("Teacher").ToString()
                                Dim room As String = reader("Room").ToString()
                                If teacher.Length > 0 Then
                                    detail = teacher
                                End If
                                If room.Length > 0 Then
                                    If detail.Length > 0 Then
                                        detail &= ", "
                                    End If
                                    detail &= "room " & room
                                End If

                                Dim text As String = reader("SubjectName").ToString()
                                If detail.Length > 0 Then
                                    text &= " (" & detail & ")"
                                End If

                                Dim cell As DataGridViewCell = dgv.Rows(p - 1).Cells(d - 1)
                                If cell.Value Is Nothing OrElse cell.Value.ToString().Length = 0 Then
                                    cell.Value = text
                                Else
                                    cell.Value = cell.Value.ToString() & vbNewLine & text
                                End If
                            End If
                        Loop
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Db.ShowError(ex)
        End Try
    End Sub

    ' How many OTHER subjects at this day/period share the same Room or Teacher text?
    Private Function CountClash(ByVal con As OleDbConnection, ByVal field As String, ByVal value As String, ByVal subjectId As Integer, ByVal day As Integer, ByVal period As Integer) As Integer
        If value.Length = 0 Then
            Return 0
        End If
        Dim sql As String = "SELECT COUNT(*) FROM Timetable AS t INNER JOIN Subjects AS s ON t.SubjectID = s.SubjectID " & _
                            "WHERE t.DayNum = ? AND t.PeriodNum = ? AND s." & field & " = ? AND t.SubjectID <> ?"
        Using cmd As New OleDbCommand(sql, con)
            cmd.Parameters.AddWithValue("@d", day)
            cmd.Parameters.AddWithValue("@p", period)
            cmd.Parameters.AddWithValue("@v", value)
            cmd.Parameters.AddWithValue("@s", subjectId)
            Return Convert.ToInt32(cmd.ExecuteScalar())
        End Using
    End Function

    Private Sub btadd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btadd.Click
        Dim row As DataRowView = TryCast(cbsubject.SelectedItem, DataRowView)
        If row Is Nothing Then
            MessageBox.Show("There are no subjects yet. Add some under Subjects first.", "Timetable", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim subjectId As Integer = Convert.ToInt32(row("SubjectID"))
        Dim day As Integer = cbday.SelectedIndex + 1
        Dim period As Integer = cbperiod.SelectedIndex + 1

        Try
            Using con As OleDbConnection = Db.OpenConnection()
                'already there?
                Using cmd As New OleDbCommand("SELECT COUNT(*) FROM Timetable WHERE SubjectID = ? AND DayNum = ? AND PeriodNum = ?", con)
                    cmd.Parameters.AddWithValue("@s", subjectId)
                    cmd.Parameters.AddWithValue("@d", day)
                    cmd.Parameters.AddWithValue("@p", period)
                    If Convert.ToInt32(cmd.ExecuteScalar()) > 0 Then
                        MessageBox.Show("That subject is already in that slot.", "Timetable", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If
                End Using

                'warn about a room or teacher being in two places at once
                Dim room As String = ""
                Dim teacher As String = ""
                Using cmd As New OleDbCommand("SELECT Room, Teacher FROM Subjects WHERE SubjectID = ?", con)
                    cmd.Parameters.AddWithValue("@s", subjectId)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            room = reader("Room").ToString()
                            teacher = reader("Teacher").ToString()
                        End If
                    End Using
                End Using

                Dim warning As String = ""
                If CountClash(con, "Room", room, subjectId, day, period) > 0 Then
                    warning &= "Room " & room & " is already used in that slot." & vbNewLine
                End If
                If CountClash(con, "Teacher", teacher, subjectId, day, period) > 0 Then
                    warning &= teacher & " is already teaching in that slot." & vbNewLine
                End If
                If warning.Length > 0 Then
                    If MessageBox.Show(warning & vbNewLine & "Add it anyway?", "Clash", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> DialogResult.Yes Then
                        Return
                    End If
                End If

                Db.Exec(con, Nothing, "INSERT INTO Timetable (SubjectID, DayNum, PeriodNum) VALUES (?, ?, ?)", subjectId, day, period)
            End Using
            RefreshGrid()
        Catch ex As Exception
            Db.ShowError(ex)
        End Try
    End Sub

    Private Sub btclear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btclear.Click
        If dgv.CurrentCell Is Nothing Then
            MessageBox.Show("Click a slot in the grid first.", "Timetable", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim day As Integer = dgv.CurrentCell.ColumnIndex + 1
        Dim period As Integer = dgv.CurrentCell.RowIndex + 1

        If MessageBox.Show("Remove everything in " & DayNames(day - 1) & ", period " & period & "?", "Clear slot", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Try
            Using con As OleDbConnection = Db.OpenConnection()
                Db.Exec(con, Nothing, "DELETE FROM Timetable WHERE DayNum = ? AND PeriodNum = ?", day, period)
            End Using
            RefreshGrid()
        Catch ex As Exception
            Db.ShowError(ex)
        End Try
    End Sub

End Class
