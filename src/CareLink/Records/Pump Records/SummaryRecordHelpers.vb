' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module SummaryRecordHelpers

    Private s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Private Sub DataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim caption As String = CType(dgv.DataSource, DataTable).Columns(e.Column.Index).Caption
        e.DgvColumnAdded(GetCellStyle(e.Column.Name),
                        False,
                        True,
                        caption)
    End Sub

    <Extension>
    Private Function FormatTimeOnly(rawTime As String, format As String) As String
        Return New TimeOnly(CInt(rawTime.Substring(0, 2)), CInt(rawTime.Substring(3, 2))).ToString(format)
    End Function

    Private Function TranslateNotificationMessageId(jsonDictionary As Dictionary(Of String, String), entryValue As String) As String
        Dim formattedMessage As String = ""
        Try
            If s_NotificationMessages.TryGetValue(entryValue, formattedMessage) Then
                Dim splitMessageValue As String() = formattedMessage.Split(":")
                Dim key As String = ""
                Dim replacementValue As String = ""
                If splitMessageValue.Length > 1 Then
                    key = splitMessageValue(1)
                    If key = "lastSetChange" Then
                        replacementValue = s_oneToNineteen(CInt(jsonDictionary(key))).ToTitle
                    Else
                        replacementValue = jsonDictionary(key)
                        Dim resultDate As Date
                        If replacementValue.TryParseDate(resultDate, key) Then
                            replacementValue = resultDate.ToString
                        End If
                    End If
                End If

                Dim secondaryTime As String = If(jsonDictionary.ContainsKey(NameOf(ClearedNotificationsRecord.secondaryTime)), jsonDictionary(NameOf(ClearedNotificationsRecord.secondaryTime)).FormatTimeOnly(s_timeWithMinuteFormat), "")
                Dim triggeredDateTime As String = ""
                If jsonDictionary.ContainsKey(NameOf(ClearedNotificationsRecord.triggeredDateTime)) Then
                    triggeredDateTime = $" {jsonDictionary(NameOf(ClearedNotificationsRecord.triggeredDateTime)).ParseDate("triggeredDateTime")}"
                ElseIf jsonDictionary.ContainsKey(NameOf(SgRecord.datetime)) Then
                    triggeredDateTime = $" {jsonDictionary(NameOf(SgRecord.datetime)).ParseDate(NameOf(SgRecord.datetime))}"
                ElseIf jsonDictionary.ContainsKey(NameOf(TimeChangeRecord.dateTime)) Then
                    triggeredDateTime = $" {jsonDictionary(NameOf(TimeChangeRecord.dateTime)).ParseDate("dateTime")}"
                Else
                    Stop
                End If

                formattedMessage = splitMessageValue(0) _
                    .Replace("(0)", replacementValue) _
                    .Replace("(triggeredDateTime)", $", happened at {triggeredDateTime}") _
                    .Replace("(CriticalLow)", s_criticalLow.ToString(CurrentUICulture)) _
                    .Replace("(units)", BgUnitsString) _
                    .Replace($"(secondaryTime)", secondaryTime)
            Else
                If Debugger.IsAttached Then
                    MsgBox($"Unknown sensor message '{entryValue}'", MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Unknown Sensor Message")
                End If
                formattedMessage = entryValue.Replace("_", " ")
            End If
        Catch ex As Exception
            Stop
        End Try
        Return formattedMessage
    End Function

    Friend Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToCoumnAlignment(Of SummaryRecord)(s_alignmentTable, columnName)
    End Function

    Friend Function GetSummaryRecords(dic As Dictionary(Of String, String), Optional rowsToHide As List(Of String) = Nothing) As List(Of SummaryRecord)
        Dim summaryList As New List(Of SummaryRecord)
        For Each row As KeyValuePair(Of String, String) In dic
            If row.Value Is Nothing OrElse (rowsToHide IsNot Nothing AndAlso rowsToHide.Contains(row.Key)) Then
                Continue For
            End If

            Select Case row.Key
                Case "messageId"
                    Dim message As String = ""
                    If s_NotificationMessages.TryGetValue(row.Value, message) Then
                        message = TranslateNotificationMessageId(dic, row.Value)
                    Else
                        If Not String.IsNullOrWhiteSpace(row.Value) AndAlso Debugger.IsAttached Then
                            MsgBox($"{row.Value} is unknown system status message", MsgBoxStyle.OkOnly, $"Form 1 line:{New StackFrame(0, True).GetFileLineNumber()}")
                        End If
                        message = row.Value.ToTitle
                    End If
                    summaryList.Add(New SummaryRecord(summaryList.Count + 1, row, message))
                Case "autoModeReadinessState"
                    summaryList.Add(New SummaryRecord(summaryList.Count + 1, row, s_sensorMessages, NameOf(s_sensorMessages)))
                Case "autoModeShieldState"
                    summaryList.Add(New SummaryRecord(summaryList.Count + 1, row, s_autoModeShieldMessages, NameOf(s_autoModeShieldMessages)))
                Case "plgmLgsState"
                    summaryList.Add(New SummaryRecord(summaryList.Count + 1, row, s_plgmLgsMessages, NameOf(s_plgmLgsMessages)))
                Case Else
                    summaryList.Add(New SummaryRecord(summaryList.Count + 1, row))
            End Select
        Next
        Return summaryList

    End Function


    Public Sub AttachHandlers(dgv As DataGridView)
        AddHandler dgv.ColumnAdded, AddressOf DataGridView_ColumnAdded
    End Sub

    Public Function GetTabIndexFromName(tabPageName As String) As Integer
        Return CInt(tabPageName.Replace(NameOf(TabPage), "").Substring(0, 2)) - 1
    End Function

    Public Function CAnyType(Of T)(UTO As Object) As T
        Return CType(UTO, T)
    End Function

    <Extension>
    Public Function GetValue(Of T)(l As List(Of SummaryRecord), Key As String, Optional throwError As Boolean = True) As T

        Try
            For Each s As SummaryRecord In l
                If s.Key = Key Then
                    Return CAnyType(Of T)(s.Value)
                End If
            Next
            If throwError Then
                Stop
                Throw New ArgumentException("Key not found", NameOf(Key))
            End If
        Catch ex As Exception

        End Try

        Return Nothing
    End Function
End Module
