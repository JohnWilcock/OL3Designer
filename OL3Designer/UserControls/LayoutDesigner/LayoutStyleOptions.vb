Public Class LayoutStyleOptions

    Public Function getstyleCSS() As String
        Dim styleCSS As String = ""
        'borders
        If CheckBox1.Checked Then
            styleCSS = styleCSS & "border:solid;border-width:" & NumericUpDown1.Value & "px;" & "border-color:" & ColorTranslator.ToHtml(Button1.BackColor) & ";"
        End If

        Return styleCSS
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim cPicker As New ColorDialog
        cPicker.Color = Button1.BackColor
        If cPicker.ShowDialog() = DialogResult.OK Then
            Button1.BackColor = cPicker.Color
        End If
    End Sub

    Private Sub LayoutStyleOptions_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class