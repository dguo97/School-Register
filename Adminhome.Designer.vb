<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Adminhome
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
        Me.lbtitle = New System.Windows.Forms.Label
        Me.btpupils = New System.Windows.Forms.Button
        Me.btteachers = New System.Windows.Forms.Button
        Me.btsubjects = New System.Windows.Forms.Button
        Me.btclubs = New System.Windows.Forms.Button
        Me.bttimetable = New System.Windows.Forms.Button
        Me.btenrol = New System.Windows.Forms.Button
        Me.btuniform = New System.Windows.Forms.Button
        Me.btbus = New System.Windows.Forms.Button
        Me.btlogout = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lbtitle
        '
        Me.lbtitle.AutoSize = True
        Me.lbtitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbtitle.Location = New System.Drawing.Point(138, 16)
        Me.lbtitle.Name = "lbtitle"
        Me.lbtitle.Size = New System.Drawing.Size(128, 20)
        Me.lbtitle.TabIndex = 9
        Me.lbtitle.Text = "Administrator"
        '
        'btpupils
        '
        Me.btpupils.Location = New System.Drawing.Point(30, 56)
        Me.btpupils.Name = "btpupils"
        Me.btpupils.Size = New System.Drawing.Size(170, 44)
        Me.btpupils.TabIndex = 0
        Me.btpupils.Text = "Pupils"
        Me.btpupils.UseVisualStyleBackColor = True
        '
        'btteachers
        '
        Me.btteachers.Location = New System.Drawing.Point(220, 56)
        Me.btteachers.Name = "btteachers"
        Me.btteachers.Size = New System.Drawing.Size(170, 44)
        Me.btteachers.TabIndex = 1
        Me.btteachers.Text = "Teachers"
        Me.btteachers.UseVisualStyleBackColor = True
        '
        'btsubjects
        '
        Me.btsubjects.Location = New System.Drawing.Point(30, 112)
        Me.btsubjects.Name = "btsubjects"
        Me.btsubjects.Size = New System.Drawing.Size(170, 44)
        Me.btsubjects.TabIndex = 2
        Me.btsubjects.Text = "Subjects"
        Me.btsubjects.UseVisualStyleBackColor = True
        '
        'btclubs
        '
        Me.btclubs.Location = New System.Drawing.Point(220, 112)
        Me.btclubs.Name = "btclubs"
        Me.btclubs.Size = New System.Drawing.Size(170, 44)
        Me.btclubs.TabIndex = 3
        Me.btclubs.Text = "Clubs"
        Me.btclubs.UseVisualStyleBackColor = True
        '
        'bttimetable
        '
        Me.bttimetable.Location = New System.Drawing.Point(30, 168)
        Me.bttimetable.Name = "bttimetable"
        Me.bttimetable.Size = New System.Drawing.Size(170, 44)
        Me.bttimetable.TabIndex = 4
        Me.bttimetable.Text = "Timetable"
        Me.bttimetable.UseVisualStyleBackColor = True
        '
        'btenrol
        '
        Me.btenrol.Location = New System.Drawing.Point(220, 168)
        Me.btenrol.Name = "btenrol"
        Me.btenrol.Size = New System.Drawing.Size(170, 44)
        Me.btenrol.TabIndex = 5
        Me.btenrol.Text = "Assign pupils"
        Me.btenrol.UseVisualStyleBackColor = True
        '
        'btuniform
        '
        Me.btuniform.Location = New System.Drawing.Point(30, 224)
        Me.btuniform.Name = "btuniform"
        Me.btuniform.Size = New System.Drawing.Size(170, 44)
        Me.btuniform.TabIndex = 6
        Me.btuniform.Text = "Uniform"
        Me.btuniform.UseVisualStyleBackColor = True
        '
        'btbus
        '
        Me.btbus.Location = New System.Drawing.Point(220, 224)
        Me.btbus.Name = "btbus"
        Me.btbus.Size = New System.Drawing.Size(170, 44)
        Me.btbus.TabIndex = 7
        Me.btbus.Text = "School bus list"
        Me.btbus.UseVisualStyleBackColor = True
        '
        'btlogout
        '
        Me.btlogout.Location = New System.Drawing.Point(125, 288)
        Me.btlogout.Name = "btlogout"
        Me.btlogout.Size = New System.Drawing.Size(170, 34)
        Me.btlogout.TabIndex = 8
        Me.btlogout.Text = "Log out"
        Me.btlogout.UseVisualStyleBackColor = True
        '
        'Adminhome
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(420, 346)
        Me.Controls.Add(Me.btlogout)
        Me.Controls.Add(Me.btbus)
        Me.Controls.Add(Me.btuniform)
        Me.Controls.Add(Me.btenrol)
        Me.Controls.Add(Me.bttimetable)
        Me.Controls.Add(Me.btclubs)
        Me.Controls.Add(Me.btsubjects)
        Me.Controls.Add(Me.btteachers)
        Me.Controls.Add(Me.btpupils)
        Me.Controls.Add(Me.lbtitle)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "Adminhome"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "School Register - Admin"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbtitle As System.Windows.Forms.Label
    Friend WithEvents btpupils As System.Windows.Forms.Button
    Friend WithEvents btteachers As System.Windows.Forms.Button
    Friend WithEvents btsubjects As System.Windows.Forms.Button
    Friend WithEvents btclubs As System.Windows.Forms.Button
    Friend WithEvents bttimetable As System.Windows.Forms.Button
    Friend WithEvents btenrol As System.Windows.Forms.Button
    Friend WithEvents btuniform As System.Windows.Forms.Button
    Friend WithEvents btbus As System.Windows.Forms.Button
    Friend WithEvents btlogout As System.Windows.Forms.Button
End Class
