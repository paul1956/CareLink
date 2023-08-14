' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module TimeZoneExtensions

#Region "Time Zone Helper"

    Private s_pumpTimeZoneInfo As TimeZoneInfo

    Friend Property PumpTimeZoneInfo As TimeZoneInfo
        Get
            Return If(s_pumpTimeZoneInfo, TimeZoneInfo.Local)
        End Get
        Set
            s_pumpTimeZoneInfo = Value
        End Set
    End Property

#End Region

    Private ReadOnly s_specialKnownTimeZones As New Dictionary(Of String, String) From {
            {"Amazon Standard Time", "Central Brazilian Standard Time"},
            {"Argentina Standard Time", "Argentina Standard Time"},
            {"Bolivia Time", "SA Western Standard Time"},
            {"Brasilia Standard Time", "Central Brazilian Standard Time"},
            {"British Summer Time", "GMT Daylight Time"},
            {"Central European Summer Time", "W. Europe Standard Time"},
            {"Eastern European Summer Time", "E. Europe Daylight Time"},
            {"Eastern European Standard Time", "E. Europe Standard Time"},
            {"Irish Standard Time", "GMT Daylight Time"},
            {"Mitteleuropäische Zeit", "W. Europe Standard Time"}
        }

    Private s_systemTimeZones As List(Of TimeZoneInfo)

    Friend Function CalculateTimeZone(Name As String) As TimeZoneInfo
        If String.IsNullOrWhiteSpace(Name) Then
            Return Nothing
        End If

        If My.Settings.UseLocalTimeZone Then
            Return TimeZoneInfo.Local
        End If

        Dim id As String = ""
        If Not s_specialKnownTimeZones.TryGetValue(Name, id) Then
            id = Name
        End If

        Dim possibleTimeZone As TimeZoneInfo
        Try
            possibleTimeZone = TimeZoneInfo.FindSystemTimeZoneById(id)
            If possibleTimeZone IsNot Nothing Then
                Return possibleTimeZone
            End If
        Catch ex As TimeZoneNotFoundException
        Catch ex1 As Exception
            Stop
        End Try

        If s_systemTimeZones Is Nothing Then
            s_systemTimeZones = TimeZoneInfo.GetSystemTimeZones.ToList
        End If

        If id.Contains("Daylight") Then
            possibleTimeZone = s_systemTimeZones.Where(Function(t As TimeZoneInfo)
                                                           Return t.DaylightName = id
                                                       End Function).FirstOrDefault
            If possibleTimeZone IsNot Nothing Then
                Return possibleTimeZone
            End If
        End If
        possibleTimeZone = s_systemTimeZones.Where(Function(t As TimeZoneInfo)
                                                       Return t.StandardName = id
                                                   End Function).FirstOrDefault
        If possibleTimeZone IsNot Nothing Then
            Return possibleTimeZone
        End If

        possibleTimeZone = s_systemTimeZones.Where(Function(t As TimeZoneInfo)
                                                       Return t.Id = id
                                                   End Function).FirstOrDefault
        Return If(possibleTimeZone,
                  TimeZoneInfo.Local
                 )
    End Function

    Public Function PumpNow() As Date
        Return TimeZoneInfo.ConvertTime(Now, PumpTimeZoneInfo)
    End Function

End Module
