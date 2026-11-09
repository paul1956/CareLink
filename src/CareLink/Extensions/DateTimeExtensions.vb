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

    Private ReadOnly s_epochLocal As New Date(
        year:=1970,
        month:=1,
        day:=1,
        hour:=0,
        minute:=0,
        second:=0,
        kind:=DateTimeKind.Utc)

    Friend ReadOnly s_epochUTC As New Date(
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
                    Dim formatList As New List(Of String) From {
                        CurrentDateCulture.DateTimeFormat.FullDateTimePattern}

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
        Dim usCultureLocal As CultureInfo = UsDataCulture

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
    '''  Converts Unix time in milliseconds to a UTC date.
    ''' </summary>
    ''' <param name="milliseconds">Unix time in milliseconds.</param>
    ''' <returns>A UTC date.</returns>
    <Extension>
    Private Function FromUnixTime(milliseconds As Double) As Date
        Return s_epochUTC.AddMilliseconds(value:=milliseconds)
    End Function

    ''' <summary>
    '''  Joins a larger and smaller unit as "big and small", omitting zero parts.
    '''  Returns "0 <paramref name="smallPlural"/>" when both are zero.
    ''' </summary>
    Private Function JoinUnits(big As Long,
                               bigSingular As String,
                               bigPlural As String,
                               small As Long,
                               smallSingular As String,
                               smallPlural As String) As String
        Dim bigText As String =
            Pluralize(count:=big, singular:=bigSingular, plural:=bigPlural)
        Dim smallText As String =
            Pluralize(count:=small, singular:=smallSingular, plural:=smallPlural)
        Return If(big > 0 AndAlso small > 0,
                  $"{bigText} and {smallText}",
                  If(big > 0,
                     bigText,
                     smallText))
    End Function

    ''' <summary>
    '''  Returns "n unit" using the singular form when <paramref name="count"/> is 1.
    ''' </summary>
    Private Function Pluralize(count As Long, singular As String, plural As String) As String
        Return $"{count} {If(count = 1, singular, plural)}"
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
            offset = TimeZoneInfo.Local.GetUtcOffset(dateTime:=Date.Now)
            Dim localTimeOffset As New DateTimeOffset(dateTime:=Date.Now, offset)

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
                                   $"    Local and Pump Time are {localTime}",
                                   $"    Local time is {localTime}, Pump Time is {pumpTime}")

        Return $"{unixTime} UTC, {timeStr}"
    End Function

    ''' <summary>
    '''  Converts Unix time in milliseconds to the pump's date and time.
    ''' </summary>
    ''' <param name="unixTimeSpan">Unix time in milliseconds.</param>
    ''' <returns>The corresponding date and time in the pump's time zone.</returns>
    <Extension>
    Friend Function Epoch2PumpDateTime(unixTimeSpan As Long) As Date
        Return TimeZoneInfo.ConvertTimeFromUtc(dateTime:=FromUnixTime(CDbl(unixTimeSpan)),
                                               destinationTimeZone:=PumpTimeZoneInfo)
    End Function

    ''' <summary>
    '''  Converts Unix time in milliseconds to the pump's date and time.
    ''' </summary>
    ''' <param name="epoch">Unix time in milliseconds.</param>
    ''' <returns>The corresponding date and time in the pump's time zone.</returns>
    <Extension>
    Friend Function Epoch2PumpDateTime(epoch As String) As Date
        Return TimeZoneInfo.ConvertTimeFromUtc(dateTime:=epoch.FromUnixTime(), destinationTimeZone:=PumpTimeZoneInfo)
    End Function

    ''' <summary>
    '''  Converts Unix time in milliseconds to a UTC date.
    ''' </summary>
    ''' <param name="value">Unix time in milliseconds, as a string.</param>
    ''' <returns>A UTC date.</returns>
    <Extension>
    Friend Function FromUnixTime(value As String) As Date
        Return If(IsNullOrWhiteSpace(value),
                  s_epochLocal,
                  value.ParseDoubleInvariant.FromUnixTime)
    End Function

    ''' <summary>
    '''  Gets the current date culture for the specified country code.
    ''' </summary>
    ''' <param name="countryCode">The two-letter country code.</param>
    ''' <returns>The matching English culture, or <see cref="CultureInfo"/> for en-US if none is found.</returns>
    <Extension>
    Friend Function GetCurrentDateCulture(countryCode As String) As CultureInfo
        Dim code As String = $"en-{countryCode}"
        Dim predicate As Func(Of CultureInfo, Boolean) =
            Function(c As CultureInfo) As Boolean
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
    '''  Determines whether a given <see cref="Date"/> occurs earlier than a cutoff point defined by subtracting
    '''  a <see cref="TimeSpan"/> from a reference date.
    ''' </summary>
    ''' <param name="dateTime">The date and time to evaluate.</param>
    ''' <param name="referenceDate">The baseline date from which the comparison window is calculated.</param>
    ''' <param name="span">
    '''  The amount of time to subtract from <paramref name="referenceDate"/> to establish the cutoff threshold.
    ''' </param>
    ''' <returns>
    '''  True if <paramref name="dateTime"/> is earlier than
    '''  (<paramref name="referenceDate"/> minus <paramref name="span"/>);
    '''  otherwise, false.
    ''' </returns>
    ''' <example>
    '''  Dim referenceDate As Date = Date.Now
    '''  Dim span As TimeSpan = TimeSpan.FromDays(7)
    '''  Dim inputDate As Date = Date.Now.AddDays(-10)
    '''
    '''  ' Cutoff = referenceDate - 7 days
    '''  ' inputDate is 10 days ago, so it is older than the cutoff → True
    '''  Dim result As Boolean = inputDate.IsDateOlderThan(referenceDate, span)
    ''' </example>

    <Extension>
    Public Function IsDateOlderThan(dateTime As Date, referenceDate As Date, span As TimeSpan) As Boolean
        Return dateTime < (referenceDate - span)
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

        Dim result As Date
        If dateAsString.TryParseDate(key, result) Then
            Return result
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
        Return New Date(d.Year, d.Month, d.Day, d.Hour, d.Minute, second:=0)
    End Function

    ''' <summary>
    '''  Converts a number of hours into text such as "1 day and 2 hrs".
    ''' </summary>
    ''' <param name="hours">The number of hours, zero or greater.</param>
    ''' <returns>The text, or "0 hrs" when <paramref name="hours"/> is zero.</returns>
    <Extension>
    Public Function ToDaysHours(hours As Integer) As String
        If hours < 0 Then
            Const message As String = "hours must not be negative"
            Throw New ArgumentOutOfRangeException(paramName:=NameOf(hours), message)
        End If

        Return JoinUnits(big:=hours \ 24, bigSingular:="day", bigPlural:="days",
                         small:=hours Mod 24, smallSingular:="hr", smallPlural:="hrs")
    End Function

    ''' <summary>
    '''  Converts an <see langword="Integer"/> number of minutes into "h:mm",
    '''  with a leading space when the hour has a single digit.
    ''' </summary>
    ''' <param name="minutes">The number of minutes to convert.</param>
    <Extension>
    Public Function ToHoursMinutes(minutes As Integer) As String
        Dim sign As String = If(minutes < 0, "-", "")
        Dim absMinutes As Integer = Math.Abs(value:=minutes)
        Dim hours As Integer = absMinutes \ 60
        Dim hoursText As String = hours.ToString(provider:=CultureInfo.InvariantCulture)
        Dim minutesText As String = (absMinutes Mod 60).ToString(format:="00",
                                                                 provider:=CultureInfo.InvariantCulture)
        Dim result As String = $"{sign}{hoursText}:{minutesText}"
        Return If(result.IndexOf(value:=":"c) = 1, $" {result}", result)
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
        Dim totalMinutes As Integer = CInt(Math.Round(CDbl(timeInHours) * 60.0R))
        Dim sign As String = If(totalMinutes < 0, "-", "")
        totalMinutes = Math.Abs(value:=totalMinutes)
        Dim hoursText As String = (totalMinutes \ 60).ToString(provider:=CultureInfo.InvariantCulture)
        Dim minutesText As String = (totalMinutes Mod 60).ToString(format:="00",
                                                                   provider:=CultureInfo.InvariantCulture)
        Return $"{sign}{hoursText}:{minutesText}"
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
    '''  Converts a <see langword="Long"/> representing minutes into
    '''  a <see langword="String"/> formatted as hours and minutes.
    ''' </summary>
    ''' <param name="minutes">The number of minutes to convert.</param>
    ''' <returns>
    '''  A <see langword="String"/> representing the time in hours and minutes.
    ''' </returns>
    <Extension>
    Public Function ToHrMinText(minutes As Long) As String
        If minutes < 0 Then
            Const message As String = "minutes must not be negative"
            Throw New ArgumentOutOfRangeException(paramName:=NameOf(minutes), message)
        End If

        Return JoinUnits(big:=minutes \ 60, bigSingular:="hr", bigPlural:="hrs",
                         small:=minutes Mod 60, smallSingular:="min", smallPlural:="mins")
    End Function

    ''' <summary>
    '''  Converts a <see langword="Date"/> to a <see langword="String"/>
    '''  formatted as "MMM dd, yyyy HH:mm".
    ''' </summary>
    ''' <param name="triggeredDateTime">
    '''  The <see langword="Date"/> to convert.
    ''' </param>
    ''' <returns>
    '''  A <see langword="String"/> representing the date in the specified format.
    ''' </returns>
    <Extension>
    Public Function ToNotificationString(triggeredDateTime As Date) As String
        Return triggeredDateTime.ToString(format:=$"MMM dd, yyyy {s_timeWithMinuteFormat}")
    End Function

    ''' <summary>
    '''  Converts a <see langword="Date"/> to a <see langword="String"/>
    '''  formatted as "MM/dd/yyyy HH:mm:ss".
    ''' </summary>
    ''' <param name="dateValue">
    '''  The <see langword="Date"/> to convert.
    ''' </param>
    ''' <param name="showSeconds">Controls if seconds are displayed</param>
    ''' <returns>
    '''  A <see langword="String"/> representing the date in "MM/dd/yyyy HH:mm:ss" format or
    '''  "MM/dd/yyyy HH:mm" is showSeconds os false.
    ''' </returns>
    <Extension>
    Public Function ToShortDateTime(dateValue As Date,
                                    Optional showSeconds As Boolean = True) As String

        Dim timeFormat As String = If(showSeconds, "T", "t")
        Return $"{dateValue:d} {dateValue.ToString(format:=timeFormat)}"

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
            Case NameOf(TimeChange.Timestamp), "dateTime"
                result = s.CultureSpecificParse(styles:=DateTimeStyles.AdjustToUniversal, success)
            Case NameOf(ActiveNotification.SecondaryTime)
                result = s.CultureSpecificParse(styles:=DateTimeStyles.NoCurrentDateDefault, success)
            Case NameOf(ActiveNotification.TriggeredDateTime)
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
        If IsNullOrWhiteSpace(value:=s) Then
            Return Nothing
        End If

        Dim provider As IFormatProvider = CultureInfo.InvariantCulture
        Dim result As Date
        ' Try the original exact format first to preserve existing behavior
        If Date.TryParseExact(s, Format, provider, style:=DateTimeStyles.None, result) Then
            Return result
        End If

        ' Fallback: support ISO 8601 inputs with fractional seconds and timezone offsets
        ' Parse as DateTimeOffset then convert to Local Date per chosen behavior
        Dim dto As DateTimeOffset
        If DateTimeOffset.TryParse(input:=s,
                                   formatProvider:=provider,
                                   styles:=DateTimeStyles.None,
                                   result:=dto) Then
            Return dto.LocalDateTime
        End If

        ' As a last resort, try a broader Date parse using invariant culture
        If Date.TryParse(s, provider:=CultureInfo.InvariantCulture, styles:=DateTimeStyles.None, result) Then
            Return result
        End If

        ' Let caller handle invalid format by throwing like original code would
        Throw New FormatException(message:=$"String '{s}' was not recognized as a valid DateTime.")
    End Function

End Module
