Public Class OL3LayerStyleDateRanges

    Public DateRangesStyleList As New List(Of StyleProperties)
    Public theStylePicker As New OLStylePicker
    Public layerPath As String
    Public layerType As String
    Public cr As New ColourRamps
    Dim firstLoaded As Boolean = False

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        refreshDateRangesStyles()
    End Sub

    Sub New(ByVal layerT As String, ByVal layerP As String)
        layerType = layerT
        layerPath = layerP

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.


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
            getKeyText = getKeyText & "," & Chr(34) & DataGridView1.Rows(x).Cells(3).Value.ToString & Chr(34)
        Next
        getKeyText = getKeyText.Substring(1)
    End Function

    Sub refreshDateRangesStyles()
        'clear previous
        DataGridView1.Rows.Clear()
        Dim currentRowIndex As Integer = 0
        Dim fieldMinMax() As Long
        Dim equalIncrement As Double

        If firstLoaded = False Then
            Exit Sub
        End If

        'get the NumericRanges styles for chosen column - return as array
        'run function her to return distinct elements of chosen field
        Dim GDAL As New GDALImport

        'is it a numeric column - error if not
        If GDAL.getFieldType(layerPath, ComboBox1.Text) <> "Date" Then
            MsgBox("Not a Date field")
            Exit Sub
        End If

        'get min and max values
        fieldMinMax = GDAL.getDateFieldRanges(layerPath, ComboBox1.Text)

        If CLng(fieldMinMax(0)) <= New Date(1900, 1, 1, 12, 0, 0).Ticks Then
            fieldMinMax(0) = New Date(1900, 1, 1, 12, 0, 0).Ticks
        End If

        If CLng(fieldMinMax(1)) <= New Date(2100, 1, 1, 12, 0, 0).Ticks Then
            fieldMinMax(1) = New Date(2100, 1, 1, 12, 0, 0).Ticks
        End If

        'max-min = range ->>> divide by num of rows in numeric up/down
        equalIncrement = (fieldMinMax(1) - fieldMinMax(0)) / NumericUpDown1.Value

        'Dim UVs As List(Of String) = GDAL.getFieldValues(layerPath, ComboBox1.Text, True)
        'Dim chosenColumnNumericRangesValues As String() = UVs.ToArray '{"type1", "type2", "unknown Type"}

        Dim layerFieldList As List(Of String) = GDAL.getFieldList(layerPath)

        'for each row create a default style and put it in the style list, put its bitmap in the image column and value in value column
        Dim currentRow As DateRangesRow
        For l As Integer = 0 To NumericUpDown1.Value - 1
            currentRowIndex = DataGridView1.Rows.Add(New DateRangesRow)
            currentRow = DataGridView1.Rows(currentRowIndex)
            'currentRow.NumericRangesStyle.OLStyleSettings.OLGeomType = "Point"

            currentRow.DateRangesStyle.OLStyleSettings.OLGeomType = layerType
            currentRow.DateRangesStyle.layerPath = layerPath
            currentRow.DateRangesStyle.fieldList = layerFieldList

            'set a random fill colour
            Dim rand As New Random
            currentRow.DateRangesStyle.OLStyleSettings.OLFillColour = Color.FromArgb(rand.Next(0, 254), rand.Next(0, 254), rand.Next(0, 254), rand.Next(0, 254))
            currentRow.DateRangesStyle.OLStyleSettings.OLStrokeColor = Color.Black



            'currentRow.uniqueStyle2.OLGeomType = layerType
            'theStylePicker.OLStyleSettings = currentRow.uniqueStyle2
            theStylePicker.OLStyleSettings = currentRow.DateRangesStyle.OLStyleSettings
            Application.DoEvents()

            'add data
            currentRow.DateRangesValue = New Date(fieldMinMax(0) + (equalIncrement * l))
            currentRow.DateRangesEndValue = New Date(fieldMinMax(0) + (equalIncrement * (l + 1)))
            currentRow.DateRangesLabel = fieldMinMax(0) + (equalIncrement * l) & " - " & fieldMinMax(0) + (equalIncrement * (l + 1))

            DataGridView1.Rows(currentRowIndex).Cells("OLStyle").Value = currentRow.DateRangesStyle.PanelToBitmap
            'DataGridView1.Rows(currentRowIndex).Cells("OLStyle").Value = theStylePicker.PanelToBitmap
            DataGridView1.Rows(currentRowIndex).Cells("OLValue").Value = currentRow.DateRangesValue
            DataGridView1.Rows(currentRowIndex).Cells("OLEndValue").Value = currentRow.DateRangesEndValue
            DataGridView1.Rows(currentRowIndex).Cells("Label").Value = currentRow.DateRangesLabel
        Next

    End Sub

    Sub ManualyAlterNumericRangesStyles(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        Select Case e.ColumnIndex
            Case 0
                Dim selectedRow As DateRangesRow = DataGridView1.Rows(e.RowIndex)
                'theStylePicker.OLStyleSettings = selectedRow.uniqueStyle2
                ' theStylePicker.showPicker()
                DataGridView1.Rows(e.RowIndex).Cells("OLStyle").Value = theStylePicker.PanelToBitmap

                selectedRow.DateRangesStyle.showPicker()
                DataGridView1.Rows(e.RowIndex).Cells("OLStyle").Value = selectedRow.DateRangesStyle.PanelToBitmap
            Case Else

        End Select


    End Sub

    Private Sub OL3LayerStyleDateRangesValues_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        populateComboBoxes()
    End Sub

    Public Sub populateComboBoxes()
        Dim GDAL As New GDALImport
        'GDAL.getGeoJson({layerPath})
        Dim UVs As List(Of String) = GDAL.getFieldList(layerPath)
        ComboBox1.Items.AddRange(UVs.ToArray)


    End Sub

    Sub applyColourRamp()
        Dim numRows As Integer = DataGridView1.Rows.Count
        Dim theColourRamp As Image = cr.RampList.Images(ComboBox2.SelectedIndex)
        Dim rand As New Random


        Dim theColourRampBMP As Bitmap = New Bitmap(theColourRamp)
        Dim thePixel As Color
        Dim theRow As DateRangesRow
        For c As Integer = 0 To numRows - 1
            thePixel = theColourRampBMP.GetPixel(CInt(((c) / numRows) * theColourRampBMP.Width) + 1, 1)
            theRow = DataGridView1.Rows(c)

            'check for random colour distribution - always the first row
            If ComboBox2.SelectedIndex = 0 Then
                theRow.DateRangesStyle.OLStyleSettings.OLFillColour = Color.FromArgb(255, rand.Next(1, 254), rand.Next(1, 254), rand.Next(1, 254))
            Else
                theRow.DateRangesStyle.OLStyleSettings.OLFillColour = thePixel
            End If

            'apply size ramp
            If layerType = "Point" Then
                theRow.DateRangesStyle.OLStyleSettings.OLSize = SizeRamps1.getSize(numRows, c)
            Else
                theRow.DateRangesStyle.OLStyleSettings.OLStrokeWidth = SizeRamps1.getSize(numRows, c)
            End If

            Application.DoEvents()
            DataGridView1.Rows(c).Cells("OLStyle").Value = theRow.DateRangesStyle.PanelToBitmap
            Application.DoEvents()

        Next


    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If firstLoaded And ComboBox2.Text <> "" Then
            applyColourRamp()
        End If

    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        refreshDateRangesStyles()
    End Sub

End Class

Public Class DateRangesRow
    Inherits DataGridViewRow
    Public DateRangesStyle As OLStylePicker
    'Public uniqueStyle2 As StyleProperties
    Public DateRangesBitmap As Bitmap
    Public DateRangesValue As Date
    Public DateRangesEndValue As Date
    Public DateRangesLabel As String

    Sub New()
        DateRangesStyle = New OLStylePicker
        'uniqueStyle2 = New StyleProperties
    End Sub


End Class

