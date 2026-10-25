Imports System.Data.OleDb
Imports System.Drawing
Imports System.Windows.Forms

' Edit how many of each uniform item one pupil needs. Set StudentId before ShowDialog.
Public Class UniformForm
    Inherits Form

    Public StudentId As Integer = 0

    Private boxes As New List(Of NumericUpDown)
    Private lbname As Label
    Private WithEvents btsave As Button
    Private WithEvents btall As Button
    Private WithEvents btnone As Button
    Private WithEvents btclose As Button

    Public Sub New()
        Me.Text = "Uniform"
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.StartPosition = FormStartPosition.CenterParent
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.ShowInTaskbar = False

        lbname = New Label
        lbname.Location = New Point(12, 14)
        lbname.Size = New Size(470, 20)
        Me.Controls.Add(lbname)

        Dim n As Integer = Db.UniformLabels.Length
        Dim perColumn As Integer = (n + 1) \ 2
        For i As Integer = 0 To n - 1
            Dim col As Integer = i \ perColumn
            Dim row As Integer = i Mod perColumn

            Dim lb As New Label
            lb.Text = Db.UniformLabels(i)
            lb.Location = New Point(12 + col * 250, 49 + row * 30)
            lb.Size = New Size(150, 18)

            Dim nud As New NumericUpDown
            nud.Minimum = 0
            nud.Maximum = Db.MaxUniform
            nud.Location = New Point(168 + col * 250, 46 + row * 30)
            nud.Size = New Size(55, 20)
            boxes.Add(nud)

            Me.Controls.Add(lb)
            Me.Controls.Add(nud)
        Next

        Dim y As Integer = 46 + perColumn * 30 + 12
        btall = MakeButton("All = 1 (new pupil)", 12, y, 150)
        btnone = MakeButton("Clear all", 170, y, 90)
        btsave = MakeButton("Save", 320, y, 75)
        btclose = MakeButton("Cancel", 400, y, 75)
        btclose.DialogResult = System.Windows.Forms.DialogResult.Cancel

        Me.CancelButton = btclose
        Me.ClientSize = New Size(490, y + 46)
    End Sub

    Private Function MakeButton(ByVal text As String, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer) As Button
        Dim b As New Button
        b.Text = text
        b.Location = New Point(x, y)
        b.Size = New Size(width, 26)
        Me.Controls.Add(b)
        Return b
    End Function

    Private Sub UniformForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Using con As OleDbConnection = Db.OpenConnection()
                Using cmd As New OleDbCommand("SELECT FirstName, SecondName FROM studentsdetail WHERE StudentID = ?", con)
                    cmd.Parameters.AddWithValue("@id", StudentId)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            lbname.Text = "Uniform for " & Db.FormatStudentId(StudentId) & "   " & reader("FirstName").ToString() & " " & reader("SecondName").ToString()
                        End If
                    End Using
                End Using

                Using cmd As New OleDbCommand("SELECT * FROM Uniform WHERE StudentID = ?", con)
                    cmd.Parameters.AddWithValue("@id", StudentId)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            For i As Integer = 0 To boxes.Count - 1
                                boxes(i).Value = Math.Min(Db.MaxUniform, Math.Max(0, Db.ToInt(reader(Db.UniformColumns(i)))))
                            Next
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Db.ShowError(ex)
            btsave.Enabled = False
        End Try
    End Sub

    Private Sub btall_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btall.Click
        For Each b As NumericUpDown In boxes
            b.Value = 1
        Next
    End Sub

    Private Sub btnone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnone.Click
        For Each b As NumericUpDown In boxes
            b.Value = 0
        Next
    End Sub

    Private Sub btsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btsave.Click
        Dim cols As New System.Text.StringBuilder
        Dim marks As New System.Text.StringBuilder
        Dim values(boxes.Count) As Object
        values(0) = StudentId
        For i As Integer = 0 To boxes.Count - 1
            cols.Append(", ").Append(Db.UniformColumns(i))
            marks.Append(", ?")
            values(i + 1) = CInt(boxes(i).Value)
        Next

        Try
            Using con As OleDbConnection = Db.OpenConnection()
                Dim tx As OleDbTransaction = con.BeginTransaction()
                Try
                    Db.Exec(con, tx, "DELETE FROM Uniform WHERE StudentID = ?", StudentId)
                    Db.Exec(con, tx, "INSERT INTO Uniform (StudentID" & cols.ToString() & ") VALUES (?" & marks.ToString() & ")", values)
                    tx.Commit()
                Catch
                    tx.Rollback()
                    Throw
                End Try
            End Using
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Catch ex As Exception
            Db.ShowError(ex)
        End Try
    End Sub

End Class
