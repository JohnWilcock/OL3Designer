Imports System.Text.RegularExpressions

Public Class LayoutDesigner
    Public linkedLayoutPreview As LayoutPreview
    Public WithEvents mapList As New ToolStripComboBox
    Public OL3mapsObject As OL3Maps
    Public Event onLayoutItemsChange()
    Public elementList As LayoutElements

    Private _layoutList As String = ""
    Public Property layoutList() As String
        Get
            Return _layoutList
        End Get
        Set(ByVal value As String)
            _layoutList = value
            RaiseEvent onLayoutItemsChange()
        End Set
    End Property

    Public keyObjectLiterols As String
    Public keyCount As Integer

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim tableLayout As New SplitContainer
        tableLayout.Orientation = Orientation.Horizontal
        Panel1.Parent = tableLayout.Panel1
        tableLayout.Panel1.Controls.Add(Panel1)
        TableLayoutPanel1.Controls.Add(tableLayout, 1, 0)
        tableLayout.Dock = DockStyle.Fill
        Panel1.Dock = DockStyle.Fill

        elementList = New LayoutElements
        tableLayout.Panel2.Controls.Add(elementList)
        elementList.Dock = DockStyle.Fill
        tableLayout.SplitterDistance = 260


    End Sub


    Sub refreshLayoutList() Handles Me.onLayoutItemsChange

        Dim thePanel As modifiedPanel = Panel1
        thePanel.layoutList = layoutList
        thePanel.OL3MapsObject = OL3mapsObject

    End Sub

    Public Function getHTML() As String 'preview
        'reset key string and count
        keyObjectLiterols = ""
        keyCount = 0

        Dim DocHTML As String = "<html style='height: 100%;'><body style='height: 100%;'>" & convertLayoutToHTML(Sp1) & "</html>" & collapseScript()
        'addAllLabels(Sp1)
        Return DocHTML

    End Function

    Public Function getOutputHTML(ByVal theSP As SP) As String
        'function gets output html
        'reset key string and count
        keyObjectLiterols = ""
        keyCount = 0

        Return convertLayoutToHTML(theSP)
    End Function

    Private Function addKeyObjectLiterol(ByVal theLabel As Label, ByVal theKeyOptions As LayoutKeyOptions) As String
        'is it a key
        If theLabel.Text.ToLower.Substring(0, 3) = "key" Then
            'get the key object
            Dim keyObj As LayoutKeyOptions = theKeyOptions
            'add key object and ini js to keystring
            keyObjectLiterols = keyObjectLiterols & keyObj.getKeyJSLiterol(keyCount) & Chr(10)

            keyCount = keyCount + 1
            Return keyCount - 1
        Else
            Return ""
        End If
    End Function

    Function getCellDimensions(ByVal theSP As SP, ByVal thePanel As Panel, ByVal otherFixed As FixedDimensions, ByVal theFixed As FixedDimensions) As String()
        Dim tHeight As String = 100
        Dim tWidth As String = 100

        If theSP.Orientation = Orientation.Vertical Then tHeight = "height:" & 100 & "%;" : tWidth = "width:" & CInt((thePanel.Width / theSP.Width) * 100) & "%;"
        If theSP.Orientation = Orientation.Horizontal Then tHeight = "height:" & CInt((thePanel.Height / theSP.Height) * 100) & "%;" : tWidth = "width:" & 100 & "%;"

        'override if fixed dimensions of this panel
        If theSP.Orientation = Orientation.Vertical And theFixed.fixed Then tHeight = "height:" & 100 & "%;" : tWidth = "width:" & theFixed.dimension & "px;"
        If theSP.Orientation = Orientation.Horizontal And theFixed.fixed Then tHeight = "height:" & theFixed.dimension & "px;" : tWidth = "width:" & 100 & "%;"

        'if other cell is fixed, set dims to 100%, except opposite of fixed
        If theSP.Orientation = Orientation.Vertical And otherFixed.fixed Then tHeight = "height:100%;" : tWidth = ""
        If theSP.Orientation = Orientation.Horizontal And otherFixed.fixed Then tHeight = "" : tWidth = "width:100%;"

        Return {tHeight, tWidth}

    End Function

    Function convertLayoutToHTML(ByVal theSP As SP) As String
        Dim convertToHTML As String = ""
        Dim Ori As String = ""
        Dim OriEnd As String = ""
        Dim keyDivNumber As String
        Dim HTMLText As String = ""

        convertToHTML = convertToHTML & "<table style='height:100%;width:100%;margin:0px;padding:0px;border-collapse:collapse;border-spacing:0px;'>"
        If theSP.Orientation = Orientation.Horizontal Then
            Ori = "<tr>"
            OriEnd = "</tr>"
        Else
            convertToHTML = convertToHTML & "<tr>"
        End If


        'get cell dimensions : width = 1 height = 0
        Dim cell1dims() As String = getCellDimensions(theSP, theSP.Panel1, theSP.p2Fixed, theSP.p1Fixed)
        Dim cell2dims() As String = getCellDimensions(theSP, theSP.Panel2, theSP.p1Fixed, theSP.p2Fixed)


        'cell 1
        'if contents of panel1 is another splitter, recall this sub, else create div
        removeAllButtons(theSP.Panel1)
        If theSP.Panel1.Controls.Count > 0 Then

            convertToHTML = convertToHTML & "<td id='table" & theSP.tableID & "_1' style='" & cell1dims(1) & "'>" & convertLayoutToHTML(theSP.Panel1.Controls(theSP.Panel1.Controls.Count - 1)) & "</td>"
        Else
            'if its a key get the key num , else returns blank
            keyDivNumber = addKeyObjectLiterol(theSP.aL1, theSP.p1KeyOptions)
            If theSP.p1Type.Text = "Text" Then HTMLText = StripHTMLHead(theSP.p1Text.HTMLedit.DocumentText) Else  'HTMLText = ""
            If theSP.p1Type.Text = "Image" Then HTMLText = "<img style='max-width:100%;max-height:100%;' src='" & theSP.p1ImageType.getPath(theSP.p1Img.ImageLocation, OL3mapsObject.outputLocation) & "'></img>" ' Else HTMLText = ""
            If theSP.p1Type.Text = "Controls" Then HTMLText = theSP.p1ControlOptions.getControlHTML

            convertToHTML = convertToHTML & Ori & "<td  id='table" & theSP.tableID & "_1' style='" & cell1dims(0) & cell1dims(1) & theSP.p1StyleOptions.getstyleCSS & "'><div id='" & theSP.aL1.Text.ToLower.Replace(" ", "") & keyDivNumber & "' style='position:relative;overflow:auto;" & cell1dims(0) & cell1dims(1) & "'>" & theSP.p1CollapseButtonHTML & HTMLText & "</div></td>"

            If theSP.Orientation = Orientation.Horizontal Then
                convertToHTML = convertToHTML & "</tr>"
                'convertToHTML = setButtonParameters(convertToHTML, tHeight, 100 - tHeight, 1)
            Else
                'convertToHTML = setButtonParameters(convertToHTML, tWidth, 100 - tWidth, 0)
            End If

        End If


        'cell2
        removeAllButtons(theSP.Panel2)
        HTMLText = ""
        If theSP.Panel2.Controls.Count Then

            convertToHTML = convertToHTML & Ori & "<td  id='table" & theSP.tableID & "_2'  style='" & cell2dims(1) & "'>" & convertLayoutToHTML(theSP.Panel2.Controls(theSP.Panel2.Controls.Count - 1)) & "</td>" & OriEnd
        Else
            'if its a key get the key num , else returns blank
            keyDivNumber = addKeyObjectLiterol(theSP.aL2, theSP.p2KeyOptions)
            If theSP.p2Type.Text = "Text" Then HTMLText = StripHTMLHead(theSP.p2Text.HTMLedit.DocumentText) 'Else HTMLText = ""
            If theSP.p2Type.Text = "Image" Then HTMLText = "<img style='max-width:100%;max-height:100%;' src='" & theSP.p2ImageType.getPath(theSP.p2Img.ImageLocation, OL3mapsObject.outputLocation) & "'></img>" 'Else HTMLText = ""
            If theSP.p2Type.Text = "Controls" Then HTMLText = theSP.p1ControlOptions.getControlHTML


            convertToHTML = convertToHTML & Ori & "<td  id='table" & theSP.tableID & "_2' style='" & cell2dims(0) & cell2dims(1) & theSP.p2StyleOptions.getstyleCSS & "'><div id='" & theSP.aL2.Text.ToLower.Replace(" ", "") & keyDivNumber & "' style='position:relative;" & cell2dims(0) & cell2dims(1) & "'>" & theSP.p2CollapseButtonHTML & HTMLText & "</div></td>"
            If theSP.Orientation = Orientation.Horizontal Then convertToHTML = convertToHTML & "</tr>"
        End If



        If theSP.Orientation = Orientation.Horizontal Then
            ' convertToHTML = setButtonParameters(convertToHTML, tHeight, 100 - tHeight, 1)
        Else
            convertToHTML = convertToHTML & "</tr>"
            ' convertToHTML = setButtonParameters(convertToHTML, tWidth, 100 - tWidth, 0)
        End If

        convertToHTML = convertToHTML & "</table>"


        'reset labels
        addAllLabels(Sp1)
        Return convertToHTML
    End Function

    Function StripHTMLHead(ByVal htmlText As String) As String
        'remove head eleemnts from auto created html

        StripHTMLHead = ""
        StripHTMLHead = Regex.Replace(htmlText, "</?(!DOCTYPE).*?>", "")
        StripHTMLHead = Regex.Replace(StripHTMLHead, "</?(HTML).*?>", "")
        StripHTMLHead = Regex.Replace(StripHTMLHead, "</?(HEAD).*?>", "")
        StripHTMLHead = Regex.Replace(StripHTMLHead, "</?(META).*?>", "")
        StripHTMLHead = Regex.Replace(StripHTMLHead, "</?(BODY).*?>", "")
    End Function

    Sub addLayerToAllKeys(ByVal theSP As SP, ByVal mapNumber As Integer, ByVal layerNum As Integer)

        'cell 1
        'if contents of panel1 is another splitter, recall this sub, else check for key
        removeAllButtons(theSP.Panel1)
        If theSP.Panel1.Controls.Count > 0 Then
            addLayerToAllKeys(theSP.Panel1.Controls(theSP.Panel1.Controls.Count - 1), mapNumber, layerNum)
        Else
            'if its a key add the new layer
            If theSP.p1Type.Text = "Key" And theSP.p1KeyOptions.CheckBox1.Checked Then
                theSP.p1KeyOptions.addKeyLayer(New TreeNode, mapNumber - 1, layerNum)
            End If

        End If


        'cell2
        removeAllButtons(theSP.Panel2)
        If theSP.Panel2.Controls.Count > 0 Then
            addLayerToAllKeys(theSP.Panel2.Controls(theSP.Panel2.Controls.Count - 1), mapNumber, layerNum)
        Else
            'if its a key add the new layer
            If theSP.p2Type.Text = "Key" And theSP.p2KeyOptions.CheckBox1.Checked Then
                theSP.p2KeyOptions.addKeyLayer(New TreeNode, mapNumber - 1, layerNum)
            End If

        End If

        'reset labels
        addAllLabels(Sp1)

    End Sub

    Sub refreshAllKeysAndControls(ByVal theSP As SP)
        'recursive function to refresh all key - used to force remove items from removed layers

        'cell 1
        'if contents of panel1 is another splitter, recall this sub, else check for key
        removeAllButtons(theSP.Panel1)
        If theSP.Panel1.Controls.Count > 0 Then
            refreshAllKeysAndControls(theSP.Panel1.Controls(theSP.Panel1.Controls.Count - 1))
        Else
            'if its a key refresh
            If theSP.p1Type.Text = "Key" Then
                theSP.p1KeyOptions.refreshKey()
            End If

            If theSP.p1Type.Text = "Controls" Then
                theSP.p1ControlOptions.refreshFilters()
            End If
        End If


        'cell2
        removeAllButtons(theSP.Panel2)
        If theSP.Panel2.Controls.Count > 0 Then
            refreshAllKeysAndControls(theSP.Panel2.Controls(theSP.Panel2.Controls.Count - 1))
        Else
            'if its a key refresh
            If theSP.p2Type.Text = "Key" Then
                theSP.p2KeyOptions.refreshKey()
            End If

            If theSP.p2Type.Text = "Controls" Then
                theSP.p2controloptions.refreshFilters()
            End If
        End If

        'reset labels
        addAllLabels(Sp1)

    End Sub


    Public Sub removeAllButtonsAllPanels(ByVal theSP As SP)
        'cell 1
        removeAllButtons(theSP.Panel1)
        If theSP.Panel1.Controls.Count > 0 Then
            removeAllButtonsAllPanels(theSP.Panel1.Controls(0))
            theSP.Panel1.BackColor = Control.DefaultBackColor
        Else

        End If

        'cell2
        removeAllButtons(theSP.Panel2)
        If theSP.Panel2.Controls.Count > 0 Then
            removeAllButtonsAllPanels(theSP.Panel2.Controls(0))
            theSP.Panel2.BackColor = Control.DefaultBackColor
        Else

        End If
    End Sub

    Sub removeAllButtons(ByVal thePanel As SplitterPanel)
        Dim b1 As New Button
        Dim b2 As New CheckBox
        Dim b3 As New ComboBox
        Dim b4 As New Label
        Dim buttonsToRemove As New List(Of Control)

        For Each item As Control In thePanel.Controls
            If item.GetType.Equals(b1.GetType) Or item.GetType.Equals(b2.GetType) Or item.GetType.Equals(b3.GetType) Or item.GetType.Equals(b4.GetType) Then
                buttonsToRemove.Add(item)
            End If
        Next

        For Each item As Control In buttonsToRemove
            thePanel.Controls.Remove(item)
        Next

    End Sub


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Sp1.properties = Panel1

        Sp1.p1Type.Text = "Key"
        Sp1.p2Type.Text = "Map 1"

        refreshElementList()
    End Sub

    Sub addAllLabels(ByVal theSP As SP)
        'cell 1
        removeAllButtons(theSP.Panel1)
        If theSP.Panel1.Controls.Count > 0 Then
            addAllLabels(theSP.Panel1.Controls(0))
        Else
            theSP.Panel1.Controls.Add(theSP.aL1)
        End If

        'cell2
        removeAllButtons(theSP.Panel2)
        If theSP.Panel2.Controls.Count > 0 Then
            addAllLabels(theSP.Panel2.Controls(0))
        Else
            theSP.Panel2.Controls.Add(theSP.aL2)
        End If
    End Sub

    Function collapseScript() As String
        collapseScript = "<script type='text/javascript'>function collapsePanel(panelID,oppPanelID,one,two,orientation) {if (document.getElementById(panelID).style.display != 'none'){document.getElementById(panelID).style.display = 'none';if (orientation == 0){document.getElementById(panelID).style.width = '0%';document.getElementById(oppPanelID).style.width = '100%';}else{document.getElementById(panelID).style.height = '0%';document.getElementById(oppPanelID).style.height = '100%';}}else{document.getElementById(panelID).style.display = '';if (orientation == 0){document.getElementById(oppPanelID).style.width = one + '%';document.getElementById(panelID).style.width = two + '%';}else{document.getElementById(oppPanelID).style.height = one + '%';document.getElementById(panelID).style.height = two + '%';}}}</script>"
    End Function

    Function setButtonParameters(ByVal buttonString As String, ByVal one As String, ByVal two As String, ByVal orientation As String) As String
        Return buttonString.Replace("999,999,9", one & "," & two & "," & orientation)
    End Function

    Sub refreshElementList()
        elementList.TreeView1.Nodes.Clear()
        elementList.TreeView1.Nodes.Add(elementList.getAmendedElementList(Me.Sp1, Me))
    End Sub

    Public Function save() As OL3LayoutContainerSaveObject
        save = Sp1.save(Me)
    End Function



    Public Sub loadObj(ByVal saveObj As OL3LayoutContainerSaveObject)
        'clear layout designer first to start from scratch -> now done in SP (layout container)
        'now load saved layout designer.
        Sp1.loadObj(saveObj)

        'put labels back on
        addAllLabels(Sp1)
    End Sub


End Class

Class modifiedPanel
    'modified panel to hold layout list string
    Inherits Panel
    Public layoutList As String
    Public OL3MapsObject As OL3Maps


End Class

