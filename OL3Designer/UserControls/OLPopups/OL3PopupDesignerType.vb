Public Class OL3PopupDesignerType

    Private Sub OL3PopupDesignerType_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub


    Function GetPopupDesignerType() As String
        If RadioButton1.Checked Then
            Return "Simple"
        Else
            Return "Complex"
        End If
    End Function
End Class
