Public Class FixedDimensions
    Public fixed As Boolean = False
    Public dimension As Integer

    Private _Ori As String = ""
    Public Property Ori As String
        Get
            Return _Ori
        End Get
        Set(ByVal value As String)
            _Ori = value
            RaiseEvent onOriChange()
        End Set
    End Property

    Public Event onOriChange()


    Sub setImage() Handles Me.onOriChange
        If Ori = "H" Then
            PictureBox1.Image = My.Resources.size
            CheckBox1.Text = "Fixed Width"
        Else
            PictureBox1.Image = My.Resources.size
            PictureBox1.Image.RotateFlip(RotateFlipType.Rotate90FlipNone)
            CheckBox1.Text = "Fixed Height"
        End If
    End Sub




    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            NumericUpDown1.Enabled = True
            fixed = True
        Else
            NumericUpDown1.Enabled = False
            fixed = False

        End If
    End Sub




    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        dimension = NumericUpDown1.Value
    End Sub
End Class
