<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LayoutDesigner
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Sp1 = New OL3Designer.SP()
        Me.Panel1 = New OL3Designer.modifiedPanel()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.Sp1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Sp1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Sp1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 1, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(728, 334)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Sp1
        '
        Me.Sp1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Sp1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Sp1.Location = New System.Drawing.Point(3, 3)
        Me.Sp1.Name = "Sp1"
        Me.Sp1.Size = New System.Drawing.Size(472, 328)
        Me.Sp1.SplitterDistance = 156
        Me.Sp1.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(481, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(244, 328)
        Me.Panel1.TabIndex = 1
        '
        'LayoutDesigner
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Name = "LayoutDesigner"
        Me.Size = New System.Drawing.Size(728, 334)
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.Sp1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Sp1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Sp1 As OL3Designer.SP
    Friend WithEvents Panel1 As OL3Designer.modifiedPanel

End Class
