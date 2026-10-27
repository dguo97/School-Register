<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RegisterForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.fnl = New System.Windows.Forms.Label
        Me.lbsecondname = New System.Windows.Forms.Label
        Me.lbgender = New System.Windows.Forms.Label
        Me.lbdob = New System.Windows.Forms.Label
        Me.lbyear = New System.Windows.Forms.Label
        Me.lbhousenumber = New System.Windows.Forms.Label
        Me.lbpostcode = New System.Windows.Forms.Label
        Me.lbphonenumber = New System.Windows.Forms.Label
        Me.lbphone2 = New System.Windows.Forms.Label
        Me.lbemail = New System.Windows.Forms.Label
        Me.lbschoolbus = New System.Windows.Forms.Label
        Me.lbaddress = New System.Windows.Forms.Label
        Me.lbdistance = New System.Windows.Forms.Label
        Me.lbpassword = New System.Windows.Forms.Label
        Me.lbconfirm = New System.Windows.Forms.Label
        Me.tbfirstname = New System.Windows.Forms.TextBox
        Me.tbsecondname = New System.Windows.Forms.TextBox
        Me.cbgender = New System.Windows.Forms.ComboBox
        Me.tpDOB = New System.Windows.Forms.DateTimePicker
        Me.cbyear = New System.Windows.Forms.ComboBox
        Me.nphousenumber = New System.Windows.Forms.NumericUpDown
        Me.tbpostcode = New System.Windows.Forms.TextBox
        Me.tbphonenumber = New System.Windows.Forms.TextBox
        Me.tbphone2 = New System.Windows.Forms.TextBox
        Me.tbemail = New System.Windows.Forms.TextBox
        Me.chkschoolbus = New System.Windows.Forms.CheckBox
        Me.tbaddress = New System.Windows.Forms.TextBox
        Me.npdistance = New System.Windows.Forms.NumericUpDown
        Me.tbpassword = New System.Windows.Forms.TextBox
        Me.tbconfirm = New System.Windows.Forms.TextBox
        Me.btsave = New System.Windows.Forms.Button
        Me.btcancel = New System.Windows.Forms.Button
        CType(Me.nphousenumber, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.npdistance, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'fnl
        '
        Me.fnl.AutoSize = True
        Me.fnl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fnl.Location = New System.Drawing.Point(20, 0)
        Me.fnl.Name = "fnl"
        Me.fnl.Size = New System.Drawing.Size(100, 15)
        Me.fnl.TabIndex = 17
        Me.fnl.Text = "First name"
        '
        'lbsecondname
        '
        Me.lbsecondname.AutoSize = True
        Me.lbsecondname.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbsecondname.Location = New System.Drawing.Point(20, 0)
        Me.lbsecondname.Name = "lbsecondname"
        Me.lbsecondname.Size = New System.Drawing.Size(100, 15)
        Me.lbsecondname.TabIndex = 18
        Me.lbsecondname.Text = "Second name"
        '
        'lbgender
        '
        Me.lbgender.AutoSize = True
        Me.lbgender.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbgender.Location = New System.Drawing.Point(20, 0)
        Me.lbgender.Name = "lbgender"
        Me.lbgender.Size = New System.Drawing.Size(100, 15)
        Me.lbgender.TabIndex = 19
        Me.lbgender.Text = "Gender"
        '
        'lbdob
        '
        Me.lbdob.AutoSize = True
        Me.lbdob.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbdob.Location = New System.Drawing.Point(20, 0)
        Me.lbdob.Name = "lbdob"
        Me.lbdob.Size = New System.Drawing.Size(100, 15)
        Me.lbdob.TabIndex = 20
        Me.lbdob.Text = "Date of birth"
        '
        'lbyear
        '
        Me.lbyear.AutoSize = True
        Me.lbyear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbyear.Location = New System.Drawing.Point(20, 0)
        Me.lbyear.Name = "lbyear"
        Me.lbyear.Size = New System.Drawing.Size(100, 15)
        Me.lbyear.TabIndex = 21
        Me.lbyear.Text = "Year"
        '
        'lbhousenumber
        '
        Me.lbhousenumber.AutoSize = True
        Me.lbhousenumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbhousenumber.Location = New System.Drawing.Point(20, 0)
        Me.lbhousenumber.Name = "lbhousenumber"
        Me.lbhousenumber.Size = New System.Drawing.Size(100, 15)
        Me.lbhousenumber.TabIndex = 22
        Me.lbhousenumber.Text = "House number"
        '
        'lbpostcode
        '
        Me.lbpostcode.AutoSize = True
        Me.lbpostcode.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbpostcode.Location = New System.Drawing.Point(20, 0)
        Me.lbpostcode.Name = "lbpostcode"
        Me.lbpostcode.Size = New System.Drawing.Size(100, 15)
        Me.lbpostcode.TabIndex = 23
        Me.lbpostcode.Text = "Post code (optional)"
        '
        'lbphonenumber
        '
        Me.lbphonenumber.AutoSize = True
        Me.lbphonenumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbphonenumber.Location = New System.Drawing.Point(20, 0)
        Me.lbphonenumber.Name = "lbphonenumber"
        Me.lbphonenumber.Size = New System.Drawing.Size(100, 15)
        Me.lbphonenumber.TabIndex = 24
        Me.lbphonenumber.Text = "Phone number"
        '
        'lbphone2
        '
        Me.lbphone2.AutoSize = True
        Me.lbphone2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbphone2.Location = New System.Drawing.Point(20, 0)
        Me.lbphone2.Name = "lbphone2"
        Me.lbphone2.Size = New System.Drawing.Size(100, 15)
        Me.lbphone2.TabIndex = 25
        Me.lbphone2.Text = "Phone number 2 (optional)"
        '
        'lbemail
        '
        Me.lbemail.AutoSize = True
        Me.lbemail.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbemail.Location = New System.Drawing.Point(20, 0)
        Me.lbemail.Name = "lbemail"
        Me.lbemail.Size = New System.Drawing.Size(100, 15)
        Me.lbemail.TabIndex = 26
        Me.lbemail.Text = "Email (optional)"
        '
        'lbschoolbus
        '
        Me.lbschoolbus.AutoSize = True
        Me.lbschoolbus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbschoolbus.Location = New System.Drawing.Point(20, 0)
        Me.lbschoolbus.Name = "lbschoolbus"
        Me.lbschoolbus.Size = New System.Drawing.Size(100, 15)
        Me.lbschoolbus.TabIndex = 27
        Me.lbschoolbus.Text = "School bus"
        '
        'lbaddress
        '
        Me.lbaddress.AutoSize = True
        Me.lbaddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbaddress.Location = New System.Drawing.Point(20, 0)
        Me.lbaddress.Name = "lbaddress"
        Me.lbaddress.Size = New System.Drawing.Size(100, 15)
        Me.lbaddress.TabIndex = 28
        Me.lbaddress.Text = "Bus pick-up address"
        '
        'lbdistance
        '
        Me.lbdistance.AutoSize = True
        Me.lbdistance.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbdistance.Location = New System.Drawing.Point(20, 0)
        Me.lbdistance.Name = "lbdistance"
        Me.lbdistance.Size = New System.Drawing.Size(100, 15)
        Me.lbdistance.TabIndex = 29
        Me.lbdistance.Text = "Distance to school"
        '
        'lbpassword
        '
        Me.lbpassword.AutoSize = True
        Me.lbpassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbpassword.Location = New System.Drawing.Point(20, 0)
        Me.lbpassword.Name = "lbpassword"
        Me.lbpassword.Size = New System.Drawing.Size(100, 15)
        Me.lbpassword.TabIndex = 30
        Me.lbpassword.Text = "Password"
        '
        'lbconfirm
        '
        Me.lbconfirm.AutoSize = True
        Me.lbconfirm.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbconfirm.Location = New System.Drawing.Point(20, 0)
        Me.lbconfirm.Name = "lbconfirm"
        Me.lbconfirm.Size = New System.Drawing.Size(100, 15)
        Me.lbconfirm.TabIndex = 31
        Me.lbconfirm.Text = "Confirm password"
        '
        'tbfirstname
        '
        Me.tbfirstname.Location = New System.Drawing.Point(200, 0)
        Me.tbfirstname.MaxLength = 30
        Me.tbfirstname.Name = "tbfirstname"
        Me.tbfirstname.Size = New System.Drawing.Size(170, 20)
        Me.tbfirstname.TabIndex = 0
        '
        'tbsecondname
        '
        Me.tbsecondname.Location = New System.Drawing.Point(200, 0)
        Me.tbsecondname.MaxLength = 30
        Me.tbsecondname.Name = "tbsecondname"
        Me.tbsecondname.Size = New System.Drawing.Size(170, 20)
        Me.tbsecondname.TabIndex = 1
        '
        'cbgender
        '
        Me.cbgender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbgender.FormattingEnabled = True
        Me.cbgender.Location = New System.Drawing.Point(200, 0)
        Me.cbgender.Name = "cbgender"
        Me.cbgender.Size = New System.Drawing.Size(120, 20)
        Me.cbgender.TabIndex = 2
        '
        'tpDOB
        '
        Me.tpDOB.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.tpDOB.Location = New System.Drawing.Point(200, 0)
        Me.tpDOB.Name = "tpDOB"
        Me.tpDOB.Size = New System.Drawing.Size(110, 20)
        Me.tpDOB.TabIndex = 3
        '
        'cbyear
        '
        Me.cbyear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbyear.FormattingEnabled = True
        Me.cbyear.Location = New System.Drawing.Point(200, 0)
        Me.cbyear.Name = "cbyear"
        Me.cbyear.Size = New System.Drawing.Size(120, 20)
        Me.cbyear.TabIndex = 4
        '
        'nphousenumber
        '
        Me.nphousenumber.Location = New System.Drawing.Point(200, 0)
        Me.nphousenumber.Maximum = New Decimal(New Integer() {999, 0, 0, 0})
        Me.nphousenumber.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nphousenumber.Name = "nphousenumber"
        Me.nphousenumber.Size = New System.Drawing.Size(70, 20)
        Me.nphousenumber.TabIndex = 5
        Me.nphousenumber.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'tbpostcode
        '
        Me.tbpostcode.Location = New System.Drawing.Point(200, 0)
        Me.tbpostcode.MaxLength = 10
        Me.tbpostcode.Name = "tbpostcode"
        Me.tbpostcode.Size = New System.Drawing.Size(110, 20)
        Me.tbpostcode.TabIndex = 6
        '
        'tbphonenumber
        '
        Me.tbphonenumber.Location = New System.Drawing.Point(200, 0)
        Me.tbphonenumber.MaxLength = 13
        Me.tbphonenumber.Name = "tbphonenumber"
        Me.tbphonenumber.Size = New System.Drawing.Size(130, 20)
        Me.tbphonenumber.TabIndex = 7
        '
        'tbphone2
        '
        Me.tbphone2.Location = New System.Drawing.Point(200, 0)
        Me.tbphone2.MaxLength = 13
        Me.tbphone2.Name = "tbphone2"
        Me.tbphone2.Size = New System.Drawing.Size(130, 20)
        Me.tbphone2.TabIndex = 8
        '
        'tbemail
        '
        Me.tbemail.Location = New System.Drawing.Point(200, 0)
        Me.tbemail.MaxLength = 100
        Me.tbemail.Name = "tbemail"
        Me.tbemail.Size = New System.Drawing.Size(170, 20)
        Me.tbemail.TabIndex = 9
        '
        'chkschoolbus
        '
        Me.chkschoolbus.AutoSize = True
        Me.chkschoolbus.Location = New System.Drawing.Point(200, 0)
        Me.chkschoolbus.Name = "chkschoolbus"
        Me.chkschoolbus.Size = New System.Drawing.Size(150, 20)
        Me.chkschoolbus.TabIndex = 10
        Me.chkschoolbus.Text = "Uses the school bus"
        Me.chkschoolbus.UseVisualStyleBackColor = True
        '
        'tbaddress
        '
        Me.tbaddress.Location = New System.Drawing.Point(200, 0)
        Me.tbaddress.MaxLength = 100
        Me.tbaddress.Name = "tbaddress"
        Me.tbaddress.Size = New System.Drawing.Size(170, 20)
        Me.tbaddress.TabIndex = 11
        '
        'npdistance
        '
        Me.npdistance.Location = New System.Drawing.Point(200, 0)
        Me.npdistance.DecimalPlaces = 1
        Me.npdistance.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
        Me.npdistance.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
        Me.npdistance.Name = "npdistance"
        Me.npdistance.Size = New System.Drawing.Size(70, 20)
        Me.npdistance.TabIndex = 12
        Me.npdistance.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'tbpassword
        '
        Me.tbpassword.Location = New System.Drawing.Point(200, 0)
        Me.tbpassword.Name = "tbpassword"
        Me.tbpassword.Size = New System.Drawing.Size(170, 20)
        Me.tbpassword.TabIndex = 13
        Me.tbpassword.UseSystemPasswordChar = True
        '
        'tbconfirm
        '
        Me.tbconfirm.Location = New System.Drawing.Point(200, 0)
        Me.tbconfirm.Name = "tbconfirm"
        Me.tbconfirm.Size = New System.Drawing.Size(170, 20)
        Me.tbconfirm.TabIndex = 14
        Me.tbconfirm.UseSystemPasswordChar = True
        '
        'btsave
        '
        Me.btsave.Location = New System.Drawing.Point(150, 480)
        Me.btsave.Name = "btsave"
        Me.btsave.Size = New System.Drawing.Size(90, 26)
        Me.btsave.TabIndex = 15
        Me.btsave.Text = "Save"
        Me.btsave.UseVisualStyleBackColor = True
        '
        'btcancel
        '
        Me.btcancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btcancel.Location = New System.Drawing.Point(255, 480)
        Me.btcancel.Name = "btcancel"
        Me.btcancel.Size = New System.Drawing.Size(90, 26)
        Me.btcancel.TabIndex = 16
        Me.btcancel.Text = "Cancel"
        Me.btcancel.UseVisualStyleBackColor = True
        '
        'RegisterForm
        '
        Me.AcceptButton = Me.btsave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btcancel
        Me.ClientSize = New System.Drawing.Size(400, 530)
        Me.Controls.Add(Me.fnl)
        Me.Controls.Add(Me.lbsecondname)
        Me.Controls.Add(Me.lbgender)
        Me.Controls.Add(Me.lbdob)
        Me.Controls.Add(Me.lbyear)
        Me.Controls.Add(Me.lbhousenumber)
        Me.Controls.Add(Me.lbpostcode)
        Me.Controls.Add(Me.lbphonenumber)
        Me.Controls.Add(Me.lbphone2)
        Me.Controls.Add(Me.lbemail)
        Me.Controls.Add(Me.lbschoolbus)
        Me.Controls.Add(Me.lbaddress)
        Me.Controls.Add(Me.lbdistance)
        Me.Controls.Add(Me.lbpassword)
        Me.Controls.Add(Me.lbconfirm)
        Me.Controls.Add(Me.tbfirstname)
        Me.Controls.Add(Me.tbsecondname)
        Me.Controls.Add(Me.cbgender)
        Me.Controls.Add(Me.tpDOB)
        Me.Controls.Add(Me.cbyear)
        Me.Controls.Add(Me.nphousenumber)
        Me.Controls.Add(Me.tbpostcode)
        Me.Controls.Add(Me.tbphonenumber)
        Me.Controls.Add(Me.tbphone2)
        Me.Controls.Add(Me.tbemail)
        Me.Controls.Add(Me.chkschoolbus)
        Me.Controls.Add(Me.tbaddress)
        Me.Controls.Add(Me.npdistance)
        Me.Controls.Add(Me.tbpassword)
        Me.Controls.Add(Me.tbconfirm)
        Me.Controls.Add(Me.btsave)
        Me.Controls.Add(Me.btcancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RegisterForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Register"
        CType(Me.nphousenumber, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.npdistance, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub
    Friend WithEvents fnl As System.Windows.Forms.Label
    Friend WithEvents lbsecondname As System.Windows.Forms.Label
    Friend WithEvents lbgender As System.Windows.Forms.Label
    Friend WithEvents lbdob As System.Windows.Forms.Label
    Friend WithEvents lbyear As System.Windows.Forms.Label
    Friend WithEvents lbhousenumber As System.Windows.Forms.Label
    Friend WithEvents lbpostcode As System.Windows.Forms.Label
    Friend WithEvents lbphonenumber As System.Windows.Forms.Label
    Friend WithEvents lbphone2 As System.Windows.Forms.Label
    Friend WithEvents lbemail As System.Windows.Forms.Label
    Friend WithEvents lbschoolbus As System.Windows.Forms.Label
    Friend WithEvents lbaddress As System.Windows.Forms.Label
    Friend WithEvents lbdistance As System.Windows.Forms.Label
    Friend WithEvents lbpassword As System.Windows.Forms.Label
    Friend WithEvents lbconfirm As System.Windows.Forms.Label
    Friend WithEvents tbfirstname As System.Windows.Forms.TextBox
    Friend WithEvents tbsecondname As System.Windows.Forms.TextBox
    Friend WithEvents cbgender As System.Windows.Forms.ComboBox
    Friend WithEvents tpDOB As System.Windows.Forms.DateTimePicker
    Friend WithEvents cbyear As System.Windows.Forms.ComboBox
    Friend WithEvents nphousenumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents tbpostcode As System.Windows.Forms.TextBox
    Friend WithEvents tbphonenumber As System.Windows.Forms.TextBox
    Friend WithEvents tbphone2 As System.Windows.Forms.TextBox
    Friend WithEvents tbemail As System.Windows.Forms.TextBox
    Friend WithEvents chkschoolbus As System.Windows.Forms.CheckBox
    Friend WithEvents tbaddress As System.Windows.Forms.TextBox
    Friend WithEvents npdistance As System.Windows.Forms.NumericUpDown
    Friend WithEvents tbpassword As System.Windows.Forms.TextBox
    Friend WithEvents tbconfirm As System.Windows.Forms.TextBox
    Friend WithEvents btsave As System.Windows.Forms.Button
    Friend WithEvents btcancel As System.Windows.Forms.Button
End Class
