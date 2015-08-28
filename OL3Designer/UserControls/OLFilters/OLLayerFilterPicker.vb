Public Class OLLayerFilterPicker
    Public layerPath As String
    Public previousFilterPick As String = ""
    Public tempFilerbyUniqueValues As OLLayerFilterUniqueValues
    Public mapNumber As Integer
    Public LayerNumber As Integer

    Public Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        refreshFilterControl()
    End Sub

    Sub refreshFilterControl()


        If TreeView1.SelectedNode.Text <> previousFilterPick Then 'esures it is not fired onload events
            Select Case TreeView1.SelectedNode.Text
                Case "Filter by Unique Values"
                    Panel1.Controls.Clear()
                    tempFilerbyUniqueValues = New OLLayerFilterUniqueValues(layerPath)
                    tempFilerbyUniqueValues.layerPath = layerPath
                    Panel1.Controls.Add(tempFilerbyUniqueValues)

            End Select
        End If

        previousFilterPick = TreeView1.SelectedNode.Text
    End Sub
End Class
