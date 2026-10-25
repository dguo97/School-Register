Imports System.Data.OleDb

' Adds a new pupil (studentsdetail) or teacher (teachersdetail), or edits an existing one.
'   IsTeacher = False -> pupil, True -> teacher
'   EditId    = 0     -> add new, otherwise the StudentID / TeacherID number to edit
' Set both BEFORE calling ShowDialog.
Public Class RegisterForm

    Public IsTeacher As Boolean = False
    Public EditId As Integer = 0

    Private ReadOnly ErrorColour As Color = Color.MistyRose

    Private Sub RegisterForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim today As Date = Date.Today

        If EditId > 0 Then
            'wide limits so an existing date of birth is never rejected
            tpDOB.MaxDate = today
            tpDOB.MinDate = today.AddYears(-100)
        ElseIf IsTeacher Then
            tpDOB.MaxDate = today.AddYears(-18)
            tpDOB.MinDate = today.AddYears(-80)
            tpDOB.Value = today.AddYears(-30)
        Else
            'creche to primary (and older pupils)
            tpDOB.MaxDate = today
            tpDOB.MinDate = today.AddYears(-20)
            tpDOB.Value = today.AddYears(-5)
        End If

        cbgender.Items.Add("Male")
        cbgender.Items.Add("Female")
        cbyear.Items.AddRange(Db.YearLevels)
        lbdistance.Text = "Distance to school (" & Db.DistanceUnit & ")"

        If IsTeacher Then
            Me.Text = "Register new teacher"
        Else
            Me.Text = "Register new pupil"
        End If

        If EditId > 0 Then
            LoadExisting()
        End If
        LayoutRows()
    End Sub

    ' Shows only the rows that apply, one under the other, and sizes the form to fit.
    Private Sub LayoutRows()
        Dim everything() As Control = New Control() {fnl, tbfirstname, lbsecondname, tbsecondname, lbgender, cbgender, lbdob, tpDOB, lbyear, cbyear, _
            lbhousenumber, nphousenumber, lbpostcode, tbpostcode, lbphonenumber, tbphonenumber, lbphone2, tbphone2, lbemail, tbemail, _
            lbschoolbus, chkschoolbus, lbaddress, tbaddress, lbdistance, npdistance, lbpassword, tbpassword, lbconfirm, tbconfirm}
        For Each c As Control In everything
            c.Visible = False
        Next

        Dim order As New List(Of Control())
        order.Add(New Control() {fnl, tbfirstname})
        order.Add(New Control() {lbsecondname, tbsecondname})
        If IsTeacher Then
            order.Add(New Control() {lbdob, tpDOB})
            order.Add(New Control() {lbhousenumber, nphousenumber})
            order.Add(New Control() {lbphonenumber, tbphonenumber})
            If EditId = 0 Then
                'a teacher's password is chosen when they are added (later: Reset password)
                order.Add(New Control() {lbpassword, tbpassword})
                order.Add(New Control() {lbconfirm, tbconfirm})
            End If
        Else
            order.Add(New Control() {lbgender, cbgender})
            order.Add(New Control() {lbdob, tpDOB})
            order.Add(New Control() {lbyear, cbyear})
            order.Add(New Control() {lbhousenumber, nphousenumber})
            order.Add(New Control() {lbpostcode, tbpostcode})
            order.Add(New Control() {lbphonenumber, tbphonenumber})
            order.Add(New Control() {lbphone2, tbphone2})
            order.Add(New Control() {lbemail, tbemail})
            order.Add(New Control() {lbschoolbus, chkschoolbus})
            If chkschoolbus.Checked Then
                order.Add(New Control() {lbaddress, tbaddress})
                order.Add(New Control() {lbdistance, npdistance})
            End If
        End If

        Dim y As Integer = 18
        For Each pair As Control() In order
            pair(0).Top = y + 3
            pair(1).Top = y
            pair(0).Visible = True
            pair(1).Visible = True
            y += 31
        Next
        btsave.Top = y + 12
        btcancel.Top = y + 12
        Me.ClientSize = New Size(400, y + 56)
    End Sub

    Private Sub chkschoolbus_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkschoolbus.CheckedChanged
        LayoutRows()      'show / hide the bus address and distance rows
    End Sub

    Private Sub LoadExisting()
        Try
            Using con As OleDbConnection = Db.OpenConnection()
                Dim sql As String
                If IsTeacher Then
                    sql = "SELECT FirstName, SecondName, " & Db.TeacherHouseColumn(con) & " AS HouseNo, PhoneNumber, DOB FROM teachersdetail WHERE TeacherID = ?"
                Else
                    sql = "SELECT FirstName, SecondName, HouseNumber AS HouseNo, PhoneNumber, DOB, Schoolbus, Gender, PostCode, Phone2, Email, YearGroup FROM studentsdetail WHERE StudentID = ?"
                End If
                Using cmd As New OleDbCommand(sql, con)
                    cmd.Parameters.AddWithValue("@id", EditId)
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            tbfirstname.Text = reader("FirstName").ToString()
                            tbsecondname.Text = reader("SecondName").ToString()
                            tbphonenumber.Text = reader("PhoneNumber").ToString()
                            If Not IsDBNull(reader("HouseNo")) Then
                                nphousenumber.Value = Math.Min(999, Math.Max(1, Convert.ToInt32(reader("HouseNo"))))
                            End If
                            If Not IsDBNull(reader("DOB")) Then
                                Dim dob As Date = CDate(reader("DOB"))
                                If dob < tpDOB.MinDate Then
                                    dob = tpDOB.MinDate
                                End If
                                If dob > tpDOB.MaxDate Then
                                    dob = tpDOB.MaxDate
                                End If
                                tpDOB.Value = dob
                            End If

                            If Not IsTeacher Then
                                tbpostcode.Text = reader("PostCode").ToString()
                                tbphone2.Text = reader("Phone2").ToString()
                                tbemail.Text = reader("Email").ToString()

                                Dim g As String = reader("Gender").ToString()
                                If g = "M" Then
                                    cbgender.SelectedIndex = 0
                                ElseIf g = "F" Then
                                    cbgender.SelectedIndex = 1
                                End If

                                Dim yr As String = reader("YearGroup").ToString()
                                If yr.Length > 0 Then
                                    Dim idx As Integer = cbyear.FindStringExact(yr)
                                    If idx < 0 Then
                                        idx = cbyear.Items.Add(yr)      'a year that is no longer in the list
                                    End If
                                    cbyear.SelectedIndex = idx
                                End If

                                chkschoolbus.Checked = (reader("Schoolbus").ToString() = "Yes")
                            End If
                        End If
                    End Using
                End Using

                If Not IsTeacher AndAlso chkschoolbus.Checked Then
                    Using cmd2 As New OleDbCommand("SELECT Address, DistanceKm FROM BusList WHERE StudentID = ?", con)
                        cmd2.Parameters.AddWithValue("@id", EditId)
                        Using r2 As OleDbDataReader = cmd2.ExecuteReader()
                            If r2.Read() Then
                                tbaddress.Text = r2("Address").ToString()
                                If Not IsDBNull(r2("DistanceKm")) Then
                                    npdistance.Value = Math.Min(200D, Math.Max(0D, Convert.ToDecimal(r2("DistanceKm"))))
                                End If
                            End If
                        End Using
                    End Using
                End If
            End Using
            If IsTeacher Then
                Me.Text = "Edit teacher " & Db.FormatTeacherId(EditId)
            Else
                Me.Text = "Edit pupil " & Db.FormatStudentId(EditId)
            End If
        Catch ex As Exception
            Db.ShowError(ex)
            btsave.Enabled = False
        End Try
    End Sub

    ' When the user edits a box, take the "error" colour off it.
    Private Sub ClearErrorColour(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbfirstname.TextChanged, tbsecondname.TextChanged, tbphonenumber.TextChanged, tbphone2.TextChanged, tbpostcode.TextChanged, tbemail.TextChanged, tbaddress.TextChanged, tbpassword.TextChanged, tbconfirm.TextChanged
        CType(sender, Control).BackColor = SystemColors.Window
    End Sub

    ' A name needs at least 2 characters: letters, spaces, hyphens and apostrophes only.
    Private Function ValidName(ByVal s As String) As Boolean
        If s.Length < 2 Then
            Return False
        End If
        For Each ch As Char In s
            If Not (Char.IsLetter(ch) OrElse ch = "-"c OrElse ch = "'"c OrElse ch = " "c) Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Sub btsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btsave.Click
        Dim problems As New List(Of String)
        Dim first As String = tbfirstname.Text.Trim()
        Dim second As String = tbsecondname.Text.Trim()
        Dim phone As String = tbphonenumber.Text.Trim()

        If Not ValidName(first) Then
            problems.Add("First name needs at least 2 characters: letters, spaces, - or ' only.")
            tbfirstname.BackColor = ErrorColour
        End If
        If Not ValidName(second) Then
            problems.Add("Second name needs at least 2 characters: letters, spaces, - or ' only.")
            tbsecondname.BackColor = ErrorColour
        End If
        If Not Db.IsValidPhone(phone) Then
            problems.Add("Phone number needs to be 11 digits and start with 0.")
            tbphonenumber.BackColor = ErrorColour
        End If

        If IsTeacher Then
            If EditId = 0 Then
                If tbpassword.Text.Length < Db.MinPasswordLength Then
                    problems.Add("Password needs at least " & Db.MinPasswordLength & " characters.")
                    tbpassword.BackColor = ErrorColour
                    tbconfirm.BackColor = ErrorColour
                ElseIf tbpassword.Text <> tbconfirm.Text Then
                    problems.Add("The two passwords don't match.")
                    tbconfirm.BackColor = ErrorColour
                End If
            End If
        Else
            If cbgender.SelectedIndex < 0 Then
                problems.Add("Please choose the pupil's gender.")
            End If
            If cbyear.SelectedIndex < 0 Then
                problems.Add("Please choose the pupil's year.")
            End If
            If tbphone2.Text.Trim().Length > 0 AndAlso Not Db.IsValidPhone(tbphone2.Text) Then
                problems.Add("Phone number 2 needs to be 11 digits and start with 0 (or leave it empty).")
                tbphone2.BackColor = ErrorColour
            End If
            If Not Db.IsValidPostcode(tbpostcode.Text) Then
                problems.Add("Post code should be 3 to 10 letters or digits (or leave it empty).")
                tbpostcode.BackColor = ErrorColour
            End If
            If Not Db.IsValidEmail(tbemail.Text) Then
                problems.Add("The email address doesn't look right (or leave it empty).")
                tbemail.BackColor = ErrorColour
            End If
            If chkschoolbus.Checked AndAlso tbaddress.Text.Trim().Length = 0 Then
                problems.Add("Please enter the bus pick-up address.")
                tbaddress.BackColor = ErrorColour
            End If
        End If

        If problems.Count > 0 Then
            MessageBox.Show("Please correct the following:" & vbNewLine & vbNewLine & String.Join(vbNewLine, problems.ToArray()), _
                            "Check the details", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim newId As Integer = 0
        Try
            Using con As OleDbConnection = Db.OpenConnection()
                Dim tableName As String = "studentsdetail"
                Dim idColumn As String = "StudentID"
                Dim houseColumn As String = "HouseNumber"
                If IsTeacher Then
                    tableName = "teachersdetail"
                    idColumn = "TeacherID"
                    houseColumn = Db.TeacherHouseColumn(con)
                End If

                If AlreadyExists(con, tableName, idColumn, first, second) Then
                    If MessageBox.Show("Someone with this name and date of birth is already registered. Save anyway?", "Possible duplicate", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                        Return
                    End If
                End If

                'everything is saved together, or not at all
                Dim tx As OleDbTransaction = con.BeginTransaction()
                Try
                    If IsTeacher Then
                        If EditId > 0 Then
                            UpdateTeacher(con, tx, houseColumn, first, second, phone)
                        Else
                            newId = InsertTeacher(con, tx, houseColumn, first, second, phone)
                        End If
                    Else
                        If EditId > 0 Then
                            UpdatePupil(con, tx, first, second, phone)
                        Else
                            newId = InsertPupil(con, tx, first, second, phone)
                        End If
                    End If
                    tx.Commit()
                Catch
                    tx.Rollback()
                    Throw
                End Try
            End Using
        Catch ex As Exception
            Db.ShowError(ex)
            Return
        End Try

        If EditId > 0 Then
            MessageBox.Show("Changes saved.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf IsTeacher Then
            MessageBox.Show("Teacher saved." & vbNewLine & "Their Teacher ID (used to sign in) is " & Db.FormatTeacherId(newId) & ".", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Pupil saved." & vbNewLine & "Student ID: " & Db.FormatStudentId(newId) & "." & vbNewLine & vbNewLine & _
                            "A uniform record with one of every item was created. Change it under Uniform if needed.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        Me.DialogResult = DialogResult.OK     'closes the form
    End Sub

    Private Function AlreadyExists(ByVal con As OleDbConnection, ByVal tableName As String, ByVal idColumn As String, ByVal first As String, ByVal second As String) As Boolean
        Dim sql As String = "SELECT COUNT(*) FROM " & tableName & " WHERE FirstName = ? AND SecondName = ? AND DOB = ? AND " & idColumn & " <> ?"
        Using cmd As New OleDbCommand(sql, con)
            cmd.Parameters.AddWithValue("@first", first)
            cmd.Parameters.AddWithValue("@second", second)
            cmd.Parameters.Add("@dob", OleDbType.Date).Value = tpDOB.Value.Date
            cmd.Parameters.AddWithValue("@id", EditId)
            Return Convert.ToInt32(cmd.ExecuteScalar()) > 0
        End Using
    End Function

    ' ---------------- Pupils ----------------

    ' The 11 pupil values, in the same order as the INSERT / UPDATE statements below.
    Private Sub AddPupilParameters(ByVal cmd As OleDbCommand, ByVal first As String, ByVal second As String, ByVal phone As String)
        cmd.Parameters.AddWithValue("@first", first)
        cmd.Parameters.AddWithValue("@second", second)
        cmd.Parameters.Add("@house", OleDbType.SmallInt).Value = CShort(nphousenumber.Value)
        cmd.Parameters.AddWithValue("@phone", phone)
        cmd.Parameters.Add("@dob", OleDbType.Date).Value = tpDOB.Value.Date
        cmd.Parameters.AddWithValue("@bus", If(chkschoolbus.Checked, "Yes", "No"))
        cmd.Parameters.AddWithValue("@gender", If(cbgender.SelectedIndex = 0, "M", "F"))
        cmd.Parameters.Add("@post", OleDbType.VarWChar).Value = Db.NullIfEmpty(tbpostcode.Text)
        cmd.Parameters.Add("@phone2", OleDbType.VarWChar).Value = Db.NullIfEmpty(tbphone2.Text)
        cmd.Parameters.Add("@email", OleDbType.VarWChar).Value = Db.NullIfEmpty(tbemail.Text)
        cmd.Parameters.AddWithValue("@year", cbyear.SelectedItem.ToString())
    End Sub

    Private Function InsertPupil(ByVal con As OleDbConnection, ByVal tx As OleDbTransaction, ByVal first As String, ByVal second As String, ByVal phone As String) As Integer
        Dim sql As String = "INSERT INTO studentsdetail (FirstName, SecondName, HouseNumber, PhoneNumber, DOB, Schoolbus, Gender, PostCode, Phone2, Email, YearGroup) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"
        Using cmd As OleDbCommand = Db.NewCommand(con, tx, sql)
            AddPupilParameters(cmd, first, second, phone)
            cmd.ExecuteNonQuery()
        End Using
        Dim id As Integer = Db.LastInsertId(con, tx)

        SaveBusDetails(con, tx, id)

        'a new pupil starts with one of every uniform item (as in the school's design)
        Dim cols As New System.Text.StringBuilder
        Dim ones As New System.Text.StringBuilder
        For Each c As String In Db.UniformColumns
            cols.Append(", ").Append(c)
            ones.Append(", 1")
        Next
        Db.Exec(con, tx, "INSERT INTO Uniform (StudentID" & cols.ToString() & ") VALUES (?" & ones.ToString() & ")", id)
        Return id
    End Function

    Private Sub UpdatePupil(ByVal con As OleDbConnection, ByVal tx As OleDbTransaction, ByVal first As String, ByVal second As String, ByVal phone As String)
        Dim sql As String = "UPDATE studentsdetail SET FirstName = ?, SecondName = ?, HouseNumber = ?, PhoneNumber = ?, DOB = ?, Schoolbus = ?, Gender = ?, PostCode = ?, Phone2 = ?, Email = ?, YearGroup = ? WHERE StudentID = ?"
        Using cmd As OleDbCommand = Db.NewCommand(con, tx, sql)
            AddPupilParameters(cmd, first, second, phone)
            cmd.Parameters.AddWithValue("@id", EditId)
            cmd.ExecuteNonQuery()
        End Using
        SaveBusDetails(con, tx, EditId)
    End Sub

    ' Pupils who use the bus have a row in BusList; others don't.
    Private Sub SaveBusDetails(ByVal con As OleDbConnection, ByVal tx As OleDbTransaction, ByVal studentId As Integer)
        Db.Exec(con, tx, "DELETE FROM BusList WHERE StudentID = ?", studentId)
        If chkschoolbus.Checked Then
            Using cmd As OleDbCommand = Db.NewCommand(con, tx, "INSERT INTO BusList (StudentID, Address, DistanceKm) VALUES (?, ?, ?)")
                cmd.Parameters.AddWithValue("@id", studentId)
                cmd.Parameters.AddWithValue("@addr", tbaddress.Text.Trim())
                cmd.Parameters.Add("@km", OleDbType.Double).Value = CDbl(npdistance.Value)
                cmd.ExecuteNonQuery()
            End Using
        End If
    End Sub

    ' ---------------- Teachers ----------------

    Private Function InsertTeacher(ByVal con As OleDbConnection, ByVal tx As OleDbTransaction, ByVal houseColumn As String, ByVal first As String, ByVal second As String, ByVal phone As String) As Integer
        Dim sql As String = "INSERT INTO teachersdetail (FirstName, SecondName, " & houseColumn & ", PhoneNumber, DOB, PasswordHash) VALUES (?, ?, ?, ?, ?, ?)"
        Using cmd As OleDbCommand = Db.NewCommand(con, tx, sql)
            cmd.Parameters.AddWithValue("@first", first)
            cmd.Parameters.AddWithValue("@second", second)
            cmd.Parameters.Add("@house", OleDbType.SmallInt).Value = CShort(nphousenumber.Value)
            cmd.Parameters.AddWithValue("@phone", phone)
            cmd.Parameters.Add("@dob", OleDbType.Date).Value = tpDOB.Value.Date
            cmd.Parameters.AddWithValue("@pw", Db.HashPassword(tbpassword.Text))
            cmd.ExecuteNonQuery()
        End Using
        Return Db.LastInsertId(con, tx)
    End Function

    Private Sub UpdateTeacher(ByVal con As OleDbConnection, ByVal tx As OleDbTransaction, ByVal houseColumn As String, ByVal first As String, ByVal second As String, ByVal phone As String)
        Dim sql As String = "UPDATE teachersdetail SET FirstName = ?, SecondName = ?, " & houseColumn & " = ?, PhoneNumber = ?, DOB = ? WHERE TeacherID = ?"
        Using cmd As OleDbCommand = Db.NewCommand(con, tx, sql)
            cmd.Parameters.AddWithValue("@first", first)
            cmd.Parameters.AddWithValue("@second", second)
            cmd.Parameters.Add("@house", OleDbType.SmallInt).Value = CShort(nphousenumber.Value)
            cmd.Parameters.AddWithValue("@phone", phone)
            cmd.Parameters.Add("@dob", OleDbType.Date).Value = tpDOB.Value.Date
            cmd.Parameters.AddWithValue("@id", EditId)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

End Class
