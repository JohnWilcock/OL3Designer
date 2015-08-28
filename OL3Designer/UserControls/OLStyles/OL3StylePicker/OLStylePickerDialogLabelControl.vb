Public Class OLStylePickerDialogLabelControl
    Public styleSettings As StyleProperties

    Private Sub NumericUpDown3_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown3.ValueChanged
        styleSettings.OLTextSize = NumericUpDown3.Value
    End Sub

    Sub loadValuesToControls()
        'load existing settings
        NumericUpDown3.Value = styleSettings.OLTextSize
        ComboBox5.Text = styleSettings.OLTextFont
        NumericUpDown1.Value = styleSettings.OLTextXOffset
        NumericUpDown2.Value = styleSettings.OLTextYOffset
        NumericUpDown4.Value = styleSettings.OLMaskWidth
        Button11.BackColor = styleSettings.OLMaskColor
        Button10.BackColor = styleSettings.OLTextColour
    End Sub

    Private Sub OLStylePickerDialogLabelControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadValuesToControls()
    End Sub

    Private Sub ComboBox5_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox5.SelectedIndexChanged
        styleSettings.OLTextFont = ComboBox5.Text
    End Sub

    Private Sub NumericUpDown4_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown4.ValueChanged
        styleSettings.OLMaskWidth = NumericUpDown4.Value
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        styleSettings.OLTextXOffset = NumericUpDown1.Value
    End Sub

    Private Sub NumericUpDown2_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown2.ValueChanged
        styleSettings.OLTextYOffset = NumericUpDown2.Value
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Dim sColour As New ColorDialog
        sColour.Color = styleSettings.OLTextColour
        sColour.ShowDialog()
        styleSettings.OLTextColour = sColour.Color
        Button10.BackColor = sColour.Color
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Dim sColour As New ColorDialog
        sColour.Color = styleSettings.OLMaskColor
        sColour.ShowDialog()
        styleSettings.OLMaskColor = sColour.Color
        Button11.BackColor = sColour.Color
    End Sub

    Public vertAlignment As String = ""
    Public horizAlignment As String = ""

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        clearAlignment()
        If RadioButton1.Checked Then RadioButton1.FlatAppearance.BorderSize = 2


        styleSettings.OLTextVAlign = "bottom"
        styleSettings.OLTextHAlign = "center"
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        clearAlignment()
        If RadioButton2.Checked Then RadioButton2.FlatAppearance.BorderSize = 2

        styleSettings.OLTextVAlign = "bottom"
        styleSettings.OLTextHAlign = "right"
    End Sub

    Private Sub RadioButton5_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton5.CheckedChanged
        clearAlignment()
        If RadioButton5.Checked Then RadioButton5.FlatAppearance.BorderSize = 2

        styleSettings.OLTextVAlign = "top"
        styleSettings.OLTextHAlign = "left"
    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton4.CheckedChanged
        clearAlignment()
        If RadioButton4.Checked Then RadioButton4.FlatAppearance.BorderSize = 2

        styleSettings.OLTextVAlign = "top"
        styleSettings.OLTextHAlign = "center"
    End Sub

    Private Sub RadioButton6_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton6.CheckedChanged
        clearAlignment()
        If RadioButton6.Checked Then RadioButton6.FlatAppearance.BorderSize = 2

        styleSettings.OLTextVAlign = "top"
        styleSettings.OLTextHAlign = "right"
    End Sub

    Private Sub RadioButton7_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton7.CheckedChanged
        clearAlignment()
        If RadioButton7.Checked Then RadioButton7.FlatAppearance.BorderSize = 2

        styleSettings.OLTextVAlign = "middle"
        styleSettings.OLTextHAlign = "left"
    End Sub

    Private Sub RadioButton8_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton8.CheckedChanged
        clearAlignment()
        If RadioButton8.Checked Then RadioButton8.FlatAppearance.BorderSize = 2

        styleSettings.OLTextVAlign = "middle"
        styleSettings.OLTextHAlign = "center"
    End Sub

    Private Sub RadioButton9_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton9.CheckedChanged
        clearAlignment()
        If RadioButton9.Checked Then RadioButton9.FlatAppearance.BorderSize = 2

        styleSettings.OLTextVAlign = "middle"
        styleSettings.OLTextHAlign = "right"
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        clearAlignment()
        If RadioButton3.Checked Then RadioButton3.FlatAppearance.BorderSize = 2

        styleSettings.OLTextVAlign = "bottom"
        styleSettings.OLTextHAlign = "left"
    End Sub

    Sub clearAlignment()
        RadioButton1.FlatAppearance.BorderSize = 1
        RadioButton2.FlatAppearance.BorderSize = 1
        RadioButton3.FlatAppearance.BorderSize = 1

        RadioButton4.FlatAppearance.BorderSize = 1
        RadioButton5.FlatAppearance.BorderSize = 1
        RadioButton6.FlatAppearance.BorderSize = 1

        RadioButton7.FlatAppearance.BorderSize = 1
        RadioButton8.FlatAppearance.BorderSize = 1
        RadioButton9.FlatAppearance.BorderSize = 1


    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        styleSettings.OLTextCol = ComboBox1.Text
    End Sub
End Class
