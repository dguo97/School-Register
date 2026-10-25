<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class teacherhomepage
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
        Me.btstudents = New System.Windows.Forms.Button
        Me.btsubjects = New System.Windows.Forms.Button
        Me.btclubs = New System.Windows.Forms.Button
        Me.bttimetable = New System.Windows.Forms.Button
        Me.btenrol = New System.Windows.Forms.Button
        Me.btuniform = New System.Windows.Forms.Button
        Me.btbus = New System.Windows.Forms.Button
        Me.btprofile = New System.Windows.Forms.Button
        Me.btchangepw = New System.Windows.Forms.Button
        Me.btlogout = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btstudents
        '
        Me.btstudents.Location = New System.Drawing.Point(30, 24)
        Me.btstudents.Name = "btstudents"
        Me.btstudents.Size = New System.Drawing.Size(170, 44)
        Me.btstudents.TabIndex = 0
        Me.btstudents.Text = "Students"
        Me.btstudents.UseVisualStyleBackColor = True
        '
        'btsubjects
        '
        Me.btsubjects.Location = New System.Drawing.Point(220, 24)
        Me.btsubjects.Name = "btsubjects"
        Me.btsubjects.Size = New System.Drawing.Size(170, 44)
        Me.btsubjects.TabIndex = 1
        Me.btsubjects.Text = "Subjects"
        Me.btsubjects.UseVisualStyleBackColor = True
        '
        'btclubs
        '
        Me.btclubs.Location = New System.Drawing.Point(30, 80)
        Me.btclubs.Name = "btclubs"
        Me.btclubs.Size = New System.Drawing.Size(170, 44)
        Me.btclubs.TabIndex = 2
        Me.btclubs.Text = "Clubs"
        Me.btclubs.UseVisualStyleBackColor = True
        '
        'bttimetable
        '
        Me.bttimetable.Location = New System.Drawing.Point(220, 80)
        Me.bttimetable.Name = "bttimetable"
        Me.bttimetable.Size = New System.Drawing.Size(170, 44)
        Me.bttimetable.TabIndex = 3
        Me.bttimetable.Text = "Timetable"
        Me.bttimetable.UseVisualStyleBackColor = True
        '
        'btenrol
        '
        Me.btenrol.Location = New System.Drawing.Point(30, 136)
        Me.btenrol.Name = "btenrol"
        Me.btenrol.Size = New System.Drawing.Size(170, 44)
        Me.btenrol.TabIndex = 4
        Me.btenrol.Text = "Assign pupils"
        Me.btenrol.UseVisualStyleBackColor = True
        '
        'btuniform
        '
        Me.btuniform.Location = New System.Drawing.Point(220, 136)
        Me.btuniform.Name = "btuniform"
        Me.btuniform.Size = New System.Drawing.Size(170, 44)
        Me.btuniform.TabIndex = 5
        Me.btuniform.Text = "Uniform"
        Me.btuniform.UseVisualStyleBackColor = True
        '
        'btbus
        '
        Me.btbus.Location = New System.Drawing.Point(30, 192)
        Me.btbus.Name = "btbus"
        Me.btbus.Size = New System.Drawing.Size(170, 44)
        Me.btbus.TabIndex = 6
        Me.btbus.Text = "School bus list"
        Me.btbus.UseVisualStyleBackColor = True
        '
        'btprofile
        '
        Me.btprofile.Location = New System.Drawing.Point(220, 192)
        Me.btprofile.Name = "btprofile"
        Me.btprofile.Size = New System.Drawing.Size(170, 44)
        Me.btprofile.TabIndex = 7
        Me.btprofile.Text = "My profile"
        Me.btprofile.UseVisualStyleBackColor = True
        '
        'btchangepw
        '
        Me.btchangepw.Location = New System.Drawing.Point(30, 248)
        Me.btchangepw.Name = "btchangepw"
        Me.btchangepw.Size = New System.Drawing.Size(170, 44)
        Me.btchangepw.TabIndex = 8
        Me.btchangepw.Text = "Change my password"
        Me.btchangepw.UseVisualStyleBackColor = True
        '
        'btlogout
        '
        Me.btlogout.Location = New System.Drawing.Point(220, 248)
        Me.btlogout.Name = "btlogout"
        Me.btlogout.Size = New System.Drawing.Size(170, 44)
        Me.btlogout.TabIndex = 9
        Me.btlogout.Text = "Log out"
        Me.btlogout.UseVisualStyleBackColor = True
        '
        'teacherhomepage
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(420, 316)
        Me.Controls.Add(Me.btlogout)
        Me.Controls.Add(Me.btchangepw)
        Me.Controls.Add(Me.btprofile)
        Me.Controls.Add(Me.btbus)
        Me.Controls.Add(Me.btuniform)
        Me.Controls.Add(Me.btenrol)
        Me.Controls.Add(Me.bttimetable)
        Me.Controls.Add(Me.btclubs)
        Me.Controls.Add(Me.btsubjects)
        Me.Controls.Add(Me.btstudents)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "teacherhomepage"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "School Register - Teacher"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btstudents As System.Windows.Forms.Button
    Friend WithEvents btsubjects As System.Windows.Forms.Button
    Friend WithEvents btclubs As System.Windows.Forms.Button
    Friend WithEvents bttimetable As System.Windows.Forms.Button
    Friend WithEvents btenrol As System.Windows.Forms.Button
    Friend WithEvents btuniform As System.Windows.Forms.Button
    Friend WithEvents btbus As System.Windows.Forms.Button
    Friend WithEvents btprofile As System.Windows.Forms.Button
    Friend WithEvents btchangepw As System.Windows.Forms.Button
    Friend WithEvents btlogout As System.Windows.Forms.Button
End Class
