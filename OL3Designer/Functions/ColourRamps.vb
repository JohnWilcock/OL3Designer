Imports System.Drawing

Public Class ColourRamps
    Public RampList As New ImageList
    Public fullRampList As New List(Of fullRamp)
    Public rampPicker As ComboBox
    Public Sub init(ByRef theComboBox As ComboBox)
        RampList.ImageSize = New Size(100, 10)
        RampList.ColorDepth = ColorDepth.Depth24Bit
        rampPicker = theComboBox

        'get ramp settings file
        Dim rampFile As String = My.Resources.colourRamps
        Dim allRamps() As String = rampFile.Split(vbLf)
        Dim currentRamp() As String
        Dim currentRampClass As singleRamp
        Dim currentFullRampClass As fullRamp
        Dim currentColours() As String

        'add random ramp first
        Dim tempFullRamp As New fullRamp
        RampList.Images.Add(tempFullRamp.createRandomBitmap)

        'cycle through ramps (1 per line) adding each to image list
        For Each ramp As String In allRamps
            currentRamp = ramp.Split("|")
            currentFullRampClass = New fullRamp
            For i = 0 To currentRamp.Count - 2
                currentRampClass = New singleRamp
                currentColours = currentRamp(i).Split(",")
                currentRampClass.startColour = Color.FromArgb(currentColours(0), currentColours(1), currentColours(2))
                currentColours = currentRamp(i + 1).Split(",")
                currentRampClass.endColour = Color.FromArgb(currentColours(0), currentColours(1), currentColours(2))
                currentFullRampClass.rampList.Add(currentRampClass)
                currentFullRampClass.createBitmap()
            Next
            fullRampList.Add(currentFullRampClass)
            RampList.Images.Add(currentFullRampClass.rampBitmap)

        Next

        'add images to combobox
        For i As Int32 = 0 To RampList.Images.Count - 1
            theComboBox.Items.Add("Item " & i.ToString)
        Next

        theComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        theComboBox.DrawMode = DrawMode.OwnerDrawVariable
        theComboBox.ItemHeight = RampList.ImageSize.Height + 4
        theComboBox.Width = RampList.ImageSize.Width + 18
        theComboBox.MaxDropDownItems = RampList.Images.Count
        theComboBox.DropDownHeight = 200


        AddHandler theComboBox.DrawItem, AddressOf CB_DrawItem



    End Sub


    Sub CB_DrawItem(ByVal sender As System.Object, ByVal e As DrawItemEventArgs)

        If e.Index <> -1 Then
            e.Graphics.DrawImage(RampList.Images(e.Index), e.Bounds.Left, e.Bounds.Top)
        End If

    End Sub

End Class

Public Class singleRamp
    Private _startColour As Color
    Public Property startColour() As Color
        Get
            Return _startColour
        End Get
        Set(ByVal value As Color)
            _startColour = value
        End Set
    End Property

    Private _endColour As Color
    Public Property endColour() As Color
        Get
            Return _endColour
        End Get
        Set(ByVal value As Color)
            _endColour = value
        End Set
    End Property
End Class

<Serializable()> _
Public Class fullRamp
    Public rampList As New List(Of singleRamp)

    Private _rampbitmap As Bitmap
    Public Property rampBitmap() As Bitmap
        Get
            Return _rampbitmap
        End Get
        Set(ByVal value As Bitmap)
            _rampbitmap = value
        End Set
    End Property

    Sub createBitmap()
        Dim theRampBitmap As New Bitmap(100, 10)
        Dim theCol As Color
        For i = 0 To 99
            theCol = getColourByPercent(i + 1)
            For y As Integer = 0 To 9
                theRampBitmap.SetPixel(i, y, theCol)
            Next


        Next

        rampBitmap = theRampBitmap
    End Sub

    Function createRandomBitmap() As Bitmap
        Dim rand As New Random

        Dim theRampBitmap As New Bitmap(100, 10)
        Dim theCol As Color
        For i = 0 To 99
            theCol = Color.FromArgb(255, rand.Next(1, 254), rand.Next(1, 254), rand.Next(1, 254))
            For y As Integer = 0 To 9
                theRampBitmap.SetPixel(i, y, theCol)
            Next

        Next

        Return theRampBitmap
    End Function

    Function getColourByPercent(ByVal thePercent As Integer) As Color

        Dim ColourRampDescNum As Double = (100 / rampList.Count)
        Dim theRamp As singleRamp = rampList(Math.Floor((thePercent - 0.1) / ColourRampDescNum))
        Dim finalPercent As Integer = Math.Floor((thePercent Mod ColourRampDescNum)) * rampList.Count
        ' If finalPercent = 0 Then Return theRamp.endColour

        'Form1.TextBox1.Text = Form1.TextBox1.Text & vbCrLf & finalPercent




        Return BlendColor(theRamp.endColour, theRamp.startColour, (finalPercent / 100) * 255)
    End Function

    Public Shared Function BlendColor(color1 As Color, color2 As Color, ratio As Byte) As Color
        Dim color2Ratio = 255 - ratio
        Dim newR = CByte((CInt(color1.R) * ratio + CInt(color2.R) * color2Ratio) / 255)
        Dim newG = CByte((CInt(color1.G) * ratio + CInt(color2.G) * color2Ratio) / 255)
        Dim newB = CByte((CInt(color1.B) * ratio + CInt(color2.B) * color2Ratio) / 255)

        Return Color.FromArgb(newR, newG, newB)
    End Function

End Class
