Public Class OL3MapOptions

    Public theParentLayerList As OL3LayerList
    Dim hasLoaded As Boolean = 0
    Dim firstCheck As Boolean = 0
    Public defaultExtentCoordinates As New OL3BBox
    Public mapProjectionWKT As String
    Public syncedMapID As String
    Public syncEventActive As Boolean = True
    Public numberOfZoomLevels As Integer = 22

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        'set default projection to web mecrator
        OL3Projections1.TextBox1.Text = "3857"
        OL3Projections1.TextBox2.Text = "+proj=merc +a=6378137 +b=6378137 +lat_ts=0.0 +lon_0=0.0 +x_0=0.0 +y_0=0 +k=1.0 +units=m +nadgrids=@null +wktext  +no_defs"
        mapProjectionWKT = "PROJCS[" & Chr(34) & "Google Maps Global Mercator" & Chr(34) & ",GEOGCS[" & Chr(34) & "WGS 84" & Chr(34) & ",DATUM[" & Chr(34) & "WGS_1984" & Chr(34) & ",SPHEROID[" & Chr(34) & "WGS 84" & Chr(34) & ",6378137,298.257223563,AUTHORITY[" & Chr(34) & "EPSG" & Chr(34) & "," & Chr(34) & "7030" & Chr(34) & "]],AUTHORITY[" & Chr(34) & "EPSG" & Chr(34) & "," & Chr(34) & "6326" & Chr(34) & "]],PRIMEM[" & Chr(34) & "Greenwich" & Chr(34) & ",0,AUTHORITY[" & Chr(34) & "EPSG" & Chr(34) & "," & Chr(34) & "8901" & Chr(34) & "]],UNIT[" & Chr(34) & "degree" & Chr(34) & ",0.01745329251994328,AUTHORITY[" & Chr(34) & "EPSG" & Chr(34) & "," & Chr(34) & "9122" & Chr(34) & "]],AUTHORITY[" & Chr(34) & "EPSG" & Chr(34) & "," & Chr(34) & "4326" & Chr(34) & "]],PROJECTION[" & Chr(34) & "Mercator_2SP" & Chr(34) & "],PARAMETER[" & Chr(34) & "standard_parallel_1" & Chr(34) & ",0],PARAMETER[" & Chr(34) & "latitude_of_origin" & Chr(34) & ",0],PARAMETER[" & Chr(34) & "central_meridian" & Chr(34) & ",0],PARAMETER[" & Chr(34) & "false_easting" & Chr(34) & ",0],PARAMETER[" & Chr(34) & "false_northing" & Chr(34) & ",0],UNIT[" & Chr(34) & "Meter" & Chr(34) & ",1],EXTENSION[" & Chr(34) & "PROJ4" & Chr(34) & "," & Chr(34) & "+proj=merc +a=6378137 +b=6378137 +lat_ts=0.0 +lon_0=0.0 +x_0=0.0 +y_0=0 +k=1.0 +units=m +nadgrids=@null +wktext  +no_defs" & Chr(34) & "],AUTHORITY[" & Chr(34) & "EPSG" & Chr(34) & "," & Chr(34) & "900913" & Chr(34) & "]]"
        OL3Projections1.ChosenCoordSystemWKT = "PROJCS[" & Chr(34) & "Google Maps Global Mercator" & Chr(34) & ",GEOGCS[" & Chr(34) & "WGS 84" & Chr(34) & ",DATUM[" & Chr(34) & "WGS_1984" & Chr(34) & ",SPHEROID[" & Chr(34) & "WGS 84" & Chr(34) & ",6378137,298.257223563,AUTHORITY[" & Chr(34) & "EPSG" & Chr(34) & "," & Chr(34) & "7030" & Chr(34) & "]],AUTHORITY[" & Chr(34) & "EPSG" & Chr(34) & "," & Chr(34) & "6326" & Chr(34) & "]],PRIMEM[" & Chr(34) & "Greenwich" & Chr(34) & ",0,AUTHORITY[" & Chr(34) & "EPSG" & Chr(34) & "," & Chr(34) & "8901" & Chr(34) & "]],UNIT[" & Chr(34) & "degree" & Chr(34) & ",0.01745329251994328,AUTHORITY[" & Chr(34) & "EPSG" & Chr(34) & "," & Chr(34) & "9122" & Chr(34) & "]],AUTHORITY[" & Chr(34) & "EPSG" & Chr(34) & "," & Chr(34) & "4326" & Chr(34) & "]],PROJECTION[" & Chr(34) & "Mercator_2SP" & Chr(34) & "],PARAMETER[" & Chr(34) & "standard_parallel_1" & Chr(34) & ",0],PARAMETER[" & Chr(34) & "latitude_of_origin" & Chr(34) & ",0],PARAMETER[" & Chr(34) & "central_meridian" & Chr(34) & ",0],PARAMETER[" & Chr(34) & "false_easting" & Chr(34) & ",0],PARAMETER[" & Chr(34) & "false_northing" & Chr(34) & ",0],UNIT[" & Chr(34) & "Meter" & Chr(34) & ",1],EXTENSION[" & Chr(34) & "PROJ4" & Chr(34) & "," & Chr(34) & "+proj=merc +a=6378137 +b=6378137 +lat_ts=0.0 +lon_0=0.0 +x_0=0.0 +y_0=0 +k=1.0 +units=m +nadgrids=@null +wktext  +no_defs" & Chr(34) & "],AUTHORITY[" & Chr(34) & "EPSG" & Chr(34) & "," & Chr(34) & "900913" & Chr(34) & "]]"
        OL3Projections1.ChosenCoordSystemEPSG = "3857"

        ComboBox1.Items.Add("none")

        OL3Basemaps1.parentMapOptions = Me
        OL3Basemaps1.hasLoaded = True

    End Sub

    Public Function getMapProjectionDefinition() As String
        getMapProjectionDefinition = ""
        'getLayerProjectionDefinition = "Proj4js.defs['EPSG:" & Me.Cells.Item(0).RowIndex & "'] = " & Chr(34) & OL3Edit.OL3Projections1.TextBox2.Text & Chr(34) & Chr(10)
        getMapProjectionDefinition = "proj4.defs('USER:" & theParentLayerList.mapNumber & "999'," & Chr(34) & OL3Projections1.TextBox2.Text & Chr(34) & ");" & Chr(10)
    End Function

    Private Sub OL3MapOptions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        hasLoaded = 0
        firstCheck = 0

        Dim projConv As New ProjectionsAndTransformations

        'refresh list of layers in default extent windows
        'cycle through layers and check if layer is on for default extent
        Dim theLayer As OLLayer
        CheckedListBox1.Items.Clear()
        CheckedListBox2.Items.Clear()

        If OL3Projections1.ChosenCoordSystemWKT <> Nothing Then
            mapProjectionWKT = OL3Projections1.ChosenCoordSystemWKT
        End If

        'set list of layers in checkboxes - checkboxlist 2 is dependant on this
        For i As Integer = 0 To theParentLayerList.DataGridView1.Rows.Count - 1
            theLayer = theParentLayerList.DataGridView1.Rows(i)
            CheckedListBox1.Items.Add(theLayer.layerName)
            CheckedListBox2.Items.Add(theLayer.layerName)
        Next

        'only refesh coords if coord view NOT selected as it is not to be refeshed if mannualy altered
        If Button3.Text = "Coordinates" Then

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
        End If


        'create list of other maps - for map sync options
        'only list maps with lower numbers to avoid undeclared vars
        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("none")
        Dim numMaps As Integer = theParentLayerList.parentMapList.mapList.Count
        For u As Integer = 0 To theParentLayerList.parentMapList.mapList.Count - 1
            If u < theParentLayerList.mapNumber - 1 Then
                ComboBox1.Items.Add("Map " & u + 1)
            End If
        Next

        'does previously synced map still exist
        checkSyncedMap()

        'set min/max possible zoom levels in combo boxes
        refreshMinMax()

        'check restricted extent layers are still present
        refreshCheckedListBox2()

        'if tms 'copy from other map' is active - check map still exists
        checkTMSMapCopy()


        hasLoaded = 1
    End Sub

    Sub checkTMSMapCopy()
        Dim TR As OL3BasemapTiledRaster
        If OL3Basemaps1.Panel1.Controls.Count > 0 Then
            If TypeOf OL3Basemaps1.Panel1.Controls(0) Is OL3BasemapTiledRaster Then
                TR = OL3Basemaps1.Panel1.Controls(0)
                TR.checkCopyFromMap()
            End If
        End If
    End Sub

    Sub refreshMinMax()
        'set min/max possible zoom levels in combo boxes
        Dim selectedMin As Integer
        If ComboBox2.Items.Count > 0 Then
            selectedMin = CInt(ComboBox2.Text)
        End If

        Dim selectedMax As Integer
        If ComboBox2.Items.Count > 0 Then
            selectedMax = CInt(ComboBox3.Text)
        End If

        ComboBox2.Items.Clear()
        ComboBox3.Items.Clear()
        For p As Integer = 1 To numberOfZoomLevels
            ComboBox2.Items.Add(p)
            ComboBox3.Items.Add(p)
        Next

        'put back original numbers if still valid
        If selectedMin > 0 Then
            If selectedMin <= numberOfZoomLevels Then
                ComboBox2.SelectedIndex = selectedMin - 1
            Else
                ComboBox2.SelectedIndex = 0
            End If
        Else
            ComboBox2.SelectedIndex = 0
        End If

        If selectedMin > 0 Then
            If selectedMax <= numberOfZoomLevels Then
                ComboBox3.SelectedIndex = selectedMax - 1
            Else
                ComboBox3.SelectedIndex = numberOfZoomLevels - 1
            End If
        Else
            ComboBox3.SelectedIndex = numberOfZoomLevels - 1
        End If
    End Sub

    Public Sub checkSyncedMap()
        syncEventActive = False
        ComboBox1.SelectedIndex = 0
        For u As Integer = 0 To theParentLayerList.parentMapList.mapList.Count - 1
            If theParentLayerList.parentMapList.mapList(u).mapID = syncedMapID Then
                ComboBox1.SelectedIndex = u
            End If
        Next
        syncEventActive = True
        'hmmm need to check that no other map is syncing to it otherwise both are trying to sync to each other - fixed by only allowing syncs to lower numbers
    End Sub

    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As ItemCheckEventArgs) Handles CheckedListBox1.ItemCheck, CheckedListBox1.SelectedIndexChanged
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

        If OL3Projections1.ChosenCoordSystemWKT <> Nothing Then
            mapProjectionWKT = OL3Projections1.ChosenCoordSystemWKT
        End If

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
            mapProjectionWKT = OL3Projections1.ChosenCoordSystemWKT
            refreshDefaultExtents()
            Panel1.Controls.Add(defaultExtentCoordinates)

        Else
            Button3.Text = "Coordinates"
            Panel1.Controls.Add(CheckedListBox1)

        End If
    End Sub


    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If syncEventActive Then
            setSyncedMap()
        End If

    End Sub


    Sub setSyncedMap()
        If hasLoaded = True And ComboBox1.SelectedIndex > 0 Then
            syncedMapID = theParentLayerList.parentMapList.mapList(ComboBox1.SelectedIndex).mapID
        End If

        If hasLoaded = True And ComboBox1.SelectedIndex = 0 Then
            syncedMapID = "none"
        End If

    End Sub

    Function getExtentString() As String
        getExtentString = ""

        If Button3.Text = "Coordinates" Then 'if a choice of layers
            If CheckedListBox1.CheckedItems.Count = CheckedListBox1.Items.Count Then
                'all layers selected 
                getExtentString = "setMaxExtent(map" & theParentLayerList.mapNumber & ");"
            Else
                'subset of layers selected
                getExtentString = "map" & theParentLayerList.mapNumber
                For t As Integer = 0 To CheckedListBox1.Items.Count - 1
                    If CheckedListBox1.GetItemChecked(t) Then
                        getExtentString = getExtentString & ", map" & theParentLayerList.mapNumber & "_vectorLayer_" & t
                    End If
                Next
                getExtentString = "setMaxExtentByLayer(" & getExtentString & ");"
            End If



        Else ' if coordinates
            getExtentString = "map" & theParentLayerList.mapNumber
            getExtentString = getExtentString & ", [" & defaultExtentCoordinates.XtopLeft.Value & ", " & defaultExtentCoordinates.YbottomRight.Value & ", " & defaultExtentCoordinates.XbottomRight.Value & ", " & defaultExtentCoordinates.YtopLeft.Value & "]"
            getExtentString = "setMaxExtentCoords(" & getExtentString & ");"
        End If

    End Function

    Function getRestrictedExtentAndZoomString() As String
        Dim tempString As String = ""
        getRestrictedExtentAndZoomString = ""
        getRestrictedExtentAndZoomString = ",minZoom:" & ComboBox2.Text & ", " & "maxZoom:" & ComboBox3.Text

        If CheckedListBox2.CheckedItems.Count > 0 Then
            Dim theLayer As OLLayer
            For s As Integer = 0 To theParentLayerList.DataGridView1.Rows.Count - 1
                theLayer = theParentLayerList.DataGridView1.Rows(s)
                If theLayer.useLayerForRestrictedExtent Then
                    tempString = tempString & ", map" & theParentLayerList.mapNumber & "_vectorLayer_" & s
                End If
            Next

            getRestrictedExtentAndZoomString = getRestrictedExtentAndZoomString & ", " & "extent: getMaxExtentByLayerAndMargin([" & tempString.Substring(1) & "], " & 1 + (NumericUpDown1.Value / 100) & ")"
        End If

    End Function


    Private Sub checklistbox2CheckChanged(sender As Object, e As ItemCheckEventArgs) Handles CheckedListBox2.ItemCheck
        Dim theLayer As OLLayer


        theLayer = theParentLayerList.DataGridView1.Rows(e.Index)
        If CheckedListBox2.GetItemChecked(e.Index) Then
            theLayer.useLayerForRestrictedExtent = False
        Else
            theLayer.useLayerForRestrictedExtent = True
        End If

    End Sub


    Sub resetCheckedListBoxs()
        Dim theLayer As OLLayer
        CheckedListBox1.Items.Clear()
        CheckedListBox2.Items.Clear()


        'set list of layers in checkboxes - checkboxlist 2 is dependant on this
        For i As Integer = 0 To theParentLayerList.DataGridView1.Rows.Count - 1
            theLayer = theParentLayerList.DataGridView1.Rows(i)
            CheckedListBox1.Items.Add(theLayer.layerName)
            CheckedListBox2.Items.Add(theLayer.layerName)
        Next
    End Sub

    Sub refreshCheckedListBox2()
        Dim theLayer As OLLayer

        'add all layers selected back if still in layer list

        For y As Integer = 0 To theParentLayerList.DataGridView1.Rows.Count - 1
            theLayer = theParentLayerList.DataGridView1.Rows(y)


            If theLayer.useLayerForRestrictedExtent Then
                CheckedListBox2.SetItemChecked(y, True)
            Else
                CheckedListBox2.SetItemChecked(y, False)
            End If

        Next


    End Sub

    Public Function save() As OL3MapOptionsSaveObject
        save = New OL3MapOptionsSaveObject

        save.mapProjection = OL3Projections1.save
        save.basemaps = OL3Basemaps1.save

        'default extent
        If Button3.Text = "Coordinates" Then
            save.ExtentsDefaultExtentType = "Layers"
            For f As Integer = 0 To CheckedListBox1.Items.Count - 1
                If CheckedListBox1.GetItemChecked(f) Then
                    save.ExtentsDefaultExtentSelectedLayers.Add(True)
                Else
                    save.ExtentsDefaultExtentSelectedLayers.Add(False)
                End If

            Next
        Else
            save.ExtentsDefaultExtentType = "Coordinates"
            Dim tempBBOX As OL3BBox = Panel1.Controls(0)
            save.ExtentsDefaultExtentCoords = {tempBBOX.XtopLeft.Value, tempBBOX.YtopLeft.Value, tempBBOX.XbottomRight.Value, tempBBOX.YbottomRight.Value}
        End If


        'min/max zoom
        save.ExtentsRestrictedExtentMaxZoom = ComboBox3.Text
        save.ExtentsRestrictedExtentMinZoom = ComboBox2.Text

        save.ExtentsRestrictedExtentBufferMargin = NumericUpDown1.Value
        For h As Integer = 0 To CheckedListBox2.Items.Count - 1
            If CheckedListBox2.GetItemChecked(h) = True Then
                save.ExtentsRestrictedExtentSelectedLayers.Add(True)
            Else
                save.ExtentsRestrictedExtentSelectedLayers.Add(False)
            End If
        Next




    End Function


    Public Sub loadObj(ByVal saveObj As OL3MapOptionsSaveObject)
        resetCheckedListBoxs()

        OL3Projections1.loadObj(saveObj.mapProjection)
        OL3Basemaps1.loadObj(saveObj.basemaps)

        'default extent
        If saveObj.ExtentsDefaultExtentType = "Layers" Then
            Button3.Text = "Coordinates"

            For f As Integer = 0 To saveObj.ExtentsDefaultExtentSelectedLayers.Count - 1
                CheckedListBox1.SetItemChecked(f, saveObj.ExtentsDefaultExtentSelectedLayers(f))
            Next
        Else
            Button3.Text = "Layers"
            Dim tempBBOX As New OL3BBox

            tempBBOX.XtopLeft.Value = saveObj.ExtentsDefaultExtentCoords(0)
            tempBBOX.YtopLeft.Value = saveObj.ExtentsDefaultExtentCoords(1)
            tempBBOX.XbottomRight.Value = saveObj.ExtentsDefaultExtentCoords(2)
            tempBBOX.YbottomRight.Value = saveObj.ExtentsDefaultExtentCoords(3)
            Panel1.Controls.Clear()
            Panel1.Controls.Add(tempBBOX)

        End If


        'min/max zoom
        ComboBox3.Text = saveObj.ExtentsRestrictedExtentMaxZoom
        ComboBox2.Text = saveObj.ExtentsRestrictedExtentMinZoom

        NumericUpDown1.Value = saveObj.ExtentsRestrictedExtentBufferMargin
        For h As Integer = 0 To saveObj.ExtentsRestrictedExtentSelectedLayers.Count - 1
            CheckedListBox2.SetItemChecked(h, saveObj.ExtentsRestrictedExtentSelectedLayers(h))
        Next


    End Sub
End Class


<Serializable()> _
Public Class OL3MapOptionsSaveObject

    Public mapProjection As OL3ProjectionSaveObject
    Public basemaps As OL3BasemapsSaveObject

    Public ExtentsLinkedMap As String

    Public ExtentsDefaultExtentType As String
    Public ExtentsDefaultExtentSelectedLayers As New List(Of Boolean)
    Public ExtentsDefaultExtentCoords(3) As Double

    Public ExtentsRestrictedExtentMinZoom As Integer
    Public ExtentsRestrictedExtentMaxZoom As Integer
    Public ExtentsRestrictedExtentBufferMargin As Integer
    Public ExtentsRestrictedExtentSelectedLayers As New List(Of Boolean)


End Class

