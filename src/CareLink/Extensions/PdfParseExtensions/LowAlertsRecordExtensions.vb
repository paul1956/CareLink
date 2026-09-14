' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module LowAlertsRecordExtensions

    Private ReadOnly Property Options As StringSplitOptions =
        StringSplitOptions.RemoveEmptyEntries

    <Extension>
    Public Sub InitializeFromStringTable(this As LowAlertsRecord, sTable As StringTable, listOfAllTextLines As List(Of String))
        If sTable Is Nothing Then Return
        Try
            Dim snoozeTime As String =
                New TimeSpan(hours:=0,
                             minutes:=20,
                             seconds:=0).ToString()
            PdfSettingsRecord.GetSnoozeInfo(listOfAllTextLines,
                                            target:="Low Alerts",
                                            this.SnoozeOn,
                                            snoozeTime)

            Dim valueUnits As String = EmptyString
            For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
                Dim s As StringTable.Row = e.Value
                If e.IsFirst Then
                    valueUnits =
                        s.Columns(index:=0) _
                         .Replace(oldValue:="Start Low Time (", newValue:=EmptyString) _
                         .Trim(trimChar:=")"c)
                    Continue For
                End If

                Dim value As String = sTable.Rows(index:=e.Index + 1).Columns(index:=0)
                Dim endTimeOnly As TimeOnly =
                    If(e.IsLast OrElse IsNullOrWhiteSpace(value),
                       Midnight,
                       TimeOnly.Parse(
                        s:=sTable.Rows(index:=e.Index + 1) _
                                 .Columns(index:=0) _
                                 .Split(separator:=" ", Options)(0)))

                Dim item As New LowAlertRecord()
                item.InitializeFromRow(row:=s, valueUnits)
                item.End = endTimeOnly.ToString()
                If item.IsValid Then
                    this.LowAlert.Add(item)
                Else
                    Exit For
                End If
            Next
        Catch
        End Try
    End Sub

End Module
