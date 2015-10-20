Public Class OL3General

    Public AttributionIconImageSourceList As New List(Of String)

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        DataGridView1.Columns(2).DefaultCellStyle.NullValue = Nothing

    End Sub

    Private Sub OL3General_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Function getAttributionsJS() As String
        If DataGridView1.Rows.Count > 0 Then



            getAttributionsJS = ""
            Dim tempLink As String = ""
            Dim tempImg As String = ""
            Dim imageCount As Integer = 0

            For d As Integer = 0 To DataGridView1.Rows.Count - 1
                If DataGridView1.Rows(d).Cells(2).Value Is Nothing Then
                    tempImg = ""
                Else
                    tempImg = "<img src='" & AttributionIconImageSourceList(imageCount).Replace("\", "/") & "'></img>"
                    imageCount = imageCount + 1
                End If


                tempLink = "http://" & DataGridView1.Rows(d).Cells(1).FormattedValue.ToString.Replace("http://", "").Replace("https://", "")
                getAttributionsJS = getAttributionsJS & ",new ol.Attribution({html:" & Chr(34) & tempImg & "<a href='" & tempLink & "'>" & DataGridView1.Rows(d).Cells(0).FormattedValue & "</a>" & Chr(34) & "})" & Chr(10)
            Next

            Return "[" & getAttributionsJS.Substring(1) & "]"
        Else
            Return "[]"
        End If
    End Function


    'places X and edit pencil in remove column button cell
    Private Sub removeRow_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles DataGridView1.CellPainting
        If e.ColumnIndex = 3 AndAlso e.RowIndex >= 0 Then
            e.Paint(e.CellBounds, DataGridViewPaintParts.All)

            Dim bmpFind As Bitmap = My.Resources.Offline_16xLG__2
            e.Graphics.DrawImage(bmpFind, e.CellBounds.Left + 2, e.CellBounds.Top + 2)
            e.Handled = True
        End If

    End Sub


    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        DataGridView1.Rows.Add()

    End Sub

    Private Sub DataGridView1_Click(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        'remove row
        If e.ColumnIndex = 3 AndAlso e.RowIndex >= 0 Then
            'remove icon from list if present
            If DataGridView1.Rows(e.RowIndex).Cells(2).Value <> Nothing Then
                AttributionIconImageSourceList.RemoveAt(e.RowIndex)
            End If

            DataGridView1.Rows.RemoveAt(e.RowIndex)
        End If

        'change icon
        If e.ColumnIndex = 2 AndAlso e.RowIndex >= 0 Then

            Dim OFD As New OpenFileDialog
            OFD.Multiselect = True
            OFD.Filter = "layer icon(*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png"
            OFD.Title = "Select an icon for this attribution (Optional)"

            If OFD.ShowDialog = DialogResult.OK Then
                DataGridView1.Rows(e.RowIndex).Cells(2).Value = New Bitmap(resizeIcons(New Bitmap(OFD.FileName)))
                AttributionIconImageSourceList.Add(OFD.FileName)
            End If
        End If

    End Sub


    Function resizeIcons(ByVal picImage As Bitmap) As Image

        Dim proportion As Integer = 0
        Dim startx As Decimal = 0
        Dim startY As Decimal = 0
        Dim drawwidth As Decimal = 0
        Dim drawheight As Decimal = 0
        Dim org_Image As Bitmap = picImage
        Dim final_Bitmap As Bitmap = New Bitmap(20, 20) 'attribution icon size
        Dim gr As Graphics = Graphics.FromImage(final_Bitmap)
        Dim factorscale As Decimal
        factorscale = org_Image.Height / org_Image.Width
        drawwidth = final_Bitmap.Width
        drawheight = final_Bitmap.Width * factorscale
        If drawheight > final_Bitmap.Height Then
            proportion = 1
            factorscale = org_Image.Width / org_Image.Height
            drawheight = final_Bitmap.Height
            drawwidth = final_Bitmap.Height * factorscale
        End If
        startx = 0
        startY = final_Bitmap.Height - drawheight
        gr.DrawImage(org_Image, startx, startY, drawwidth, drawheight)
        Return final_Bitmap
    End Function
End Class
