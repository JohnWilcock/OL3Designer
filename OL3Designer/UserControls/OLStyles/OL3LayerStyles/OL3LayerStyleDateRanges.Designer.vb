<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OL3LayerStyleDateRanges
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
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NumericUpDown1 = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SizeRamps1 = New OL3Designer.SizeRamps()
        Me.OLStyle = New System.Windows.Forms.DataGridViewImageColumn()
        Me.OLValue = New OL3Designer.CalendarColumn()
        Me.OLEndValue = New OL3Designer.CalendarColumn()
        Me.Label = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(3, 34)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox1.TabIndex = 1
        '
        'ComboBox2
        '
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(186, 34)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(121, 21)
        Me.ComboBox2.TabIndex = 2
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.OLStyle, Me.OLValue, Me.OLEndValue, Me.Label})
        Me.DataGridView1.Location = New System.Drawing.Point(3, 108)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersWidth = 20
        Me.DataGridView1.Size = New System.Drawing.Size(304, 114)
        Me.DataGridView1.TabIndex = 0
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "Column2"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        '
        'NumericUpDown1
        '
        Me.NumericUpDown1.Location = New System.Drawing.Point(86, 65)
        Me.NumericUpDown1.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.NumericUpDown1.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumericUpDown1.Name = "NumericUpDown1"
        Me.NumericUpDown1.Size = New System.Drawing.Size(38, 20)
        Me.NumericUpDown1.TabIndex = 3
        Me.NumericUpDown1.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(36, 67)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Ranges"
        '
        'SizeRamps1
        '
        Me.SizeRamps1.Location = New System.Drawing.Point(186, 62)
        Me.SizeRamps1.Name = "SizeRamps1"
        Me.SizeRamps1.Size = New System.Drawing.Size(122, 30)
        Me.SizeRamps1.TabIndex = 4
        '
        'OLStyle
        '
        Me.OLStyle.HeaderText = "Style"
        Me.OLStyle.Name = "OLStyle"
        Me.OLStyle.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.OLStyle.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.OLStyle.Width = 60
        '
        'OLValue
        '
        Me.OLValue.HeaderText = "Start Date"
        Me.OLValue.Name = "OLValue"
        Me.OLValue.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.OLValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.OLValue.Width = 85
        '
        'OLEndValue
        '
        Me.OLEndValue.HeaderText = "End Date"
        Me.OLEndValue.Name = "OLEndValue"
        Me.OLEndValue.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.OLEndValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.OLEndValue.Width = 85
        '
        'Label
        '
        Me.Label.HeaderText = "Label"
        Me.Label.Name = "Label"
        Me.Label.Width = 40
        '
        'OL3LayerStyleDateRanges
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.SizeRamps1)
        Me.Controls.Add(Me.NumericUpDown1)
        Me.Controls.Add(Me.ComboBox2)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Name = "OL3LayerStyleDateRanges"
        Me.Size = New System.Drawing.Size(320, 260)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericUpDown1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NumericUpDown1 As System.Windows.Forms.NumericUpDown
    Friend WithEvents SizeRamps1 As OL3Designer.SizeRamps
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents OLStyle As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents OLValue As OL3Designer.CalendarColumn
    Friend WithEvents OLEndValue As OL3Designer.CalendarColumn
    Friend WithEvents Label As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
