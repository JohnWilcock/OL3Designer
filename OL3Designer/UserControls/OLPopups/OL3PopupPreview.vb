Imports System.Runtime.InteropServices

Public Class OL3PopupPreview

    Const FEATURE_DISABLE_NAVIGATION_SOUNDS As Integer = 21
    Const SET_FEATURE_ON_PROCESS As Integer = &H2

    <DllImport("urlmon.dll")> _
     <PreserveSig> _
    Private Shared Function CoInternetSetFeatureEnabled(FeatureEntry As Integer, <MarshalAs(UnmanagedType.U4)> dwFlags As Integer, fEnable As Boolean) As <MarshalAs(UnmanagedType.[Error])> Integer
    End Function

    Private Shared Sub DisableClickSounds()
        CoInternetSetFeatureEnabled(FEATURE_DISABLE_NAVIGATION_SOUNDS, SET_FEATURE_ON_PROCESS, True)
    End Sub


    Private Sub OL3PopupPreview_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DisableClickSounds()

    End Sub

    Sub refreshControl()
        'set browser source to the string
        WebBrowser1.DocumentText = PopupHTML
        'http://stackoverflow.com/questions/174403/net-webbrowser-documenttext-isnt-changing
        WebBrowser1.Document.OpenNew(True)
        WebBrowser1.Document.Write(PopupHTML)
        WebBrowser1.DocumentText = PopupHTML
    End Sub

    Private _PopupHTML As String
    Property PopupHTML As String
        Get
            Return _PopupHTML
        End Get
        Set(ByVal value As String)
            _PopupHTML = value
            refreshControl()
        End Set
    End Property
End Class
