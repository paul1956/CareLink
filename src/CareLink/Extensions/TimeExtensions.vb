' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices

Friend Module TimeExtensions

    Public ReadOnly s_fiveMinuteSpan As New TimeSpan(hours:=0, minutes:=5, seconds:=0)

    <Extension>
    Public Function DateParse(dateAsString As String, currentDataCulture As IFormatProvider, CurrentUICulture As IFormatProvider,
                              <CallerMemberName> Optional memberName As String = Nothing,
                              <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As Date
        Dim resultDate As Date
        If Date.TryParse(dateAsString, currentDataCulture, DateTimeStyles.None, resultDate) Then
            Return resultDate
        End If
        If Date.TryParse(dateAsString, CurrentUICulture, DateTimeStyles.None, resultDate) Then
            Return resultDate
        End If
        MsgBox($"System.FormatException: String '{dateAsString}' in {memberName} line {sourceLineNumber} was not recognized as a valid DateTime", MsgBoxStyle.ApplicationModal Or MsgBoxStyle.Critical)
        End
    End Function

    <Extension>
    Friend Function SafeGetSgDateTime(sgList As List(Of Dictionary(Of String, String)), index As Integer, currentDataCulture As CultureInfo, currentUICulture As CultureInfo) As Date
        Dim sgDateTimeString As String = ""
        Dim sgDateTime As Date
        If sgList(index).Count < 7 Then
            index -= 1
        End If
        If sgList(index).TryGetValue("previousDateTime", sgDateTimeString) Then
            sgDateTime = sgDateTimeString.DateParse(currentDataCulture, currentUICulture)
        ElseIf sgList(index).TryGetValue("datetime", sgDateTimeString) Then
            sgDateTime = sgDateTimeString.DateParse(currentDataCulture, currentUICulture)
        ElseIf sgList(index).TryGetValue("dateTime", sgDateTimeString) Then
            sgDateTime = sgDateTimeString.Split("-")(0).DateParse(currentDataCulture, currentUICulture)
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
