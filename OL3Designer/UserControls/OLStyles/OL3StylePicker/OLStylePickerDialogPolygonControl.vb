Public Class OLStylePickerDialogPolygonControl
    Public styleSettings As New StyleProperties
    Private Sub OLStylePickerDialogPolygonControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadValuesToControls()
    End Sub

    Sub loadValuesToControls()
        'load existing settings
        NumericUpDown2.Value = styleSettings.OLStrokeWidth
        Button1.BackColor = styleSettings.OLStrokeColor
        NumericUpDown1.Value = styleSettings.OLLineDash
        NumericUpDown3.Value = styleSettings.OLMiterLimit
        ComboBox2.Text = styleSettings.OLLineJoin
        ComboBox1.Text = styleSettings.OLLineCap
        Button2.BackColor = styleSettings.OLFillColour
        NumericUpDown4.Value = 100 - (styleSettings.OlTransparancy * 100)
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

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        styleSettings.OLLineCap = ComboBox1.Text
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        styleSettings.OLLineJoin = ComboBox2.Text
    End Sub

    Private Sub NumericUpDown3_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown3.ValueChanged
        styleSettings.OLMiterLimit = NumericUpDown3.Value
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        styleSettings.OLLineDash = NumericUpDown1.Value
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim fColour As New ColorDialog
        fColour.Color = styleSettings.OLFillColour
        fColour.ShowDialog()
        styleSettings.OLFillColour = fColour.Color
        Button2.BackColor = fColour.Color
    End Sub

    Private Sub NumericUpDown4_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown4.ValueChanged
        styleSettings.OlTransparancy = 1 - (NumericUpDown4.Value / 100)
    End Sub
End Class
