
Imports System.IO
Imports OSGeo.GDAL

Public Class OL3BasemapTiledRaster

    Public listOfAllImageFiles As New List(Of imagefile)
    Public listOfAllTiles As New List(Of tile)
    Public minX As Double
    Public minY As Double
    Public maxX As Double
    Public maxY As Double
    Public tilesize As Integer = 256
    Public outputpath As String = ""
    Public BMArray(3) As Bitmap
    Public scaleFactor As Double = 1
    Public scaleMultiplier As Double
    Public finalTileSize As Integer
    Public parentMapList As OL3Maps

    Public copiedFromMapID As String = ""
    Public listOfMapIDs As New List(Of String)

    Private _ZoomLevels As List(Of zoomLevel)
    Public Property ZoomLevels() As List(Of zoomLevel)
        Get
            Return _ZoomLevels
        End Get
        Set(ByVal value As List(Of zoomLevel))
            _ZoomLevels = value
        End Set

    End Property

    Sub New(ByVal theParent As OL3Maps)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ZoomLevels = New List(Of zoomLevel)
        parentMapList = theParent

    End Sub


    Private Sub TiledRaster_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        refreshZoomList()
        populateMapList()
    End Sub

    Sub populateMapList()
        Dim TR As OL3BasemapTiledRaster

        ToolStripComboBox1.Items.Clear()
        listOfMapIDs.Clear()

        ToolStripComboBox1.Items.Add("New")
        listOfMapIDs.Add("New")

        For n As Integer = 0 To parentMapList.mapList.Count - 1
            'is a control present
            If parentMapList.mapList(n).mapOptions.OL3Basemaps1.Panel1.Controls.Count > 0 Then
                '1. is a tms used ?
                If TypeOf parentMapList.mapList(n).mapOptions.OL3Basemaps1.Panel1.Controls(0) Is OL3BasemapTiledRaster And parentMapList.mapList(n).mapOptions.OL3Basemaps1.Panel1.Controls(0) IsNot Me Then
                    TR = parentMapList.mapList(n).mapOptions.OL3Basemaps1.Panel1.Controls(0)
                    '2. is it bounced off another TMS ?
                    If TR.ToolStripComboBox1.SelectedIndex = 0 Then
                        ToolStripComboBox1.Items.Add("map " & parentMapList.mapList(n).mapNumber)
                        listOfMapIDs.Add(parentMapList.mapList(n).mapID)
                    End If

                End If
            End If
        Next

        'check if previous map still exists and if so select it, else select new TMS
        checkCopyFromMap("New")

    End Sub

    Sub checkCopyFromMap(Optional ByVal setTo As String = "OSM")
        If copiedFromMapID = "New" Then Exit Sub

        Dim found As Boolean = False
        'cycle through other maps and check a chosen one still exists - if not set to OSM or "new"
        For n As Integer = 0 To parentMapList.mapList.Count - 1
            If parentMapList.mapList(n).mapID = copiedFromMapID Then
                found = True
                Continue For
            End If
        Next

        Dim mapOpt As OL3MapOptions = Me.ParentForm

        If found = False Then
            If setTo = "OSM" Then
                mapOpt.OL3Basemaps1.TreeView1.SelectedNode = mapOpt.OL3Basemaps1.TreeView1.Nodes("Web based files").Nodes("OpenStreetMap")
            Else
                ToolStripComboBox1.SelectedIndex = 0
            End If
        End If

    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        addZoomLevel()
    End Sub

    Sub addZoomLevel()
        ZoomLevels.Add(New zoomLevel)
        ZoomLevels(ZoomLevels.Count - 1).Zoom = ZoomLevels.Count
        ListBox1.Items.Add(ZoomLevels.Count)
    End Sub

    Sub refreshZoomList()
        ListBox1.Items.Clear()
        For x As Integer = 0 To ZoomLevels.Count - 1
            ListBox1.Items.Add(ZoomLevels(x).Zoom)
        Next

    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        addRaster()
    End Sub

    Sub addRaster()

        'is a zoom level selected
        If ListBox1.SelectedIndex = -1 Then
            MsgBox("Please select a zoom level first")
            Exit Sub
        End If

        Dim theZL As zoomLevel
        Dim tempImage As New imagefile
        Dim tempRes As Double
        Dim OL3tempRes As Double
        Dim tempBMP As Bitmap
        Dim gdalclass As New GDALImport
        Dim coordList As List(Of String)

        Dim ofd As New OpenFileDialog
        If ofd.ShowDialog = DialogResult.OK Then
            theZL = ZoomLevels(ListBox1.SelectedIndex)

            'add file name to image class
            tempImage.filePath = ofd.FileName

            'is there georef properties for the image (use gdal), if so place in image property (minx/y maxx/y)
            coordList = gdalclass.getCoords(tempImage.filePath)
            tempImage.minX = CDbl(coordList(0).Split(",")(0))
            tempImage.minY = CDbl(coordList(1).Split(",")(1))
            tempImage.maxX = CDbl(coordList(2).Split(",")(0))
            tempImage.maxY = CDbl(coordList(2).Split(",")(1))
            tempImage.BBOX = New Rectangle(tempImage.minX, tempImage.minY, tempImage.maxX - tempImage.minX, tempImage.maxY - tempImage.minY)

            'determine image resolution
            tempBMP = New Bitmap(tempImage.filePath)
            tempRes = tempBMP.Width / (tempImage.maxX - tempImage.minX)
            OL3tempRes = (tempImage.maxX - tempImage.minX) / tempBMP.Width
            tempBMP.Dispose()

            'if there are images currently in zoom level check resolution is the same (pixels per unit)
            If theZL.Images.Count > 0 Then
                'get new image resolution and get last image resolution
                'check floored int (allows minor deviations due to rounding)
                If theZL.Resolution = tempRes Then
                    'ok

                Else
                    'not ok
                    MsgBox("Image resolutions (px per unit) does not match other images in this zoom level")
                    Exit Sub
                End If
            Else
                'first image in zoom
                theZL.Resolution = tempRes
                theZL.OL3Resolution = OL3tempRes
            End If

            'add imagefile name to zoom level
            theZL.Images.Add(tempImage)
            refreshRasterList()
        End If

    End Sub

    Sub refreshRasterList()
        ListBox2.Items.Clear()

        'check zoom level is selected
        If ListBox1.SelectedIndex = -1 Then
            MsgBox("Please select a zoom level first")
            Exit Sub
        End If

        For c As Integer = 0 To ZoomLevels(ListBox1.SelectedIndex).Images.Count - 1
            ListBox2.Items.Add(Path.GetFileNameWithoutExtension(ZoomLevels(ListBox1.SelectedIndex).Images(c).filePath))
        Next


    End Sub

    Sub showRasterProperties()
        PropertyGrid1.SelectedObject = ZoomLevels(ListBox1.SelectedIndex).Images(ListBox2.SelectedIndex)
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        If ListBox1.SelectedIndex > -1 Then
            refreshRasterList()
        End If


    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        If ListBox2.SelectedIndex > -1 Then
            showRasterProperties()
        End If
    End Sub






    '************************Tiling functions********************************

    Sub createAllTileLevels(ByVal outputFile As String, ByVal mapNum As Integer, ByVal outName As String)
        outputpath = outputFile 'outputfile is directory

        'create tiles folder
        If Not Directory.Exists(outputpath & "\" & outName) Then
            System.IO.Directory.CreateDirectory(outputpath & "\" & outName)
        End If

        'create map folder
        If Not Directory.Exists(outputpath & "\" & outName & "\" & mapNum) Then
            System.IO.Directory.CreateDirectory(outputpath & "\" & outName & "\" & mapNum)
        End If

        outputpath = outputpath & "\" & outName & "\" & mapNum & "\"

        'for each zoom level
        For v As Integer = 0 To ZoomLevels.Count - 1
            'these functions tile the images based on top left (google style, NOT TMS). to convert in JS place origin at maxY of Tile extent and invert y (-y)
            listOfAllImageFiles = ZoomLevels(v).Images
            createTileList(v)
            tileUp2()
        Next
    End Sub



    Sub getMaxX()
        If listOfAllImageFiles.Count > 1 Then
            maxX = Math.Max(listOfAllImageFiles(0).maxX, listOfAllImageFiles(1).maxX)

            For d As Integer = 0 To listOfAllImageFiles.Count - 1
                maxX = Math.Max(maxX, listOfAllImageFiles(d).maxX)
            Next
        Else
            maxX = listOfAllImageFiles(0).maxX
        End If
    End Sub

    Sub getMinX()
        If listOfAllImageFiles.Count > 1 Then
            minX = Math.Min(listOfAllImageFiles(0).minX, listOfAllImageFiles(1).minX)

            For d As Integer = 0 To listOfAllImageFiles.Count - 1
                minX = Math.Min(minX, listOfAllImageFiles(d).minX)
            Next
        Else
            minX = listOfAllImageFiles(0).minX
        End If
    End Sub

    Sub getMaxY()
        If listOfAllImageFiles.Count > 1 Then
            maxY = Math.Max(listOfAllImageFiles(0).maxY, listOfAllImageFiles(1).maxY)

            For d As Integer = 0 To listOfAllImageFiles.Count - 1
                maxY = Math.Max(maxY, listOfAllImageFiles(d).maxY)
            Next
        Else
            maxY = listOfAllImageFiles(0).maxY
        End If
    End Sub

    Sub getMinY()
        If listOfAllImageFiles.Count > 1 Then
            minY = Math.Min(listOfAllImageFiles(0).minY, listOfAllImageFiles(1).minY)

            For d As Integer = 0 To listOfAllImageFiles.Count - 1
                minY = Math.Min(minY, listOfAllImageFiles(d).minY)
            Next
        Else
            minY = listOfAllImageFiles(0).minY
        End If
    End Sub


    Sub setMinMaxofImages(ByVal zlevel As Integer)

        getMaxX()
        getMinX()
        getMaxY()
        getMinY()

        ZoomLevels(zlevel).minX = minX
        ZoomLevels(zlevel).minY = minY
        ZoomLevels(zlevel).maxX = maxX
        ZoomLevels(zlevel).maxY = maxY
    End Sub

    Sub resetBMArray()
        For h As Integer = 0 To 3
            If BMArray(h) Is Nothing Then
            Else
                BMArray(h).Dispose()
                BMArray(h) = Nothing
            End If
        Next
    End Sub

    Sub createTileList(ByVal zLevel As Integer)

        listOfAllTiles = New List(Of tile)

        finalTileSize = tilesize
        tilesize = tilesize * scaleFactor
        scaleMultiplier = 1 / scaleFactor


        setMinMaxofImages(zLevel)
        Dim tempTile As tile
        Dim tempRect As Rectangle

        For x As Integer = 0 To (((maxX - minX) * ZoomLevels(zLevel).Resolution) / tilesize)  'For x As Integer = 0 To ((maxX - minX) / tilesize) - 1
            For y As Integer = 0 To (((maxY - minY) * ZoomLevels(zLevel).Resolution) / tilesize)  'For y As Integer = 0 To ((maxY - minY) / tilesize)
                tempTile = New tile
                tempTile.minX = (x * tilesize / ZoomLevels(zLevel).Resolution) + minX
                tempTile.maxX = ((x + 1) * tilesize / ZoomLevels(zLevel).Resolution) + minX
                tempTile.minY = (y * tilesize / ZoomLevels(zLevel).Resolution) + minY
                tempTile.maxY = ((y + 1) * tilesize / ZoomLevels(zLevel).Resolution) + minY

                tempTile.boundingRect = New Rectangle(tempTile.minX, tempTile.minY, tilesize / ZoomLevels(zLevel).Resolution, tilesize / ZoomLevels(zLevel).Resolution)

                tempTile.x = x
                tempTile.y = y + 1
                'tempTile.y = Math.Floor(((maxY - minY) * TiledRaster1.ZoomLevels(zLevel).Resolution) / tilesize) - y - 1
                tempTile.z = ZoomLevels.Count - zLevel
                tempTile.resolution = ZoomLevels(zLevel).Resolution

                tempTile.outputFileName = outputpath & (tempTile.z - 1).ToString & "_" & x.ToString & "_" & tempTile.y & ".jpg"

                tempTile.intersectingImages = New List(Of imagefile)
                For i As Integer = 0 To listOfAllImageFiles.Count - 1
                    tempRect = doesImageIntersect(listOfAllImageFiles(i).BBOX, tempTile.boundingRect)
                    If tempRect <> Rectangle.Empty Then
                        tempTile.intersectingImages.Add(listOfAllImageFiles(i))
                        tempTile.intersections.Add(tempRect)
                    End If
                Next

                listOfAllTiles.Add(tempTile)
            Next
        Next


    End Sub


    Function doesImageIntersect(ByVal rect1 As Rectangle, ByVal rect2 As Rectangle) As Rectangle
        If Rectangle.Intersect(rect1, rect2) = Rectangle.Empty Then
            Return Rectangle.Empty
        Else
            Return Rectangle.Intersect(rect1, rect2)
        End If
    End Function



    'the original "tileup" took all the images in the list and stiched them together to form one large bitmap from which it cycled through cropping out tile sized chunks.  this was inefficant as the large bitmap soon reached memory limitation.
    'Tileup2 creates a list of tiles needed and the source image files which intersect the tile bounding box.  it only loads the images required when creating each tile, so doesn't have the mem limitations of the original "tileup", however due to multiple loading of the same images it IS much slower.
    'ideally GDAL would be used for this, however "GDAL2Tiles" is a script and cannot be run as a .net method, therefore the complexatly of using GDAL for this is way beyond my very limited ability.(hence the crappy .net solution) 

    'this could be speeded up by taking the intersecting images from one tile, then finding all other tiles which require the exact same intersecting images and proccessing them before discarding the image files.
    Sub tileUp2()
        Dim tempIntersect As Rectangle
        Dim imageSourceRect As Rectangle
        Dim CropImage As Bitmap
        Dim originalImage As Image
        Dim combinedImage As Bitmap
        Dim tempRes As Double

        'is TMS copy from other map active, if so skip
        If ToolStripComboBox1.SelectedIndex > 0 Then
            Exit Sub
        End If

        For t As Integer = 0 To listOfAllTiles.Count - 1

            'does image curently exist ?
            If File.Exists(listOfAllTiles(t).outputFileName) Then
                'if file exists and "override" is turned off, do not write image 
                '(this reduces the proccing time considerably if someone is tweaking the map) 
                If Not ToolStripButton1.Checked Then
                    Continue For
                End If

            End If

            resetBMArray()
            For i As Integer = 0 To listOfAllTiles(t).intersectingImages.Count - 1

                tempIntersect = Rectangle.Intersect(listOfAllTiles(t).intersectingImages(i).BBOX, listOfAllTiles(t).boundingRect)
                'this is the intersct of the map units not the image pixels
                ' tempIntersect = New Rectangle(tempIntersect.X - listOfAllTiles(t).intersectingImages(i).minX, tempIntersect.Y - listOfAllTiles(t).intersectingImages(i).minY, tempIntersect.Width, tempIntersect.Height)

                'create rectangle of image crop
                'XXXXXXX
                tempRes = listOfAllTiles(t).resolution
                imageSourceRect = New Rectangle((tempIntersect.X - listOfAllTiles(t).intersectingImages(i).minX) * listOfAllTiles(t).resolution, (tempIntersect.Y - listOfAllTiles(t).intersectingImages(i).minY) * listOfAllTiles(t).resolution, Math.Floor(tempIntersect.Width * listOfAllTiles(t).resolution), Math.Floor(tempIntersect.Height * listOfAllTiles(t).resolution))
                tempIntersect = imageSourceRect
                'XXXXXXX


                CropImage = New Bitmap(tempIntersect.Width, tempIntersect.Height)
                originalImage = Image.FromFile(listOfAllTiles(t).intersectingImages(i).filePath)
                Using grp = Graphics.FromImage(CropImage)
                    grp.Clear(Color.White)
                    grp.DrawImage(originalImage, New Rectangle(0, 0, tempIntersect.Width, tempIntersect.Height), tempIntersect, GraphicsUnit.Pixel)

                    BMArray(i) = CropImage
                    'correct crops = yes
                    'BMArray(i).Save(listOfAllTiles(t).outputFileName & "_" & i, System.Drawing.Imaging.ImageFormat.Jpeg)
                End Using
                originalImage.Dispose()
            Next

            ' GC.WaitForFullGCComplete()
            'combine bitmaps onto blanck canvaas
            combinedImage = New Bitmap(finalTileSize, finalTileSize)

            Using canvasGraphic As Graphics = Graphics.FromImage(combinedImage)
                canvasGraphic.Clear(Color.White)
                For u As Integer = 0 To 3
                    If BMArray(u) Is Nothing Then
                    Else
                        canvasGraphic.DrawImage(BMArray(u), CInt((listOfAllTiles(t).intersections(u).X - listOfAllTiles(t).minX) * scaleMultiplier * tempRes), CInt((listOfAllTiles(t).intersections(u).Y - listOfAllTiles(t).minY) * scaleMultiplier * tempRes), CInt(listOfAllTiles(t).intersections(u).Width * scaleMultiplier * tempRes), CInt(listOfAllTiles(t).intersections(u).Height * scaleMultiplier * tempRes))
                    End If
                Next

                'canvasGraphic.ScaleTransform(1 / scaleFactor, 1 / scaleFactor)
            End Using

            combinedImage.Save(listOfAllTiles(t).outputFileName, System.Drawing.Imaging.ImageFormat.Jpeg)
            'End If

            'release image from mem
            originalImage.Dispose()
            CropImage.Dispose()
            combinedImage.Dispose()
        Next

    End Sub

    Function getOriginsJSArray() As String
        getOriginsJSArray = ""
        For v As Integer = 0 To ZoomLevels.Count - 1
            getOriginsJSArray = getOriginsJSArray & ",[" & ZoomLevels(v).minX & ", " & ZoomLevels(v).maxY & "]"

        Next

        Return "[" & getOriginsJSArray.Substring(1) & "],"
    End Function

    Function getResolutionsJSArray() As String
        getResolutionsJSArray = ""
        For v As Integer = 0 To ZoomLevels.Count - 1
            getResolutionsJSArray = getResolutionsJSArray & "," & ZoomLevels(v).OL3Resolution

        Next

        Return "[" & getResolutionsJSArray.Substring(1) & "],"
    End Function

    Function getBasemapJS(ByVal outputFolder As String, ByVal mapNumber As Integer, ByVal outName As String) As String

        'is TMS copy from other map active, if so skip
        If ToolStripComboBox1.SelectedIndex > 0 Then
            'get this function from chosen map
            For n As Integer = 0 To parentMapList.mapList.Count
                If parentMapList.mapList(n).mapID = copiedFromMapID Then
                    Dim TR As OL3BasemapTiledRaster = parentMapList.mapList(n).mapOptions.OL3Basemaps1.Panel1.Controls(0)
                    Return TR.getBasemapJS(outputFolder, parentMapList.mapList(n).mapNumber, outName)

                End If
            Next

            'then exit function 

        End If

        'outputFolder = outputFolder.Replace("\", "/")
        outputFolder = outName & "/" & mapNumber & "/"
        getBasemapJS = ""

        getBasemapJS = getBasemapJS & "new ol.layer.Tile({" & Chr(10)
        getBasemapJS = getBasemapJS & " source: new ol.source.TileImage({" & Chr(10)
        getBasemapJS = getBasemapJS & " tileGrid: new ol.tilegrid.TileGrid({" & Chr(10)
        getBasemapJS = getBasemapJS & "  origins:" & getOriginsJSArray() & Chr(10)
        getBasemapJS = getBasemapJS & "  resolutions:" & getResolutionsJSArray() & Chr(10)
        getBasemapJS = getBasemapJS & " })," & Chr(10)

        getBasemapJS = getBasemapJS & " tileUrlFunction: function(coordinate) {" & Chr(10)
        getBasemapJS = getBasemapJS & "  if (coordinate === null) return undefined;" & Chr(10)

        getBasemapJS = getBasemapJS & "  var z = coordinate[0];" & Chr(10)
        getBasemapJS = getBasemapJS & "  var x = coordinate[1];" & Chr(10)
        getBasemapJS = getBasemapJS & "  var y = coordinate[2];" & Chr(10)
        getBasemapJS = getBasemapJS & "  //NOTE: NOT TMS - hence maxY is used for origin above and 'y' below is inverted" & Chr(10)

        getBasemapJS = getBasemapJS & "  var url = '" & outputFolder & "' + z + '_'+ x +'_'+ -y +'.jpg';" & Chr(10)
        getBasemapJS = getBasemapJS & "  return url;" & Chr(10)
        getBasemapJS = getBasemapJS & " }" & Chr(10)
        getBasemapJS = getBasemapJS & "})" & Chr(10)
        getBasemapJS = getBasemapJS & "});" & Chr(10) & Chr(10)


    End Function




    Private Sub mapListChanged() Handles ToolStripComboBox1.SelectedIndexChanged
        copiedFromMapID = listOfMapIDs(ToolStripComboBox1.SelectedIndex)

        If ToolStripComboBox1.SelectedIndex = 0 Then 'if "New"
            GroupBox1.Visible = True
        Else
            GroupBox1.Visible = False
        End If
    End Sub
End Class



Public Class zoomLevel


    Sub New()

    End Sub

    Private _Zoom As Integer
    Public Property Zoom() As Integer
        Get
            Return _Zoom
        End Get
        Set(ByVal value As Integer)
            _Zoom = value

        End Set
    End Property

    Private _Resolution As Double
    Public Property Resolution() As Double
        Get
            Return _Resolution
        End Get
        Set(ByVal value As Double)
            _Resolution = value
        End Set
    End Property

    Private _OL3Resolution As Double
    Public Property OL3Resolution() As Double
        Get
            Return _OL3Resolution
        End Get
        Set(ByVal value As Double)
            _OL3Resolution = value
        End Set
    End Property

    Private _Images As New List(Of imagefile)
    Public Property Images() As List(Of imagefile)
        Get
            Return _Images
        End Get
        Set(ByVal value As List(Of imagefile))
            _Images = value
        End Set
    End Property




    Private _minX As Double
    Public Property minX() As Double
        Get
            Return _minX
        End Get
        Set(ByVal value As Double)
            _minX = value
        End Set
    End Property

    Private _maxX As Double
    Public Property maxX() As Double
        Get
            Return _maxX
        End Get
        Set(ByVal value As Double)
            _maxX = value
        End Set
    End Property

    Private _minY As Double
    Public Property minY() As Double
        Get
            Return _minY
        End Get
        Set(ByVal value As Double)
            _minY = value
        End Set
    End Property

    Private _maxY As Double
    Public Property maxY() As Double
        Get
            Return _maxY
        End Get
        Set(ByVal value As Double)
            _maxY = value
        End Set
    End Property
End Class



Public Class imagefile


    Private _minX As Double
    Public Property minX() As Double
        Get
            Return _minX
        End Get
        Set(ByVal value As Double)
            _minX = value
        End Set
    End Property

    Private _maxX As Double
    Public Property maxX() As Double
        Get
            Return _maxX
        End Get
        Set(ByVal value As Double)
            _maxX = value
        End Set
    End Property

    Private _minY As Double
    Public Property minY() As Double
        Get
            Return _minY
        End Get
        Set(ByVal value As Double)
            _minY = value
        End Set
    End Property

    Private _maxY As Double
    Public Property maxY() As Double
        Get
            Return _maxY
        End Get
        Set(ByVal value As Double)
            _maxY = value
        End Set
    End Property


    Private _filePath As String
    Public Property filePath() As String
        Get
            Return _filePath
        End Get
        Set(ByVal value As String)
            _filePath = value
        End Set
    End Property


    Public BBOX As Rectangle

End Class

Public Class tile
    Public minX As Double
    Public minY As Double
    Public maxX As Double
    Public maxY As Double

    Public x As Integer
    Public y As Integer
    Public z As Integer

    Public insX As Integer
    Public insY As Integer

    Public boundingRect As Rectangle

    Public outputFileName As String

    Public intersectingImages As List(Of imagefile)
    Public intersections As New List(Of Rectangle)

    Public resolution As Double
End Class




