Public Class OLLayerFilterPicker
    Public layerPath As String
    Public previousFilterPick As String = ""
    Public tempFilerbyUniqueValues As OLLayerFilterUniqueValues
    Public tempFilerbyNumericValues As OLLayerFilterNumericValues
    Public mapNumber As Integer
    Public LayerNumber As Integer

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        refreshFilterControl()
    End Sub

    Sub refreshFilterControl()


        If TreeView1.SelectedNode.Text <> previousFilterPick Then 'esures it is not fired onload events
            Select Case TreeView1.SelectedNode.Text
                Case "Filter by Unique Values"
                    Panel1.Controls.Clear()
                    If tempFilerbyUniqueValues Is Nothing Then
                        tempFilerbyUniqueValues = New OLLayerFilterUniqueValues(layerPath)
                        tempFilerbyUniqueValues.layerPath = layerPath
                    End If
                    Panel1.Controls.Add(tempFilerbyUniqueValues)

            End Select
        End If

        previousFilterPick = TreeView1.SelectedNode.Text
    End Sub


    Public Function save() As OL3FilterSaveObject
        save = New OL3FilterSaveObject
        save.filterUniqueValue = tempFilerbyUniqueValues.save
        'save.filterNumericValue = tempFilerbyNumericValues.save
    End Function

    Public Sub loadObj(ByVal saveObj As OL3FilterSaveObject)
        tempFilerbyUniqueValues.loadObj(saveObj.filterUniqueValue)

    End Sub

End Class




<Serializable()> _
Public Class OL3FilterSaveObject

    Public filterUniqueValue As OL3UniqueFilterSaveObject
    Public filterNumericValue As OL3NumericFilterSaveObject


End Class
