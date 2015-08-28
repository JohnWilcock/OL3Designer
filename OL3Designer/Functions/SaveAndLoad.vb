
Imports System.IO
Imports System.Text
Imports System.Collections
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization


Public Class SaveAndLoad


    'Functions
    Public Function Load(ByVal mstrSaveFile As String) As Object
        If My.Computer.FileSystem.FileExists(mstrSaveFile) Then
            Dim fs As Stream = New FileStream(mstrSaveFile, FileMode.Open)
            Dim bf As BinaryFormatter = New BinaryFormatter()
            Return bf.Deserialize(fs)
            fs.Close()
        End If
        Return True
    End Function

    Public Function Save(ByVal mstrSaveFile As String, ByVal mstrData As Object)
        If My.Computer.FileSystem.FileExists(mstrSaveFile) = True Then
            My.Computer.FileSystem.DeleteFile(mstrSaveFile)
        End If

        Dim F As Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim s As IO.Stream
        F = New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
        s = New IO.FileStream(mstrSaveFile, IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.None)

        F.Serialize(s, mstrData)

        s.Close()
        Return True
    End Function


End Class
