Public Class OL3LayerStyleFeatureStyle
    Public layerType As String
    Public layerPath As String

    Sub New(ByVal layerT As String, ByVal layerP As String)

        ' This call is required by the designer.
        InitializeComponent()
        layerPath = layerP
        Dim GDAL As New GDALImport

        ' Add any initialization after the InitializeComponent() call.
        layerType = layerT
        OlStylePicker1.OLStyleSettings.OLGeomType = layerType
        OlStylePicker1.layerPath = layerPath
        OlStylePicker1.fieldList = GDAL.getFieldList(layerPath)


    End Sub

    Public Function getKeyText() As String
        Return Chr(34) & TextBox1.Text & Chr(34)
    End Function

    Private Sub OL3LayerStyleFeatureStyle_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Function save() As OL3LayerFeatureStyleSaveObject
        save = New OL3LayerFeatureStyleSaveObject
        save.styleSettings = OlStylePicker1.OLStyleSettings
        save.description = TextBox1.Text
    End Function

    Public Sub loadObj(ByVal saveObj As OL3LayerFeatureStyleSaveObject)
        OlStylePicker1.OLStyleSettings = saveObj.styleSettings
        OlStylePicker1.ChangeOLStylePickerdialog.styleSettings = saveObj.styleSettings
        TextBox1.Text = saveObj.description

        OlStylePicker1.refreshControl()
    End Sub

End Class

<Serializable()> _
Public Class OL3LayerFeatureStyleSaveObject
    Public styleSettings As StyleProperties
    Public description As String
End Class
