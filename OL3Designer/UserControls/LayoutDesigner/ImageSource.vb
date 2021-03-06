﻿
Imports System.IO

Public Class ImageSource

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ComboBox1.SelectedIndex = 1

    End Sub

    Function getPath(ByVal theAbsolutePath As String, ByVal outputLocation As String) As String
        If outputLocation Is Nothing Or outputLocation = "" Then 'is preview
            Return "file:///" & theAbsolutePath.Replace("\", "/")
        End If


        Select Case ComboBox1.SelectedIndex
            Case 1 'abs
                Return "file:///" & theAbsolutePath.Replace("\", "/")
            Case 2 'rel
                Return MakeRelativePath(theAbsolutePath, outputLocation)
            Case 3

        End Select



    End Function

    Function MakeRelativePath(ByVal fromPath As [String], ByVal toPath As [String]) As [String]
        If fromPath.Substring(fromPath.Length - 1) <> "/" Then fromPath = fromPath & "/"
        If toPath.Substring(toPath.Length - 1) <> "/" Then toPath = toPath & "/"

        If [String].IsNullOrEmpty(fromPath) Then
            Throw New ArgumentNullException("fromPath")
        End If
        If [String].IsNullOrEmpty(toPath) Then
            Throw New ArgumentNullException("toPath")
        End If
        Dim fromUri As New Uri(fromPath)
        Dim toUri As New Uri(toPath)
        Dim relativeUri As Uri = fromUri.MakeRelativeUri(toUri)
        Dim relativePath As [String] = Uri.UnescapeDataString(relativeUri.ToString())

        'If relativePath.Substring(0, 1) = "/" Then
        '    relativePath = relativePath.Substring(1)
        'End If

        Return relativePath.Replace("/"c, Path.DirectorySeparatorChar)
    End Function

    Function getPopupImageFetcher(ByVal fieldName As String) As String

        Select Case ComboBox1.SelectedIndex
            Case 0 'absolute
                Return "file:///" & Chr(34) & " + getCorrectPath(feature,'" & fieldName & "','abs') + " & Chr(34)

            Case 1 'reletive
                'as the output path is not known until export, then save both absolute path and output path in js, then figure out the relitave path on the fly.
                Return Chr(34) & " + getCorrectPath(feature,'" & fieldName & "','rel') + " & Chr(34)

        End Select

    End Function


    Public Function save() As OL3ImageSourceSaveObject
        save = New OL3ImageSourceSaveObject
        Select Case ComboBox1.SelectedIndex
            Case 0
                save.ImageSource = OL3ImageSourceSaveObject.ImageSourceType.Absolute
            Case 1
                save.ImageSource = OL3ImageSourceSaveObject.ImageSourceType.Relative
            Case 2
                save.ImageSource = OL3ImageSourceSaveObject.ImageSourceType.RelativeAndCopy
            Case Else
                save.ImageSource = OL3ImageSourceSaveObject.ImageSourceType.Absolute
        End Select

    End Function

    Public Sub loadObj(ByVal saveObj As OL3ImageSourceSaveObject)
        Select Case saveObj.ImageSource
            Case OL3ImageSourceSaveObject.ImageSourceType.Absolute
                ComboBox1.SelectedIndex = 0
            Case OL3ImageSourceSaveObject.ImageSourceType.Relative
                ComboBox1.SelectedIndex = 1
            Case OL3ImageSourceSaveObject.ImageSourceType.RelativeAndCopy
                ComboBox1.SelectedIndex = 2
            Case Else
                ComboBox1.SelectedIndex = 0
        End Select

    End Sub
End Class

<Serializable()> _
Public Class OL3ImageSourceSaveObject

    Public Enum ImageSourceType
        Absolute = 0
        Relative = 1
        RelativeAndCopy = 2
    End Enum

    Public ImageSource As ImageSourceType

End Class



