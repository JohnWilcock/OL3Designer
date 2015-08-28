Public Class SizeRamps
    Public sizeFrom As Double = 8
    Public sizeTo As Double = 8



    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        sizeFrom = ComboBox1.Text
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        sizeTo = ComboBox2.Text
    End Sub

    Public Function getSize(ByVal numValues As Integer, ByVal sequenceNum As Integer) As Double
        Dim diff As Double = sizeTo - sizeFrom
        Dim increment As Double = diff / numValues

        Return sizeFrom + Math.Round((increment * sequenceNum), 1)

    End Function
End Class
