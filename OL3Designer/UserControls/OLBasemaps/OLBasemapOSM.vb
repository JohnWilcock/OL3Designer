Public Class OLBasemapOSM

    Public OSMImageList As New ImageList
    Public UrlList As List(Of String)
    Public UrlTitle As List(Of String)
    Dim croppedImageSize As Integer = 80

    '/15/6219/12958.jpg
    Dim prevX As Long = 6219 '10
    Dim prevY As Long = 12958 '22
    Dim prevZ As Long = 15 '6

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        OSMImageList.ImageSize = New System.Drawing.Size(croppedImageSize, croppedImageSize)

        setUrlStringList()
        getOSMImageList()


    End Sub

    Function getBasemapJS() As String


        Select Case ListView1.SelectedItems(0).Text
            Case "MapQuest"
                Return "new ol.layer.Tile({style:'Road',source: new ol.source.MapQuest({layer: 'osm'})})"
            Case "MapQuest Aerial"
                Return "new ol.layer.Tile({style:'Aerial',source: new ol.source.MapQuest({layer: 'sat'})});"
            Case "MapQuest Hybrid"
                Return "new ol.layer.Tile({style: 'AerialWithLabels', source: new ol.source.MapQuest({layer: 'hyb'})});"
            Case "Open Steet Map"
                Return "new ol.layer.Tile({source: new ol.source.OSM()});"
            Case "Staman Watercolour"
                Return "new ol.layer.Tile({style:'AerialWithLabels',source: new  ol.source.Stamen({layer: 'watercolor'})});"
            Case Else
                Return "new ol.layer.Tile({style:'Road',source: new ol.source.MapQuest({layer: 'osm'})})"
        End Select


    End Function

    Sub setUrlStringList()
        UrlList = New List(Of String)
        UrlTitle = New List(Of String)

        UrlTitle.Add("MapQuest")
        UrlList.Add("http://otile1.mqcdn.com/tiles/1.0.0/map/" & prevZ & "/" & prevX & "/" & prevY & ".png")
        UrlTitle.Add("MapQuest Aerial")
        UrlList.Add("http://otile2.mqcdn.com/tiles/1.0.0/sat/" & prevZ & "/" & prevX & "/" & prevY & ".png")
        UrlTitle.Add("MapQuest Hybrid")
        UrlList.Add("http://otile2.mqcdn.com/tiles/1.0.0/hyb/" & prevZ & "/" & prevX & "/" & prevY & ".png")
        UrlTitle.Add("Open Steet Map")
        UrlList.Add("http://a.tile.openstreetmap.org/" & prevZ & "/" & prevX & "/" & prevY & ".png")
        UrlTitle.Add("Staman Watercolour")
        UrlList.Add("https://stamen-tiles-c.a.ssl.fastly.net/watercolor/" & prevZ & "/" & prevX & "/" & prevY & ".jpg")



    End Sub

    Sub getOSMImageList()
        'gets a single tile from each of the OSM providers ....  skips if error found

        'clear current list
        OSMImageList.Images.Clear()


        Dim CropRect As New Rectangle(croppedImageSize, 0, croppedImageSize, croppedImageSize)
        Dim CropImage = New Bitmap(CropRect.Width, CropRect.Height)
        Dim OriginalImage As Image
        For j As Integer = 0 To UrlList.Count - 1
            Try
                CropImage = New Bitmap(CropRect.Width, CropRect.Height)
                OriginalImage = Bitmap.FromStream(System.Net.HttpWebRequest.Create(UrlList(j)).GetResponse.GetResponseStream)
                Using grp = Graphics.FromImage(CropImage)
                    grp.DrawImage(OriginalImage, New Rectangle(0, 0, CropRect.Width, CropRect.Height), CropRect, GraphicsUnit.Pixel)
                End Using

                OSMImageList.Images.Add(CropImage)

                ListView1.Items.Add(UrlTitle(j), j)

            Catch ex As Exception

            End Try
        Next


        ListView1.LargeImageList = OSMImageList
    End Sub



End Class

<Serializable()> _
Public Class OL3BasemapsOSMSaveObject

    Sub New(ByVal theControl As OLBasemapOSM)
        If theControl IsNot Nothing And theControl.ListView1.SelectedItems.Count > 0 Then
            OSMType = theControl.ListView1.SelectedItems(0).Text
        Else
            OSMType = "MapQuest"
        End If

    End Sub

    Public OSMType As String

End Class