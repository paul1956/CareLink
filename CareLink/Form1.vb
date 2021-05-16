''' Licensed to the .NET Foundation under one or more agreements.
''' The .NET Foundation licenses this file to you under the MIT license.
''' See the LICENSE file in the project root for more information.

Public Class Form1
    Const ActiveInsulin As Integer = 38
    Const Basal As Integer = 45
    Const LastAlarm As Integer = 37
    Const LastSG As Integer = 36
    Const Limits As Integer = 40
    Const Markers As Integer = 41
    Const NotificationHistory As Integer = 42
    Const SGS As Integer = 39

    Private Shared ReadOnly markersFilter As New List(Of String) From {
        "id",
        "index",
        "kind",
        "version",
        "relativeOffset"
        }

    Private Shared ReadOnly LastAlarmFilter As New List(Of String) From {
        "code",
        "instanceID",
        "kind",
        "referenceGUID",
        "relativeOffset",
        "version"
        }
    Private Shared ReadOnly LastSgFilter As New List(Of String) From {
        "kind",
        "version",
        "relativeOffset"
        }

    Private Shared ReadOnly sgsFilter As New List(Of String) From {
        "kind",
        "version",
        "relativeOffset"
        }
    Private Shared ReadOnly FilterList As New Dictionary(Of Integer, List(Of String)) From {
        {LastAlarm, LastAlarmFilter},
        {LastSG, LastSgFilter},
        {Markers, markersFilter},
        {SGS, sgsFilter}
        }


    Private ReadOnly listOfSingleItems As New List(Of Integer) From {
        LastSG,
        LastAlarm,
        ActiveInsulin,
        SGS,
        Limits,
        Markers,
        NotificationHistory,
        Basal
    }

    Private ReadOnly loginDialog As New LoginForm1
    Public Client As CareLinkClient
    Public RecentData As Dictionary(Of String, String)

    Private Shared Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

    Private Shared Sub GetInnerTable(tableLevel1Blue As TableLayoutPanel, innerJson As Dictionary(Of String, String), itemIndex As Integer)
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle())
        tableLevel1Blue.ColumnStyles.Add(New ColumnStyle())

        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In innerJson.WithIndex()
            Dim innerRow As KeyValuePair(Of String, String) = c.Value
            If FilterList.ContainsKey(itemIndex) Then
                If FilterList(itemIndex).Contains(innerRow.Key) Then
                    Continue For
                End If
            End If
            If itemIndex = SGS Then
                If sgsFilter.Contains(innerRow.Key) Then
                    Continue For
                End If
            End If
            tableLevel1Blue.RowCount += 1
            tableLevel1Blue.RowStyles.Add(New RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0))
            Dim label As Label = New Label With {
                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                    .Text = innerRow.Key,
                    .AutoSize = True
                    }

            If innerRow.Value.StartsWith("[") Then
                Dim innerJson1 As List(Of Dictionary(Of String, String)) = Json.LoadList(innerRow.Value)
                If innerJson1.Count > 0 Then
                    Dim tableLevel2Pink As New TableLayoutPanel With {
                            .AutoScroll = True,
                            .AutoSize = True,
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
                                .ColumnCount = 2,
                                .Dock = System.Windows.Forms.DockStyle.Fill
                                }
                        For Each e As IndexClass(Of KeyValuePair(Of String, String)) In dic.WithIndex()
                            tableLevel3Orange.RowStyles.Add(New RowStyle(System.Windows.Forms.SizeType.Absolute, 22.0))
                            tableLevel3Orange.RowCount += 1
                            tableLevel3Orange.Controls.AddRange({New Label With {
                                                                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                                    .Text = e.Value.Key,
                                                                    .AutoSize = True},
                                                                 New TextBox With {
                                                                    .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                                    .AutoSize = True,
                                                                     .Text = e.Value.Value}})
                            Application.DoEvents()
                        Next
                        tableLevel2Pink.Controls.Add(tableLevel3Orange, 0, innerDictionary.Index)
                        Application.DoEvents()
                    Next
                    tableLevel1Blue.Controls.AddRange({label, tableLevel2Pink})
                Else
                    tableLevel1Blue.Controls.AddRange({label, New TextBox With {
                                               .Text = "",
                                               .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                               .AutoSize = True
                                               }})
                End If
            Else
                tableLevel1Blue.Controls.AddRange({label, New TextBox With {
                                           .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                           .AutoSize = True,
                                           .Text = innerRow.Value}})
            End If
        Next
        If itemIndex = LastSG Then
            tableLevel1Blue.AutoSize = False
            tableLevel1Blue.RowCount += 2
            tableLevel1Blue.Width=400
        End If
        Application.DoEvents()
        Return
    End Sub

    Private Sub LoginToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoginToolStripMenuItem.Click
        Timer1.Enabled = False

        loginDialog.ShowDialog()
        Client = loginDialog.Client

        RecentData = Client.getRecentData()
        If RecentData Is Nothing Then
            Exit Sub
        End If
        UpdateAllTabPages()
        Timer1.Interval = CType(New TimeSpan(0, 5, 0).TotalMilliseconds, Integer)
        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        RecentData = Client.getRecentData()
        Timer1.Enabled = True

        UpdateAllTabPages()

        Select Case CInt($"0{CurrentBGToolStripTextBox.Text}")
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

    Private Sub UpdateAllTabPages()
        CurrentBGToolStripTextBox.Text = UpdateDataTableWithSG(RecentData)
    End Sub

    Private Function UpdateDataTableWithSG(localRecentData As Dictionary(Of String, String)) As String
        Dim returnValue As String = CurrentBGToolStripTextBox.Text
        TableLayoutPanel1.Controls.Clear()
        TableLayoutPanel1.RowCount = localRecentData.Count - 8
        Dim currentRowIndex As Integer = 0
        Dim singleItem As Boolean
        Dim LayoutPanel1 As TableLayoutPanel
        For Each c As IndexClass(Of KeyValuePair(Of String, String)) In localRecentData.WithIndex()
            Dim singleItemIndex As Integer = 0
            Select Case c.Index
                Case LastSG
                    LayoutPanel1 = TableLayoutPanelTop1
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case LastAlarm
                    LayoutPanel1 = TableLayoutPanelTop2
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case ActiveInsulin
                    LayoutPanel1 = TableLayoutPanel2
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case SGS
                    LayoutPanel1 = TableLayoutPanel3
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case Limits
                    LayoutPanel1 = TableLayoutPanel4
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case Markers
                    LayoutPanel1 = TableLayoutPanel5
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case NotificationHistory
                    LayoutPanel1 = TableLayoutPanel6
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case Basal
                    LayoutPanel1 = TableLayoutPanel7
                    LayoutPanel1.Controls.Clear()
                    singleItemIndex = c.Index
                    LayoutPanel1.RowCount = 1
                    singleItem = True
                Case Else
                    LayoutPanel1 = TableLayoutPanel1
                    singleItem = False
            End Select
            LayoutPanel1.AutoSize = True

            If listOfSingleItems.Contains(c.Index) OrElse singleItem Then
                If Not (singleItem AndAlso singleItemIndex = c.Index) Then
                    Continue For
                End If
            End If
            Dim tableRelitiveRow As Integer
            If singleItem Then
                tableRelitiveRow = 0
            Else
                tableRelitiveRow = currentRowIndex
                currentRowIndex += 1
            End If
            LayoutPanel1.RowStyles(tableRelitiveRow).SizeType = SizeType.AutoSize
            Dim row As KeyValuePair(Of String, String) = c.Value
            If Not singleItem Then
                LayoutPanel1.Controls.Add(New Label With {
                                                  .Text = $"{c.Index} {row.Key}",
                                                  .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                  .AutoSize = True
                                                  }, 0, tableRelitiveRow)
            End If
            If row.Value.StartsWith("[") Then
                Dim innerJson As List(Of Dictionary(Of String, String)) = Json.LoadList(row.Value)
                If innerJson.Count > 0 Then
                    For Each Dic As IndexClass(Of Dictionary(Of String, String)) In innerJson.WithIndex()
                        Dim tableLevel1Blue As New TableLayoutPanel With {
                                .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                .AutoScroll = False,
                                .AutoSize = True,
                                .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                                .ColumnCount = 2,
                                .Dock = System.Windows.Forms.DockStyle.Fill,
                                .Margin = New Padding(0),
                                .Name = "InnerTable",
                                .Padding = New Padding(0)
                                }
                        LayoutPanel1.Controls.Add(tableLevel1Blue, 1, Dic.Index)
                        GetInnerTable(tableLevel1Blue, Dic.Value, c.Index)
                    Next
                Else
                    LayoutPanel1.Controls.Add(New TextBox With {
                                                      .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                      .AutoSize = True,
                                                      .Text = row.Value}, If(singleItem, 0, 1), tableRelitiveRow)

                End If
            ElseIf row.Value.StartsWith("{") Then
                LayoutPanel1.RowStyles(tableRelitiveRow).SizeType = SizeType.AutoSize
                Dim innerJson As Dictionary(Of String, String) = Json.Loads(row.Value)
                If row.Key = "lastSG" Then
                    returnValue = innerJson("sg")
                End If
                Dim tableLevel1Blue As New TableLayoutPanel With {
                        .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                        .AutoScroll = False,
                        .AutoSize = True,
                        .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        .ColumnCount = 2,
                        .Dock = System.Windows.Forms.DockStyle.Fill,
                        .Margin = New Padding(0),
                        .Name = "InnerTable",
                        .Padding = New Padding(0)
                        }
                LayoutPanel1.Controls.Add(tableLevel1Blue, If(singleItem, 0, 1), tableRelitiveRow)
                GetInnerTable(tableLevel1Blue, innerJson, c.Index)
            Else
                LayoutPanel1.Controls.Add(New TextBox With {
                                                  .Anchor = AnchorStyles.Left Or AnchorStyles.Right,
                                                  .AutoSize = True,
                                                  .Text = row.Value}, If(singleItem, 0, 1), tableRelitiveRow)
            End If
            Application.DoEvents()
        Next
        Return returnValue
    End Function

End Class
