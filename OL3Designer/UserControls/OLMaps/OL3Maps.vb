Imports System.IO

<Serializable()> Public Class OL3Maps
    Public mapList As New List(Of OL3LayerList)
    Public linkedBox As ComboBox
    'Public layerControl As OL3LayerList
    Public layerPanel As Panel
    Public currentSelectedIndex As Integer
    Public layout As LayoutDesigner
    Public outputLocation As String


    Sub New(ByRef linkedCB As ComboBox, ByVal linkedLayerPanel As Panel, ByVal theLayout As LayoutDesigner)
        linkedBox = linkedCB
        'layerControl = layerC

        layerPanel = linkedLayerPanel
        layerPanel.Controls.Clear()
        add()

        'add change handler to combo box
        AddHandler linkedBox.SelectedIndexChanged, AddressOf mapChanged
        AddHandler linkedBox.TextChanged, AddressOf mapTextChanged

        'set as first map (change handler will tigger the rest
        linkedBox.SelectedIndex = 0

        'set designer variable if available
        layout = theLayout
        layout.mapList = linkedCB

        'put map object reference in layout designer
        layout.OL3mapsObject = Me
        layout.Sp1.p1KeyOptions.OL3mapsObject = Me

        refreshList()
    End Sub


    Sub add()
        mapList.Add(New OL3LayerList)
        mapList(mapList.Count - 1).mapNumber = mapList.Count
        mapList(mapList.Count - 1).mapName = "Map " & mapList.Count
        mapList(mapList.Count - 1).parentMapList = Me
        mapList(mapList.Count - 1).Dock = DockStyle.Fill


        'refresh and add map to combo box
        refreshList()

    End Sub

    Sub remove(ByVal index As Integer)
        mapList.Remove(mapList(index))
    End Sub

    Public Function getKeyJS() As String

        ' read contents of embedded file
        Dim keyString As String = My.Resources.keyFunction
        Return keyString

    End Function

    Function getGetExtentFunction() As String


        getGetExtentFunction = ""
        ' read contents of embedded file
        Dim ExtentFunction As String = My.Resources.extentFunction

        Return ExtentFunction


    End Function

    Private Sub refreshList()
        If Not linkedBox Is Nothing Then

            Dim currentItem As Integer = linkedBox.SelectedIndex
            linkedBox.Items.Clear()
            For x As Integer = 0 To mapList.Count - 1
                linkedBox.Items.Add(mapList(x).mapName)
            Next

            linkedBox.SelectedIndex = currentItem
        End If


        'now send new map list to layout designer
        Dim layoutItemsList As String = ""
        For i As Integer = 0 To linkedBox.Items.Count - 1
            layoutItemsList = layoutItemsList & "," & linkedBox.Items.Item(i).ToString
        Next
        'add standard items
        layoutItemsList = (layoutItemsList & "," & "Key,Controls,Image,Text").Substring(1)
        'send to layout designer
        If layout IsNot Nothing Then
            layout.layoutList = layoutItemsList
        End If

    End Sub

    'change layer list to correct map
    Sub mapChanged()
        layerPanel.Controls.Clear()
        layerPanel.Controls.Add(mapList(linkedBox.SelectedIndex))
        'layerControl = mapList(linkedBox.SelectedIndex)

        currentSelectedIndex = linkedBox.SelectedIndex


    End Sub

    Sub mapTextChanged()
        'change relevant map name and refresh list 
        'If mapList(currentSelectedIndex).mapName <> linkedBox.Text And mapList(currentSelectedIndex).mapName <> "" Then
        '    mapList(currentSelectedIndex).mapName = linkedBox.Text
        '    refreshList()
        'End If
    End Sub


    '********************************************************************
    '********************************************************************
    '********************************************************************
    'These function amalgomate the info from all maps to create the map parameters
    '********************************************************************
    '********************************************************************
    '********************************************************************


    Function getAllMapParameters() As String
        'cycle through each map and get the parameters js

    End Function


    '********************************************************************
    '********************************************************************
    '********************************************************************
    'These function produce the final html and js for the end output
    '********************************************************************
    '********************************************************************
    '********************************************************************


    Function getHTML() As String
        getHTML = ""

        getHTML = getHTML & layout.getOutputHTML(layout.Sp1)

    End Function


    Function getJS(ByVal outName As String, Optional ByVal outPath As String = "") As String
        getJS = "<script type='text/javascript'>" & Chr(10)

        Dim gapStr As String = New String(Chr(10), 5)

        'original output save path
        getJS = getJS & "//*************original output path DO NOT ALTER EVEN IF YOU MOVE THE FILE, required to determine relative paths*****************" & Chr(10) & "mapOutputPath = '" & (outPath & "\").Replace("\", "/") & "';" & Chr(10) & Chr(10)


        'layer style types
        getJS = getJS & "//*************layer Style Types*****************" & Chr(10) & getAllStyleTypesLiterolAllMaps()

        For x As Integer = 0 To mapList.Count - 1
            'cycle through each map and get all the javascript
            getJS = getJS & gapStr & "//************** MAP " & x + 1 & "*******************>>>" & Chr(10) & mapList(x).getAllJS(outName, outPath)
        Next

        'dump extent functions and key functions
        getJS = getJS & Chr(10) & "//****************Extent Functions ********************" & Chr(10) & getGetExtentFunction() & Chr(10)
        getJS = getJS & Chr(10) & "//****************Key Functions************************" & Chr(10) & getKeyJS() & Chr(10)

        'initilise keys
        getHTML() ' needed to ini keys
        getJS = getJS & "//*************Keys*****************" & Chr(10) & layout.keyObjectLiterols & Chr(10) & Chr(10)
        getJS = getJS & "//*************Full key refresh*****************" & Chr(10)
        getJS = getJS & "function refreshAllKeys(){var numKeys = " & layout.keyCount & ";for (o = 0; o < numKeys; o++) {if (document.getElementById('key' + o) != null){refreshKey('key' + o);}}}" & Chr(10) & Chr(10)


        getJS = getJS & Chr(10) & "</script>"
    End Function


    Function getAllStyleTypesLiterolAllMaps() As String
        getAllStyleTypesLiterolAllMaps = ""
        Dim heatmapParams As String = ""

        For n As Integer = 0 To mapList.Count - 1
            getAllStyleTypesLiterolAllMaps = getAllStyleTypesLiterolAllMaps & "," & mapList(n).getAllStyleTypesLiterol
            heatmapParams = heatmapParams & "," & mapList(n).getAllHeatmapParameters
        Next

        getAllStyleTypesLiterolAllMaps = "var mapStyleTypes = {" & getAllStyleTypesLiterolAllMaps.Substring(1) & "}" & Chr(10) & Chr(10)
        getAllStyleTypesLiterolAllMaps = getAllStyleTypesLiterolAllMaps & "var mapHeatmapParameters = {" & heatmapParams.Substring(1) & "}" & Chr(10) & Chr(10)

    End Function



    Sub createOutputFile(ByVal outputPath As String, ByVal theJS As String, ByVal theHTML As String)
        outputLocation = outputPath



        If File.Exists(outputPath) Then
            If MsgBoxResult.Cancel = MsgBox("File exists, overide ?", MsgBoxStyle.OkCancel, "File exists") Then
                Exit Sub
            End If
        End If

        'write the external javascript libraries
        Dim helper As New HelperFunctions
        Dim scriptTags As String = helper.writeAllLibraries(outputPath)

        theHTML = "<!doctype html><html style='height: 100%;'><body style='height: 100%;'><head>" & scriptTags & layout.collapseScript & "</head><body>" & theHTML & ""
        System.IO.File.WriteAllText(outputPath, theHTML & theJS & "</body></html>" & vbCrLf)

    End Sub

End Class
