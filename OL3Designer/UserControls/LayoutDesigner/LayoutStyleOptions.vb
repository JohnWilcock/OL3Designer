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



    Public Function save() As OL3LayoutStyleOptionsSaveObject
        save = New OL3LayoutStyleOptionsSaveObject
        save.Border = CheckBox1.Checked
        save.BorderColour = Button1.BackColor
        save.BorderWidth = NumericUpDown1.Value

    End Function

    Public Sub loadObj(ByVal saveObj As OL3LayoutStyleOptionsSaveObject)
        CheckBox1.Checked = saveObj.Border
        Button1.BackColor = saveObj.BorderColour
        NumericUpDown1.Value = saveObj.BorderWidth

    End Sub
End Class

<Serializable()> _
Public Class OL3LayoutStyleOptionsSaveObject
    Public Border As Boolean
    Public BorderColour As Color
    Public BorderWidth As Integer

End Class