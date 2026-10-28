Imports System.Data.OleDb
Imports System.IO
Imports System.Security.Cryptography

' Shared helpers: finding/opening the database and storing passwords safely.
Module Db

    Public Const MinPasswordLength As Integer = 6

    Private dbFile As String = Nothing

    ' Looks for Database.accdb next to the program (and up to 5 folders above it, which
    ' covers running from bin\Debug inside the project folder). If it is not found the
    ' user is asked to locate it. Returns Nothing if they cancel.
    Public Function GetDatabasePath() As String
        If dbFile IsNot Nothing AndAlso File.Exists(dbFile) Then
            Return dbFile
        End If

        Dim folder As DirectoryInfo = New DirectoryInfo(Application.StartupPath)
        Dim levels As Integer = 0
        Do While folder IsNot Nothing AndAlso levels < 6
            Dim candidate As String = Path.Combine(folder.FullName, "Database.accdb")
            If File.Exists(candidate) Then
                dbFile = candidate
                Return dbFile
            End If
            folder = folder.Parent
            levels += 1
        Loop

        'not found: create a new empty one, or let the user choose an existing one
        Dim answer As DialogResult = MessageBox.Show("Database.accdb was not found." & vbNewLine & vbNewLine & _
                      "Yes: create a new, empty database in the program folder" & vbNewLine & _
                      "No: choose an existing Database.accdb file", "School Register", _
                      MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
        If answer = DialogResult.Yes Then
            Dim newFile As String = Path.Combine(Application.StartupPath, "Database.accdb")
            If CreateNewDatabase(newFile) Then
                dbFile = newFile
                Return dbFile
            End If
        ElseIf answer = DialogResult.No Then
            Using dlg As New OpenFileDialog
                dlg.Title = "Locate Database.accdb"
                dlg.Filter = "Access database (*.accdb)|*.accdb"
                If dlg.ShowDialog() = DialogResult.OK Then
                    dbFile = dlg.FileName
                    Return dbFile
                End If
            End Using
        End If
        Return Nothing
    End Function

    ' Creates an empty Access database file (the tables are added by DbSchema.EnsureSchema).
    ' Uses ADOX, which comes with Windows; it needs the Access Database Engine installed.
    Private Function CreateNewDatabase(ByVal fullPath As String) As Boolean
        Try
            Dim cat As Object = CreateObject("ADOX.Catalog")
            cat.Create("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & fullPath)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(cat)
            Return File.Exists(fullPath)
        Catch ex As Exception
            MessageBox.Show("A new database could not be created:" & vbNewLine & ex.Message & vbNewLine & vbNewLine & ProviderHelp(), _
                            "School Register", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' Opens a connection. The caller must Close/Dispose it (use "Using").
    Public Function OpenConnection() As OleDbConnection
        Dim path As String = GetDatabasePath()
        If path Is Nothing Then
            Throw New FileNotFoundException("Database.accdb was not found.")
        End If
        Dim con As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & path)
        con.Open()
        Return con
    End Function

    Public Sub ShowError(ByVal ex As Exception)
        MessageBox.Show("The database could not be used:" & vbNewLine & ex.Message & vbNewLine & vbNewLine & ProviderHelp(), _
                        "Database error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    ' "32-bit (x86)" or "64-bit (x64)": which kind of program this is.
    Public Function ProgramBits() As String
        If Environment.Is64BitProcess Then
            Return "64-bit (x64)"
        End If
        Return "32-bit (x86)"
    End Function

    ' True if this program can see the Microsoft Access Database Engine (ACE).
    ' A 64-bit program cannot see a 32-bit engine, and the other way round.
    Public Function AceInstalled() As Boolean
        Try
            Dim reader As OleDbDataReader = OleDbEnumerator.GetRootEnumerator()
            Try
                Do While reader.Read()
                    Dim name As String = Convert.ToString(reader("SOURCES_NAME"))
                    If name.StartsWith("Microsoft.ACE.OLEDB") Then
                        Return True
                    End If
                Loop
            Finally
                reader.Close()
            End Try
        Catch ex As Exception
            'could not check: treat as not found
        End Try
        Return False
    End Function

    ' Plain-language help, based on what this program can really see on this computer.
    Public Function ProviderHelp() As String
        Dim mine As String = "32-bit"
        Dim other As String = "x64"
        If Environment.Is64BitProcess Then
            mine = "64-bit"
            other = "x86"
        End If

        Dim sb As New System.Text.StringBuilder
        sb.Append("This program is running as ").Append(ProgramBits()).Append(".").Append(vbNewLine)
        If AceInstalled() Then
            sb.Append("The Microsoft Access Database Engine was found, so the cause is in the message above ")
            sb.Append("(for example the database file is open in Access, or is in a protected folder).")
        Else
            sb.Append("It cannot see the Microsoft Access Database Engine.").Append(vbNewLine).Append(vbNewLine)
            sb.Append("How to fix it:").Append(vbNewLine)
            sb.Append("1. Install the Microsoft Access Database Engine from Microsoft (search for ""Microsoft Access Database Engine 2016 Redistributable""). Choose the ").Append(mine).Append(" version.").Append(vbNewLine)
            sb.Append("2. Or, if you already have Microsoft Office, download the ").Append(other).Append(" version of School Register instead (it must match your Office: 32-bit Office needs x86, 64-bit Office needs x64).").Append(vbNewLine)
            sb.Append("3. Or, if you have Microsoft Access, create a blank database called Database.accdb in the program folder, then start this program again.")
        End If
        Return sb.ToString()
    End Function

    ' ID of the row just inserted (call on the same open connection, straight after the INSERT).
    Public Function LastInsertId(ByVal con As OleDbConnection, Optional ByVal tx As OleDbTransaction = Nothing) As Integer
        Using cmd As OleDbCommand = NewCommand(con, tx, "SELECT @@IDENTITY")
            Return Convert.ToInt32(cmd.ExecuteScalar())
        End Using
    End Function

    ' A command that takes part in the transaction tx (tx may be Nothing).
    Public Function NewCommand(ByVal con As OleDbConnection, ByVal tx As OleDbTransaction, ByVal sql As String) As OleDbCommand
        Dim c As New OleDbCommand(sql, con)
        If tx IsNot Nothing Then
            c.Transaction = tx
        End If
        Return c
    End Function

    ' Empty text is stored as NULL.
    Public Function NullIfEmpty(ByVal s As String) As Object
        If s Is Nothing OrElse s.Trim().Length = 0 Then
            Return DBNull.Value
        End If
        Return s.Trim()
    End Function

    ' The teachers table has a column called "HouseNumber?" (with a question mark).
    ' This finds whatever it is really called, so the code works before and after renaming it.
    Public Function TeacherHouseColumn(ByVal con As OleDbConnection) As String
        Dim dt As DataTable = con.GetSchema("Columns", New String() {Nothing, Nothing, "teachersdetail", Nothing})
        Dim oddName As String = ""
        For Each row As DataRow In dt.Rows
            Dim name As String = row("COLUMN_NAME").ToString()
            If name = "HouseNumber" Then
                Return "HouseNumber"
            End If
            If name.StartsWith("HouseNumber") Then
                oddName = name
            End If
        Next
        If oddName.Length > 0 Then
            Return "[" & oddName & "]"
        End If
        Return "HouseNumber"
    End Function

    ' ---------- School settings: change these to suit a different school ----------

    ' The choices in the "Year" drop-down (pupil's class / year group).
    Public ReadOnly YearLevels() As String = New String() {"Creche", "Preschool 1", "Preschool 2", "Nursery 1", "Nursery 2", "Grade 1", "Grade 2", "Grade 3", "Grade 4", "Grade 5", "Grade 6"}

    ' The uniform items (database column names and the words shown on screen). Keep both lists in step.
    Public ReadOnly UniformColumns() As String = New String() {"GirlsDressTop", "BoysTrouserTop", "SkyBlueTop", "Dress", "Trouser", "Skirt", "PEShortTop", "PEShort", "PETop", "Tie", "Badge", "TankTop", "Sweater"}
    Public ReadOnly UniformLabels() As String = New String() {"Girls Dress and Top", "Boys Trouser and Top", "Sky blue top", "Dress", "Trouser", "Skirt", "PE Short and Top", "PE Short", "PE Top", "Tie", "Badge", "Tank top", "Sweater or Jumper"}
    Public Const MaxUniform As Integer = 5          'most of one item a pupil can need

    Public Const DistanceUnit As String = "km"      'distance from home to school (bus list)

    ' Telephone numbers: 11 digits starting with 0 (Nigeria, UK). Change the pattern for another country.
    Public Function IsValidPhone(ByVal s As String) As Boolean
        Return s.Trim() Like "0##########"
    End Function

    ' Post code is optional. Any 3-10 letters, digits, spaces or hyphens is accepted, so it
    ' works for Nigeria (123456), the UK (SW1A 1AA), the US (12345) and most other countries.
    Public Function IsValidPostcode(ByVal s As String) As Boolean
        Dim t As String = s.Trim()
        If t.Length = 0 Then
            Return True
        End If
        If t.Length < 3 OrElse t.Length > 10 Then
            Return False
        End If
        For Each ch As Char In t
            If Not (Char.IsLetterOrDigit(ch) OrElse ch = " "c OrElse ch = "-"c) Then
                Return False
            End If
        Next
        Return True
    End Function

    ' Email is optional. Needs one @ with text before it and a dot after it.
    Public Function IsValidEmail(ByVal s As String) As Boolean
        Dim t As String = s.Trim()
        If t.Length = 0 Then
            Return True
        End If
        If t.Contains(" ") Then
            Return False
        End If
        Dim at As Integer = t.IndexOf("@"c)
        If at < 1 OrElse t.IndexOf("@"c, at + 1) >= 0 Then
            Return False
        End If
        Dim dot As Integer = t.LastIndexOf("."c)
        Return dot > at + 1 AndAlso dot < t.Length - 1
    End Function

    ' ---------- Small helpers ----------

    ' Runs an INSERT/UPDATE/DELETE with positional (?) parameters. tx may be Nothing.
    Public Sub Exec(ByVal con As OleDbConnection, ByVal tx As OleDbTransaction, ByVal sql As String, ByVal ParamArray values() As Object)
        Using cmd As New OleDbCommand(sql, con)
            If tx IsNot Nothing Then
                cmd.Transaction = tx
            End If
            For Each v As Object In values
                cmd.Parameters.AddWithValue("@p", v)
            Next
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Public Function ToInt(ByVal o As Object) As Integer
        If o Is Nothing OrElse IsDBNull(o) Then
            Return 0
        End If
        Return Convert.ToInt32(o)
    End Function

    ' Builds a DataView.RowFilter that matches the search text in ANY column (except RawID).
    Public Function BuildRowFilter(ByVal dt As DataTable, ByVal text As String) As String
        Dim t As String = text.Trim()
        If t.Length = 0 Then
            Return ""
        End If
        Dim safe As New System.Text.StringBuilder
        For Each ch As Char In t
            If ch = "'"c Then
                safe.Append("''")
            ElseIf ch = "%"c OrElse ch = "*"c OrElse ch = "["c OrElse ch = "]"c Then
                safe.Append("[").Append(ch).Append("]")
            Else
                safe.Append(ch)
            End If
        Next
        Dim parts As New List(Of String)
        For Each col As DataColumn In dt.Columns
            If col.ColumnName <> "RawID" Then
                parts.Add("Convert([" & col.ColumnName & "], 'System.String') LIKE '%" & safe.ToString() & "%'")
            End If
        Next
        Return String.Join(" OR ", parts.ToArray())
    End Function

    ' ---------- Login lock-out ----------
    Public Const MaxFailedLogins As Integer = 5
    Public Const LockMinutes As Integer = 5

    ' Minutes left on a lock (0 = not locked). lockedUntil is the raw database value.
    Public Function MinutesLocked(ByVal lockedUntil As Object) As Integer
        If lockedUntil Is Nothing OrElse IsDBNull(lockedUntil) Then
            Return 0
        End If
        Dim t As Date = CDate(lockedUntil)
        If t <= Date.Now Then
            Return 0
        End If
        Return CInt(Math.Ceiling((t - Date.Now).TotalMinutes))
    End Function

    ' idColumn = "" means the table has a single row (adminpassword).
    Public Sub SetLockState(ByVal con As OleDbConnection, ByVal tableName As String, ByVal idColumn As String, ByVal id As Integer, ByVal attempts As Integer, ByVal lockedUntil As Object)
        Dim sql As String = "UPDATE " & tableName & " SET FailedAttempts = ?, LockedUntil = ?"
        If idColumn.Length > 0 Then
            sql &= " WHERE " & idColumn & " = ?"
        End If
        Using cmd As New OleDbCommand(sql, con)
            cmd.Parameters.AddWithValue("@a", attempts)
            Dim p As OleDbParameter = cmd.Parameters.Add("@l", OleDbType.Date)
            p.Value = lockedUntil
            If idColumn.Length > 0 Then
                cmd.Parameters.AddWithValue("@id", id)
            End If
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    ' Call after a wrong password. Returns True if the account is now locked.
    Public Function RecordFailure(ByVal con As OleDbConnection, ByVal tableName As String, ByVal idColumn As String, ByVal id As Integer, ByVal currentAttempts As Integer) As Boolean
        Dim attempts As Integer = currentAttempts + 1
        If attempts >= MaxFailedLogins Then
            SetLockState(con, tableName, idColumn, id, 0, Date.Now.AddMinutes(LockMinutes))
            Return True
        End If
        SetLockState(con, tableName, idColumn, id, attempts, DBNull.Value)
        Return False
    End Function

    Public Sub ClearFailures(ByVal con As OleDbConnection, ByVal tableName As String, ByVal idColumn As String, ByVal id As Integer)
        SetLockState(con, tableName, idColumn, id, 0, DBNull.Value)
    End Sub

    ' ---------- Display IDs ----------
    ' The database stores plain numbers (AutoNumber). The letter is only added for display.

    Public Function FormatTeacherId(ByVal id As Integer) As String
        Return "T" & id
    End Function

    Public Function FormatStudentId(ByVal id As Integer) As String
        Return "S" & id
    End Function

    ' Accepts "T7", "t7" or just "7". Anything else (for example a student ID like S7) is rejected.
    Public Function TryParseTeacherId(ByVal text As String, ByRef id As Integer) As Boolean
        Dim s As String = text.Trim()
        If s.Length > 0 AndAlso (s(0) = "T"c OrElse s(0) = "t"c) Then
            s = s.Substring(1)
        End If
        Return Integer.TryParse(s, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, id) AndAlso id > 0
    End Function

    ' ---------- Passwords ----------
    ' Stored as  salt:hash  (both Base64), using PBKDF2. The plain password is never stored.

    Public Function HashPassword(ByVal password As String) As String
        Dim salt(15) As Byte
        Dim rng As New RNGCryptoServiceProvider
        rng.GetBytes(salt)
        Return Convert.ToBase64String(salt) & ":" & Convert.ToBase64String(Pbkdf2(password, salt))
    End Function

    Private Function Pbkdf2(ByVal password As String, ByVal salt() As Byte) As Byte()
        Dim kdf As New Rfc2898DeriveBytes(password, salt, 10000)
        Return kdf.GetBytes(20)
    End Function

    ' True if the typed password matches the stored value.
    ' needsUpgrade is set when the stored value was an old plain-text password, which the
    ' caller should then re-save using HashPassword.
    Public Function VerifyPassword(ByVal typed As String, ByVal stored As String, ByRef needsUpgrade As Boolean) As Boolean
        needsUpgrade = False
        If typed Is Nothing OrElse typed.Length = 0 OrElse stored Is Nothing OrElse stored.Length = 0 Then
            Return False
        End If

        Dim parts() As String = stored.Split(":"c)
        If parts.Length = 2 Then
            Try
                Dim salt() As Byte = Convert.FromBase64String(parts(0))
                Dim expected() As Byte = Convert.FromBase64String(parts(1))
                If salt.Length = 16 AndAlso expected.Length = 20 Then
                    Dim actual() As Byte = Pbkdf2(typed, salt)
                    Dim diff As Integer = 0
                    For i As Integer = 0 To 19
                        diff = diff Or (CInt(actual(i)) Xor CInt(expected(i)))
                    Next
                    Return diff = 0
                End If
            Catch ex As FormatException
                ' Not a hash after all - fall through and treat it as an old plain-text password.
            End Try
        End If

        If typed = stored Then
            needsUpgrade = True
            Return True
        End If
        Return False
    End Function

End Module
