''' Licensed to the .NET Foundation under one or more agreements.
''' The .NET Foundation licenses this file to you under the MIT license.
''' See the LICENSE file in the project root for more information.

Public Class Form1
    Public Client As CareLinkClient
    Public RecentData As Dictionary(Of String, String)

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        RecentData = Client.getRecentData()
        Timer1.Enabled = False

        CurrentBGToolStripTextBox.Text = UpdateDataTableWithSG(TableLayoutPanel1, RecentData)
    End Sub

    Private Sub LoginToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoginToolStripMenuItem.Click
        Timer1.Enabled = False

        Using loginDialog As New LoginForm1
            loginDialog.ShowDialog()
            Client = loginDialog.Client
        End Using

        RecentData = Client.getRecentData()
        If RecentData Is Nothing Then
            Exit Sub
        End If
        CurrentBGToolStripTextBox.Text = UpdateDataTableWithSG(TableLayoutPanel1, RecentData)
        Timer1.Interval = 50000
        Timer1.Enabled = True
    End Sub

    Private Shared Function UpdateDataTableWithSG(LayoutPanel1 As TableLayoutPanel, localRecentData As Dictionary(Of String, String)) As String
        Dim returnValue As String = ""
        LayoutPanel1.Controls.Clear()
        LayoutPanel1.RowCount = localRecentData.Count
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In localRecentData.WithIndex()
            Dim row As KeyValuePair(Of String, String) = c.Value
            LayoutPanel1.Controls.Add(New Label With {
                                              .Text = $"{c.Index} {row.Key}",
                                              .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                              .AutoSize = True
                                              }, 0, c.Index)
            LayoutPanel1.RowStyles(c.Index).SizeType = SizeType.AutoSize
            If row.Value.StartsWith("[") Then
                Dim innerJson As List(Of Dictionary(Of String, String)) = Json.LoadList(row.Value)
                If innerJson.Count > 0 Then
                    Dim arrayTable As New TableLayoutPanel With {
                            .AutoScroll = False,
                            .AutoSize = True,
                            .ColumnCount = 1,
                            .Dock = System.Windows.Forms.DockStyle.Fill,
                            .RowCount = innerJson(0).Count
                            }
                    If row.Key = "sgs" Then
                        arrayTable.ColumnCount = 2
                        Dim listTable As New List(Of KeyValuePair(Of String, String))
                        For Each entry As Dictionary(Of String, String) In innerJson
                            If entry("sensorState") <> "NO_ERROR_MESSAGE" Then
                                listTable.Add(New KeyValuePair(Of String, String)(entry("datetime"), entry("sensorState")))
                            End If
                        Next
                        For i As Integer = 0 To listTable.Count - 1
                            arrayTable.RowStyles.Add(New RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
                            arrayTable.Controls.Add(New Label With {
                                                       .Text = listTable(i).Key,
                                                       .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                       .AutoSize = True
                                                       }, 0, i)

                            arrayTable.Controls.Add(New TextBox With {
                                                       .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                       .AutoSize = True,
                                                       .Text = listTable(i).Value}, 1, i)
                        Next
                        LayoutPanel1.Controls.Add(arrayTable)
                    Else
                        For Each Dic As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                            arrayTable.Controls.Add(GetInnerTable(Dic.Value), 0, Dic.Index)
                        Next

                    End If
                    LayoutPanel1.Controls.Add(arrayTable)
                Else
                    LayoutPanel1.Controls.Add(New TextBox With {
                                                      .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                      .AutoSize = True,
                                                      .Text = row.Value}, 1, c.Index)

                End If
            ElseIf row.Value.StartsWith("{") Then
                LayoutPanel1.RowStyles(c.Index).SizeType = SizeType.AutoSize
                Dim innerJson As Dictionary(Of String, String) = Json.Loads(row.Value)
                If row.Key = "lastSG" Then
                    returnValue = innerJson("sg")
                End If
                LayoutPanel1.Controls.Add(GetInnerTable(innerJson), 1, c.Index)
            Else
                LayoutPanel1.Controls.Add(New TextBox With {
                                                  .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                  .AutoSize = True,
                                                  .Text = row.Value}, 1, c.Index)
            End If
        Next
        Return returnValue
    End Function

    Private Shared Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Shared Function GetInnerTable(innerJson As Dictionary(Of String, String)) As TableLayoutPanel
        Dim innerTable As New TableLayoutPanel With {
                .AutoScroll = False,
                .AutoSize = True,
                .ColumnCount = 2,
                .Dock = System.Windows.Forms.DockStyle.Fill,
                .Name = "InnerTable",
                .RowCount = innerJson.Count
                }
        innerTable.AutoSize = False
        innerTable.ColumnStyles.Add(New ColumnStyle())
        innerTable.ColumnStyles.Add(New ColumnStyle())
        ' ReSharper disable once RedundantAssignment
        For i As Integer = 0 To innerJson.Count - 1
            innerTable.RowStyles.Add(New RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
        Next
        Dim tempSize As Size = innerTable.Size
        innerTable.Size = New Size(tempSize.Width, (innerJson.Count * 22) + 8)
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In innerJson.WithIndex()
            Dim innerRow As KeyValuePair(Of String, String) = c.Value
            innerTable.Controls.Add(New Label With {
                                       .Text = innerRow.Key,
                                       .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                       .AutoSize = True
                                       }, 0, c.Index)

            If innerRow.Value.StartsWith("[") Then
                Dim innerJson1 As List(Of Dictionary(Of String, String)) = Json.LoadList(innerRow.Value)
                If innerJson1.Count > 0 Then
                    Dim arrayTable As New TableLayoutPanel With {
                            .AutoScroll = False,
                            .AutoSize = True,
                            .ColumnCount = 1,
                            .Dock = System.Windows.Forms.DockStyle.Fill,
                            .RowCount = innerJson1(0).Count
                            }

                    arrayTable.ColumnCount = 2
                    Dim listTable As New List(Of KeyValuePair(Of String, String))
                    For Each dic As Dictionary(Of String, String) In innerJson1
                        For Each entry As KeyValuePair(Of String, String) In dic
                            listTable.Add(New KeyValuePair(Of String, String)(entry.Key, entry.Value))
                        Next
                    Next
                    For i As Integer = 0 To listTable.Count - 1
                        arrayTable.RowStyles.Add(New RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
                        arrayTable.Controls.Add(New Label With {
                                                   .Text = listTable(i).Key,
                                                   .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                   .AutoSize = True
                                                   }, 0, i)

                        arrayTable.Controls.Add(New TextBox With {
                                                   .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                   .AutoSize = True,
                                                   .Text = listTable(i).Value}, 1, i)
                    Next
                    innerTable.Controls.Add(arrayTable, 1, c.Index)
                End If
            Else
                innerTable.Controls.Add(New TextBox With {
                                           .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                           .AutoSize = True,
                                           .Text = innerRow.Value}, 1, c.Index)
            End If
        Next
        Return innerTable
    End Function
End Class
