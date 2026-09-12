' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports Spire.Pdf.Utilities

Public Module NamedBasalRecordExtensions

    <Extension>
    Public Sub InitializeFromPdfTable(this As NamedBasalRecord, table As PdfTable, isActive As Boolean)
        If table Is Nothing Then
            Return
        End If
        Try
            Dim sTable As StringTable = table.PdfTableToStringTable(tableHeader:="24-Hour")
            this.Total24Hour = sTable.GetSingleLineValue(Of String)(key:="24-Hour", endsWith:="Total")
            this.Active = isActive
            ' Populate BasalRates
            For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
                If e.IsFirst Then Continue For
                Dim value As String = e.Value.Columns(index:=0)
                Dim item As New BasalRateRecord()
                item.InitializeFromString(value)
                If Not item.IsValid Then Exit For
                this.BasalRates.Add(item)
            Next
        Catch
        End Try
    End Sub

End Module
