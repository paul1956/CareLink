' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
'''  Provides extension methods and helpers for working with time zones in the application.
''' </summary>
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

    ''' <summary>
    '''  Cached dictionary of time zones, mapping IDs to <see cref="TimeZoneInfo"/> instances.
    ''' </summary>
    ''' <remarks>
    '''  This is used to avoid repeated lookups for the same time zone ID.
    ''' </remarks>
    Private ReadOnly s_timeZoneMap As New Dictionary(Of String, TimeZoneInfo) _
            (StringComparer.OrdinalIgnoreCase)

    ''' <summary>
    '''  Special known time zones with their corresponding standard time zone IDs.
    ''' </summary>
    ''' <remarks>
    '''  This is used to map common names to their official time zone IDs.
    ''' </remarks>
    Private ReadOnly s_specialKnownTimeZones As New Dictionary(Of String, String)(comparer:=StringComparer.OrdinalIgnoreCase) From {
            {"Amazon Standard Time", "Central Brazilian Standard Time"},
            {"Argentina Standard Time", "Argentina Standard Time"},
            {"Bolivia Time", "SA Western Standard Time"},
            {"Brasilia Standard Time", "Central Brazilian Standard Time"},
            {"British Summer Time", "GMT Daylight Time"},
            {"Central European Summer Time", "W. Europe Standard Time"},
            {"Eastern European Summer Time", "E. Europe Daylight Time"},
            {"Eastern European Standard Time", "E. Europe Standard Time"},
            {"Irish Standard Time", "GMT Daylight Time"},
            {"Mitteleuropäische Zeit", "W. Europe Standard Time"}}

    ''' <summary>
    '''  Cached list of system time zones.
    ''' </summary>
    Private s_systemTimeZones As List(Of TimeZoneInfo)

    ''' <summary>
    '''  Attempts to resolve a time zone by name, using known mappings and system time zones.
    ''' </summary>
    ''' <param name="timeZoneName">The name or ID of the time zone to resolve.</param>
    ''' <returns>
    '''  The resolved <see cref="TimeZoneInfo"/> if found; otherwise, <see cref="TimeZoneInfo.Local"/>.
    '''  Returns <see langword="Nothing"/> if the input is null or whitespace.
    ''' </returns>
    Friend Function CalculateTimeZone(timeZoneName As String) As TimeZoneInfo
        If String.IsNullOrWhiteSpace(timeZoneName) Then
            Return Nothing
        End If

        If My.Settings.UseLocalTimeZone Then
            Return TimeZoneInfo.Local
        End If

        Dim id As String = ""
        If Not s_specialKnownTimeZones.TryGetValue(timeZoneName, id) Then
            id = timeZoneName
        End If

        Dim possibleTimeZone As TimeZoneInfo = Nothing
        If s_timeZoneMap.TryGetValue(id, possibleTimeZone) Then
            Return possibleTimeZone
        End If

        Try
            possibleTimeZone = TimeZoneInfo.FindSystemTimeZoneById(id)
            If possibleTimeZone IsNot Nothing Then
                s_timeZoneMap.Add(id, possibleTimeZone)
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
            possibleTimeZone = s_systemTimeZones.Where(
                predicate:=Function(t As TimeZoneInfo)
                               Return t.DaylightName = id
                           End Function).FirstOrDefault
            If possibleTimeZone IsNot Nothing Then
                s_timeZoneMap.Add(id, possibleTimeZone)
                Return possibleTimeZone
            End If
        End If

        possibleTimeZone = s_systemTimeZones.Where(
                predicate:=Function(t As TimeZoneInfo)
                               Return t.StandardName = id
                           End Function).FirstOrDefault
        If possibleTimeZone IsNot Nothing Then
            s_timeZoneMap.Add(id, possibleTimeZone)
            Return possibleTimeZone
        End If

        possibleTimeZone = s_systemTimeZones.Where(
            predicate:=Function(t As TimeZoneInfo)
                           Return t.Id = id
                       End Function).FirstOrDefault

        possibleTimeZone = If(possibleTimeZone, TimeZoneInfo.Local)
        s_timeZoneMap.Add(id, possibleTimeZone)

        Return possibleTimeZone
    End Function

    ''' <summary>
    '''  Gets the local date and time from the pump's configured time zone.
    ''' </summary>
    ''' <returns>The pumps current <see cref="Date"/> in the applications local time.</returns>
    Public Function PumpNow() As Date
        Return TimeZoneInfo.ConvertTime(dateTime:=Now, destinationTimeZone:=PumpTimeZoneInfo)
    End Function

End Module
