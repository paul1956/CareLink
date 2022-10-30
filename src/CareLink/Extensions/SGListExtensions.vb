' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module SGListExtensions

    <Extension()>
    Friend Function ToSgList(innerJson As List(Of Dictionary(Of String, String))) As List(Of SgRecord)
        Dim sGs As New List(Of SgRecord)
        Dim lastValidTime As Date = Now - New TimeSpan(24, 0, 0)
        For firstValidTimeIndex As Integer = 0 To innerJson.Count - 1
            Dim dateTimeString As String = Nothing
            If innerJson(firstValidTimeIndex).TryGetValue(NameOf(SgRecord.datetime), dateTimeString) Then
                lastValidTime = dateTimeString.ParseDate(NameOf(SgRecord.datetime)) - (firstValidTimeIndex * s_fiveMinuteSpan)
                Exit For
            End If
        Next
        For i As Integer = 0 To innerJson.Count - 1

            Dim sgItem As New SgRecord(innerJson, i, lastValidTime)
            sGs.Add(sgItem)
        Next
        Return sGs
    End Function

End Module
