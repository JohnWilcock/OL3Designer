Imports System.IO

Public Class OL3LayerStylePicker
    Public layerPath As String
    Public LayerType As String
    Dim currentStyleRow As styleRow
    Public currentStyleIndex As Integer = 0


    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        TreeView1.SelectedNode = TreeView1.TopNode


        DataGridView1.Rows.Add(New styleRow)
        DataGridView1.Rows(0).Cells(0).Value = "Style 1"
        DataGridView1.Rows(0).Selected = True


    End Sub

    Function checkType(ByVal theType As Type) As Boolean
        'check for existing user control in panel1, do not fire if the same as prsently selected
        If Panel1.Controls.Count > 0 Then
            If Panel1.Controls(0).GetType.Equals(theType) Then
                Return True
            End If
        End If

        Return False
    End Function

    Function areClustersPresentInAnyStyleRow() As Boolean
        areClustersPresentInAnyStyleRow = False
        Dim theRow As styleRow
        For t As Integer = 0 To DataGridView1.Rows.Count - 1
            theRow = DataGridView1.Rows(t)
            If theRow.StyleType = "Cluster and Statistics" Or theRow.StyleType = "Simple Cluster" Then
                Return True
            End If
        Next

    End Function

    Function areHeatmapsPresentInAnyStyleRow() As Boolean
        areHeatmapsPresentInAnyStyleRow = False
        Dim theRow As styleRow
        For t As Integer = 0 To DataGridView1.Rows.Count - 1
            theRow = DataGridView1.Rows(t)
            If theRow.StyleType = "Heatmap" Then
                Return True
            End If
        Next

    End Function

    Function isHeatmap(ByVal rowNum As Integer) As Boolean
        Dim theRow As styleRow
        theRow = DataGridView1.Rows(rowNum)
        If theRow.StyleType = "Heatmap" Then
            Return True
        Else
            Return False
        End If
    End Function


    Function isCluster(ByVal rowNum As Integer) As Boolean
        Dim theRow As styleRow
        theRow = DataGridView1.Rows(rowNum)
        If theRow.StyleType = "Cluster and Statistics" Or theRow.StyleType = "Simple Cluster" Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub TreeView1_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles TreeView1.AfterSelect
        newStyleSelection()
    End Sub

    Public Sub newStyleSelection()
        If DataGridView1.Rows.Count > currentStyleIndex Then
            currentStyleRow = DataGridView1.Rows(currentStyleIndex)
        Else
            If DataGridView1.Rows.Count > 0 Then
                currentStyleRow = DataGridView1.Rows(DataGridView1.Rows.Count - 1)
                currentStyleIndex = DataGridView1.Rows.Count - 1
            Else
                Exit Sub
            End If
        End If

        Select Case TreeView1.SelectedNode.Text
            Case "Single Feature Style"

                'check for existing user control in panel1, do not fire if the same as prsently selected
                If checkType(GetType(OL3LayerStyleFeatureStyle)) Then Exit Sub

                Panel1.Controls.Clear()
                Dim newFeatureStyle As New OL3LayerStyleFeatureStyle(LayerType, layerPath)
                newFeatureStyle.TextBox1.Text = Path.GetFileNameWithoutExtension(layerPath)
                currentStyleRow.StyleControl = newFeatureStyle 'New OL3LayerStyleFeatureStyle(LayerType)
                newFeatureStyle.layerPath = layerPath

                currentStyleRow.StyleType = "Single Feature Style"
                Panel1.Controls.Add(currentStyleRow.StyleControl)
            Case "Unique Values"

                'check for existing user control in panel1, do not fire if the same as prsently selected
                If checkType(GetType(OL3LayerStyleUniqueValues)) Then Exit Sub

                Panel1.Controls.Clear()
                Dim newUniqueValues As New OL3LayerStyleUniqueValues(LayerType)
                newUniqueValues.layerPath = layerPath
                currentStyleRow.StyleControl = newUniqueValues
                'currentStyleRow.StyleControl = New OL3LayerStyleUniqueValues

                currentStyleRow.StyleType = "Unique Values"
                Panel1.Controls.Add(currentStyleRow.StyleControl)
            Case "Numeric Ranges"

                'check for existing user control in panel1, do not fire if the same as prsently selected
                If checkType(GetType(OL3LayerStyleNumericRanges)) Then Exit Sub

                Panel1.Controls.Clear()
                Dim newNumericRanges As New OL3LayerStyleNumericRanges(LayerType, layerPath)
                newNumericRanges.layerPath = layerPath
                currentStyleRow.StyleControl = newNumericRanges
                'currentStyleRow.StyleControl = New OL3LayerStyleUniqueValues

                currentStyleRow.StyleType = "Numeric Ranges"
                Panel1.Controls.Add(currentStyleRow.StyleControl)

            Case "Date Ranges"

                'check for existing user control in panel1, do not fire if the same as prsently selected
                If checkType(GetType(OL3LayerStyleDateRanges)) Then Exit Sub

                Panel1.Controls.Clear()
                Dim newdateRanges As New OL3LayerStyleDateRanges(LayerType, layerPath)
                newdateRanges.layerPath = layerPath
                currentStyleRow.StyleControl = newdateRanges
                'currentStyleRow.StyleControl = New OL3LayerStyleUniqueValues

                currentStyleRow.StyleType = "Date Ranges"
                Panel1.Controls.Add(currentStyleRow.StyleControl)


            Case "Simple Cluster"

                'check for existing user control in panel1, do not fire if the same as prsently selected
                If checkType(GetType(OL3LayerStyleCluster)) Then Exit Sub

                Panel1.Controls.Clear()
                Dim newClusterStyle As New OL3LayerStyleCluster(LayerType, layerPath)
                newClusterStyle.layerPath = layerPath
                currentStyleRow.StyleControl = newClusterStyle
                'currentStyleRow.StyleControl = New OL3LayerStyleUniqueValues

                currentStyleRow.StyleType = "Simple Cluster"
                Panel1.Controls.Add(currentStyleRow.StyleControl)

            Case "Cluster and Statistics"

                'check for existing user control in panel1, do not fire if the same as prsently selected
                If checkType(GetType(OL3LayerStyleClusterStats)) Then Exit Sub

                Panel1.Controls.Clear()
                Dim newClusterStyle As New OL3LayerStyleClusterStats(LayerType, layerPath)

                If newClusterStyle.ComboBox1.Text = "" Then
                    'it only gets to this point if no numeric fields are found
                    MsgBox("No Numeric Fields Found" & vbNewLine & "Defaulting to Simple Cluster")
                    TreeView1.SelectedNode = TreeView1.Nodes(3).Nodes(0)
                    Exit Sub
                End If


                newClusterStyle.layerPath = layerPath
                currentStyleRow.StyleControl = newClusterStyle
                'currentStyleRow.StyleControl = New OL3LayerStyleUniqueValues

                currentStyleRow.StyleType = "Cluster and Statistics"
                Panel1.Controls.Add(currentStyleRow.StyleControl)

            Case "Heatmap"

                'check for existing user control in panel1, do not fire if the same as prsently selected
                If checkType(GetType(OL3LayerStyleHeatmap)) Then Exit Sub

                Panel1.Controls.Clear()
                Dim newHeatmapStyle As New OL3LayerStyleHeatmap(layerPath)
                currentStyleRow.StyleControl = newHeatmapStyle
                'currentStyleRow.StyleControl = New OL3LayerStyleUniqueValues

                currentStyleRow.StyleType = "Heatmap"
                Panel1.Controls.Add(currentStyleRow.StyleControl)
        End Select

    End Sub



    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        addLayerStyleRow()
    End Sub

    Private Sub addLayerStyleRow()
        Panel1.Controls.Clear()
        Dim newStyleRow As New styleRow
        newStyleRow.StyleControl = New OL3LayerStyleFeatureStyle(LayerType, layerPath)
        Dim rowID As Integer = DataGridView1.Rows.Add(newStyleRow)
        currentStyleIndex = rowID

        'Panel1.Controls.Add(newStyleRow.StyleControl)
        TreeView1.SelectedNode = TreeView1.Nodes(0)
        DataGridView1.Rows(rowID).Selected = True
        newStyleSelection()

        DataGridView1.Rows(rowID).Cells(0).Value = "New Style"




    End Sub


    Private Sub layerStyleRowChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.ColumnIndex = 1 Then
            removeStyleRow(e.RowIndex)
            If e.RowIndex > 0 Then
                styleRowChanged(e.RowIndex - 1)
            Else
                'If DataGridView1.Rows.Count > 1 Then
                '    styleRowChanged(e.RowIndex + 1)
                'Else
                '    styleRowChanged(0)
                'End If

            End If

        ElseIf e.ColumnIndex = 2 Then
            styleRowChanged(e.RowIndex)
        End If


    End Sub

    Sub removeStyleRow(ByVal rowIndex As Integer)
        If rowIndex > 0 Or DataGridView1.Rows.Count > 1 Then
            If rowIndex > 0 Then
                DataGridView1.Rows(rowIndex - 1).Selected = True
            Else
                DataGridView1.Rows(rowIndex + 1).Selected = True
            End If


            DataGridView1.Rows(rowIndex).Selected = False
            DataGridView1.Rows.RemoveAt(rowIndex)
        End If

    End Sub

    Private Sub DGV1Selected(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        'DataGridView1.ClearSelection()
    End Sub

    Sub styleRowChanged(ByVal theRow As Integer)
        'If DataGridView1.SelectedRows.Count = 0 Then Exit Sub

        Dim currentStyleRow1 As styleRow = DataGridView1.Rows(theRow)

        Panel1.Controls.Clear()
        Panel1.Controls.Add(currentStyleRow1.StyleControl)
        currentStyleIndex = theRow
    End Sub

    'places X and edit pencil in remove column button cell
    Private Sub removeRow_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles DataGridView1.CellPainting
        If e.ColumnIndex = 1 AndAlso e.RowIndex >= 0 Then
            e.Paint(e.CellBounds, DataGridViewPaintParts.All)

            Dim bmpFind As Bitmap = My.Resources.Offline_16xLG__2
            e.Graphics.DrawImage(bmpFind, e.CellBounds.Left + 2, e.CellBounds.Top + 2)
            e.Handled = True
        End If

        If e.ColumnIndex = 2 AndAlso e.RowIndex >= 0 Then
            e.Paint(e.CellBounds, DataGridViewPaintParts.All)

            Dim bmpFind As Bitmap = My.Resources.PencilTool_206
            e.Graphics.DrawImage(bmpFind, e.CellBounds.Left + 2, e.CellBounds.Top + 2)
            e.Handled = True
        End If
    End Sub


    Function getAllLabelExpresions() As List(Of String)
        getAllLabelExpresions = New List(Of String)

        Dim currentStyle As styleRow
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            currentStyle = DataGridView1.Rows(i)
            getAllLabelExpresions.Add(getSingleLabelExpresion(currentStyle.StyleControl, currentStyle.StyleType))
        Next

    End Function

    Function getAllLabelVars() As List(Of String)
        getAllLabelVars = New List(Of String)

        Dim currentStyle As styleRow
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            currentStyle = DataGridView1.Rows(i)
            getAllLabelVars.Add(getsingleLabelVars(currentStyle.StyleControl, currentStyle.StyleType))
        Next

    End Function



    Function getAllStyles() As List(Of String)
        getAllStyles = New List(Of String)
        Dim currentStyle As styleRow

        'see if multiple sytles apply to layer
        'If DataGridView1.Rows.Count > 1 Then
        'multiple styles are present , cycle through all styles in DGV1
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            currentStyle = DataGridView1.Rows(i)
            getAllStyles.Add(getSingleStyle(currentStyle.StyleControl, currentStyle.StyleType))
        Next

        'Else
        '    'only one style for layer
        '    currentStyle = DataGridView1.Rows(0)
        '    getAllStyles.Add(getSingleStyle(currentStyle.StyleControl, currentStyle.StyleType))
        'End If

    End Function

    Function getAllConditions() As List(Of String)
        'sub gets all the various conditions for each style 
        getAllConditions = New List(Of String)
        Dim currentStyle As styleRow


        'for each dynamic style
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            currentStyle = DataGridView1.Rows(i)
            getAllConditions.Add(getSingleCondition(currentStyle.StyleControl, currentStyle.StyleType))
        Next
    End Function

    Function getPreConditionCode(ByVal theMap As String, ByVal theLayer As String, ByVal theStyleRow As Integer) As String
        'gets code which runs before the conditions are interrogated in the style functions
        getPreConditionCode = ""
        Dim theClusterStyleRow As styleRow
        Dim clusterStyleUserControl As OL3LayerStyleCluster
        Dim clusterStatsStyleUserControl As OL3LayerStyleClusterStats

        'is layer in group where clusters are used. if so set source before anything else
        If areClustersPresentInAnyStyleRow() Then
            'is it a cluster source or vector source
            If isCluster(theStyleRow) Then
                'if cluster source 
                'set cluster source
                getPreConditionCode = getPreConditionCode & "map" & theMap & "_vectorLayer_" & theLayer & ".setSource(map" & theMap & "_clusterSource_" & theLayer & "_Style" & theStyleRow & ");" & Chr(10)

                'if its a stat cluster then set the correct stat var
                theClusterStyleRow = DataGridView1.Rows(theStyleRow)
                If theClusterStyleRow.StyleType = "Simple Cluster" Then
                    clusterStyleUserControl = theClusterStyleRow.StyleControl
                Else
                    clusterStatsStyleUserControl = theClusterStyleRow.StyleControl
                    getPreConditionCode = getPreConditionCode & "statValue = returnClusterStatistics(feature,'" & clusterStatsStyleUserControl.ComboBox1.Text & "','" & clusterStatsStyleUserControl.ComboBox3.Text & "');"
                End If



            Else
                'if not cluster set vector source
                getPreConditionCode = getPreConditionCode & "map" & theMap & "_vectorLayer_" & theLayer & ".setSource(map" & theMap & "_vectorSource_" & theLayer & ");"
            End If
        End If
        getPreConditionCode = getPreConditionCode & Chr(10) & Chr(10)


    End Function

    Function getsingleLabelVars(ByVal StyleControl As UserControl, ByVal StyleType As String, Optional ByVal styleNum As Integer = 0) As String
        'a case for each feature type

        Select Case StyleType

            Case "Single Feature Style"
                Dim currentUserControl As OL3LayerStyleFeatureStyle = StyleControl
                Return "var tempLabel" & styleNum & " = '';"

            Case "Unique Values"
                Dim currentUserControl As OL3LayerStyleUniqueValues = StyleControl

                Dim allUniques As String = ""
                Dim tempUniqueRow As uniqueRow
                'for each unique style row
                For g As Integer = 0 To currentUserControl.DataGridView1.Rows.Count - 1
                    tempUniqueRow = currentUserControl.DataGridView1.Rows(g)
                    allUniques = allUniques & "var tempLabel" & g & " = '';"
                Next
                Return allUniques

            Case "Numeric Ranges"
                Dim currentUserControl As OL3LayerStyleNumericRanges = StyleControl

                Dim allNumericRanges As String = ""
                Dim tempNumericRangesRow As NumericRangesRow
                'for each unique style row
                For g As Integer = 0 To currentUserControl.DataGridView1.Rows.Count - 1
                    tempNumericRangesRow = currentUserControl.DataGridView1.Rows(g)
                    allNumericRanges = allNumericRanges & "var tempLabel" & g & " = '';"
                Next
                Return allNumericRanges

            Case "Date Ranges"
                Dim currentUserControl As OL3LayerStyleDateRanges = StyleControl

                Dim allDateRanges As String = ""
                Dim tempDateRangesRow As DateRangesRow
                'for each unique style row
                For g As Integer = 0 To currentUserControl.DataGridView1.Rows.Count - 1
                    tempDateRangesRow = currentUserControl.DataGridView1.Rows(g)
                    allDateRanges = allDateRanges & "var tempLabel" & g & " = '';"
                Next
                Return allDateRanges

            Case "Simple Cluster"
                Dim currentUserControl As OL3LayerStyleCluster = StyleControl
                'cycle through all style pickers getting multistylestrings...hmm do in unique style usercontrol

                Return "var tempLabel" & styleNum & " = '';"


            Case "Cluster and Statistics"
                Dim currentUserControl As OL3LayerStyleClusterStats = StyleControl
                'cycle through all style pickers getting multistylestrings...hmm do in unique style usercontrol


                If currentUserControl.ComboBox1.Text <> "None" Then
                    'if stats then cycle through rows
                    Dim allClusterRanges As String = ""
                    Dim tempClusterRangesRow As ClusterStatsRow
                    'for each unique style row
                    For g As Integer = 0 To currentUserControl.DataGridView1.Rows.Count - 1
                        tempClusterRangesRow = currentUserControl.DataGridView1.Rows(g)
                        allClusterRanges = allClusterRanges & "var tempLabel" & g & " = '';"
                    Next
                    Return allClusterRanges
                Else
                    Return "var tempLabel" & styleNum & " = '';"
                End If
        End Select
    End Function

    

    Function getSingleLabelExpresion(ByVal StyleControl As UserControl, ByVal StyleType As String, Optional ByVal styleNum As Integer = 0) As String
        'a case for each feature type
        'add conditional resolution "If" before assigning vars to ensurethey only dispay when needed.
        Select Case StyleType

            Case "Single Feature Style"
                Dim currentUserControl As OL3LayerStyleFeatureStyle = StyleControl
                Return currentUserControl.OlStylePicker1.ChangeOLStylePickerdialog.getLabelResolution & "{ tempLabel" & styleNum & " = " & currentUserControl.OlStylePicker1.getLabelExpresion & "} "

            Case "Unique Values"
                Dim currentUserControl As OL3LayerStyleUniqueValues = StyleControl
                'cycle through all style pickers getting label expesions (i.e column to label features with)

                Dim allUniques As String = ""
                Dim tempUniqueRow As uniqueRow
                'for each unique style row
                For g As Integer = 0 To currentUserControl.DataGridView1.Rows.Count - 1
                    tempUniqueRow = currentUserControl.DataGridView1.Rows(g)
                    allUniques = allUniques & tempUniqueRow.uniqueStyle.ChangeOLStylePickerdialog.getLabelResolution & "{ tempLabel" & g & " = " & tempUniqueRow.uniqueStyle.getLabelExpresion & ";} "
                Next
                Return allUniques

            Case "Numeric Ranges"
                Dim currentUserControl As OL3LayerStyleNumericRanges = StyleControl

                Dim allNumericRanges As String = ""
                Dim tempNumericRangesRow As NumericRangesRow
                'for each unique style row
                For g As Integer = 0 To currentUserControl.DataGridView1.Rows.Count - 1
                    tempNumericRangesRow = currentUserControl.DataGridView1.Rows(g)
                    allNumericRanges = allNumericRanges & tempNumericRangesRow.NumericRangesStyle.ChangeOLStylePickerdialog.getLabelResolution & "{ tempLabel" & g & " = " & tempNumericRangesRow.NumericRangesStyle.getLabelExpresion & ";}"
                Next
                Return allNumericRanges

            Case "Date Ranges"
                Dim currentUserControl As OL3LayerStyleDateRanges = StyleControl

                Dim allDateRanges As String = ""
                Dim tempDateRangesRow As DateRangesRow
                'for each unique style row
                For g As Integer = 0 To currentUserControl.DataGridView1.Rows.Count - 1
                    tempDateRangesRow = currentUserControl.DataGridView1.Rows(g)
                    allDateRanges = allDateRanges & tempDateRangesRow.DateRangesStyle.ChangeOLStylePickerdialog.getLabelResolution & "{ tempLabel" & g & " = " & tempDateRangesRow.DateRangesStyle.getLabelExpresion & ";}"
                Next
                Return allDateRanges

            Case "Simple Cluster"
                Dim currentUserControl As OL3LayerStyleCluster = StyleControl

                Return currentUserControl.OlStylePicker1.ChangeOLStylePickerdialog.getLabelResolution & "{ tempLabel" & styleNum & " = " & currentUserControl.OlStylePicker1.getLabelExpresion & ";}"


            Case "Cluster and Statistics"
                Dim currentUserControl As OL3LayerStyleClusterStats = StyleControl


                If currentUserControl.ComboBox1.Text <> "None" Then
                    'if stats then cycle through rows
                    Dim allClusterRanges As String = ""
                    Dim tempClusterRangesRow As ClusterStatsRow
                    'for each unique style row
                    For g As Integer = 0 To currentUserControl.DataGridView1.Rows.Count - 1
                        tempClusterRangesRow = currentUserControl.DataGridView1.Rows(g)
                        allClusterRanges = allClusterRanges & tempClusterRangesRow.ClusterRangesStyle.ChangeOLStylePickerdialog.getLabelResolution & "{ tempLabel" & g & " = " & tempClusterRangesRow.ClusterRangesStyle.getLabelExpresion & ";}"
                    Next
                    Return allClusterRanges
                Else

                End If
        End Select
    End Function

    Function getSingleStyle(ByVal StyleControl As UserControl, ByVal StyleType As String, Optional ByVal styleNum As Integer = 0) As String
        'a case for each feature type
        Select Case StyleType

            Case "Single Feature Style"
                Dim currentUserControl As OL3LayerStyleFeatureStyle = StyleControl
                Return "'" & styleNum & "':[" & currentUserControl.OlStylePicker1.getIndividualStyleString(styleNum) & "]"

            Case "Unique Values"
                Dim currentUserControl As OL3LayerStyleUniqueValues = StyleControl
                'cycle through all style pickers getting multistylestrings...hmm do in unique style usercontrol

                Dim allUniques As String = ""
                Dim tempUniqueRow As uniqueRow
                'for each unique style row
                For g As Integer = 0 To currentUserControl.DataGridView1.Rows.Count - 1
                    tempUniqueRow = currentUserControl.DataGridView1.Rows(g)
                    allUniques = allUniques & ",'" & g & "':[" & tempUniqueRow.uniqueStyle.getIndividualStyleString(g) & "]"
                Next
                Return allUniques.Substring(1)

            Case "Numeric Ranges"
                Dim currentUserControl As OL3LayerStyleNumericRanges = StyleControl
                'cycle through all style pickers getting multistylestrings...hmm do in unique style usercontrol

                Dim allNumericRanges As String = ""
                Dim tempNumericRangesRow As NumericRangesRow
                'for each unique style row
                For g As Integer = 0 To currentUserControl.DataGridView1.Rows.Count - 1
                    tempNumericRangesRow = currentUserControl.DataGridView1.Rows(g)
                    allNumericRanges = allNumericRanges & ",'" & g & "':[" & tempNumericRangesRow.NumericRangesStyle.getIndividualStyleString(g) & "]"
                Next
                Return allNumericRanges.Substring(1)

            Case "Date Ranges"
                Dim currentUserControl As OL3LayerStyleDateRanges = StyleControl
                'cycle through all style pickers getting multistylestrings...hmm do in unique style usercontrol

                Dim allDateRanges As String = ""
                Dim tempDateRangesRow As DateRangesRow
                'for each unique style row
                For g As Integer = 0 To currentUserControl.DataGridView1.Rows.Count - 1
                    tempDateRangesRow = currentUserControl.DataGridView1.Rows(g)
                    allDateRanges = allDateRanges & ",'" & g & "':[" & tempDateRangesRow.DateRangesStyle.getIndividualStyleString(g) & "]"
                Next
                Return allDateRanges.Substring(1)

            Case "Simple Cluster"
                Dim currentUserControl As OL3LayerStyleCluster = StyleControl
                'cycle through all style pickers getting multistylestrings...hmm do in unique style usercontrol

                Return "'" & styleNum & "':[" & currentUserControl.OlStylePicker1.getIndividualStyleString(styleNum) & "]"

            Case "Cluster and Statistics"
                Dim currentUserControl As OL3LayerStyleClusterStats = StyleControl
                'cycle through all style pickers getting multistylestrings...hmm do in unique style usercontrol


                If currentUserControl.ComboBox1.Text <> "None" Then
                    'if stats then cycle through rows
                    Dim allClusterRanges As String = ""
                    Dim tempClusterRangesRow As ClusterStatsRow
                    'for each unique style row
                    For g As Integer = 0 To currentUserControl.DataGridView1.Rows.Count - 1
                        tempClusterRangesRow = currentUserControl.DataGridView1.Rows(g)
                        allClusterRanges = allClusterRanges & ",'" & g & "':[" & tempClusterRangesRow.ClusterRangesStyle.getIndividualStyleString(g) & "]"
                    Next
                    Return allClusterRanges.Substring(1)
                Else

                End If
        End Select
    End Function

    Function getSingleCondition(ByVal StyleControl As UserControl, ByVal StyleType As String, Optional ByVal styleNum As Integer = 0) As String
        'gets the if statment
        'a case for each feature type
        Select Case StyleType

            Case "Single Feature Style"
                Dim currentUserControl As OL3LayerStyleFeatureStyle = StyleControl
                Return "if('0' == '0'){|" 'always true

            Case "Unique Values"
                Dim currentUserControl As OL3LayerStyleUniqueValues = StyleControl
                'cycle through all style pickers getting conditions...hmm do in unique style usercontrol
                '"if(feature.get('NAME') == 'Spain'){|""

                Dim allIfs As String = ""
                'for each unique style row
                For g As Integer = 0 To currentUserControl.DataGridView1.Rows.Count - 1
                    allIfs = allIfs & "if(feature.get('" & currentUserControl.ComboBox1.Text & "') == '" & currentUserControl.DataGridView1.Rows(g).Cells(1).FormattedValue & "'){|"
                Next
                Return allIfs

            Case "Numeric Ranges"
                Dim currentUserControl As OL3LayerStyleNumericRanges = StyleControl
                'cycle through all style pickers getting conditions...hmm do in unique style usercontrol
                '"if(feature.get('NAME') == 'Spain'){|""

                Dim allIfs As String = ""
                'for each unique style row
                For g As Integer = 0 To currentUserControl.DataGridView1.Rows.Count - 1
                    allIfs = allIfs & "if(feature.get('" & currentUserControl.ComboBox1.Text & "') >= " & currentUserControl.DataGridView1.Rows(g).Cells(1).FormattedValue & " && " & "feature.get('" & currentUserControl.ComboBox1.Text & "') < " & currentUserControl.DataGridView1.Rows(g).Cells(2).FormattedValue & "){|"
                Next
                Return allIfs

            Case "Simple Cluster"
                Dim currentUserControl As OL3LayerStyleCluster = StyleControl
                'cycle through all style pickers getting conditions...hmm do in unique style usercontrol
                '"if(feature.get('NAME') == 'Spain'){|""

                Return "if('0' == '0'){|" 'always true


            Case "Cluster and Statistics"
                Dim currentUserControl As OL3LayerStyleClusterStats = StyleControl
                'cycle through all style pickers getting conditions...hmm do in unique style usercontrol
                '"if(feature.get('NAME') == 'Spain'){|""

                'if stats then cycle through rows
                If currentUserControl.ComboBox1.Text <> "None" Then
                    Dim allIfs As String = ""
                    'for each unique style row
                    For g As Integer = 0 To currentUserControl.DataGridView1.Rows.Count - 1
                        allIfs = allIfs & "if(statValue >= " & currentUserControl.DataGridView1.Rows(g).Cells(1).FormattedValue & " && " & "statValue < " & currentUserControl.DataGridView1.Rows(g).Cells(2).FormattedValue & "){|"
                    Next
                    Return allIfs
                Else
                    Return "if('0' == '0'){|" 'always true
                End If

            Case "Heatmap"
                Return "if('0' == '0'){|" 'always true ... dummy 'if' will never fire as styles not used in heatmaps


        End Select
    End Function


    Function getStyleText() As String
        getStyleText = ""
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            getStyleText = getStyleText & "," & Chr(34) & DataGridView1.Rows(x).Cells(0).EditedFormattedValue & Chr(34)
        Next
        Return "[" & getStyleText.Substring(1) & "]"
    End Function

    Function getKeyText() As String
        getKeyText = ""
        Dim tempRow As New styleRow
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            tempRow = DataGridView1.Rows(x)
            Select Case tempRow.StyleType
                Case "Single Feature Style"
                    Dim currentUserControl As OL3LayerStyleFeatureStyle = tempRow.StyleControl
                    getKeyText = getKeyText & ",[" & currentUserControl.getKeyText & "]"

                Case "Unique Values"
                    Dim currentUserControl As OL3LayerStyleUniqueValues = tempRow.StyleControl
                    getKeyText = getKeyText & ",[" & currentUserControl.getKeyText & "]"

                Case "Numeric Ranges"
                    Dim currentUserControl As OL3LayerStyleNumericRanges = tempRow.StyleControl
                    getKeyText = getKeyText & ",[" & currentUserControl.getKeyText & "]"

                Case "Simple Cluster"
                    Dim currentUserControl As OL3LayerStyleCluster = tempRow.StyleControl
                    getKeyText = getKeyText & ",[" & currentUserControl.getKeyText & "]"

                Case "Cluster and Statistics"
                    Dim currentUserControl As OL3LayerStyleClusterStats = tempRow.StyleControl
                    getKeyText = getKeyText & ",[" & currentUserControl.getKeyText & "]"

                Case "Heatmap"
                    Dim currentUserControl As OL3LayerStyleHeatmap = tempRow.StyleControl
                    getKeyText = getKeyText & ",[" & currentUserControl.getKeyText & "]"

            End Select

        Next
        Return "[" & getKeyText.Substring(1) & "]"
    End Function


    Function getHeatmapParameters() As String
        getHeatmapParameters = ""
        Dim tempRow As New styleRow
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            tempRow = DataGridView1.Rows(x)
            Select Case tempRow.StyleType
              
                Case "Heatmap"
                    Dim currentUserControl As OL3LayerStyleHeatmap = tempRow.StyleControl
                    getHeatmapParameters = getHeatmapParameters & ",[" & currentUserControl.olBlur & "," & currentUserControl.olRadius & "," & Chr(34) & currentUserControl.olGradient & Chr(34) & "]"
                Case Else

                    getHeatmapParameters = getHeatmapParameters & ",[]"
            End Select

        Next
        Return "[" & getHeatmapParameters.Substring(1) & "]"
    End Function




End Class

Class styleRow
    Inherits DataGridViewRow
    Public StyleType As String
    Public StyleControl As UserControl
    'Public layerPath As String



End Class
