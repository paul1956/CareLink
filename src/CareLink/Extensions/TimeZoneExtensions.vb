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
    ''' <remarks>
    '''  If <see cref="My.Settings.UseLocalTimeZone"/> is set, it returns <see cref="TimeZoneInfo.Local"/>.
    '''  Otherwise, it tries to find the time zone in the cache or system time zones.
    ''' </remarks>
    Friend Function CalculateTimeZone(timeZoneName As String) As TimeZoneInfo
        If String.IsNullOrWhiteSpace(value:=timeZoneName) Then
            Return Nothing
        End If

        If My.Settings.UseLocalTimeZone Then
            Return TimeZoneInfo.Local
        End If

        ' Try to map special known time zones, otherwise use input as ID
        Dim value As String = ""
        value = If(s_specialKnownTimeZones.TryGetValue(key:=timeZoneName, value),
                   value,
                   timeZoneName)

        ' Attempt to get from cache
        Dim tz As TimeZoneInfo = Nothing
        If s_timeZoneMap.TryGetValue(key:=value, value:=tz) Then Return tz

        ' Try to find system time zone by id
        Try
            tz = TimeZoneInfo.FindSystemTimeZoneById(id:=value)
            s_timeZoneMap(key:=value) = tz
            Return tz
        Catch ex As TimeZoneNotFoundException
        Catch ex As Exception
            Stop
        End Try

        ' Cache system time zones for search
        If s_systemTimeZones Is Nothing Then
            s_systemTimeZones = TimeZoneInfo.GetSystemTimeZones.ToList()
        End If

        ' Look for matching DaylightName or StandardName or Id
        For Each selector As Func(Of TimeZoneInfo, String) In {
            Function(t As TimeZoneInfo) t.DaylightName,
            Function(t As TimeZoneInfo) t.StandardName,
            Function(t As TimeZoneInfo) t.Id}

            Dim predicate As Func(Of TimeZoneInfo, Boolean) =
                Function(arg As TimeZoneInfo) As Boolean
                    Return selector(arg) = value
                End Function
            tz = s_systemTimeZones.FirstOrDefault(predicate)
            If tz IsNot Nothing Then
                s_timeZoneMap(key:=value) = tz
                Return tz
            End If
        Next

        ' Fallback: use local time zone and cache
        tz = TimeZoneInfo.Local
        s_timeZoneMap(key:=value) = tz
        Return tz
    End Function

    ''' <summary>
    '''  Gets the local date and time from the pump's configured time zone.
    ''' </summary>
    ''' <returns>The pumps current <see cref="Date"/> in the applications local time.</returns>
    Public Function PumpNow() As Date
        Return TimeZoneInfo.ConvertTime(dateTime:=Now, destinationTimeZone:=PumpTimeZoneInfo)
    End Function

End Module
