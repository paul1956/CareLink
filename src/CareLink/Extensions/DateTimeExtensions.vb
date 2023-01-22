' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices

Friend Module DateTimeExtensions

    Private ReadOnly s_dateTimeFormatUniqueCultures As New List(Of CultureInfo)

#Region "OaDateTime Constants"

    Public ReadOnly s_fiveMinuteOADate As New OADate(Date.MinValue + New TimeSpan(hours:=0, minutes:=5, seconds:=0))
    Public ReadOnly s_hourAsOADate As New OADate(Date.MinValue + New TimeSpan(hours:=1, minutes:=0, seconds:=0))
    Public ReadOnly s_sixMinuteOADate As New OADate(Date.MinValue + New TimeSpan(hours:=0, minutes:=6, seconds:=0))
    Public ReadOnly s_twoHalfMinuteOADate As New OADate(Date.MinValue + New TimeSpan(hours:=0, minutes:=2, seconds:=30))

#End Region ' OaDateTime Constants

    Public ReadOnly s_fiveMinuteSpan As New TimeSpan(hours:=0, minutes:=5, seconds:=0)

#Region "Millisecond Constants"

    Public ReadOnly s_fiveMinutesInMilliseconds As Integer = CInt(New TimeSpan(0, minutes:=5, 0).TotalMilliseconds)
    Public ReadOnly s_oneMinutesInMilliseconds As Integer = CInt(New TimeSpan(0, minutes:=1, 0).TotalMilliseconds)
    Public ReadOnly s_thirtySecondInMilliseconds As Integer = CInt(New TimeSpan(0, 0, seconds:=30).TotalMilliseconds)

#End Region 'Millisecond Constants

    Public Enum RoundTo
        Second
        Minute
        Hour
        Day
    End Enum

    Private Function DoCultureSpecificParse(dateAsString As String, ByRef success As Boolean, defaultCulture As CultureInfo, styles As DateTimeStyles) As Date
        If s_dateTimeFormatUniqueCultures.Count = 0 Then
            s_dateTimeFormatUniqueCultures.Add(CurrentDateCulture)
            Dim fullDateTimeFormats As New List(Of String) From {
                CurrentDateCulture.DateTimeFormat.FullDateTimePattern
            }
            For Each oneCulture As CultureInfo In s_cultureInfoArray.ToList()
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
        If CurrentDateCulture.Name <> CurrentUICulture.Name AndAlso
        Date.TryParse(dateAsString, CurrentUICulture, styles, resultDate) Then
            Return resultDate
        End If
        If Date.TryParse(dateAsString, CurrentDataCulture, styles, resultDate) Then
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
    ''' <param name="unixTime" type="String"></param>
    ''' <returns>UTC Date</returns>
    <Extension>
    Private Function FromUnixTime(unixTime As String) As Date
        Dim epoch As New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)
        If String.IsNullOrWhiteSpace(unixTime) Then
            Return epoch
        End If

        Return CDbl(unixTime).FromUnixTime
    End Function

    ''' <summary>
    ''' Converts a Unix Milliseconds TimeSpan to UTC Date
    ''' </summary>
    ''' <param name="unixTime" type="Double">TimeSpan in Milliseconds</param>
    ''' <returns>UTC Date</returns>
    <Extension>
    Private Function FromUnixTime(unixTime As Double) As Date
        Dim epoch As New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        Return epoch.AddMilliseconds(unixTime)
    End Function

    ''' <summary>
    ''' Converts a Unix Milliseconds TimeSpan to local date
    ''' </summary>
    ''' <param name="epoch"></param>
    ''' <returns>Local Date as string</returns>
    <Extension>
    Friend Function Epoch2DateString(epoch As String) As String
        Return epoch.FromUnixTime.ToLongDateString
    End Function

    ''' <summary>
    ''' Converts a Unix Milliseconds TimeSpan to local DateTime
    ''' </summary>
    ''' <param name="epoch"></param>
    ''' <returns>Local DateTime</returns>
    <Extension>
    Friend Function Epoch2DateTime(epoch As Long) As Date
        Return epoch.ToString.FromUnixTime.ToLocalTime
    End Function

    ''' <summary>
    ''' Converts a Unix Milliseconds TimeSpan to local DateTime
    ''' </summary>
    ''' <param name="epoch"></param>
    ''' <returns>Local DateTime</returns>
    <Extension>
    Friend Function Epoch2DateTime(epoch As String) As Date
        Return epoch.FromUnixTime.ToLocalTime
    End Function

    ''' <summary>
    ''' Converts a UNIX Timespan string to UTC DateTime
    ''' </summary>
    ''' <param name="epoch">In Milliseconds As String</param>
    ''' <returns>DateTime String in UTC</returns>
    <Extension>
    Friend Function Epoch2DateTimeString(epoch As String) As String
        If epoch = "0" Then
            Return ""
        End If
        Return $"{epoch.FromUnixTime.ToShortDateTimeString} UTC"
    End Function

    <Extension>
    Friend Function GetCurrentDateCulture(countryCode As String) As CultureInfo
        Dim localDateCulture As List(Of CultureInfo) = s_cultureInfoArray.Where(Function(c As CultureInfo)
                                                                                    Return c.Name = $"en-{countryCode}"
                                                                                End Function)?.ToList
        If localDateCulture Is Nothing OrElse localDateCulture.Count = 0 Then
            Return New CultureInfo("en-US")
        End If
        Return localDateCulture(0)
    End Function

    <Extension>
    Friend Function SafeGetSgDateTime(sgList As List(Of Dictionary(Of String, String)), index As Integer) As Date
        Dim sgDateTimeString As String = ""
        Dim sgDateTime As Date
        If sgList(index).Count < 7 Then
            index -= 1
        End If
        If sgList(index).TryGetValue(NameOf(SgRecord.datetime), sgDateTimeString) Then
            sgDateTime = sgDateTimeString.ParseDate(NameOf(SgRecord.datetime))
        ElseIf sgList(index).TryGetValue(NameOf(TimeChangeRecord.dateTime), sgDateTimeString) Then
            sgDateTime = sgDateTimeString.ParseDate(NameOf(TimeChangeRecord.dateTime))
        ElseIf sgList(index).TryGetValue(NameOf(TimeChangeRecord.previousDateTime), sgDateTimeString) Then
            sgDateTime = sgDateTimeString.ParseDate(NameOf(TimeChangeRecord.previousDateTime))
        Else
            sgDateTime = Now
        End If
        If sgDateTime.Year = 2000 Then
            sgDateTime = Date.Now - ((sgList.Count - index) * s_fiveMinuteSpan)
        End If
        If sgList(index).Count < 7 Then
            sgDateTime = sgDateTime.AddMinutes(5)
        End If
        Return sgDateTime
    End Function

    <Extension>
    Public Function CDateOrDefault(dateAsString As String, key As String, provider As IFormatProvider) As String
        Dim resultDate As Date
        If TryParseDate(dateAsString, resultDate, key) Then
            Return resultDate.ToString(provider)
        End If
        Return dateAsString
    End Function

    <Extension>
    Public Function ParseDate(dateAsString As String, key As String, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As Date
        Dim resultDate As Date
        If dateAsString.TryParseDate(resultDate, key) Then
            Return resultDate
        End If

        Throw New System.FormatException($"String '{dateAsString}' with {NameOf(key)} = {key} from {memberName} line {sourceLineNumber} was not recognized as a valid DateTime in any supported culture.")
    End Function

    <Extension>
    Public Function RoundTimeDown(d As Date, rt As RoundTo) As Date
        Dim dtRounded As New DateTime()

        Select Case rt
            Case RoundTo.Second
                dtRounded = New DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second)
            Case RoundTo.Minute
                dtRounded = New DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0)
            Case RoundTo.Hour
                dtRounded = New DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0)
            Case RoundTo.Day
                dtRounded = New DateTime(d.Year, d.Month, d.Day, 0, 0, 0)
        End Select

        Return dtRounded
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
            Case NameOf(ItemIndexes.lastConduitDateTime)
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AssumeLocal)
            Case NameOf(ItemIndexes.medicalDeviceTime)
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AdjustToUniversal)
            Case NameOf(ItemIndexes.lastSensorTS)
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AdjustToUniversal)
            Case "loginDateUTC"
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AssumeUniversal)
                Stop
            Case NameOf(SgRecord.datetime), NameOf(TimeChangeRecord.dateTime)
                If key = NameOf(SgRecord.datetime) Then
                    resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AdjustToUniversal)
                Else
                    resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AdjustToUniversal)
                End If
            Case NameOf(TimeChangeRecord.previousDateTime)
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AssumeUniversal)
            Case NameOf(ClearedNotificationsRecord.secondaryTime)
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.NoCurrentDateDefault)
            Case NameOf(ClearedNotificationsRecord.triggeredDateTime)
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AdjustToUniversal)
            Case Else
        End Select

        Return success
    End Function

End Module
