' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports CareLink.Form1

Public Module JsonExtensions

    Private Function CreateValueTextBox(dic As Dictionary(Of String, String), eValue As KeyValuePair(Of String, String), timeFormat As String, isScaledForm As Boolean) As TextBox
        Dim valueTextBox As New TextBox With {.Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                              .AutoSize = True,
                                              .ReadOnly = True
                                             }

        If eValue.Key.Equals("messageId", StringComparison.OrdinalIgnoreCase) Then
            valueTextBox.Text = TranslateMessageId(dic, eValue.Value, timeFormat)
            AddHandler valueTextBox.Click, AddressOf MessageIdTextBox_Click
        Else
            Dim result As Single = Nothing
            If Single.TryParse(eValue.Value, NumberStyles.Number, CurrentDataCulture, result) Then
                valueTextBox.Text = result.ToString(CurrentUICulture)
            ElseIf eValue.Value IsNot Nothing Then
                valueTextBox.Text = eValue.Value.ToString(CurrentUICulture)
            Else
                valueTextBox.Text = ""
            End If

            If eValue.Key = "sg" AndAlso isScaledForm Then
                Dim timeOfLastSGString As String = ""
                If Not (dic.TryGetValue("datetime", timeOfLastSGString) OrElse
                        dic.TryGetValue("dateTime", timeOfLastSGString)) Then
                    Dim sb As New StringBuilder
                    For Each item As KeyValuePair(Of String, String) In dic
                        sb.AppendLine($"{item.Key} = {item.Value}")
                    Next
                    MsgBox($"Could not find datetime or dateTime in dictionary{Environment.NewLine}{sb}")
                    timeOfLastSGString = Now.ToString(CurrentDateCulture)
                End If
                valueTextBox.Text &= $"     @ {timeOfLastSGString}"
            End If
        End If

        Return valueTextBox
    End Function

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
        End Select
        Return valueAsString
    End Function

    Private Sub MessageIdTextBox_Click(sender As Object, e As EventArgs)
        Dim textBox1 As TextBox = CType(sender, TextBox)
        Dim selectionStart As Integer = textBox1.SelectionStart
        Dim openParen As Integer = textBox1.Text.IndexOf("(")
        If selectionStart < openParen Then
            textBox1.Select(0, openParen)
        ElseIf selectionStart > openParen Then
            textBox1.Select(openParen + 1, textBox1.TextLength - (openParen + 2))
        End If
    End Sub

    Friend Sub GetInnerTable(innerJson As Dictionary(Of String, String), tableLevel1Blue As TableLayoutPanel, itemIndex As ItemIndexs, filterJsonData As Boolean, timeFormat As String, isScaledForm As Boolean)
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle())
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle())
        tableLevel1Blue.BackColor = Color.LightBlue
        Dim messageOrDefault As KeyValuePair(Of String, String) = innerJson.Where(Function(kvp As KeyValuePair(Of String, String)) kvp.Key = "messageId").FirstOrDefault
        If itemIndex = ItemIndexs.lastAlarm AndAlso messageOrDefault.Key IsNot Nothing Then
            tableLevel1Blue.RowStyles.Add(New RowStyle(SizeType.Absolute, 22))
            Dim keyLabel As New Label With {.Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                            .Text = "messageId",
                                            .AutoSize = True
                                           }

            tableLevel1Blue.RowCount += 1
            Dim textBox1 As TextBox = CreateValueTextBox(innerJson, messageOrDefault, timeFormat, isScaledForm)

            If textBox1.Text.Length > 100 Then
                Form1.ToolTip1.SetToolTip(textBox1, textBox1.Text)
            Else
                Form1.ToolTip1.SetToolTip(textBox1, Nothing)
            End If
            tableLevel1Blue.Controls.AddRange({keyLabel,
                                                       textBox1
                                                      }
                                             )
        End If

        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In innerJson.WithIndex()
            Application.DoEvents()
            Dim innerRow As KeyValuePair(Of String, String) = c.Value
            ' Comment out 4 lines below to see all data fields.
            ' I did not see any use to display the filtered out ones
            If filterJsonData AndAlso s_zFilterList.ContainsKey(itemIndex) AndAlso innerJson.Count > 4 Then
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
                Dim innerJson1 As List(Of Dictionary(Of String, String)) = LoadList(innerRow.Value, False)
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
                            Dim eValue As KeyValuePair(Of String, String) = e.Value

                            If filterJsonData AndAlso s_zFilterList.ContainsKey(itemIndex) Then
                                If s_zFilterList(itemIndex).Contains(eValue.Key) Then
                                    Continue For
                                End If
                            End If
                            tableLevel3.RowCount += 1
                            tableLevel3.RowStyles.Add(New RowStyle(SizeType.Absolute, 22.0))

                            Dim valueLabel As New Label With {.Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                              .Text = eValue.Key,
                                                              .AutoSize = True}
                            tableLevel3.Controls.AddRange({valueLabel, CreateValueTextBox(dic, eValue, timeFormat, isScaledForm)})
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
                                                                   .ReadOnly = True
                                                                   }
                                                               })
                End If
            Else
                If innerRow.Key <> "messageId" Then
                    Dim textBox1 As TextBox = CreateValueTextBox(innerJson, innerRow, timeFormat, isScaledForm)
                    Form1.ToolTip1.SetToolTip(textBox1, textBox1.Text)
                    tableLevel1Blue.Controls.AddRange({keyLabel, textBox1})
                End If
            End If
        Next

        If itemIndex = ItemIndexs.lastSG Then
            tableLevel1Blue.AutoSize = False
            tableLevel1Blue.RowCount += 1
            tableLevel1Blue.Width = 400
        ElseIf itemIndex = ItemIndexs.lastAlarm Then
            Dim parentTableLayoutPanel As TableLayoutPanel = CType(tableLevel1Blue.Parent, TableLayoutPanel)
            parentTableLayoutPanel.AutoSize = False
            tableLevel1Blue.Dock = DockStyle.Fill
            Application.DoEvents()
            tableLevel1Blue.ColumnStyles(1).SizeType = SizeType.Absolute
            If tableLevel1Blue.RowCount > 7 Then
                parentTableLayoutPanel.AutoScroll = True
            Else
                parentTableLayoutPanel.Width = 870
                tableLevel1Blue.AutoScroll = False
            End If
            Dim tableLevel1BlueWidth As Integer = tableLevel1Blue.Width
            tableLevel1Blue.AutoSize = False
            tableLevel1Blue.RowCount += 1
            tableLevel1Blue.Height = 22 * (tableLevel1Blue.RowCount - 1)
            tableLevel1Blue.Dock = DockStyle.None
            Application.DoEvents()
            tableLevel1Blue.Width = tableLevel1BlueWidth - 30
            Application.DoEvents()
            tableLevel1Blue.Dock = DockStyle.Fill
            Application.DoEvents()
        ElseIf itemIndex = ItemIndexs.notificationHistory Then
            tableLevel1Blue.RowStyles(1).SizeType = SizeType.AutoSize
        End If
        Application.DoEvents()
    End Sub

    <Extension>
    Public Function CleanUserData(cleanRecentData As Dictionary(Of String, String)) As String
        cleanRecentData("firstName") = "First"
        cleanRecentData("lastName") = "Last"
        cleanRecentData("medicalDeviceSerialNumber") = "NG1234567H"
        Return JsonSerializer.Serialize(cleanRecentData, New JsonSerializerOptions)
    End Function

    Public Function LoadList(value As String, isSG As Boolean) As List(Of Dictionary(Of String, String))
        Dim resultDictionaryArray As New List(Of Dictionary(Of String, String))
        Dim options As New JsonSerializerOptions() With {
                .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                .NumberHandling = JsonNumberHandling.WriteAsString}

        For Each e As IndexClass(Of Dictionary(Of String, Object)) In JsonSerializer.Deserialize(Of List(Of Dictionary(Of String, Object)))(value, options).WithIndex
            Dim resultDictionary As New Dictionary(Of String, String)
            For Each item As KeyValuePair(Of String, Object) In e.Value
                If item.Value Is Nothing Then
                    resultDictionary.Add(item.Key, Nothing)
                Else
                    resultDictionary.Add(item.Key, item.ItemAsString)
                End If
                If Not isSG Then Continue For
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

End Module
