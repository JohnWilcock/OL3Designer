Public Class OL3MapOptions

    Public theParentLayerList As OL3LayerList
    Dim hasLoaded As Boolean = 0
    Dim firstCheck As Boolean = 0
    Public defaultExtentCoordinates As New OL3BBox
    Public mapProjectionWKT As String
    Public syncedMapID As String

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        'set default projection to web mecrator
        OL3Projections1.TextBox1.Text = "3857"
        OL3Projections1.TextBox2.Text = "+proj=merc +a=6378137 +b=6378137 +lat_ts=0.0 +lon_0=0.0 +x_0=0.0 +y_0=0 +k=1.0 +units=m +nadgrids=@null +wktext  +no_defs"
        mapProjectionWKT = "PROJCS[" & Chr(34) & "Google Maps Global Mercator" & Chr(34) & ",GEOGCS[" & Chr(34) & "WGS 84" & Chr(34) & ",DATUM[" & Chr(34) & "WGS_1984" & Chr(34) & ",SPHEROID[" & Chr(34) & "WGS 84" & Chr(34) & ",6378137,298.257223563,AUTHORITY[" & Chr(34) & "EPSG" & Chr(34) & "," & Chr(34) & "7030" & Chr(34) & "]],AUTHORITY[" & Chr(34) & "EPSG" & Chr(34) & "," & Chr(34) & "6326" & Chr(34) & "]],PRIMEM[" & Chr(34) & "Greenwich" & Chr(34) & ",0,AUTHORITY[" & Chr(34) & "EPSG" & Chr(34) & "," & Chr(34) & "8901" & Chr(34) & "]],UNIT[" & Chr(34) & "degree" & Chr(34) & ",0.01745329251994328,AUTHORITY[" & Chr(34) & "EPSG" & Chr(34) & "," & Chr(34) & "9122" & Chr(34) & "]],AUTHORITY[" & Chr(34) & "EPSG" & Chr(34) & "," & Chr(34) & "4326" & Chr(34) & "]],PROJECTION[" & Chr(34) & "Mercator_2SP" & Chr(34) & "],PARAMETER[" & Chr(34) & "standard_parallel_1" & Chr(34) & ",0],PARAMETER[" & Chr(34) & "latitude_of_origin" & Chr(34) & ",0],PARAMETER[" & Chr(34) & "central_meridian" & Chr(34) & ",0],PARAMETER[" & Chr(34) & "false_easting" & Chr(34) & ",0],PARAMETER[" & Chr(34) & "false_northing" & Chr(34) & ",0],UNIT[" & Chr(34) & "Meter" & Chr(34) & ",1],EXTENSION[" & Chr(34) & "PROJ4" & Chr(34) & "," & Chr(34) & "+proj=merc +a=6378137 +b=6378137 +lat_ts=0.0 +lon_0=0.0 +x_0=0.0 +y_0=0 +k=1.0 +units=m +nadgrids=@null +wktext  +no_defs" & Chr(34) & "],AUTHORITY[" & Chr(34) & "EPSG" & Chr(34) & "," & Chr(34) & "900913" & Chr(34) & "]]"
        ComboBox1.Items.Add("none")
    End Sub

    Public Function getSetMapExtentJS() As String

    End Function

    Public Function getMapProjectionDefinition() As String
        getMapProjectionDefinition = ""
        'getLayerProjectionDefinition = "Proj4js.defs['EPSG:" & Me.Cells.Item(0).RowIndex & "'] = " & Chr(34) & OL3Edit.OL3Projections1.TextBox2.Text & Chr(34) & Chr(10)
        getMapProjectionDefinition = "proj4.defs('USER:9999'," & Chr(34) & OL3Projections1.TextBox2.Text & Chr(34) & ");" & Chr(10)
    End Function

    Private Sub OL3MapOptions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        hasLoaded = 0
        firstCheck = 0

        Dim projConv As New ProjectionsAndTransformations

        'refresh list of layers in default extent windows
        'cycle through layers and check if layer is on for default extent
        Dim theLayer As OLLayer
        CheckedListBox1.Items.Clear()

        For i As Integer = 0 To theParentLayerList.DataGridView1.Rows.Count - 1
            theLayer = theParentLayerList.DataGridView1.Rows(i)

            'convert to same coords as map
            Dim layerTopLeftPoint As Double() = New Double() {theLayer.MinX, theLayer.MaxY}
            Dim layerBottomRightPoint As Double() = New Double() {theLayer.MaxX, theLayer.MinY}

            'what is the map coords ?
            layerTopLeftPoint = projConv.convertCoords(layerTopLeftPoint(0), layerTopLeftPoint(1), theLayer.layerProjWKT, mapProjectionWKT)
            layerBottomRightPoint = projConv.convertCoords(layerBottomRightPoint(0), layerBottomRightPoint(1), theLayer.layerProjWKT, mapProjectionWKT)


            CheckedListBox1.Items.Add(theLayer.layerName)
            If theLayer.useLayerForDefaultExtent Then
                CheckedListBox1.SetItemChecked(i, True)

                'get the extents for the coordinates option
                If firstCheck = 0 Then
                    defaultExtentCoordinates.XtopLeft.Value = layerTopLeftPoint(0) 'theLayer.MinX
                    defaultExtentCoordinates.YbottomRight.Value = layerBottomRightPoint(1) 'theLayer.MinY
                    defaultExtentCoordinates.YtopLeft.Value = layerTopLeftPoint(1) 'theLayer.MaxY
                    defaultExtentCoordinates.XbottomRight.Value = layerBottomRightPoint(0) 'theLayer.MaxX
                    firstCheck = 1
                Else
                    defaultExtentCoordinates.XtopLeft.Value = Math.Min(layerTopLeftPoint(0), defaultExtentCoordinates.XtopLeft.Value)
                    defaultExtentCoordinates.YbottomRight.Value = Math.Min(layerBottomRightPoint(1), defaultExtentCoordinates.YbottomRight.Value)
                    defaultExtentCoordinates.YtopLeft.Value = Math.Max(layerTopLeftPoint(1), defaultExtentCoordinates.YtopLeft.Value)
                    defaultExtentCoordinates.XbottomRight.Value = Math.Max(layerBottomRightPoint(0), defaultExtentCoordinates.XbottomRight.Value)
                End If

            Else
                CheckedListBox1.SetItemChecked(i, False)
            End If
        Next



        'create list of other maps - for map sync options
        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("none")
        For u As Integer = 0 To theParentLayerList.parentMapList.mapList.Count - 1
            If u <> theParentLayerList.mapNumber - 1 Then
                ComboBox1.Items.Add("Map " & u + 1)
            End If
        Next

        'does previously synced map still exist
        checkSyncedMap()


        hasLoaded = 1
    End Sub

    Public Sub checkSyncedMap()

        ComboBox1.SelectedIndex = 0
        For u As Integer = 0 To theParentLayerList.parentMapList.mapList.Count - 1
            If theParentLayerList.parentMapList.mapList(u).mapID = syncedMapID Then
                ComboBox1.SelectedIndex = u
            End If
        Next

        'hmmm need to check that no other map is syncing to it otherwise both are trying to sync to each other
    End Sub

    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As ItemCheckEventArgs) Handles CheckedListBox1.ItemCheck
        If hasLoaded Then
            updateLayersUsedForDefaultExtent(e.Index, e.NewValue)
        End If


    End Sub

    Sub updateLayersUsedForDefaultExtent(ByVal index As Integer, ByVal newValue As Boolean)
        Dim theLayer As OLLayer

        'update checked list box
        theLayer = theParentLayerList.DataGridView1.Rows(index)
        theLayer.useLayerForDefaultExtent = newValue

        'update coordinate bbox
        refreshDefaultExtents()

    End Sub

    Sub refreshDefaultExtents()

        Dim theLayer As OLLayer

        hasLoaded = 0
        firstCheck = 0

        Dim projConv As New ProjectionsAndTransformations

        For i As Integer = 0 To theParentLayerList.DataGridView1.Rows.Count - 1
            theLayer = theParentLayerList.DataGridView1.Rows(i)
            'convert to same coords as map
            Dim layerTopLeftPoint As Double() = New Double() {theLayer.MinX, theLayer.MaxY}
            Dim layerBottomRightPoint As Double() = New Double() {theLayer.MaxX, theLayer.MinY}

            'what is the map coords ?
            layerTopLeftPoint = projConv.convertCoords(layerTopLeftPoint(0), layerTopLeftPoint(1), theLayer.layerProjWKT, mapProjectionWKT)
            layerBottomRightPoint = projConv.convertCoords(layerBottomRightPoint(0), layerBottomRightPoint(1), theLayer.layerProjWKT, mapProjectionWKT)

            If theLayer.useLayerForDefaultExtent Then
                CheckedListBox1.SetItemChecked(i, True)

                'get the extents for the coordinates option
                If firstCheck = 0 Then
                    defaultExtentCoordinates.XtopLeft.Value = layerTopLeftPoint(0) 'theLayer.MinX
                    defaultExtentCoordinates.YbottomRight.Value = layerBottomRightPoint(1) 'theLayer.MinY
                    defaultExtentCoordinates.YtopLeft.Value = layerTopLeftPoint(1) 'theLayer.MaxY
                    defaultExtentCoordinates.XbottomRight.Value = layerBottomRightPoint(0) 'theLayer.MaxX
                    firstCheck = 1
                Else
                    defaultExtentCoordinates.XtopLeft.Value = Math.Min(layerTopLeftPoint(0), defaultExtentCoordinates.XtopLeft.Value)
                    defaultExtentCoordinates.YbottomRight.Value = Math.Min(layerBottomRightPoint(1), defaultExtentCoordinates.YbottomRight.Value)
                    defaultExtentCoordinates.YtopLeft.Value = Math.Max(layerTopLeftPoint(1), defaultExtentCoordinates.YtopLeft.Value)
                    defaultExtentCoordinates.XbottomRight.Value = Math.Max(layerBottomRightPoint(0), defaultExtentCoordinates.XbottomRight.Value)
                End If

            Else
                CheckedListBox1.SetItemChecked(i, False)
            End If
        Next

        hasLoaded = 1
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        switchCoordinateOrLayerView()
    End Sub

    Sub switchCoordinateOrLayerView()
        Panel1.Controls.Clear()

        If Button3.Text = "Coordinates" Then
            Button3.Text = "Layers"
            Panel1.Controls.Add(defaultExtentCoordinates)
            mapProjectionWKT = OL3Projections1.ChosenCoordSystemWKT
        Else
            Button3.Text = "Coordinates"
            Panel1.Controls.Add(CheckedListBox1)

        End If
    End Sub


    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        setSyncedMap()
    End Sub


    Sub setSyncedMap()
        If hasLoaded = True And ComboBox1.SelectedIndex > 0 Then
            syncedMapID = theParentLayerList.parentMapList.mapList(ComboBox1.SelectedIndex).mapID
        End If

        If hasLoaded = True And ComboBox1.SelectedIndex = 0 Then
            syncedMapID = "none"
        End If

    End Sub

 
End Class