' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module TimeExtensions

    Public ReadOnly s_fiveMinuteSpan As New TimeSpan(hours:=0, minutes:=5, seconds:=0)
    Public ReadOnly s_thirtySecondInMilliseconds As Integer = CInt(New TimeSpan(0, 0, seconds:=30).TotalMilliseconds)

    <Extension>
    Friend Function FormatTimeOnly(rawTime As String, format As String) As String
        Return New TimeOnly(CInt(rawTime.Substring(0, 2)), CInt(rawTime.Substring(3, 2))).ToString(format)
    End Function

    <Extension>
    Friend Function SafeGetSgDateTime(sgList As List(Of Dictionary(Of String, String)), index As Integer, currentDataCulture As Globalization.CultureInfo) As Date
        Dim sgDateTimeString As String = ""
        Dim sgDateTime As Date
        If sgList(index).Count < 7 Then
            index -= 1
        End If
        If sgList(index).TryGetValue("previousDateTime", sgDateTimeString) Then
            sgDateTime = Date.Parse(sgDateTimeString, currentDataCulture)
        ElseIf sgList(index).TryGetValue("datetime", sgDateTimeString) Then
            sgDateTime = Date.Parse(sgDateTimeString, currentDataCulture)
        ElseIf sgList(index).TryGetValue("dateTime", sgDateTimeString) Then
            sgDateTime = Date.Parse(sgDateTimeString.Split("-")(0), currentDataCulture)
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
