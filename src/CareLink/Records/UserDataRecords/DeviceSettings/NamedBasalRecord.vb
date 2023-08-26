' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports Spire.Pdf.Utilities

Public Class NamedBasalRecord

    Public Sub New(tables As List(Of PdfTable), i As Integer, allText As String, name As String)
        Dim lines As List(Of String)
        Dim tableNumber As Integer
        If i >= 1 AndAlso i <= 3 Then
            lines = ExtractPdfTableLines(tables(i), "24-Hour")
            Me.Total24Hour = lines.GetSingleLineValue(Of Single)("Total ")
            tableNumber = i + 9
        Else
            tableNumber = i
        End If

        Dim index As Integer = allText.IndexOf(name)
        Me.Active = allText.Substring(index + name.Length + 2, 1) = "("c

        lines = ExtractPdfTableLines(tables(tableNumber), Me.GetColumnTitle)
        Me.basalRates.Clear()
        For Each e As IndexClass(Of String) In lines.WithIndex
            If e.IsFirst Then Continue For
            Dim line As String = e.Value
            Dim item As New BasalRateRecord(line)
            If Not item.IsValid Then Exit For
            Me.basalRates.Add(item)
        Next
    End Sub

    Public Sub New()
    End Sub

    Private ReadOnly Property ColumnTitles() As New List(Of String) From {
                    {"Time"},
                    {"U/Hr"}
                }

    Public Property Active As Boolean
    Public Property Total24Hour As Single
    Public Property basalRates As New List(Of BasalRateRecord)

    Private Function GetColumnTitle() As String
        Return Me.ColumnTitles.ToArray.JoinLines(" ")
    End Function

End Class
