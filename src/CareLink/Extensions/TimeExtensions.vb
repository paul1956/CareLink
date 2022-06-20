' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Module TimeExtensions

    <Extension>
    Friend Function FormatTimeOnly(rawTime As String, format As String) As String
        Return New TimeOnly(CInt(rawTime.Substring(0, 2)), CInt(rawTime.Substring(3, 2))).ToString(format)
    End Function

    <Extension>
    Friend Function GetMilitaryHour(selectedStartTime As String) As Integer
        Return CInt(Format(Date.Parse(selectedStartTime), "HH"))
    End Function

    <Extension>
    Friend Function SafeGetSgDateTime(sgList As List(Of Dictionary(Of String, String)), index As Integer) As Date
        Dim sgDateTimeString As String = ""
        Dim sgDateTime As Date
        If sgList(index).Count < 7 Then
            index -= 1
        End If
        If sgList(index).TryGetValue("previousDateTime", sgDateTimeString) Then
            sgDateTime = Date.Parse(sgDateTimeString)
        ElseIf sgList(index).TryGetValue("datetime", sgDateTimeString) Then
            sgDateTime = Date.Parse(sgDateTimeString)
        ElseIf sgList(index).TryGetValue("dateTime", sgDateTimeString) Then
            sgDateTime = Date.Parse(sgDateTimeString.Split("-")(0))
        Else
            sgDateTime = Now
        End If
        If sgDateTime.Year = 2000 Then
            sgDateTime = Date.Now - ((sgList.Count - index) * Form1._FiveMinuteSpan)
        End If
        If sgList(index).Count < 7 Then
            sgDateTime = sgDateTime.AddMinutes(5)
        End If
        Return sgDateTime
    End Function

End Module
