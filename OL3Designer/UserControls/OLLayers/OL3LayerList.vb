Imports System.ComponentModel
Imports System.IO
Imports System.Reflection

Public Class OL3LayerList
    Public mapName As String
    Public mapNumber As Integer
    Public mapID As String
    Public parentMapList As OL3Maps
    Public mapOptions As OL3MapOptions
    'Dim OL3FullLayerList As BindingList(Of OLLayer)

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        mapOptions = New OL3MapOptions
        mapOptions.theParentLayerList = Me
        mapOptions.refreshMinMax()

        Dim ran1 As New Random
        mapID = ran1.Next(1000000, 9999999).ToString

    End Sub


    Private Sub OL3LayerList_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub


    Public Sub add(ByVal layer As String)
        'get file path
        Dim newOLLayer As New OLLayer(layer, mapNumber, Me)
        Dim layerIndex As Integer = DataGridView1.Rows.Add(newOLLayer)
        DataGridView1.Rows(layerIndex).Cells(0).Value = newOLLayer.layerName

        parentMapList.layout.addLayerToAllKeys(parentMapList.layout.Sp1, mapNumber, layerIndex)
    End Sub

    'places red X and edit pencil in remove column button cell
    Private Sub removeRow_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles DataGridView1.CellPainting
        If e.ColumnIndex = 1 AndAlso e.RowIndex >= 0 Then
            e.Paint(e.CellBounds, DataGridViewPaintParts.All)

            Dim bmpFind As Bitmap = My.Resources.Offline_16xLG__2
            e.Graphics.DrawImage(bmpFind, e.CellBounds.Left + 2, e.CellBounds.Top + 2)
            e.Handled = True
        End If

        If e.ColumnIndex = 2 AndAlso e.RowIndex >= 0 Then
            e.Paint(e.CellBounds, DataGridViewPaintParts.All)

            Dim bmpFind As Bitmap = My.Resources.PencilTool_206
            e.Graphics.DrawImage(bmpFind, e.CellBounds.Left + 2, e.CellBounds.Top + 2)
            e.Handled = True
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim theRow As OLLayer = DataGridView1.Rows(e.RowIndex)
        If e.ColumnIndex = 1 Then ' remove Layer
            DataGridView1.Rows.RemoveAt(e.RowIndex)
            'force remove any items in keys
            parentMapList.layout.refreshAllKeysAndControls(parentMapList.layout.Sp1)

        End If

        If e.ColumnIndex = 2 Then 'edit layer
            theRow.OL3Edit.ShowDialog()
        End If

    End Sub

    '********************************************************************
    '********************************************************************
    '********************************************************************
    'These function amalgomate the info from all layers
    '********************************************************************
    '********************************************************************
    '********************************************************************
    Function getAllLayersDeclarations() As String
        'cycle through each layer and get the var list js
        getAllLayersDeclarations = ""
        Dim theLayer As OLLayer

        Dim layerNameRegister As String = "" 'list of free text detailing the user names of the layers

        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            theLayer = DataGridView1.Rows(x)
            layerNameRegister = layerNameRegister & Chr(34) & theLayer.layerName & Chr(34) & ","
            getAllLayersDeclarations = getAllLayersDeclarations & "var map" & mapNumber & "_vectorLayer_" & x & ";" & Chr(10)
            getAllLayersDeclarations = getAllLayersDeclarations & "var map" & mapNumber & "_vectorSource_" & x & ";" & Chr(10)
        Next

        'remove last comma of layer names
        layerNameRegister = layerNameRegister.Substring(0, layerNameRegister.Length - 1)
        layerNameRegister = "var registerOfLayerNames = [" & layerNameRegister & "];" & Chr(10)

        getAllLayersDeclarations = getAllLayersDeclarations & Chr(10) & layerNameRegister & Chr(10) & Chr(10)
    End Function

    Function getAllProjectionDefinitions() As String
        'gets all the WKT definitions for the layers in this map
        getAllProjectionDefinitions = Chr(10)
        Dim theLayer As OLLayer
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            theLayer = DataGridView1.Rows(x)
            getAllProjectionDefinitions = getAllProjectionDefinitions & theLayer.getLayerProjectionDefinition.Replace(Chr(10), " ")
        Next
        getAllProjectionDefinitions = getAllProjectionDefinitions & Chr(10)

        'get the map projection defination
        getAllProjectionDefinitions = getAllProjectionDefinitions & mapOptions.getMapProjectionDefinition() & Chr(10)

        'Projections can be proj or wkt strings.
        'GEOGCS["unnamed",DATUM["WGS_1984",SPHEROID["WGS 84",6378137,298.257223563],TOWGS84[0,0,0,0,0,0,0]],PRIMEM["Greenwich",0],UNIT["degree",0.0174532925199433]]
        'Proj4js.defs['EPSG:0'] = "+proj=tmerc +lat_0=49 +lon_0=-2 +k=0.9996012717 +x_0=400000 +y_0=-100000 +ellps=airy +datum=OSGB36 +units=m +no_defs ";

        'projections are based on dummmy epsg codes. these are named after the layer number integer
    End Function

    Function getAllStyleTypesLiterol() As String
        getAllStyleTypesLiterol = ""
        Dim theStyleRow As styleRow
        Dim theLayer As OLLayer
        Dim tempStyleTypes As String = ""

        'for each layer
        For t As Integer = 0 To DataGridView1.Rows.Count - 1
            theLayer = DataGridView1.Rows(t)
            getAllStyleTypesLiterol = getAllStyleTypesLiterol & ",'" & t & "'" & ":["

            For a As Integer = 0 To theLayer.OL3Edit.OL3LayerStylePicker1.DataGridView1.Rows.Count - 1
                theStyleRow = theLayer.OL3Edit.OL3LayerStylePicker1.DataGridView1.Rows(a)
                tempStyleTypes = tempStyleTypes & ",'" & theStyleRow.StyleType & "'"
            Next

            getAllStyleTypesLiterol = getAllStyleTypesLiterol & tempStyleTypes.Substring(1) & "]" & Chr(10)
            tempStyleTypes = ""
        Next

        getAllStyleTypesLiterol = "'" & mapNumber & "':{" & getAllStyleTypesLiterol.Substring(1) & "}"

    End Function

    Function getAllHeatmapParameters() As String
        getAllHeatmapParameters = ""
        Dim theLayer As OLLayer
        Dim tempStyleTypes As String = ""

        'for each layer
        For t As Integer = 0 To DataGridView1.Rows.Count - 1
            theLayer = DataGridView1.Rows(t)
            getAllHeatmapParameters = getAllHeatmapParameters & ",'" & t & "'" & ":" & theLayer.OL3Edit.OL3LayerStylePicker1.getHeatmapParameters

            getAllHeatmapParameters = getAllHeatmapParameters & "" & Chr(10)
            tempStyleTypes = ""
        Next

        getAllHeatmapParameters = "'" & mapNumber & "':{" & getAllHeatmapParameters.Substring(1) & "}"

    End Function

    Function getAllLayersSource() As String
        'cycle through each layer and get the var list js
        Dim theLayer As OLLayer

        'get list of layers
        getAllLayersSource = ""
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            theLayer = DataGridView1.Rows(x)
            getAllLayersSource = getAllLayersSource & theLayer.getLayerSource
        Next


    End Function

    Function getAllClusterSource() As String
        'cycle through each layer and get the var list js
        Dim theLayer As OLLayer

        'get list of layers
        getAllClusterSource = ""
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            theLayer = DataGridView1.Rows(x)
            getAllClusterSource = getAllClusterSource & theLayer.getClusterSource
        Next


    End Function

    Function getAllLayersLayers() As String
        'cycle through each layer and get the var list js
        Dim theLayer As OLLayer

        'get list of layers
        getAllLayersLayers = ""
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            theLayer = DataGridView1.Rows(x)
            getAllLayersLayers = getAllLayersLayers & theLayer.getLayerLayer
        Next

    End Function



    Function getAllLayersPopup() As String
        'cycle through each layer and get the popup js

    End Function


    Function getAllLayersStyle() As String
        getAllLayersStyle = ""
        Dim allLayerStyleFunctions As String = ""
        Dim allLayersSetStyleLine As String = ""

        'cycle through each layer and get the style js
        Dim theLayer As OLLayer
        Dim tempListOfStyles As New List(Of String)
        Dim tempLayerStyleFunctions As New List(Of String)
        Dim tempLayerStyleLabelExpresions As New List(Of String)
        Dim tempLayerStyleRotationExpresions As New List(Of String)
        Dim tempLayerStyleLabelVars As New List(Of String)
        Dim tempLayerStyleLabelRes As New List(Of String)


        'for each layer
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            theLayer = DataGridView1.Rows(x)
            tempListOfStyles = theLayer.OL3Edit.OL3LayerStylePicker1.getAllStyles
            tempLayerStyleLabelExpresions = theLayer.OL3Edit.OL3LayerStylePicker1.getAllLabelExpresions
            tempLayerStyleRotationExpresions = theLayer.OL3Edit.OL3LayerStylePicker1.getAllRotationExpresions
            tempLayerStyleFunctions = theLayer.OL3Edit.OL3LayerStylePicker1.getAllConditions
            tempLayerStyleLabelVars = theLayer.OL3Edit.OL3LayerStylePicker1.getAllLabelVars

            'for each dynamic style get style var
            For y As Integer = 0 To tempListOfStyles.Count - 1
                'getAllLayersStyle = getAllLayersStyle & "var map" & mapNumber & "_vectorLayer_" & x & "_Style" & y & " = " & tempListOfStyles(y) & ";" & Chr(10)
                'get layer style array(these have no knownledge of conditions
                getAllLayersStyle = getAllLayersStyle & "function map" & mapNumber & "_vectorLayer_" & x & "_Style" & y & "(feature, resolution) {" & tempLayerStyleLabelVars(y) & " if ( feature != 'key'  ){" & tempLayerStyleRotationExpresions(y) & ";" & tempLayerStyleLabelExpresions(y) & "} return {" & tempListOfStyles(y) & "}; };" & Chr(10)

                'get layer style function
                allLayerStyleFunctions = allLayerStyleFunctions & "var map" & mapNumber & "_vectorLayer_" & x & "_Style" & y & "_Function = function(feature, resolution) {" & Chr(10)
                'Select Case theLayer.OL3Edit.OlLayerFilterPicker1.Panel1.Controls(0).GetType
                'Case GetType(OLLayerFilterUniqueValues)
                'tempUniqueFilter = theLayer.OL3Edit.OlLayerFilterPicker1.Panel1.Controls(0)
                'allLayerStyleFunctions = allLayerStyleFunctions & tempUniqueFilter.getFilterIfStringForStyleFunction()
                'End Select
                allLayerStyleFunctions = allLayerStyleFunctions & amalgamateIfs(tempLayerStyleFunctions(y), "map" & mapNumber & "_vectorLayer_" & x & "_Style" & y & "") & "}" & Chr(10)

            Next
            'set default style (check for heatmaps & don't fire if it is one)
            allLayersSetStyleLine = allLayersSetStyleLine & "if (mapStyleTypes['" & mapNumber & "']['" & x & "'][0] != 'Heatmap'){" & Chr(10)
            allLayersSetStyleLine = allLayersSetStyleLine & "map" & mapNumber & "_vectorLayer_" & x & ".setStyle(" & "map" & mapNumber & "_vectorLayer_" & x & "_Style" & "0" & "_Function);" & Chr(10) & Chr(10)
            allLayersSetStyleLine = allLayersSetStyleLine & "}" & Chr(10)
        Next

        'add functions
        getAllLayersStyle = getAllLayersStyle & Chr(10) & Chr(10) & "//********** STYLE FUNCTIONS ********" & Chr(10) & allLayerStyleFunctions
        'add setSyyle lines .... must be after unctions declared
        getAllLayersStyle = getAllLayersStyle & Chr(10) & Chr(10) & "//********** SET STYLES ********" & Chr(10) & allLayersSetStyleLine
    End Function

    Function amalgamateIfs(ByVal allIfs As String, ByVal thePrefix As String) As String
        'creates the list of conditional if statements from the list of conditions passed
        amalgamateIfs = ""

        'get map, layer and style num from prefix
        Dim layerIdentification() As String = thePrefix.Split("_")
        Dim mapNumIdentification As String = layerIdentification(0).Replace("map", "")
        Dim layerNumIdentification As String = layerIdentification(2)
        Dim styleNumIdentification As String = layerIdentification(3).Replace("Style", "")

        'get any pre styling code to be run in the style function
        Dim theOLLayer As OLLayer = DataGridView1.Rows(CInt(layerNumIdentification))
        amalgamateIfs = amalgamateIfs & theOLLayer.OL3Edit.OL3LayerStylePicker1.getPreConditionCode(mapNumIdentification, layerNumIdentification, CInt(styleNumIdentification))

        'split out ifs
        Dim theIfs() As String = allIfs.Split("|")

        'cycle through ifs and construct selection statments
        For x As Integer = 0 To theIfs.Count - 2 '-2 as there will be an extra "|" on the string passed to this function
            'only add as condition if it sarts with an "if", else it is code to be run prior to styling
            If theIfs(x).Substring(0, 3) = "if(" Then
                amalgamateIfs = amalgamateIfs & theIfs(x) & "return " & thePrefix & "(feature, resolution)[" & x & "];}"
            Else
                amalgamateIfs = amalgamateIfs & theIfs(x) & Chr(10) & Chr(10)
            End If

        Next

    End Function



    Function getMapParameters() As String
        'getbasemaps
        Dim basemapName As String = "map" & mapNumber & "_Basemap"
        If DataGridView1.Rows.Count > 0 Then 'if at least 1 layer is present 
            basemapName = basemapName & ","
        End If
        'override if none selected
        If mapOptions.OL3Basemaps1.TreeView1.SelectedNode.Text = "None" Then basemapName = ""


        'get list of layers
        Dim LayerListText As String = ""
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            LayerListText = LayerListText & "map" & mapNumber & "_vectorLayer_" & x & ","
        Next

        LayerListText = basemapName & LayerListText.Substring(0, LayerListText.Length - 1) 'remove final comma

        ' get the parameters js
        getMapParameters = ""
        getMapParameters = getMapParameters & "var map" & mapNumber & "_View =  new ol.View({center: [0, 0],zoom: 3,  projection: 'USER:" & mapNumber & "999'" & mapOptions.getRestrictedExtentAndZoomString & "})" & Chr(10)

        'ensure map still exists and hasn't been deleted since setup
        mapOptions.checkSyncedMap()

        Dim mapViewJS As String
        If mapOptions.ComboBox1.SelectedIndex <> 0 And mapOptions.ComboBox1.Text <> "" Then
            mapViewJS = mapOptions.ComboBox1.Text.Replace(" ", "").ToLower & ".getView()"  'mapViewJS = mapOptions.ComboBox1.Text.Replace(" ", "").ToLower & "_View"
        Else
            mapViewJS = "map" & mapNumber & "_View" & Chr(10)
        End If

        getMapParameters = getMapParameters & "var map" & mapNumber & " = new ol.Map({" & Chr(10)
        getMapParameters = getMapParameters & "layers: [" & LayerListText & "]," & Chr(10)
        getMapParameters = getMapParameters & "target: document.getElementById('map" & mapNumber & "')," & Chr(10)
        getMapParameters = getMapParameters & "controls: [new ol.control.Zoom()]," & Chr(10)
        getMapParameters = getMapParameters & "view: " & mapViewJS & Chr(10) 'map" & mapNumber & "_View" & Chr(10)
        getMapParameters = getMapParameters & "});" & Chr(10)

        'set default extents
        getMapParameters = getMapParameters & mapOptions.getExtentString & Chr(10) & Chr(10)


        'prepare popup event handler
        getMapParameters = getMapParameters & "// display popup on click" & Chr(10)
        getMapParameters = getMapParameters & "map" & mapNumber & ".on('click', function(evt) { " & Chr(10)
        getMapParameters = getMapParameters & "var feature_map" & mapNumber & " = map" & mapNumber & ".forEachFeatureAtPixel(evt.pixel, " & Chr(10)
        getMapParameters = getMapParameters & "function(feature, layer) { " & Chr(10)
        getMapParameters = getMapParameters & "currentLayer = layer " & Chr(10)
        getMapParameters = getMapParameters & " return feature; " & Chr(10)
        getMapParameters = getMapParameters & "}); " & Chr(10)
        getMapParameters = getMapParameters & "if (feature_map" & mapNumber & ") {" & Chr(10)
        getMapParameters = getMapParameters & "var geometry_map" & mapNumber & " = feature_map" & mapNumber & ".getGeometry(); " & Chr(10)
        getMapParameters = getMapParameters & "var coord_map" & mapNumber & " = geometry_map" & mapNumber & ".getCoordinates(); " & Chr(10) & Chr(10)

        getMapParameters = getMapParameters & ""

        getMapParameters = getMapParameters & getAllPopupConditions() & Chr(10)
        getMapParameters = getMapParameters & "}"

        'if not a feature and click has occured in blank space destroy all popups
        Dim destroyAll As String = "else{"
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            destroyAll = destroyAll & "$(map" & mapNumber & "_vectorLayer_" & i & "_popup).popover('destroy');"
        Next
        destroyAll = destroyAll & "}" & Chr(10)

        getMapParameters = getMapParameters & destroyAll & "});"

    End Function

    Function getAllPopupConditions() As String
        'cycle through each layer and get the popup conditions js
        Dim theLayer As OLLayer

        'get list of layers
        getAllPopupConditions = ""
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            theLayer = DataGridView1.Rows(x)
            getAllPopupConditions = getAllPopupConditions & theLayer.getPopupCondition
        Next

    End Function

    Function getAllPopupFunctions(Optional ByVal outPath As String = "") As String
        'cycle through each layer and get the popup function js
        Dim theLayer As OLLayer

        'get list of layers
        getAllPopupFunctions = ""
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            theLayer = DataGridView1.Rows(x)
            getAllPopupFunctions = getAllPopupFunctions & theLayer.getPopupFunction(outPath)
        Next
    End Function

 
    Function getAllPopupElements() As String
        'cycle through each layer and get the popup element js
        Dim theLayer As OLLayer

        'get list of layers
        getAllPopupElements = ""
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            theLayer = DataGridView1.Rows(x)
            getAllPopupElements = getAllPopupElements & theLayer.getPopupElements()
        Next

    End Function

    Public Function createAllPopupOverlays() As String
        'cycle through each layer and get the popup overlay js -> must be placed after map initialisation
        Dim theLayer As OLLayer

        'get list of layers
        createAllPopupOverlays = ""
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            theLayer = DataGridView1.Rows(x)
            createAllPopupOverlays = createAllPopupOverlays & theLayer.createPopupOverlays()
        Next
    End Function


    Function getAllExternalFilterJS() As String
        'function gets the variables and onchange function -> to be placed in JS, anywhere.
        Dim theLayer As OLLayer
        getAllExternalFilterJS = ""

        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            theLayer = DataGridView1.Rows(x)
            getAllExternalFilterJS = getAllExternalFilterJS & theLayer.getFilterExternalJS(x) & Chr(10)
        Next

    End Function



    Function getBasemap(ByVal outPath As String, ByVal outName As String) As String
        getBasemap = ""

        getBasemap = "var map" & mapNumber & "_Basemap = " & mapOptions.OL3Basemaps1.getBasemapJS(outPath, mapNumber, outName) & "" & Chr(10)
    End Function

    Public Function getAllJS(ByVal outName As String, Optional ByVal outPath As String = "") As String
        getAllJS = ""

        getAllJS = getAllJS & Chr(10) & "//**************Create popup elements ******************" & Chr(10) & getAllPopupElements()
        getAllJS = getAllJS & Chr(10) & "//**************Layer declarations******************" & Chr(10) & getAllLayersDeclarations()
        getAllJS = getAllJS & Chr(10) & "//**************Projection definition******************" & Chr(10) & getAllProjectionDefinitions()

        getAllJS = getAllJS & Chr(10) & "//**************basemaps******************" & Chr(10) & getBasemap(outPath, outName)
        getAllJS = getAllJS & Chr(10) & "//**************Layer sources******************" & Chr(10) & getAllLayersSource()
        getAllJS = getAllJS & Chr(10) & "//**************Cluster sources******************" & Chr(10) & getAllClusterSource()
        getAllJS = getAllJS & Chr(10) & "//**************Layer layers******************" & Chr(10) & getAllLayersLayers()
        getAllJS = getAllJS & Chr(10) & "//**************Filter vars and onchange fuction******************" & Chr(10) & getAllExternalFilterJS()
        getAllJS = getAllJS & Chr(10) & "//**************Layer styles******************" & Chr(10) & getAllLayersStyle()
        getAllJS = getAllJS & Chr(10) & "//**************Map Parameters******************" & Chr(10) & getMapParameters()
        getAllJS = getAllJS & Chr(10) & "//**************Popup overlays******************" & Chr(10) & createAllPopupOverlays()
        ' getAllJS = getAllJS & Chr(10) & "//**************Get max Extent function******************" & Chr(10) & getGetExtentFunction()
        ' getAllJS = getAllJS & Chr(10) & "//**************Get key function******************" & Chr(10) & getKeyJS()
        getAllJS = getAllJS & Chr(10) & "//**************Get popup functions******************" & Chr(10) & getAllPopupFunctions(outPath)
    End Function

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        'show map options
        mapOptions.ShowDialog()
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        'add
        Dim OFD As New OpenFileDialog
        OFD.Multiselect = True
        OFD.Filter = "layer files(*.shp;*.tab;*.kml)|*.shp;*.tab;*.kml"
        OFD.Title = "Select a vector layer to add to the interactive map"

        If OFD.ShowDialog = DialogResult.OK Then
            For Each item As String In OFD.FileNames
                add(item)
            Next
        End If

    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        Dim gps As New GPSPhotographs
        gps.importGPSPhotos(Me)
    End Sub

    Function getLayerNumberByID(ByVal layerID As Integer) As Integer
        Dim theLayer As OLLayer
        For t As Integer = 0 To DataGridView1.Rows.Count - 1
            theLayer = DataGridView1.Rows(t)
            If theLayer.layerID = layerID Then
                Return t
            End If
        Next
        Return -1
    End Function


    Public Function save() As OL3LayerListSaveObject
        save = New OL3LayerListSaveObject
        Dim tempRow As OLLayer

        For y As Integer = 0 To DataGridView1.Rows.Count - 1
            tempRow = DataGridView1.Rows(y)
            save.layerList.Add(tempRow.save())
        Next


        save.mapOptions = mapOptions.save()

    End Function


    Public Sub loadObj(ByVal saveObj As OL3LayerListSaveObject)

        Dim tempRow As OLLayer
        DataGridView1.Rows.Clear()
        For y As Integer = 0 To saveObj.layerList.Count - 1
            tempRow = New OLLayer(saveObj.layerList(y).layerPath, saveObj.layerList(y).MapNum, Me)
            tempRow.loadObj(saveObj.layerList(y))
            DataGridView1.Rows.Add(tempRow)

            DataGridView1.Rows(DataGridView1.Rows.Count - 1).Cells(0).Value = Path.GetFileNameWithoutExtension(saveObj.layerList(y).layerPath)


        Next

        mapOptions.loadObj(saveObj.mapOptions)

    End Sub


End Class








Class OLLayer
    Inherits DataGridViewRow
    Public OL3LayerPath As String
    Public layerName As String
    Public layerProjWKT As String
    Public mapNumber As Integer
    Public layerID As Integer
    Public OL3Edit As OL3EditLayer
    Public useLayerForDefaultExtent As Boolean = 1
    Public useLayerForRestrictedExtent As Boolean = 0
    Public parentLayerList As OL3LayerList

    Public MinX As Double
    Public MinY As Double
    Public MaxX As Double
    Public MaxY As Double


    Public Sub New(ByVal layerPath As String, ByVal mapNum As Integer, ByVal theParentLayerList As OL3LayerList)
        parentLayerList = theParentLayerList
        Dim randomNum As Random = New Random()
        layerID = randomNum.Next(1, 50000)
        Dim GDAL As New GDALImport

        mapNumber = mapNum
        OL3LayerPath = layerPath
        layerName = Path.GetFileNameWithoutExtension(layerPath)
        OL3Edit = New OL3EditLayer(layerPath, GDAL.getLayerType(layerPath, 0))


        Dim nameCell As New DataGridViewTextBoxCell
        nameCell.Value = " "
        Cells.Add(nameCell)

        'set style
        'OL3Edit.OL3LayerStylePicker1.LayerType = GDAL.getLayerType(layerPath, 0)

        'set projection if layer has one
        Dim projList As List(Of String) = GDAL.getCRS(layerPath)
        OL3Edit.OL3Projections1.TextBox1.Text = projList(0)
        OL3Edit.OL3Projections1.TextBox2.Text = projList(1)
        layerProjWKT = projList(2)

        'get BBox
        Dim BBox As List(Of Double)
        BBox = GDAL.getExtents(layerPath)
        MinX = BBox(0)
        MaxX = BBox(1)
        MinY = BBox(2)
        MaxY = BBox(3)

        'set general 
        OL3Edit.OL3General1.TextBox1.Text = layerPath
        OL3Edit.OL3General1.TextBox2.Text = layerName
        OL3Edit.OL3General1.Label4.Text = OL3Edit.layerType



    End Sub

    Public Function getFilterExternalJS(ByVal layerNum As Integer) As String
        getFilterExternalJS = Chr(10) & ""
        Select Case OL3Edit.OlLayerFilterPicker1.Panel1.Controls(0).GetType
            Case GetType(OLLayerFilterUniqueValues)
                Dim tempUniqueFilters As OLLayerFilterUniqueValues = OL3Edit.OlLayerFilterPicker1.Panel1.Controls(0)
                tempUniqueFilters.layerNumber = layerNum
                getFilterExternalJS = getFilterExternalJS & tempUniqueFilters.getFilterString() & Chr(10) & Chr(10)


                'second function cycles through features and checks againts filter conditions
                getFilterExternalJS = getFilterExternalJS & "function changeFilter_map_" & mapNumber & "_layer_" & layerNum & "_Filters(){" & Chr(10)
                getFilterExternalJS = getFilterExternalJS & "var allfeatures = map" & mapNumber & "_vectorSource_" & layerNum & ".getFeatures();" & Chr(10)
                getFilterExternalJS = getFilterExternalJS & "var feature ;" & Chr(10)
                getFilterExternalJS = getFilterExternalJS & "for (i = 0; i < allfeatures.length; i++) { " & Chr(10)
                getFilterExternalJS = getFilterExternalJS & "feature = allfeatures[i];" & Chr(10)


                Dim tempListOfStyles As New List(Of String)
                Dim tempUniqueFilter As OLLayerFilterUniqueValues
                tempListOfStyles = OL3Edit.OL3LayerStylePicker1.getAllStyles

                tempUniqueFilter = OL3Edit.OlLayerFilterPicker1.Panel1.Controls(0)
                getFilterExternalJS = getFilterExternalJS & tempUniqueFilter.getFilterIfStringForStyleFunction() & Chr(10)


                getFilterExternalJS = getFilterExternalJS & "}" & Chr(10)
                getFilterExternalJS = getFilterExternalJS & "}" & Chr(10) & Chr(10)
        End Select

    End Function

    Public Function getPopupElements() As String
        getPopupElements = Chr(10) & ""

        'return js to create a div in the map div for this layers popups + apply css to it
        getPopupElements = getPopupElements & "var map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_popup = document.createElement('Div'); " & Chr(10)
        getPopupElements = getPopupElements & "map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_popup.id = 'map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_popupDiv';" & Chr(10)
        getPopupElements = getPopupElements & "document.getElementById('map" & mapNumber & "').appendChild(map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_popup);" & Chr(10)
        getPopupElements = getPopupElements & "$('.map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_popupDiv').addClass($('.popup'));" & Chr(10)

    End Function

    Public Function createPopupOverlays() As String
        createPopupOverlays = Chr(10) & ""

        createPopupOverlays = createPopupOverlays & "var Overlay_map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_popup = new ol.Overlay({" & Chr(10)
        createPopupOverlays = createPopupOverlays & "element: map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_popup," & Chr(10)
        createPopupOverlays = createPopupOverlays & "positioning: 'bottom-center'," & Chr(10)
        createPopupOverlays = createPopupOverlays & "stopEvent: true" & Chr(10)
        createPopupOverlays = createPopupOverlays & "});" & Chr(10)
        createPopupOverlays = createPopupOverlays & "map" & mapNumber & ".addOverlay(Overlay_map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_popup);" & Chr(10)

    End Function

    Public Function getPopupCondition() As String '>>> move to popupPicker user control ? ->> no, just the function
        getPopupCondition = "" & Chr(10)

        If OL3Edit.OL3LayerPopupPicker1.Panel1.Controls.Count > 0 Then
            getPopupCondition = "" & Chr(10)
            getPopupCondition = getPopupCondition & "if(currentLayer.get('name') == 'map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "'){" & Chr(10)

            'destroy any old popup
            getPopupCondition = getPopupCondition & "$(map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_popup).popover('destroy');" & Chr(10)

            'if polygon or line then place popup at click not 
            Select Case OL3Edit.layerType
                Case "point"
                    'do nothing
                Case Else
                    getPopupCondition = getPopupCondition & "coord_map" & mapNumber & " = evt.coordinate; " & Chr(10)
            End Select

            getPopupCondition = getPopupCondition & "Overlay_map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_popup.setPosition(coord_map" & mapNumber & "); " & Chr(10)

            getPopupCondition = getPopupCondition & "$(map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_popup).popover({" & Chr(10)
            getPopupCondition = getPopupCondition & "'placement': '" & getPopupOri() & "'," & Chr(10)
            getPopupCondition = getPopupCondition & "'html': true," & Chr(10)
            'getPopupCondition = "'title': 'Test Title',"
            getPopupCondition = getPopupCondition & "'content': map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_popup_function(feature_map" & mapNumber & ")" & Chr(10)
            getPopupCondition = getPopupCondition & "});" & Chr(10)
            getPopupCondition = getPopupCondition & "$(map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_popup).popover('show');" & Chr(10)
            getPopupCondition = getPopupCondition & "} else {" & Chr(10)
            getPopupCondition = getPopupCondition & "$(map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_popup).popover('destroy');" & Chr(10)
            getPopupCondition = getPopupCondition & "}" & Chr(10)
            getPopupCondition = getPopupCondition & "" & Chr(10) & Chr(10)



        End If
    End Function

    Public Function getPopupOri() As String
        Dim popupControlSimple As OL3PopupDesignerSimple
        If OL3Edit.OL3LayerPopupPicker1.Panel1.Controls.Count > 0 Then
            Select Case OL3Edit.OL3LayerPopupPicker1.Panel1.Controls(0).GetType()
                Case GetType(OL3PopupDesignerSimple) 'single popup - simple
                    popupControlSimple = OL3Edit.OL3LayerPopupPicker1.Panel1.Controls(0)
                    Return popupControlSimple.selectedOrientation
                    'single popup - complex

                    'conditional popup - unique values

                    'conditional popup - value ranges

            End Select

        End If
    End Function

    Public Function getPopupFunction(Optional ByVal outPath As String = "") As String
        Dim popupControlSimple As OL3PopupDesignerSimple

        If OL3Edit.OL3LayerPopupPicker1.Panel1.Controls.Count > 0 Then
            Select Case OL3Edit.OL3LayerPopupPicker1.Panel1.Controls(0).GetType()
                Case GetType(OL3PopupDesignerSimple) 'single popup - simple
                    popupControlSimple = OL3Edit.OL3LayerPopupPicker1.Panel1.Controls(0)
                    Return popupControlSimple.getPopupFunction(mapNumber, Me.Cells.Item(0).RowIndex, outPath)
                    'single popup - complex

                    'conditional popup - unique values

                    'conditional popup - value ranges

            End Select

        End If
    End Function

    Public Function getLayerSource() As String
        getLayerSource = ""
        Dim GDAL As New GDALImport
        Dim ProjectionString As String = ""
        'for duplicate layer test -> see further down
        Dim duplicateIds As Integer() = parentLayerList.parentMapList.isLayerDuplicated(OL3LayerPath, mapNumber, parentLayerList.getLayerNumberByID(layerID))

        ''///////////////OL3.4/////////////
        ''///////////////function to re-set layer source to all feature -> prefixed by "a" ///////////////
        'If duplicateIds(0) = -1 Then 'not required a duplicate layer
        '    'setup var
        '    getLayerSource = getLayerSource & " map" & mapNumber & "_vectorSource_" & Me.Cells.Item(0).RowIndex & "a  = new ol.source.GeoJSON({" & Chr(10)
        '    'add projection (out, i.e. the map projection)
        '    getLayerSource = getLayerSource & "projection: 'USER:" & mapNumber & "999'," & Chr(10) 'map projection is always called <map number>999

        '    'add features
        '    getLayerSource = getLayerSource & "object: " & GDAL.getGeoJson({OL3LayerPath}) & Chr(10)

        '    'add IN projection (i.e. the source data projection)
        '    getLayerSource = getLayerSource.Replace("{" & Chr(34) & "type" & Chr(34) & ":" & Chr(34) & "FeatureCollection" & Chr(34) & ",", "{'type':'FeatureCollection', 'crs': { 'type': 'name','properties': {'name': 'USER:" & mapNumber & "00" & Me.Cells.Item(0).RowIndex & "'}},")
        '    '{'type':'FeatureCollection', crs': { 'type': 'name','properties': {'name': 'USER:0'}},

        '    'finish
        '    getLayerSource = getLayerSource & "});" & Chr(10) & Chr(10)
        '    '///////////////////////////////////////////////////////////////////////////////////////////////////
        'End If

        '///////////////OL3.9/////////////////////////
        '///////////////function to re-set layer source to all feature -> prefixed by "a" ///////////////
        If duplicateIds(0) = -1 Then 'not required a duplicate layer
            'setup var
            getLayerSource = getLayerSource & " map" & mapNumber & "_vectorSource_" & Me.Cells.Item(0).RowIndex & "a  = new ol.source.Vector({" & Chr(10)
            'add projection (in and out, in = the data projection and out = the projection the source data is available to openlayers, i.e. the map projection)
            'feature projection = 'out' projection.  Data projection = 'in' projection
            ProjectionString = ",{featureProjection:  'USER:" & mapNumber & "999', dataProjection:'USER:" & mapNumber & "00" & Me.Cells.Item(0).RowIndex & "'}"  'map projection ('out' projection) is always called <map number>999. layer projection ('in' projection) allways called <map number>00<layer number>

            'add features
            getLayerSource = getLayerSource & "features: (new ol.format.GeoJSON()).readFeatures(" & GDAL.getGeoJson({OL3LayerPath}) & ProjectionString & ")" & Chr(10)

            'add IN projection in to the geojson (i.e. the source data projection)
            getLayerSource = getLayerSource.Replace("{" & Chr(34) & "type" & Chr(34) & ":" & Chr(34) & "FeatureCollection" & Chr(34) & ",", "{'type':'FeatureCollection', 'crs': { 'type': 'name','properties': {'name': 'USER:" & mapNumber & "00" & Me.Cells.Item(0).RowIndex & "'}},")
            '{'type':'FeatureCollection', crs': { 'type': 'name','properties': {'name': 'USER:0'}},

            'finish
            getLayerSource = getLayerSource & "});" & Chr(10) & Chr(10)
            '///////////////////////////////////////////////////////////////////////////////////////////////////
        End If

        '///////OL3.4
        ''*********************variabls to hold empty source and call to function to populate it with all features(alows filtering and re-seting)***********************
        'getLayerSource = getLayerSource & "var map" & mapNumber & "_vectorSource_" & Me.Cells.Item(0).RowIndex & " =  new ol.source.GeoJSON({projection: 'USER:" & mapNumber & "999'});" & Chr(10)
        'getLayerSource = getLayerSource & "map" & mapNumber & "_vectorSource_" & Me.Cells.Item(0).RowIndex & "_SetSource();" & Chr(10) & Chr(10)
        ''*******************************************************************************************************************************

        '/////OL3.8
        '*********************variabls to hold empty source and call to function to populate it with all features(alows filtering and re-seting)***********************
        getLayerSource = getLayerSource & "var map" & mapNumber & "_vectorSource_" & Me.Cells.Item(0).RowIndex & " =  new ol.source.Vector({defaultDataProjection: 'USER:" & mapNumber & "999', format: new ol.format.GeoJSON(), attributions:" & Me.OL3Edit.OL3General1.getAttributionsJS & "});" & Chr(10)
        getLayerSource = getLayerSource & "map" & mapNumber & "_vectorSource_" & Me.Cells.Item(0).RowIndex & "_SetSource();" & Chr(10) & Chr(10)
        '*******************************************************************************************************************************

        '\\\\\\\\\\\\\\\\\\\\\Function to set re-set layer source to original (after all to a filter)\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        getLayerSource = getLayerSource & "function map" & mapNumber & "_vectorSource_" & Me.Cells.Item(0).RowIndex & "_SetSource() {" & Chr(10)
        getLayerSource = getLayerSource & "map" & mapNumber & "_vectorSource_" & Me.Cells.Item(0).RowIndex & ".clear('fast');" & Chr(10)

        'check for multiple use of same layer (so as not to duplicate geojson strings)
        If duplicateIds(0) = -1 Then
            'layer is unique or first occurance of itself
            getLayerSource = getLayerSource & "map" & mapNumber & "_vectorSource_" & Me.Cells.Item(0).RowIndex & ".addFeatures(map" & mapNumber & "_vectorSource_" & Me.Cells.Item(0).RowIndex & "a.getFeatures());" & Chr(10)
        Else
            'layer is a duplicate - point it to another layer source
            getLayerSource = getLayerSource & "map" & mapNumber & "_vectorSource_" & Me.Cells.Item(0).RowIndex & ".addFeatures(map" & duplicateIds(0) + 1 & "_vectorSource_" & duplicateIds(1) & "a.getFeatures());" & Chr(10)
        End If

        getLayerSource = getLayerSource & "}" & Chr(10) & Chr(10) & Chr(10)
        '\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\


    End Function

    Public Function getClusterSource() As String
        getClusterSource = ""
        Dim isClusterStyle As Boolean = False
        Dim currentRow As styleRow
        Dim currentCluster As OL3LayerStyleCluster
        Dim currentClusterStats As OL3LayerStyleClusterStats
        'are there any cluster styles in the style list
        For b As Integer = 0 To OL3Edit.OL3LayerStylePicker1.DataGridView1.Rows.Count - 1
            currentRow = OL3Edit.OL3LayerStylePicker1.DataGridView1.Rows(b)
            If currentRow.StyleType = "Simple Cluster" Then
                isClusterStyle = True
                currentCluster = currentRow.StyleControl

                getClusterSource = getClusterSource & " map" & mapNumber & "_clusterSource_" & Me.Cells.Item(0).RowIndex & "_Style" & b & " = new ol.source.Cluster({"
                getClusterSource = getClusterSource & "distance:" & currentCluster.NumericUpDown2.Value & ","
                getClusterSource = getClusterSource & "source:   map" & mapNumber & "_vectorSource_" & Me.Cells.Item(0).RowIndex & "})" & Chr(10) & Chr(10)

            ElseIf currentRow.StyleType = "Cluster and Statistics" Then
                isClusterStyle = True
                currentClusterStats = currentRow.StyleControl

                getClusterSource = getClusterSource & " map" & mapNumber & "_clusterSource_" & Me.Cells.Item(0).RowIndex & "_Style" & b & " = new ol.source.Cluster({"
                getClusterSource = getClusterSource & "distance:" & currentClusterStats.NumericUpDown2.Value & ","
                getClusterSource = getClusterSource & "source:   map" & mapNumber & "_vectorSource_" & Me.Cells.Item(0).RowIndex & "})" & Chr(10) & Chr(10)


            End If

        Next

        'exit if no cluster styles
        If isClusterStyle = False Then Return ""

    End Function

    Function getLayerLayer() As String
        getLayerLayer = ""

        '//////////Declare layer first as avector layer so other code will not error///////////////
        getLayerLayer = getLayerLayer & "// '//////////Declare layer first as avector layer so other code will not error///////////////" & Chr(10)
        'setup var
        getLayerLayer = getLayerLayer & " map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "  = new ol.layer.Vector({" & Chr(10)
        'add title (as shown in key)
        getLayerLayer = getLayerLayer & "title: '" & layerName & "'," & Chr(10)
        'add type 
        getLayerLayer = getLayerLayer & "geoType: '" & OL3Edit.layerType & "'," & Chr(10)
        'add name(variable name)
        getLayerLayer = getLayerLayer & "name: '" & "map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "'," & Chr(10)
        'add source
        getLayerLayer = getLayerLayer & "source: map" & mapNumber & "_vectorSource_" & Me.Cells.Item(0).RowIndex & Chr(10)
        'finish
        getLayerLayer = getLayerLayer & "});" & Chr(10) & Chr(10)




        getLayerLayer = getLayerLayer & "// '//////////get style index if there are multiple user switchable styles///////////////" & Chr(10)
        'is style picker valid dom element
        getLayerLayer = getLayerLayer & "var map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_currentSelectedStyle;" & Chr(10)
        getLayerLayer = getLayerLayer & "var map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_currentSelectedStyleIndex;" & Chr(10)
        getLayerLayer = getLayerLayer & "map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_currentSelectedStyle = document.getElementById('styleSelectmap" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "');" & Chr(10)
        'else call SwitchLayerDeclaration with dummy value 0 if no style selector - > 0 for first style (could also be set to default)
        getLayerLayer = getLayerLayer & "if (map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_currentSelectedStyle == null){" & Chr(10)
        getLayerLayer = getLayerLayer & "map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_currentSelectedStyleIndex = 0;" & Chr(10)
        'if it is call SwitchLayerDeclaration function with selected index
        getLayerLayer = getLayerLayer & "}else{" & Chr(10)
        getLayerLayer = getLayerLayer & "map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_currentSelectedStyle " & Chr(10)
        getLayerLayer = getLayerLayer & "}" & Chr(10) & Chr(10)


        'call layer declaration function
        getLayerLayer = getLayerLayer & " map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_SwitchLayerDeclaration(map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_currentSelectedStyleIndex);" & Chr(10) & Chr(10)



        'if a heatmap style is present then also include a function to switch layer type from heatmap to vector
        'have a differnt heatmap layer declaration for each heatmap style declaration

        'check for heatmaps -> if none in style list no need for heatmap layer declarations = skip
        ' If OL3Edit.OL3LayerStylePicker1.areHeatmapsPresentInAnyStyleRow = True Then
        Dim tempHeatmapUC As OL3LayerStyleHeatmap
        Dim tempStyleRow As styleRow

        'set up switcher function
        getLayerLayer = getLayerLayer & "function map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "_SwitchLayerDeclaration (styleID){" & Chr(10)
        'set flag .... this is altered when a layer has been applied, if not altered then regular vector layer is used
        getLayerLayer = getLayerLayer & "var theFlag = false ; " & Chr(10)
        'remove current layer from map...only if map exists
        getLayerLayer = getLayerLayer & "if (map" & mapNumber & " != null){" & Chr(10)
        getLayerLayer = getLayerLayer & "map" & mapNumber & ".removeLayer(map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & ");"
        getLayerLayer = getLayerLayer & "}" & Chr(10)


        'for each row check for heatmap , if so create a layer declaration
        For k As Integer = 0 To OL3Edit.OL3LayerStylePicker1.DataGridView1.Rows.Count - 1
            If OL3Edit.OL3LayerStylePicker1.isHeatmap(k) Then
                tempStyleRow = OL3Edit.OL3LayerStylePicker1.DataGridView1.Rows(k)
                tempHeatmapUC = tempStyleRow.StyleControl
                getLayerLayer = getLayerLayer & "if(styleID == " & k & "){" & Chr(10)

                'alter weight column to be between 0 and 1
                getLayerLayer = getLayerLayer & "var allfeatures = map" & mapNumber & "_vectorSource_" & Me.Cells.Item(0).RowIndex & ".getFeatures();"
                getLayerLayer = getLayerLayer & "var feature ;"
                getLayerLayer = getLayerLayer & "for (i = 0; i < allfeatures.length; i++) {"
                getLayerLayer = getLayerLayer & "feature = allfeatures[i];"
                getLayerLayer = getLayerLayer & "feature.set('" & tempHeatmapUC.ComboBox2.Text & "_weight', (feature.get('" & tempHeatmapUC.ComboBox2.Text & "') - " & tempHeatmapUC.adjustmentFactor & ")/" & tempHeatmapUC.adjustmentDivisor & " );"
                getLayerLayer = getLayerLayer & "}"


                'setup var
                getLayerLayer = getLayerLayer & " map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "  = new ol.layer.Heatmap({" & Chr(10)
                'add title (as shown in key)
                getLayerLayer = getLayerLayer & "title: '" & layerName & "'," & Chr(10)
                'add type 
                getLayerLayer = getLayerLayer & "geoType: '" & OL3Edit.layerType & "'," & Chr(10)
                'add name(variable name)
                getLayerLayer = getLayerLayer & "name: '" & "map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "'," & Chr(10)
                'add source
                getLayerLayer = getLayerLayer & "source: map" & mapNumber & "_vectorSource_" & Me.Cells.Item(0).RowIndex & "," & Chr(10)
                'add heatmap specific prperties
                'weight
                getLayerLayer = getLayerLayer & "weight:'" & tempHeatmapUC.ComboBox2.Text & "_weight'," & Chr(10)
                'blur
                getLayerLayer = getLayerLayer & "blur:" & tempHeatmapUC.olBlur & "," & Chr(10)
                'radius
                getLayerLayer = getLayerLayer & "radius:" & tempHeatmapUC.olRadius & "," & Chr(10)
                'gradient
                getLayerLayer = getLayerLayer & "gradient:[" & tempHeatmapUC.olGradient & "]," & Chr(10)
                'finish
                getLayerLayer = getLayerLayer & "});" & Chr(10)
                'set flag to true
                getLayerLayer = getLayerLayer & "theFlag = true;" & Chr(10)
                'end if
                getLayerLayer = getLayerLayer & "}" & Chr(10)

            End If


        Next

        'add condition for regular vector layer
        getLayerLayer = getLayerLayer & "if(theFlag == false){" & Chr(10)
        'setup var
        getLayerLayer = getLayerLayer & " map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "  = new ol.layer.Vector({" & Chr(10)
        'add title (as shown in key)
        getLayerLayer = getLayerLayer & "title: '" & layerName & "'," & Chr(10)
        'add type 
        getLayerLayer = getLayerLayer & "geoType: '" & OL3Edit.layerType & "'," & Chr(10)
        'add name(variable name)
        getLayerLayer = getLayerLayer & "name: '" & "map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & "'," & Chr(10)
        'add source
        getLayerLayer = getLayerLayer & "source: map" & mapNumber & "_vectorSource_" & Me.Cells.Item(0).RowIndex & Chr(10)
        'finish
        getLayerLayer = getLayerLayer & "});" & Chr(10)
        getLayerLayer = getLayerLayer & "}" & Chr(10) & Chr(10)


        're add layer to map
        getLayerLayer = getLayerLayer & "if (map" & mapNumber & " != null){" & Chr(10)
        getLayerLayer = getLayerLayer & "map" & mapNumber & ".addLayer(map" & mapNumber & "_vectorLayer_" & Me.Cells.Item(0).RowIndex & ");"
        getLayerLayer = getLayerLayer & "}" & Chr(10)
        'end function
        getLayerLayer = getLayerLayer & "}" & Chr(10) & Chr(10)
        'End If

    End Function

    Public Function getLayerProjectionDefinition() As String
        getLayerProjectionDefinition = ""
        'getLayerProjectionDefinition = "Proj4js.defs['EPSG:" & Me.Cells.Item(0).RowIndex & "'] = " & Chr(34) & OL3Edit.OL3Projections1.TextBox2.Text & Chr(34) & Chr(10)
        getLayerProjectionDefinition = "proj4.defs('USER:" & mapNumber & "00" & Me.Cells.Item(0).RowIndex & "'," & Chr(34) & OL3Edit.OL3Projections1.TextBox2.Text & Chr(34) & ");" & Chr(10)
    End Function




    Public Function save() As OL3LayerSaveObject
        save = New OL3LayerSaveObject
        save.layerID = layerID
        save.layerPath = OL3LayerPath
        save.MapNum = mapNumber
        save.layerPopups = OL3Edit.OL3LayerPopupPicker1.save()
        save.layerProjection = OL3Edit.OL3Projections1.save()
        save.layerStyles = OL3Edit.OL3LayerStylePicker1.save()
        save.layerFilters = OL3Edit.OlLayerFilterPicker1.save()
        save.layerGeneral = OL3Edit.OL3General1.save()


    End Function


    Public Sub loadObj(ByVal saveObj As OL3LayerSaveObject)

        OL3LayerPath = saveObj.layerPath
        mapNumber = saveObj.MapNum
        layerID = saveObj.layerID
        OL3Edit.OL3LayerPopupPicker1.loadObj(saveObj.layerPopups)
        OL3Edit.OL3Projections1.loadObj(saveObj.layerProjection)
        OL3Edit.OL3LayerStylePicker1.loadObj(saveObj.layerStyles)
        OL3Edit.OlLayerFilterPicker1.loadObj(saveObj.layerFilters)
        OL3Edit.OL3General1.loadObj(saveObj.layerGeneral)

    End Sub

End Class


<Serializable()> _
Public Class OL3LayerSaveObject
    Public layerID As Integer
    Public layerPath As String
    Public MapNum As Integer
    Public layerStyles As OL3LayerStyleObject
    Public layerPopups As OL3PopupObject
    Public layerProjection As OL3ProjectionSaveObject
    Public layerFilters As OL3FilterSaveObject
    Public layerGeneral As OL3GeneralSaveObject


End Class

<Serializable()> _
Public Class OL3LayerListSaveObject
    Public layerList As New List(Of OL3LayerSaveObject)
    Public mapOptions As OL3MapOptionsSaveObject
End Class