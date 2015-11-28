Public Class LayoutElements

    Public thelayoutDesigner As LayoutDesigner
    Dim Context As New ContextMenuStrip



    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Context.Items.Clear()
        Context.Items.Add("Remove")
        AddHandler Context.Items(0).Click, AddressOf removeSelectedNode
    End Sub

    Sub rClick(sender As Object, e As MouseEventArgs) Handles TreeView1.MouseDown
        If TreeView1.SelectedNode Is Nothing Or e.Button <> Windows.Forms.MouseButtons.Right Then Exit Sub
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Context.Show(Me, e.Location)
        End If
    End Sub

    Function getAmendedElementList(ByVal theSP As SP, ByRef theLD As LayoutDesigner) As TreeNode

        Dim tempTreenode As TreeNode = getElementList(theSP, theLD)
        tempTreenode.Text = "Layout"
        Return tempTreenode
    End Function

    Function setNullSP(ByVal theSP As SP, ByVal panelNum As Integer)
        If panelNum = 1 Then
            theSP.p1Type.Hide()
            theSP.p1Text.Hide()
            theSP.p1Path.Hide()
            theSP.p1KeyButton.Hide()
            theSP.p1ImageType.Hide()
            theSP.p1ControlsButton.Hide()
            theSP.p1Collapsable.Hide()
            theSP.p1Collapsed.Hide()
        Else
            theSP.p2Type.Hide()
            theSP.p2Text.Hide()
            theSP.p2Path.Hide()
            theSP.p2KeyButton.Hide()
            theSP.p2ImageType.Hide()
            theSP.p2ControlsButton.Hide()
            theSP.p2Collapsable.Hide()
            theSP.p2collapsed.Hide()
        End If


    End Function

    Function getElementList(ByVal theSP As SP, ByRef theLD As LayoutDesigner) As TreeNode
        getElementList = New TreeNode
        thelayoutDesigner = theLD



        'cell 1
        'if contents of panel1 is another splitter, recall this sub, else create node
        theLD.removeAllButtons(theSP.Panel1)
        If theSP.Panel1.Controls.Count > 0 Then
            'create node - subnodes come from this sub


            Dim nodeNum As Integer = getElementList.Nodes.Add(getElementList(theSP.Panel1.Controls(theSP.Panel1.Controls.Count - 1), theLD))
            If theSP.Orientation = Orientation.Horizontal Then
                getElementList.Nodes(nodeNum).Text = "Top"
            Else
                getElementList.Nodes(nodeNum).Text = "Left"
            End If
            setNullSP(theSP, 1)
            getElementList.Nodes(nodeNum).Name = theSP.tableID
        Else
            'if its a key get the key num , else returns blank

            If theSP.p1Type.Text = "Text" Then
                getElementList.Nodes.Add("Text")
            End If
            If theSP.p1Type.Text = "Image" Then
                getElementList.Nodes.Add("Image")
            End If
            If theSP.p1Type.Text = "Controls" Then
                getElementList.Nodes.Add("Controls")
            End If
            If theSP.p1Type.Text.Length > 3 Then
                If theSP.p1Type.Text.Substring(0, 3) = "Map" Then
                    getElementList.Nodes.Add(theSP.p1Type.Text)
                End If
            End If
            If theSP.p1Type.Text = "Key" Then
                getElementList.Nodes.Add("Key")
            End If

            getElementList.Nodes(getElementList.Nodes.Count - 1).Name = theSP.tableID

        End If


        'cell2
        theLD.removeAllButtons(theSP.Panel2)
        If theSP.Panel2.Controls.Count > 0 Then
            'create node - subnodes come from this sub

            Dim nodeNum As Integer = getElementList.Nodes.Add(getElementList(theSP.Panel2.Controls(theSP.Panel2.Controls.Count - 1), theLD))
            If theSP.Orientation = Orientation.Horizontal Then
                getElementList.Nodes(nodeNum).Text = "Bottom"
            Else
                getElementList.Nodes(nodeNum).Text = "Right"
            End If
            setNullSP(theSP, 2)
            getElementList.Nodes(nodeNum).Name = theSP.tableID
        Else
            'if its a key get the key num , else returns blank

            If theSP.p2Type.Text = "Text" Then
                getElementList.Nodes.Add("Text")
            End If
            If theSP.p2Type.Text = "Image" Then
                getElementList.Nodes.Add("Image")
            End If
            If theSP.p2Type.Text = "Controls" Then
                getElementList.Nodes.Add("Controls")
            End If
            If theSP.p2Type.Text.Substring(0, 3) = "Map" Then
                getElementList.Nodes.Add(theSP.p2Type.Text)
            End If
            If theSP.p2Type.Text = "Key" Then
                getElementList.Nodes.Add("Key")
            End If

            getElementList.Nodes(getElementList.Nodes.Count - 1).Name = theSP.tableID
        End If


        'expand list
        getElementList.ExpandAll()

        'reset labels
        theLD.addAllLabels(theSP)
        Return getElementList



    End Function


    Sub nodeSelect(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseClick
        If (e.Node.Text = "" Or e.Node.Text = "Layout") And e.Node.Name = "" Then Exit Sub

        Dim theTableID As Integer = e.Node.Name
        Dim tempSP As SP = getSPfromTableID(theTableID, thelayoutDesigner.Sp1)

        'set selected panel
        If e.Node.Index = 0 Then
            tempSP.selectedPanel = tempSP.Panel1
        Else
            tempSP.selectedPanel = tempSP.Panel2
        End If

        'show properties
        tempSP.displayProperties()


    End Sub

    Sub removeSelectedNode()

        If (TreeView1.SelectedNode.Text = "" Or TreeView1.SelectedNode.Text = "Layout") And TreeView1.SelectedNode.Name = "" Then Exit Sub

        Dim theTableID As Integer = TreeView1.SelectedNode.Name
        Dim tempSP As SP = getSPfromTableID(theTableID, thelayoutDesigner.Sp1)

        'set selected panel
        If TreeView1.SelectedNode.Index = 0 Then
            tempSP.selectedPanel = tempSP.Panel1
        Else
            tempSP.selectedPanel = tempSP.Panel2
        End If



        'only do if a null panel
        If TreeView1.SelectedNode.Index = 0 And tempSP.p1Type.Visible = False Then
            tempSP.selectedPanel = tempSP.Panel1
            tempSP.selectedPanel.Controls(0).Dispose()
        ElseIf TreeView1.SelectedNode.Index = 1 And tempSP.p2Type.Visible = False Then
            tempSP.selectedPanel.Controls(0).Dispose()
        End If


        'un-null panel
        tempSP.setPropertyPanels()

    End Sub

    Function getSPfromTableID(ByVal theTableID As Integer, ByVal theSP As SP) As SP

        If theSP.tableID = theTableID Then
            Return theSP
        Else
            thelayoutDesigner.removeAllButtons(theSP.Panel1)
            thelayoutDesigner.removeAllButtons(theSP.Panel2)
            If theSP.Panel1.Controls.Count > 0 Then
                getSPfromTableID = getSPfromTableID(theTableID, theSP.Panel1.Controls(0))

            End If
            If theSP.Panel2.Controls.Count > 0 Then
                getSPfromTableID = getSPfromTableID(theTableID, theSP.Panel2.Controls(0))

            End If

            thelayoutDesigner.addAllLabels(theSP)
        End If

    End Function


End Class


