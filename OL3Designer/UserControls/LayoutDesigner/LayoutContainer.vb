

Public Class SP
    Inherits SplitContainer
    Public tableID As Integer
    Dim buttonList As New List(Of Control)
    Dim aC As New Button
    Dim aR As New Button
    Dim aP As New Button
    Public aL1 As New Label
    Public aL2 As New Label


    Dim selectedPanel As SplitterPanel
    Dim rows As Boolean

    Public WithEvents p1P As New FlowLayoutPanel
    Public WithEvents p2P As New FlowLayoutPanel
    Public p1Type As New ComboBox
    Public p2Type As New ComboBox
    Public p1Collapsable As New CheckBox
    Public p2Collapsable As New CheckBox
    Public p1Collapsed As New CheckBox
    Public p2collapsed As New CheckBox
    Public p1Path As New Button
    Public p2Path As New Button
    Public p1Img As New PictureBox
    Public p2Img As New PictureBox
    Public p1Text As New HTMLEditor
    Public p2Text As New HTMLEditor
    Public p1CollapseButtonHTML As String
    Public p2CollapseButtonHTML As String

    'for style
    Public p1StyleButton As New Button
    Public p2StyleButton As New Button
    Public p1StyleOptions As New LayoutStyleOptions
    Public p2StyleOptions As New LayoutStyleOptions

    'for key
    Public p1KeyButton As New Button
    Public p2KeyButton As New Button
    Public p1KeyOptions As New LayoutKeyOptions
    Public p2KeyOptions As New LayoutKeyOptions

    'for controls
    Public p1ControlsButton As New Button
    Public p2ControlsButton As New Button
    Public p1ControlOptions As New LayoutControlOptions
    Public p2controloptions As New LayoutControlOptions

    'for image
    Public p1ImageType As New ImageSource
    Public p2ImageType As New ImageSource


    Public properties As Panel
    Public parentSP As SP



    Sub clickMouse(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.Click

    End Sub

    Sub New(Optional theSP As SP = Nothing)
        If theSP Is Nothing Then
            parentSP = Me
            Exit Sub
        Else

        End If
        parentSP = theSP
    End Sub

    Protected Overrides Sub InitLayout()
        MyBase.InitLayout()
        buttonList.Add(aC)
        buttonList.Add(aR)
        buttonList.Add(aP)

        Dim typeItems() As String = {"Map 1", "Key", "Image", "Text", "Controls"}
        p1Type.Items.AddRange(typeItems)
        p2Type.Items.AddRange(typeItems)


        AddHandler aC.Click, AddressOf addRow
        AddHandler aR.Click, AddressOf addCol
        AddHandler Me.aP.Click, AddressOf displayProperties

        Me.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
        AddHandler Me.Panel1.MouseEnter, AddressOf clicked1
        AddHandler Me.Panel2.MouseEnter, AddressOf clicked2

        AddHandler p1Type.TextChanged, AddressOf setPropertyPanels
        AddHandler p2Type.TextChanged, AddressOf setPropertyPanels

        AddHandler p1Collapsable.CheckedChanged, AddressOf setCollapseHTML
        AddHandler p2Collapsable.CheckedChanged, AddressOf setCollapseHTML

        'key handlers
        AddHandler p1KeyButton.Click, AddressOf showKeyOptions1
        AddHandler p2KeyButton.Click, AddressOf showKeyOptions2

        'Control handlers
        AddHandler p1ControlsButton.Click, AddressOf showControlsOptions1
        AddHandler p2ControlsButton.Click, AddressOf showControlsOptions1

        'style handlers
        AddHandler p1StyleButton.Click, AddressOf showStyleOptions1
        AddHandler p2StyleButton.Click, AddressOf showStyleOptions2

        'link image buttons to FOD
        AddHandler p1Path.Click, AddressOf setP1Path
        AddHandler p2Path.Click, AddressOf setP2Path

        'set control text
        p1Collapsable.Text = "Collapsble"
        p2Collapsable.Text = "Collapsble"
        p1Collapsed.Text = "Collapsed"
        p2collapsed.Text = "Collapsed"
        p1Path.Width = 30
        p2Path.Width = 30
        p1Path.Text = "..."
        p2Path.Text = "..."
        p1KeyButton.Text = "Key Items"
        p2KeyButton.Text = "Key Items"
        p1Img.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
        p2Img.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
        p1StyleButton.Text = "Style"
        p2StyleButton.Text = "Style"
        p1ControlsButton.Text = "Controls"
        p2ControlsButton.Text = "Controls"

        'set pictureboxes up
        p1Img.SizeMode = PictureBoxSizeMode.Zoom
        p2Img.SizeMode = PictureBoxSizeMode.Zoom

        'set up property panels
        Dim p1Controls() As Control = {p1Type, p1Collapsable, p1Collapsed, p1Path, p1Text, p1KeyButton, p1Img, p1ImageType, p1StyleButton, p1ControlsButton}
        p1P.Controls.AddRange(p1Controls)
        p1P.FlowDirection = FlowDirection.TopDown
        p1P.AutoScroll = True

        Dim p2Controls() As Control = {p2Type, p2Collapsable, p2collapsed, p2Path, p2Text, p2KeyButton, p2Img, p2ImageType, p2StyleButton, p2ControlsButton}
        p2P.Controls.AddRange(p2Controls)
        p2P.FlowDirection = FlowDirection.TopDown
        p2P.AutoScroll = True

        'set label positions
        aL1.Text = "Text"
        aL1.Parent = Panel1
        aL1.Location = New Point(5, 5)
        aL1.Show()

        aL2.Text = "Text"
        aL2.Parent = Panel2
        aL2.Location = New Point(5, 5)
        aL2.Show()

    End Sub

    Sub clicked1()

        Me.Panel2.BackColor = Control.DefaultBackColor
        Me.Panel1.BackColor = Color.Azure
        selectedPanel = Panel1
        showAdd(Panel1)
    End Sub

    Sub clicked2()

        Me.Panel1.BackColor = Control.DefaultBackColor
        Me.Panel2.BackColor = Color.Azure
        selectedPanel = Panel2
        showAdd(Panel2)
    End Sub



    Sub showAdd(ByVal thePanel As SplitterPanel)

        'aC.Text = "C+"
        aC.Image = My.Resources.column_16xLG
        aC.Width = 20
        aC.Height = 30
        aC.Parent = thePanel
        aC.Location = New Point((thePanel.Width / 2) - (aC.Width / 2) - 15, (thePanel.Height / 2) - (aC.Height / 2))

        aC.Show()


        ' aR.Text = "R+"
        aR.Image = My.Resources.column_16xLG___Copy
        aR.Width = 30
        aR.Height = 20
        aR.Parent = thePanel
        aR.Location = New Point((thePanel.Width / 2) - (aR.Width / 2) + 10, (thePanel.Height / 2) - (aR.Height / 2) - 20)

        aR.Show()



        'aP.Text = "P"
        aP.Image = My.Resources.PencilTool_206
        aP.Width = 30
        aP.Height = 24
        aP.Parent = thePanel
        aP.Location = New Point((thePanel.Width / 2) - (aR.Width / 2) + 10, (thePanel.Height / 2) - (aR.Height / 2))

        aP.Show()

    End Sub

    Sub addRow()
        rows = True
        splitOut()
    End Sub

    Sub addCol()
        rows = False
        splitOut()
    End Sub

    Sub splitOut()
        Dim thePanel As SplitterPanel = selectedPanel
        Dim newSP As New SP(Me)
        Dim randomNum As New Random
        newSP.tableID = randomNum.Next(1000, 9999)

        If Panel1.Equals(selectedPanel) Then
            aL1.Hide()
        Else
            aL2.Hide()
        End If

        newSP.properties = properties
        thePanel.Controls.Remove(aC)
        thePanel.Controls.Remove(aR)
        thePanel.Controls.Remove(aP)

        newSP.Parent = thePanel
        newSP.Dock = DockStyle.Fill

        'set panel types + copy previous
        If Panel1.Equals(selectedPanel) Then
            newSP.p1Type.Text = Me.p1Type.Text
            newSP.p1Img.ImageLocation = Me.p1Img.ImageLocation
            newSP.p1ImageType.ComboBox1.Text = Me.p1ImageType.ComboBox1.Text
            newSP.p1KeyOptions.keyItems = Me.p1KeyOptions.keyItems
            newSP.p1KeyOptions = p1KeyOptions

            newSP.p2Type.Text = "Text"
        Else
            newSP.p2Type.Text = Me.p2Type.Text
            newSP.p2Img.ImageLocation = Me.p2Img.ImageLocation
            newSP.p2ImageType.ComboBox1.Text = Me.p2ImageType.ComboBox1.Text
            newSP.p2KeyOptions.keyItems = Me.p2KeyOptions.keyItems
            newSP.p2KeyOptions = p2KeyOptions

            newSP.p1Type.Text = "Text"
        End If



        If rows Then
            newSP.Orientation = Windows.Forms.Orientation.Vertical
            newSP.rows = 0
            thePanel.Controls.Add(newSP)
        Else
            newSP.Orientation = Windows.Forms.Orientation.Horizontal
            newSP.rows = 1
            thePanel.Controls.Add(newSP)

        End If

    End Sub


    Sub displayProperties()
        'shows the proptery panel in designated panel
        properties.Controls.Clear()
        Dim thePanel As modifiedPanel = properties
        If selectedPanel.Equals(Panel1) Then
            p1Type.Items.Clear()
            p1Type.Items.AddRange(thePanel.layoutList.Split(","))
            properties.Controls.Add(p1P)
            p1P.Dock = DockStyle.Fill
        Else
            p2Type.Items.Clear()
            p2Type.Items.AddRange(thePanel.layoutList.Split(","))
            properties.Controls.Add(p2P)
            p2P.Dock = DockStyle.Fill
        End If
    End Sub

    Sub setPropertyPanels()
        'set which controls are visible depending on which type is selected
        aL1.Text = p1Type.Text
        aL2.Text = p2Type.Text

        'lookups for case select
        Dim p1Lookup As String = p1Type.Text
        Dim p2Lookup As String = p2Type.Text

        If p1Lookup.Length > 0 Then p1Lookup = p1Lookup.Substring(0, 3)
        If p2Lookup.Length > 0 Then p2Lookup = p2Lookup.Substring(0, 3)



        Select Case p1Lookup
            Case "Map"
                p1Collapsable.Hide()
                p1Collapsed.Hide()
                p1Path.Hide()
                p1Text.Hide()
                p1KeyButton.Hide()
                p1Img.Hide()
                p1ImageType.Hide()
                p1ControlsButton.Hide()
            Case "Key"
                p1Collapsable.Hide()
                p1Collapsed.Hide()
                p1Path.Hide()
                p1Text.Hide()
                p1KeyButton.Show()
                p1Img.Hide()
                p1ImageType.Hide()
                p1ControlsButton.Hide()

                Dim propertiesPanel As modifiedPanel = properties
                p1KeyOptions.OL3mapsObject = propertiesPanel.OL3MapsObject
            Case "Ima" ' Image
                p1Collapsable.Hide()
                p1Collapsed.Hide()
                p1Path.Show()
                p1Text.Hide()
                p1KeyButton.Hide()
                p1Img.Show()
                p1ImageType.Show()
                p1ControlsButton.Hide()
            Case "Tex" ' Text
                p1Collapsable.Hide()
                p1Collapsed.Hide()
                p1Path.Hide()
                p1Text.Show()
                p1KeyButton.Hide()
                p1Img.Hide()
                p1ImageType.Hide()
                p1ControlsButton.Hide()
            Case "Con" 'Controls
                p1Collapsable.Hide()
                p1Collapsed.Hide()
                p1Path.Hide()
                p1Text.Hide()
                p1KeyButton.Hide()
                p1Img.Hide()
                p1ImageType.Hide()
                p1ControlsButton.Show()

        End Select

        Select Case p2Lookup
            Case "Map"
                p2Collapsable.Hide()
                p2collapsed.Hide()
                p2Path.Hide()
                p2Text.Hide()
                p2KeyButton.Hide()
                p2Img.Hide()
                p2ImageType.Hide()
                p2ControlsButton.Hide()
            Case "Key"
                p2Collapsable.Hide()
                p2collapsed.Hide()
                p2Path.Hide()
                p2Text.Hide()
                p2KeyButton.Show()
                p2Img.Hide()
                p2ImageType.Hide()
                p2ControlsButton.Hide()

                Dim propertiesPanel As modifiedPanel = properties
                p2KeyOptions.OL3mapsObject = propertiesPanel.OL3MapsObject
            Case "Ima"
                p2Collapsable.Hide()
                p2collapsed.Hide()
                p2Path.Show()
                p2Text.Hide()
                p2KeyButton.Hide()
                p2Img.Show()
                p2ImageType.Show()
                p2ControlsButton.Hide()
            Case "Tex"
                p2Collapsable.Hide()
                p2collapsed.Hide()
                p2Path.Hide()
                p2Text.Show()
                p2KeyButton.Hide()
                p2Img.Hide()
                p2ImageType.Hide()
                p2ControlsButton.Hide()
            Case "Con"
                p2Collapsable.Hide()
                p2collapsed.Hide()
                p2Path.Hide()
                p2Text.Hide()
                p2KeyButton.Hide()
                p2Img.Hide()
                p2ImageType.Hide()
                p2ControlsButton.Show()

        End Select
    End Sub

    Sub setCollapseHTML()
        p1CollapseButtonHTML = ""
        p2CollapseButtonHTML = ""

        If p1Collapsable.Checked Then
            If rows Then 'table, panel , cell, 
                p1CollapseButtonHTML = "<button style='width:25px;position:absolute;right:0;bottom:0;' onclick='collapsePanel(" & Chr(34) & "table" & tableID & "_2" & Chr(34) & "," & Chr(34) & "table" & tableID & "_1" & Chr(34) & ",999,999,9)'>&#8597;</button>"
            Else
                p1CollapseButtonHTML = "<button style='width:25px;position:absolute;right:0;top:0;'  onclick='collapsePanel(" & Chr(34) & "table" & tableID & "_2" & Chr(34) & "," & Chr(34) & "table" & tableID & "_1" & Chr(34) & ",999,999,9)'>&harr;</button>"
            End If
        End If

        If p2Collapsable.Checked Then
            If rows Then
                p2CollapseButtonHTML = "<button style='width:25px;position:absolute;right:0;top:0;' onclick='collapsePanel(" & Chr(34) & "table" & tableID & "_1" & Chr(34) & "," & Chr(34) & "table" & tableID & "_2" & Chr(34) & ",999,999,9)'>&#8597;</button>"
            Else
                p2CollapseButtonHTML = "<button style='width:25px;position:absolute;left:0;top:0;'  onclick='collapsePanel(" & Chr(34) & "table" & tableID & "_1" & Chr(34) & "," & Chr(34) & "table" & tableID & "_2" & Chr(34) & ",999,999,9)'>&harr;</button>"
            End If
        End If
    End Sub


    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        Dim clientPoint As Point = PointToClient(Cursor.Position)
        If Me.DisplayRectangle.Contains(clientPoint) Then
            'destroy buttons
            For Each item In buttonList
                item.Hide()

            Next

            Return
        Else

        End If

        MyBase.OnMouseLeave(e)

    End Sub


    Sub showKeyOptions1()
        'pass maps object to key designer
        Dim propertiesPanel As modifiedPanel = properties
        p1KeyOptions.OL3mapsObject = propertiesPanel.OL3MapsObject

        p1KeyOptions.ShowDialog()
    End Sub

    Sub showKeyOptions2()
        'pass maps object to key designer
        Dim propertiesPanel As modifiedPanel = properties
        p2KeyOptions.OL3mapsObject = propertiesPanel.OL3MapsObject

        p2KeyOptions.ShowDialog()
    End Sub

    Sub showStyleOptions1()
        p1StyleOptions.ShowDialog()
    End Sub

    Sub showStyleOptions2()
        p2StyleOptions.ShowDialog()
    End Sub

    Sub showControlsOptions1()
        'pass maps object to control designer
        Dim propertiesPanel As modifiedPanel = properties
        p1ControlOptions.OL3mapsObject = propertiesPanel.OL3MapsObject

        p1ControlOptions.ShowDialog()
    End Sub

    Sub showControlsOptions2()
        'pass maps object to control designer
        Dim propertiesPanel As modifiedPanel = properties
        p1ControlOptions.OL3mapsObject = propertiesPanel.OL3MapsObject

        p1ControlOptions.ShowDialog()
    End Sub

    Sub setP1Path()
        Dim OFD As New OpenFileDialog
        If OFD.ShowDialog() = DialogResult.OK Then
            p1Img.ImageLocation = OFD.FileName
        End If

    End Sub

    Sub setP2Path()
        Dim OFD As New OpenFileDialog
        If OFD.ShowDialog() = DialogResult.OK Then
            p2Img.ImageLocation = OFD.FileName
        End If
    End Sub
End Class

