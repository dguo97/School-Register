Imports System.Data.OleDb
Imports System.Drawing
Imports System.Windows.Forms

' Choose a club or a subject, then tick the pupils who take it and press Save.
Public Class EnrolForm
    Inherits Form

    Private WithEvents rbclub As RadioButton
    Private WithEvents rbsubject As RadioButton
    Private WithEvents cbitem As ComboBox
    Private WithEvents clbpupils As CheckedListBox
    Private WithEvents btsave As Button
    Private WithEvents btclose As Button

    Private Class PupilItem
        Public Id As Integer
        Public Text As String
        Public Overrides Function ToString() As String
            Return Text
        End Function
    End Class

    Public Sub New()
        Me.Text = "Assign pupils"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.ShowInTaskbar = False
        Me.MinimumSize = New Size(380, 360)
        Me.ClientSize = New Size(420, 460)

        rbclub = New RadioButton
        rbclub.Text = "Club"
        rbclub.Location = New Point(12, 12)
        rbclub.Size = New Size(70, 20)

        rbsubject = New RadioButton
        rbsubject.Text = "Subject"
        rbsubject.Location = New Point(90, 12)
        rbsubject.Size = New Size(80, 20)

        cbitem = New ComboBox
        cbitem.DropDownStyle = ComboBoxStyle.DropDownList
        cbitem.Location = New Point(12, 40)
        cbitem.Size = New Size(396, 21)
        cbitem.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right

        Dim lb As New Label
        lb.Text = "Tick the pupils who take it:"
        lb.Location = New Point(12, 72)
        lb.Size = New Size(300, 18)

        clbpupils = New CheckedListBox
        clbpupils.CheckOnClick = True
        clbpupils.Location = New Point(12, 94)
        clbpupils.Size = New Size(396, 310)
        clbpupils.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right

        btsave = New Button
        btsave.Text = "Save"
        btsave.Size = New Size(80, 28)
        btsave.Location = New Point(238, 420)
        btsave.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right

        btclose = New Button
        btclose.Text = "Close"
        btclose.Size = New Size(80, 28)
        btclose.Location = New Point(328, 420)
        btclose.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btclose.DialogResult = System.Windows.Forms.DialogResult.Cancel

        Me.Controls.Add(rbclub)
        Me.Controls.Add(rbsubject)
        Me.Controls.Add(cbitem)
        Me.Controls.Add(lb)
        Me.Controls.Add(clbpupils)
        Me.Controls.Add(btsave)
        Me.Controls.Add(btclose)
        Me.CancelButton = btclose
    End Sub

    Private Sub EnrolForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadPupils()
        rbclub.Checked = True      'this also fills the club list (see rbclub_CheckedChanged)
    End Sub

    Private Sub LoadPupils()
        Try
            Using con As OleDbConnection = Db.OpenConnection()
                Using cmd As New OleDbCommand("SELECT StudentID, FirstName, SecondName FROM studentsdetail ORDER BY SecondName, FirstName", con)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        Do While reader.Read()
                            Dim p As New PupilItem
                            p.Id = Convert.ToInt32(reader("StudentID"))
                            p.Text = Db.FormatStudentId(p.Id) & "   " & reader("FirstName").ToString() & " " & reader("SecondName").ToString()
                            clbpupils.Items.Add(p)
                        Loop
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Db.ShowError(ex)
        End Try
    End Sub

    ' Fires whenever the Club/Subject choice changes (picking one un-picks the other).
    Private Sub rbclub_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbclub.CheckedChanged
        Try
            Using con As OleDbConnection = Db.OpenConnection()
                Dim sql As String
                If rbclub.Checked Then
                    sql = "SELECT Clubs.Clubs AS ItemID, IIf(Clubs.ClubName Is Null, 'Club ' & Clubs.Clubs, Clubs.ClubName) AS ItemName FROM Clubs ORDER BY Clubs.ClubName"
                Else
                    Dim col As String = DbSchema.SubjectNameColumn(con)
                    sql = "SELECT SubjectID AS ItemID, " & col & " AS ItemName FROM Subjects ORDER BY " & col
                End If
                Using adp As New OleDbDataAdapter(sql, con)
                    Dim dt As New DataTable
                    adp.Fill(dt)
                    cbitem.DataSource = Nothing
                    cbitem.DisplayMember = "ItemName"
                    cbitem.ValueMember = "ItemID"
                    cbitem.DataSource = dt
                End Using
            End Using
            LoadMembers()
        Catch ex As Exception
            Db.ShowError(ex)
        End Try
    End Sub

    Private Sub cbitem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbitem.SelectedIndexChanged
        LoadMembers()
    End Sub

    Private Function SelectedItemId() As Integer
        Dim row As DataRowView = TryCast(cbitem.SelectedItem, DataRowView)
        If row Is Nothing Then
            Return 0
        End If
        Return Convert.ToInt32(row("ItemID"))
    End Function

    Private Function LinkTable() As String
        If rbclub.Checked Then
            Return "StudentClubs"
        End If
        Return "StudentSubjects"
    End Function

    Private Function LinkColumn() As String
        If rbclub.Checked Then
            Return "ClubID"
        End If
        Return "SubjectID"
    End Function

    ' Ticks the pupils already assigned to the chosen club/subject.
    Private Sub LoadMembers()
        For i As Integer = 0 To clbpupils.Items.Count - 1
            clbpupils.SetItemChecked(i, False)
        Next
        Dim itemId As Integer = SelectedItemId()
        If itemId = 0 Then
            Return
        End If

        Dim members As New List(Of Integer)
        Try
            Using con As OleDbConnection = Db.OpenConnection()
                Using cmd As New OleDbCommand("SELECT StudentID FROM " & LinkTable() & " WHERE " & LinkColumn() & " = ?", con)
                    cmd.Parameters.AddWithValue("@id", itemId)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        Do While reader.Read()
                            members.Add(Convert.ToInt32(reader(0)))
                        Loop
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Db.ShowError(ex)
            Return
        End Try

        For i As Integer = 0 To clbpupils.Items.Count - 1
            If members.Contains(CType(clbpupils.Items(i), PupilItem).Id) Then
                clbpupils.SetItemChecked(i, True)
            End If
        Next
    End Sub

    Private Sub btsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btsave.Click
        Dim itemId As Integer = SelectedItemId()
        If itemId = 0 Then
            MessageBox.Show("There is nothing selected to assign pupils to. Add a club or subject first.", "Assign pupils", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            Using con As OleDbConnection = Db.OpenConnection()
                Dim tx As OleDbTransaction = con.BeginTransaction()
                Try
                    Db.Exec(con, tx, "DELETE FROM " & LinkTable() & " WHERE " & LinkColumn() & " = ?", itemId)
                    For Each p As Object In clbpupils.CheckedItems
                        Db.Exec(con, tx, "INSERT INTO " & LinkTable() & " (StudentID, " & LinkColumn() & ") VALUES (?, ?)", CType(p, PupilItem).Id, itemId)
                    Next
                    tx.Commit()
                Catch
                    tx.Rollback()
                    Throw
                End Try
            End Using
            MessageBox.Show("Saved. " & clbpupils.CheckedItems.Count & " pupil(s) assigned.", "Assign pupils", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            Db.ShowError(ex)
        End Try
    End Sub

End Class
