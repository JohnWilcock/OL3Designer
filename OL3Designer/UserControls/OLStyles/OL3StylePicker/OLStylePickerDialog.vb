Imports System.Drawing
Imports System.Reflection
Imports System.Collections
Imports System.Globalization
Imports System.Resources
Imports System.IO
Imports System.Text.RegularExpressions


Public Class OLStylePickerDialog

    'Public originalStyleCaller As OLStylePicker
    'Public previewStylePicker As OLStylePicker
    Public standardStylesImageList As New ImageList
    Public standardStylesImageListName As New List(Of String)
    Public standardStylesImageListGroup As New List(Of String)

    Public OLPointPicker As New OLStylePickerDialogPointControl
    Public OLLinePicker As New OLStylePickerDialogLineControl
    Public OLPolygonPicker As New OLStylePickerDialogPolygonControl
    Public OLIconPicker As New OLStylePickerDialogIconControl
    Public OLTextPicker As New OLStylePickerDialogTextControl
    Public OLLabelPicker As New OLStylePickerDialogLabelControl

    Public styleSettings As New StyleProperties
    Public selectionStyleSettings As New StyleProperties
    Public labelStyleSettings As New StyleProperties

    Function getLabelResolution() As String
        If OLLabelPicker.ComboBox2.Text <> "All" Then
            Return "if (resolution > " & OLLabelPicker.ComboBox2.Text.Replace(",", "") & ")"
        Else
            Return ""
        End If
    End Function

    Private Sub OLStylePickerDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        refreshDialog()
        Application.DoEvents()
    End Sub

    Sub refreshDialog()
        'disable refresh
        styleSettings.active = False

        'set up control styles linked style
        OLPointPicker.styleSettings = styleSettings
        OLLinePicker.styleSettings = styleSettings
        OLPolygonPicker.styleSettings = styleSettings
        OLIconPicker.styleSettings = styleSettings
        OLTextPicker.styleSettings = styleSettings
        OLLabelPicker.styleSettings = styleSettings

        'add all controls to panel - then hide then
        Panel1.Controls.Clear()
        Panel1.Controls.Add(OLPointPicker)
        Panel1.Controls.Add(OLLinePicker)
        Panel1.Controls.Add(OLPolygonPicker)
        Panel1.Controls.Add(OLIconPicker)
        Panel1.Controls.Add(OLTextPicker)

        Panel2.Controls.Clear()
        Panel2.Controls.Add(OLLabelPicker)

        'For Each item As Control In Panel1.Controls
        '    item.Hide()
        'Next

        Panel1.Controls.Item(0).Hide()
        Panel1.Controls.Item(1).Hide()
        Panel1.Controls.Item(2).Hide()
        Panel1.Controls.Item(3).Hide()
        Panel1.Controls.Item(4).Hide()

        'show relevent user control
        Select Case styleSettings.OLGeomType
            Case "Point"
                Panel1.Controls.Item(0).Show()
            Case "Line"
                Panel1.Controls.Item(1).Show()
            Case "Polygon"
                Panel1.Controls.Item(2).Show()
            Case "Icon"
                Panel1.Controls.Item(3).Show()
            Case "Text"
                Panel1.Controls.Item(4).Show()
        End Select

        setupStandardStyles()

        'enable refresh
        styleSettings.active = True
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'originalStyleCaller = DirectCast(OlStylePicker1, OLStylePicker)

        Me.Hide()
    End Sub

    Sub setupStandardStyles()
        standardStylesImageList.ImageSize = New Size(22, 22)
        standardStylesImageList.ColorDepth = ColorDepth.Depth32Bit
        standardStylesImageList.TransparentColor = Color.White
        'wipe listbox
        standardStylesImageList.Images.Clear()
        standardStylesImageListName.Clear()
        ListView1.Clear()

        Dim runTimeResourceSet As Object
        Dim dictEntry As DictionaryEntry
        Dim tempString As String
        Dim regexString As String

        Select Case styleSettings.OLGeomType
            Case "Point"
                regexString = "^(olstyle_icon|olstyle_marker)"
            Case "Icon"
                regexString = "^(olstyle_icon|olstyle_marker)"
            Case "Line"
                regexString = "^(olstyle_line)"
            Case "Polygon"
                regexString = "^(olstyle_polygon)"
        End Select

        runTimeResourceSet = My.Resources.ResourceManager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, False, True)
        For Each dictEntry In runTimeResourceSet

            If Regex.IsMatch(dictEntry.Key.ToString, regexString) Then
                standardStylesImageList.Images.Add(resizeIcons(My.Resources.ResourceManager.GetObject(dictEntry.Key.ToString), standardStylesImageList))
                tempString = dictEntry.Key.ToString.Replace(dictEntry.Key.ToString.Split("_")(0) & "_" & dictEntry.Key.ToString.Split("_")(1), "").Replace("_", " ")
                standardStylesImageListName.Add(tempString)
                standardStylesImageListGroup.Add(dictEntry.Key.ToString.Split("_")(1))
            End If
        Next


        'find custom icons
        If styleSettings.OLGeomType = "Icon" Or styleSettings.OLGeomType = "Point" Then
            Dim theAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
            Dim customIconPath As String = Path.Combine(Path.GetDirectoryName(theAssembly.Location), "Custom Icons")
            If System.IO.Directory.Exists(customIconPath) Then
                Dim di As New DirectoryInfo(customIconPath)
                ' Get a reference to each file in that directory.
                Dim fiArr As FileInfo() = di.GetFiles()
                Dim fri As FileInfo
                Dim tempBitmap As Bitmap
                For Each fri In fiArr
                    'check is valid image file
                    If Path.GetExtension(fri.FullName).ToUpper = ".PNG" Then
                        tempBitmap = New Bitmap(fri.FullName)
                        standardStylesImageList.Images.Add(resizeIcons(tempBitmap, standardStylesImageList))
                        standardStylesImageListName.Add(Path.GetFileNameWithoutExtension(fri.FullName))
                        standardStylesImageListGroup.Add("cicon")
                    End If
                Next
            End If
        End If

        ListView1.LargeImageList = standardStylesImageList
        ListView1.SmallImageList = standardStylesImageList

        For i As Integer = 0 To standardStylesImageListName.Count - 1
            ListView1.Items.Add(standardStylesImageListName(i), i)
            ListView1.Items.Item(i).Group = ListView1.Groups(standardStylesImageListGroup(i))
        Next


        ListView1.ShowGroups = True
        ListView1.Refresh()
    End Sub

    Function resizeIcons(ByVal picImage As Image, ByVal ImageList1 As ImageList) As Image

        Dim proportion As Integer = 0
        Dim startx As Decimal = 0
        Dim startY As Decimal = 0
        Dim drawwidth As Decimal = 0
        Dim drawheight As Decimal = 0
        Dim org_Image As Bitmap = New Bitmap(picImage)
        Dim final_Bitmap As Bitmap = New Bitmap(ImageList1.ImageSize.Width, ImageList1.ImageSize.Height)
        Dim gr As Graphics = Graphics.FromImage(final_Bitmap)
        Dim factorscale As Decimal
        factorscale = org_Image.Height / org_Image.Width
        drawwidth = final_Bitmap.Width
        drawheight = final_Bitmap.Width * factorscale
        If drawheight > final_Bitmap.Height Then
            proportion = 1
            factorscale = org_Image.Width / org_Image.Height
            drawheight = final_Bitmap.Height
            drawwidth = final_Bitmap.Height * factorscale
        End If
        startx = 0
        startY = final_Bitmap.Height - drawheight
        gr.DrawImage(org_Image, startx, startY, drawwidth, drawheight)
        Return final_Bitmap
    End Function

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        styleSettings.active = False
        Dim hasChanged As Boolean = False

        If ListView1.SelectedItems.Count > 0 Then
            hasChanged = True
            Dim theItem As ListViewItem = ListView1.SelectedItems.Item(0)

            Select Case theItem.Group.Name

                Case "icon", "cicon"
                    styleSettings.OLGeomType = "Icon"
                    Panel1.Controls.Item(0).Hide()
                    Panel1.Controls.Item(3).Show()

                    Dim theAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()

                    'save bitmap down
                    Dim theBitmap As Bitmap = New Bitmap(standardStylesImageList.Images(theItem.ImageIndex))
                    theBitmap.Save(Path.Combine(Path.GetDirectoryName(theAssembly.Location), theItem.Text) & ".png")

                    'set src
                    styleSettings.OLSRC = (Path.Combine(Path.GetDirectoryName(theAssembly.Location), theItem.Text) & ".png").Replace("\", "/")

                    'set default styles
                    If theItem.Group.Name <> "cicon" Then
                        Dim defStyle() As String = getDefaulStyleSettings(theItem.Text).Split(",")
                        styleSettings.OLScale = defStyle(0)
                        styleSettings.OLRotation = defStyle(1)
                        styleSettings.OLAnchorOrigin = defStyle(2)
                        styleSettings.OLXAnchor = defStyle(3)
                        styleSettings.OLXAnchorUnit = defStyle(4)
                        styleSettings.OLYAnchor = defStyle(5)
                        styleSettings.OLYAnchorUnit = defStyle(6)
                    End If

                    'place style values into dialog controls
                    OLIconPicker.loadValuesToControls()


                Case "marker"
                    styleSettings.OLGeomType = "Point"
                    Panel1.Controls.Item(3).Hide()
                    Panel1.Controls.Item(0).Show()

                    'set default styles
                    Dim defStyle() As String = getDefaulStyleSettings(theItem.Text).Split(",")
                    styleSettings.OLSize = defStyle(0)
                    styleSettings.OLStrokeWidth = defStyle(1)
                    styleSettings.OLStrokeColor = ColorTranslator.FromHtml(defStyle(2))
                    styleSettings.OLFillColour = ColorTranslator.FromHtml(defStyle(3))

                    'place style values into dialog controls
                    OLPointPicker.loadValuesToControls()

                Case "line" 
                    'Panel1.Controls.Item(1).Show()

                    'set default styles
                    Dim defStyle() As String = getDefaulStyleSettings(theItem.Text).Split(",")
                    styleSettings.OLStrokeWidth = defStyle(0)
                    styleSettings.OLStrokeColor = ColorTranslator.FromHtml(defStyle(1))
                    styleSettings.OLLineCap = defStyle(2)
                    styleSettings.OLLineJoin = defStyle(3)
                    styleSettings.OLMiterLimit = defStyle(4)
                    styleSettings.OLLineDash = defStyle(5)

                    'place style values into dialog controls
                    OLLinePicker.loadValuesToControls()

                Case "polygon"
                    'Panel1.Controls.Item(2).Show()

                    'set default styles
                    Dim defStyle() As String = getDefaulStyleSettings(theItem.Text).Split(",")
                    styleSettings.OLStrokeWidth = defStyle(0)
                    styleSettings.OLStrokeColor = ColorTranslator.FromHtml(defStyle(1))
                    styleSettings.OLLineCap = defStyle(2)
                    styleSettings.OLLineJoin = defStyle(3)
                    styleSettings.OLMiterLimit = defStyle(4)
                    styleSettings.OLLineDash = defStyle(5)
                    styleSettings.OLFillColour = ColorTranslator.FromHtml(defStyle(6))


                    'place style values into dialog controls
                    OLPolygonPicker.loadValuesToControls()
                Case Else


            End Select

        End If

        'only refresh if an item was selected (not unselected)
        If hasChanged Then
            styleSettings.active = True
            styleSettings.trigger()
        End If


    End Sub

    Function getDefaulStyleSettings(ByVal styleName As String) As String
        'get default strings
        Dim defaultString As String = My.Resources.DefaultStyleSettings
        Dim defaultStrings() As String = defaultString.Split(";")
        Dim currentStyle() As String

        For Each item As String In defaultStrings
            currentStyle = item.Split(":")
            Dim a As String = currentStyle(0).Replace(vbCrLf, "").ToString.Trim(" ")
            Dim b As String = styleName.ToString.Trim(" ")
            If currentStyle(0).Replace(vbCrLf, "").ToString.Trim(" ") = styleName.ToString.Trim(" ") Then
                Return currentStyle(1)
            End If
        Next

    End Function

End Class