' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports Spire.Pdf.Utilities

Public Class NamedBasalRecord

    Public Sub New(table As PdfTable, isActive As Boolean)
        Dim sTable As StringTable = table.PdfTableToStringTable(tableHeader:="24-Hour")
        Me.Total24Hour = sTable.GetSingleLineValue(Of String)(key:="24-Hour Total")
        Me.Active = isActive
    End Sub

    Public Sub New()
    End Sub

    Public Property Active As Boolean = False
    Public Property basalRates As New List(Of BasalRateRecord)
    Public Property Total24Hour As String = EmptyString

    Public Sub UpdateBasalRates(sTable As StringTable)
        For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
            If e.IsFirst Then
                Continue For
            End If
            Dim value As String = e.Value.Columns(index:=0)
            Dim item As New BasalRateRecord(value)
            If Not item.IsValid Then Exit For
            Me.basalRates.Add(item)
        Next
    End Sub

End Class
