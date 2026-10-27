<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class signin
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
        Me.sib = New System.Windows.Forms.Button
        Me.pwtx = New System.Windows.Forms.TextBox
        Me.pwl = New System.Windows.Forms.Label
        Me.untb = New System.Windows.Forms.TextBox
        Me.unl = New System.Windows.Forms.Label
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPageR = New System.Windows.Forms.TabPage
        Me.TabPageA = New System.Windows.Forms.TabPage
        Me.lbcp = New System.Windows.Forms.Label
        Me.btali = New System.Windows.Forms.Button
        Me.tbsp = New System.Windows.Forms.TextBox
        Me.Lbadminp = New System.Windows.Forms.Label
        Me.TabControl1.SuspendLayout()
        Me.TabPageR.SuspendLayout()
        Me.TabPageA.SuspendLayout()
        Me.SuspendLayout()
        '
        'sib
        '
        Me.sib.Location = New System.Drawing.Point(245, 164)
        Me.sib.Name = "sib"
        Me.sib.Size = New System.Drawing.Size(75, 23)
        Me.sib.TabIndex = 1
        Me.sib.Text = "Sign in"
        Me.sib.UseVisualStyleBackColor = True
        '
        'pwtx
        '
        Me.pwtx.Location = New System.Drawing.Point(140, 102)
        Me.pwtx.Name = "pwtx"
        Me.pwtx.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.pwtx.Size = New System.Drawing.Size(100, 20)
        Me.pwtx.TabIndex = 7
        '
        'pwl
        '
        Me.pwl.AutoSize = True
        Me.pwl.Location = New System.Drawing.Point(52, 104)
        Me.pwl.Name = "pwl"
        Me.pwl.Size = New System.Drawing.Size(53, 13)
        Me.pwl.TabIndex = 6
        Me.pwl.Text = "&Password"
        '
        'untb
        '
        Me.untb.Location = New System.Drawing.Point(140, 40)
        Me.untb.Name = "untb"
        Me.untb.Size = New System.Drawing.Size(100, 20)
        Me.untb.TabIndex = 5
        '
        'unl
        '
        Me.unl.AutoSize = True
        Me.unl.Location = New System.Drawing.Point(52, 51)
        Me.unl.Name = "unl"
        Me.unl.Size = New System.Drawing.Size(55, 13)
        Me.unl.TabIndex = 4
        Me.unl.Text = "&Teacher ID"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPageR)
        Me.TabControl1.Controls.Add(Me.TabPageA)
        Me.TabControl1.Location = New System.Drawing.Point(12, 13)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(352, 220)
        Me.TabControl1.TabIndex = 9
        '
        'TabPageR
        '
        Me.TabPageR.Controls.Add(Me.unl)
        Me.TabPageR.Controls.Add(Me.untb)
        Me.TabPageR.Controls.Add(Me.sib)
        Me.TabPageR.Controls.Add(Me.pwtx)
        Me.TabPageR.Controls.Add(Me.pwl)
        Me.TabPageR.Location = New System.Drawing.Point(4, 22)
        Me.TabPageR.Name = "TabPageR"
        Me.TabPageR.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageR.Size = New System.Drawing.Size(344, 194)
        Me.TabPageR.TabIndex = 0
        Me.TabPageR.Text = "Teachers"
        Me.TabPageR.UseVisualStyleBackColor = True
        '
        'TabPageA
        '
        Me.TabPageA.Controls.Add(Me.lbcp)
        Me.TabPageA.Controls.Add(Me.btali)
        Me.TabPageA.Controls.Add(Me.tbsp)
        Me.TabPageA.Controls.Add(Me.Lbadminp)
        Me.TabPageA.Location = New System.Drawing.Point(4, 22)
        Me.TabPageA.Name = "TabPageA"
        Me.TabPageA.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageA.Size = New System.Drawing.Size(344, 194)
        Me.TabPageA.TabIndex = 1
        Me.TabPageA.Text = "admin"
        Me.TabPageA.UseVisualStyleBackColor = True
        '
        'lbcp
        '
        Me.lbcp.AutoSize = True
        Me.lbcp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbcp.Location = New System.Drawing.Point(127, 124)
        Me.lbcp.Name = "lbcp"
        Me.lbcp.Size = New System.Drawing.Size(91, 13)
        Me.lbcp.TabIndex = 3
        Me.lbcp.Text = "change password"
        '
        'btali
        '
        Me.btali.Location = New System.Drawing.Point(243, 146)
        Me.btali.Name = "btali"
        Me.btali.Size = New System.Drawing.Size(75, 25)
        Me.btali.TabIndex = 2
        Me.btali.Text = "Log in"
        Me.btali.UseVisualStyleBackColor = True
        '
        'tbsp
        '
        Me.tbsp.Location = New System.Drawing.Point(103, 60)
        Me.tbsp.Name = "tbsp"
        Me.tbsp.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.tbsp.Size = New System.Drawing.Size(141, 20)
        Me.tbsp.TabIndex = 1
        '
        'Lbadminp
        '
        Me.Lbadminp.AutoSize = True
        Me.Lbadminp.Location = New System.Drawing.Point(25, 60)
        Me.Lbadminp.Name = "Lbadminp"
        Me.Lbadminp.Size = New System.Drawing.Size(56, 13)
        Me.Lbadminp.TabIndex = 0
        Me.Lbadminp.Text = "Password:"
        '
        'signin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(373, 250)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "signin"
        Me.ShowInTaskbar = True
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "School Register - Sign in"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPageR.ResumeLayout(False)
        Me.TabPageR.PerformLayout()
        Me.TabPageA.ResumeLayout(False)
        Me.TabPageA.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents sib As System.Windows.Forms.Button
    Friend WithEvents pwtx As System.Windows.Forms.TextBox
    Friend WithEvents pwl As System.Windows.Forms.Label
    Friend WithEvents untb As System.Windows.Forms.TextBox
    Friend WithEvents unl As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPageR As System.Windows.Forms.TabPage
    Friend WithEvents TabPageA As System.Windows.Forms.TabPage
    Friend WithEvents tbsp As System.Windows.Forms.TextBox
    Friend WithEvents Lbadminp As System.Windows.Forms.Label
    Friend WithEvents btali As System.Windows.Forms.Button
    Friend WithEvents lbcp As System.Windows.Forms.Label

End Class
