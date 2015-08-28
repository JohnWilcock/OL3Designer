Imports System.Collections.Generic
Imports System.Text
Imports ProjNet.CoordinateSystems
Imports ProjNet.Converters
Imports System.Reflection
Imports System.IO

Public Class ProjectionsAndTransformations

    Function convertCoords(ByVal GridX As Double, ByVal GridY As Double, ByVal fromCoord As String, ByVal toCoord As String) As Double()
        Dim fromCoordSystem As ProjNet.CoordinateSystems.ICoordinateSystem = TryCast(ProjNet.Converters.WellKnownText.CoordinateSystemWktReader.Parse(fromCoord), ICoordinateSystem)
        Dim toCoordSystem As ProjNet.CoordinateSystems.ICoordinateSystem = TryCast(ProjNet.Converters.WellKnownText.CoordinateSystemWktReader.Parse(toCoord), ICoordinateSystem)


        Dim ctfac As New ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory()
        Dim trans As ProjNet.CoordinateSystems.Transformations.ICoordinateTransformation = ctfac.CreateFromCoordinateSystems(fromCoordSystem, toCoordSystem)

        'converts coords
        Dim fromPoint As Double() = New Double() {GridX, GridY}
        convertCoords = trans.MathTransform.Transform(fromPoint)

    End Function
End Class




Public Class SridReader

    'Private sridFile As System.IO.StreamReader = System.IO.File.OpenText(Global.My.Resources.Resources.SRID)
    Shared filename As String = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\CRS\SRID.csv"

    'Change this to point to the SRID.CSV file.
    Public Structure WKTstring
        ''' <summary>Well-known ID</summary>
        Public WKID As Integer
        ''' <summary>Well-known Text</summary>
        Public WKT As String
        ''' <summary>Well-known name</summary>
        Public WKTName As String
    End Structure
    ''' <summary>Enumerates all SRID's in the SRID.csv file.</summary>
    ''' <returns>Enumerator</returns>
    Public Shared Iterator Function GetSRIDs() As IEnumerable(Of WKTstring)
        Using sr As System.IO.StreamReader = System.IO.File.OpenText(filename)
            'Using sr As System.IO.StreamReader = System.IO.File.OpenText(Global.My.Resources.Resources.SRID)
            While Not sr.EndOfStream
                Dim line As String = sr.ReadLine()
                Dim split As Integer = line.IndexOf(";"c)
                If split > -1 Then
                    Dim wkt As New WKTstring()
                    wkt.WKID = Integer.Parse(line.Substring(0, split))
                    wkt.WKT = line.Substring(split + 1)
                    wkt.WKTName = line.Split(",")(0).Split(";")(1).Substring(7)
                    Yield wkt
                End If
            End While
            sr.Close()
        End Using
    End Function
    ''' <summary>Gets a coordinate system from the SRID.csv file</summary>
    ''' <param name="id">EPSG ID</param>
    ''' <returns>Coordinate system, or null if SRID was not found.</returns>
    Public Shared Function GetCSbyID(id As Integer) As ICoordinateSystem
        For Each wkt As SridReader.WKTstring In SridReader.GetSRIDs()
            If wkt.WKID = id Then
                'We found it!
                Return TryCast(ProjNet.Converters.WellKnownText.CoordinateSystemWktReader.Parse(wkt.WKT), ICoordinateSystem)
            End If
        Next
        Return Nothing
    End Function



End Class
