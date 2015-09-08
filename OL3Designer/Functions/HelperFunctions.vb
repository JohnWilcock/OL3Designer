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


    Function replaceIconPathsWithOutputPaths(ByVal outputText As String, ByVal outputPath As String) As String
        'this whole function needs re-thinking for something more robust
        replaceIconPathsWithOutputPaths = outputText
        'for each icon in list, 
        'combine with exe file path and replace \ with /.
        'if match is found copy icon to output folder
        'then use for loop and for each match replace with output path + icon file name.


        'get exe path 
        Dim exePath As String = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace("\", "/")

        'cycle through icons in style settings and put in list
        Dim defaultString As String = My.Resources.DefaultStyleSettings

        'now add any custom icons to the list and pad out with dummy values
        If Directory.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Custom Icons")) Then
            Dim dir As New DirectoryInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Custom Icons"))
            Dim allFiles As FileInfo() = dir.GetFiles()
            For Each filei As FileInfo In allFiles
                'if a png file
                If filei.Extension.ToLower = ".png" Then
                    defaultString = defaultString & Path.GetFileNameWithoutExtension(filei.FullName) & ":1,0,bottom-left,0.5,fraction,0.5,fraction;"

                End If
            Next
        End If


        Dim defaultStrings() As String = defaultString.Split(";")
        Dim currentStyle() As String
        Dim iconName As String = ""
        Dim iconShortName As String = ""
        Dim listOfIconsToCopyFrom As New List(Of String)
        Dim listOfIconsToCopyTo As New List(Of String)

        For Each item As String In defaultStrings
            currentStyle = item.Split(":")
            iconName = "olstyle_icon_" & currentStyle(0).Replace(vbCrLf, "").ToString.Trim(" ")
            iconShortName = "OL3Icon_" & currentStyle(0).Replace(vbCrLf, "_").Replace(" ", "_").ToString

            'is icon referenced in output
            If outputText.IndexOf(iconShortName) <> -1 And iconShortName.Length > 4 Then
                'replaceIconPathsWithOutputPaths = outputText.Replace(exePath & "/" & iconShortName & ".png", outputPath.Replace("\", "/") & "/" & iconShortName & ".png")
                replaceIconPathsWithOutputPaths = replaceIconPathsWithOutputPaths.Replace(exePath & "/" & iconShortName & ".png", iconShortName & ".png")
                listOfIconsToCopyFrom.Add(exePath.Replace("/", "\") & "\" & iconShortName & ".png")
                listOfIconsToCopyTo.Add(outputPath & "\" & iconShortName & ".png")
            End If

        Next

        'get unique values from lists
        Dim listFrom() As String
        Dim listTo() As String
        If listOfIconsToCopyFrom.Count > 0 Then
            listFrom = listOfIconsToCopyFrom.Distinct.ToArray
            listTo = listOfIconsToCopyTo.Distinct.ToArray

            For s As Integer = 0 To listFrom.Count - 1
                If File.Exists(listFrom(s)) Then 'check source icon file exists
                    If Not File.Exists(listTo(s)) Then ' check destination file does not.
                        File.Copy(listFrom(s), listTo(s))
                    End If
                End If
            Next
        End If



        Return replaceIconPathsWithOutputPaths
    End Function



End Class
