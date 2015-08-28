<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OL3LayerStyleFeatureStyle
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
        Dim StyleProperties1 As OL3Designer.StyleProperties = New OL3Designer.StyleProperties()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.OlStylePicker1 = New OL3Designer.OLStylePicker()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.OlStylePicker1)
        Me.GroupBox1.Location = New System.Drawing.Point(15, 13)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(280, 99)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Style"
        '
        'OlStylePicker1
        '
        Me.OlStylePicker1.fieldList = Nothing
        Me.OlStylePicker1.Location = New System.Drawing.Point(36, 29)
        Me.OlStylePicker1.Name = "OlStylePicker1"
        StyleProperties1.OLAnchorOrigin = "bottom-left"
        StyleProperties1.OLFillColour = System.Drawing.Color.AliceBlue
        StyleProperties1.OLGeomType = "Point"
        StyleProperties1.OLLineCap = "round"
        StyleProperties1.OLLineDash = 0
        StyleProperties1.OLLineJoin = "round"
        StyleProperties1.OLMaskColor = System.Drawing.Color.White
        StyleProperties1.OLMaskWidth = 2
        StyleProperties1.OLMiterLimit = 10
        StyleProperties1.OLRotation = 0
        StyleProperties1.OLScale = 1.0R
        StyleProperties1.OLSize = 8
        StyleProperties1.OLSRC = "C:/Users/John/Documents/visual studio 2013/Projects/OL3Designer/OL3Designer/test/" & _
    "icon2.png"
        StyleProperties1.OLStrokeColor = System.Drawing.Color.Aquamarine
        StyleProperties1.OLStrokeWidth = 2
        StyleProperties1.OLTextCol = ""
        StyleProperties1.OLTextColour = System.Drawing.Color.Black
        StyleProperties1.OLTextFont = "Arial"
        StyleProperties1.OLTextHAlign = "Center"
        StyleProperties1.OLTextRotation = 0
        StyleProperties1.OLTextSize = 8
        StyleProperties1.OlTextTransparancy = 1.0R
        StyleProperties1.OLTextVAlign = "Center"
        StyleProperties1.OLTextXOffset = 0
        StyleProperties1.OLTextYOffset = 0
        StyleProperties1.OlTransparancy = 1.0R
        StyleProperties1.OLXAnchor = 0.0R
        StyleProperties1.OLXAnchorUnit = "fraction"
        StyleProperties1.OLXOffset = 0
        StyleProperties1.OLYAnchor = 0.0R
        StyleProperties1.OLYAnchorUnit = "pixels"
        StyleProperties1.OLYOffset = 0
        Me.OlStylePicker1.OLStyleSettings = StyleProperties1
        Me.OlStylePicker1.Size = New System.Drawing.Size(50, 50)
        Me.OlStylePicker1.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.TextBox1)
        Me.GroupBox2.Location = New System.Drawing.Point(15, 118)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(280, 100)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Description"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(57, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Key text"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(60, 43)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(132, 39)
        Me.TextBox1.TabIndex = 0
        '
        'OL3LayerStyleFeatureStyle
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "OL3LayerStyleFeatureStyle"
        Me.Size = New System.Drawing.Size(316, 232)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents OlStylePicker1 As OL3Designer.OLStylePicker

End Class
