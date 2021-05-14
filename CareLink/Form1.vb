''' Licensed to the .NET Foundation under one or more agreements.
''' The .NET Foundation licenses this file to you under the MIT license.
''' See the LICENSE file in the project root for more information.

Public Class Form1
    Public Client As CareLinkClient
    Public RecentData As Dictionary(Of String, String)

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
    End Sub

    Private Sub LoginToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoginToolStripMenuItem.Click
        Using loginDialog As New LoginForm1
            loginDialog.ShowDialog()
            Client = loginDialog.Client
        End Using

        RecentData = Client.getRecentData()
        TableLayoutPanel1.RowCount = RecentData.Count
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In RecentData.WithIndex()
            Dim row As KeyValuePair(Of String, String) = c.Value
            TableLayoutPanel1.Controls.Add(New Label With {
                                              .Text = $"{c.Index} {row.Key}",
                                              .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                              .AutoSize = True
                                              }, 0, c.Index)
            If row.Value.StartsWith("[") Then
                TableLayoutPanel1.RowStyles(c.Index).SizeType = SizeType.AutoSize
                Dim innerJson As List(Of Dictionary(Of String, String)) = Json.LoadList(row.Value)
                If innerJson.Count > 0 Then
                    Dim arrayTable As New TableLayoutPanel With {
                            .AutoScroll = False,
                            .AutoSize = True,
                            .ColumnCount = 1,
                            .Dock = System.Windows.Forms.DockStyle.Fill,
                            .Name = "FiveMinuteTable",
                            .RowCount = innerJson(0).Count
                            }
                    For Each Dic As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        arrayTable.Controls.Add(GetInnerTable(Dic.Value), 0, Dic.Index)
                    Next

                    TableLayoutPanel1.Controls.Add(arrayTable)
                Else
                    TableLayoutPanel1.Controls.Add(New TextBox With {
                                                      .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                      .AutoSize = True,
                                                      .Text = row.Value}, 1, c.Index)

                End If
            ElseIf row.Value.StartsWith("{") Then
                TableLayoutPanel1.RowStyles(c.Index).SizeType = SizeType.AutoSize
                TableLayoutPanel1.Controls.Add(GetInnerTable(Json.Loads(row.Value)), 1, c.Index)
            Else
                TableLayoutPanel1.Controls.Add(New TextBox With {
                                                  .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                  .AutoSize = True,
                                                  .Text = row.Value}, 1, c.Index)
            End If
        Next
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
        For Each innerKvp As IndexClass(Of KeyValuePair(Of String, String)) In innerJson.WithIndex()
            Dim innerRow As KeyValuePair(Of String, String) = innerKvp.Value
            innerTable.Controls.Add(New Label With {
                                       .Text = innerRow.Key,
                                       .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                       .AutoSize = True
                                       }, 0, innerKvp.Index)

            innerTable.Controls.Add(New TextBox With {
                                       .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                       .AutoSize = True,
                                       .Text = innerRow.Value}, 1, innerKvp.Index)
        Next
        Return innerTable
    End Function
End Class
