Public Class OLStylePickerDialogPointControl
    Public styleSettings As New StyleProperties
    Public vertixFlag As Boolean = True

    Private Sub OLStylePickerDialogPointControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadValuesToControls()
    End Sub

    Sub loadValuesToControls()
        'load existing settings
        NumericUpDown1.Value = styleSettings.OLSize
        NumericUpDown2.Value = styleSettings.OLStrokeWidth
        NumericUpDown3.Value = styleSettings.OLVertices
        NumericUpDown4.Value = styleSettings.OLRotation

        ComboBox1.Text = styleSettings.OLRotationField

        Button1.BackColor = styleSettings.OLStrokeColor
        Button2.BackColor = styleSettings.OLFillColour

    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        styleSettings.OLSize = NumericUpDown1.Value
    End Sub

    Private Sub NumericUpDown2_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown2.ValueChanged
        styleSettings.OLStrokeWidth = NumericUpDown2.Value
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim sColour As New ColorDialog
        sColour.Color = styleSettings.OLStrokeColor
        sColour.ShowDialog()
        styleSettings.OLStrokeColor = sColour.Color
        Button1.BackColor = sColour.Color
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim fColour As New ColorDialog
        fColour.Color = styleSettings.OLFillColour
        fColour.ShowDialog()
        styleSettings.OLFillColour = fColour.Color
        Button2.BackColor = fColour.Color
    End Sub


    Private Sub NumericUpDown3_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown3.ValueChanged

        If vertixFlag Then
            vertixFlag = False

            'prevent shapes with 1 or 2 (these produce blank and a line)
            If NumericUpDown3.Value = 1 Then
                NumericUpDown3.Value = 3
            End If

            If NumericUpDown3.Value = 2 Then
                NumericUpDown3.Value = 0
            End If

            If NumericUpDown3.Value > 0 Then
                styleSettings.OLPointType = "RegularShape"
            Else
                styleSettings.OLPointType = "Circle"
            End If

            styleSettings.OLVertices = NumericUpDown3.Value

            vertixFlag = True
        End If
    End Sub

    Private Sub NumericUpDown4_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown4.ValueChanged
        styleSettings.OLRotation = NumericUpDown4.Value

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        styleSettings.OLRotationField = ComboBox1.Text
    End Sub
End Class
