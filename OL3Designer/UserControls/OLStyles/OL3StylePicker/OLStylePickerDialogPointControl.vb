Public Class OLStylePickerDialogPointControl
    Public styleSettings As New StyleProperties
    Private Sub OLStylePickerDialogPointControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadValuesToControls()
    End Sub

    Sub loadValuesToControls()
        'load existing settings
        NumericUpDown1.Value = styleSettings.OLSize
        NumericUpDown2.Value = styleSettings.OLStrokeWidth
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


End Class
