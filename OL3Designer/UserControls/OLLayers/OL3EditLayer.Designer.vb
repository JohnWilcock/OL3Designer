<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OL3EditLayer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OL3EditLayer))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.OL3General1 = New OL3Designer.OL3General()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.OL3LayerStylePicker1 = New OL3Designer.OL3LayerStylePicker()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.OL3LayerPopupPicker1 = New OL3Designer.OL3LayerPopupPicker()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.OlLayerFilterPicker1 = New OL3Designer.OLLayerFilterPicker()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.OL3Projections1 = New OL3Designer.OL3Projections()
        Me.TabControl1.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(661, 351)
        Me.TabControl1.TabIndex = 8
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.OL3General1)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(653, 325)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "General"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'OL3General1
        '
        Me.OL3General1.Location = New System.Drawing.Point(6, 6)
        Me.OL3General1.Name = "OL3General1"
        Me.OL3General1.Size = New System.Drawing.Size(505, 270)
        Me.OL3General1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.OL3LayerStylePicker1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(653, 325)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Symbology"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'OL3LayerStylePicker1
        '
        Me.OL3LayerStylePicker1.Location = New System.Drawing.Point(6, 6)
        Me.OL3LayerStylePicker1.Name = "OL3LayerStylePicker1"
        Me.OL3LayerStylePicker1.Size = New System.Drawing.Size(637, 305)
        Me.OL3LayerStylePicker1.TabIndex = 6
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.OL3LayerPopupPicker1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(653, 325)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Popups"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'OL3LayerPopupPicker1
        '
        Me.OL3LayerPopupPicker1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.OL3LayerPopupPicker1.Location = New System.Drawing.Point(3, 3)
        Me.OL3LayerPopupPicker1.Name = "OL3LayerPopupPicker1"
        Me.OL3LayerPopupPicker1.Size = New System.Drawing.Size(647, 319)
        Me.OL3LayerPopupPicker1.TabIndex = 0
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.OlLayerFilterPicker1)
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage5.Size = New System.Drawing.Size(653, 325)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "Filters"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'OlLayerFilterPicker1
        '
        Me.OlLayerFilterPicker1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.OlLayerFilterPicker1.Location = New System.Drawing.Point(3, 3)
        Me.OlLayerFilterPicker1.Name = "OlLayerFilterPicker1"
        Me.OlLayerFilterPicker1.Size = New System.Drawing.Size(647, 319)
        Me.OlLayerFilterPicker1.TabIndex = 0
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.OL3Projections1)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(653, 325)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Projection"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'OL3Projections1
        '
        Me.OL3Projections1.ChosenCoordSystemEPSG = "4326"
        Me.OL3Projections1.ChosenCoordSystemWKT = Nothing
        Me.OL3Projections1.Location = New System.Drawing.Point(6, 6)
        Me.OL3Projections1.Name = "OL3Projections1"
        Me.OL3Projections1.Size = New System.Drawing.Size(462, 226)
        Me.OL3Projections1.TabIndex = 0
        '
        'OL3EditLayer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(687, 373)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "OL3EditLayer"
        Me.Text = "Edit  Layer"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents OL3LayerStylePicker1 As OL3Designer.OL3LayerStylePicker
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents OL3LayerPopupPicker1 As OL3Designer.OL3LayerPopupPicker
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents OL3Projections1 As OL3Designer.OL3Projections
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents OL3General1 As OL3Designer.OL3General
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents OlLayerFilterPicker1 As OL3Designer.OLLayerFilterPicker
End Class
