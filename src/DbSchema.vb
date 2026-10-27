Imports System.Data.OleDb

' Makes sure the extra tables and columns this program needs exist in Database.accdb.
' It only ADDS what is missing and never changes or deletes existing data, so it is safe
' to run every time the program starts.
Module DbSchema

    Private subjectColumn As String = Nothing

    Public Sub EnsureSchema()
        Using con As OleDbConnection = Db.OpenConnection()
            'the basic tables (only created when they are missing, e.g. in a brand-new database)
            If Not TableExists(con, "adminpassword") Then
                Run(con, "CREATE TABLE adminpassword (PasswordHash TEXT(255))")
            End If
            If Not TableExists(con, "studentsdetail") Then
                Run(con, "CREATE TABLE studentsdetail (StudentID COUNTER PRIMARY KEY, FirstName TEXT(30), SecondName TEXT(30), HouseNumber SHORT, PhoneNumber TEXT(13), DOB DATETIME, Schoolbus TEXT(255))")
            End If
            If Not TableExists(con, "teachersdetail") Then
                Run(con, "CREATE TABLE teachersdetail (TeacherID COUNTER PRIMARY KEY, FirstName TEXT(30), SecondName TEXT(30), HouseNumber SHORT, PhoneNumber TEXT(13), DOB DATETIME, PasswordHash TEXT(255))")
            End If
            If Not TableExists(con, "Clubs") Then
                Run(con, "CREATE TABLE Clubs (Clubs COUNTER PRIMARY KEY, Teacher TEXT(255), Location TEXT(255), Days TEXT(255))")
            End If
            If Not TableExists(con, "Subjects") Then
                Run(con, "CREATE TABLE Subjects (SubjectID COUNTER PRIMARY KEY, Teacher TEXT(255), Subject TEXT(255), Room TEXT(255))")
            End If

            'older databases kept password hashes in teachersdetail.Runnerpassword and adminpassword.ID:
            'copy them to the PasswordHash column (nothing is lost)
            MigratePasswords(con, "teachersdetail", "Runnerpassword")
            MigratePasswords(con, "adminpassword", "ID")

            'a pupil can be in many clubs / many subjects (the old tables only allowed one)
            If Not TableExists(con, "StudentClubs") Then
                Run(con, "CREATE TABLE StudentClubs (StudentID LONG NOT NULL, ClubID LONG NOT NULL, CONSTRAINT pk_StudentClubs PRIMARY KEY (StudentID, ClubID))")
            End If
            If Not TableExists(con, "StudentSubjects") Then
                Run(con, "CREATE TABLE StudentSubjects (StudentID LONG NOT NULL, SubjectID LONG NOT NULL, CONSTRAINT pk_StudentSubjects PRIMARY KEY (StudentID, SubjectID))")
            End If
            'when each subject takes place: DayNum 1-5 (Mon-Fri), PeriodNum 1-6
            If Not TableExists(con, "Timetable") Then
                Run(con, "CREATE TABLE Timetable (TimetableID COUNTER PRIMARY KEY, SubjectID LONG NOT NULL, DayNum LONG NOT NULL, PeriodNum LONG NOT NULL)")
                Run(con, "CREATE UNIQUE INDEX ux_Timetable ON Timetable (SubjectID, DayNum, PeriodNum)")
            End If

            AddColumn(con, "Clubs", "ClubName", "TEXT(50)")

            'pupil details from the school's design: gender, post code, 2nd phone, email, year
            AddColumn(con, "studentsdetail", "Gender", "TEXT(1)")
            AddColumn(con, "studentsdetail", "PostCode", "TEXT(10)")
            AddColumn(con, "studentsdetail", "Phone2", "TEXT(13)")
            AddColumn(con, "studentsdetail", "Email", "TEXT(100)")
            AddColumn(con, "studentsdetail", "YearGroup", "TEXT(20)")

            'longer names (the old 10-character surname limit is too short)
            WidenColumn(con, "studentsdetail", "FirstName", 30)
            WidenColumn(con, "studentsdetail", "SecondName", 30)
            WidenColumn(con, "teachersdetail", "FirstName", 30)
            WidenColumn(con, "teachersdetail", "SecondName", 30)

            'school bus details (only pupils who use the bus have a row)
            If Not TableExists(con, "BusList") Then
                Run(con, "CREATE TABLE BusList (StudentID LONG NOT NULL PRIMARY KEY, Address TEXT(100), DistanceKm DOUBLE)")
            End If

            'how many of each uniform item a pupil needs
            If Not TableExists(con, "Uniform") Then
                Dim cols As New System.Text.StringBuilder
                For Each c As String In Db.UniformColumns
                    cols.Append(", ").Append(c).Append(" LONG")
                Next
                Run(con, "CREATE TABLE Uniform (StudentID LONG NOT NULL PRIMARY KEY" & cols.ToString() & ")")
            End If
            'for the login lock-out
            AddColumn(con, "teachersdetail", "FailedAttempts", "LONG")
            AddColumn(con, "teachersdetail", "LockedUntil", "DATETIME")
            AddColumn(con, "adminpassword", "FailedAttempts", "LONG")
            AddColumn(con, "adminpassword", "LockedUntil", "DATETIME")

            TidyLegacy(con)
            subjectColumn = Nothing      'look the subject column name up again after any renaming
        End Using
    End Sub

    ' Removes or renames leftovers from the earlier (running-club) version of the database.
    ' Every step is optional: if Access refuses one, the program carries on and the old
    ' column or table simply stays. Nothing is deleted unless its data has been copied or it is empty.
    Private Sub TidyLegacy(ByVal con As OleDbConnection)
        'teachers: the running-club "weight" column is not used
        If ColumnExists(con, "teachersdetail", "Weight") Then
            TryRun(con, "ALTER TABLE teachersdetail DROP COLUMN Weight")
        End If

        'teachers: the column called "HouseNumber?" becomes "HouseNumber"
        If ColumnExists(con, "teachersdetail", "HouseNumber?") Then
            Dim done As Boolean = ColumnExists(con, "teachersdetail", "HouseNumber")
            If Not done Then
                Try
                    AddColumn(con, "teachersdetail", "HouseNumber", "SHORT")
                    Run(con, "UPDATE teachersdetail SET HouseNumber = [HouseNumber?]")
                    done = True
                Catch ex As Exception
                    'the copy did not work: undo, so the old column keeps being used
                    TryRun(con, "ALTER TABLE teachersdetail DROP COLUMN HouseNumber")
                End Try
            End If
            If done Then
                TryRun(con, "ALTER TABLE teachersdetail DROP COLUMN [HouseNumber?]")
            End If
        End If

        'subjects: the misspelt column "Subeject" becomes "Subject"
        If ColumnExists(con, "Subjects", "Subeject") Then
            Dim copied As Boolean = ColumnExists(con, "Subjects", "Subject")
            If Not copied Then
                Try
                    AddColumn(con, "Subjects", "Subject", "TEXT(255)")
                    Run(con, "UPDATE Subjects SET Subject = Subeject")
                    copied = True
                Catch ex As Exception
                    'the copy did not work: undo, so the old column keeps being used
                    TryRun(con, "ALTER TABLE Subjects DROP COLUMN Subject")
                End Try
            End If
            If copied Then
                TryRun(con, "ALTER TABLE Subjects DROP COLUMN Subeject")
            End If
        End If

        'two old tables that only allowed one club / subject per pupil (replaced by StudentClubs / StudentSubjects)
        DropIfEmpty(con, "Students-clubs")
        DropIfEmpty(con, "Student-subject")
    End Sub

    Private Sub TryRun(ByVal con As OleDbConnection, ByVal sql As String)
        Try
            Run(con, sql)
        Catch ex As Exception
            'not critical
        End Try
    End Sub

    Private Sub DropIfEmpty(ByVal con As OleDbConnection, ByVal tableName As String)
        If Not TableExists(con, tableName) Then
            Return
        End If
        Try
            Using cmd As New OleDbCommand("SELECT COUNT(*) FROM [" & tableName & "]", con)
                If Convert.ToInt32(cmd.ExecuteScalar()) = 0 Then
                    Run(con, "DROP TABLE [" & tableName & "]")
                End If
            End Using
        Catch ex As Exception
            'not critical
        End Try
    End Sub

    ' The subjects table has a typo'd column called "Subeject". This returns whichever
    ' spelling really exists, so the program works before and after it is renamed.
    Public Function SubjectNameColumn(ByVal con As OleDbConnection) As String
        If subjectColumn Is Nothing Then
            If ColumnExists(con, "Subjects", "Subject") Then
                subjectColumn = "Subject"
            Else
                subjectColumn = "Subeject"
            End If
        End If
        Return subjectColumn
    End Function

    Private Sub Run(ByVal con As OleDbConnection, ByVal sql As String)
        Using cmd As New OleDbCommand(sql, con)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Function TableExists(ByVal con As OleDbConnection, ByVal name As String) As Boolean
        Dim dt As DataTable = con.GetSchema("Tables", New String() {Nothing, Nothing, name, "TABLE"})
        Return dt.Rows.Count > 0
    End Function

    Private Function ColumnExists(ByVal con As OleDbConnection, ByVal tableName As String, ByVal columnName As String) As Boolean
        Dim dt As DataTable = con.GetSchema("Columns", New String() {Nothing, Nothing, tableName, columnName})
        Return dt.Rows.Count > 0
    End Function

    ' Copies password hashes from an old column to the PasswordHash column, then empties the old
    ' column so no password is left lying around. Does nothing for a new database.
    Private Sub MigratePasswords(ByVal con As OleDbConnection, ByVal tableName As String, ByVal oldColumn As String)
        AddColumn(con, tableName, "PasswordHash", "TEXT(255)")
        If ColumnExists(con, tableName, oldColumn) Then
            Run(con, "UPDATE " & tableName & " SET PasswordHash = [" & oldColumn & "] WHERE PasswordHash IS NULL AND [" & oldColumn & "] IS NOT NULL")
            Try
                Run(con, "UPDATE " & tableName & " SET [" & oldColumn & "] = NULL WHERE PasswordHash IS NOT NULL")
            Catch ex As Exception
                'not critical: the old column just keeps its values
            End Try
        End If
    End Sub

    ' Makes a text column longer if it is shorter than newSize. Never makes it shorter. Ignores failures.
    Private Sub WidenColumn(ByVal con As OleDbConnection, ByVal tableName As String, ByVal columnName As String, ByVal newSize As Integer)
        Try
            Dim dt As DataTable = con.GetSchema("Columns", New String() {Nothing, Nothing, tableName, columnName})
            If dt.Rows.Count > 0 AndAlso Not IsDBNull(dt.Rows(0)("CHARACTER_MAXIMUM_LENGTH")) Then
                If Convert.ToInt32(dt.Rows(0)("CHARACTER_MAXIMUM_LENGTH")) < newSize Then
                    Run(con, "ALTER TABLE " & tableName & " ALTER COLUMN " & columnName & " TEXT(" & newSize & ")")
                End If
            End If
        Catch ex As Exception
            'not critical: the old length stays
        End Try
    End Sub

    Private Sub AddColumn(ByVal con As OleDbConnection, ByVal tableName As String, ByVal columnName As String, ByVal columnType As String)
        If Not ColumnExists(con, tableName, columnName) Then
            Run(con, "ALTER TABLE " & tableName & " ADD COLUMN " & columnName & " " & columnType)
        End If
    End Sub

End Module
