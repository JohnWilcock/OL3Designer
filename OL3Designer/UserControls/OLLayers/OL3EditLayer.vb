Public Class OL3EditLayer
    Sub New(ByVal layer As String, ByVal layerT As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        layerPath = layer
        layerType = layerT


        OL3LayerStylePicker1.LayerType = layerT
        OL3LayerStylePicker1.layerPath = layerPath
        OlLayerFilterPicker1.layerPath = layerPath

        'remove cluster option for none point layers
        If layerT <> "Point" Then
            OL3LayerStylePicker1.TreeView1.Nodes(3).Remove()

        End If

        'set a default layer style - individual feature style. must be done after layer type has been set
        Dim gdal As New GDALImport
        Dim TVArg As TreeViewEventArgs
        Dim object1 As Object = -1

        If layerT = "Point" And gdal.getFeatureCount(layerPath) > 1000 Then 'if a large point layer then auto cluster
            OL3LayerStylePicker1.TreeView1.SelectedNode = OL3LayerStylePicker1.TreeView1.Nodes(3).Nodes(0)
            'use dummy values to trigger the change handler
            TVArg = New TreeViewEventArgs(OL3LayerStylePicker1.TreeView1.Nodes(3).Nodes(0))
            OL3LayerStylePicker1.TreeView1_AfterSelect(object1, TVArg)
        Else
            OL3LayerStylePicker1.TreeView1.SelectedNode = OL3LayerStylePicker1.TreeView1.Nodes(0)
            'use dummy values to trigger the change handler
            TVArg = New TreeViewEventArgs(OL3LayerStylePicker1.TreeView1.Nodes(0))
            OL3LayerStylePicker1.TreeView1_AfterSelect(object1, TVArg)
        End If




        'set a default filter user control
        OlLayerFilterPicker1.TreeView1.SelectedNode = OlLayerFilterPicker1.TreeView1.Nodes(0)
        'use dummy values to trigger the change handler
        TVArg = New TreeViewEventArgs(OlLayerFilterPicker1.TreeView1.Nodes(0))
        OlLayerFilterPicker1.TreeView1_AfterSelect(object1, TVArg)

    End Sub

    Public layerPath As String
    Public layerType As String

    Private Sub OL3EditLayer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        OL3LayerStylePicker1.layerPath = layerPath
        OL3LayerPopupPicker1.layerPath = layerPath
        OlLayerFilterPicker1.layerPath = layerPath
        'OL3LayerStylePicker1.LayerType = layerType
    End Sub
End Class