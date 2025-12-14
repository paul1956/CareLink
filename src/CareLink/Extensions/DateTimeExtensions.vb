' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices

''' <summary>
'''  Provides extension methods for <see cref="Date"/> and related types,
'''  including parsing, formatting, and conversions between Unix time and DateTime.
''' </summary>
Friend Module DateTimeExtensions
    Private Const Format As String = "yyyy-MM-ddTHH:mm:ss"

    Private ReadOnly s_dateTimeFormatUniqueCultures As New List(Of CultureInfo)

    Private ReadOnly s_epochLocal As New DateTime(
        year:=1970,
        month:=1,
        day:=1,
        hour:=0,
        minute:=0,
        second:=0,
        kind:=DateTimeKind.Utc)

    Private ReadOnly s_epochUTC As New DateTime(
        year:=1970,
        month:=1,
        day:=1,
        hour:=0,
        minute:=0,
        second:=0,
        kind:=DateTimeKind.Utc)

    ''' <summary>
    '''  Parses a date string using culture-specific formats.
    '''  This method attempts to parse the date string in various cultures,
    '''  including the current culture, the provider culture, and a list of
    '''  unique cultures with different date formats.
    ''' </summary>
    ''' <param name="success">
    '''  Output parameter indicating whether the parsing was successful.
    ''' </param>
    ''' <param name="s">The date string to parse.</param>
    ''' <param name="defaultCulture">The default culture to use for parsing.</param>
    ''' <param name="styles">The styles to apply during parsing.</param>
    ''' <returns>
    '''  A <see cref="Date"/> object if parsing is successful;
    '''  otherwise, <see langword="Nothing"/>.
    ''' </returns>
    <Extension>
    Private Function CultureSpecificParse(s As String, styles As DateTimeStyles, ByRef success As Boolean) As Date
        If s_dateTimeFormatUniqueCultures.Count = 0 Then
            SyncLock s_dateTimeFormatUniqueCultures
                If s_dateTimeFormatUniqueCultures.Count = 0 Then
                    s_dateTimeFormatUniqueCultures.Add(item:=CurrentDateCulture)
                    Dim formatList As New List(Of String) From {CurrentDateCulture.DateTimeFormat.FullDateTimePattern}
                    For Each item As CultureInfo In CultureInfoList
                        If formatList.Contains(item:=item.DateTimeFormat.FullDateTimePattern) OrElse
                           IsNullOrWhiteSpace(value:=item.Name) OrElse
                           Not item.Name.Contains(value:="-"c) Then

                            Continue For
                        End If
                        s_dateTimeFormatUniqueCultures.Add(item)
                        formatList.Add(item:=item.DateTimeFormat.FullDateTimePattern)
                    Next
                End If
            End SyncLock
        End If
        Dim result As Date
        success = False
        Dim currentCultureLocal As CultureInfo = CurrentDateCulture
        Dim uiCultureLocal As CultureInfo = CultureInfo.CurrentUICulture
        Dim usCultureLocal As CultureInfo = usDataCulture

        If Date.TryParse(s, provider:=currentCultureLocal, styles, result) Then
            success = True
            Return result
        End If
        If currentCultureLocal.Name <> uiCultureLocal.Name AndAlso
            Date.TryParse(s, provider:=uiCultureLocal, styles, result) Then

            success = True
            Return result
        End If
        If Date.TryParse(s, provider:=usCultureLocal, styles, result) Then
            success = True
            Return result
        End If
        For Each provider As CultureInfo In s_dateTimeFormatUniqueCultures
            If Date.TryParse(s, provider, styles, result) Then
                success = True
                Return result
            End If
        Next
        Return Nothing
    End Function

    ''' <summary>
    '''  Converts a Unix Milliseconds TimeSpan to UTC Date
    ''' </summary>
    ''' <param name="unixTimeSpan" kind="Double">TimeSpan in Milliseconds</param>
    ''' <returns>UTC Date</returns>
    <Extension>
    Private Function FromUnixTime(unixTimeSpan As Double) As Date
        Return s_epochUTC.AddMilliseconds(value:=unixTimeSpan)
    End Function

    ''' <summary>
    '''  Converts a UNIX TimeSpan string to UTC DateTime
    ''' </summary>
    ''' <param name="unixTimeSpan">In Milliseconds As String</param>
    ''' <returns>DateTime String in UTC and local Times</returns>
    ''' <param name="isLocalTime"></param>
    <Extension>
    Friend Function Epoch2DateTimeString(unixTimeSpan As String, Optional isLocalTime As Boolean = False) As String
        If unixTimeSpan = "0" Then
            Return String.Empty
        End If
        Dim localTime As Date
        Dim unixTime As Date
        Dim pumpTime As Date
        If isLocalTime Then
            pumpTime = Date.SpecifyKind(value:=unixTimeSpan.FromUnixTime, kind:=DateTimeKind.Local)
            Dim offset As TimeSpan = TimeZoneInfo.Local.GetUtcOffset(dateTime:=pumpTime)
            Dim pumpTimeOffset As New DateTimeOffset(dateTime:=pumpTime, offset)
            offset = TimeZoneInfo.Local.GetUtcOffset(dateTime:=Now)
            Dim localTimeOffset As New DateTimeOffset(dateTime:=Now, offset)

            If pumpTimeOffset.Offset = localTimeOffset.Offset Then
                localTime = pumpTime
            Else
                ' They are in different zones or observed at different offsets (e.g., due to DST).
                ' Convert pumpTime to local
                localTime = TimeZoneInfo.ConvertTime(dateTime:=pumpTime, destinationTimeZone:=TimeZoneInfo.Local)
            End If
            unixTime = localTime.ToUniversalTime
        Else
            unixTime = unixTimeSpan.FromUnixTime
            localTime = unixTime.ToLocalTime
            pumpTime = unixTimeSpan.Epoch2PumpDateTime
        End If
        Dim localAndPumpTimeEqual As Boolean = pumpTime.ToString = localTime.ToString
        Dim timeStr As String = If(localAndPumpTimeEqual,
                                   $"Local & Pump = {localTime}",
                                   $"Local = {localTime}, Pump = {pumpTime}")

        Return $"{$"{unixTime} UTC"}, {timeStr}"
    End Function

    ''' <summary>
    '''  Converts a Unix Milliseconds TimeSpan to Pump DateTime
    ''' </summary>
    ''' <param name="unixTimeSpan">Long</param>
    ''' <returns>Local DateTime</returns>
    <Extension>
    Friend Function Epoch2PumpDateTime(unixTimeSpan As Long) As Date
        Return unixTimeSpan.ToString.Epoch2PumpDateTime
    End Function

    ''' <summary>
    '''  Converts a Unix Milliseconds TimeSpan to Pump DateTime
    ''' </summary>
    ''' <param name="unixTimeSpan>In Milliseconds As String</param>
    ''' <returns>Local DateTime</returns>
    <Extension>
    Friend Function Epoch2PumpDateTime(epoch As String) As Date
        Return TimeZoneInfo.ConvertTimeFromUtc(dateTime:=epoch.FromUnixTime(), destinationTimeZone:=PumpTimeZoneInfo)
    End Function

    ''' <summary>
    '''  Converts a Unix Milliseconds TimeSpan to UTC Date
    ''' </summary>
    ''' <param name="value" kind="String">In Milliseconds As String</param>
    ''' <returns>UTC Date</returns>
    <Extension>
    Friend Function FromUnixTime(value As String) As Date
        Return If(IsNullOrWhiteSpace(value),
                  s_epochLocal,
                  value.ParseDoubleInvariant.FromUnixTime)
    End Function

    ''' <summary>
    '''  Converts a Unix Milliseconds TimeSpan to UTC Date
    ''' </summary>
    ''' <param name="unixTime" kind="Double">In Milliseconds As Double</param>
    ''' <returns>UTC Date</returns>
    <Extension>
    Friend Function GetCurrentDateCulture(countryCode As String) As CultureInfo
        Dim code As String = $"en-{countryCode}"
        Dim predicate As Func(Of CultureInfo, Boolean) = Function(c As CultureInfo) As Boolean
                                                             Return c.Name = code
                                                         End Function
        Dim culture As CultureInfo = CultureInfoList.FirstOrDefault(predicate)
        Return If(culture, New CultureInfo(name:="en-US"))
    End Function

    ''' <summary>
    '''  Gets the timestamp of a marker, rounded down to the nearest minute.
    ''' </summary>
    ''' <param name="marker">
    '''  The <see cref="Marker"/> object whose timestamp is to be retrieved.
    ''' </param>
    ''' <returns>
    '''  The <see langword="Date"/> value of the marker's timestamp,
    '''  rounded down to the nearest minute.
    '''  Returns <see langword="Nothing"/> if an exception occurs.
    ''' </returns>
    ''' <remarks>
    '''  This method attempts to round down the marker's timestamp to the nearest minute.
    '''  If an exception occurs, execution is stopped for debugging
    '''  and <see langword="Nothing"/> is returned.
    ''' </remarks>
    <Extension>
    Friend Function GetMarkerTimestamp(marker As Marker) As Date
        Try
            Return marker.Timestamp.RoundDownToMinute()
        Catch ex As Exception
            Stop
        End Try
        Return Nothing
    End Function

    ''' <summary>
    '''  Parses a date <see langword="String"/> and returns a <see langword="Date"/> object.
    '''  If the parsing fails, it throws a <see cref="FormatException"/>
    '''  with details about the failure.
    ''' </summary>
    ''' <param name="dateAsString">The date string to parse.</param>
    ''' <param name="key">
    '''  A key that indicates the context of the date string, used for parsing rules.
    ''' </param>
    ''' <param name="memberName">
    '''  The name of the member calling this method, used for debugging.
    ''' </param>
    ''' <param name="sourceLineNumber">
    '''  The line number in the source code where this method is called, used for debugging.
    ''' </param>
    ''' <returns>A <see langword="Date"/> object if parsing is successful.</returns>
    ''' <exception cref="FormatException">
    '''  Thrown when the input string cannot be parsed as a valid
    '''  <see langword="Date"/> in any supported culture.
    ''' </exception>
    <Extension>
    Public Function ParseDate(
        dateAsString As String,
        key As String,
        <CallerMemberName> Optional memberName As String = Nothing,
        <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As Date

        Dim resultDate As Date
        If dateAsString.TryParseDate(key, resultDate) Then
            Return resultDate
        End If
        Stop
        Dim message As String = $"String '{dateAsString}' with {NameOf(key)} = {key} from " &
                                $"{memberName} line {sourceLineNumber} was not recognized as a valid " &
                                "DateTime in any supported culture."
        Throw New FormatException(message)
    End Function

    ''' <summary>
    '''  Rounds down the specified <see cref="Date"/> to the nearest minute by
    '''  setting seconds to zero.
    ''' </summary>
    ''' <param name="d">The <see cref="Date"/> to round down.</param>
    ''' <returns>A new <see cref="Date"/> with seconds set to zero.</returns>
    <Extension>
    Public Function RoundDownToMinute(d As Date) As Date
        Return New DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, second:=0)
    End Function

    Public Function ToDaysHours(hours As Integer) As String
        ' Hours must be positive and non-zero
        If hours <= 0 Then
            Throw New ArgumentOutOfRangeException(paramName:=NameOf(hours), message:="hours must be positive and non-zero")
        End If

        Dim days As Integer = hours \ 24
        Dim hrs As Integer = hours Mod 24

        Dim parts As New List(Of String)
        If days > 0 Then
            parts.Add($"{days} {If(days = 1, "day", "days")}")
        End If
        If hrs > 0 Then
            parts.Add($"{hrs} {If(hrs = 1, "hr", "hrs")}")
        End If

        If parts.Count = 0 Then
            ' This should not happen because hours > 0, but handle defensively
            Return "0 hrs"
        ElseIf parts.Count = 1 Then
            Return parts(0)
        Else
            Return $"{parts(0)} and {parts(1)}"
        End If
    End Function

    <Extension>
    Public Function ToHoursMinutes(minutes As Integer) As String
        Return New TimeSpan(hours:=0, minutes:=minutes \ 60, seconds:=minutes Mod 60).ToString.Substring(startIndex:=4)
    End Function

    ''' <summary>
    '''  Converts a <see langword="Single"/> representing hours into
    '''  a <see langword="String"/> formatted as hours and minutes.
    ''' </summary>
    ''' <param name="timeInHours">The number of hours to convert.</param>
    ''' <returns>
    '''  A <see langword="String"/> representing the time in "HH:mm" format.
    ''' </returns>
    <Extension>
    Public Function ToHoursMinutes(timeInHours As Single) As String
        Dim hours As Integer = CInt(timeInHours)
        Return $"{New TimeSpan(hours, minutes:=CInt((timeInHours - hours) * 60), seconds:=0):h\:mm}"
    End Function

    ''' <summary>
    '''  Converts a <see langword="TimeOnly"/> to a <see langword="String"/>
    '''  formatted as "HH:mm".
    ''' </summary>
    ''' <param name="timeOnly">The <see langword="TimeOnly"/> to convert.</param>
    ''' <returns>
    '''  A <see langword="String"/> representing the time in "HH:mm" format.
    ''' </returns>
    <Extension>
    Public Function ToHoursMinutes(timeOnly As TimeOnly) As String
        Dim rawTimeOnly As String = $"{timeOnly.ToString(provider:=CurrentDateCulture)}"
        Return If(rawTimeOnly.Split(separator:=":")(0).Length = 1,
                  $" {rawTimeOnly}",
                  rawTimeOnly)

    End Function

    ''' <summary>
    '''  Converts a <see langword="Date"/> to a <see langword="String"/>
    '''  formatted as "ddd, MMM d HH:mm".
    ''' </summary>
    ''' <param name="triggeredDateTime">
    '''  The <see langword="Date"/> to convert.
    ''' </param>
    ''' <returns>
    '''  A <see langword="String"/> representing the date in the specified format.
    ''' </returns>
    <Extension>
    Public Function ToNotificationString(triggeredDateTime As Date) As String
        Return triggeredDateTime.ToString(format:=$"ddd, MMM d,{s_timeWithMinuteFormat}")
    End Function

    ''' <summary>
    '''  Converts a <see langword="Date"/> to a <see langword="String"/>
    '''  formatted as "MM/dd/yyyy HH:mm:ss".
    ''' </summary>
    ''' <param name="dateValue">
    '''  The <see langword="Date"/> to convert.
    ''' </param>
    ''' <returns>
    '''  A <see langword="String"/> representing the date in "MM/dd/yyyy HH:mm:ss" format.
    ''' </returns>
    <Extension>
    Public Function ToShortDateTime(dateValue As Date) As String
        Return $"{dateValue:d} {dateValue:T}"
    End Function

    ''' <summary>
    '''  Converts a <see cref="Date"/> to a <see langword="String"/>
    '''  with the specified format.
    '''  Defaults to "yyyy-MM-ddTHH:mm:ss" if no format is provided.
    ''' </summary>
    ''' <param name="d">The date to convert.</param>
    ''' <returns>The formatted date as a string.</returns>
    <Extension>
    Public Function ToStringExact(d As Date) As String
        Return d.ToString(Format, provider:=CultureInfo.InvariantCulture)
    End Function

    ''' <summary>
    '''  Try to parse a date <see langword="String"/> (<paramref name="s"/>) into a
    '''  <see langword="Date"/>, using different parsing rules depending on the
    '''  provided key.
    ''' </summary>
    ''' <param name="s">The <see langword="String"/> to parse.</param>
    ''' <param name="key">
    '''  A <see langword="String"/> that determines which parsing rules to use.
    ''' </param>
    ''' <param name="result">The output variable for the parsed date.</param>
    ''' <returns>
    '''  <see langword="True"/> if parsing succeeds;
    '''  otherwise <see langword="False"/>.
    ''' </returns>
    <Extension>
    Public Function TryParseDate(s As String, key As String, ByRef result As Date) As Boolean
        Dim success As Boolean
        Select Case key
            Case String.Empty
                result = s.CultureSpecificParse(styles:=DateTimeStyles.AssumeLocal, success)
            Case NameOf(ServerDataEnum.lastConduitDateTime)
                result = s.CultureSpecificParse(styles:=DateTimeStyles.AssumeLocal, success)
            Case NameOf(ServerDataEnum.medicalDeviceTime)
                result = s.CultureSpecificParse(styles:=DateTimeStyles.AssumeLocal, success)
            Case "loginDateUTC"
                result = s.CultureSpecificParse(styles:=DateTimeStyles.AssumeUniversal, success)
            Case NameOf(SG.Timestamp)
                result = s.CultureSpecificParse(styles:=DateTimeStyles.AdjustToUniversal, success)
            Case NameOf(TimeChange.Timestamp), NameOf(ClearedNotifications.dateTime)
                result = s.CultureSpecificParse(styles:=DateTimeStyles.AdjustToUniversal, success)
            Case NameOf(ActiveNotification.SecondaryTime)
                result = s.CultureSpecificParse(styles:=DateTimeStyles.NoCurrentDateDefault, success)
            Case NameOf(ActiveNotification.triggeredDateTime)
                result = s.CultureSpecificParse(styles:=DateTimeStyles.AdjustToUniversal, success)
            Case "dateTime"
                result = s.CultureSpecificParse(styles:=DateTimeStyles.AdjustToUniversal, success)
            Case Else
        End Select
        Return success
    End Function

    ''' <summary>
    '''  Attempts to parse a date string in the format "yyyy-MM-ddTHH:mm:ss"
    '''  and returns the parsed date. If the string is null or whitespace,
    '''  it returns <see langword="Nothing"/>.
    ''' </summary>
    ''' <param name="s">The date string to parse.</param>
    ''' <returns>Parsed Date or Nothing if parsing fails.</returns>
    ''' <remarks>
    '''  This method is an extension method for the <see langword="String"/> class.
    '''  It uses the invariant culture to ensure consistent parsing regardless
    '''  of the current culture settings.
    ''' </remarks>
    <Extension>
    Public Function TryParseDateStr(s As String) As Date
        Dim provider As IFormatProvider = CultureInfo.InvariantCulture
        Return If(IsNotNullOrWhiteSpace(value:=s),
                  Date.ParseExact(s, format:="yyyy-MM-ddTHH:mm:ss", provider),
                  Nothing)
    End Function

End Module
