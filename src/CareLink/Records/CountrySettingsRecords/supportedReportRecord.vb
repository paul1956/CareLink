' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text

<DebuggerDisplay("{GetDebuggerDisplay(),nq}")>
Public Class supportedReportRecord
    Public Sub New(Values As Dictionary(Of String, String), recordnumber As Integer)
        If Values.Count <> 3 Then
            Throw New Exception($"{NameOf(supportedReportRecord)}({Values}) contains {Values.Count} entries, 3 expected.")
        End If
        Me.recordNumber = recordnumber
        Me.report = Values(NameOf(report))
        Me.onlyFor = kvpToString(LoadList(Values(NameOf(onlyFor)))).ToString.TrimStart(" "c).TrimEnd(",")
        Me.notFor = kvpToString(LoadList(Values(NameOf(notFor)))).ToString.TrimStart(" "c).TrimEnd(",")

    End Sub

#If True Then ' Prevent reordering
    Public Property recordNumber As Integer
    Public Property report As String
    Public Property onlyFor As String
    Public Property notFor As String

#End If  ' Prevent reordering

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
