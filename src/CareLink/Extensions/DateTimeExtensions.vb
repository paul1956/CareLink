' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices

Friend Module DateTimeExtensions

    Private ReadOnly s_dateTimeFormatUniqueCultures As New List(Of CultureInfo)
    Public ReadOnly s_fiveMinuteSpan As New TimeSpan(hours:=0, minutes:=5, seconds:=0)

    Private Function DoCultureSpecificParse(dateAsString As String, ByRef success As Boolean, defaultCulture As CultureInfo, styles As DateTimeStyles) As Date
        If s_dateTimeFormatUniqueCultures.Count = 0 Then
            s_dateTimeFormatUniqueCultures.Add(CurrentDateCulture)
            Dim fullDateTimeFormats As New List(Of String) From {
                CurrentDateCulture.DateTimeFormat.FullDateTimePattern
            }
            For Each oneCulture As CultureInfo In s_cultureInfos.ToList()
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

    <Extension>
    Private Function GmtToLocalTime(gmtDate As Date) As Date
        Dim dt As Date
        Dim dtUtc As Date
        dt = Date.Now
        dtUtc = Date.UtcNow
        Dim ts As TimeSpan = dtUtc.Subtract(dt)
        Return gmtDate.Add(ts)
    End Function

    <Extension>
    Friend Function GetCurrentDateCulture(countryCode As String) As CultureInfo
        Dim localDateCulture As List(Of CultureInfo) = s_cultureInfos.Where(Function(c As CultureInfo)
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
        If sgList(index).TryGetValue("previousDateTime", sgDateTimeString) Then
            sgDateTime = sgDateTimeString.ParseDate("previousDateTime")
        ElseIf sgList(index).TryGetValue("datetime", sgDateTimeString) Then
            sgDateTime = sgDateTimeString.ParseDate("datetime")
        ElseIf sgList(index).TryGetValue("dateTime", sgDateTimeString) Then
            sgDateTime = sgDateTimeString.ParseDate("dateTime")
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
    Public Function ParseDate(dateAsString As String, key As String, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As Date
        Dim resultDate As Date
        If dateAsString.TryParseDate(resultDate, key) Then
            Return resultDate
        End If

        MsgBox($"System.FormatException: String '{dateAsString}' in {memberName} line {sourceLineNumber} was not recognized as a valid DateTime in any supported culture.", MsgBoxStyle.ApplicationModal Or MsgBoxStyle.Critical)
        Throw New System.FormatException($"String '{dateAsString}' in {memberName} line {sourceLineNumber} was not recognized as a valid DateTime in any supported culture.")
    End Function

    <Extension>
    Public Function TryParseDate(dateAsString As String, ByRef resultDate As Date, key As String) As Boolean
        Dim success As Boolean
        Select Case key
            Case ""
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AssumeLocal)
            Case "previousDateTime"
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AssumeLocal)
            Case "sMedicalDeviceTime"
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AssumeUniversal).GmtToLocalTime
            Case "sLastSensorTime"
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AssumeUniversal).GmtToLocalTime
            Case "triggeredDateTime"
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AssumeLocal)
            Case "loginDateUTC"
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AssumeUniversal)
            Case "datetime"
                If key = "datetime" Then
                    ' "2022-07-31T22:56:00.000Z"
                    resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AssumeUniversal).GmtToLocalTime
                Else
                    resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AssumeLocal)
                End If
            Case "secondaryTime"
                resultDate = DoCultureSpecificParse(dateAsString, success, CurrentDateCulture, DateTimeStyles.AssumeLocal)
            Case Else
                Stop
        End Select

        Return success
    End Function

End Module
