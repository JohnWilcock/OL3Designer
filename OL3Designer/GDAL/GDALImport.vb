
Imports OSGeo.OGR
Imports OSGeo.OSR
Imports OSGeo.GDAL


Public Class GDALImport

    Dim OutputString As String = ""

    Public Sub usage()

    End Sub

    Public Function getGeoJson(ByRef args As String()) As String

        If args.Length <> 1 Then
            usage()
        End If

        ' Using early initialization of System.Console
        'OutputString = OutputString  & vbCrLf &  ("")

        ' -------------------------------------------------------------------- 

        '      Register format(s).                                             

        ' -------------------------------------------------------------------- 

        Ogr.RegisterAll()

        ' -------------------------------------------------------------------- 

        '      Open data source.                                               

        ' -------------------------------------------------------------------- 

        Dim ds As DataSource = Ogr.Open(args(0), 0)

        If ds Is Nothing Then
            OutputString = OutputString & vbCrLf & ("Can't open " + args(0))
            System.Environment.[Exit](-1)
        End If

        ' -------------------------------------------------------------------- 

        '      Get driver                                                      

        ' -------------------------------------------------------------------- 

        Dim drv As OSGeo.OGR.Driver = ds.GetDriver()

        If drv Is Nothing Then
            OutputString = OutputString & vbCrLf & ("Can't get driver.")
            System.Environment.[Exit](-1)
        End If
        ' TODO: drv.name is still unsafe with lazy initialization (Bug 1339)
        OutputString = OutputString & vbCrLf & ("Using driver " + drv.name)

        ' -------------------------------------------------------------------- 

        '      Iterating through the layers                                    

        ' -------------------------------------------------------------------- 


        For iLayer As Integer = 0 To ds.GetLayerCount() - 1
            Dim layer As Layer = ds.GetLayerByIndex(iLayer)
            'layer.GetLayerDefn.GetGeomType
            If layer Is Nothing Then
                OutputString = OutputString & vbCrLf & ("FAILURE: Couldn't fetch advertised layer " + iLayer)
                System.Environment.[Exit](-1)
            End If
            getGeoJson = ReportLayer(layer)
        Next

        'getGeoJson = OutputString

    End Function

    Public Function ReportLayer(layer As Layer) As String
        Dim def As FeatureDefn = layer.GetLayerDefn()
        OutputString = OutputString & vbCrLf & ("Layer name: " & def.GetName())
        OutputString = OutputString & vbCrLf & ("Feature Count: " & layer.GetFeatureCount(1))
        Dim ext As New Envelope()
        layer.GetExtent(ext, 1)
        OutputString = OutputString & vbCrLf & ("Extent: " & ext.MinX & "," & ext.MaxX & "," & ext.MinY & "," & ext.MaxY)

        ' -------------------------------------------------------------------- 

        '      Reading the spatial reference                                   

        ' -------------------------------------------------------------------- 

        Dim sr As OSGeo.OSR.SpatialReference = layer.GetSpatialRef()
        Dim srs_wkt As String
        If sr IsNot Nothing Then
            sr.ExportToPrettyWkt(srs_wkt, 1)
        Else
            srs_wkt = "(unknown)"
        End If


        OutputString = OutputString & vbCrLf & (Convert.ToString("Layer SRS WKT: ") & srs_wkt)

        ' -------------------------------------------------------------------- 

        '      Reading the fields                                              

        ' -------------------------------------------------------------------- 

        OutputString = OutputString & vbCrLf & ("Field definition:")
        For iAttr As Integer = 0 To def.GetFieldCount() - 1
            Dim fdef As FieldDefn = def.GetFieldDefn(iAttr)

            OutputString = OutputString & vbCrLf & (fdef.GetNameRef() & ": " & fdef.GetFieldTypeName(fdef.GetFieldType()) & " (" & fdef.GetWidth() & "." & fdef.GetPrecision() & ")")
        Next

        ' -------------------------------------------------------------------- 

        '      Reading the shapes                                              

        ' -------------------------------------------------------------------- 

        OutputString = OutputString & vbCrLf & ("")
        Dim feat As Feature
        While (InlineAssignHelper(feat, layer.GetNextFeature())) IsNot Nothing
            ReportLayer = ReportLayer & ReportFeature(feat, def)
            feat.Dispose()
        End While

        ReportLayer = "{" & Chr(34) & "type" & Chr(34) & ":" & Chr(34) & "FeatureCollection" & Chr(34) & "," & Chr(34) & "features" & Chr(34) & ":[" & ReportLayer.Substring(0, ReportLayer.Length - 1) & "]}"

    End Function

    Public Function getExtents(ByVal layerPath As String) As List(Of Double)
        Dim theLayer As Layer = loadLayer(layerPath)
        getExtents = New List(Of Double)

        Dim ext As New Envelope()
        theLayer.GetExtent(ext, 1)

        getExtents.Add(ext.MinX)
        getExtents.Add(ext.MaxX)
        getExtents.Add(ext.MinY)
        getExtents.Add(ext.MaxY)

    End Function

    Public Function ReportFeature(feat As Feature, def As FeatureDefn) As String

        Dim attributeString As String = ""
        Dim properties As String = "," & Chr(34) & "properties" & Chr(34) & ": {"


        OutputString = OutputString & vbCrLf & ("Feature(" & def.GetName() & "): " & feat.GetFID())
        For iField As Integer = 0 To feat.GetFieldCount() - 1
            Dim fdef As FieldDefn = def.GetFieldDefn(iField)

            OutputString = OutputString & vbCrLf & (fdef.GetNameRef() & " (" & fdef.GetFieldTypeName(fdef.GetFieldType()) & ") = ")
            properties = properties & Chr(34) & fdef.GetNameRef() & Chr(34) & ":"

            If feat.IsFieldSet(iField) Then

                If fdef.GetFieldType() = FieldType.OFTStringList Then

                    Dim sList As String() = feat.GetFieldAsStringList(iField)
                    For Each s As String In sList
                        s = s.Replace(Chr(34), "'")
                        OutputString = OutputString & vbCrLf & ((Convert.ToString("""") & s) & """ ")
                        properties = properties & Chr(34) & Replace(((Convert.ToString("""") & s) & """ "), "\", "/") & Chr(34)
                    Next


                ElseIf fdef.GetFieldType() = FieldType.OFTIntegerList Then
                    Dim count As Integer
                    Dim iList As Integer() = feat.GetFieldAsIntegerList(iField, count)
                    For i As Integer = 0 To count - 1
                        OutputString = OutputString & vbCrLf & (iList(i) & " ")
                        properties = properties & (iList(i) & " ")
                    Next


                ElseIf fdef.GetFieldType() = FieldType.OFTRealList Then
                    Dim count As Integer
                    Dim iList As Double() = feat.GetFieldAsDoubleList(iField, count)
                    For i As Integer = 0 To count - 1
                        OutputString = OutputString & vbCrLf & (iList(i).ToString() & " ")
                        properties = properties & (iList(i).ToString() & " ")
                    Next

                Else
                    OutputString = OutputString & vbCrLf & (feat.GetFieldAsString(iField))
                    properties = properties & Chr(34) & Replace(feat.GetFieldAsString(iField), "\", "/").Replace(Chr(34), "'") & Chr(34)
                End If

            Else
                OutputString = OutputString & vbCrLf & ("(null)")
                properties = properties & Chr(34) & ("(null)") & Chr(34)
            End If
            properties = properties & ","

        Next
        properties = properties.Substring(0, properties.Length - 1) & "}"


        If feat.GetStyleString() IsNot Nothing Then
            OutputString = OutputString & vbCrLf & ("  Style = " & feat.GetStyleString())
        End If

        Dim geom As Geometry = feat.GetGeometryRef()
        ReportFeature = "{" & Chr(34) & "type" & Chr(34) & ":" & Chr(34) & "Feature" & Chr(34) & "," & Chr(34) & "geometry" & Chr(34) & ":" & geom.ExportToJson({""}) & properties & "},"

        If geom IsNot Nothing Then
            OutputString = OutputString & vbCrLf & ("  " & geom.GetGeometryName() & "(" & geom.GetGeometryType() & ")")
            Dim sub_geom As Geometry
            For i As Integer = 0 To geom.GetGeometryCount() - 1
                sub_geom = geom.GetGeometryRef(i)
                If sub_geom IsNot Nothing Then
                    OutputString = OutputString & vbCrLf & ("  subgeom" & i & ": " & sub_geom.GetGeometryName() & "(" & sub_geom.GetGeometryType() & ")")
                End If
            Next
            Dim env As New Envelope()
            geom.GetEnvelope(env)
            OutputString = OutputString & vbCrLf & ("   ENVELOPE: " & env.MinX & "," & env.MaxX & "," & env.MinY & "," & env.MaxY)

            Dim geom_wkt As String
            geom.ExportToWkt(geom_wkt)
            OutputString = OutputString & vbCrLf & (Convert.ToString("  ") & geom_wkt)
        End If

        OutputString = OutputString & vbCrLf & ("")
    End Function
    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function




    Function loadLayer(ByVal args As String) As Layer

        Ogr.RegisterAll()

        ' -------------------------------------------------------------------- 

        '      Open data source.                                               

        ' -------------------------------------------------------------------- 

        Dim ds As DataSource = Ogr.Open(args, 0)

        If ds Is Nothing Then
            OutputString = OutputString & vbCrLf & ("Can't open " + args(0))
            System.Environment.[Exit](-1)
        End If

        ' -------------------------------------------------------------------- 

        '      Get driver                                                      

        ' -------------------------------------------------------------------- 

        Dim drv As OSGeo.OGR.Driver = ds.GetDriver()

        If drv Is Nothing Then
            OutputString = OutputString & vbCrLf & ("Can't get driver.")
            System.Environment.[Exit](-1)
        End If
        ' TODO: drv.name is still unsafe with lazy initialization (Bug 1339)
        OutputString = OutputString & vbCrLf & ("Using driver " + drv.name)

        ' -------------------------------------------------------------------- 

        '      Iterating through the layers                                    

        ' -------------------------------------------------------------------- 

        Dim theLayer As Layer
        For iLayer As Integer = 0 To ds.GetLayerCount() - 1
            theLayer = ds.GetLayerByIndex(iLayer)

            If theLayer Is Nothing Then
                OutputString = OutputString & vbCrLf & ("FAILURE: Couldn't fetch advertised layer " + iLayer)
                System.Environment.[Exit](-1)
            End If
            Return theLayer
        Next



    End Function


    Function getLayerType(ByVal filepath As String, ByVal layerNumber As Integer) As String
        Ogr.RegisterAll()

        Dim ds As DataSource = Ogr.Open(filepath, 0)

        If ds Is Nothing Then
            System.Environment.[Exit](-1)
        End If

        '      Get driver                                                      
        Dim drv As OSGeo.OGR.Driver = ds.GetDriver()

        If drv Is Nothing Then
            System.Environment.[Exit](-1)
        End If

        Dim layer As Layer = ds.GetLayerByIndex(layerNumber)

        If layer Is Nothing Then      
            System.Environment.[Exit](-1)
        End If

        Select Case layer.GetNextFeature.GetGeometryRef.GetGeometryType
            Case 6, 3 'poly
                Return "Polygon"
            Case 2, 101, 5 ' line
                Return "Line"
            Case 4, 1 'point
                Return "Point"
            Case 100 ' none
                Return "none"
            Case -2147483642 ' multipoly?
                Return "Polygon"
            Case 0 ' unknown
                Return "unknown"
            Case Else
                Return "unknown"
        End Select

        ' layer.GetLayerDefn.GetGeomType
    End Function

    Function getFieldList(ByVal filepath As String) As List(Of String)
        getFieldList = New List(Of String)
        Dim theLayer As Layer = loadLayer(filepath)
        Dim def As FeatureDefn = theLayer.GetLayerDefn()

        For iAttr As Integer = 0 To def.GetFieldCount() - 1
            Dim fdef As FieldDefn = def.GetFieldDefn(iAttr)

            getFieldList.Add(fdef.GetNameRef())
        Next

    End Function

    Function getFieldValues(ByVal filepath As String, ByVal fieldname As String, Optional justUniqueValues As Boolean = False) As List(Of String)
        getFieldValues = New List(Of String)
        Dim layer As Layer = loadLayer(filepath)
        Dim def As FeatureDefn = layer.GetLayerDefn()

        Dim yr As Integer = 0
        Dim mon As Integer = 0
        Dim day As Integer = 0
        Dim hr As Integer
        Dim min As Integer
        Dim sec As Integer
        Dim flag As Integer


        Dim feat As Feature
        While (InlineAssignHelper(feat, layer.GetNextFeature())) IsNot Nothing
            For iField As Integer = 0 To feat.GetFieldCount() - 1
                
                Dim fdef As FieldDefn = def.GetFieldDefn(iField)
                If fdef.GetNameRef = fieldname Then

                    If feat.IsFieldSet(iField) Then
                        If fdef.GetFieldType = FieldType.OFTDate Or fdef.GetFieldType = FieldType.OFTDateTime Then
                            feat.GetFieldAsDateTime(iField, yr, mon, day, hr, min, sec, flag)

                            If yr < 1 Or mon < 1 Or day < 1 Then
                                yr = 1900
                                mon = 1
                                day = 1
                                hr = 12
                            End If



                            getFieldValues.Add(New Date(yr, mon, day, hr, min, sec).Ticks)

                        Else
                            getFieldValues.Add(feat.GetFieldAsString(iField))
                        End If

                    Else
                        getFieldValues.Add("(null)")
                    End If
                End If
            Next
            feat.Dispose()
        End While

        If justUniqueValues Then
            getFieldValues = getFieldValues.ToArray().Distinct().ToArray().ToList()
        End If
    End Function

    Function getFieldValuesDbl(ByVal filepath As String, ByVal fieldname As String) As List(Of Double)
        getFieldValuesDbl = New List(Of Double)
        Dim layer As Layer = loadLayer(filepath)
        Dim def As FeatureDefn = layer.GetLayerDefn()

        Dim feat As Feature
        While (InlineAssignHelper(feat, layer.GetNextFeature())) IsNot Nothing
            For iField As Integer = 0 To feat.GetFieldCount() - 1

                Dim fdef As FieldDefn = def.GetFieldDefn(iField)
                If fdef.GetNameRef = fieldname Then

                    If feat.IsFieldSet(iField) Then
                        getFieldValuesDbl.Add(feat.GetFieldAsString(iField))
                    Else
                        getFieldValuesDbl.Add("(null)")
                    End If
                End If
            Next
            feat.Dispose()
        End While

    End Function

    Function getFieldValue(ByVal filepath As String, ByVal featureNumber As Long) As List(Of String)
        getFieldValue = New List(Of String)
        Dim layer As Layer = loadLayer(filepath)
        Dim def As FeatureDefn = layer.GetLayerDefn()

        Dim feat As Feature
        If (InlineAssignHelper(feat, layer.GetFeature(featureNumber))) IsNot Nothing Then
            For iField As Integer = 0 To feat.GetFieldCount() - 1

                Dim fdef As FieldDefn = def.GetFieldDefn(iField)
                If feat.IsFieldSet(iField) Then
                    getFieldValue.Add(feat.GetFieldAsString(iField))
                Else
                    getFieldValue.Add("(null)")
                End If

            Next
            feat.Dispose()
        End If

    End Function

    Function getFieldRanges(ByVal filepath As String, ByVal fieldname As String) As Double() 'return min/max
        Dim fieldValueList As List(Of String) = getFieldValues(filepath, fieldname)
        Dim fieldMin As String = 0
        Dim fieldMax As String = 0
        Dim result(1) As Double

        For Each value As String In fieldValueList
            fieldMin = Math.Min(CDbl(fieldMin), CDbl(value))
            fieldMax = Math.Max(CDbl(fieldMin), CDbl(value))
        Next

        result(0) = CDbl(fieldMin)
        result(1) = CDbl(fieldMax)

        getFieldRanges = result

    End Function

    Function getDateFieldRanges(ByVal filepath As String, ByVal fieldname As String) As Long() 'return min/max
        Dim fieldValueList As List(Of String) = getFieldValues(filepath, fieldname)
        Dim fieldMin As Long = 0
        Dim fieldMax As Long = 0
        Dim result(1) As Long

        For Each value As String In fieldValueList
            fieldMin = Math.Min(CLng(fieldMin), CLng(value))
            fieldMax = Math.Max(CLng(fieldMin), CLng(value))
        Next

        result(0) = CDbl(fieldMin)
        result(1) = CDbl(fieldMax)

        getDateFieldRanges = result

    End Function

    Function getFieldType(ByVal filepath As String, ByVal fieldname As String) As String
        Dim layer As Layer = loadLayer(filepath)
        Dim def As FeatureDefn = layer.GetLayerDefn()

        For iAttr As Integer = 0 To def.GetFieldCount() - 1
            Dim fdef As FieldDefn = def.GetFieldDefn(iAttr)
            'If fdef.GetFieldTypeName(fdef.GetFieldType()) = fieldname Then
            If fdef.GetNameRef() = fieldname Then
                Return fdef.GetFieldTypeName(fdef.GetFieldType())
            End If
        Next
        Return "String"
    End Function

    Function getFeatureCount(ByVal filepath As String) As Long

        Dim layer As Layer = loadLayer(filepath)
        Return layer.GetFeatureCount(0)

    End Function

    Function getCRS(ByVal filepath As String) As List(Of String)

        Dim thelayer As Layer = loadLayer(filepath)
        getCRS = New List(Of String)

        Dim sr As SpatialReference = theLayer.GetSpatialRef()
        Dim srs_projJS As String = ""
        Dim srs_wkt As String = ""

        If Not sr.Equals(Nothing) Then

            sr.ExportToProj4(srs_projJS)
            sr.ExportToWkt(srs_wkt)
        Else
            srs_wkt = "(unknown)"

        End If

        getCRS.Add(sr.AutoIdentifyEPSG)
        getCRS.Add(srs_projJS)
        getCRS.Add(srs_wkt)

        Return getCRS
    End Function


    Function getProj4JSCRS(ByVal WKT As String) As String

        Dim sr As New SpatialReference(WKT)
        Dim srs_projJS As String = ""
        sr.ExportToProj4(srs_projJS)

        Return srs_projJS
    End Function



    Function getCoords(ByVal theFile As String) As List(Of String)
        Gdal.AllRegister()
        getCoords = New List(Of String)

        Dim ds As Dataset = Gdal.Open(theFile, Access.GA_ReadOnly)

        If ds Is Nothing Then
            Exit Function
        End If



        getCoords.Add(GDALInfoGetPosition(ds, 0.0, 0.0)) 'ul
        getCoords.Add(GDALInfoGetPosition(ds, 0.0, ds.RasterYSize)) 'll
        getCoords.Add(GDALInfoGetPosition(ds, ds.RasterXSize, 0.0)) 'ur
        getCoords.Add(GDALInfoGetPosition(ds, ds.RasterXSize, ds.RasterYSize)) 'lr

    End Function



    Private Shared Function GDALInfoGetPosition(ds As Dataset, x As Double, y As Double) As String
        Dim adfGeoTransform As Double() = New Double(5) {}
        Dim dfGeoX As Double, dfGeoY As Double
        ds.GetGeoTransform(adfGeoTransform)

        dfGeoX = adfGeoTransform(0) + adfGeoTransform(1) * x + adfGeoTransform(2) * y
        dfGeoY = adfGeoTransform(3) + adfGeoTransform(4) * x + adfGeoTransform(5) * y

        Return dfGeoX.ToString() & "," & dfGeoY.ToString()
    End Function

End Class


Public NotInheritable Class EnvironmentalGdal
    'http://gdal2tilescsharp.codeplex.com/
    Private Sub New()
    End Sub
    ''' <summary>
    ''' Status for Set enviroment (Register drive, set path,...)
    ''' </summary>
    Private Shared _MakeEnvironment As Boolean = False

    ''' <summary>
    ''' Set environment (Register drive, set path,...)
    ''' </summary>
    Public Shared Sub MakeEnvironment(sPathProgram As String)
        If _MakeEnvironment Then
            Return
        End If

        _SetEnvironment(sPathProgram)

        ' Gdal.AllRegister()

        _MakeEnvironment = True
    End Sub

    ''' <summary>
    ''' Make and set variables system for GDAL DLL´s
    ''' Create variables GDAL (IF not exist):
    ''' - GDAL_DATA        -> %Program Path% \data
    ''' - GEOTIFF_CSV      -> %Program Path% \data
    ''' - GDAL_DRIVER_PATH -> %Program Path% \gdalplugins
    ''' 
    '''  Add PATH
    '''  - PATH            -> PATH + %Program Path% + %Program Path%\dll
    '''  
    ''' Struct folder for this program and files (Folder : files)
    ''' -- %Program Path% : %name program%, %this DLL%, %*_csharp.dll% (Ex.: GdalToTilesWin.exe, MngImg.dll, gdal_csharp.dll, osr_csharp.dll)
    '''          |_ dll: FWTools Dll´s for this program
    '''          |_ data: files find in FWTools\data
    '''          |_ gdalplugins: files find in FWTools\gdalplugins
    ''' </summary>
    Private Shared Sub _SetEnvironment(sPathProgram As String)
        ' Check exist variables, else, set variable
        _SetValueNewVariable("GDAL_DATA", sPathProgram & "\data")
        _SetValueNewVariable("GEOTIFF_CSV", sPathProgram & "\data")
        _SetValueNewVariable("GDAL_DRIVER_PATH", sPathProgram & "\gdalplugins")

        ' Add variable Path new folders
        Dim sVarPath As String = System.Environment.GetEnvironmentVariable("Path")
        sVarPath += sPathProgram & ";" & sPathProgram & "\dll"
        System.Environment.SetEnvironmentVariable("Path", sVarPath)
    End Sub
    Private Shared Sub _SetValueNewVariable(sVar As String, sValue As String)
        If System.Environment.GetEnvironmentVariable(sVar) Is Nothing Then
            System.Environment.SetEnvironmentVariable(sVar, sValue)
        End If
    End Sub
End Class




