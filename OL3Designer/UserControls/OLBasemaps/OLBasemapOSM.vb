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

        If ListView1.SelectedItems.Count > 0 Then
            Select Case ListView1.SelectedItems(0).Text
                Case "Open Steet Map"
                    Return "new ol.layer.Tile({source: new ol.source.OSM()});"
                Case "Staman Watercolour"
                    Return "new ol.layer.Tile({style:'AerialWithLabels',source: new  ol.source.Stamen({layer: 'watercolor'})});"
                Case Else
                    Return "new ol.layer.Tile({source: new ol.source.OSM()})"
            End Select

        Else
            Return "new ol.layer.Tile({source: new ol.source.OSM()})"
        End If



    End Function

    Sub setUrlStringList()
        UrlList = New List(Of String)
        UrlTitle = New List(Of String)

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
            OSMType = "Open Steet Map"
        End If

    End Sub

    Public OSMType As String

End Class