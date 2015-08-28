Imports System.IO
Imports System.Reflection

Public Class HelperFunctions


    Function createOL3Script(Optional ByVal outputPath As String = "-1") As String

        'find location of ol.js script, if not present get it from resourses
        Dim olFilePath As String
        If outputPath = "-1" Then
            olFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\"
        Else
            olFilePath = outputPath & "\"
        End If

        'ol.js
        If File.Exists(Path.Combine(olFilePath, "ol.js")) = False Then
            IO.File.WriteAllText(Path.Combine(olFilePath, "ol.js"), My.Resources.ol1)
        End If

        'ol.css
        If File.Exists(Path.Combine(olFilePath, "ol.css")) = False Then
            IO.File.WriteAllText(Path.Combine(olFilePath, "ol.css"), My.Resources.ol)
        End If

        Return "<script src='ol.js' type='text/javascript'></script> <link rel='stylesheet' href='ol.css' type='text/css'>"

    End Function



    Function createProj4Script(Optional ByVal outputPath As String = "-1") As String

        'find location of proj4.js script, if not present get it from resourses
        Dim olFilePath As String
        If outputPath = "-1" Then
            olFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\"
        Else
            olFilePath = outputPath & "\"
        End If

        'proj.js
        If File.Exists(Path.Combine(olFilePath, "proj4.js")) = False Then
            IO.File.WriteAllText(Path.Combine(olFilePath, "proj4.js"), My.Resources.proj4)
        End If

        Return "<script src='proj4.js' type='text/javascript'></script> "
    End Function

    Function createBootstrapScript(Optional ByVal outputPath As String = "-1") As String

        'find location of BS script, if not present get it from resourses
        Dim olFilePath As String
        If outputPath = "-1" Then
            olFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\"
        Else
            olFilePath = outputPath & "\"
        End If

        'bootstrap.js (for popups etc...)
        If File.Exists(Path.Combine(olFilePath, "bootstrap.js")) = False Then
            IO.File.WriteAllText(Path.Combine(olFilePath, "bootstrap.js"), My.Resources.bootstrap1)
        End If

        'bootstrap.css
        If File.Exists(Path.Combine(olFilePath, "bootstrap.css")) = False Then
            IO.File.WriteAllText(Path.Combine(olFilePath, "bootstrap.css"), My.Resources.bootstrap)
        End If

        Return "<script src='bootstrap.js' type='text/javascript'></script> <link rel='stylesheet' href='bootstrap.css' type='text/css'>"
    End Function

    Function createBaseScript(Optional ByVal outputPath As String = "-1") As String

        'find location of base.js script, if not present get it from resourses
        Dim olFilePath As String
        If outputPath = "-1" Then
            olFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\"
        Else
            olFilePath = outputPath & "\"
        End If

        'base.js
        If File.Exists(Path.Combine(olFilePath, "base.js")) = False Then
            IO.File.WriteAllText(Path.Combine(olFilePath, "base.js"), My.Resources.base)
        End If

        Return "<script src='base.js' type='text/javascript'></script>"
    End Function


    Function createjQueryScript(Optional ByVal outputPath As String = "-1") As String

        'find location of jQuery.js script, if not present get it from resourses
        Dim olFilePath As String
        If outputPath = "-1" Then
            olFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\"
        Else
            olFilePath = outputPath & "\"
        End If

        'jQueryjs
        If File.Exists(Path.Combine(olFilePath, "jQuery.js")) = False Then
            IO.File.WriteAllText(Path.Combine(olFilePath, "jQuery.js"), My.Resources.jQuery)
        End If

        Return "<script src='jQuery.js' type='text/javascript'></script>"
    End Function

    Function createDepsScript(Optional ByVal outputPath As String = "-1") As String

        'find location of ol-deps.js script, if not present get it from resourses
        Dim olFilePath As String
        If outputPath = "-1" Then
            olFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\"
        Else
            olFilePath = outputPath & "\"
        End If

        'ol-deps
        If File.Exists(Path.Combine(olFilePath, "ol-deps.js")) = False Then
            IO.File.WriteAllText(Path.Combine(olFilePath, "ol-deps.js"), My.Resources.ol_deps)
        End If

        Return "<script src='ol-deps.js' type='text/javascript'></script>"
    End Function

    Function createURIScript(Optional ByVal outputPath As String = "-1") As String

        'find location of ol-deps.js script, if not present get it from resourses
        Dim olFilePath As String
        If outputPath = "-1" Then
            olFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\"
        Else
            olFilePath = outputPath & "\"
        End If

        'ol-deps
        If File.Exists(Path.Combine(olFilePath, "URI.js")) = False Then
            IO.File.WriteAllText(Path.Combine(olFilePath, "URI.js"), My.Resources.URI)
        End If

        Return "<script src='URI.js' type='text/javascript'></script>"
    End Function

    Function writeAllLibraries(ByVal outputPath As String) As String
        writeAllLibraries = ""
        writeAllLibraries = writeAllLibraries & Chr(10) & createOL3Script(Path.GetDirectoryName(outputPath))
        writeAllLibraries = writeAllLibraries & Chr(10) & createProj4Script(Path.GetDirectoryName(outputPath))
        writeAllLibraries = writeAllLibraries & Chr(10) & createjQueryScript(Path.GetDirectoryName(outputPath))
        writeAllLibraries = writeAllLibraries & Chr(10) & createBootstrapScript(Path.GetDirectoryName(outputPath))
        writeAllLibraries = writeAllLibraries & Chr(10) & createBaseScript(Path.GetDirectoryName(outputPath))
        writeAllLibraries = writeAllLibraries & Chr(10) & createDepsScript(Path.GetDirectoryName(outputPath))
        writeAllLibraries = writeAllLibraries & Chr(10) & createURIScript(Path.GetDirectoryName(outputPath))


    End Function




End Class
