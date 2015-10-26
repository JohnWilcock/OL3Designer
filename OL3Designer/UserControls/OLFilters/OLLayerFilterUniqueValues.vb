Public Class OLLayerFilterUniqueValues

    Public layerPath As String
    Public AllFilters As New List(Of List(Of UniqueFilterRow))
    Public typeTemplate As DataGridViewComboBoxColumn
    Public mapNumber As Integer
    Public layerNumber As Integer

    Private Sub OLLayerFilterUniqueValues_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Sub setTypeTemplate()
        typeTemplate = DataGridView1.Columns(0)
        typeTemplate.Items.Clear()
        typeTemplate.Items.Add("=")
    End Sub

    Public Sub New(layerP)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        layerPath = layerP

        Dim typeColumn As DataGridViewComboBoxColumn = DataGridView1.Columns(0)
        typeColumn.DefaultCellStyle.NullValue = "="

        Dim gdal As New GDALImport
        CheckedListBox1.Items.AddRange(gdal.getFieldList(layerPath).ToArray)

        'add all unique values to the lists and create data grid view rows
        Dim currentList As List(Of UniqueFilterRow)
        Dim currentUniqueValues As List(Of String)
        Dim tempDatagridviewComboCell As DataGridViewComboBoxCell
        For x As Integer = 0 To CheckedListBox1.Items.Count - 1
            currentList = New List(Of UniqueFilterRow)
            currentUniqueValues = gdal.getFieldValues(layerPath, CheckedListBox1.Items(x).ToString, True)
            For y As Integer = 0 To currentUniqueValues.Count - 1
                currentList.Add(New UniqueFilterRow)
                currentList.Item(currentList.Count - 1).Cells.Add(New DataGridViewComboBoxCell)
                currentList.Item(currentList.Count - 1).Cells.Add(New DataGridViewTextBoxCell)
                currentList.Item(currentList.Count - 1).Cells.Add(New DataGridViewTextBoxCell)
                currentList.Item(currentList.Count - 1).Cells.Add(New DataGridViewButtonCell)

                tempDatagridviewComboCell = New DataGridViewComboBoxCell
                tempDatagridviewComboCell.Items.AddRange({"="})

                currentList.Item(currentList.Count - 1).Cells(0) = tempDatagridviewComboCell
                currentList.Item(currentList.Count - 1).Cells(1).Value = currentUniqueValues(y)
                currentList.Item(currentList.Count - 1).Cells(2).Value = currentUniqueValues(y)

            Next

            AllFilters.Add(currentList)
        Next

    End Sub

    'places X in remove column button cell
    Private Sub removeRow_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles DataGridView1.CellPainting
        If e.ColumnIndex = 3 AndAlso e.RowIndex >= 0 Then
            e.Paint(e.CellBounds, DataGridViewPaintParts.All)

            Dim bmpFind As Bitmap = My.Resources.Offline_16xLG__2
            e.Graphics.DrawImage(bmpFind, e.CellBounds.Left + 2, e.CellBounds.Top + 2)
            e.Handled = True
        End If
    End Sub

    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckedListBox1.SelectedIndexChanged
        refreshFilterList(CheckedListBox1.SelectedIndex)

    End Sub

    Sub refreshFilterList(fieldIndex)
        Dim currentList As List(Of UniqueFilterRow) = AllFilters(fieldIndex)
        DataGridView1.Rows.Clear()
        For t As Integer = 0 To currentList.Count - 1
            DataGridView1.Rows.Add(currentList(t))
        Next

    End Sub

    Sub removeFromFilterList(ByVal fieldIndex As Integer, ByVal removeIndex As Integer)
        Dim currentList As List(Of UniqueFilterRow) = AllFilters(fieldIndex)
        currentList.RemoveAt(removeIndex)
    End Sub

    Function getFilterString() As String


        'creates onchange function -> to be place staight into JS ...anywhere
        Dim variableList As String = ""
        Dim changeFilterFunction As String = ""
        Dim ifStatementsSTART As String = ""
        Dim ifStatementsEND As String = ""

        Dim tempList As List(Of UniqueFilterRow)
        For x As Integer = 0 To CheckedListBox1.Items.Count - 1
            tempList = AllFilters(x)
            If CheckedListBox1.GetItemChecked(x) Then
                '// create variables for each filter (true/false for on/off)
                variableList = variableList & "var map" & mapNumber + 1 & "_vectorLayer_" & layerNumber & "_Filter" & x & " = false;" & Chr(10)
                variableList = variableList & "var map" & mapNumber + 1 & "_vectorLayer_" & layerNumber & "_FilterCondition" & x & " = '';" & Chr(10) & Chr(10)

                '// create change filter function (inside function only, put start and end off afterwards)
                changeFilterFunction = changeFilterFunction & "function changeFilter_map_" & mapNumber + 1 & "_layer_" & layerNumber & "_Filter_" & x & "(){" & Chr(10)
                '//     Get the filter value from the drop down - if "Any" then set filter to false, else set to true
                changeFilterFunction = changeFilterFunction & "if(document.getElementById('map_" & mapNumber + 1 & "_layer_" & layerNumber & "_Filter_" & x & "').options[document.getElementById('map_" & mapNumber + 1 & "_layer_" & layerNumber & "_Filter_" & x & "').selectedIndex].value != 'Any'){" & Chr(10)
                '//     set filter on
                changeFilterFunction = changeFilterFunction & "map" & mapNumber + 1 & "_vectorLayer_" & layerNumber & "_Filter" & x & " = true;" & Chr(10)
                '//     set the filterX condition value and condition type
                changeFilterFunction = changeFilterFunction & "map" & mapNumber + 1 & "_vectorLayer_" & layerNumber & "_FilterCondition" & x & " = document.getElementById('map_" & mapNumber + 1 & "_layer_" & layerNumber & "_Filter_" & x & "').options[document.getElementById('map_" & mapNumber + 1 & "_layer_" & layerNumber & "_Filter_" & x & "').selectedIndex].value;" & Chr(10)
                '//     trigger setSource to reset all features
                changeFilterFunction = changeFilterFunction & "map" & mapNumber + 1 & "_vectorSource_" & layerNumber & "_SetSource(); //forces styles to be re-applied " & Chr(10)
                '// call second function to test features against conditions
                changeFilterFunction = changeFilterFunction & "changeFilter_map_" & mapNumber + 1 & "_layer_" & layerNumber & "_Filters();" & Chr(10)
                changeFilterFunction = changeFilterFunction & "} else {" & Chr(10)
                '//     set filter off if 'any' selected
                changeFilterFunction = changeFilterFunction & "map" & mapNumber + 1 & "_vectorLayer_" & layerNumber & "_Filter" & x & " = false;" & Chr(10)
                changeFilterFunction = changeFilterFunction & "map" & mapNumber + 1 & "_vectorSource_" & layerNumber & "_SetSource(); //forces styles to be re-applied " & Chr(10)
                changeFilterFunction = changeFilterFunction & "} " & Chr(10)
                changeFilterFunction = changeFilterFunction & "}" & Chr(10) & Chr(10)


                'if requested as a javascript variable to place in auto generated key
                changeFilterFunction = changeFilterFunction & "var map" & mapNumber + 1 & "_vectorLayer_" & layerNumber & "_controlHTML = " & Chr(34) & getFilterHTML(mapNumber + 1, layerNumber, New List(Of String), True).Replace(Chr(10), "") & Chr(34) & ";"
                'End If
            End If
        Next
        Return variableList & changeFilterFunction
    End Function

    Function getFilterIfStringForStyleFunction() As String
        'creates filter if string to be placed at start of style function to remove feature which don't meet conditions
        Dim variableList As String = ""
        Dim changeFilterFunction As String = ""
        Dim ifStatementsSTART As String = ""
        Dim ifStatementsEND As String = ""

        Dim tempList As List(Of UniqueFilterRow)
        For x As Integer = 0 To CheckedListBox1.Items.Count - 1
            tempList = AllFilters(x)
            If CheckedListBox1.GetItemChecked(x) Then
                '// create filter "If" statements which apply the filter within the style function
                ifStatementsSTART = ifStatementsSTART & " if (feature.get('" & CheckedListBox1.Items(x).ToString & "') == map" & mapNumber + 1 & "_vectorLayer_" & layerNumber & "_FilterCondition" & x & " || map" & mapNumber + 1 & "_vectorLayer_" & layerNumber & "_Filter" & x & " == false ) {" & Chr(10)

                ifStatementsEND = ifStatementsEND & "} else {map" & mapNumber + 1 & "_vectorSource_" & layerNumber & ".removeFeature(feature)}" & Chr(10)
            End If
        Next


        '//combine start of "If" statments and end of them
        ifStatementsSTART = ifStatementsSTART & ifStatementsEND & Chr(10) & Chr(10)

        Return ifStatementsSTART
    End Function


    Public Function getListOfUniqueFilterRows(ByVal mapNumber As Integer, ByVal layerNumber As Integer, fieldName As String) As List(Of UniqueFilterRow)
        Dim variableList As String = ""
        Dim changeFilterFunction As String = ""
        Dim ifStatementsSTART As String = ""
        Dim ifStatementsEND As String = ""

        Dim tempList As List(Of UniqueFilterRow)
        For x As Integer = 0 To CheckedListBox1.Items.Count
            tempList = AllFilters(x)
            If CheckedListBox1.Items(x) = fieldName Then
                Return tempList
            End If
        Next


    End Function


    Function getFilterHTML(ByVal mapNumber As Integer, ByVal LayerNumber As Integer, ByVal fieldNames As List(Of String), Optional allInLayer As Boolean = False) As String
        'creates html for controls
        getFilterHTML = ""

        Dim tempList As List(Of UniqueFilterRow)
        Dim tempRow As UniqueFilterRow
        For x As Integer = 0 To CheckedListBox1.Items.Count - 1
            tempList = AllFilters(x)
            If CheckedListBox1.GetItemChecked(x) Then
                If isInFieldList(fieldNames, CheckedListBox1.Items(x)) Or allInLayer Then
                    getFilterHTML = getFilterHTML & "<select id='map_" & mapNumber & "_layer_" & LayerNumber & "_Filter_" & x & "' onclick='changeFilter_map_" & mapNumber & "_layer_" & LayerNumber & "_Filter_" & x & "()'  onkeyup='changeFilter_map_" & mapNumber & "_layer_" & LayerNumber & "_Filter_" & x & "()'>" & Chr(10)
                    getFilterHTML = getFilterHTML & "<option value='Any'>Any</option>" & Chr(10)
                    For y As Integer = 0 To tempList.Count - 1
                        'construct html combobox for all filters for this layer(hmm... perhaps options to choose which filters)
                        tempRow = tempList(y)
                        getFilterHTML = getFilterHTML & "<option value='" & tempRow.Cells(1).Value.ToString & "'>" & tempRow.Cells(2).Value.ToString & "</option>" & Chr(10)

                    Next
                    getFilterHTML = getFilterHTML & "</select><Br>" & Chr(10)
                End If
            End If
        Next
    End Function


    Function isInFieldList(ByVal fieldNames As List(Of String), ByVal theFieldName As String) As Boolean
        For d As Integer = 0 To fieldNames.Count - 1
            If theFieldName = fieldNames(d) Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        handleFilterRowClick(e.RowIndex, e.ColumnIndex)
    End Sub

    Sub handleFilterRowClick(ByVal rowIndex As Integer, ByVal colIndex As Integer)
        Select Case colIndex
            Case 3 'remove
                DataGridView1.Rows.RemoveAt(rowIndex)
                removeFromFilterList(CheckedListBox1.SelectedIndex, rowIndex)
        End Select
    End Sub


    Public Function save() As OL3UniqueFilterSaveObject
        save = New OL3UniqueFilterSaveObject
        save.layerPath = layerPath

        For t As Integer = 0 To AllFilters.Count - 1
            save.AllFilters.Add(New List(Of OL3UniqueFilterValueSaveObject))

            'if filter is selected then add true to list of fields
            If CheckedListBox1.GetItemChecked(t) Then
                save.activeFilters.Add(True)
            Else
                save.activeFilters.Add(False)
            End If

            For m As Integer = 0 To AllFilters(t).Count - 1
                save.AllFilters(t).Add(New OL3UniqueFilterValueSaveObject)

                save.AllFilters(t)(m).filterType = AllFilters(t)(m).Cells(0).Value
                save.AllFilters(t)(m).filterValue = AllFilters(t)(m).Cells(1).Value
                save.AllFilters(t)(m).filterLabel = AllFilters(t)(m).Cells(2).Value
            Next
        Next

    End Function

    Public Sub loadObj(ByVal saveObj As OL3UniqueFilterSaveObject)
        AllFilters.Clear()
        Dim tempDatagridviewComboCell As DataGridViewComboBoxCell

        For t As Integer = 0 To saveObj.AllFilters.Count - 1
            AllFilters.Add(New List(Of UniqueFilterRow))

            'load checked filters
            CheckedListBox1.SetItemChecked(t, saveObj.activeFilters(t))

            'load filter values
            For m As Integer = 0 To saveObj.AllFilters(t).Count - 1
                AllFilters(t).Add(New UniqueFilterRow)

                AllFilters(t)(m).Cells.Add(New DataGridViewComboBoxCell)
                AllFilters(t)(m).Cells.Add(New DataGridViewTextBoxCell)
                AllFilters(t)(m).Cells.Add(New DataGridViewTextBoxCell)
                AllFilters(t)(m).Cells.Add(New DataGridViewButtonCell)

                tempDatagridviewComboCell = New DataGridViewComboBoxCell
                tempDatagridviewComboCell.Items.AddRange({"="})

                AllFilters(t)(m).Cells(0) = tempDatagridviewComboCell
                AllFilters(t)(m).Cells(1).Value = saveObj.AllFilters(t)(m).filterValue
                AllFilters(t)(m).Cells(2).Value = saveObj.AllFilters(t)(m).filterLabel

            Next
        Next



    End Sub
End Class

Public Class UniqueFilterRow
    Inherits DataGridViewRow
End Class

<Serializable()> _
Public Class OL3UniqueFilterValueSaveObject
    Public filterValue As String
    Public filterLabel As String
    Public filterType As String
End Class


<Serializable()> _
Public Class OL3UniqueFilterSaveObject
    Public activeFilters As New List(Of Boolean)
    Public AllFilters As New List(Of List(Of OL3UniqueFilterValueSaveObject))
    Public layerPath As String
End Class
