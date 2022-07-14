' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices

Public Module SGListExtensions
    <Extension()>
    Friend Function ToSgList(innerJson As List(Of Dictionary(Of String, String)), currentDataCulture As IFormatProvider) As List(Of SgRecord)
        Dim sGs As New List(Of SgRecord)
        Dim lastValidTime As Date
        Dim firstValidTime As Integer
        For firstValidTime = 0 To innerJson.Count - 1
            Dim dateTimeString As String = Nothing
            If innerJson(firstValidTime).TryGetValue("datetime", dateTimeString) Then
                lastValidTime = dateTimeString.DateParse - (firstValidTime * s_fiveMinuteSpan)
                Exit For
            End If
        Next
        For i As Integer = 0 To innerJson.Count - 1

            Dim sgItem As New SgRecord(innerJson, i, lastValidTime, currentDataCulture)
            If sgItem.sg = 0 Then
                sgItem.sg = Single.NaN
            End If
            sGs.Add(sgItem)
        Next
        Return sGs
    End Function

End Module
