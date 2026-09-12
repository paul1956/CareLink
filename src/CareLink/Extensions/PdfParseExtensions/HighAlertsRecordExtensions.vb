' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module HighAlertsRecordExtensions

    <Extension>
    Public Sub InitializeFromStringTable(this As HighAlertsRecord, sTable As StringTable, listOfAllTextLines As List(Of String))
        If sTable Is Nothing Then Return
        Try
            this.HighAlert(index:=0).SnoozeTime = OneHourSpan.ToString
            PdfSettingsRecord.GetSnoozeInfo(listOfAllTextLines,
                                            target:="High Alerts",
                                            this.HighAlert(index:=0).SnoozeOn,
                                            this.HighAlert(index:=0).SnoozeTime)
            Dim valueUnits As String = EmptyString
            For Each e As IndexClass(Of StringTable.Row) In sTable.Rows.WithIndex
                Dim row As StringTable.Row = e.Value
                If e.IsFirst Then
                    Dim oldValue As String = "Start High Time ("
                    valueUnits = row.Columns(index:=0).Replace(oldValue, newValue:=EmptyString).Trim(trimChar:=")"c)
                    Continue For
                End If

                Const options As StringSplitOptions = StringSplitOptions.RemoveEmptyEntries
                Dim value As String = sTable.Rows(index:=e.Index + 1).Columns(index:=0)
                Dim item As New HighAlertRecord()
                item.InitializeFromRow(row, valueUnits)
                item.End = If(e.IsLast OrElse IsNullOrWhiteSpace(value),
                              Midnight.ToString,
                              sTable.Rows(index:=e.Index + 1) _
                                                 .Columns(index:=0) _
                                                 .Split(separator:=" ", options)(0)).ToString
                If item.IsValid Then
                    this.HighAlert.Add(item)
                Else
                    Exit For
                End If

            Next
        Catch
        End Try
    End Sub

End Module
