Imports System.Reflection
Imports System.IO

Public Class OL3PopupDesignerSimple
    Public layerPath As String
    Public Fieldlist As New List(Of String)
    Public numberOfFeatures As Long
    Public loaded As Boolean = False
    Public FieldTemplate As New DataGridViewComboBoxColumn
    Public selectedOrientation As String = "top"
    Public imageType As New ImageSource

    Public Function getPopupFunction(ByVal mapNumber As Integer, ByVal vectorLayerID As Integer, Optional ByVal outPath As String = "") As String
        'returrns the popup div function determined by the user control designer.
        Dim popupTemplate As String = createPopupHTMLPreview("output", outPath)

        'function determines if a cluster was selected, if so calls a function to get all features
        Dim popupFunction As String = "function map" & mapNumber & "_vectorLayer_" & vectorLayerID & "_popup_function(feature){" & Chr(10)
        popupFunction = popupFunction & "var popupString = '';" & Chr(10)
        popupFunction = popupFunction & "var allFeatures = feature.get('features');" & Chr(10)
        popupFunction = popupFunction & "if (typeof allFeatures == 'undefined'){" & Chr(10)
        popupFunction = popupFunction & "popupString = " & Chr(34) & "<div style='max-height:200px;overflow:auto;'>" & Chr(34) & " + map" & mapNumber & "_vectorLayer_" & vectorLayerID & "_popupHTML(feature) + " & Chr(34) & "</div>" & Chr(34) & ";" & Chr(10)
        popupFunction = popupFunction & "}else{" & Chr(10)
        popupFunction = popupFunction & "popupString = " & Chr(34) & "<div style='max-height:200px;width:100%;overflow:auto;padding:2px;'>" & Chr(34) & " + returnClusterPP(feature,'map" & mapNumber & "_vectorLayer_" & vectorLayerID & "') + " & Chr(34) & "</div>" & Chr(34) & ";" & Chr(10)
        popupFunction = popupFunction & "}" & Chr(10)
        popupFunction = popupFunction & "return popupString " & Chr(10)
        popupFunction = popupFunction & "}" & Chr(10) & Chr(10)

        'popup html
        popupFunction = popupFunction & "function map" & mapNumber & "_vectorLayer_" & vectorLayerID & "_popupHTML(feature){" & Chr(10)
        popupFunction = popupFunction & "return " & Chr(34) & popupTemplate & Chr(34) & Chr(10)
        popupFunction = popupFunction & "}" & Chr(10) & Chr(10)
        Return popupFunction

    End Function


    Public Sub populateFieldlist()
        Dim GDAL As New GDALImport
        Fieldlist = GDAL.getFieldList(layerPath)
        numberOfFeatures = GDAL.getFeatureCount(layerPath)

        FieldTemplate = DataGridView1.Columns(0)
        FieldTemplate.Items.Clear()
        FieldTemplate.Items.AddRange(Fieldlist.ToArray)

    End Sub


    Private Sub OL3PopupDesignerSimple_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        init()

    End Sub

    Sub init()
        populateFieldlist()

        'populate number of features combobox for preview
        ToolStripComboBox1.Items.Clear()
        For y As Integer = 1 To numberOfFeatures
            ToolStripComboBox1.Items.Add(y)
        Next

        'set defult feature number for preview
        ToolStripComboBox1.SelectedIndex = 0

        'set the form to loaded so the chnage handles work as expected
        loaded = True

        Dim typeColumn As DataGridViewComboBoxColumn = DataGridView1.Columns(1)
        typeColumn.DefaultCellStyle.NullValue = "Text"

        'set default popup feature and width
        ToolStripComboBox1.SelectedIndex = 0
        ToolStripComboBox2.SelectedIndex = 1

    End Sub


    'places X in remove column button cell
    Private Sub removeRow_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles DataGridView1.CellPainting
        If e.ColumnIndex = 2 AndAlso e.RowIndex >= 0 Then
            e.Paint(e.CellBounds, DataGridViewPaintParts.All)

            Dim bmpFind As Bitmap = My.Resources.Offline_16xLG__2
            e.Graphics.DrawImage(bmpFind, e.CellBounds.Left + 2, e.CellBounds.Top + 2)
            e.Handled = True
        End If
    End Sub

    Sub showHideImagetype()
        If isImagePopup() Then
            imageType.Visible = True
        Else
            imageType.Visible = False
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        'remove column
        If e.ColumnIndex = 2 Then
            If e.RowIndex < DataGridView1.Rows.Count - 1 Then
                DataGridView1.Rows.RemoveAt(e.RowIndex)
                showHideImagetype()
            End If
        End If


    End Sub

    Private Sub DataGridView1_CellEndEdit(sender As Object, e As EventArgs) Handles DataGridView1.CellEndEdit 'wrong needs after edit
        refreshPreview()
    End Sub

    Private Sub DataGridView1_CellDirty(sender As Object, e As EventArgs) Handles DataGridView1.CurrentCellDirtyStateChanged
        'force dgv to loose control
        ActiveControl = Button1
        'check for image types
        showHideImagetype()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        addAllFields()
    End Sub

    Sub addAllFields()
        DataGridView1.Rows.Clear()

        For x As Integer = 0 To Fieldlist.Count - 1
            'create new row
            DataGridView1.Rows.Add()
            DataGridView1.Rows(DataGridView1.Rows.Count - 2).Cells(0).Value = Fieldlist(x)

        Next
    End Sub

    Function createPopupHTMLPreview(Optional ByVal popupType As String = "preview", Optional ByVal outPath As String = "") As String ' preview popup only
        Dim popupHTML As String = ""
        Dim popupCSS As String = ""
        Dim feature As String = ""

        'shps feature index starts at 1 index, tabs start at 0 ?! What !!
        Dim correctedFirstFeatureIndex As Integer = 1
        If Path.GetExtension(layerPath).ToUpper = ".TAB" Then
            correctedFirstFeatureIndex = 0
        End If


        'get feature values 
        Dim featureValues As List(Of String) = returnFeatureValuesList(CLng(ToolStripComboBox1.Text) - correctedFirstFeatureIndex)

        'set up table
        Dim interalPadding As Integer = 2
        popupHTML = "<html style='width:100%;height:100%;'><body style='width:100%;height:100%;'><div style='overflow:auto;padding:" & interalPadding & "px;'><table style='width:100%;border-collapse:collapse;padding:3px;border-spacing: 0px;'>"

        'cycle through elements in popup
        For x As Integer = 0 To DataGridView1.Rows.Count - 2
            'popupCSS = "width:100%;"
            popupCSS = "max-width:" & CInt(ToolStripComboBox2.Text) - (interalPadding * 2) - 24 & "px;max-height:" & "450" & "px;"

            'if preview, get values, if output construct a request to feature.get
            Select Case popupType
                Case "preview"
                    feature = featureValues(getFieldindex(DataGridView1.Rows(x).Cells("Column1").FormattedValue))
                Case "output"
                    feature = Chr(34) & "+ feature.get('" & DataGridView1.Rows(x).Cells("Column1").FormattedValue & "') + " & Chr(34)
            End Select

            'construct html table
            Select Case DataGridView1.Rows(x).Cells("Column2").FormattedValue
                Case "Text"
                    popupHTML = popupHTML & "<tr style='width:100%;'><td style='" & popupCSS & "'><p>" & feature & "</p></td></tr>"
                Case "Image"
                    Select Case popupType
                        Case "preview"
                            popupHTML = popupHTML & "<tr style='width:100%;'><td ><img style='" & popupCSS & "' src='" & imageType.getPath(feature, "") & "'></img></td></tr>"
                        Case "output"
                            popupHTML = popupHTML & "<tr style='width:100%;'><td ><img style='" & popupCSS & "' onclick= " & Chr(34) & " + String.fromCharCode(34) + " & Chr(34) & " window.open('" & Chr(34) & " + getCorrectPath(feature,'" & DataGridView1.Rows(x).Cells("Column1").FormattedValue & "','rel') + " & Chr(34) & "')" & Chr(34) & " + String.fromCharCode(34) + " & Chr(34) & "   src='" & imageType.getPopupImageFetcher(DataGridView1.Rows(x).Cells("Column1").FormattedValue) & "'></img></td></tr>"
                    End Select
            End Select


        Next

        'finish table
        popupHTML = popupHTML & "</table></div></body></html>"

        Return popupHTML
    End Function



    Sub refreshPreview()
        'exit if no data
        If DataGridView1.Rows.Count <= 1 Then Exit Sub

        'check for ol3 script and css, crete if not present
        Dim helper As New HelperFunctions
        helper.createOL3Script()

        'get popup preview html/js string from resourses
        Dim PopupPreviewHTML As String = My.Resources.PopupJSPreview

        'get popup preview container css
        Dim PopupCSS As String = getPopupPreviewCSS()

        'replace OL3FEATUREVALUES with feature values
        PopupPreviewHTML = PopupPreviewHTML.Replace("POPUPPREVIEWHTML", PopupCSS)

        'replace OL3GETDIVSTRING with popup div
        PopupPreviewHTML = PopupPreviewHTML.Replace("POPUPPREVIEWCONTENT", createPopupHTMLPreview())

        'place string in html preview (this will trigger a refresh)
        OL3PopupPreview1.PopupHTML = PopupPreviewHTML
    End Sub

    Function getPopupPreviewCSS() As String
        getPopupPreviewCSS = ""

        'orientation
        getPopupPreviewCSS = getPopupPreviewCSS & "<DIV style='max-width:" & ToolStripComboBox2.Text & "px;max-height:" & "450" & "px;' class='popover " & selectedOrientation & "'>"

        getPopupPreviewCSS = getPopupPreviewCSS & "<DIV class='arrow'></DIV>"

        'title
        getPopupPreviewCSS = getPopupPreviewCSS '& "<H3 class='popover-title'>" & "title" & "</H3>"

        'content
        getPopupPreviewCSS = getPopupPreviewCSS & "<DIV class='popover-content' >POPUPPREVIEWCONTENT</DIV>"

        'end
        getPopupPreviewCSS = getPopupPreviewCSS & "</DIV>"

        Return getPopupPreviewCSS
    End Function

    Function returnFeatureValuesJSON(ByVal featureNumber As Long) As String
        Dim GDAL As New GDALImport
        Dim fieldValues As List(Of String) = GDAL.getFieldValue(layerPath, featureNumber)
        Dim OLFeatureJS As String = ""

        'create OL3 javascript for feature
        For x As Integer = 0 To Fieldlist.Count - 1
            OLFeatureJS = OLFeatureJS & Fieldlist(x) & ":'" & fieldValues(x) & "',"
        Next
        'trim off the last comma
        OLFeatureJS = OLFeatureJS.Substring(0, OLFeatureJS.Length - 1)
        Return OLFeatureJS
    End Function

    Function returnFeatureValuesList(ByVal featureNumber As Long) As List(Of String)
        Dim GDAL As New GDALImport
        Dim fieldValues As List(Of String) = GDAL.getFieldValue(layerPath, featureNumber)
        Return fieldValues
    End Function

    Function getFieldindex(ByVal FieldText As String) As Integer
        For x As Integer = 0 To Fieldlist.Count - 1
            If FieldText = Fieldlist(x) Then Return x
        Next
    End Function

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        refreshPreview()
    End Sub

    Private Sub ToolStripComboBox1_Changed(sender As Object, e As EventArgs) Handles ToolStripComboBox1.SelectedIndexChanged
        refreshPreview()
    End Sub

    Private Sub ToolStripComboBox2_Changed(sender As Object, e As EventArgs) Handles ToolStripComboBox2.SelectedIndexChanged
        refreshPreview()
    End Sub


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        'set imagesource control
        imageType.Width = imageType.Width + 70
        imageType.ComboBox1.Top = 2
        imageType.ComboBox1.Left = 70
        imageType.Parent = Me
        imageType.Location = New Point(100, 8)
        imageType.Visible = False

    End Sub

    Function isImagePopup() As Boolean
        'cycle through elements
        For x As Integer = 0 To DataGridView1.Rows.Count - 2
            If DataGridView1.Rows(x).Cells(1).Value = "Image" Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub ToolStripDropDownButton1_Click(sender As Object, e As EventArgs) Handles ToolStripDropDownButton1.Click
        'popup orientaion



    End Sub

    Private Sub TopToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TopToolStripMenuItem.Click
        ToolStripDropDownButton1.Image = TopToolStripMenuItem.Image
        selectedOrientation = "top"
    End Sub

    Private Sub BottomToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BottomToolStripMenuItem.Click
        ToolStripDropDownButton1.Image = BottomToolStripMenuItem.Image
        selectedOrientation = "bottom"
    End Sub

    Private Sub LeftToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LeftToolStripMenuItem.Click
        ToolStripDropDownButton1.Image = LeftToolStripMenuItem.Image
        selectedOrientation = "left"
    End Sub

    Private Sub RightToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RightToolStripMenuItem.Click
        ToolStripDropDownButton1.Image = RightToolStripMenuItem.Image
        selectedOrientation = "right"
    End Sub


End Class


