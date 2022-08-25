' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class supportedReportsRecord
    Public Reports As New List(Of supportedReportRecord)

    Public Sub New(jsonList As String)
        Dim l As List(Of Dictionary(Of String, String)) = LoadList(jsonList)
        For Each dic As Dictionary(Of String, String) In l
            Reports.Add(New supportedReportRecord(dic))
        Next
    End Sub
End Class
