''' Licensed to the .NET Foundation under one or more agreements.
''' The .NET Foundation licenses this file to you under the MIT license.
''' See the LICENSE file in the project root for more information.

Public Class Form1
    Private ReadOnly loginDialog As New LoginForm1
    Public Client As CareLinkClient
    Public RecentData As Dictionary(Of String, String)

    Private Shared Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Shared Function GetInnerTable(innerJson As Dictionary(Of String, String)) As TableLayoutPanel
        Dim tableLevel1Blue As New TableLayoutPanel With {
                .AutoScroll = True,
                .AutoSize = True,
                .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                .ColumnCount = 2,
                .Dock = System.Windows.Forms.DockStyle.Fill,
                .Name = "InnerTable",
                .RowCount = innerJson.Count
                }
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle())
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle())
        ' ReSharper disable once RedundantAssignment
        For i As Integer = 0 To innerJson.Count - 1
            tableLevel1Blue.RowStyles.Add(New RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Next
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In innerJson.WithIndex()
            Dim innerRow As KeyValuePair(Of String, String) = c.Value
            tableLevel1Blue.Controls.Add(New Label With {
                                       .Text = innerRow.Key,
                                       .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                       .AutoSize = True
                                       }, 0, c.Index)

            If innerRow.Value.StartsWith("[") Then
                Dim innerJson1 As List(Of Dictionary(Of String, String)) = Json.LoadList(innerRow.Value)
                If innerJson1.Count > 0 Then
                    Dim tableLevel2Pink As New TableLayoutPanel With {
                            .AutoScroll = True,
                            .AutoSize = True,
                            .BackColor = Color.DeepPink,
                            .ColumnCount = 1,
                            .Dock = System.Windows.Forms.DockStyle.Fill,
                            .RowCount = innerJson1.Count
                            }

                    For Each innerDictionary As IndexClass(Of Dictionary(Of String, String)) In innerJson1.WithIndex()
                        tableLevel2Pink.RowStyles.Add(New RowStyle(System.Windows.Forms.SizeType.AutoSize))
                        Dim dic As Dictionary(Of String, String) = innerDictionary.Value
                        Dim tableLevel3Orange As New TableLayoutPanel With {
                                .AutoScroll = True,
                                .AutoSize = True,
                                .BackColor = Color.Orange,
                                .ColumnCount = 2,
                                .Dock = System.Windows.Forms.DockStyle.Fill,
                                .RowCount = dic.Keys.Count
                                }
                        For Each e As IndexClass(Of KeyValuePair(Of String, String)) In dic.WithIndex()
                            tableLevel3Orange.RowStyles.Add(New RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0))
                            tableLevel3Orange.Controls.Add(New Label With {
                                                       .Text = e.Value.Key,
                                                       .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                       .AutoSize = True
                                                       }, 0, e.Index)

                            tableLevel3Orange.Controls.Add(New TextBox With {
                                                       .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                       .AutoSize = True,
                                                       .Text = e.Value.Value}, 1, e.Index)
                            Application.DoEvents()
                        Next
                        tableLevel2Pink.Controls.Add(tableLevel3Orange, 0, innerDictionary.Index)
                        Application.DoEvents()
                    Next
                    tableLevel1Blue.Controls.Add(tableLevel2Pink, 1, c.Index)
                    Application.DoEvents()
                Else
                    tableLevel1Blue.Controls.Add(New TextBox With {
                                               .Text = "",
                                               .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                               .AutoSize = True
                                               }, 1, c.Index)

                End If
            Else
                tableLevel1Blue.Controls.Add(New TextBox With {
                                           .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                           .AutoSize = True,
                                           .Text = innerRow.Value}, 1, c.Index)
            End If
        Next
        Application.DoEvents()
        Return tableLevel1Blue
    End Function

    Private Function UpdateDataTableWithSG(LayoutPanel1 As TableLayoutPanel, localRecentData As Dictionary(Of String, String), tabPageIndex As Integer) As String
        Dim returnValue As String = CurrentBGToolStripTextBox.Text
        Dim EndIndex As Integer
        Dim StartIndex As Integer
        LayoutPanel1.Controls.Clear()
        LayoutPanel1.AutoSize = True
        Select Case tabPageIndex
            Case 1
                EndIndex = 38
                StartIndex = 42
                LayoutPanel1.RowCount = localRecentData.Count - 6
            Case 2 ' Active Insulin
                EndIndex = 38
                StartIndex = 38
                LayoutPanel1.RowCount = 1
            Case 3 ' SGS
                EndIndex = 39
                StartIndex = 39
                LayoutPanel1.RowCount = 1
            Case 4 ' Limits
                EndIndex = 40
                StartIndex = 40
                LayoutPanel1.RowCount = 1
            Case 5 ' Markers
                EndIndex = 41
                StartIndex = 41
                LayoutPanel1.RowCount = 1
            Case 6 ' Notification History
                EndIndex = 42
                StartIndex = 42
                LayoutPanel1.RowCount = 1
            Case 7 ' Basal
                EndIndex = 45
                StartIndex = 45
                LayoutPanel1.RowCount = 1
        End Select
        Dim singleItem As Boolean = EndIndex = StartIndex
        LayoutPanel1.ColumnCount = If(singleItem, 1, 2)
        Dim currentRowIndex As Integer = 0
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In localRecentData.WithIndex()
            If (Not (c.Index < EndIndex OrElse c.Index > StartIndex) OrElse c.Index = 45) OrElse singleItem Then
                If Not (singleItem AndAlso EndIndex = c.Index) Then
                    Continue For
                End If
            End If
            LayoutPanel1.RowStyles(currentRowIndex).SizeType = SizeType.AutoSize
            Dim row As KeyValuePair(Of String, String) = c.Value
            If Not singleItem Then
                LayoutPanel1.Controls.Add(New Label With {
                                                  .Text = $"{c.Index} {row.Key}",
                                                  .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                  .AutoSize = True
                                                  }, 0, currentRowIndex)
            End If
            If row.Value.StartsWith("[") Then
                Dim innerJson As List(Of Dictionary(Of String, String)) = Json.LoadList(row.Value)
                If innerJson.Count > 0 Then
                    Dim arrayTable As New TableLayoutPanel With {
                            .AutoScroll = False,
                            .AutoSize = True,
                            .BackColor = Color.Yellow,
                            .CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble,
                            .ColumnCount = 1,
                            .Dock = System.Windows.Forms.DockStyle.Fill
                            }
                    If row.Key = "sgs" AndAlso Not singleItem Then
                        arrayTable.ColumnCount = 2
                        Dim listTable As New List(Of KeyValuePair(Of String, String))
                        For Each entry As Dictionary(Of String, String) In innerJson
                            Dim sensorState As String = ""
                            If entry.TryGetValue("sensorState", sensorState) Then
                                If sensorState <> "NO_ERROR_MESSAGE" Then
                                    listTable.Add(New KeyValuePair(Of String, String)(entry("datetime"), sensorState))
                                End If

                            End If
                        Next
                        arrayTable.RowCount = listTable.Count - 1
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
                                                      .Text = row.Value}, If(singleItem, 0, 1), currentRowIndex)

                End If
            ElseIf row.Value.StartsWith("{") Then
                LayoutPanel1.RowStyles(currentRowIndex).SizeType = SizeType.AutoSize
                Dim innerJson As Dictionary(Of String, String) = Json.Loads(row.Value)
                If row.Key = "lastSG" Then
                    returnValue = innerJson("sg")
                End If
                LayoutPanel1.Controls.Add(GetInnerTable(innerJson), If(singleItem, 0, 1), currentRowIndex)
            Else
                LayoutPanel1.Controls.Add(New TextBox With {
                                                  .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                  .AutoSize = True,
                                                  .Text = row.Value}, If(singleItem, 0, 1), currentRowIndex)
            End If
            currentRowIndex += 1
            If StartIndex = EndIndex Then
                Exit For
            End If
        Next
        Return returnValue
    End Function

    Private Sub LoginToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoginToolStripMenuItem.Click
        Timer1.Enabled = False

        loginDialog.ShowDialog()
        Client = loginDialog.Client

        RecentData = Client.getRecentData()
        If RecentData Is Nothing Then
            Exit Sub
        End If
        UpdateAllTabPages()
        Timer1.Interval = 300_000
        Timer1.Enabled = True
    End Sub

    Private Sub UpdateAllTabPages()
        For i As Integer = 1 To 7
            Dim tableLayout As TableLayoutPanel = CType(Me.Controls.Find($"TableLayoutPanel{i}", True).FirstOrDefault(), TableLayoutPanel)
            CurrentBGToolStripTextBox.Text = UpdateDataTableWithSG(tableLayout, RecentData, i)
        Next
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        RecentData = Client.getRecentData()
        Timer1.Enabled = True

        UpdateAllTabPages()

        Select Case CInt(CurrentBGToolStripTextBox.Text)
            Case < 60
                CurrentBGToolStripTextBox.BackColor = Color.Red
            Case < 70
                CurrentBGToolStripTextBox.BackColor = Color.Yellow
            Case > 180
                CurrentBGToolStripTextBox.BackColor = Color.Red
            Case > 150
                CurrentBGToolStripTextBox.BackColor = Color.Yellow
            Case Else
                CurrentBGToolStripTextBox.BackColor = Color.White
        End Select


        Application.DoEvents()
    End Sub

End Class
