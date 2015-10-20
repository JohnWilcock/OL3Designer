Imports System.IO
Imports System.Reflection

Public Class OL3LayerStyleHeatmap

    Public olPath As String = ""
    Public olBlur As String = ""
    Public olRadius As String = ""
    Public olGradient As String = "'#00f', '#0ff', '#0f0', '#ff0', '#f00'"
    Public layerP As String = ""
    Public FieldMin As Double
    Public FieldMax As Double
    Public adjustmentFactor As Double 'whats required to make the minimum value 0 (could be pos of neg)
    Public adjustmentDivisor As Double 'whats required to divide values by to make range extend from 0 to 1

    Public firstLoaded As Boolean = True

    Public cr As New ColourRamps

    Private Sub OL3LayerStyleHeatmap_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Sub New(ByVal layerPath As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        layerP = layerPath
        'find location of ol.js script, if not present get it from resourses

        olPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\"
        cr.init(ComboBox1)

        Dim helper As New HelperFunctions
        Dim scriptTags As String = helper.writeAllLibraries(olPath)

        Dim fullHeatmapHTML As String = My.Resources.HeatmapPreview.Replace("OLFILEPATH", olPath)

        olBlur = 20
        olRadius = 20

        fullHeatmapHTML = fullHeatmapHTML.Replace("OLBLUR", olBlur)
        fullHeatmapHTML = fullHeatmapHTML.Replace("OLRADIUS", olRadius)

        fullHeatmapHTML = fullHeatmapHTML.Replace("OLGRADIENT", "'#00f', '#0ff', '#0f0', '#ff0', '#f00'")

        'set browser source to the string
        WebBrowser1.DocumentText = fullHeatmapHTML


        'populate combobox with numeric fields only
        Dim gdal As New GDALImport
        ComboBox2.Items.Clear()
        Dim theFieldList As List(Of String) = gdal.getFieldList(layerPath)
        Dim tempFieldType As String
        For f As Integer = 0 To theFieldList.Count - 1
            tempFieldType = gdal.getFieldType(layerPath, theFieldList(f))
            If tempFieldType = "Integer" Or tempFieldType = "Real" Then
                ComboBox2.Items.Add(theFieldList(f))
            End If
        Next

        ComboBox1.SelectedIndex = 1
    End Sub


    Sub refreshPreview()

        If firstLoaded = False Then Exit Sub

        Dim helper As New HelperFunctions
        Dim scriptTags As String = helper.writeAllLibraries(olPath)

        Dim fullHeatmapHTML As String = My.Resources.HeatmapPreview.Replace("OLFILEPATH", olPath)
        fullHeatmapHTML = fullHeatmapHTML.Replace("OLBLUR", olBlur)
        fullHeatmapHTML = fullHeatmapHTML.Replace("OLRADIUS", olRadius)
        fullHeatmapHTML = fullHeatmapHTML.Replace("OLGRADIENT", olGradient)

        'set browser source to the string
        'WebBrowser1.DocumentText = fullHeatmapHTML

        'http://stackoverflow.com/questions/174403/net-webbrowser-documenttext-isnt-changing
        WebBrowser1.Document.OpenNew(True)
        WebBrowser1.Document.Write(fullHeatmapHTML)
        WebBrowser1.DocumentText = fullHeatmapHTML

        'refresh browser
        WebBrowser1.Refresh()

    End Sub

    Private Sub TrackBar2_Scroll(sender As Object, e As EventArgs) Handles TrackBar2.Scroll
        olBlur = TrackBar2.Value
        refreshPreview()
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        olRadius = TrackBar1.Value
        refreshPreview()
    End Sub


    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim rand As New Random

        If ComboBox1.SelectedIndex = 0 Then
            olGradient = "'" & ColorTranslator.ToHtml(Color.FromArgb(255, rand.Next(1, 254), rand.Next(1, 254), rand.Next(1, 254))) & "', '" & ColorTranslator.ToHtml(Color.FromArgb(255, rand.Next(1, 254), rand.Next(1, 254), rand.Next(1, 254))) & "', '" & ColorTranslator.ToHtml(Color.FromArgb(255, rand.Next(1, 254), rand.Next(1, 254), rand.Next(1, 254))) & "', '" & ColorTranslator.ToHtml(Color.FromArgb(255, rand.Next(1, 254), rand.Next(1, 254), rand.Next(1, 254))) & "', '" & ColorTranslator.ToHtml(Color.FromArgb(255, rand.Next(1, 254), rand.Next(1, 254), rand.Next(1, 254))) & "'"
        Else
            olGradient = "'" & ColorTranslator.ToHtml(cr.fullRampList(ComboBox1.SelectedIndex - 1).getColourByPercent(1)) & "', '" & ColorTranslator.ToHtml(cr.fullRampList(ComboBox1.SelectedIndex - 1).getColourByPercent(25)) & "', '" & ColorTranslator.ToHtml(cr.fullRampList(ComboBox1.SelectedIndex - 1).getColourByPercent(50)) & "', '" & ColorTranslator.ToHtml(cr.fullRampList(ComboBox1.SelectedIndex - 1).getColourByPercent(75)) & "', '" & ColorTranslator.ToHtml(cr.fullRampList(ComboBox1.SelectedIndex - 1).getColourByPercent(99)) & "'"

        End If
        refreshPreview()

    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        getNewRanges()
    End Sub

    Sub getNewRanges()
        'get the field ranges and calcs what if required to divide it so range is between 0 and 1 
        Dim gdal As New GDALImport
        Dim fieldRanges() As Double = gdal.getFieldRanges(layerP, ComboBox2.Text)
        FieldMin = fieldRanges(0)
        FieldMax = fieldRanges(1)


        adjustmentFactor = FieldMin
        adjustmentDivisor = FieldMax - adjustmentFactor
    End Sub

    Function getKeyText() As String
        If TextBox1.Text = "" Then
            Return Chr(34) & Chr(34)
        Else
            Return Chr(34) & TextBox1.Text & Chr(34)
        End If

    End Function



    Public Function save() As OL3LayerHeatmapSaveObject
        save = New OL3LayerHeatmapSaveObject

        save.blur = olBlur
        save.radius = olRadius

        save.keyDescription = TextBox1.Text

        save.weightField = ComboBox2.Text

        save.colourRamp = ComboBox1.SelectedIndex


    End Function

    Public Sub loadObj(ByVal saveObj As OL3LayerHeatmapSaveObject)

        firstLoaded = False

        TrackBar2.Value = saveObj.blur
        TrackBar1.Value = saveObj.radius

        olBlur = saveObj.blur
        olRadius = saveObj.radius

        TextBox1.Text = saveObj.keyDescription

        ComboBox2.SelectedText = saveObj.weightField

        ComboBox1.SelectedIndex = saveObj.colourRamp

        firstLoaded = True

        refreshPreview()
    End Sub
End Class




<Serializable()> _
Public Class OL3LayerHeatmapSaveObject
    Public weightField As String
    Public colourRamp As Integer
    Public radius As Double
    Public blur As Double
    Public keyDescription As String
End Class