Imports System.IO
Imports System.Drawing
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Windows


Public Class OLStylePicker

    Public WithEvents OLStyleSettings As StyleProperties 'Public WithEvents OLStyleSettings As New StyleProperties
    Public ChangeOLStylePickerdialog As OLStylePickerDialog 'Public ChangeOLStylePickerdialog As New OLStylePickerDialog
    Public pickerEnabled As Boolean = True
    Public StylePickerImage As Image
    Public StylePickerTextImage As Image
    Dim HH As HtmlElementEventHandler = New HtmlElementEventHandler(AddressOf WebBrowser1_MouseDown)
    Public isCluster As Boolean = False
    Public clusterStatField As String = ""

    Public layerPath As String = ""
    'Public fieldList As List(Of String)
    Public Event fieldListChange()

    Const FEATURE_DISABLE_NAVIGATION_SOUNDS As Integer = 21
    Const SET_FEATURE_ON_PROCESS As Integer = &H2

    <DllImport("urlmon.dll")> _
     <PreserveSig> _
    Private Shared Function CoInternetSetFeatureEnabled(FeatureEntry As Integer, <MarshalAs(UnmanagedType.U4)> dwFlags As Integer, fEnable As Boolean) As <MarshalAs(UnmanagedType.[Error])> Integer
    End Function

    Private _fieldList As List(Of String)
    Public Property fieldList As List(Of String)
        Get
            Return _fieldList
        End Get
        Set(ByVal value As List(Of String))
            _fieldList = value
            RaiseEvent fieldListChange()
        End Set
    End Property

    Sub setLabelFieldList() Handles Me.fieldListChange
        If fieldList IsNot Nothing Then
            If isCluster Then
                ChangeOLStylePickerdialog.OLLabelPicker.ComboBox1.Items.Clear()
                ChangeOLStylePickerdialog.OLLabelPicker.ComboBox1.Items.AddRange({"Sum", "Mean", "Min", "Max", "Count"})
            Else
                ChangeOLStylePickerdialog.OLLabelPicker.ComboBox1.Items.Clear()
                ChangeOLStylePickerdialog.OLLabelPicker.ComboBox1.Items.AddRange(fieldList.ToArray)
            End If

        End If
    End Sub


    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        OLStyleSettings = New StyleProperties
        ChangeOLStylePickerdialog = New OLStylePickerDialog


    End Sub

    Private Shared Sub DisableClickSounds()
        CoInternetSetFeatureEnabled(FEATURE_DISABLE_NAVIGATION_SOUNDS, SET_FEATURE_ON_PROCESS, True)
    End Sub

    Sub OLupdate() Handles OLStyleSettings.onChange
        If OLStyleSettings.active = True Then
            refreshControl()
            StylePickerImage = PanelToBitmap()
            'ChangeOLStylePickerdialog.refreshDialog()
        End If
    End Sub

    Sub OLTextupdate() Handles OLStyleSettings.onTextChange
        If OLStyleSettings.active = True Then
            refreshTextControl()

        End If
    End Sub

    Function getIndividualStyleString(Optional styleNum As Integer = 9999) As String

        Select Case OLStyleSettings.OLGeomType
            Case "Icon"
                getIndividualStyleString = "new ol.style.Style({image: new ol.style.Icon( ({scale: " & OLStyleSettings.OLScale & ", rotation: " & OLStyleSettings.OLRotation & ", anchor: [" & OLStyleSettings.OLXAnchor & ", " & OLStyleSettings.OLYAnchor & "],anchorXUnits: '" & OLStyleSettings.OLXAnchorUnit & "',anchorYUnits: '" & OLStyleSettings.OLYAnchorUnit & "',opacity: 1,src: '" & OLStyleSettings.OLSRC & "'})) " & getIndividualLabelString(styleNum) & "  })"
            Case Else
                getIndividualStyleString = "new ol.style.Style({image: new ol.style.Circle({radius: " & OLStyleSettings.OLSize & ",fill: new ol.style.Fill({color: '" & ColorTranslator.ToHtml(OLStyleSettings.OLFillColour) & "'}),stroke: new ol.style.Stroke({color: '" & ColorTranslator.ToHtml(OLStyleSettings.OLStrokeColor) & "', width: " & OLStyleSettings.OLStrokeWidth & ",lineCap: '" & OLStyleSettings.OLLineCap & "', lineJoin: '" & OLStyleSettings.OLLineJoin & "', lineDash: [" & OLStyleSettings.OLLineDash + 1 & "], miterLimit:" & OLStyleSettings.OLMiterLimit & "})}),stroke: new ol.style.Stroke({color:  '" & ColorTranslator.ToHtml(OLStyleSettings.OLStrokeColor) & "',width: " & OLStyleSettings.OLStrokeWidth & ",lineCap: '" & OLStyleSettings.OLLineCap & "', lineJoin: '" & OLStyleSettings.OLLineJoin & "', lineDash: [" & OLStyleSettings.OLLineDash + 1 & "], miterLimit:" & OLStyleSettings.OLMiterLimit & "}),	fill: new ol.style.Fill({color:  'rgba(" & OLStyleSettings.OLFillColour.R & ", " & OLStyleSettings.OLFillColour.G & ", " & OLStyleSettings.OLFillColour.B & ", " & OLStyleSettings.OlTransparancy & ")'}) " & getIndividualLabelString(styleNum) & "  })"
                'getIndividualStyleString = "new ol.style.Style({image: new ol.style.Circle({radius: " & OLStyleSettings.OLSize & ",fill: new ol.style.Fill({color: '" & ColorTranslator.ToHtml(OLStyleSettings.OLFillColour) & "'}),stroke: new ol.style.Stroke({color: '" & ColorTranslator.ToHtml(OLStyleSettings.OLStrokeColor) & "', width: " & OLStyleSettings.OLStrokeWidth & ",lineCap: '" & OLStyleSettings.OLLineCap & "', lineJoin: '" & OLStyleSettings.OLLineJoin & "', lineDash: [" & OLStyleSettings.OLLineDash & "], miterLimit:" & OLStyleSettings.OLMiterLimit & "})}),stroke: new ol.style.Stroke({color:  '" & ColorTranslator.ToHtml(OLStyleSettings.OLStrokeColor) & "',width: " & OLStyleSettings.OLStrokeWidth & ",lineCap: '" & OLStyleSettings.OLLineCap & "', lineJoin: '" & OLStyleSettings.OLLineJoin & "', lineDash: [" & OLStyleSettings.OLLineDash & "], miterLimit:" & OLStyleSettings.OLMiterLimit & "}),	fill: new ol.style.Fill({color:  '" & ColorTranslator.ToHtml(OLStyleSettings.OLFillColour) & "'})})"
        End Select

    End Function

    Function getMultiStyleString() As String
        'not used
    End Function

    Function getLabelExpresion() As String
        If isCluster Then
            getLabelExpresion = "returnClusterStatistics(feature,'" & clusterStatField & "','" & ChangeOLStylePickerdialog.OLLabelPicker.ComboBox1.Text & "') "
            If ChangeOLStylePickerdialog.OLLabelPicker.ComboBox1.Text = "" Then getLabelExpresion = "''"

        Else
            getLabelExpresion = "feature.get('" & ChangeOLStylePickerdialog.OLLabelPicker.ComboBox1.Text & "')"
        End If



    End Function

    Sub refreshControl()

        'find location of ol.js script, if not present get it from resourses
        Dim olFilePath As String = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\"

        'get current geometry type and draw it 
        Dim GeomTypeString As String = "features: [PointFeature]"
        Select Case OLStyleSettings.OLGeomType
            Case "Point"
                GeomTypeString = "features: [PointFeature]"
            Case "Icon"
                'requires icon.png text replacement in stylejs
                GeomTypeString = "features: [IconFeature]"
            Case "Line"
                GeomTypeString = "features: [LineFeature]"
            Case "Polygon"
                GeomTypeString = "features: [PolyFeature]"
        End Select

        'create sytle based on public dims
        'use getindividualstyleString 
        Dim fullStyleHTML As String = My.Resources.STYLEJS.Replace("FEATURETYPEINSERTPOINT", GeomTypeString)

        Select Case OLStyleSettings.OLGeomType
            Case "Icon"
                fullStyleHTML = fullStyleHTML.Replace("'ICONSTYLESTRINGINSERTPOINT'", getIndividualStyleString())
            Case Else
                fullStyleHTML = fullStyleHTML.Replace("'STYLESTRINGINSERTPOINT'", getIndividualStyleString())

        End Select

        fullStyleHTML = fullStyleHTML.Replace("OLFILEPATH", olFilePath)


        'set browser source to the string
        WebBrowser1.DocumentText = fullStyleHTML
        ChangeOLStylePickerdialog.WebBrowser1.DocumentText = fullStyleHTML
        'http://stackoverflow.com/questions/174403/net-webbrowser-documenttext-isnt-changing
        WebBrowser1.Document.OpenNew(True)
        ChangeOLStylePickerdialog.WebBrowser1.Document.OpenNew(True)
        WebBrowser1.Document.Write(fullStyleHTML)
        ChangeOLStylePickerdialog.WebBrowser1.Document.Write(fullStyleHTML)
        WebBrowser1.DocumentText = fullStyleHTML

        'refresh browser
        WebBrowser1.Refresh()
        ChangeOLStylePickerdialog.WebBrowser1.Refresh()
    End Sub

    Sub refreshTextControl()

        'find location of ol.js script, if not present get it from resourses
        Dim olFilePath As String = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\"

        Dim textParameters As String = ""
        textParameters = getIndividualLabelString()

        'create sytle based on public dims
        Dim fullStyleHTML As String = My.Resources.LABELJS.Replace("LABELEXPRESIONINSERTPOINT", textParameters)

        fullStyleHTML = fullStyleHTML.Replace("OLFILEPATH", olFilePath)


        'set browser source to the string
        ChangeOLStylePickerdialog.WebBrowser2.DocumentText = fullStyleHTML
        'http://stackoverflow.com/questions/174403/net-webbrowser-documenttext-isnt-changing
        ChangeOLStylePickerdialog.WebBrowser2.Document.OpenNew(True)
        ChangeOLStylePickerdialog.WebBrowser2.Document.Write(fullStyleHTML)

        'refresh browser
        ChangeOLStylePickerdialog.WebBrowser2.Refresh()
    End Sub

    Function getIndividualLabelString(Optional OutputTempVar As Integer = 9999) As String
        'label for preview or output file
        Dim labelTempVar As String = "'Abc'"
        If OutputTempVar <> 9999 Then
            labelTempVar = "tempLabel" & OutputTempVar
        End If

        'if not label expresion selected then exit sub and don't return a text string (blank)
        If ChangeOLStylePickerdialog.OLLabelPicker.ComboBox1.Text = "" Or ChangeOLStylePickerdialog.OLLabelPicker.ComboBox1.Text = "No Label" Then
            Return ""
        End If

        getIndividualLabelString = ", text:new ol.style.Text({textAlign: '" & OLStyleSettings.OLTextHAlign & "', textBaseline: '" & OLStyleSettings.OLTextVAlign & "', font: 'Bold " & OLStyleSettings.OLTextSize & "px " & OLStyleSettings.OLTextFont & "',text: " & labelTempVar & ", fill: new ol.style.Fill({color: 'rgba(" & OLStyleSettings.OLTextColour.R & "," & OLStyleSettings.OLTextColour.G & ", " & OLStyleSettings.OLTextColour.B & ", 1)'}), stroke: new ol.style.Stroke({color: 'rgba(" & OLStyleSettings.OLMaskColor.R & "," & OLStyleSettings.OLMaskColor.G & ", " & OLStyleSettings.OLMaskColor.B & ", 1)', width:" & OLStyleSettings.OLMaskWidth & "}),offsetX: " & OLStyleSettings.OLTextXOffset & ",offsetY: " & OLStyleSettings.OLTextYOffset & ",rotation: " & OLStyleSettings.OLTextRotation & "})"
    End Function


    Sub resetToDefault()
        OLStyleSettings.active = False
        OLStyleSettings.OLGeomType = "Line"
        OLStyleSettings.OLFillColour = Color.AliceBlue
        OLStyleSettings.OLStrokeColor = Color.Aquamarine
        OLStyleSettings.OLStrokeWidth = 2
        OLStyleSettings.OLSize = 8
        OLStyleSettings.OlTransparancy = 1
        OLStyleSettings.OLRotation = 0
        OLStyleSettings.OLXOffset = 0
        OLStyleSettings.OLYOffset = 0
        OLStyleSettings.active = True

    End Sub



    Private Sub OLStylePicker_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'http://stackoverflow.com/questions/10456/howto-disable-webbrowser-click-sound-in-your-app-only
        DisableClickSounds()
        ChangeOLStylePickerdialog.styleSettings = Me.OLStyleSettings
        refreshControl()
        '

    End Sub





    Public Sub showPicker()
        Application.DoEvents()
        ChangeOLStylePickerdialog.ShowDialog()

        RemoveHandler WebBrowser1.Document.Body.MouseDown, AddressOf WebBrowser1_MouseDown
        addWBClickHandler()
    End Sub


    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted
        addWBClickHandler()
    End Sub

    Sub addWBClickHandler()
        'Attach MouseDown event handler
        AddHandler WebBrowser1.Document.Body.MouseDown, HH
    End Sub

    Private Sub WebBrowser1_MouseDown(ByVal sender As Object, ByVal e As HtmlElementEventArgs)
        If pickerEnabled = False Then Exit Sub
        If e.MouseButtonsPressed = Windows.Forms.MouseButtons.Left Then
            showPicker()
            'MouseDown event
            'Debug.WriteLine("MouseDown at " & e.MousePosition.X & "," & e.MousePosition.Y)
        End If
    End Sub


    Public Function PanelToBitmap() As Image
        Dim pnl As Control = Me
        Dim bmp = New Bitmap(pnl.Width, pnl.Height)
        pnl.DrawToBitmap(bmp, New Rectangle(0, 0, bmp.Width, bmp.Height))
        Return bmp
    End Function



End Class



Public Class StyleProperties

    Public Event onChange()
    Public Event onTextChange()
    Public active As Boolean = True

    Public Sub trigger()
        RaiseEvent onChange()
    End Sub

    Public Sub triggerText()
        RaiseEvent onTextChange()
    End Sub

    Private _OLSize As Integer = 8
    Public Property OLSize() As Integer
        Get
            Return _OLSize
        End Get
        Set(ByVal value As Integer)
            _OLSize = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLGeomType As String = "Point"
    Public Property OLGeomType As String
        Get
            Return _OLGeomType
        End Get
        Set(ByVal value As String)
            _OLGeomType = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLFillColour As Color = Color.AliceBlue
    Public Property OLFillColour As Color
        Get
            Return _OLFillColour
        End Get
        Set(ByVal value As Color)
            _OLFillColour = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLStrokeColor As Color = Color.Aquamarine
    Public Property OLStrokeColor As Color
        Get
            Return _OLStrokeColor
        End Get
        Set(ByVal value As Color)
            _OLStrokeColor = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLStrokeWidth As Integer = 2
    Public Property OLStrokeWidth As Integer
        Get
            Return _OLStrokeWidth
        End Get
        Set(ByVal value As Integer)
            _OLStrokeWidth = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OlTransparancy As Double = 1
    Public Property OlTransparancy As Double
        Get
            Return _OlTransparancy
        End Get
        Set(ByVal value As Double)
            _OlTransparancy = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLRotation As Integer = 0
    Public Property OLRotation As Integer
        Get
            Return _OLRotation
        End Get
        Set(ByVal value As Integer)
            _OLRotation = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLXOffset As Integer = 0
    Public Property OLXOffset As Integer
        Get
            Return _OLXOffset
        End Get
        Set(ByVal value As Integer)
            _OLXOffset = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLYOffset As Integer = 0
    Public Property OLYOffset As Integer
        Get
            Return _OLYOffset
        End Get
        Set(ByVal value As Integer)
            _OLYOffset = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLXAnchor As Double = 0
    Public Property OLXAnchor As Double
        Get
            Return _OLXAnchor
        End Get
        Set(ByVal value As Double)
            _OLXAnchor = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLYAnchor As Double = 0
    Public Property OLYAnchor As Double
        Get
            Return _OLYAnchor
        End Get
        Set(ByVal value As Double)
            _OLYAnchor = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLYAnchorUnit As String = "pixels"
    Public Property OLYAnchorUnit As String
        Get
            Return _OLYAnchorUnit
        End Get
        Set(ByVal value As String)
            _OLYAnchorUnit = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLXAnchorUnit As String = "fraction"
    Public Property OLXAnchorUnit As String
        Get
            Return _OLXAnchorUnit
        End Get
        Set(ByVal value As String)
            _OLXAnchorUnit = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLSRC As String = "C:/Users/John/Documents/visual studio 2013/Projects/OL3Designer/OL3Designer/test/icon2.png"
    Public Property OLSRC As String
        Get
            Return _OLSRC
        End Get
        Set(ByVal value As String)
            _OLSRC = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLLineCap As String = "round"
    Public Property OLLineCap As String
        Get
            Return _OLLineCap
        End Get
        Set(ByVal value As String)
            _OLLineCap = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLLineJoin As String = "round"
    Public Property OLLineJoin As String
        Get
            Return _OLLineJoin
        End Get
        Set(ByVal value As String)
            _OLLineJoin = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLLineDash As Integer = 0
    Public Property OLLineDash As Integer
        Get
            Return _OLLineDash
        End Get
        Set(ByVal value As Integer)
            _OLLineDash = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLMiterLimit As Integer = 10
    Public Property OLMiterLimit As Integer
        Get
            Return _OLMiterLimit
        End Get
        Set(ByVal value As Integer)
            _OLMiterLimit = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLScale As Double = 1
    Public Property OLScale As Double
        Get
            Return _OLScale
        End Get
        Set(ByVal value As Double)
            _OLScale = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLAnchorOrigin As String = "bottom-left"
    Public Property OLAnchorOrigin As String
        Get
            Return _OLAnchorOrigin
        End Get
        Set(ByVal value As String)
            _OLAnchorOrigin = value
            If active Then RaiseEvent onChange()
        End Set
    End Property

    Private _OLTextSize As Integer = 8
    Public Property OLTextSize As Integer
        Get
            Return _OLTextSize
        End Get
        Set(ByVal value As Integer)
            _OLTextSize = value
            If active Then RaiseEvent onTextChange()
        End Set
    End Property

    Private _OLTextColour As Color = Color.Black
    Public Property OLTextColour As Color
        Get
            Return _OLTextColour
        End Get
        Set(ByVal value As Color)
            _OLTextColour = value
            If active Then RaiseEvent onTextChange()
        End Set
    End Property

    Private _OLMaskColor As Color = Color.White
    Public Property OLMaskColor As Color
        Get
            Return _OLMaskColor
        End Get
        Set(ByVal value As Color)
            _OLMaskColor = value
            If active Then RaiseEvent onTextChange()
        End Set
    End Property

    Private _OLMaskWidth As Integer = 2
    Public Property OLMaskWidth As Integer
        Get
            Return _OLMaskWidth
        End Get
        Set(ByVal value As Integer)
            _OLMaskWidth = value
            If active Then RaiseEvent onTextChange()
        End Set
    End Property

    Private _OlTextTransparancy As Double = 1
    Public Property OlTextTransparancy As Double
        Get
            Return _OlTextTransparancy
        End Get
        Set(ByVal value As Double)
            _OlTextTransparancy = value
            If active Then RaiseEvent onTextChange()
        End Set
    End Property

    Private _OLTextRotation As Integer = 0
    Public Property OLTextRotation As Integer
        Get
            Return _OLTextRotation
        End Get
        Set(ByVal value As Integer)
            _OLTextRotation = value
            If active Then RaiseEvent onTextChange()
        End Set
    End Property

    Private _OLTextXOffset As Integer = 0
    Public Property OLTextXOffset As Integer
        Get
            Return _OLTextXOffset
        End Get
        Set(ByVal value As Integer)
            _OLTextXOffset = value
            If active Then RaiseEvent onTextChange()
        End Set
    End Property

    Private _OLTextYOffset As Integer = 0
    Public Property OLTextYOffset As Integer
        Get
            Return _OLTextYOffset
        End Get
        Set(ByVal value As Integer)
            _OLTextYOffset = value
            If active Then RaiseEvent onTextChange()
        End Set
    End Property

    Private _OLTextFont As String = "Arial"
    Public Property OLTextFont As String
        Get
            Return _OLTextFont
        End Get
        Set(ByVal value As String)
            _OLTextFont = value
            If active Then RaiseEvent onTextChange()
        End Set
    End Property

    Private _OLTextCol As String = ""
    Public Property OLTextCol As String
        Get
            Return _OLTextCol
        End Get
        Set(ByVal value As String)
            _OLTextCol = value
            If active Then RaiseEvent onTextChange()
        End Set
    End Property

    Private _OLTextHAlign As String = "Center"
    Public Property OLTextHAlign As String
        Get
            Return _OLTextHAlign
        End Get
        Set(ByVal value As String)
            _OLTextHAlign = value
            If active Then RaiseEvent onTextChange()
        End Set
    End Property

    Private _OLTextVAlign As String = "Center"
    Public Property OLTextVAlign As String
        Get
            Return _OLTextVAlign
        End Get
        Set(ByVal value As String)
            _OLTextVAlign = value
            If active Then RaiseEvent onTextChange()
        End Set
    End Property
End Class
