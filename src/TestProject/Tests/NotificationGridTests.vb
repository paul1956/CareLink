' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Drawing
Imports System.IO
Imports System.Runtime.ExceptionServices
Imports System.Text.Json
Imports System.Threading
Imports System.Windows.Forms
Imports CareLink
Imports FluentAssertions
Imports Xunit

<Collection("Sequential")>
Public Class NotificationGridTests

    <Theory>
    <InlineData(True)>
    <InlineData(False)>
    Public Sub ClearedNotifications_SampleHasNoExcessSpaceBelowRows(visibleBeforeLoading As Boolean)
        Dim failure As Exception = Nothing
        Dim start As ThreadStart =
            Sub()
                Dim previousHistory As Dictionary(Of String, String) = s_notificationHistoryValue
                Dim previousPatient As PatientDataInfo = PatientData
                Try
                    Dim json As String = File.ReadAllText(path:=FileIoHelpers.GetTestDataPath())
                    Using document As JsonDocument = JsonDocument.Parse(json)
                        PatientData = New PatientDataInfo With {
                            .MedicalDeviceFamily = document.RootElement.GetProperty(propertyName:="medicalDeviceFamily").GetString()}
                        Dim history As JsonElement = document.RootElement.GetProperty(propertyName:="notificationHistory")
                        s_notificationHistoryValue = New Dictionary(Of String, String) From {
                            {"clearedNotifications", history.GetProperty(propertyName:="clearedNotifications").GetRawText()},
                            {"activeNotifications", "[]"}}
                    End Using

                    Using owner As New Form1()
                        Dim size As New Size(width:=1384, height:=800)
                        owner.ClientSize = size
                        owner.Show()
                        owner.DeviceDpi.Should().Be(expected:=96, because:="the application currently supports 96 DPI")
                        owner.ClientSize.Should().Be(expected:=size, because:="the test uses the supported fixed client size")
                        owner.TabControlPage1.Visible = True
                        If visibleBeforeLoading Then
                            owner.TabControlPage1.SelectedTab = owner.TabPage11NotificationsCleared
                            Application.DoEvents()
                        End If
                        For refresh As Integer = 1 To 2
                            owner.UpdateNotificationTabs()
                            owner.TabControlPage1.SelectedTab = owner.TabPage11NotificationsCleared
                            Application.DoEvents()
                            Dim panel As TableLayoutPanel = owner.TlpNotificationsCleared
                            Dim scenario As String = $"visibleBeforeLoading={visibleBeforeLoading}, refresh={refresh}"
                            panel.Visible.Should().BeTrue(because:=$"the cleared tab must be visible ({{0}})",
                                                          scenario)
                            Dim grids As DataGridView() = panel.Controls.OfType(Of DataGridView)().ToArray()
                            grids.Should().HaveCount(expected:=7,
                                                     because:=$"the sample has seven notifications ({{0}})",
                                                     scenario)
                            For Each grid As DataGridView In grids
                                Dim scrollY As Integer = Math.Max(0, grid.Top - panel.AutoScrollPosition.Y)
                                panel.AutoScrollPosition = New Point(x:=0, y:=scrollY)
                                Application.DoEvents()
                                Dim rowsHeight As Integer =
                                    grid.Rows.GetRowsHeight(includeFilter:=DataGridViewElementStates.Visible)
                                Dim rowIndex As Integer = panel.GetRow(control:=grid)
                                Dim lastRow As Integer =
                                    grid.Rows.GetLastRow(includeFilter:=DataGridViewElementStates.Visible)
                                Dim lastRowBounds As Rectangle =
                                    grid.GetRowDisplayRectangle(rowIndex:=lastRow, cutOverflow:=False)
                                Dim rowHeight As Integer = panel.GetRowHeights()(rowIndex)
                                Dim details As String =
                                    $"{scenario}, size={size}, row={rowIndex}, grid={grid.Bounds}, " &
                                    $"client={grid.ClientRectangle}, rowsHeight={rowsHeight}, lastRow={lastRowBounds}, " &
                                    $"panelRowHeight={rowHeight}, scroll={panel.AutoScrollPosition}"
                                lastRowBounds.Bottom.Should().BeLessThanOrEqualTo(
                                    expected:=grid.ClientRectangle.Bottom + 2,
                                    because:=$"the last row must not be clipped ({{0}})", details)
                                Dim gridGap As Integer = grid.ClientSize.Height - rowsHeight
                                gridGap.Should().BeInRange(minimumValue:=0,
                                                           maximumValue:=2,
                                                           because:="the grid must fit its visible rows without excess space ({0})",
                                                           details)
                                Dim rowGap As Integer = rowHeight - rowsHeight
                                rowGap.Should().BeInRange(minimumValue:=0,
                                                          maximumValue:=20,
                                                          because:=$"the panel row must fit the notification without excess space ({{0}})",
                                                          details)
                            Next
                            panel.VerticalScroll.Visible.Should().BeTrue(
                                because:=$"the seven notifications must be scrollable ({{0}}, size={{1}}, client={{2}})",
                                scenario, size, panel.ClientSize)
                            panel.AutoScrollPosition = New Point(x:=0, y:=Integer.MaxValue)
                            Application.DoEvents()
                            Dim lastGrid As DataGridView =
                                grids.OrderBy(
                                    keySelector:=Function(grid As DataGridView)
                                                     Return panel.GetRow(control:=grid)
                                                 End Function).Last()
                            Dim gap As Integer = panel.ClientSize.Height - lastGrid.Bottom
                            Dim bottomDetails As String =
                                $"{scenario}, size={size}, client={panel.ClientSize}, " &
                                $"display={panel.DisplayRectangle}, scroll={panel.AutoScrollPosition}, " &
                                $"last={lastGrid.Bounds}, rows={String.Join(separator:=",", values:=panel.GetRowHeights())}"
                            gap.Should().BeInRange(minimumValue:=0,
                                                   maximumValue:=20,
                                                   because:=$"the last grid must be visible without excess scroll space below it ({{0}})",
                                                   bottomDetails)
                        Next
                    End Using
                Catch ex As Exception
                    failure = ex
                Finally
                    s_notificationHistoryValue = previousHistory
                    PatientData = previousPatient
                End Try
            End Sub

        Dim thread As New Thread(start)
        thread.SetApartmentState(state:=ApartmentState.STA)
        thread.Start()
        thread.Join()
        If failure IsNot Nothing Then
            ExceptionDispatchInfo.Capture(source:=failure).Throw()
        End If
    End Sub

    <Theory>
    <InlineData(True)>
    <InlineData(False)>
    Public Sub NotificationBinding_ReleasesCursorWithoutWaitingForHiddenTabs(visibleBeforeLoading As Boolean)
        Dim failure As Exception = Nothing
        Dim start As ThreadStart =
            Sub()
                Dim previousHistory As Dictionary(Of String, String) = s_notificationHistoryValue
                Try
                    s_notificationHistoryValue = New Dictionary(Of String, String) From {
                        {"clearedNotifications", "[{""type"":""sample"",""value"":""one""}]"},
                        {"activeNotifications", "[{""type"":""sample"",""value"":""active""}]"}}
                    Using owner As New Form1(), otherGrid As New DataGridView()
                        owner.Show()
                        If visibleBeforeLoading Then
                            owner.TabControlPage1.SelectedTab = owner.TabPage11NotificationsCleared
                        End If
                        Application.DoEvents()
                        For refresh As Integer = 1 To 2
                            owner.UpdateNotificationTabs()
                            Assert.Equal(expected:=0,
                                         actual:=GetPendingBindingsCount(owner))
                            Assert.Equal(expected:=Cursors.Default,
                                         actual:=owner.Cursor)
                        Next

                        BindingStarted(owner, dgv:=otherGrid)
                        Try
                            owner.UpdateNotificationTabs()
                            Assert.Equal(expected:=1,
                                         actual:=GetPendingBindingsCount(owner))
                            Assert.Equal(expected:=Cursors.WaitCursor,
                                         actual:=owner.Cursor)
                        Finally
                            BindingFinished(owner, dgv:=otherGrid)
                        End Try
                        Assert.Equal(expected:=0,
                                     actual:=GetPendingBindingsCount(owner))
                        Assert.Equal(expected:=Cursors.Default,
                                     actual:=owner.Cursor)
                    End Using
                Catch ex As Exception
                    failure = ex
                Finally
                    s_notificationHistoryValue = previousHistory
                End Try
            End Sub

        Dim thread As New Thread(start)
        thread.SetApartmentState(state:=ApartmentState.STA)
        thread.Start()
        thread.Join()
        If failure IsNot Nothing Then
            ExceptionDispatchInfo.Capture(source:=failure).Throw()
        End If
    End Sub

    <Theory>
    <InlineData(True, True)>
    <InlineData(True, False)>
    <InlineData(False, True)>
    <InlineData(False, False)>
    Public Sub SelectingGridTab_ReleasesWaitCursor(notifications As Boolean, visible As Boolean)
        Dim failure As Exception = Nothing
        Dim start As ThreadStart =
            Sub()
                Try
                    Using owner As New Form1()
                        owner.Show()
                        owner.TabControlPage1.Visible = visible
                        Application.DoEvents()
                        owner.TabControlPage1.SelectedTab =
                            If(notifications,
                               owner.TabPage11NotificationsCleared,
                               owner.TabPage13SensorGlucose)
                        Application.DoEvents()
                        Assert.Equal(expected:=Cursors.Default,
                                     actual:=owner.Cursor)
                    End Using
                Catch ex As Exception
                    failure = ex
                End Try
            End Sub

        Dim thread As New Thread(start)
        thread.SetApartmentState(state:=ApartmentState.STA)
        thread.Start()
        thread.Join()
        If failure IsNot Nothing Then
            ExceptionDispatchInfo.Capture(source:=failure).Throw()
        End If
    End Sub

    <Fact>
    Public Sub UpdateNotificationTabs_PreservesWrappingAndPanelSizingAfterRefreshAndResize()
        Dim failure As Exception = Nothing
        Dim start As ThreadStart =
            Sub()
                Dim previousHistory As Dictionary(Of String, String) = s_notificationHistoryValue
                Try
                    s_notificationHistoryValue = New Dictionary(Of String, String) From {
                        {"clearedNotifications", "[{""type"":""sample"",""value"":""one"",""status"":""ok""}," &
                            "{""type"":""sample"",""value"":""two"",""status"":""ok""}]"},
                        {"activeNotifications", "[{""type"":""sample"",""value"":""active""}]"}}
                    Using owner As New Form1(), host As New Form()
                        Dim panel As TableLayoutPanel = owner.TlpNotificationsCleared
                        host.ClientSize = New Size(width:=900, height:=600)
                        host.Controls.Add(value:=panel)
                        host.Show()

                        For refresh As Integer = 1 To 2
                            owner.UpdateNotificationTabs()
                            Application.DoEvents()
                            Dim grids As DataGridView() = panel.Controls.OfType(Of DataGridView)().ToArray()
                            Assert.Equal(expected:=2,
                                         actual:=grids.Length)
                            Dim activeGrid As DataGridView =
                                Assert.Single(collection:=owner.TlpNotificationActive.Controls.OfType(Of DataGridView)())
                            Assert.Equal(expected:=BorderStyle.FixedSingle,
                                         actual:=activeGrid.BorderStyle)
                            For Each grid As DataGridView In grids
                                Assert.Equal(expected:=BorderStyle.FixedSingle,
                                             actual:=grid.BorderStyle)
                                Assert.True(condition:=grid.Rows.Count > 1)
                                For Each column As DataGridViewColumn In grid.Columns
                                    If column.Visible Then
                                        Dim padding As Padding = grid.Rows(index:=0).Cells(column.Index).GetFormattedStyle().Padding
                                        Assert.True(condition:=padding.Left >= 2 AndAlso padding.Right >= 2,
                                                    userMessage:=$"{column.Name} padding was {padding}.")
                                    End If
                                Next
                                Dim lastColumn As DataGridViewColumn = grid.Columns(index:=grid.ColumnCount - 1)
                                Assert.Equal(expected:=Fill,
                                             actual:=lastColumn.AutoSizeMode)
                                Assert.Equal(expected:=DataGridViewTriState.True,
                                             actual:=grid.Rows(index:=0).Cells(lastColumn.Index).InheritedStyle.WrapMode)
                                Assert.Equal(expected:=DataGridViewTriState.False,
                                             actual:=grid.Rows(index:=1).Cells(lastColumn.Index).InheritedStyle.WrapMode)
                                Assert.Equal(expected:=SizeType.Absolute,
                                             actual:=panel.RowStyles(index:=panel.Controls.IndexOf(control:=grid)).SizeType)
                                Assert.Equal(expected:=CSng(grid.Height + grid.Margin.Vertical),
                                             actual:=panel.RowStyles(index:=panel.Controls.IndexOf(control:=grid)).Height)
                                Dim rowsHeight As Integer =
                                    grid.Rows.GetRowsHeight(includeFilter:=DataGridViewElementStates.Visible)
                                Assert.InRange(actual:=grid.ClientSize.Height - rowsHeight,
                                               low:=0,
                                               high:=2)
                                Dim rowIndex As Integer = panel.GetRow(control:=grid)
                                If rowIndex < panel.RowCount - 1 Then
                                    Assert.InRange(actual:=panel.GetRowHeights()(rowIndex) - rowsHeight,
                                                   low:=0,
                                                   high:=20)
                                End If
                            Next

                            host.ClientSize = New Size(width:=host.ClientSize.Width - 100, height:=600)
                            Application.DoEvents()
                            Dim orderedGrids As DataGridView() =
                                grids.OrderBy(keySelector:=Function(grid As DataGridView)
                                                               Return panel.GetRow(control:=grid)
                                                           End Function).ToArray()
                            Assert.True(condition:=orderedGrids(1).Top > orderedGrids(0).Bottom)
                            For Each grid As DataGridView In grids
                                Assert.Equal(expected:=grid.Height + grid.Margin.Vertical,
                                             actual:=panel.RowStyles(index:=panel.Controls.IndexOf(control:=grid)).Height)
                            Next
                        Next
                    End Using
                Catch ex As Exception
                    failure = ex
                Finally
                    s_notificationHistoryValue = previousHistory
                End Try
            End Sub

        Dim thread As New Thread(start)
        thread.SetApartmentState(state:=ApartmentState.STA)
        thread.Start()
        thread.Join()
        If failure IsNot Nothing Then
            ExceptionDispatchInfo.Capture(source:=failure).Throw()
        End If
    End Sub

End Class
