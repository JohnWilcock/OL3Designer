Imports System.IO
Imports System.Security.Cryptography
Imports System.Xml.Serialization
Imports System.Runtime.Serialization.Formatters.Binary

<Serializable()> Public Class OL3Maps
    Public mapList As New List(Of OL3LayerList)
    Public linkedBox As ToolStripComboBox 'ComboBox
    'Public layerControl As OL3LayerList
    Public layerPanel As Panel
    Public currentSelectedIndex As Integer
    Public layout As LayoutDesigner
    Public outputLocation As String


    Sub New(ByRef linkedCB As ToolStripComboBox, ByVal linkedLayerPanel As Panel, ByVal theLayout As LayoutDesigner)
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

    Sub refreshList()
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

        'now replace default icon filepaths with new ones and copy them to output folder
        Dim hf As New HelperFunctions
        getJS = hf.replaceIconPathsWithOutputPaths(getJS, outPath)

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



    Function createOutputFile(ByVal outputPath As String, ByVal theJS As String, ByVal theHTML As String) As Boolean
        createOutputFile = 0
        outputLocation = outputPath


        If My.Settings.MapOverwrite Then
            If File.Exists(outputPath) Then
                If MsgBoxResult.Cancel = MsgBox("File exists, overide ?", MsgBoxStyle.OkCancel, "File exists") Then
                    Return 0
                End If
            End If
        End If


        'write the external javascript libraries
        Dim helper As New HelperFunctions
        Dim scriptTags As String = helper.writeAllLibraries(outputPath)

        theHTML = "<!doctype html><html style='height: 100%;'><head>" & scriptTags & layout.collapseScript & "</head><body style='height: 100%; width:100%'>" & theHTML & ""
        System.IO.File.WriteAllText(outputPath, theHTML & theJS & "</body></html>" & vbCrLf)
        Return 1
    End Function

    Function isLayerDuplicated(ByVal layerPath As String, ByVal mapNum As Integer, ByVal layerNum As Integer) As Integer() 'returns lowest number duplicate mapnum & layerNum. -1 if no duplicate found
        'by checking this, the application can divert a layer to the source of a different layer, this avoids duplicating long geoJson strings 

        Dim theLayer As OLLayer

        'for each map
        For u As Integer = 0 To mapList.Count - 1

            'for each layer
            For a As Integer = 0 To mapList(u).DataGridView1.Rows.Count - 1
                'do files match  
                theLayer = mapList(u).DataGridView1.Rows(a)
                If FileCompare(layerPath, theLayer.OL3LayerPath) Then ' identical layer sources

                    'if so, are map num and layer num identical - if then not a duplicate
                    If mapNum = u + 1 And layerNum = a Then
                        'layer is querying itself
                        Return {-1, -1}
                    Else
                        Return {u, a}
                    End If

                Else 'layers not identical - do  nothing


                End If



            Next

        Next

        Return {-1, -1}
    End Function



    'https://support.microsoft.com/en-us/kb/320348
    Private Function FileCompare(file1 As String, file2 As String) As Boolean
        Dim file1byte As Integer
        Dim file2byte As Integer
        Dim fs1 As FileStream
        Dim fs2 As FileStream

        ' Determine if the same file was referenced two times.
        If file1 = file2 Then
            ' Return true to indicate that the files are the same.
            Return True
        End If

        ' Open the two files.
        fs1 = New FileStream(file1, FileMode.Open)
        fs2 = New FileStream(file2, FileMode.Open)

        ' Check the file sizes. If they are not the same, the files 
        ' are not the same.
        If fs1.Length <> fs2.Length Then
            ' Close the file
            fs1.Close()
            fs2.Close()

            ' Return false to indicate files are different
            Return False
        End If

        ' Read and compare a byte from each file until either a
        ' non-matching set of bytes is found or until the end of
        ' file1 is reached.
        Do
            ' Read one byte from each file.
            file1byte = fs1.ReadByte()
            file2byte = fs2.ReadByte()
        Loop While (file1byte = file2byte) AndAlso (file1byte <> -1)

        ' Close the files.
        fs1.Close()
        fs2.Close()

        ' Return the success of the comparison. "file1byte" is 
        ' equal to "file2byte" at this point only if the files are 
        ' the same.
        Return ((file1byte - file2byte) = 0)
    End Function




    Public Function save() As OL3MapListSaveObject
        save = New OL3MapListSaveObject

        For y As Integer = 0 To mapList.Count - 1
            save.mapList.Add(mapList(y).save())
        Next

        save.theLayoutDesigner = layout.save()

    End Function


    Public Sub loadObj(ByVal saveObj As OL3MapListSaveObject)

        mapList.Clear()
        linkedBox.Items.Clear()
        For y As Integer = 0 To saveObj.mapList.Count - 1
            Me.add()
            mapList(mapList.Count - 1).loadObj(saveObj.mapList(y))
        Next

        refreshList()
        If mapList.Count > 0 Then
            linkedBox.SelectedIndex = 0
        End If

        layout.loadObj(saveObj.theLayoutDesigner)
        'mapChanged()
    End Sub


    Public Sub deserialize()
        'load string from file sys
        Dim OFD As New OpenFileDialog
        OFD.Filter = "OL3 Designer files (*.OL3)|*.ol3"
        OFD.Title = "Open OL3 Designer file"
        OFD.Multiselect = False

        If OFD.ShowDialog = Windows.Forms.DialogResult.OK Then


            'load  to map object
            Dim loadedObj As OL3MapListSaveObject
            Dim TestFileStream As Stream = File.OpenRead(OFD.FileName)
            Dim deserializer As New BinaryFormatter
            loadedObj = CType(deserializer.Deserialize(TestFileStream), OL3MapListSaveObject)
            TestFileStream.Close()

            loadObj(loadedObj)


        End If
    End Sub

    Public Sub serialize()

        'save as file
        Dim SFD As New SaveFileDialog
        SFD.DefaultExt = "OL3"
        SFD.Filter = "OL3 Designer files (*.OL3)|*.ol3"
        SFD.AddExtension = True
        If SFD.ShowDialog <> DialogResult.OK Then Exit Sub


        'serialize save object
        Dim TestFileStream As Stream = File.Create(SFD.FileName)
        Dim serializer As New BinaryFormatter
        Dim sObj As Object = save()
        serializer.Serialize(TestFileStream, sObj)
        TestFileStream.Close()
    End Sub



End Class




<Serializable()> _
Public Class OL3MapListSaveObject
    Public mapList As New List(Of OL3LayerListSaveObject)
    Public theLayoutDesigner As OL3LayoutContainerSaveObject
End Class