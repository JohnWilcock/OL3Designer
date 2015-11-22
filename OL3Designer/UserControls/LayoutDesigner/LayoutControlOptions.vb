Public Class LayoutControlOptions

    Public OL3mapsObject As OL3Maps
    Dim FilterItems As New List(Of ControlItem)


    Sub populateAvailableControls()
        TreeView1.Nodes.Clear()

        'get olmaps object - done on click

        Dim nodeIndex As Integer = 0
        Dim subNodeIndex As Integer = 0
        Dim colNameIndex As Integer = 0
        Dim tempUniqueFilterControl As OLLayerFilterUniqueValues
        'cycle through each map
        For Each map As OL3LayerList In OL3mapsObject.mapList
            TreeView1.Nodes.Add(map.mapName)
            'cycle through each layer
            For Each mapLayer As OLLayer In map.DataGridView1.Rows
                TreeView1.Nodes(nodeIndex).Nodes.Add(mapLayer.layerName)

                'for each layer list the filter controlsd available
                Select Case mapLayer.OL3Edit.OlLayerFilterPicker1.Panel1.Controls(0).GetType
                    Case GetType(OLLayerFilterUniqueValues)
                        tempUniqueFilterControl = mapLayer.OL3Edit.OlLayerFilterPicker1.Panel1.Controls(0)
                        For v As Integer = 0 To tempUniqueFilterControl.CheckedListBox1.CheckedItems.Count - 1
                            TreeView1.Nodes(nodeIndex).Nodes(subNodeIndex).Nodes.Add(tempUniqueFilterControl.CheckedListBox1.CheckedItems(colNameIndex))
                            colNameIndex = colNameIndex + 1
                        Next
                        colNameIndex = 0
                End Select

                'list the style controls available
                If mapLayer.OL3Edit.OL3LayerStylePicker1.DataGridView1.Rows.Count > 1 Then
                    TreeView1.Nodes(nodeIndex).Nodes(subNodeIndex).Nodes.Add("Style Selector")
                End If

                subNodeIndex = subNodeIndex + 1
            Next
            subNodeIndex = 0
            TreeView1.Nodes(nodeIndex).Expand()
            nodeIndex = nodeIndex + 1
        Next
    End Sub

    Private Sub LayoutControlOptions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        populateAvailableControls()
        refreshFilters()
        expandAll()
    End Sub

    Sub expandAll()
        For t As Integer = 0 To TreeView1.Nodes.Count - 1
            TreeView1.Nodes(t).Expand()
            TreeView1.Nodes(t).ExpandAll()
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'check for valid node, only level 3 can be added
        Select Case TreeView1.SelectedNode.Level

            Case 2
                'add to list of controls
                If TreeView1.SelectedNode.Text <> "Style Selector" Then
                    addUniqueFilterLayer(TreeView1.SelectedNode.Parent, TreeView1.SelectedNode, TreeView1.SelectedNode.Parent.Parent.Index, TreeView1.SelectedNode.Parent.Index)
                Else
                    addStyleSelector(TreeView1.SelectedNode.Parent, TreeView1.SelectedNode, TreeView1.SelectedNode.Parent.Parent.Index, TreeView1.SelectedNode.Parent.Index)
                End If
        End Select





    End Sub

    Sub addStyleSelector(ByVal FilterLayerNode As TreeNode, ByVal FilterFieldNode As TreeNode, ByVal mapNumber As Integer, ByVal layerNumber As Integer)

        Dim theLayer As OLLayer = OL3mapsObject.mapList(mapNumber).DataGridView1.Rows(layerNumber)
        Dim newStyleItem As New ControlItem
        newStyleItem.LayerID = theLayer.layerID
        newStyleItem.layerName = theLayer.layerName
        newStyleItem.mapNumber = theLayer.mapNumber
        newStyleItem.label = FilterFieldNode.Text & ", " & theLayer.layerName & " (Map " & mapNumber + 1 & ")"
        newStyleItem.layerType = theLayer.OL3Edit.layerType
        newStyleItem.layerNumber = layerNumber
        newStyleItem.fieldName = FilterFieldNode.Text
        newStyleItem.controlType = "Style Selector"

        newStyleItem.StyleNames = New List(Of String)
        For k As Integer = 0 To theLayer.OL3Edit.OL3LayerStylePicker1.DataGridView1.Rows.Count - 1
            newStyleItem.StyleNames.Add(theLayer.OL3Edit.OL3LayerStylePicker1.DataGridView1.Rows(k).Cells(0).Value.ToString)
        Next

        'add to list
        FilterItems.Add(newStyleItem)

        'add to list in key
        TreeView2.Nodes.Add(newStyleItem.label)
    End Sub




    Sub addUniqueFilterLayer(ByVal FilterLayerNode As TreeNode, ByVal FilterFieldNode As TreeNode, ByVal mapNumber As Integer, ByVal layerNumber As Integer)

        Dim theLayer As OLLayer = OL3mapsObject.mapList(mapNumber).DataGridView1.Rows(layerNumber)
        Dim newFilterItem As New ControlItem
        newFilterItem.LayerID = theLayer.layerID
        newFilterItem.layerName = theLayer.layerName
        newFilterItem.mapNumber = theLayer.mapNumber
        newFilterItem.label = FilterFieldNode.Text & ", " & theLayer.layerName & " (Map " & mapNumber + 1 & ")"
        newFilterItem.layerType = theLayer.OL3Edit.layerType
        newFilterItem.layerNumber = layerNumber
        newFilterItem.fieldName = FilterFieldNode.Text
        newFilterItem.controlType = "Unique Filter"

        Select Case theLayer.OL3Edit.OlLayerFilterPicker1.Panel1.Controls(0).GetType
            Case GetType(OLLayerFilterUniqueValues)
                Dim tempFilterUniqueValues As OLLayerFilterUniqueValues
                tempFilterUniqueValues = theLayer.OL3Edit.OlLayerFilterPicker1.Panel1.Controls(0)
                If tempFilterUniqueValues.getListOfUniqueFilterRows(mapNumber, layerNumber, FilterFieldNode.Text) Is Nothing Then
                Else
                    newFilterItem.controlFields = convertUniqueFilterRowListToValuePairList(tempFilterUniqueValues.getListOfUniqueFilterRows(mapNumber, layerNumber, FilterFieldNode.Text))
                End If
        End Select



        'add to list
        FilterItems.Add(newFilterItem)

        'add to list in key
        TreeView2.Nodes.Add(newFilterItem.label)

    End Sub

    Private Sub TreeView2_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView2.AfterSelect
        If FilterItems(TreeView2.SelectedNode.Index).controlFields IsNot Nothing Then
            displayUniqueFieldsOfSelectedUniqueFilter()
        ElseIf FilterItems(TreeView2.SelectedNode.Index).StyleNames IsNot Nothing Then
            displayStyleNamelist()
        End If


    End Sub

    Sub displayStyleNamelist()
        'clear current items
        CheckedListBox1.Items.Clear()

        'get selected node
        Dim selectedNode As Integer = TreeView2.SelectedNode.Index
        Dim tempListOfValues As List(Of String) = FilterItems(selectedNode).StyleNames

        For s As Integer = 0 To tempListOfValues.Count - 1
            CheckedListBox1.Items.Add(tempListOfValues(s))
            CheckedListBox1.SetItemCheckState(CheckedListBox1.Items.Count - 1, 1)
        Next
    End Sub

    Sub displayUniqueFieldsOfSelectedUniqueFilter()
        'clear current items
        CheckedListBox1.Items.Clear()

        'get selected node
        Dim selectedNode As Integer = TreeView2.SelectedNode.Index
        Dim tempListOfValues As List(Of UniqueFilterValueCheckPair) = FilterItems(selectedNode).controlFields

        For s As Integer = 0 To tempListOfValues.Count - 1
            CheckedListBox1.Items.Add(tempListOfValues(s).value)
            CheckedListBox1.SetItemCheckState(CheckedListBox1.Items.Count - 1, 1)
        Next

    End Sub

    Function convertUniqueFilterRowListToValuePairList(ByVal rowList As List(Of UniqueFilterRow)) As List(Of UniqueFilterValueCheckPair)
        convertUniqueFilterRowListToValuePairList = New List(Of UniqueFilterValueCheckPair)

        For s As Integer = 0 To rowList.Count - 1

            Dim temp As New UniqueFilterValueCheckPair
            temp.checked = 1
            temp.value = rowList(s).Cells(2).Value.ToString

            convertUniqueFilterRowListToValuePairList.Add(temp)
        Next

    End Function


    Sub removeFilter(ByVal index As Integer)
        FilterItems.RemoveAt(index)
        TreeView2.Nodes(index).Remove()
        CheckedListBox1.Items.Clear()
    End Sub

    Sub refreshFilters()
        'for each item match id to layers in maps ... call on form load and key generation
        'if layer still exists update all the properties (i.e. layer name, map num, layer num)

        For filterItemCount As Integer = 0 To FilterItems.Count - 1
            If findFilterID(filterItemCount) = False Then
                'if item has been removed
                FilterItems.RemoveAt(filterItemCount)
            End If
        Next
    End Sub


    Function findFilterID(ByVal filterItemCount As Long) As Boolean
        For Each map As OL3LayerList In OL3mapsObject.mapList
            For Each mapLayer As OLLayer In map.DataGridView1.Rows
                If mapLayer.layerID = FilterItems(filterItemCount).LayerID Then

                    Return True
                End If
            Next
        Next
        Return False
    End Function

    Function getControlHTML() As String
        getControlHTML = ""
        Dim theLayer As OLLayer
        Dim tempListOfField As New List(Of String)
        Dim tempUniqueFilterControl As OLLayerFilterUniqueValues
        'for each itm in the FiltersItems list convert to html
        For m As Integer = 0 To FilterItems.Count - 1
            tempListOfField.Clear()
            theLayer = OL3mapsObject.mapList(FilterItems(m).mapNumber - 1).DataGridView1.Rows(FilterItems(m).layerNumber)
            Select Case FilterItems(m).controlType
                Case "Unique Filter"
                    tempUniqueFilterControl = theLayer.OL3Edit.OlLayerFilterPicker1.Panel1.Controls(0)
                    'get html aspect only and return it
                    tempListOfField.Add(FilterItems(m).fieldName)
                    getControlHTML = getControlHTML & tempUniqueFilterControl.getFilterHTML(FilterItems(m).mapNumber, FilterItems(m).layerNumber, tempListOfField)
                    '(other aspects are requested when layers are proccessed)
                Case "Style Selector"
                    getControlHTML = getControlHTML & "<select style='width:100%;height:35px;' id='styleSelectmap" & FilterItems(m).mapNumber & "_vectorLayer_" & FilterItems(m).layerNumber & "'>"
                    For a As Integer = 0 To FilterItems(m).StyleNames.Count - 1
                        getControlHTML = getControlHTML & "<option value='map" & FilterItems(m).mapNumber & "_vectorLayer_" & FilterItems(m).layerNumber & "_Style" & a & "_Function'>" & FilterItems(m).StyleNames(a) & "</option>"
                    Next
                    getControlHTML = getControlHTML & "</select>"
                    'now add change handler
                    getControlHTML = getControlHTML & "<script type='text/javascript'> document.getElementById('styleSelectmap" & FilterItems(m).mapNumber & "_vectorLayer_" & FilterItems(m).layerNumber & "').onchange = function(){ eval(map" & FilterItems(m).mapNumber & "_vectorLayer_" & FilterItems(m).layerNumber & "_SwitchLayerDeclaration( document.getElementById('styleSelectmap" & FilterItems(m).mapNumber & "_vectorLayer_" & FilterItems(m).layerNumber & "').selectedIndex)); if(mapStyleTypes['" & FilterItems(m).mapNumber & "']['" & FilterItems(m).layerNumber & "'][document.getElementById('styleSelectmap" & FilterItems(m).mapNumber & "_vectorLayer_" & FilterItems(m).layerNumber & "').selectedIndex] != 'Heatmap'){map" & FilterItems(m).mapNumber & "_vectorLayer_" & FilterItems(m).layerNumber & ".setStyle(eval(document.getElementById('styleSelectmap" & FilterItems(m).mapNumber & "_vectorLayer_" & FilterItems(m).layerNumber & "').value));refreshAllKeys();}else{refreshAllKeys();}  }; </script>"
            End Select
        Next


    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        removeControl()
    End Sub

    Sub removeControl()
        If TreeView2.SelectedNode Is Nothing Then Exit Sub
        FilterItems.RemoveAt(TreeView2.SelectedNode.Level)
        TreeView2.Nodes.Remove(TreeView2.SelectedNode)

        CheckedListBox1.Items.Clear()
    End Sub


    Sub refreshFiltersTreeview()
        CheckedListBox1.Items.Clear()

        TreeView2.Nodes.Clear()
        For d As Integer = 0 To FilterItems.Count - 1
            TreeView2.Nodes.Add(FilterItems(d).label)
        Next

    End Sub

    Public Function save() As List(Of ControlItem)
        save = FilterItems
       

    End Function

    Public Sub loadObj(ByVal saveObj As List(Of ControlItem))
        FilterItems = saveObj

        refreshFiltersTreeview()
    End Sub

End Class


<Serializable()> _
Public Class UniqueFilterValueCheckPair
    Public value As String
    Public checked As Boolean
End Class


<Serializable()> _
Public Class ControlItem
    'Public controlFields As List(Of UniqueFilterRow) 'change to string 
    Public controlFields As List(Of UniqueFilterValueCheckPair)
    Public StyleNames As List(Of String)
    Public fieldName As String
    Public label As String
    Public LayerID As Long
    Public layerName As String
    Public layerTitle As String
    Public layerType As String
    Public mapNumber As String
    Public layerNumber As Integer
    Public controlType As String
    Private _FilterPickerInKey As Integer


    Public Property FilterPickerInKey() As Boolean
        Get
            Return _FilterPickerInKey
        End Get
        Set(ByVal Value As Boolean)
            _FilterPickerInKey = Value
        End Set
    End Property

    Sub New()
        FilterPickerInKey = True
    End Sub
End Class