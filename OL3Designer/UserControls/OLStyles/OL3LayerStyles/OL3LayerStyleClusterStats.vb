Public Class OL3LayerStyleClusterStats

    Public NumericRangesStyleList As New List(Of StyleProperties)
    Public theStylePicker As New OLStylePicker
    Public layerPath As String
    Public layerType As String




    Public statMin As Double
    Public statMax As Double
    Public statSum As Double


    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        refreshClusterStyles()
    End Sub

    Sub New(ByVal layerT As String, ByVal layerP As String)
        layerPath = layerP
        layerType = layerT
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.


        'Dim gdal2 As New GDALImport
        'OlStylePicker1.fieldList = gdal2.getFieldList(layerPath)

        populateComboBoxes()
    End Sub

    Public Function getKeyText() As String
        getKeyText = ""
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            getKeyText = getKeyText & "," & Chr(34) & DataGridView1.Rows(x).Cells(3).Value.ToString & Chr(34)
        Next


        If ComboBox1.Text = "None" Then

        Else
            getKeyText = getKeyText.Substring(1)
        End If

    End Function

    Sub refreshClusterStyles()

        'get javascript to call a JS function which takes a cluster, column name and stat type and returns the value -> done, placed at end of key function.txt  

        'clear previous
        DataGridView1.Rows.Clear()
        Dim currentRowIndex As Integer = 0
        Dim fieldMinMax() As Double
        Dim equalIncrement As Double


        'get the NumericRanges styles for chosen column - return as array
        'run function her to return distinct elements of chosen field
        Dim GDAL As New GDALImport

        'is it a numeric column - error/exit if not
        If GDAL.getFieldType(layerPath, ComboBox1.Text) = "String" Then
            If ComboBox1.Items.Count = 0 Or ComboBox1.Text = "None" Then
                Exit Sub
            End If

            MsgBox("Not a numeric field")
            Exit Sub
        End If



        'get min and max values default 
        fieldMinMax = New Double() {0, 100}

        'max-min = range ->>> divide by num of rows in numeric up/down
        Select Case ComboBox3.Text
            Case "Sum"
                'find the sum of all values
                fieldMinMax = New Double() {GDAL.getFieldValuesDbl(layerPath, ComboBox1.Text).Min, GDAL.getFieldValuesDbl(layerPath, ComboBox1.Text).Sum}

            Case "Count"
                'find the count of all values
                fieldMinMax = New Double() {1, GDAL.getFeatureCount(layerPath) + 1}

            Case "Mean", "Min", "Max"
                'use min/max as depending on which points are in a cluster it could be between these values
                fieldMinMax = GDAL.getFieldRanges(layerPath, ComboBox1.Text)

        End Select
        equalIncrement = (fieldMinMax(1) - fieldMinMax(0)) / NumericUpDown1.Value

        Dim layerFieldList As List(Of String) = GDAL.getFieldList(layerPath)

        'for each row create a default style and put it in the style list, put its bitmap in the image column and value in value column
        Dim currentRow As ClusterStatsRow
        For l As Integer = 0 To NumericUpDown1.Value - 1
            currentRowIndex = DataGridView1.Rows.Add(New ClusterStatsRow)
            currentRow = DataGridView1.Rows(currentRowIndex)
            currentRow.ClusterRangesStyle.isCluster = True
            currentRow.ClusterRangesStyle.clusterStatField = ComboBox1.Text
            'currentRow.NumericRangesStyle.OLStyleSettings.OLGeomType = "Point"

            currentRow.ClusterRangesStyle.OLStyleSettings.OLGeomType = layerType
            currentRow.ClusterRangesStyle.layerPath = layerPath
            currentRow.ClusterRangesStyle.fieldList = layerFieldList

            'currentRow.uniqueStyle2.OLGeomType = layerType
            'theStylePicker.OLStyleSettings = currentRow.uniqueStyle2
            theStylePicker.OLStyleSettings = currentRow.ClusterRangesStyle.OLStyleSettings
            Application.DoEvents()

            'add data
            currentRow.ClusterRangesValue = fieldMinMax(0) + (equalIncrement * l)
            currentRow.ClusterRangesEndValue = fieldMinMax(0) + (equalIncrement * (l + 1))
            currentRow.ClusterRangesLabel = fieldMinMax(0) + (equalIncrement * l) & " - " & fieldMinMax(0) + (equalIncrement * (l + 1))

            DataGridView1.Rows(currentRowIndex).Cells("OLStyle").Value = currentRow.ClusterRangesStyle.PanelToBitmap
            DataGridView1.Rows(currentRowIndex).Cells("OLStyle").Value = theStylePicker.PanelToBitmap
            DataGridView1.Rows(currentRowIndex).Cells("OLValue").Value = currentRow.ClusterRangesValue
            DataGridView1.Rows(currentRowIndex).Cells("OLEndValue").Value = currentRow.ClusterRangesEndValue
            DataGridView1.Rows(currentRowIndex).Cells("Label").Value = currentRow.ClusterRangesLabel
        Next

    End Sub

    Sub ManualyAlterClusterRangesStyles(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        Select Case e.ColumnIndex
            Case 0
                Dim selectedRow As ClusterStatsRow = DataGridView1.Rows(e.RowIndex)
                'theStylePicker.OLStyleSettings = selectedRow.uniqueStyle2
                ' theStylePicker.showPicker()
                DataGridView1.Rows(e.RowIndex).Cells("OLStyle").Value = theStylePicker.PanelToBitmap

                selectedRow.ClusterRangesStyle.showPicker()
                DataGridView1.Rows(e.RowIndex).Cells("OLStyle").Value = selectedRow.ClusterRangesStyle.PanelToBitmap
            Case Else

        End Select


    End Sub

    Private Sub OL3LayerStyleClusterRangesValues_Load(sender As Object, e As EventArgs) Handles MyBase.Load





    End Sub

    Public Sub populateComboBoxes()
        Dim GDAL As New GDALImport
        ComboBox1.Items.Clear()
        'GDAL.getGeoJson({layerPath})
        Dim UVs As List(Of String) = GDAL.getFieldList(layerPath)
        ComboBox1.Items.AddRange(UVs.ToArray)

        'ComboBox1.Items.Add("None")
        'ComboBox1.Text = "None"

        'find a numeric field and alter the combobox to it - if none, say so and default to "simple cluster"
        For h As Integer = 0 To UVs.Count - 1
            If GDAL.getFieldType(layerPath, UVs(h)) = "Real" Or GDAL.getFieldType(layerPath, UVs(h)) = "Integer" Then

                ComboBox1.Text = UVs(h)
                Exit Sub
            End If
        Next


    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        refreshClusterStyles()
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        refreshClusterStyles()
    End Sub
End Class

Public Class ClusterStatsRow
    Inherits DataGridViewRow
    Public ClusterRangesStyle As OLStylePicker
    'Public uniqueStyle2 As StyleProperties
    Public ClusterRangesBitmap As Bitmap
    Public ClusterRangesValue As String
    Public ClusterRangesEndValue As String
    Public ClusterRangesLabel As String

    Sub New()
        ClusterRangesStyle = New OLStylePicker
        'uniqueStyle2 = New StyleProperties
    End Sub


End Class

