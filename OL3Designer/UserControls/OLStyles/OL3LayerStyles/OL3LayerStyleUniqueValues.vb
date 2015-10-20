Imports System.Xml.Serialization
Imports System.IO
Imports System.Reflection
Imports System.Runtime.Serialization.Formatters.Binary

Public Class OL3LayerStyleUniqueValues

    Public uniqueStyleList As New List(Of StyleProperties)
    Public theStylePicker As New OLStylePicker
    Public layerPath As String
    Public layerType As String
    Public cr As New ColourRamps
    Dim firstLoaded As Boolean = False
    Dim test As String

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        refreshUniqueStyles()
    End Sub

    Sub New(ByVal layerT As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        layerType = layerT

        cr.init(ComboBox2)
        'set colour ramp to random
        ComboBox2.SelectedIndex = 0

        If layerType <> "Point" Then
            SizeRamps1.ComboBox1.Text = 1
            SizeRamps1.ComboBox2.Text = 1
        End If

        'add event handlers for size ramp
        AddHandler SizeRamps1.ComboBox1.SelectedIndexChanged, AddressOf changeSizeRamp
        AddHandler SizeRamps1.ComboBox2.SelectedIndexChanged, AddressOf changeSizeRamp
        firstLoaded = True
    End Sub

    Sub changeSizeRamp()
        applyColourRamp()
    End Sub

    'places red X  in remove column button cell
    Private Sub removeRow_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles DataGridView1.CellPainting
        If e.ColumnIndex = 3 AndAlso e.RowIndex >= 0 Then
            e.Paint(e.CellBounds, DataGridViewPaintParts.All)

            Dim bmpFind As Bitmap = My.Resources.Offline_16xLG__2
            e.Graphics.DrawImage(bmpFind, e.CellBounds.Left + 2, e.CellBounds.Top + 2)
            e.Handled = True
        End If

    End Sub

    Public Function getKeyText() As String
        getKeyText = ""
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(x).Cells(2).Value IsNot Nothing Then 'check for blank values
                getKeyText = getKeyText & "," & Chr(34) & DataGridView1.Rows(x).Cells(2).Value.ToString & Chr(34)
            Else
                getKeyText = getKeyText & "," & Chr(34) & " " & Chr(34)
            End If

        Next
        getKeyText = getKeyText.Substring(1)
    End Function

    Sub refreshUniqueStyles()

        'get the unique styles for chosen column - return as array
        'run function her to return distinct elements of chosen field
        Dim GDAL As New GDALImport
        Dim UVs As List(Of String) = GDAL.getFieldValues(layerPath, ComboBox1.Text, True)

        Dim chosenColumnUniqueValues As String() = UVs.ToArray '{"type1", "type2", "unknown Type"}

        'check for sensible amount - as using webrowser controls is slow and clunky it would take hours to load 1000s of styles
        If UVs.Count > 5 Then
            If MsgBox("There are more than 5 unique values in this field, loading styles for all values many be slow. Continue ?", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then Exit Sub
        End If

        'clear previous
        DataGridView1.Rows.Clear()
        Dim currentRowIndex As Integer = 0

        Dim layerFieldList As List(Of String) = GDAL.getFieldList(layerPath)

        'for each row create a default style and put it in the style list, put its bitmap in the image column and value in value column
        Dim currentRow As uniqueRow
        For Each uniqueValue As String In chosenColumnUniqueValues
            currentRowIndex = DataGridView1.Rows.Add(New uniqueRow)
            currentRow = DataGridView1.Rows(currentRowIndex)
            'currentRow.uniqueStyle.OLStyleSettings.OLGeomType = "Point"

            currentRow.uniqueStyle.OLStyleSettings.OLGeomType = layerType
            currentRow.uniqueStyle.layerPath = layerPath
            currentRow.uniqueStyle.fieldList = layerFieldList


            'set a random fill colour
            Dim rand As New Random
            currentRow.uniqueStyle.OLStyleSettings.OLFillColour = Color.FromArgb(rand.Next(0, 254), rand.Next(0, 254), rand.Next(0, 254), rand.Next(0, 254))
            currentRow.uniqueStyle.OLStyleSettings.OLStrokeColor = Color.Black

            'currentRow.uniqueStyle2.OLGeomType = layerType
            'theStylePicker.OLStyleSettings = currentRow.uniqueStyle2
            theStylePicker.OLStyleSettings = currentRow.uniqueStyle.OLStyleSettings
            Application.DoEvents()

            'add data
            currentRow.uniqueValue = uniqueValue
            currentRow.uniqueLabel = uniqueValue

            DataGridView1.Rows(currentRowIndex).Cells("OLStyle").Value = currentRow.uniqueStyle.PanelToBitmap
            'DataGridView1.Rows(currentRowIndex).Cells("OLStyle").Value = theStylePicker.PanelToBitmap
            DataGridView1.Rows(currentRowIndex).Cells("OLValue").Value = currentRow.uniqueValue
            DataGridView1.Rows(currentRowIndex).Cells("Label").Value = currentRow.uniqueLabel
        Next

    End Sub

    Sub ManualyAlterUniqueStyles(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        Select Case e.ColumnIndex
            Case 0
                Dim selectedRow As uniqueRow = DataGridView1.Rows(e.RowIndex)
                'theStylePicker.OLStyleSettings = selectedRow.uniqueStyle2
                ' theStylePicker.showPicker()
                DataGridView1.Rows(e.RowIndex).Cells("OLStyle").Value = theStylePicker.PanelToBitmap

                selectedRow.uniqueStyle.showPicker()
                DataGridView1.Rows(e.RowIndex).Cells("OLStyle").Value = selectedRow.uniqueStyle.PanelToBitmap


            Case 3 ' remove row
                DataGridView1.Rows.RemoveAt(e.RowIndex)



            Case Else
        End Select


    End Sub

    Private Sub OL3LayerStyleUniqueValues_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        populateComboBoxes()



    End Sub

    Public Sub populateComboBoxes()
        Dim GDAL As New GDALImport
        'GDAL.getGeoJson({layerPath})
        Dim UVs As List(Of String) = GDAL.getFieldList(layerPath)
        ComboBox1.Items.AddRange(UVs.ToArray)


    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If firstLoaded And ComboBox2.Text <> "" Then
            applyColourRamp()
        End If

    End Sub

    Sub applyColourRamp()
        Dim numRows As Integer = DataGridView1.Rows.Count
        Dim theColourRamp As Image = cr.RampList.Images(ComboBox2.SelectedIndex)
        Dim rand As New Random

        'check for valid rows
        If numRows = 0 Then Exit Sub

        Dim theColourRampBMP As Bitmap = New Bitmap(theColourRamp)
        Dim thePixel As Color
        Dim theRow As uniqueRow
        For c As Integer = 0 To numRows - 1
            thePixel = theColourRampBMP.GetPixel(CInt(((c) / numRows) * theColourRampBMP.Width) + 1, 1)
            theRow = DataGridView1.Rows(c)

            'check for random colour distribution - always the first row
            If ComboBox2.SelectedIndex = 0 Then
                theRow.uniqueStyle.OLStyleSettings.OLFillColour = Color.FromArgb(255, rand.Next(1, 254), rand.Next(1, 254), rand.Next(1, 254))
            Else
                theRow.uniqueStyle.OLStyleSettings.OLFillColour = thePixel
            End If

            'apply size ramp
            If layerType = "Point" Then
                theRow.uniqueStyle.OLStyleSettings.OLSize = SizeRamps1.getSize(numRows, c)
            Else
                theRow.uniqueStyle.OLStyleSettings.OLStrokeWidth = SizeRamps1.getSize(numRows, c)
            End If


            Application.DoEvents()
            DataGridView1.Rows(c).Cells("OLStyle").Value = theRow.uniqueStyle.PanelToBitmap
            Application.DoEvents()

        Next


    End Sub



    Sub copyLabelToAllRows()
        Dim currentRow As uniqueRow
        Dim pasteStyleSettings As StyleProperties
        Dim copyRow As uniqueRow = DataGridView1.Rows(0)
        Dim copyStyleSettings As StyleProperties = copyRow.uniqueStyle.OLStyleSettings


        For u As Integer = 0 To DataGridView1.Rows.Count - 1
            currentRow = DataGridView1.Rows(u)
            pasteStyleSettings = currentRow.uniqueStyle.OLStyleSettings
            pasteStyleSettings.active = False

            pasteStyleSettings.OLTextCol = copyStyleSettings.OLTextCol

            pasteStyleSettings.OLTextColour = copyStyleSettings.OLTextColour
            pasteStyleSettings.OLTextFont = copyStyleSettings.OLTextFont
            pasteStyleSettings.OLTextHAlign = copyStyleSettings.OLTextHAlign
            pasteStyleSettings.OLTextRotation = copyStyleSettings.OLTextRotation
            pasteStyleSettings.OLTextSize = copyStyleSettings.OLTextSize
            pasteStyleSettings.OlTextTransparancy = copyStyleSettings.OlTextTransparancy
            pasteStyleSettings.OLTextVAlign = copyStyleSettings.OLTextVAlign
            pasteStyleSettings.OLTextXOffset = copyStyleSettings.OLTextXOffset
            pasteStyleSettings.OLTextYOffset = copyStyleSettings.OLTextYOffset

            pasteStyleSettings.active = True


        Next
    End Sub

    Public Function save() As OL3LayerUniqueValuesSaveObject
        save = New OL3LayerUniqueValuesSaveObject
        Dim tempRow As uniqueRow

        For y As Integer = 0 To DataGridView1.Rows.Count - 1
            tempRow = New uniqueRow
            tempRow = DataGridView1.Rows(y)
            save.styles.Add(tempRow.save())
        Next

        save.colourRamp = cr.rampPicker.SelectedIndex
        save.field = ComboBox1.Text
        save.fromSize = SizeRamps1.sizeFrom
        save.toSize = SizeRamps1.sizeTo
        save.layerPath = layerPath

    End Function


    Public Sub loadObj(ByVal saveObj As OL3LayerUniqueValuesSaveObject)
        firstLoaded = False
        layerPath = saveObj.layerPath

        populateComboBoxes()

        cr.rampPicker.SelectedIndex = saveObj.colourRamp
        ComboBox1.Text = saveObj.field
        SizeRamps1.sizeFrom = saveObj.fromSize
        SizeRamps1.sizeTo = saveObj.toSize



        Dim tempRow As uniqueRow
        DataGridView1.Rows.Clear()
        For y As Integer = 0 To saveObj.styles.Count - 1
            tempRow = New uniqueRow
            tempRow.loadObj(saveObj.styles(y))
            DataGridView1.Rows.Add(tempRow)

            DataGridView1.Rows(y).Cells(0).Value = tempRow.uniqueBitmap
            DataGridView1.Rows(y).Cells(1).Value = tempRow.uniqueValue
            DataGridView1.Rows(y).Cells(2).Value = tempRow.uniqueLabel

            tempRow.uniqueStyle.refreshControl()

        Next

        firstLoaded = True
    End Sub


End Class





Public Class uniqueRow
    Inherits DataGridViewRow
    Public uniqueStyle As OLStylePicker
    'Public uniqueStyle2 As StyleProperties
    Public uniqueBitmap As Bitmap
    Public uniqueValue As String
    Public uniqueLabel As String

    Sub New()
        uniqueStyle = New OLStylePicker
        'uniqueStyle2 = New StyleProperties
    End Sub

    Public Function save() As OL3LayerSingleUniqueValueSaveObject
        save = New OL3LayerSingleUniqueValueSaveObject
        save.styleProp = uniqueStyle.OLStyleSettings
        save.uniqueValue = uniqueValue
        save.uniqueLabel = uniqueLabel
        'save.uniqueBitmap = Cells(0).Value
        uniqueBitmap = Cells(0).Value

        Dim ms As New System.IO.MemoryStream()
        uniqueBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
        Dim byteImage As Byte() = ms.ToArray()
        save.uniqueBitmapBase64 = Convert.ToBase64String(byteImage)

    End Function

    Public Sub loadObj(ByVal saveObj As OL3LayerSingleUniqueValueSaveObject)
        uniqueStyle.OLStyleSettings = saveObj.styleProp
        uniqueStyle.ChangeOLStylePickerdialog.styleSettings = saveObj.styleProp
        uniqueValue = saveObj.uniqueValue
        uniqueLabel = saveObj.uniqueLabel

        Dim byteImage As Byte() = Convert.FromBase64String(saveObj.uniqueBitmapBase64)
        Dim ms As New MemoryStream(byteImage, 0, byteImage.Length)

        ' Convert byte[] to Image
        ms.Write(byteImage, 0, byteImage.Length)
        uniqueBitmap = Image.FromStream(ms, True)


    End Sub

   
End Class

<Serializable()> _
Public Class OL3LayerSingleUniqueValueSaveObject
    Public styleProp As StyleProperties
    Public uniqueValue As String
    Public uniqueLabel As String
    Public uniqueBitmap As Bitmap
    Public uniqueBitmapBase64 As String
End Class

<Serializable()> _
Public Class OL3LayerUniqueValuesSaveObject
    Public styles As New List(Of OL3LayerSingleUniqueValueSaveObject)
    Public layerPath As String
    Public field As String
    Public colourRamp As Integer
    Public fromSize As Double
    Public toSize As Double
End Class