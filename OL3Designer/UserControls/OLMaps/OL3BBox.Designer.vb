<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OL3BBox
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.XtopLeft = New System.Windows.Forms.NumericUpDown()
        Me.YtopLeft = New System.Windows.Forms.NumericUpDown()
        Me.XbottomRight = New System.Windows.Forms.NumericUpDown()
        Me.YbottomRight = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        CType(Me.XtopLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.YtopLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XbottomRight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.YbottomRight, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'XtopLeft
        '
        Me.XtopLeft.DecimalPlaces = 4
        Me.XtopLeft.Location = New System.Drawing.Point(21, 6)
        Me.XtopLeft.Maximum = New Decimal(New Integer() {99999999, 0, 0, 0})
        Me.XtopLeft.Minimum = New Decimal(New Integer() {99999999, 0, 0, -2147483648})
        Me.XtopLeft.Name = "XtopLeft"
        Me.XtopLeft.Size = New System.Drawing.Size(72, 20)
        Me.XtopLeft.TabIndex = 0
        '
        'YtopLeft
        '
        Me.YtopLeft.DecimalPlaces = 4
        Me.YtopLeft.Location = New System.Drawing.Point(21, 32)
        Me.YtopLeft.Maximum = New Decimal(New Integer() {99999999, 0, 0, 0})
        Me.YtopLeft.Minimum = New Decimal(New Integer() {99999999, 0, 0, -2147483648})
        Me.YtopLeft.Name = "YtopLeft"
        Me.YtopLeft.Size = New System.Drawing.Size(72, 20)
        Me.YtopLeft.TabIndex = 1
        '
        'XbottomRight
        '
        Me.XbottomRight.DecimalPlaces = 4
        Me.XbottomRight.Location = New System.Drawing.Point(74, 68)
        Me.XbottomRight.Maximum = New Decimal(New Integer() {99999999, 0, 0, 0})
        Me.XbottomRight.Minimum = New Decimal(New Integer() {99999999, 0, 0, -2147483648})
        Me.XbottomRight.Name = "XbottomRight"
        Me.XbottomRight.Size = New System.Drawing.Size(71, 20)
        Me.XbottomRight.TabIndex = 2
        '
        'YbottomRight
        '
        Me.YbottomRight.DecimalPlaces = 4
        Me.YbottomRight.Location = New System.Drawing.Point(74, 94)
        Me.YbottomRight.Maximum = New Decimal(New Integer() {99999999, 0, 0, 0})
        Me.YbottomRight.Minimum = New Decimal(New Integer() {99999999, 0, 0, -2147483648})
        Me.YbottomRight.Name = "YbottomRight"
        Me.YbottomRight.Size = New System.Drawing.Size(71, 20)
        Me.YbottomRight.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(54, 70)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(14, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "X"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(54, 96)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(14, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Y"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 34)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(14, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Y"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(14, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "X"
        '
        'OL3BBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.YbottomRight)
        Me.Controls.Add(Me.XbottomRight)
        Me.Controls.Add(Me.YtopLeft)
        Me.Controls.Add(Me.XtopLeft)
        Me.Name = "OL3BBox"
        Me.Size = New System.Drawing.Size(160, 127)
        CType(Me.XtopLeft, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.YtopLeft, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XbottomRight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.YbottomRight, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents XtopLeft As System.Windows.Forms.NumericUpDown
    Public WithEvents YtopLeft As System.Windows.Forms.NumericUpDown
    Public WithEvents XbottomRight As System.Windows.Forms.NumericUpDown
    Public WithEvents YbottomRight As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label

End Class
