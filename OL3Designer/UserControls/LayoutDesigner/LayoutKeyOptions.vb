Imports System.ComponentModel

Public Class LayoutKeyOptions

    Public OL3mapsObject As OL3Maps
    Public keyItems As New List(Of keyItem)


    Private Sub LayoutKeyOptions_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        populateListOfKeyableLayers()
        refreshKey()
    End Sub


    Sub populateListOfKeyableLayers()
        TreeView1.Nodes.Clear()

        'get olmaps object - done on click

        Dim nodeIndex As Integer = 0
        'cycle through each map
        For Each map As OL3LayerList In OL3mapsObject.mapList
            TreeView1.Nodes.Add(map.mapName)
            'cycle through each layer
            For Each mapLayer As OLLayer In map.DataGridView1.Rows
                TreeView1.Nodes(nodeIndex).Nodes.Add(mapLayer.layerName)
            Next
            TreeView1.Nodes(nodeIndex).Expand()
            nodeIndex = nodeIndex + 1
        Next



    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        addKeyItem()
    End Sub

    Sub addKeyItem()
        If IsNothing(TreeView1.SelectedNode) Then Exit Sub
        If TreeView1.SelectedNode.Level = 0 Then
            addKeyMap(TreeView1.SelectedNode, TreeView1.SelectedNode.Index)
        Else
            addKeyLayer(TreeView1.SelectedNode, TreeView1.SelectedNode.Parent.Index, TreeView1.SelectedNode.Index)
        End If

    End Sub

    Sub addKeyMap(ByVal keyMapNode As TreeNode, ByVal mapNumber As Integer)
        Dim count As Integer = 0
        For Each Layer As TreeNode In keyMapNode.Nodes
            addKeyLayer(Layer, Layer.Parent.Index, count)
            count = count + 1
        Next
    End Sub

    Sub addKeyLayer(ByVal keyLayerNode As TreeNode, ByVal mapNumber As Integer, ByVal layerNumber As Integer)

        Dim theLayer As OLLayer = OL3mapsObject.mapList(mapNumber).DataGridView1.Rows(layerNumber)
        Dim newKeyItem As New keyItem
        newKeyItem.LayerID = theLayer.layerID
        newKeyItem.layerName = theLayer.layerName
        newKeyItem.mapNumber = theLayer.mapNumber
        newKeyItem.label = theLayer.layerName & " (Map " & mapNumber + 1 & ")"
        newKeyItem.layerType = theLayer.OL3Edit.layerType
        newKeyItem.layerNumber = layerNumber

        'add to list
        keyItems.Add(newKeyItem)

        'add to list in key
        ListBox1.Items.Add(newKeyItem.label)

    End Sub

    Sub refreshKey()
        'for each item match id to layers in maps ... call on form load and key generation
        'if layer still exists update all the properties (i.e. layer name, map num, layer num)
        Dim keyItemCount As Integer = 0
        Dim keyItemCountAll As Integer = keyItems.Count - 1
        For item As Integer = 0 To keyItemCountAll
            'if item has been removed
            If findKeyID(keyItemCount) = False Then
                keyItems.RemoveAt(item)
                ListBox1.Items.RemoveAt(item)
            End If
            keyItemCount = keyItemCount + 1
        Next
    End Sub

    Function findKeyID(ByVal keyItemCount As Long) As Boolean
        For Each map As OL3LayerList In OL3mapsObject.mapList
            For Each mapLayer As OLLayer In map.DataGridView1.Rows

                If keyItems.Count >= keyItemCount - 1 Then
                    If mapLayer.layerID = keyItems(keyItemCount).LayerID Then
                        'update layer info if names have changed etc
                        keyItems(keyItemCount).layerName = mapLayer.layerName
                        keyItems(keyItemCount).label = mapLayer.layerName & " (Map " & mapLayer.mapNumber & ")"
                        ListBox1.Items(keyItemCount) = keyItems(keyItemCount).label

                        Return True
                    End If
                End If
            Next
        Next
        Return False
    End Function



    Function getKeyJSLiterol(ByVal keyNumber As Integer) As String
        getKeyJSLiterol = ""
        'creates the javascript object and key initalasation term
        getKeyJSLiterol = "var key" & keyNumber & " = {" & Chr(10) 'does not matter what the key is called as long as matches the div
        getKeyJSLiterol = getKeyJSLiterol & "'keyLayers':["

        'the vectorLayers
        For x As Integer = 0 To keyItems.Count - 1
            getKeyJSLiterol = getKeyJSLiterol & "map" & keyItems(x).mapNumber & "_vectorLayer_" & keyItems(x).layerNumber & ","
        Next
        getKeyJSLiterol = getKeyJSLiterol.Substring(0, getKeyJSLiterol.Length - 1) & "]," & Chr(10)

        'the key options
        getKeyJSLiterol = getKeyJSLiterol & "'keyDiv':'key" & keyNumber & "'," & Chr(10)

        'key Styles
        getKeyJSLiterol = getKeyJSLiterol & "'keyTitle':'" & TextBox1.Text & "'," & Chr(10)
        getKeyJSLiterol = getKeyJSLiterol & "'keyPadding':'" & NumericUpDown1.Value & "'," & Chr(10)
        getKeyJSLiterol = getKeyJSLiterol & "'keyPatchSize':'" & NumericUpDown2.Value & "'," & Chr(10)

        'key text
        Dim keyText As String = ""
        Dim styleText As String = ""
        Dim StylePickersVisible As String = ""
        Dim controlPickersVisible As String = ""
        Dim HeatmapKeyOptions As String = ""
        Dim alayer As OLLayer
        For x As Integer = 0 To keyItems.Count - 1 'for each key layer
            alayer = OL3mapsObject.mapList(keyItems(x).mapNumber - 1).DataGridView1.Rows(keyItems(x).layerNumber)
            keyText = keyText & "," & alayer.OL3Edit.OL3LayerStylePicker1.getKeyText & ""
            styleText = styleText & "," & alayer.OL3Edit.OL3LayerStylePicker1.getStyleText & ""
            StylePickersVisible = StylePickersVisible & "," & Math.Abs(CInt(keyItems(x).stylePickerInKey))
            controlPickersVisible = controlPickersVisible & "," & Math.Abs(CInt(keyItems(x).filterPickerInKey))
            'HeatmapKeyOptions = HeatmapKeyOptions & "," & alayer.OL3Edit.OL3LayerStylePicker1.getHeatmapParameters
        Next
        keyText = keyText & " "
        styleText = styleText & " "
        StylePickersVisible = StylePickersVisible & " "
        getKeyJSLiterol = getKeyJSLiterol & "'keyDescriptions':[" & keyText.Substring(1) & "]," & Chr(10)
        getKeyJSLiterol = getKeyJSLiterol & "'styleDescriptions':[" & styleText.Substring(1) & "]," & Chr(10)
        getKeyJSLiterol = getKeyJSLiterol & "'stylePickerInKey':[" & StylePickersVisible.Substring(1) & "]," & Chr(10)
        getKeyJSLiterol = getKeyJSLiterol & "'controlPickerInKey':[" & controlPickersVisible.Substring(1) & "]" & Chr(10)
        'getKeyJSLiterol = getKeyJSLiterol & "'HeatmapKeyOptions':[" & HeatmapKeyOptions.Substring(1) & "]"

        getKeyJSLiterol = getKeyJSLiterol & "};" & Chr(10)

        'create key term
        getKeyJSLiterol = getKeyJSLiterol & "createKey(key" & keyNumber & ");" & Chr(10) & Chr(10)

    End Function



    'to create keys
    'when designer is run to generate html, each key is to be labeled (key 1, key2 etc...) in the order it is come accross, 
    'at the same time the refreshkey then getKeyJSLiterol function is run and added to a combined string of all key declarations



    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

    End Sub

    Sub keyItemSelected(sender As Object, e As EventArgs) Handles ListBox1.DoubleClick
        Dim pG As New PropertiesForm
        If ListBox1.SelectedIndex <> -1 Then
            pG.PropertyGrid1.SelectedObject = keyItems(ListBox1.SelectedIndex)
            pG.ShowDialog()
        End If


    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        If ListBox1.SelectedIndex = 0 Then Exit Sub
        Dim oldItem As keyItem = keyItems.Item(ListBox1.SelectedIndex)
        keyItems.RemoveAt(ListBox1.SelectedIndex)
        keyItems.Insert(ListBox1.SelectedIndex - 1, oldItem)

        refreshKey()
        ListBox1.SelectedIndex = ListBox1.SelectedIndex - 1
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        If ListBox1.SelectedIndex = ListBox1.Items.Count - 1 Then Exit Sub
        Dim oldItem As keyItem = keyItems.Item(ListBox1.SelectedIndex)
        keyItems.RemoveAt(ListBox1.SelectedIndex)
        keyItems.Insert(ListBox1.SelectedIndex + 1, oldItem)

        refreshKey()
        ListBox1.SelectedIndex = ListBox1.SelectedIndex + 1
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        If ListBox1.SelectedIndex = -1 Then Exit Sub
        keyItems.RemoveAt(ListBox1.SelectedIndex)
        ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)
        refreshKey()

        If ListBox1.Items.Count > 0 Then
            ListBox1.SelectedIndex = ListBox1.Items.Count - 1
        End If

    End Sub


    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        Dim pG As New PropertiesForm
        If ListBox1.SelectedIndex <> -1 Then
            pG.PropertyGrid1.SelectedObject = keyItems(ListBox1.SelectedIndex)
            pG.ShowDialog()
        End If
    End Sub
End Class

Public Class keyItem
    Public label As String
    Public LayerID As Long
    Public layerName As String
    Public layerTitle As String
    Public layerType As String
    Public mapNumber As String
    Public layerNumber As Integer
    Private _stylePickerInKey As Integer

    <CategoryAttribute("Dynamic Styles"), DescriptionAttribute("Select false to prevent a drop down box appearing in the key when multiple styles are present for a layer.  This does not affect style selecters elsewhere."), DefaultValueAttribute(True)> _
    Public Property stylePickerInKey() As Boolean
        Get
            Return _stylePickerInKey
        End Get
        Set(ByVal Value As Boolean)
            _stylePickerInKey = Value
        End Set
    End Property

    Private _filterPickerInKey As Integer
    <CategoryAttribute("Dynamic Filters"), DescriptionAttribute("Select false to prevent filter pickers appearing in the key when filters are set for a layer.  This does not affect filter selecters elsewhere."), DefaultValueAttribute(True)> _
    Public Property filterPickerInKey() As Boolean
        Get
            Return _filterPickerInKey
        End Get
        Set(ByVal Value As Boolean)
            _filterPickerInKey = Value
        End Set
    End Property

    Sub New()
        stylePickerInKey = True
        filterPickerInKey = True
    End Sub

End Class