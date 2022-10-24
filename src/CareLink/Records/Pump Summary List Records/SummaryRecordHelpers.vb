' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module SummaryRecordHelpers

    Private Sub DataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim caption As String = CType(dgv.DataSource, DataTable).Columns(e.Column.Index).Caption
        e.DgvColumnAdded(GetCellStyle(e.Column.Name),
                        False,
                        True,
                        caption)
    End Sub
    Friend Function GetSummaryRecords(dic As Dictionary(Of String, String), Optional rowsToHide As List(Of String) = Nothing) As List(Of SummaryRecord)
        Dim summaryList As New List(Of SummaryRecord)
        For Each row As KeyValuePair(Of String, String) In dic
            If row.Value Is Nothing OrElse (rowsToHide IsNot Nothing AndAlso rowsToHide.Contains(row.Key)) Then
                Continue For
            End If

            Select Case row.Key
                Case "messageId"
                    Dim item As New SummaryRecord(row, summaryList.Count + 1)
                    Dim message As String = ""
                    If s_NotificationMessages.TryGetValue(row.Value, message) Then
                        item.Message = TranslateNotificationMessageId(dic, row.Value)
                    Else
                        If Not String.IsNullOrWhiteSpace(row.Value) AndAlso Debugger.IsAttached Then
                            MsgBox($"{row.Value} is unknown system status message", MsgBoxStyle.OkOnly, $"Form 1 line:{New StackFrame(0, True).GetFileLineNumber()}")
                        End If
                    End If
                    summaryList.Add(item)
                Case "autoModeReadinessState"
                    summaryList.Add(New SummaryRecord(row, s_autoModeReadinessMessages, NameOf(s_autoModeReadinessMessages), summaryList.Count + 1))
                Case "autoModeShieldState"
                    summaryList.Add(New SummaryRecord(row, s_autoModeShieldMessages, NameOf(s_autoModeShieldMessages), summaryList.Count + 1))
                Case "plgmLgsState"
                    summaryList.Add(New SummaryRecord(row, s_plgmLgsMessages, NameOf(s_plgmLgsMessages), summaryList.Count + 1))
                Case Else
                    If row.Value.IsPossibleMessage Then
                        summaryList.Add(New SummaryRecord(row, summaryList.Count + 1))
                    Else
                        summaryList.Add(New SummaryRecord(row, summaryList.Count + 1))
                    End If
            End Select
        Next
        Return summaryList
    End Function

    <Extension>
    Friend Function GetValue(l As List(Of SummaryRecord), Key As String) As String
        For Each s As SummaryRecord In l
            If s.Key = Key Then
                Return s.Value
            End If
        Next
        Throw New ArgumentException("Key not found", NameOf(Key))
    End Function

    Public Sub AttachHandlers(dgv As DataGridView)
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
    End Sub

    Public Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf(SummaryRecord.RecordNumber)
                cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleCenter, New Padding(1))
            Case NameOf(SummaryRecord.Key),
                 NameOf(SummaryRecord.Message)
                cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1))
            Case NameOf(SummaryRecord.Value)
                cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1))
            Case Else
                Throw UnreachableException()
        End Select
        Return cellStyle
    End Function

End Module
