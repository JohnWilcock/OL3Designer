<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OL3LayerStyleNumericRanges
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
        Me.OLStyle = New System.Windows.Forms.DataGridViewImageColumn()
        Me.OLValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.OLEndValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SizeRamps1 = New OL3Designer.SizeRamps()
        Me.ComboBox3 = New System.Windows.Forms.ComboBox()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.OLValue.HeaderText = "Start Value"
        Me.OLValue.Name = "OLValue"
        Me.OLValue.Width = 85
        '
        'OLEndValue
        '
        Me.OLEndValue.HeaderText = "End Value"
        Me.OLEndValue.Name = "OLEndValue"
        Me.OLEndValue.Width = 85
        '
        'Label
        '
        Me.Label.HeaderText = "Label"
        Me.Label.Name = "Label"
        Me.Label.Width = 40
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "Column2"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
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
        'ComboBox3
        '
        Me.ComboBox3.FormattingEnabled = True
        Me.ComboBox3.Items.AddRange(New Object() {"2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20"})
        Me.ComboBox3.Location = New System.Drawing.Point(86, 64)
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.Size = New System.Drawing.Size(37, 21)
        Me.ComboBox3.TabIndex = 6
        Me.ComboBox3.Text = "2"
        '
        'OL3LayerStyleNumericRanges
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ComboBox3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.SizeRamps1)
        Me.Controls.Add(Me.ComboBox2)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Name = "OL3LayerStyleNumericRanges"
        Me.Size = New System.Drawing.Size(320, 260)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OLStyle As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents OLValue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OLEndValue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SizeRamps1 As OL3Designer.SizeRamps
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboBox3 As System.Windows.Forms.ComboBox

End Class
