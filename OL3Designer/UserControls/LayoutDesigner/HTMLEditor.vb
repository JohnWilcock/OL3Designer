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
End Class
