Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports mshtml

Public Class HTMLEditor
    'http://www.dreamincode.net/forums/topic/48398-creating-a-wysiwyg-html-editor-in-c%23/
    Private doc As IHTMLDocument2

    Public Sub New()
        InitializeComponent()
    End Sub


    Private Sub HTMLEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        HTMLedit.DocumentText = "<html><body></body></html>"

        doc = TryCast(HTMLedit.Document.DomDocument, IHTMLDocument2)
        doc.designMode = "On"

    End Sub

    Private Sub ToolStripButton1_Click_1(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        HTMLedit.Document.ExecCommand("Bold", False, Nothing)
    End Sub

    Private Sub ToolStripButton2_Click_1(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        HTMLedit.Document.ExecCommand("Italic", False, Nothing)
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        HTMLedit.Document.ExecCommand("Underline", False, Nothing)

    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        HTMLedit.Document.ExecCommand("JustifyLeft", False, Nothing)
    End Sub

    Private Sub ToolStripButton5_Click(sender As Object, e As EventArgs) Handles ToolStripButton5.Click
        HTMLedit.Document.ExecCommand("JustifyCenter", False, Nothing)
    End Sub

    Private Sub ToolStripButton6_Click(sender As Object, e As EventArgs) Handles ToolStripButton6.Click
        HTMLedit.Document.ExecCommand("JustifyRight", False, Nothing)

    End Sub

    Private Sub ToolStripButton7_Click(sender As Object, e As EventArgs) Handles ToolStripButton7.Click
        HTMLedit.Document.ExecCommand("InsertOrderedList", False, Nothing)
    End Sub

    Private Sub ToolStripButton8_Click(sender As Object, e As EventArgs) Handles ToolStripButton8.Click
        HTMLedit.Document.ExecCommand("CreateLink", True, Nothing)

    End Sub

    Private Sub ToolStripButton9_Click(sender As Object, e As EventArgs) Handles ToolStripButton9.Click
        fontPick()
    End Sub

    Sub fontPick()


        Dim FP As New FontDialog
        FP.ScriptsOnly = True
        FP.ShowColor = True
        FP.ShowHelp = False

        If FP.ShowDialog = DialogResult.OK Then
            'apply chosen settings
            HTMLedit.Document.ExecCommand("fontName", True, FP.Font.Name)
            HTMLedit.Document.ExecCommand("fontSize", True, FP.Font.Size)
            HTMLedit.Document.ExecCommand("foreColor", True, ColorTranslator.ToHtml(FP.Color))

            If FP.Font.Strikeout Then
                HTMLedit.Document.ExecCommand("strikeThrough", True, Nothing)
            End If

            If FP.Font.Underline Then
                HTMLedit.Document.ExecCommand("Underline", False, Nothing)
            End If

        End If

    End Sub
End Class
