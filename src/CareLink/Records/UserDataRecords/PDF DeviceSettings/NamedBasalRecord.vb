' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports Spire.Pdf.Utilities

Public Class NamedBasalRecord

    Public Sub New(tables As List(Of PdfTable), i As Integer, isActive As Boolean)
        Dim sTable As StringTable
        Dim tableNumber As Integer
        If i >= 1 AndAlso i <= 3 Then
            sTable = ConvertPdfTableToStringTable(tables(i), "24-Hour")
            Me.Total24Hour = sTable.GetSingleLineValue(Of String)("24-Hour Total")
            tableNumber = i + 9
        Else
            tableNumber = i
        End If

        Me.Active = isActive

        sTable = ConvertPdfTableToStringTable(tables(tableNumber), Me.GetColumnTitle)
        For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
            If e.IsFirst Then Continue For
            Dim line As String = e.Value.Columns(0)
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
    Public Property Total24Hour As String
    Public Property basalRates As New List(Of BasalRateRecord)

    Private Function GetColumnTitle() As String
        Return Me.ColumnTitles.ToArray.JoinLines(" ")
    End Function

End Class
