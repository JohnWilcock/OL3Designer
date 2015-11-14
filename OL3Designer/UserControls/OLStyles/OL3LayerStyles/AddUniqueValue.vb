Public Class AddUniqueValue
    Public LayerP As String
    Public fieldName As String

    Sub New(ByVal layerPath As String, ByVal fName As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        LayerP = layerPath
        fieldName = fName
        populateList()
    End Sub

    Public Sub populateList()
        Dim GDAL As New GDALImport
        'GDAL.getGeoJson({layerPath})
        Dim UVs As List(Of String) = GDAL.getFieldValues(LayerP, fieldName, True)
        CheckedListBox1.Items.AddRange(UVs.ToArray)


    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox1.Text <> "" Then
            CheckedListBox1.Items.Add(TextBox1.Text)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
    End Sub
End Class