﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class supportedReportRecord

    Sub New(Values As Dictionary(Of String, String), recordnumber As Integer)
        If Values.Count <> 3 Then
            Throw New Exception($"{NameOf(supportedReportRecord)}({Values}) contains {Values.Count} entries, 3 expected.")
        End If
        Me.recordNumber = recordnumber
        Me.report = Values(NameOf(report))
        Me.onlyFor = kvpToString(LoadList(Values(NameOf(onlyFor)))).ToString.TrimStart(" "c).TrimEnd(",")
        Me.notFor = kvpToString(LoadList(Values(NameOf(notFor)))).ToString.TrimStart(" "c).TrimEnd(",")

    End Sub

#Region "MUST BE ON TOP"

    Public Property recordNumber As Integer

#End Region ' End MUST BE ON TOP

#Region "MUST BE SECOND"

    Public Property report As String

#End Region ' End MUST BE  SECOND

    Public Property notFor As String
    Public Property onlyFor As String

    Private Shared Function kvpToString(forList As List(Of Dictionary(Of String, String))) As StringBuilder
        Dim sb As New StringBuilder
        For Each dic As Dictionary(Of String, String) In forList
            For Each kvp As KeyValuePair(Of String, String) In dic
                sb.Append($" {kvp.Key}={kvp.Value},")
            Next
        Next

        Return sb
    End Function

    Private Function GetDebuggerDisplay() As String
        Return Me.report.ToString()
    End Function

End Class