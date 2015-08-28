Public Class LayoutDesigner
    Public linkedLayoutPreview As LayoutPreview
    Public WithEvents mapList As New ComboBox
    Public OL3mapsObject As OL3Maps
    Public Event onLayoutItemsChange()

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


    Function convertLayoutToHTML(ByVal theSP As SP) As String
        Dim convertToHTML As String = ""
        Dim Ori As String = ""
        Dim OriEnd As String = ""
        Dim tHeight, tWidth As Integer
        Dim keyDivNumber As String
        Dim HTMLText As String = ""

        convertToHTML = convertToHTML & "<table style='height:100%;width:100%;'>"
        If theSP.Orientation = Orientation.Horizontal Then
            Ori = "<tr>"
            OriEnd = "</tr>"
        Else
            convertToHTML = convertToHTML & "<tr>"
        End If


        'cell 1
        'if contents of panel1 is another splitter, recall this sub, else create div
        removeAllButtons(theSP.Panel1)
        If theSP.Panel1.Controls.Count > 0 Then
            convertToHTML = convertToHTML & "<td id='table" & theSP.tableID & "_1' style='width:" & CInt((theSP.Panel1.Width / theSP.Width) * 100) & "%;'>" & convertLayoutToHTML(theSP.Panel1.Controls(theSP.Panel1.Controls.Count - 1)) & "</td>"
        Else
            'if its a key get the key num , else returns blank
            keyDivNumber = addKeyObjectLiterol(theSP.aL1, theSP.p1KeyOptions)
            If theSP.p1Type.Text = "Text" Then HTMLText = theSP.p1Text.HTMLedit.DocumentText Else  'HTMLText = ""
            If theSP.p1Type.Text = "Image" Then HTMLText = "<img style='max-width:100%;max-height:100%;' src='" & theSP.p1ImageType.getPath(theSP.p1Img.ImageLocation, OL3mapsObject.outputLocation) & "'></img>" ' Else HTMLText = ""
            If theSP.p1Type.Text = "Controls" Then HTMLText = theSP.p1ControlOptions.getControlHTML

            If theSP.Orientation = Orientation.Vertical Then tHeight = 100 : tWidth = CInt((theSP.Panel1.Width / theSP.Width) * 100)
            If theSP.Orientation = Orientation.Horizontal Then tHeight = CInt((theSP.Panel1.Height / theSP.Height) * 100) : tWidth = 100

            convertToHTML = convertToHTML & Ori & "<td  id='table" & theSP.tableID & "_1' style='height:" & tHeight & "%;width:" & tWidth & "%;" & theSP.p1StyleOptions.getstyleCSS & "'><div id='" & theSP.aL1.Text.ToLower.Replace(" ", "") & keyDivNumber & "' style='position:relative;overflow:auto;height:100%;width:100%;'>" & theSP.p1CollapseButtonHTML & HTMLText & "</div></td>"

            If theSP.Orientation = Orientation.Horizontal Then
                convertToHTML = convertToHTML & "</tr>"
                convertToHTML = setButtonParameters(convertToHTML, tHeight, 100 - tHeight, 1)
            Else
                convertToHTML = setButtonParameters(convertToHTML, tWidth, 100 - tWidth, 0)
            End If

        End If


        'cell2
        removeAllButtons(theSP.Panel2)
        HTMLText = ""
        If theSP.Panel2.Controls.Count Then
            convertToHTML = convertToHTML & Ori & "<td  id='table" & theSP.tableID & "_2'  style='width:" & CInt((theSP.Panel2.Width / theSP.Width) * 100) & "%;'>" & convertLayoutToHTML(theSP.Panel2.Controls(theSP.Panel2.Controls.Count - 1)) & "</td>" & OriEnd
        Else
            'if its a key get the key num , else returns blank
            keyDivNumber = addKeyObjectLiterol(theSP.aL2, theSP.p2KeyOptions)
            If theSP.p2Type.Text = "Text" Then HTMLText = theSP.p2Text.HTMLedit.DocumentText 'Else HTMLText = ""
            If theSP.p2Type.Text = "Image" Then HTMLText = "<img style='max-width:100%;max-height:100%;' src='" & theSP.p2ImageType.getPath(theSP.p2Img.ImageLocation, OL3mapsObject.outputLocation) & "'></img>" 'Else HTMLText = ""
            If theSP.p2Type.Text = "Controls" Then HTMLText = theSP.p1ControlOptions.getControlHTML

            If theSP.Orientation = Orientation.Vertical Then tHeight = 100 : tWidth = CInt((theSP.Panel2.Width / theSP.Width) * 100)
            If theSP.Orientation = Orientation.Horizontal Then tHeight = CInt((theSP.Panel2.Height / theSP.Height) * 100) : tWidth = 100

            convertToHTML = convertToHTML & Ori & "<td  id='table" & theSP.tableID & "_2' style='height:" & tHeight & "%;width:" & tWidth & "%;" & theSP.p2StyleOptions.getstyleCSS & "'><div id='" & theSP.aL2.Text.ToLower.Replace(" ", "") & keyDivNumber & "' style='position:relative;height:100%;width:100%;'>" & theSP.p2CollapseButtonHTML & HTMLText & "</div></td>"
            If theSP.Orientation = Orientation.Horizontal Then convertToHTML = convertToHTML & "</tr>"
        End If



        If theSP.Orientation = Orientation.Horizontal Then
            convertToHTML = setButtonParameters(convertToHTML, tHeight, 100 - tHeight, 1)
        Else
            convertToHTML = convertToHTML & "</tr>"
            convertToHTML = setButtonParameters(convertToHTML, tWidth, 100 - tWidth, 0)
        End If

        convertToHTML = convertToHTML & "</table>"


        'reset labels
        addAllLabels(Sp1)
        Return convertToHTML
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
End Class

Class modifiedPanel
    'modified panel to hold layout list string
    Inherits Panel
    Public layoutList As String
    Public OL3MapsObject As OL3Maps


End Class

