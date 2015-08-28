Public Class OL3LayerPopupPicker

    Public layerPath As String
    Dim currentPopupRow As popupRow
    Dim tempOL3LayerPopupDesigner As OL3PopupDesignerSimple
    Dim previousPopupPick As String = ""

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        TreeView1.SelectedNode = TreeView1.TopNode


    End Sub


    'Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
    Private Sub TreeView1_AfterSelect() Handles TreeView1.AfterSelect

        popupTypeSelected()
    End Sub


  
    Sub popupTypeSelected()
        If TreeView1.SelectedNode.Text <> previousPopupPick Then 'esures it is not fired onload events
            Select Case TreeView1.SelectedNode.Text
                Case "Single Popup Style"
                    Panel1.Controls.Clear()
                    tempOL3LayerPopupDesigner = New OL3PopupDesignerSimple
                    tempOL3LayerPopupDesigner.layerPath = layerPath
                    Panel1.Controls.Add(tempOL3LayerPopupDesigner)

            End Select
        End If

        previousPopupPick = TreeView1.SelectedNode.Text
    End Sub




End Class


Class popupRow
    Inherits DataGridViewRow
    Public PopupType As String
    Public PopupControl As UserControl
    'Public layerPath As String

End Class