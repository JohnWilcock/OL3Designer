﻿Public Class LayoutElements

    Public thelayoutDesigner As LayoutDesigner

    Function getAmendedElementList(ByVal theSP As SP, ByRef theLD As LayoutDesigner) As TreeNode

        Dim tempTreenode As TreeNode = getElementList(theSP, theLD)
        tempTreenode.Text = "Layout"
        Return tempTreenode
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
            If theSP.p1Type.Text.Substring(0, 3) = "Map" Then
                getElementList.Nodes.Add(theSP.p1Type.Text)
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

        'reset labels
        theLD.addAllLabels(theSP)
        Return getElementList



    End Function


    Sub nodeSelect(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseClick
        If (e.Node.Text = "" Or e.Node.Text = "Layout") And e.Node.Name = "" Then Exit Sub

        Dim theTableID As Integer = e.Node.Name
        Dim tempSP As SP = getSPfromTableID(theTableID, thelayoutDesigner.Sp1)

        tempSP.displayProperties()
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

