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


    Public Function save() As OL3FixedSaveObject
        save = New OL3FixedSaveObject
        save.dimension = dimension
        save.fixed = fixed
        save.Ori = Ori



    End Function

    Public Sub loadObj(ByVal saveObj As OL3FixedSaveObject)
        dimension = saveObj.dimension
        fixed = saveObj.fixed
        Ori = saveObj.Ori


    End Sub

End Class

<Serializable()> _
Public Class OL3FixedSaveObject
    Public fixed As Boolean
    Public dimension As Integer
    Public Ori As String

End Class
