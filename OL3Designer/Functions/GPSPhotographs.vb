Imports System.IO

Public Class GPSPhotographs

    Function isGPS(ByVal photo As Bitmap, ByVal filename As String) As Boolean
        Dim byte_property_id As Byte()
        If photo.PropertyIdList.Length = 0 Then Return False

        On Error GoTo err
        byte_property_id = photo.GetPropertyItem(2).Value
        On Error GoTo 0

        'Latitude degrees minutes and seconds (rational)
        Dim _1 As Double = System.BitConverter.ToInt32(byte_property_id, 0) / System.BitConverter.ToInt32(byte_property_id, 4)
        Dim _2 As Double = System.BitConverter.ToInt32(byte_property_id, 8) / System.BitConverter.ToInt32(byte_property_id, 12)
        Dim _3 As Double = System.BitConverter.ToInt32(byte_property_id, 16) / System.BitConverter.ToInt32(byte_property_id, 20)

        If _1 + _2 + _3 > 0 Then
            Return True
        Else

            Return False
        End If
err:
        Return False
    End Function

    Function imageInfo(ByVal thePhoto As Bitmap, ByVal filepath As String) As Photo
        imageInfo = New Photo

        Dim byte_property_id As Byte()
        Dim ascii_string_property_id As String
        Dim prop_type As String
        Dim selected_image As System.Drawing.Bitmap
        Dim property_ids() As Integer
        Dim scan_property As Integer
        Dim counter As Integer
        Dim degrees As Double
        Dim minutes As Double
        Dim seconds As Double

        selected_image = thePhoto
        property_ids = selected_image.PropertyIdList

        imageInfo.filename = Path.GetFileNameWithoutExtension(filepath)
        imageInfo.filepath = filepath
        imageInfo.localX = 0
        imageInfo.localY = 0

        On Error Resume Next

        For Each scan_property In property_ids
            counter = counter + 1
            byte_property_id = selected_image.GetPropertyItem(scan_property).Value
            prop_type = selected_image.GetPropertyItem(scan_property).Type
            'MsgBox(scan_property.ToString)
            If scan_property = 1 Then
                'Latitude North or South
                ascii_string_property_id = System.Text.Encoding.ASCII.GetString(byte_property_id)
                imageInfo.NS = ascii_string_property_id


            ElseIf scan_property = 2 Then
                'Latitude degrees minutes and seconds (rational)
                degrees = System.BitConverter.ToInt32(byte_property_id, 0) / System.BitConverter.ToInt32(byte_property_id, 4)
                minutes = System.BitConverter.ToInt32(byte_property_id, 8) / System.BitConverter.ToInt32(byte_property_id, 12)
                seconds = System.BitConverter.ToInt32(byte_property_id, 16) / System.BitConverter.ToInt32(byte_property_id, 20)

                imageInfo.dlat = degrees + (minutes / 60) + (seconds / 3600)
                imageInfo.lat = degrees & " " & minutes & " " & seconds

                If Asc(imageInfo.NS) = 83 Then imageInfo.dlat = (imageInfo.dlat * -1)

            ElseIf scan_property = 3 Then
                'Longitude East or West
                ascii_string_property_id = System.Text.Encoding.ASCII.GetString(byte_property_id)
                imageInfo.EW = ascii_string_property_id


            ElseIf scan_property = 4 Then
                'Longitude degrees minutes and seconds (rational)
                degrees = System.BitConverter.ToInt32(byte_property_id, 0) / System.BitConverter.ToInt32(byte_property_id, 4)
                minutes = System.BitConverter.ToInt32(byte_property_id, 8) / System.BitConverter.ToInt32(byte_property_id, 12)
                seconds = System.BitConverter.ToInt32(byte_property_id, 16) / System.BitConverter.ToInt32(byte_property_id, 20)

                imageInfo.dlon = degrees + (minutes / 60) + (seconds / 3600)
                imageInfo.lon = degrees & " " & minutes & " " & seconds

                If Asc(imageInfo.EW) = 87 Then
                    imageInfo.dlon = (imageInfo.dlon * -1)
                End If
                'MsgBox(Asc(imageInfo.EW))
                'ElseIf scan_property = 18 Then
                ' 'Datum used at GPS acquisition (ascii)
                '   ascii_string_property_id = System.Text.Encoding.ASCII.GetString(byte_property_id)
                '  MsgBox("GPS Datum= " & ascii_string_property_id)

            ElseIf scan_property = 17 Then '24?
                imageInfo.picDirection = System.BitConverter.ToInt32(byte_property_id, 0) / System.BitConverter.ToInt32(byte_property_id, 4)

            ElseIf scan_property = 306 Then 'GetPropertyItem(&H132)
                'datetime
                Dim sdate As String = System.Text.Encoding.UTF8.GetString(byte_property_id).Trim()
                Dim secondhalf As String = sdate.Substring(sdate.IndexOf(" "), (sdate.Length - sdate.IndexOf(" ")))
                Dim firsthalf As String = sdate.Substring(0, 10)
                firsthalf = firsthalf.Replace(":", "-")
                sdate = firsthalf & secondhalf
                imageInfo.picDate = DateTime.Parse(sdate)


            ElseIf scan_property = 28 Then 'description of area
                Dim descString As String = ""
                For k As Integer = 0 To ((byte_property_id.Length) / 2) - 2
                    descString = descString & Chr(BitConverter.ToInt16(byte_property_id, k * 2))
                Next
                imageInfo.picDescription = descString.ToString.Replace(Chr(34), "")
                ' imageInfo.picDescription = System.Text.Encoding.ASCII.GetString(byte_property_id)


            ElseIf scan_property = 6 Then 'altitude
                imageInfo.alt = System.BitConverter.ToInt32(byte_property_id, 0)


            End If

        Next

        On Error GoTo 0


    End Function


    Function getGPSFiles() As List(Of Photo)

        getGPSFiles = New List(Of Photo)

        'set file open dialog
        Dim OFD As New OpenFileDialog
        Dim thePhoto As Bitmap
        Dim count As Integer = 1
        OFD.Filter = "Image Files (*.jpg)|*.jpg|All Files (*.*)|*.*"
        OFD.FilterIndex = 1
        OFD.Title = "Open Image File/s"
        OFD.Multiselect = True

        If OFD.ShowDialog = Windows.Forms.DialogResult.OK Then

            'split filesnames out
            Dim allFiles() As String
            allFiles = OFD.FileNames

            For Each item As String In allFiles

                'check for gps info
                If isGPS(Bitmap.FromFile(item), item) Then


                    'get info and place into photo class > put in list
                    thePhoto = Bitmap.FromFile(item)
                    getGPSFiles.Add(imageInfo(thePhoto, item))
                    thePhoto.Dispose()

                    count = count + 1
                Else

                    count = count + 1
                End If
            Next
        Else
            Exit Function
        End If

    End Function

    Function createGeoJsonFromPhotoList(ByVal thePhotoList As List(Of Photo)) As String
        'function creates geojson from list of gps photos
        createGeoJsonFromPhotoList = ""
        Dim allFeatures As String = ""

        Dim geojsonStart As String = "{'type':'FeatureCollection', 'crs': { 'type': 'name','properties': {'name': 'EPSG:4326'}},'features':["
        Dim geojsonEnd As String = "]}"

        For x As Integer = 0 To thePhotoList.Count - 1
            thePhotoList(x).featureGeojson = "{'type':'Feature','geometry':{ 'type': 'Point', 'coordinates': [ " & thePhotoList(x).dlon & ", " & thePhotoList(x).dlat & " ] },'properties': {'x':'" & thePhotoList(x).dlon & "', 'y':'" & thePhotoList(x).dlat & "','Filename':'" & thePhotoList(x).filename & "', 'FilePath':'" & thePhotoList(x).filepath.Replace("\", "/") & "', 'Direction':'" & thePhotoList(x).picDirection & "','Altitude':'" & thePhotoList(x).alt & "','Date':'" & thePhotoList(x).picDate & "'}}"
            allFeatures = allFeatures & "," & thePhotoList(x).featureGeojson
        Next

        If thePhotoList.Count > 0 Then
            createGeoJsonFromPhotoList = geojsonStart & allFeatures.Substring(1) & geojsonEnd
        Else : Return "none"
        End If

        createGeoJsonFromPhotoList = createGeoJsonFromPhotoList.Replace("'", Chr(34))

    End Function

    Sub importGPSPhotos(ByVal theLayerList As OL3LayerList)

        'get folder of photos 
        Dim allPhotos As List(Of Photo) = getGPSFiles()

        'create geojson
        Dim GPSGeoJson As String = createGeoJsonFromPhotoList(allPhotos)
        If GPSGeoJson = "none" Then Exit Sub

        'save geojson to filesystem
        Dim SFD As New SaveFileDialog
        SFD.DefaultExt = "GeoJson"
        SFD.AddExtension = True
        'SFD.FileName = "GPSPhotos.geojson"
        If SFD.ShowDialog <> DialogResult.OK Then Exit Sub
        File.WriteAllText(SFD.FileName, GPSGeoJson)


        'add layer will saved filepath
        theLayerList.add(SFD.FileName)

        'set style to cluster
        Dim theLayer As OLLayer = theLayerList.DataGridView1.Rows(theLayerList.DataGridView1.Rows.Count - 1)
        theLayer.OL3Edit.OL3LayerStylePicker1.TreeView1.SelectedNode = theLayer.OL3Edit.OL3LayerStylePicker1.TreeView1.Nodes(3).Nodes(0)

        'set pop ups to photo
        theLayer.OL3Edit.OL3LayerPopupPicker1.TreeView1.SelectedNode = theLayer.OL3Edit.OL3LayerPopupPicker1.TreeView1.Nodes(0)
        theLayer.OL3Edit.OL3LayerPopupPicker1.popupTypeSelected()
        Dim popupPicker As OL3PopupDesignerSimple = theLayer.OL3Edit.OL3LayerPopupPicker1.Panel1.Controls(0)
        popupPicker.layerPath = SFD.FileName

        popupPicker.init()
        popupPicker.addAllFields()
        popupPicker.DataGridView1.Rows(3).Cells(1).Value = "Image"
        popupPicker.refreshPreview()

    End Sub

End Class

Public Class Photo

    Public featureGeojson As String = ""
    'all properties extracted per photo
    Private _dlat As Double
    Public Property dlat As Double
        Get
            Return _dlat
        End Get
        Set(ByVal value As Double)
            _dlat = value
        End Set
    End Property

    Private _lat As String
    Public Property lat As String
        Get
            Return _lat
        End Get
        Set(ByVal value As String)
            _lat = value
        End Set
    End Property

    Private _NS As String
    Public Property NS As String
        Get
            Return _NS
        End Get
        Set(ByVal value As String)
            _NS = value
        End Set
    End Property

    Private _EW As String
    Public Property EW As String
        Get
            Return _EW
        End Get
        Set(ByVal value As String)
            _EW = value
        End Set
    End Property

    Private _lon As String
    Public Property lon As String
        Get
            Return _lon
        End Get
        Set(ByVal value As String)
            _lon = value
        End Set
    End Property

    Private _dlon As Double
    Public Property dlon As Double
        Get
            Return _dlon
        End Get
        Set(ByVal value As Double)
            _dlon = value
        End Set
    End Property

    Private _localX As String
    Public Property localX As String
        Get
            Return _localX
        End Get
        Set(ByVal value As String)
            _localX = value
        End Set
    End Property

    Private _localY As String
    Public Property localY As String
        Get
            Return _localY
        End Get
        Set(ByVal value As String)
            _localY = value
        End Set
    End Property

    Private _filename As String
    Public Property filename As String
        Get
            Return _filename
        End Get
        Set(ByVal value As String)
            _filename = value
        End Set
    End Property

    Private _filepath As String
    Public Property filepath As String
        Get
            Return _filepath
        End Get
        Set(ByVal value As String)
            _filepath = value
        End Set
    End Property

    Private _picDate As Date
    Public Property picDate As Date
        Get
            Return _picDate
        End Get
        Set(ByVal value As Date)
            _picDate = value
        End Set
    End Property

    Private _picDirection As Double
    Public Property picDirection As Double
        Get
            Return _picDirection
        End Get
        Set(ByVal value As Double)
            _picDirection = value
        End Set
    End Property

    Private _picDescription As String
    Public Property picDescription As String
        Get
            Return _picDescription
        End Get
        Set(ByVal value As String)
            _picDescription = value
        End Set
    End Property

    Private _alt As Double
    Public Property alt As Double
        Get
            Return _alt
        End Get
        Set(ByVal value As Double)
            _alt = value
        End Set
    End Property


End Class
