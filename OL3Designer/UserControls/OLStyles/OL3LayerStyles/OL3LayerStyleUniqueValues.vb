Public Class OL3LayerStyleUniqueValues

    Public uniqueStyleList As New List(Of StyleProperties)
    Public theStylePicker As New OLStylePicker
    Public layerPath As String
    Public layerType As String
    Public cr As New ColourRamps
    Dim firstLoaded As Boolean = False

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

    Public Function getKeyText() As String
        getKeyText = ""
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            getKeyText = getKeyText & "," & Chr(34) & DataGridView1.Rows(x).Cells(1).Value.ToString & Chr(34)
        Next
        getKeyText = getKeyText.Substring(1)
    End Function

    Sub refreshUniqueStyles()
        'clear previous
        DataGridView1.Rows.Clear()
        Dim currentRowIndex As Integer = 0

        'get the unique styles for chosen column - return as array
        'run function her to return distinct elements of chosen field
        Dim GDAL As New GDALImport
        Dim UVs As List(Of String) = GDAL.getFieldValues(layerPath, ComboBox1.Text, True)

        Dim chosenColumnUniqueValues As String() = UVs.ToArray '{"type1", "type2", "unknown Type"}

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
            'pasteStyleSettings.active = False

            pasteStyleSettings.OLTextCol = copyStyleSettings.OLTextCol
            'pasteStyleSettings.OLTextColour = copyStyleSettings.OLTextColour
            'pasteStyleSettings.OLTextFont = copyStyleSettings.OLTextFont
            'pasteStyleSettings.OLTextHAlign = copyStyleSettings.OLTextHAlign
            'pasteStyleSettings.OLTextRotation = copyStyleSettings.OLTextRotation
            'pasteStyleSettings.OLTextSize = copyStyleSettings.OLTextSize
            'pasteStyleSettings.OlTextTransparancy = copyStyleSettings.OlTextTransparancy
            'pasteStyleSettings.OLTextVAlign = copyStyleSettings.OLTextVAlign
            'pasteStyleSettings.OLTextXOffset = copyStyleSettings.OLTextXOffset
            'pasteStyleSettings.OLTextYOffset = copyStyleSettings.OLTextYOffset

            ' pasteStyleSettings.active = True
        Next
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


End Class

