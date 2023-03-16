' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module TimeZoneExtensions

    Private ReadOnly s_specialKnownTimeZones As New Dictionary(Of String, String) From {
            {"Amazon Standard Time", "Central Brazilian Standard Time"},
            {"Argentina Standard Time", "Argentina Standard Time"},
            {"Bolivia Time", "SA Western Standard Time"},
            {"Brasilia Standard Time", "Central Brazilian Standard Time"},
            {"Central European Summer Time", "W. Europe Standard Time"},
            {"Eastern European Summer Time", "E. Europe Daylight Time"},
            {"Eastern European Standard Time", "E. Europe Standard Time"},
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

        If s_systemTimeZones Is Nothing Then
            s_systemTimeZones = TimeZoneInfo.GetSystemTimeZones.ToList
        End If

        Dim possibleTimeZone As TimeZoneInfo = Nothing
        Try
            possibleTimeZone = TimeZoneInfo.FindSystemTimeZoneById(id)
        Catch ex As Exception

        End Try
        If possibleTimeZone IsNot Nothing Then
            Return possibleTimeZone
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
        If possibleTimeZone IsNot Nothing Then
            Return possibleTimeZone
        End If

        Return TimeZoneInfo.Local
    End Function

    Public Enum TimeZoneNameFormat
        BaseUtcOffset
        DisplayName
        StandardName
    End Enum

    Public Function GetTimeZoneName(format As TimeZoneNameFormat) As String
        If String.IsNullOrWhiteSpace(PumpTimeZoneInfo?.DisplayName) Then
            Return String.Empty
        End If
        Dim displayName As String = PumpTimeZoneInfo.DisplayName
        Select Case format
            Case TimeZoneNameFormat.DisplayName
                Return PumpTimeZoneInfo.DisplayName
            Case TimeZoneNameFormat.StandardName
                Return PumpTimeZoneInfo.StandardName
            Case TimeZoneNameFormat.BaseUtcOffset
                Return PumpTimeZoneInfo.BaseUtcOffset.ToString
            Case Else
                Return ""
        End Select
    End Function

    Public Function PumpNow() As Date
        Return TimeZoneInfo.ConvertTime(Now, PumpTimeZoneInfo)
    End Function

End Module
