Imports OSGeo.OGR
Imports OSGeo.OSR
Imports System.IO
Imports System.Reflection
Imports System.Text.RegularExpressions

Public Class OL3Des
    Dim maps As OL3Maps
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        EnvironmentalGdal.MakeEnvironment(Application.StartupPath)

        maps = New OL3Maps(ToolStripComboBox1, Panel1, LayoutDesigner1)

        'maps.add()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'OlStylePicker2.OLStyleSettings.OLGeomType = "Point"
        If Not Directory.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\Outputs\") Then
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\Outputs\")
        End If


        ToolStripTextBox1.Text = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\Outputs\output.html"

        Dim hf As New HelperFunctions
        hf.createOL3Script()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub






    Private Sub OL3LayerList1_Load(sender As Object, e As EventArgs)

    End Sub



    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim newHTML As String = LayoutDesigner1.getHTML

        LayoutPreview1.WebBrowser1.DocumentText = newHTML

        LayoutPreview1.WebBrowser1.Document.OpenNew(True)

        LayoutPreview1.WebBrowser1.Document.Write(newHTML)

        LayoutPreview1.WebBrowser1.DocumentText = newHTML

        LayoutPreview1.WebBrowser1.Refresh()

    End Sub




    Private Sub ToolStripButton2_Click_1(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        If ToolStripTextBox1.BackColor = Color.LightCoral Then
            MsgBox("Please ensure a valid output file is chosen")
            Exit Sub
        End If

        maps.createOutputFile(ToolStripTextBox1.Text, maps.getJS(Path.GetFileNameWithoutExtension(ToolStripTextBox1.Text), Path.GetDirectoryName(ToolStripTextBox1.Text)), maps.getHTML)
    End Sub





    Sub saveOLMaps()

    End Sub

    Sub loadOLMaps()

    End Sub


    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        LayoutDesigner1.Sp1.Panel1Collapsed = False
        LayoutDesigner1.Sp1.Orientation = Orientation.Horizontal
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        LayoutDesigner1.Sp1.Panel1Collapsed = False
        LayoutDesigner1.Sp1.Orientation = Orientation.Vertical
    End Sub

    Private Sub ToolStripButton5_Click(sender As Object, e As EventArgs) Handles ToolStripButton5.Click
        LayoutDesigner1.Sp1.Panel1Collapsed = True
        LayoutDesigner1.Sp1.p1Type.Text = "Text"
    End Sub

    Private Sub ToolStripTextBox1_Change(sender As Object, e As EventArgs) Handles ToolStripTextBox1.TextChanged
        Dim hasErr As Boolean = False

        If File.Exists(ToolStripTextBox1.Text) Then
            ToolStripTextBox1.BackColor = Color.FromArgb(255, 255, 120)
            hasErr = True
        End If

        If Not Directory.Exists(Path.GetDirectoryName(ToolStripTextBox1.Text)) Then
            ToolStripTextBox1.BackColor = Color.LightCoral
            hasErr = True
        End If

        If Not IsValidPath(ToolStripTextBox1.Text) Then
            ToolStripTextBox1.BackColor = Color.LightCoral
            hasErr = True
        End If

        If hasErr <> True Then
            If Not Path.IsPathRooted(ToolStripTextBox1.Text) Then
                ToolStripTextBox1.BackColor = Color.LightCoral
                hasErr = True
            End If
        End If



        If hasErr = False Then
            ToolStripTextBox1.BackColor = Color.LightGreen

            If Not Path.HasExtension(ToolStripTextBox1.Text) Then
                ToolStripTextBox1.Text = ToolStripTextBox1.Text & ".html"
            End If
        End If



    End Sub

    Public Shared Function IsValidPath(path As String) As Boolean
        Dim r As New Regex("^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$")
        Return r.IsMatch(path)
    End Function

    Private Sub ToolStripButton6_Click(sender As Object, e As EventArgs) Handles ToolStripButton6.Click
        Dim sfa As New SaveFileDialog
        If sfa.ShowDialog = Windows.Forms.DialogResult.OK Then
            ToolStripTextBox1.Text = sfa.FileName
        End If
    End Sub

    Private Sub ToolStripButton3_Click_1(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        Dim ab As New OL3DesAbout
        ab.Show()
    End Sub


    Private Sub ToolStripButton7_Click(sender As Object, e As EventArgs) Handles ToolStripButton7.Click
        maps.add()
    End Sub

    Private Sub ToolStripButton8_Click(sender As Object, e As EventArgs) Handles ToolStripButton8.Click
        'remove map
        If maps.mapList.Count > 1 Then
            maps.mapList.RemoveAt(maps.linkedBox.SelectedIndex)
            maps.linkedBox.Items.RemoveAt(maps.linkedBox.SelectedIndex)
            maps.linkedBox.SelectedIndex = 0

            'refersh all layout designer keys to remove layers and controls from removed map
            LayoutDesigner1.refreshAllKeysAndControls(LayoutDesigner1.Sp1)

            'trigger refresh map depentant settings (sync windows and tms copy)
            For z As Integer = 0 To maps.mapList.Count - 1
                '1. map sync
                maps.mapList(z).mapOptions.checkSyncedMap()

                '2. tms copy
                maps.mapList(z).mapOptions.checkTMSMapCopy()
            Next

        Else
            MsgBox("Cannot delete, there must be at least one map")
        End If

        'refresh linked layout component list
        maps.refreshList()


    End Sub
End Class
