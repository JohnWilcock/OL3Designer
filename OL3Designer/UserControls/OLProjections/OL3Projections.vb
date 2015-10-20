Imports System.Collections.Generic
Imports System.Text


Public Class OL3Projections




    Public ChosenCoordSystem As String
    Public WithEvents ChosenCoordSystemEPSG As String
    Public WithEvents ChosenCoordSystemWKT As String

    Public WKTList As New List(Of SridReader.WKTstring)

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        'set to wgs 84 epsg 4326
        ChosenCoordSystemEPSG = "4326"
        ChosenCoordSystem = "WGS 84 - Latlon"

        TableLayoutPanel1.Visible = False
    End Sub

    Private Sub CoordinateSystemPicker_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Sub SearchCSbyID(id As Integer)
        ListBox1.Items.Clear()
        WKTList.Clear()

        For Each wkt As SridReader.WKTstring In SridReader.GetSRIDs()
            If wkt.WKID = id Then
                'We found it!
                ListBox1.Items.Add(wkt.WKID & vbTab & wkt.WKTName)
                WKTList.Add(wkt)
            End If
        Next
    End Sub


    Public Sub SearchCSbyName(name As String)
        ListBox1.Items.Clear()
        WKTList.Clear()

        For Each wkt As SridReader.WKTstring In SridReader.GetSRIDs()
            If wkt.WKTName.ToUpper.Contains(name.ToUpper) Then
                'We found it!
                ListBox1.Items.Add(wkt.WKID & vbTab & wkt.WKTName)
                WKTList.Add(wkt)
            End If
        Next
    End Sub

    Public Sub ShowAll()
        ListBox1.Items.Clear()
        WKTList.Clear()

        For Each wkt As SridReader.WKTstring In SridReader.GetSRIDs()
            ListBox1.Items.Add(wkt.WKID & vbTab & wkt.WKTName)
            WKTList.Add(wkt)
        Next
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        SearchCSbyName(TextBox4.Text)
    End Sub

    Sub changeSel()
        ChosenCoordSystem = WKTList(ListBox1.SelectedIndex).WKTName
        ChosenCoordSystemEPSG = WKTList(ListBox1.SelectedIndex).WKID
        ChosenCoordSystemWKT = WKTList(ListBox1.SelectedIndex).WKT
        'theDlg.Label3.Text = ChosenCoordSystem
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ShowAll()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SearchCSbyID(TextBox3.Text)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TableLayoutPanel1.Visible = True Then
            TableLayoutPanel1.Visible = False
        Else
            TableLayoutPanel1.Visible = True
        End If


    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        changeSel()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If ListBox1.SelectedIndex < 0 Then
            MsgBox("No CRS selected, Highlight a coordinate system in the list")
            Exit Sub
        End If

        ChosenCoordSystem = WKTList(ListBox1.SelectedIndex).WKTName
        ChosenCoordSystemEPSG = WKTList(ListBox1.SelectedIndex).WKID
        ChosenCoordSystemWKT = WKTList(ListBox1.SelectedIndex).WKT

        Dim gdal As New GDALImport
        TextBox1.Text = ChosenCoordSystemEPSG
        TextBox2.Text = gdal.getProj4JSCRS(ChosenCoordSystemWKT)
        'theDlg.Label3.Text = ChosenCoordSystem
        'Me.Dispose()
        'Me.Parent.Controls.Remove(Me)
        TableLayoutPanel1.Visible = False

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        TextBox1.Enabled = True
        TextBox2.Enabled = True
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        If TextBox4.Text <> "" Then
            SearchCSbyName(TextBox4.Text)
        Else
            ShowAll()
        End If

    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        If TextBox3.Text <> "" Then
            SearchCSbyID(TextBox3.Text)
        Else
            ShowAll()
        End If

    End Sub




    Public Function save() As OL3ProjectionSaveObject
        save = New OL3ProjectionSaveObject

        save.EPSGCode = TextBox1.Text
        save.proj4String = TextBox2.Text

    End Function

    Public Sub loadObj(ByVal saveObj As OL3ProjectionSaveObject)
        TextBox1.Text = saveObj.EPSGCode
        TextBox2.Text = saveObj.proj4String

    End Sub
End Class



<Serializable()> _
Public Class OL3ProjectionSaveObject
    Public proj4String As String
    Public EPSGCode As Integer

End Class