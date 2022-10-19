' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module LastAlarmListHelpers
    Friend Sub GetLastAlarmSummaryRecords(dic As Dictionary(Of String, String))
        s_listOfLastAlarmSummaryRecords.Clear()
        For Each row As KeyValuePair(Of String, String) In dic
            If row.Value Is Nothing Then Continue For
            Select Case row.Key
                Case NameOf(LastAlarmRecord.messageId)
                    s_listOfLastAlarmSummaryRecords.Add(New LastAlarmSummary(row, s_listOfLastAlarmSummaryRecords.Count + 1))
                    Dim message As String = ""
                    If s_NotificationMessages.TryGetValue(row.Value, message) Then
                        s_listOfLastAlarmSummaryRecords.Add(New LastAlarmSummary(KeyValuePair.Create(NameOf(message), TranslateNotificationMessageId(dic, row.Value)), s_listOfLastAlarmSummaryRecords.Count + 1))
                    Else
                        If Not String.IsNullOrWhiteSpace(row.Value) AndAlso Debugger.IsAttached Then
                            MsgBox($"{row.Value} is unknown system status message", MsgBoxStyle.OkOnly, $"Form 1 line:{New StackFrame(0, True).GetFileLineNumber()}")
                        End If
                        s_listOfLastAlarmSummaryRecords.Add(New LastAlarmSummary(row, s_listOfLastAlarmSummaryRecords.Count + 1))
                    End If

                Case Else
                    s_listOfLastAlarmSummaryRecords.Add(New LastAlarmSummary(row, s_listOfLastAlarmSummaryRecords.Count + 1))
            End Select
        Next

    End Sub
End Module
