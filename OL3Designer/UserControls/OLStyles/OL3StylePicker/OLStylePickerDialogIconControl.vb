Imports System.IO


Public Class OLStylePickerDialogIconControl
    Public styleSettings As New StyleProperties
    Public hasLoaded As Boolean = 0
    Private Sub OLStylePickerDialogIconControl_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'load existing settings
        loadValuesToControls()
        hasLoaded = 1
    End Sub

    Sub loadValuesToControls()
        'load existing settings

        NumericUpDown1.Value = styleSettings.OLScale
        NumericUpDown3.Value = styleSettings.OLRotation
        ComboBox1.Text = styleSettings.OLAnchorOrigin

        NumericUpDown5.Value = styleSettings.OLXAnchor
        NumericUpDown6.Value = styleSettings.OLYAnchor

        ComboBox2.Text = styleSettings.OLXAnchorUnit
        ComboBox3.Text = styleSettings.OLYAnchorUnit
    End Sub

    Function hasControlLoaded() As Boolean
        If hasLoaded = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        styleSettings.OLScale = NumericUpDown1.Value
    End Sub

    Private Sub NumericUpDown3_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown3.ValueChanged

        styleSettings.OLRotation = NumericUpDown3.Value
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        styleSettings.OLAnchorOrigin = ComboBox1.Text
    End Sub

    Private Sub NumericUpDown5_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown5.ValueChanged

        styleSettings.OLXAnchor = NumericUpDown5.Value
    End Sub

    Private Sub NumericUpDown6_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown6.ValueChanged
        styleSettings.OLYAnchor = NumericUpDown6.Value
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged

        styleSettings.OLXAnchorUnit = ComboBox2.Text
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged

        styleSettings.OLYAnchorUnit = ComboBox3.Text
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'get an external image file to use as an icon
        Dim OFD As New OpenFileDialog
        Dim theAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        Dim customIconPath As String = Path.Combine(Path.GetDirectoryName(theAssembly.Location), "Custom Icons")

        If OFD.ShowDialog() = DialogResult.OK Then
            'check valid file
            If File.Exists(OFD.FileName) Then
                Dim fileEXT As String = Path.GetExtension(OFD.FileName)
                If fileEXT.ToUpper = ".JPG" Or fileEXT.ToUpper = ".PNG" Or fileEXT.ToUpper = ".GIF" Then
                    'resize to 50x50
                    Dim IM As New ImageList
                    IM.ImageSize = New Size(50, 50)
                    Dim newImage As Image = OLStylePickerDialog.resizeIcons(New Bitmap(OFD.FileName), IM)

                    'see if custom folder exisits, if not create it
                    If (Not System.IO.Directory.Exists(customIconPath)) Then
                        System.IO.Directory.CreateDirectory(customIconPath)
                    End If

                    'save to custom folder
                    newImage.Save(Path.Combine(customIconPath, Path.GetFileNameWithoutExtension(OFD.FileName) & ".png"))
                    Label9.Text = Path.GetFileNameWithoutExtension(OFD.FileName)

                End If

            End If

        End If



    End Sub

    'function to convert a character to a bitmap
    'http://www.developerfusion.com/tools/convert/csharp-to-vb/?batchId=96fc836b-209c-458c-8b13-7a415ff36781
    Public Function ConvertTextToImage(txt As String, fontname As String, fontsize As Integer, bgcolor As Color, fcolor As Color, width As Integer, _
    Height As Integer) As Bitmap
        Dim bmp As New Bitmap(width, Height)
        Using graphics__1 As Graphics = Graphics.FromImage(bmp)

            Dim font As New Font(fontname, fontsize)
            graphics__1.FillRectangle(New SolidBrush(bgcolor), 0, 0, bmp.Width, bmp.Height)
            graphics__1.DrawString(txt, font, New SolidBrush(fcolor), 0, 0)
            graphics__1.Flush()
            font.Dispose()


            graphics__1.Dispose()
        End Using
        Return bmp
    End Function

End Class
