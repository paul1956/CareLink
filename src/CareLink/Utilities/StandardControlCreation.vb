' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Text

Public Module StandardControlCreation

    ''' <summary>
    ''' Create a TextBox
    ''' .AutoSize = True
    ''' .Anchor Left and Right
    ''' .BorderStyle = BorderStyle.FixedSingle
    ''' .ReadOnly = True
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns>New TextBox</returns>
    Friend Function CreateBasicTextBox(text As String) As TextBox
        Return New TextBox With {
            .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
            .AutoSize = True,
            .BorderStyle = BorderStyle.FixedSingle,
            .ReadOnly = True,
            .Text = text
        }
    End Function

    Friend Function CreateTableLayoutPanel(tableName As String, rowCount As Integer, backColor As Color) As TableLayoutPanel
        Return New TableLayoutPanel With {
                .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                .AutoScroll = False,
                .AutoSize = True,
                .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                .BackColor = backColor,
                .BorderStyle = BorderStyle.Fixed3D,
                .ColumnCount = 2,
                .Dock = DockStyle.Top,
                .Margin = New Padding(0),
                .Name = tableName,
                .Padding = New Padding(0),
                .RowCount = rowCount
            }

    End Function

    Friend Function CreateValueTextBox(dic As Dictionary(Of String, String), eValue As KeyValuePair(Of String, String), timeFormat As String, isScaledForm As Boolean) As TextBox
        Dim valueTextBox As TextBox

        If eValue.Key.Equals("messageId", StringComparison.OrdinalIgnoreCase) Then
            valueTextBox = CreateBasicTextBox(TranslateMessageId(dic, eValue.Value, timeFormat))
        Else
            Dim result As Single = Nothing
            If Single.TryParse(eValue.Value, NumberStyles.Number, CurrentDataCulture, result) Then
                valueTextBox = CreateBasicTextBox(result.ToString(CurrentUICulture))
            ElseIf eValue.Value IsNot Nothing Then
                Dim resultDate As Date = Nothing
                If eValue.Value.TryParseDate(resultDate, eValue.Key) Then
                    If resultDate.Date = Date.MinValue Then
                        valueTextBox = CreateBasicTextBox(TimeOnly.FromDateTime(resultDate).ToString(CurrentUICulture))
                    Else
                        valueTextBox = CreateBasicTextBox(resultDate.ToString(CurrentUICulture))
                    End If
                Else
                    valueTextBox = CreateBasicTextBox(eValue.Value.ToString(CurrentUICulture))
                End If
            Else
                valueTextBox = CreateBasicTextBox("")
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
                valueTextBox.Text = timeOfLastSGString
            End If
        End If

        Return valueTextBox
    End Function

    Friend Function InitializeColumnLabel(layoutPanel1 As TableLayoutPanel, rowIndex As ItemIndexs) As Label
        Dim labelControl As Label = CreateBasicLabel($"{CInt(rowIndex)}{Environment.NewLine}{rowIndex}")
        layoutPanel1.Controls.Add(labelControl, 0, 0)
        Application.DoEvents()
        Return labelControl
    End Function

    Friend Sub InitializeWorkingPanel(ByRef layoutPanel1 As TableLayoutPanel, realPanel As TableLayoutPanel, Optional autoSize? As Boolean = Nothing)
        layoutPanel1 = realPanel
        layoutPanel1.Controls.Clear()
        layoutPanel1.RowCount = 1
        layoutPanel1.RowStyles(0).SizeType = SizeType.AutoSize
        If autoSize IsNot Nothing Then
            layoutPanel1.AutoSize = CBool(autoSize)
        End If
    End Sub

    ''' <summary>
    ''' Create a Label
    ''' .AutoSize = True
    ''' .Anchor Left and Right
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns>New Label</returns>
    Public Function CreateBasicLabel(text As String) As Label
        Return New Label With {.Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                               .AutoSize = True,
                               .Text = text}
    End Function

End Module
