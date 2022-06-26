' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports CareLink.Form1

Public Module JsonExtensions

    Friend Function GetTranslatedMessageIdOrValue(dic As Dictionary(Of String, String), entry As KeyValuePair(Of String, String), TimeFormat As String) As String
        If entry.Key <> "messageid" Then
            Return entry.Value
        End If
        Dim messageValue As String = ""
        If _messages.TryGetValue(entry.Value, messageValue) Then
            Return $"{entry.Value} = {messageValue}"
        End If
        If _messagesSpecialHandling.TryGetValue(entry.Value, messageValue) Then
            Dim splitMessageValue As String() = messageValue.Split(":")
            Dim key As String = splitMessageValue(1)
            Dim replacementValue As String
            If key = "secondaryTime" Then
                replacementValue = dic(key).FormatTimeOnly(TimeFormat)
            Else
                replacementValue = dic(key)
            End If

            Return $"{entry.Value} = {splitMessageValue(0).Replace("(0)", replacementValue)}"
        End If

        If Debugger.IsAttached Then
            MsgBox($"Unknown sensor message '{entry.Value}'", MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Unknown Sensor Message")
        End If

        Return entry.Value.Replace("_", " ")
    End Function

    Friend Sub GetInnerTable(innerJson As Dictionary(Of String, String), tableLevel1Blue As TableLayoutPanel, itemIndex As ItemIndexs, filterJsonData As Boolean, timeFormat As String)
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle())
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle())
        tableLevel1Blue.BackColor = Color.LightBlue
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In innerJson.WithIndex()
            Application.DoEvents()
            Dim innerRow As KeyValuePair(Of String, String) = c.Value
            ' Comment out 4 lines below to see all data fields.
            ' I did not see any use to display the filtered out ones
            If filterJsonData AndAlso s_zFilterList.ContainsKey(itemIndex) Then
                If s_zFilterList(itemIndex).Contains(innerRow.Key) Then
                    Continue For
                End If
            End If
            If innerRow.Key <> "activeNotifications" Then
                tableLevel1Blue.RowStyles.Add(New RowStyle(SizeType.Absolute, 22))
            Else
                tableLevel1Blue.RowStyles.Add(New RowStyle(SizeType.AutoSize))
            End If
            tableLevel1Blue.RowCount += 1
            If itemIndex = ItemIndexs.limits OrElse itemIndex = ItemIndexs.markers OrElse (itemIndex = ItemIndexs.notificationHistory AndAlso c.Value.Key = "activeNotifications") Then
                tableLevel1Blue.AutoSize = True
            End If
            Dim keyLabel As New Label With {.Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                            .Text = innerRow.Key,
                                            .AutoSize = True
                                           }

            If innerRow.Value.StartsWith("[") Then
                Dim innerJson1 As List(Of Dictionary(Of String, String)) = LoadList(innerRow.Value)
                If innerJson1.Count > 0 Then
                    Dim tableLevel2 As New TableLayoutPanel With {
                            .AutoScroll = False,
                            .AutoSize = True,
                            .BorderStyle = BorderStyle.Fixed3D,
                            .BackColor = Color.Aqua,
                            .ColumnCount = 1,
                            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                            .Dock = DockStyle.Top,
                            .RowCount = innerJson1.Count
                            }

                    For i As Integer = 0 To innerJson1.Count - 1
                        tableLevel2.RowStyles.Add(New RowStyle() With {.SizeType = SizeType.AutoSize})
                    Next
                    tableLevel2.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 80.0))
                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson1.WithIndex()
                        Dim dic As Dictionary(Of String, String) = innerDictionary.Value
                        tableLevel2.RowStyles.Add(New RowStyle(SizeType.Absolute, 4 + (dic.Keys.Count * 22)))
                        Dim tableLevel3 As New TableLayoutPanel With {
                                .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                .AutoScroll = False,
                                .AutoSize = True,
                                .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                                .BorderStyle = BorderStyle.FixedSingle,
                                .ColumnCount = 2,
                                .Dock = DockStyle.Top
                                }
                        For Each e As IndexClass(Of KeyValuePair(Of String, String)) In dic.WithIndex()
                            If filterJsonData AndAlso s_zFilterList.ContainsKey(itemIndex) Then
                                If s_zFilterList(itemIndex).Contains(e.Value.Key) Then
                                    Continue For
                                End If
                            End If
                            tableLevel3.RowCount += 1
                            tableLevel3.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0))
                            tableLevel3.Controls.AddRange({New Label With {.Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                                                   .Text = e.Value.Key,
                                                                                   .AutoSize = True},
                                                                                   New TextBox With {
                                                                                        .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                                                        .AutoSize = True,
                                                                                        .[ReadOnly] = True,
                                                                                        .Text = GetTranslatedMessageIdOrValue(dic, e.Value, timeFormat)}})
                            Application.DoEvents()
                        Next
                        tableLevel3.Height += 40
                        tableLevel2.Controls.Add(tableLevel3, 0, innerDictionary.Index)
                        tableLevel2.Height += 4
                        Application.DoEvents()
                    Next
                    tableLevel1Blue.Controls.AddRange({keyLabel, tableLevel2})
                Else
                    tableLevel1Blue.Controls.AddRange({keyLabel,
                                                               New TextBox With {
                                                                   .Text = "",
                                                                   .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                                   .AutoSize = True,
                                                                   .[ReadOnly] = True
                                                                   }
                                                               })
                End If
            Else
                Dim textBox As New TextBox With {.[ReadOnly] = True,
                                                 .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                 .AutoSize = True,
                                                 .Text = GetTranslatedMessageIdOrValue(innerJson, innerRow, timeFormat)
                                                }
                tableLevel1Blue.Controls.AddRange({keyLabel,
                                                           textBox
                                                           })
            End If
        Next

        If itemIndex = ItemIndexs.lastSG Then
            tableLevel1Blue.AutoSize = False
            tableLevel1Blue.RowCount += 1
            tableLevel1Blue.Width = 400
        ElseIf itemIndex = ItemIndexs.lastAlarm Then
            Dim parentTableLayoutPanel As TableLayoutPanel = CType(tableLevel1Blue.Parent, TableLayoutPanel)
            parentTableLayoutPanel.AutoSize = False
            tableLevel1Blue.Dock = DockStyle.Top
            Application.DoEvents()
            tableLevel1Blue.ColumnStyles(1).SizeType = SizeType.Absolute

            If tableLevel1Blue.RowCount > 5 Then
                With parentTableLayoutPanel
                    .AutoScroll = True
                    .Width = 850
                End With
            Else
                parentTableLayoutPanel.Width = 870
                tableLevel1Blue.AutoScroll = False
            End If
            tableLevel1Blue.AutoSize = False
            tableLevel1Blue.RowCount += 1
            tableLevel1Blue.Height = (22 * tableLevel1Blue.RowCount) + 4
            Application.DoEvents()
        ElseIf itemIndex = ItemIndexs.notificationHistory Then
            tableLevel1Blue.RowStyles(1).SizeType = SizeType.AutoSize
        End If
        Application.DoEvents()
    End Sub

    <Extension>
    Private Function ItemAsString(item As KeyValuePair(Of String, Object)) As String
        Dim itemValue As JsonElement = CType(item.Value, JsonElement)
        Dim valueAsString As String = itemValue.ToString
        Select Case itemValue.ValueKind
            Case JsonValueKind.False
                Return "False"
            Case JsonValueKind.Null
                Return ""
            Case JsonValueKind.Number
                Return valueAsString
            Case JsonValueKind.True
                Return "True"
            Case JsonValueKind.String
                Try
                    If Char.IsDigit(valueAsString(0)) Then
                        Dim dateSplit As String() = valueAsString.Split("T")
                        If dateSplit.Length = 2 Then
                            Dim zDateString As String() = dateSplit(0).Split("-"c)
                            Dim zTimeString As String() = dateSplit(1).TrimEnd("Z"c).Replace("+", ".").Split(":")
                            Select Case item.Key
                                Case "techHours"
                                Case "lastConduitDateTime",
                                     "medicalDeviceTimeAsString",
                                     "previousDateTime" ' "2021-05-17T01:02:22.307-07:00"
                                    Return $"{ New DateTime(CInt(zDateString(0)),
                                                             CInt(zDateString(1)),
                                                             CInt(zDateString(2)),
                                                             CInt(zTimeString(0)),
                                                             CInt(zTimeString(1)),
                                                             CInt(zTimeString(2).Substring(0, 2)),
                                                             CInt(zTimeString(2).Substring(3, 3)), DateTimeKind.Local)}{ _
                                                    valueAsString.Substring(valueAsString.Length - 6)}"
                                Case "lastSensorTSAsString",
                                    "sLastSensorTime",
                                    "sMedicalDeviceTime",
                                    "triggeredDateTime" '2021-05-16T20:28:00.000Z
                                    Return New DateTime(CInt(zDateString(0)), CInt(zDateString(1)), CInt(zDateString(2)), CInt(zTimeString(0)), CInt(zTimeString(1)), CInt(zTimeString(2).Substring(0, 2)), DateTimeKind.Local).ToString()
                                Case "loginDateUTC" ' UTC 2021-05-16T20:28:00.000Z
                                    Return New DateTime(CInt(zDateString(0)), CInt(zDateString(1)), CInt(zDateString(2)), CInt(zTimeString(0)), CInt(zTimeString(1)), CInt(zTimeString(2).Substring(0, 2)), DateTimeKind.Utc).ToString()
                                Case "datetime"
                                    If item.Value.ToString().EndsWith("Z"c) Then
                                        Return New DateTime(CInt(zDateString(0)), CInt(zDateString(1)), CInt(zDateString(2)), CInt(zTimeString(0)), CInt(zTimeString(1)), CInt(zTimeString(2).Substring(0, 2)), DateTimeKind.Local).ToString()
                                    End If
                                    ' "2021-05-17T01:02:22.307-07:00"
                                    Return $"{ New DateTime(CInt(zDateString(0)),
                                                             CInt(zDateString(1)),
                                                             CInt(zDateString(2)),
                                                             CInt(zTimeString(0)),
                                                             CInt(zTimeString(1)),
                                                             CInt(zTimeString(2).Substring(0, 2)),
                                                             CInt(zTimeString(2).Substring(3, 3)), DateTimeKind.Local)}{ _
                                                    valueAsString.Substring(valueAsString.Length - 6)}"

                                Case Else
                                    Stop
                            End Select

                        End If
                    End If
                Catch ex As Exception
                    Stop
                End Try
        End Select
        Return valueAsString
    End Function

    Public Function LoadList(value As String) As List(Of Dictionary(Of String, String))
        Dim resultDictionaryArray As New List(Of Dictionary(Of String, String))
        Dim options As New JsonSerializerOptions() With {
                .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                .NumberHandling = JsonNumberHandling.WriteAsString}

        Dim deserializeList As List(Of Dictionary(Of String, Object)) = JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, Object)))(value, options)
        For Each deserializeItem As Dictionary(Of String, Object) In deserializeList
            Dim resultDictionary As New Dictionary(Of String, String)
            For Each item As KeyValuePair(Of String, Object) In deserializeItem
                If item.Value Is Nothing Then
                    resultDictionary.Add(item.Key, Nothing)
                Else
                    resultDictionary.Add(item.Key, item.ItemAsString)
                End If
            Next
            resultDictionaryArray.Add(resultDictionary)
        Next
        Return resultDictionaryArray
    End Function

    Public Function Loads(value As String) As Dictionary(Of String, String)
        Dim resultDictionary As New Dictionary(Of String, String)
        Dim options As New JsonSerializerOptions() With {
                .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                .NumberHandling = JsonNumberHandling.WriteAsString}

        For Each item As KeyValuePair(Of String, Object) In JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(value, options).ToList()
            If item.Value Is Nothing Then
                resultDictionary.Add(item.Key, Nothing)
            Else
                resultDictionary.Add(item.Key, item.ItemAsString)
            End If
        Next
        Return resultDictionary
    End Function

    <Extension>
    Public Function UtcToLocalTime(utcTime As String) As String
        Dim convertedDate As Date = Date.SpecifyKind(Date.Parse(utcTime), DateTimeKind.Utc)
        Return convertedDate.ToLocalTime.ToString
    End Function

End Module
