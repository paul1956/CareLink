' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices

Friend Module DateTimeExtensions

    Private ReadOnly s_dateTimeFormatUniqueCultures As New List(Of CultureInfo)

    Private Function DoCultureSpecificParse(dateAsString As String, ByRef success As Boolean, defaultCulture As CultureInfo, styles As DateTimeStyles) As Date
        If s_dateTimeFormatUniqueCultures.Count = 0 Then
            s_dateTimeFormatUniqueCultures.Add(CurrentDateCulture)
            Dim fullDateTimeFormats As New List(Of String) From {
                CurrentDateCulture.DateTimeFormat.FullDateTimePattern
            }
            For Each oneCulture As CultureInfo In CultureInfoList
                If fullDateTimeFormats.Contains(oneCulture.DateTimeFormat.FullDateTimePattern) OrElse
                                String.IsNullOrWhiteSpace(oneCulture.Name) OrElse
                                Not oneCulture.Name.Contains("-"c) Then
                    Continue For
                End If
                s_dateTimeFormatUniqueCultures.Add(oneCulture)
                fullDateTimeFormats.Add(oneCulture.DateTimeFormat.FullDateTimePattern)
            Next
        End If
        Dim resultDate As Date
        success = True
        If Date.TryParse(dateAsString, defaultCulture, styles, resultDate) Then
            Return resultDate
        End If
        If CurrentDateCulture.Name <> Provider.Name AndAlso
        Date.TryParse(dateAsString, Provider, styles, resultDate) Then
            Return resultDate
        End If
        If Date.TryParse(dateAsString, usDataCulture, styles, resultDate) Then
            Return resultDate
        End If
        For Each c As CultureInfo In s_dateTimeFormatUniqueCultures
            If Date.TryParse(dateAsString, c, styles, resultDate) Then
                Return resultDate
            End If
        Next
        success = False
        Return Nothing
    End Function

    ''' <summary>
    ''' Converts a Unix Milliseconds TimeSpan to UTC Date
    ''' </summary>
    ''' <param name="unixTime" kind="Double">TimeSpan in Milliseconds</param>
    ''' <returns>UTC Date</returns>
    <Extension>
    Private Function FromUnixTime(unixTime As Double) As Date
        Dim epoch As New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        Return epoch.AddMilliseconds(unixTime)
    End Function

    ''' <summary>
    ''' Converts a UNIX Timespan string to UTC DateTime
    ''' </summary>
    ''' <param name="epoch">In Milliseconds As String</param>
    ''' <returns>DateTime String in UTC and local Times</returns>
    <Extension>
    Friend Function Epoch2DateTimeString(epoch As String) As String
        If epoch = "0" Then
            Return ""
        End If
        Dim unixTime As Date = epoch.FromUnixTime
        Dim localTime As Date = unixTime.ToLocalTime
        Dim pumpTime As Date = epoch.Epoch2PumpDateTime
        Return $"{unixTime.ToShortDateTimeString} UTC{If(pumpTime.ToString = localTime.ToString, $"{Space(15)}{localTime} Local & Pump Time", $"{Space(15)}{localTime} Local Time{Space(15)} {pumpTime}Pump Time")}"
    End Function

    ''' <summary>
    ''' Converts a Unix Milliseconds TimeSpan to Pump DateTime
    ''' </summary>
    ''' <param name="epoch"></param>
    ''' <returns>Local DateTime</returns>
    <Extension>
    Friend Function Epoch2PumpDateTime(epoch As Long) As Date
        Return epoch.ToString.Epoch2PumpDateTime
    End Function

    ''' <summary>
    ''' Converts a Unix Milliseconds TimeSpan to Pump DateTime
    ''' </summary>
    ''' <param name="epoch"></param>
    ''' <returns>Local DateTime</returns>
    <Extension>
    Friend Function Epoch2PumpDateTime(epoch As String) As Date
        Return TimeZoneInfo.ConvertTimeFromUtc(epoch.FromUnixTime(), PumpTimeZoneInfo)
    End Function

    ''' <summary>
    ''' Converts a Unix Milliseconds TimeSpan to UTC Date
    ''' </summary>
    ''' <param name="unixTime" kind="String"></param>
    ''' <returns>UTC Date</returns>
    <Extension>
    Friend Function FromUnixTime(unixTime As String) As Date
        Dim epoch As New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)
        Return If(String.IsNullOrWhiteSpace(unixTime),
                  epoch,
                  Double.Parse(unixTime).FromUnixTime)
    End Function

    <Extension>
    Friend Function GetCurrentDateCulture(countryCode As String) As CultureInfo
        Dim localDateCulture As List(Of CultureInfo) = CultureInfoList.Where(Function(c As CultureInfo)
                                                                                 Return c.Name = $"en-{countryCode}"
                                                                             End Function)?.ToList
        Return If(localDateCulture Is Nothing OrElse localDateCulture.Count = 0,
                  New CultureInfo("en-US"),
                  localDateCulture(0))
    End Function

    <Extension>
    Friend Function GetMarkerTimestamp(marker As Marker) As Date
        Try
            Return marker.Timestamp.RoundDownToMinute()
        Catch ex As Exception
            Stop
        End Try
        Return Nothing
    End Function

    <Extension>
    Public Function ParseDate(dateAsString As String, key As String, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As Date
        Dim resultDate As Date
        If dateAsString.TryParseDate(resultDate, key) Then
            Return resultDate
        End If
        Stop
        Throw New FormatException($"String '{dateAsString}' with {NameOf(key)} = {key} from {memberName} line {sourceLineNumber} was not recognized as a valid DateTime in any supported culture.")
    End Function

    <Extension>
    Public Function ParseDate(marker As Dictionary(Of String, String), key As String, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As Date
        Dim resultDate As Date
        Dim dateAsString As String = marker(key)
        If dateAsString.TryParseDate(resultDate, key) Then
            Return resultDate
        End If

        Throw New FormatException($"String '{dateAsString}' with {NameOf(key)} = {key} from {memberName} line {sourceLineNumber} was not recognized as a valid DateTime in any supported culture.")
    End Function

    <Extension>
    Public Function RoundDownToMinute(d As Date) As Date
        Return New DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0)
    End Function

    <Extension>
    Public Function ToHours(minutes As Integer) As String
        Return New TimeSpan(0, minutes \ 60, minutes Mod 60).ToString.Substring(4)
    End Function

    <Extension>
    Public Function ToHoursMinutes(timeInHours As Single) As String
        Dim hours As Integer = CInt(timeInHours)
        Return $"{New TimeSpan(hours, CInt((timeInHours - hours) * 60), 0):h\:mm}"

    End Function

    <Extension>
    Public Function ToNotificationDateTimeString(triggeredDateTime As Date) As String
        Return triggeredDateTime.ToString($"ddd, MMM d {s_timeWithMinuteFormat}")
    End Function

    <Extension>
    Public Function ToHoursMinutes(timeOnly As TimeOnly) As String
        Dim rawTimeOnly As String = $" {timeOnly.ToString(CurrentDateCulture)}"
        Return If(rawTimeOnly.Split(":")(0).Length = 1,
                  $" {rawTimeOnly}",
                  rawTimeOnly
                 )

    End Function

    <Extension>
    Public Function ToShortDateTimeString(dateValue As Date) As String
        Return $"{dateValue.ToShortDateString()} {dateValue.ToLongTimeString()}"
    End Function

    <Extension>
    Public Function TryParseDate(dateAsString As String, ByRef resultDate As Date, key As String) As Boolean
        Dim success As Boolean
        Select Case key
            Case ""
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AssumeLocal)
            Case NameOf(ServerDataIndexes.lastConduitDateTime)
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AssumeLocal)
            Case NameOf(ServerDataIndexes.medicalDeviceTime)
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AdjustToUniversal)
            Case "loginDateUTC"
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AssumeUniversal)
            Case NameOf(SG.Timestamp)
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AdjustToUniversal)
            Case NameOf(TimeChange.Timestamp), NameOf(ClearedNotifications.dateTime)
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AdjustToUniversal)
            Case NameOf(ActiveNotification.SecondaryTime)
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.NoCurrentDateDefault)
            Case NameOf(ActiveNotification.triggeredDateTime)
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AdjustToUniversal)
            Case "dateTime"
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AdjustToUniversal)
            Case Else
        End Select

        Return success
    End Function

End Module
