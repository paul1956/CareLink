' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices

Friend Module TimeExtensions

    Public ReadOnly s_fiveMinuteSpan As New TimeSpan(hours:=0, minutes:=5, seconds:=0)
    Private ReadOnly s_dateTimeFormatUniqueCultures As New List(Of CultureInfo)

    <Extension>
    Public Function DateParse(dateAsString As String, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As Date
        If s_dateTimeFormatUniqueCultures.Count = 0 Then
            s_dateTimeFormatUniqueCultures.Add(CurrentDataCulture)
            Dim dateFormats As New List(Of String) From {
            CurrentDataCulture.DateTimeFormat.LongDatePattern
        }
            Dim timeFormats As New List(Of String) From {
            CurrentDataCulture.DateTimeFormat.LongTimePattern
        }
            For Each oneCulture As CultureInfo In CultureInfo.GetCultures(CultureTypes.AllCultures).ToList()
                If timeFormats.Contains(oneCulture.DateTimeFormat.LongTimePattern) OrElse
                    dateFormats.Contains(oneCulture.DateTimeFormat.LongDatePattern) OrElse
                    String.IsNullOrWhiteSpace(oneCulture.Name) Then
                    Continue For
                End If
                s_dateTimeFormatUniqueCultures.Add(oneCulture)
                timeFormats.Add(oneCulture.DateTimeFormat.LongTimePattern)
                dateFormats.Add(oneCulture.DateTimeFormat.LongDatePattern)
            Next
        End If
        s_dateTimeFormatUniqueCultures.RemoveAt(0)
        Dim resultDate As Date
        If Date.TryParse(dateAsString, CurrentDataCulture, DateTimeStyles.None, resultDate) Then
            Return resultDate
        End If
        If CurrentDataCulture.Name <> CurrentUICulture.Name AndAlso
            Date.TryParse(dateAsString, CurrentUICulture, DateTimeStyles.None, resultDate) Then
            Return resultDate
        End If
        For Each c As CultureInfo In s_dateTimeFormatUniqueCultures
            If Date.TryParse(dateAsString, c, DateTimeStyles.None, resultDate) Then
                Return resultDate
            End If
        Next

        MsgBox($"System.FormatException: String '{dateAsString}' in {memberName} line {sourceLineNumber} was not recognized as a valid DateTime in any supported culture.", MsgBoxStyle.ApplicationModal Or MsgBoxStyle.Critical)
        End
    End Function

    <Extension>
    Friend Function SafeGetSgDateTime(sgList As List(Of Dictionary(Of String, String)), index As Integer) As Date
        Dim sgDateTimeString As String = ""
        Dim sgDateTime As Date
        If sgList(index).Count < 7 Then
            index -= 1
        End If
        If sgList(index).TryGetValue("previousDateTime", sgDateTimeString) Then
            sgDateTime = sgDateTimeString.DateParse()
        ElseIf sgList(index).TryGetValue("datetime", sgDateTimeString) Then
            sgDateTime = sgDateTimeString.DateParse()
        ElseIf sgList(index).TryGetValue("dateTime", sgDateTimeString) Then
            sgDateTime = sgDateTimeString.Split("-")(0).DateParse()
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

End Module
