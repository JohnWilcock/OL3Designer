Public Class OL3Basemaps

    Public parentMapOptions As OL3MapOptions
    Public hasLoaded As Boolean = False

    Public Function getBasemapJS(Optional ByVal outputPath As String = "", Optional ByVal mapNumber As Integer = 0, Optional ByVal outputName As String = "") As String
        Select Case TreeView1.SelectedNode.Text
            Case "OpenStreetMap"
                Return "new ol.layer.Tile({style:'Road',source: new ol.source.MapQuest({layer: 'osm'})});"

            Case "Image Files"
                Dim tiledImageControl As OL3BasemapTiledRaster = Panel1.Controls(0)


                'first tile the images
                tiledImageControl.createAllTileLevels(outputPath, mapNumber, outputName)

                'then return the JS
                Return tiledImageControl.getBasemapJS(outputPath, mapNumber, outputName)
            Case Else
                Return "'';"
        End Select

    End Function


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        TreeView1.Nodes(1).Expand()
        TreeView1.SelectedNode = TreeView1.Nodes(1).Nodes(0)


    End Sub


    Sub newBasemapSelection()

        Select Case TreeView1.SelectedNode.Text
            Case "OpenStreetMap"
                Panel1.Controls.Clear()
                If hasLoaded Then parentMapOptions.numberOfZoomLevels = 22

            Case "Image Files"
                If Panel1.Controls.Count > 0 Then
                    If TypeOf (Panel1.Controls(0)) Is OL3BasemapTiledRaster Then
                        Exit Sub
                    End If
                End If

                Panel1.Controls.Clear()
                Panel1.Controls.Add(New OL3BasemapTiledRaster(parentMapOptions.theParentLayerList.parentMapList))
                'set parent


            Case "None"
                Panel1.Controls.Clear()
                If hasLoaded Then parentMapOptions.numberOfZoomLevels = 22

        End Select

        If hasLoaded Then parentMapOptions.refreshMinMax()
    End Sub

    Private Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        newBasemapSelection()
    End Sub


    Public Function save() As OL3BasemapsSaveObject
        save = New OL3BasemapsSaveObject
        save.BasemapType = TreeView1.SelectedNode.Text


        Select Case save.BasemapType

            Case "Image Files"
                Dim ImgFiles As OL3BasemapTiledRaster = Panel1.Controls(0)
                save.basemapObject = New OL3BasemapsTiledRasterSaveObject(ImgFiles.ZoomLevels)

        End Select

    End Function

    Public Sub loadObj(ByVal saveObj As OL3BasemapsSaveObject)

        Select Case saveObj.BasemapType

            Case "Image Files"
                TreeView1.SelectedNode = TreeView1.Nodes(0).Nodes(0)

                Panel1.Controls.Clear()
                Dim ImgFiles As New OL3BasemapTiledRaster(parentMapOptions.theParentLayerList.parentMapList)

                Dim tr As OL3BasemapsTiledRasterSaveObject = saveObj.basemapObject
                ImgFiles.ZoomLevels = tr.AllZoomsLevels

                Panel1.Controls.Add(ImgFiles)

            Case "OpenStreetMap"
                TreeView1.SelectedNode = TreeView1.Nodes(1).Nodes(0)
        End Select

    End Sub


End Class


<Serializable()> _
Public Class OL3BasemapsSaveObject

    Public BasemapType As String
    Public basemapObject As Object

End Class