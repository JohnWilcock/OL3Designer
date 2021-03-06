﻿Public Class OL3LayerStyleCluster

    Public NumericRangesStyleList As New List(Of StyleProperties)
    Public theStylePicker As New OLStylePicker
    Public layerPath As String
    Public layerType As String


    Public statMin As Double
    Public statMax As Double
    Public statSum As Double



    Sub New(ByVal layerT As String, ByVal layerP As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        layerPath = layerP
        layerType = layerT
        OlStylePicker1.layerPath = layerPath
        OlStylePicker1.isCluster = True
        Dim gdal2 As New GDALImport
        OlStylePicker1.fieldList = gdal2.getFieldList(layerPath)
    End Sub

    Public Function getKeyText() As String
        getKeyText = ""

        getKeyText = Chr(34) & TextBox1.Text & Chr(34)
  


    End Function



    Public Function save() As OL3LayerClusterSaveObject
        save = New OL3LayerClusterSaveObject
        save.styleSettings = OlStylePicker1.OLStyleSettings
        save.description = TextBox1.Text
        save.clusterTolerance = NumericUpDown2.Value
    End Function

    Public Sub loadObj(ByVal saveObj As OL3LayerClusterSaveObject)
        OlStylePicker1.OLStyleSettings = saveObj.styleSettings
        OlStylePicker1.ChangeOLStylePickerdialog.styleSettings = saveObj.styleSettings
        TextBox1.Text = saveObj.description
        NumericUpDown2.Value = saveObj.clusterTolerance

        OlStylePicker1.refreshControl()
    End Sub

End Class

<Serializable()> _
Public Class OL3LayerClusterSaveObject
    Public styleSettings As StyleProperties
    Public clusterTolerance As Integer
    Public description As String
End Class





Public Class ClusterRow
    Inherits DataGridViewRow
    Public ClusterRangesStyle As OLStylePicker
    'Public uniqueStyle2 As StyleProperties
    Public ClusterRangesBitmap As Bitmap
    Public ClusterRangesValue As String
    Public ClusterRangesEndValue As String
    Public ClusterRangesLabel As String

    Sub New()
        ClusterRangesStyle = New OLStylePicker
        'uniqueStyle2 = New StyleProperties
    End Sub


End Class

