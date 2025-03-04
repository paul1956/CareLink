' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Configuration
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Text.Json
Imports System.Windows.Forms.DataVisualization.Charting

Imports DataGridViewColumnControls
Imports Microsoft.Win32
Imports TableLayputPanelTop

Public Class Form1

    Private Shared s_webViewCacheDirectory As String
    Private ReadOnly _calibrationToolTip As New ToolTip()
    Private ReadOnly _processName As String = Process.GetCurrentProcess().ProcessName
    Private ReadOnly _sensorLifeToolTip As New ToolTip()
    Private ReadOnly _sgMiniDisplay As New SgMiniForm(Me)
    Private ReadOnly _updatingLock As New Object
    Private _activeInsulinChartAbsoluteRectangle As RectangleF = RectangleF.Empty
    Private _formScale As New SizeF(1.0F, 1.0F)
    Private _inMouseMove As Boolean = False
    Private _lastMarkerTabIndex As (page As Integer, tab As Integer) = (0, 0)
    Private _lastSummaryTabIndex As Integer = 0
    Private _previousLoc As Point
    Private _showBalloonTip As Boolean = True
    Private _summaryChartAbsoluteRectangle As RectangleF
    Private _treatmentMarkerAbsoluteRectangle As RectangleF
    Private _updating As Boolean
    Private _webViewProcessId As Integer = -1

    Public Shared ReadOnly Property WebViewCacheDirectory As String
        Get
            Return s_webViewCacheDirectory
        End Get
    End Property

    Public WriteOnly Property WebViewProcessId As Integer
        Set
            _webViewProcessId = Value
        End Set
    End Property

    Public Shared Property Client As Client2
        Get
            Return Form1LoginHelpers.LoginDialog?.Client
        End Get
        Set(value As Client2)
            Form1LoginHelpers.LoginDialog.Client = value
        End Set
    End Property

    '' <summary>
    '' Overloaded System Windows Handler.
    '' </summary>
    '' <param name="m">Message <see cref="Message"/> structure</param>
    <DebuggerNonUserCode()>
    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case WM_POWERBROADCAST
                Select Case m.WParam.ToInt32()
                    'value passed when system is going on standby / suspended
                    Case PBT_APMQUERYSUSPEND
                        Me.PowerModeChanged(Nothing, New PowerModeChangedEventArgs(PowerModes.Suspend))

                        'value passed when system is resumed after suspension.
                    Case PBT_APMRESUMESUSPEND
                        Me.PowerModeChanged(Nothing, New PowerModeChangedEventArgs(PowerModes.Resume))

                    'value passed when system Suspend Failed
                    Case PBT_APMQUERYSUSPENDFAILED
                        Me.PowerModeChanged(Nothing, New PowerModeChangedEventArgs(PowerModes.Resume))

                    'value passed when system is suspended
                    Case PBT_APMSUSPEND
                        Me.PowerModeChanged(Nothing, New PowerModeChangedEventArgs(PowerModes.Suspend))

                    'value passed when system is in standby
                    Case PBT_APMSTANDBY
                        Me.PowerModeChanged(Nothing, New PowerModeChangedEventArgs(PowerModes.Suspend))

                        'value passed when system resumes from standby
                    Case PBT_APMRESUMESTANDBY
                        Me.PowerModeChanged(Nothing, New PowerModeChangedEventArgs(PowerModes.Resume))

                        'value passed when system resumes from suspend
                    Case PBT_APMRESUMESUSPEND
                        Me.PowerModeChanged(Nothing, New PowerModeChangedEventArgs(PowerModes.Resume))

                    'value passed when system is resumed automatically
                    Case PBT_APMRESUMEAUTOMATIC
                        Me.PowerModeChanged(Nothing, New PowerModeChangedEventArgs(PowerModes.Resume))

                    'value passed when system is resumed from critical
                    'suspension possibly due to battery failure
                    Case PBT_APMRESUMECRITICAL
                        Stop

                    'value passed when system is low on battery
                    Case PBT_APMBATTERYLOW
                        Stop

                    'value passed when system power status changed
                    'from battery to AC power or vice-a-versa
                    Case PBT_APMPOWERSTATUSCHANGE
                        Stop

                    'value passed when OEM Event is fired. Not sure what that is??
                    Case PBT_APMOEMEVENT
                        Stop

                    Case Else
                        'Stop
                End Select
            Case Else
        End Select
        MyBase.WndProc(m)
    End Sub

#Region "Chart Objects"

    Private WithEvents ActiveInsulinChart As Chart
    Private WithEvents SummaryChart As Chart
    Private WithEvents TimeInRangeChart As Chart
    Private WithEvents TreatmentMarkersChart As Chart

#Region "Legends"

    Friend _activeInsulinChartLegend As Legend
    Friend _summaryChartLegend As Legend
    Friend _treatmentMarkersChartLegend As Legend

#End Region

#Region "Common Series"

    Private Property ActiveInsulinActiveInsulinSeries As Series
    Private Property ActiveInsulinAutoCorrectionSeries As Series
    Private Property ActiveInsulinBasalSeries As Series
    Private Property ActiveInsulinMarkerSeries As Series
    Private Property ActiveInsulinMinBasalSeries As Series
    Private Property ActiveInsulinSgSeries As Series
    Private Property ActiveInsulinSuspendSeries As Series
    Private Property ActiveInsulinTargetSeries As Series
    Private Property ActiveInsulinTimeChangeSeries As Series

    Private Property SummaryAutoCorrectionSeries As Series
    Private Property SummaryBasalSeries As Series
    Private Property SummaryHighLimitSeries As Series
    Private Property SummaryLowLimitSeries As Series
    Private Property SummaryMarkerSeries As Series
    Private Property SummaryMinBasalSeries As Series
    Private Property SummarySgSeries As Series
    Private Property SummarySuspendSeries As Series
    Private Property SummaryTargetSgSeries As Series
    Private Property SummaryTimeChangeSeries As Series

    Private Property TimeInRangeSeries As New Series

    Private Property TreatmentMarkerAutoCorrectionSeries As Series
    Private Property TreatmentMarkerBasalSeries As Series
    Private Property TreatmentMarkerMarkersSeries As Series
    Private Property TreatmentMarkerMinBasalSeries As Series
    Private Property TreatmentMarkerSgSeries As Series
    Private Property TreatmentMarkerSuspendSeries As Series
    Private Property TreatmentMarkerTimeChangeSeries As Series
    Private Property TreatmentTargetSeries As Series

#End Region 'Common Series

#Region "Titles"

    Private WithEvents ActiveInsulinChartTitle As New Title
    Private WithEvents TreatmentMarkersChartTitle As Title

#End Region ' Titles

#End Region ' Chart Objects

#Region "Events"

#Region "Chart Events"

    Private Sub Chart_CursorPositionChanging(sender As Object, e As CursorEventArgs) Handles _
                        ActiveInsulinChart.CursorPositionChanging,
                        SummaryChart.CursorPositionChanging

        If Not ProgramInitialized Then
            Exit Sub
        End If

        Me.CursorTimer.Interval = s_30SecondInMilliseconds
        Me.CursorTimer.Start()
    End Sub

    Private Sub Chart_MouseLeave(sender As Object, e As EventArgs) Handles _
        SummaryChart.MouseLeave,
        ActiveInsulinChart.MouseLeave,
        TreatmentMarkersChart.MouseLeave

        With s_calloutAnnotations(CType(sender, Chart).Name)
            If .Visible Then
                .Visible = False
            End If
        End With
    End Sub

    Private Sub Chart_MouseMove(sender As Object, e As MouseEventArgs) Handles _
        SummaryChart.MouseMove,
        ActiveInsulinChart.MouseMove,
        TreatmentMarkersChart.MouseMove

        If Not ProgramInitialized Then
            Exit Sub
        End If
        If e.Button <> MouseButtons.None OrElse e.Clicks > 0 OrElse e.Location = _previousLoc Then
            Return
        End If
        _inMouseMove = True
        _previousLoc = e.Location
        Dim yInPixels As Double
        Dim chart1 As Chart = CType(sender, Chart)
        Dim isHomePage As Boolean = chart1.Name = NameOf(SummaryChart)
        Try
            yInPixels = chart1.ChartAreas(NameOf(ChartArea)).AxisY2.ValueToPixelPosition(e.Y)
        Catch ex As Exception
            yInPixels = Double.NaN
        End Try
        If Double.IsNaN(yInPixels) Then
            _inMouseMove = False
            Exit Sub
        End If
        Dim result As HitTestResult
        Try
            result = chart1.HitTest(e.X, e.Y, ignoreTransparent:=True)
            If result.Series Is Nothing OrElse
                result.PointIndex = -1 Then
                Me.CursorPanel.Visible = False
                Exit Sub
            End If

            Dim currentDataPoint As DataPoint = result.Series.Points(result.PointIndex)

            If currentDataPoint.IsEmpty OrElse currentDataPoint.Color = Color.Transparent Then
                Me.CursorPanel.Visible = False
                Exit Sub
            End If

            Select Case result.Series.Name
                Case HighLimitSeriesName, LowLimitSeriesName, TargetSgSeriesName
                    Me.CursorPanel.Visible = False
                Case MarkerSeriesName, BasalSeriesName
                    Dim markerTag() As String = currentDataPoint.Tag.ToString.Split(":"c)
                    If markerTag.Length <= 1 Then
                        If chart1.Name = NameOf(TreatmentMarkersChart) Then
                            Dim callout As CalloutAnnotation = chart1.FindAnnotation(currentDataPoint)
                            callout.BringToFront()
                        Else
                            Me.CursorPanel.Visible = True
                        End If
                        Exit Sub
                    End If
                    markerTag(0) = markerTag(0).Trim
                    If isHomePage Then
                        Dim xValue As Date = Date.FromOADate(currentDataPoint.XValue)
                        Me.CursorPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
                        Me.CursorPictureBox.Visible = True
                        Me.CursorMessage2Label.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold, GraphicsUnit.Point)
                        Select Case markerTag.Length
                            Case 2
                                Me.CursorMessage1Label.Text = markerTag(0)
                                Me.CursorMessage1Label.Visible = True
                                Me.CursorMessage2Label.Text = markerTag(1).Trim
                                Me.CursorMessage2Label.Visible = True
                                Me.CursorMessage3Label.Text = Date.FromOADate(currentDataPoint.XValue).ToString(s_timeWithMinuteFormat)
                                Me.CursorMessage3Label.Visible = True
                                Me.CursorMessage4Label.Visible = False
                                Select Case markerTag(0)
                                    Case "Auto Correction",
                                         "Auto Basal",
                                         "Manual Basal",
                                         "Basal",
                                         "Min Auto Basal"
                                        Me.CursorPictureBox.Image = My.Resources.InsulinVial
                                    Case "Bolus"
                                        Me.CursorPictureBox.Image = My.Resources.InsulinVial
                                    Case "Meal"
                                        Me.CursorPictureBox.Image = My.Resources.MealImageLarge
                                    Case Else
                                        Stop
                                        Me.CursorMessage1Label.Visible = False
                                        Me.CursorMessage2Label.Visible = False
                                        Me.CursorMessage3Label.Visible = False
                                        Me.CursorPictureBox.Image = Nothing
                                End Select
                                Me.CursorPanel.Visible = True
                            Case 3
                                Select Case markerTag(1).Trim
                                    Case "Calibration accepted",
                                           "Calibration not accepted"
                                        Me.CursorPictureBox.Image = My.Resources.CalibrationDotRed
                                    Case "Not used for calibration"
                                        Me.CursorPictureBox.Image = My.Resources.CalibrationDot
                                        Me.CursorMessage2Label.Font = New Font("Segoe UI", 11.0F, FontStyle.Bold, GraphicsUnit.Point)
                                    Case Else
                                        Stop
                                End Select
                                Me.CursorMessage1Label.Text = $"{markerTag(0)}@{xValue.ToString(s_timeWithMinuteFormat)}"
                                Me.CursorMessage1Label.Visible = True
                                Me.CursorMessage2Label.Text = markerTag(1).Replace("Calibration not", "Cal. not").Trim
                                Me.CursorMessage2Label.Visible = True
                                Dim sgValue As Single = markerTag(2).Trim.Split(" ")(0).Trim.ParseSingle(2)
                                Me.CursorMessage3Label.Text = markerTag(2).Trim
                                Me.CursorMessage3Label.Visible = True
                                Me.CursorMessage4Label.Text = If(NativeMmolL, $"{CInt(sgValue * MmolLUnitsDivisor)} mg/dL", $"{sgValue / MmolLUnitsDivisor:F1} mmol/L")
                                Me.CursorMessage4Label.Visible = True
                                Me.CursorPanel.Visible = True
                            Case Else
                                Stop
                                Me.CursorPanel.Visible = False
                        End Select
                    End If
                    chart1.SetUpCallout(currentDataPoint, markerTag)

                Case SgSeriesName
                    Me.CursorMessage1Label.Text = "Sensor Glucose"
                    Me.CursorMessage1Label.Visible = True
                    Me.CursorMessage2Label.Text = $"{currentDataPoint.YValues(0).RoundToSingle(3)} {SgUnitsNativeString}"
                    Me.CursorMessage2Label.Visible = True
                    Me.CursorMessage3Label.Text = If(NativeMmolL, $"{CInt(currentDataPoint.YValues(0) * MmolLUnitsDivisor)} mg/dL", $"{currentDataPoint.YValues(0) / MmolLUnitsDivisor:F1} mmol/L")
                    Me.CursorMessage3Label.Visible = True
                    Me.CursorMessage4Label.Text = Date.FromOADate(currentDataPoint.XValue).ToString(s_timeWithMinuteFormat)
                    Me.CursorMessage4Label.Visible = True
                    Me.CursorPictureBox.Image = Nothing
                    Me.CursorPanel.Visible = True
                    chart1.SetupCallout(currentDataPoint, $"Sensor Glucose {Me.CursorMessage2Label.Text}")
                Case SuspendSeriesName, TimeChangeSeriesName
                    Me.CursorPanel.Visible = False
                Case ActiveInsulinSeriesName
                    chart1.SetupCallout(currentDataPoint, $"Theoretical Active Insulin {currentDataPoint.YValues.FirstOrDefault:F3} U")
                Case Else
                    Stop
            End Select
        Catch ex As Exception
            result = Nothing
        Finally
            _inMouseMove = False
        End Try
    End Sub

    Private Sub TemporaryUseAdvanceAITDecayCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles TemporaryUseAdvanceAITDecayCheckBox.CheckedChanged
        Me.TemporaryUseAdvanceAITDecayCheckBox.Text = If(Me.TemporaryUseAdvanceAITDecayCheckBox.CheckState = CheckState.Checked,
            $"Advanced Decay, AIT will decay over {CurrentUser.InsulinRealAit} hours while using {CurrentUser.InsulinTypeName}",
            $"AIT will decay over {CurrentUser.PumpAit.ToHoursMinutes} while using {CurrentUser.InsulinTypeName}")
        CurrentUser.UseAdvancedAitDecay = Me.TemporaryUseAdvanceAITDecayCheckBox.CheckState
        Me.UpdateActiveInsulinChart()
    End Sub

#Region "Post Paint Events"

    <DebuggerNonUserCode()>
    Private Sub ActiveInsulinChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles ActiveInsulinChart.PostPaint

        If Not ProgramInitialized OrElse _inMouseMove Then
            Exit Sub
        End If
        SyncLock _updatingLock
            If _updating Then
                Exit Sub
            End If
            e.PostPaintSupport(
                chartRelativePosition:=_activeInsulinChartAbsoluteRectangle,
                insulinDictionary:=s_activeInsulinMarkerInsulinDictionary,
                mealDictionary:=Nothing,
                offsetInsulinImage:=True,
                paintOnY2:=True)
        End SyncLock
    End Sub

    <DebuggerNonUserCode()>
    Private Sub SummaryChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles SummaryChart.PostPaint

        If Not ProgramInitialized OrElse _inMouseMove Then
            Exit Sub
        End If
        SyncLock _updatingLock
            If _updating Then
                Exit Sub
            End If
            e.PostPaintSupport(
                chartRelativePosition:=_summaryChartAbsoluteRectangle,
                insulinDictionary:=s_summaryMarkerInsulinDictionary,
                mealDictionary:=s_summaryMarkerMealDictionary,
                offsetInsulinImage:=True,
                paintOnY2:=True)
        End SyncLock
    End Sub

    <DebuggerNonUserCode()>
    Private Sub TreatmentMarkersChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles TreatmentMarkersChart.PostPaint

        If Not ProgramInitialized OrElse _inMouseMove Then
            Exit Sub
        End If
        SyncLock _updatingLock
            If _updating Then
                Exit Sub
            End If
            e.PostPaintSupport(
                chartRelativePosition:=_treatmentMarkerAbsoluteRectangle,
                insulinDictionary:=s_treatmentMarkerInsulinDictionary,
                mealDictionary:=s_treatmentMarkerMealDictionary,
                offsetInsulinImage:=False,
                paintOnY2:=False)
        End SyncLock
    End Sub

#End Region ' Post Paint Events

#End Region ' Chart Events

#Region "ContextMenuStrip Menu Events"

    Private WithEvents DgvCopyWithExcelMenuStrip As New ContextMenuStrip
    Friend WithEvents DgvCopyWithoutExcelMenuStrip As New ContextMenuStrip

    Private Sub DgvCopyWithExcelMenuStrip_Opening(sender As Object, e As CancelEventArgs) Handles DgvCopyWithExcelMenuStrip.Opening
        ' Acquire references to the owning control and item.
        Dim mnuStrip As ContextMenuStrip = CType(sender, ContextMenuStrip)
        mnuStrip.Tag = CType(mnuStrip.SourceControl, DataGridView)

        ' Clear the ContextMenuStrip control's Items collection.
        mnuStrip.Items.Clear()

        ' Populate the ContextMenuStrip control with its default items.
        mnuStrip.Items.Add("Copy with Header", My.Resources.Copy, AddressOf DgvExportToClipBoardWithHeaders)
        mnuStrip.Items.Add("Copy without Header", My.Resources.Copy, AddressOf DgvExportToClipBoardWithoutHeaders)
        mnuStrip.Items.Add("Save To Excel", My.Resources.ExportData, AddressOf DgvExportToExcel)

        ' Set Cancel to false.
        ' It is optimized to true based on empty entry.
        e.Cancel = False
    End Sub

    Private Sub DgvCopyWithoutExcelMenuStrip_Opening(sender As Object, e As CancelEventArgs) Handles DgvCopyWithoutExcelMenuStrip.Opening
        ' Acquire references to the owning control and item.
        Dim mnuStrip As ContextMenuStrip = CType(sender, ContextMenuStrip)
        mnuStrip.Tag = CType(Me.DgvCopyWithExcelMenuStrip.SourceControl, DataGridView)

        ' Clear the ContextMenuStrip control's Items collection.
        mnuStrip.Items.Clear()

        ' Populate the ContextMenuStrip control with its default items.
        mnuStrip.Items.Add("Copy Selected Cells with Header", My.Resources.Copy, AddressOf DgvCopySelectedCellsToClipBoardWithHeaders)
        mnuStrip.Items.Add("Copy Selected Cells without headers", My.Resources.Copy, AddressOf DgvCopySelectedCellsToClipBoardWithoutHeaders)

        ' Set Cancel to false.
        ' It is optimized to true based on empty entry.
        e.Cancel = False
    End Sub

    Public Sub Dgv_CellContextMenuStripNeededWithExcel(sender As Object, e As DataGridViewCellContextMenuStripNeededEventArgs) Handles _
        DgvAutoBasalDelivery.CellContextMenuStripNeeded,
        DgvInsulin.CellContextMenuStripNeeded,
        DgvMeal.CellContextMenuStripNeeded,
        DgvSGs.CellContextMenuStripNeeded

        If e.RowIndex >= 0 Then
            e.ContextMenuStrip = Me.DgvCopyWithExcelMenuStrip
        End If
    End Sub

    Public Sub Dgv_CellContextMenuStripNeededWithoutExcel(
        sender As Object, e As DataGridViewCellContextMenuStripNeededEventArgs) Handles _
        DgvCareLinkUsers.CellContextMenuStripNeeded,
        DgvCurrentUser.CellContextMenuStripNeeded,
        DgvSummary.CellContextMenuStripNeeded

        If e.RowIndex >= 0 AndAlso CType(sender, DataGridView).SelectedCells.Count > 0 Then
            e.ContextMenuStrip = Me.DgvCopyWithoutExcelMenuStrip
        End If

    End Sub

#End Region 'ContextMenuStrip Events

#Region "DataGridView Events"

#Region "Dgv Auto Basal Delivery (Basal) Events"

    Private Sub DgvAutoBasalDelivery_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DgvAutoBasalDelivery.CellFormatting
        Dim dgv As DataGridView = CType(sender, DataGridView)
        If e.Value Is Nothing Then
            e.Value = String.Empty
            Return
        End If
        ' Set the background to red for negative values in the Balance column.
        Select Case dgv.Columns(e.ColumnIndex).Name
            Case NameOf(AutoBasalDelivery.bolusAmount)
                dgv.CellFormattingSingleValue(e, 3)
                If dgv.CellFormattingSingleValue(e, 3).IsMinBasal Then
                    CellFormattingApplyColor(e, Color.DarkRed, isUri:=False)
                Else
                    dgv.CellFormattingSetForegroundColor(e)
                End If
            Case NameOf(AutoBasalDelivery.DisplayTime), NameOf(AutoBasalDelivery.Timestamp)
                dgv.CellFormattingDateTime(e)
            Case Else
                dgv.CellFormattingSetForegroundColor(e)
        End Select
    End Sub

    Private Sub DgvAutoBasalDelivery_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DgvAutoBasalDelivery.ColumnAdded
        With e.Column
            If AutoBasalDeliveryHelpers.HideColumn(.Name) Then
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
                Exit Sub
            End If
            e.DgvColumnAdded(
                cellStyle:=AutoBasalDeliveryHelpers.GetCellStyle(.Name),
                wrapHeader:=False,
                forceReadOnly:=True,
                caption:=CType(CType(sender, DataGridView).DataSource, DataTable).Columns(.Index).Caption)
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
    End Sub

#End Region ' Dgv Auto Basal Delivery (Basal) Events

#Region "Dgv CareLink Users Events"

    Private Sub DgvCareLinkUsers_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles DgvCareLinkUsers.CellBeginEdit
        Dim dgv As DataGridView = CType(sender, DataGridView)
        'Here we save a current value of cell to some variable, that later we can compare with a new value
        'For example using of dgv.Tag property
        If e.RowIndex >= 0 AndAlso e.ColumnIndex > 0 Then
            dgv.Tag = dgv.CurrentCell.Value.ToString
        End If

    End Sub

    Private Sub DgvCareLinkUsers_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgvCareLinkUsers.CellContentClick
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim dataGridViewDisableButtonCell As DataGridViewDisableButtonCell = TryCast(dgv.Rows(e.RowIndex).Cells(e.ColumnIndex), DataGridViewDisableButtonCell)
        If dataGridViewDisableButtonCell IsNot Nothing Then

            If Not dataGridViewDisableButtonCell.Enabled Then
                Exit Sub
            End If

            dgv.DataSource = Nothing
            s_allUserSettingsData.RemoveAt(e.RowIndex)
            dgv.DataSource = s_allUserSettingsData
            s_allUserSettingsData.SaveAllUserRecords()
        End If

    End Sub

    Private Sub DgvCareLinkUsers_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DgvCareLinkUsers.CellEndEdit
        'after you've filled your dataSet, on event above try something like this
        Try
            '
        Catch ex As Exception
            MessageBox.Show(ex.DecodeException())
        End Try

    End Sub

    Private Sub DgvCareLinkUsers_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DgvCareLinkUsers.CellFormatting
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim col As DataGridViewTextBoxColumn = TryCast(dgv.Columns(e.ColumnIndex), DataGridViewTextBoxColumn)
        If col IsNot Nothing Then
            dgv.CellFormattingSetForegroundColor(e)
        End If
    End Sub

    Private Sub DgvCareLinkUsers_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles DgvCareLinkUsers.CellValidating
        If e.ColumnIndex = 0 Then
            Exit Sub
        End If

    End Sub

    Private Sub DgvCareLinkUsers_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DgvCareLinkUsers.ColumnAdded
        With e.Column
            Dim dgv As DataGridView = CType(sender, DataGridView)
            Dim caption As String = dgv.Columns(.Index).HeaderText
            Dim dataPropertyName As String = e.Column.DataPropertyName
            If String.IsNullOrWhiteSpace(caption) Then
                caption = dataPropertyName.Replace("DgvCareLinkUsers", "")
            End If
            If caption.Contains("DeleteRow", StringComparison.OrdinalIgnoreCase) Then
                caption = ""
            Else
                If .Index > 0 AndAlso String.IsNullOrWhiteSpace(dataPropertyName) AndAlso String.IsNullOrWhiteSpace(caption) Then
                    dataPropertyName = s_headerColumns(.Index - 2)
                End If
            End If
            If CareLinkUserDataRecordHelpers.HideColumn(dataPropertyName) Then
                e.DgvColumnAdded(CareLinkUserDataRecordHelpers.GetCellStyle(dataPropertyName),
                                 False,
                                 False,
                                 caption)
                .Visible = False
            Else
                e.DgvColumnAdded(CareLinkUserDataRecordHelpers.GetCellStyle(dataPropertyName),
                                 False,
                                 True,
                                 caption)

            End If
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With
    End Sub

    Private Sub DgvCareLinkUsers_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DgvCareLinkUsers.DataError
        Stop
    End Sub

    Private Sub DgvCareLinkUsers_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles DgvCareLinkUsers.RowsAdded
        If s_allUserSettingsData.Count = 0 Then Exit Sub
        Dim dgv As DataGridView = CType(sender, DataGridView)
        For i As Integer = e.RowIndex To e.RowIndex + (e.RowCount - 1)
            Dim disableButtonCell As DataGridViewDisableButtonCell = CType(dgv.Rows(i).Cells("DgvCareLinkUsersDeleteRow"), DataGridViewDisableButtonCell)
            disableButtonCell.Enabled = s_allUserSettingsData(i).CareLinkUserName <> Form1LoginHelpers.LoginDialog.LoggedOnUser.CareLinkUserName
        Next
    End Sub

    Private Sub InitializeDgvCareLinkUsers(dgv As DataGridView)

        Dim dgvCareLinkUsersID As New DataGridViewTextBoxColumn With {
                .DataPropertyName = "ID",
                .HeaderText = "ID",
                .Name = NameOf(dgvCareLinkUsersID),
                .ReadOnly = True,
                .Width = 43
            }

        Dim dgvCareLinkUsersDeleteRow As New DataGridViewDisableButtonColumn With {
                .DataPropertyName = "DeleteRow",
                .HeaderText = "",
                .Name = NameOf(dgvCareLinkUsersDeleteRow),
                .ReadOnly = True,
                .Text = "Delete Row",
                .UseColumnTextForButtonValue = True,
                .Width = 5
            }

        Dim dgvCareLinkUsersCareLinkUserName As New DataGridViewTextBoxColumn With {
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .DataPropertyName = "CareLinkUserName",
            .HeaderText = $"CareLink™ UserName",
            .MinimumWidth = 125,
            .Name = NameOf(dgvCareLinkUsersCareLinkUserName),
            .Width = 125
        }

        Dim dgvCareLinkUsersCareLinkPassword As New DataGridViewTextBoxColumn With {
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .DataPropertyName = "CareLinkPassword",
            .HeaderText = $"CareLink™ Password",
            .Name = NameOf(dgvCareLinkUsersCareLinkPassword),
            .Width = 120
        }

        Dim dgvCareLinkUsersCountryCode As New DataGridViewTextBoxColumn With {
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .DataPropertyName = "CountryCode",
            .HeaderText = "Country Code",
            .Name = NameOf(dgvCareLinkUsersCountryCode),
            .Width = 97
        }

        Dim dgvCareLinkUsersUseLocalTimeZone As New DataGridViewCheckBoxColumn With {
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .DataPropertyName = "UseLocalTimeZone",
            .HeaderText = $"Use Local{vbCrLf} Time Zone",
            .Name = NameOf(dgvCareLinkUsersUseLocalTimeZone),
            .Width = 86
        }

        Dim dgvCareLinkUsersAutoLogin As New DataGridViewCheckBoxColumn With {
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .DataPropertyName = "AutoLogin",
            .HeaderText = "Auto Login",
            .Name = NameOf(dgvCareLinkUsersAutoLogin),
            .Width = 65
        }

        Dim dgvCareLinkUsersCareLinkPartner As New DataGridViewCheckBoxColumn With {
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .DataPropertyName = "CareLinkPartner",
            .HeaderText = $"CareLink™ Partner",
            .Name = NameOf(dgvCareLinkUsersCareLinkPartner),
            .Width = 86
        }

        Dim dgvCareLinkPatientUserID As New DataGridViewTextBoxColumn With {
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                .DataPropertyName = "CareLinkPatientUserID",
                .HeaderText = $"CareLink™ Patient UserID",
                .Name = NameOf(dgvCareLinkPatientUserID),
                .Width = 97
            }

        dgv.Columns.AddRange(New DataGridViewColumn() {dgvCareLinkUsersID, dgvCareLinkUsersDeleteRow, dgvCareLinkUsersCareLinkUserName, dgvCareLinkUsersCareLinkPassword, dgvCareLinkUsersCountryCode, dgvCareLinkUsersUseLocalTimeZone, dgvCareLinkUsersAutoLogin, dgvCareLinkUsersCareLinkPartner, dgvCareLinkPatientUserID})
        dgv.DataSource = Me.CareLinkUserDataRecordBindingSource
    End Sub

#End Region ' Dgv CareLink Users Events

#Region "Dgv Current User Events"

    Private Sub DgvCurrentUser_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DgvCurrentUser.ColumnAdded
        e.DgvColumnAdded(
            cellStyle:=New DataGridViewCellStyle().SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1)),
            wrapHeader:=False,
            forceReadOnly:=True,
            caption:=Nothing)

    End Sub

    Private Sub DgvCurrentUser_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DgvCurrentUser.DataError
        Stop
    End Sub

#End Region ' Dgv Current User Events

#Region "Dgv Insulin Events"

    Private Sub DgvInsulin_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DgvInsulin.DataError
        Stop
    End Sub

#End Region ' Dgv Insulin Events

#Region "Dgv Meal Events"

    Private Sub DgvMeal_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DgvMeal.CellFormatting
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Select Case dgv.Columns(e.ColumnIndex).Name
            Case NameOf(Meal.amount)
                dgv.CellFormattingInteger(e, GetCarbDefaultUnit)
            Case NameOf(Meal.timestamp)
                dgv.CellFormattingDateTime(e)
        End Select
    End Sub

    Private Sub DgvMeal_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DgvMeal.DataError
        Stop
    End Sub

#End Region ' Dgv Meal Events

#Region "Dgv SGs Events"

    Private Sub DgvSGs_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DgvSGs.CellFormatting
        If e.Value Is Nothing Then
            Return
        End If
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim alternateIndex As Integer = If(dgv.Rows(0).Cells(0).Value.ToString <> "0", 0, 1)
        Select Case dgv.Columns(e.ColumnIndex).Name
            Case NameOf(SG.sensorState)
                ' Set the background to red for negative values in the Balance column.
                If Not e.Value.Equals("NO_ERROR_MESSAGE") Then
                    CellFormattingApplyColor(e, Color.Red, isUri:=False)
                End If
                dgv.CellFormattingToTitle(e)
            Case NameOf(SG.Timestamp)
                dgv.CellFormattingDateTime(e)
            Case NameOf(SG.sg), NameOf(SG.sgMmolL), NameOf(SG.sgMmDl)
                dgv.CellFormattingSgValue(e, NameOf(SG.sg))
            Case Else
                dgv.CellFormattingSetForegroundColor(e)
        End Select
    End Sub

    Private Sub DgvSGs_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DgvSGs.ColumnAdded
        With e.Column
            If SgHelpers.HideColumn(.Name) Then
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .Visible = False
                Exit Sub
            End If

            Dim dgv As DataGridView = CType(sender, DataGridView)
            e.DgvColumnAdded(
                cellStyle:=SgHelpers.GetCellStyle(.Name),
                wrapHeader:=False,
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)

            Select Case .Index
                Case 0
                    .SortMode = DataGridViewColumnSortMode.Programmatic
                    .HeaderCell.SortGlyphDirection = SortOrder.Descending
                Case 1
                    .SortMode = DataGridViewColumnSortMode.Automatic
                    .HeaderCell.SortGlyphDirection = SortOrder.None
                Case Else
                    .SortMode = DataGridViewColumnSortMode.NotSortable
            End Select
        End With
    End Sub

    Private Sub DgvSGs_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DgvSGs.ColumnHeaderMouseClick
        If e.ColumnIndex <> 0 Then Exit Sub
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim col As DataGridViewColumn = dgv.Columns(e.ColumnIndex)
        Dim dir As ListSortDirection

        Select Case col.HeaderCell.SortGlyphDirection
            Case SortOrder.None, SortOrder.Ascending
                dir = ListSortDirection.Descending
            Case SortOrder.Descending
                dir = ListSortDirection.Ascending
        End Select

        dgv.Sort(col, dir)
    End Sub

    Private Sub DgvSGs_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DgvSGs.DataBindingComplete
        Dim dgv As DataGridView = CType(sender, DataGridView)
        For Each column As DataGridViewColumn In dgv.Columns
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Next
        dgv.Columns(dgv.Columns.Count - 1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Dim order As SortOrder = SortOrder.None
        If dgv.RowCount > 0 Then
            Dim value As String = dgv.Rows(0).Cells(0).Value.ToString
            If value = "288" Then
                order = SortOrder.Descending
            ElseIf value = "1" Then
                order = SortOrder.Ascending
            End If
        End If
        dgv.Columns(0).HeaderCell.SortGlyphDirection = order
    End Sub

#End Region ' Dgv SGs Events

#Region "Dgv Summary Events"

    Private Sub DgvSummary_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DgvSummary.CellFormatting
        If e.Value Is Nothing OrElse e.ColumnIndex <> 2 Then
            Return
        End If
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim key As String = dgv.Rows(e.RowIndex).Cells("key").Value.ToString
        Select Case GetItemIndex(key)
            Case ServerDataIndexes.currentServerTime,
                 ServerDataIndexes.lastMedicalDeviceDataUpdateServerTime,
                 ServerDataIndexes.firstName, ServerDataIndexes.lastName, ServerDataIndexes.conduitSerialNumber,
                 ServerDataIndexes.conduitBatteryStatus,
                 ServerDataIndexes.sensorState,
                 ServerDataIndexes.medicalDeviceTime,
                 ServerDataIndexes.calibStatus, ServerDataIndexes.bgUnits, ServerDataIndexes.timeFormat,
                 ServerDataIndexes.lastSGTrend, ServerDataIndexes.systemStatusMessage,
                 ServerDataIndexes.lastConduitDateTime, ServerDataIndexes.clientTimeZoneName,
                 ServerDataIndexes.appModelType
                e.CellStyle = e.CellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1))

            Case ServerDataIndexes.appModelNumber, ServerDataIndexes.conduitBatteryLevel,
                 ServerDataIndexes.reservoirLevelPercent, ServerDataIndexes.reservoirAmount,
                 ServerDataIndexes.reservoirRemainingUnits,
                 ServerDataIndexes.sensorDurationHours, ServerDataIndexes.timeToNextCalibHours,
                 ServerDataIndexes.averageSG, ServerDataIndexes.belowHypoLimit,
                 ServerDataIndexes.aboveHyperLimit, ServerDataIndexes.timeInRange,
                 ServerDataIndexes.gstBatteryLevel, ServerDataIndexes.maxAutoBasalRate,
                 ServerDataIndexes.maxBolusAmount, ServerDataIndexes.sensorDurationMinutes,
                 ServerDataIndexes.timeToNextCalibrationMinutes, ServerDataIndexes.sgBelowLimit,
                 ServerDataIndexes.averageSGFloat, ServerDataIndexes.timeToNextCalibrationRecommendedMinutes,
                 ServerDataIndexes.systemStatusTimeRemaining, ServerDataIndexes.timeToNextEarlyCalibrationMinutes
                e.CellStyle = e.CellStyle.SetCellStyle(
                    alignment:=DataGridViewContentAlignment.MiddleRight,
                    padding:=New Padding(left:=0, top:=1, right:=1, bottom:=1))

            Case ServerDataIndexes.conduitInRange, ServerDataIndexes.conduitMedicalDeviceInRange,
                 ServerDataIndexes.conduitSensorInRange,
                 ServerDataIndexes.pumpCommunicationState, ServerDataIndexes.gstCommunicationState,
                 ServerDataIndexes.calFreeSensor, ServerDataIndexes.finalCalibration
                e.CellStyle = e.CellStyle.SetCellStyle(
                    alignment:=DataGridViewContentAlignment.MiddleCenter,
                    padding:=New Padding(all:=1))

            Case ServerDataIndexes.lastAlarm,
                 ServerDataIndexes.activeInsulin,
                 ServerDataIndexes.sgs,
                 ServerDataIndexes.limits,
                 ServerDataIndexes.markers,
                 ServerDataIndexes.notificationHistory,
                 ServerDataIndexes.therapyAlgorithmState,
                 ServerDataIndexes.pumpBannerState,
                 ServerDataIndexes.basal,
                 ServerDataIndexes.cgmInfo,
                 ServerDataIndexes.medicalDeviceInformation
                Exit Select
            Case Else
                e.CellStyle = e.CellStyle.SetCellStyle(
                    alignment:=DataGridViewContentAlignment.MiddleCenter,
                    padding:=New Padding(all:=1))
        End Select

    End Sub

    Private Sub DgvSummary_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DgvSummary.CellMouseClick
        If e.RowIndex < 0 Then Exit Sub
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim value As String = dgv.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString
        If value.StartsWith(ClickToShowDetails) Then
            With Me.TabControlPage1
                Select Case dgv.Rows(e.RowIndex).Cells("key").Value.ToString.GetItemIndex()
                    Case ServerDataIndexes.lastSG
                        Me.TabControlPage2.SelectedIndex = 6
                        _lastMarkerTabIndex = (1, 6)
                        .Visible = False
                    Case ServerDataIndexes.lastAlarm
                        Me.TabControlPage2.SelectedIndex = 7
                        _lastMarkerTabIndex = (1, 7)
                        .Visible = False
                    Case ServerDataIndexes.activeInsulin
                        .SelectedIndex = GetTabIndexFromName(NameOf(TabPage07ActiveInsulin))
                    Case ServerDataIndexes.sgs
                        .SelectedIndex = GetTabIndexFromName(NameOf(TabPage08SensorGlucose))
                    Case ServerDataIndexes.limits
                        .SelectedIndex = GetTabIndexFromName(NameOf(TabPage09Limits))
                    Case ServerDataIndexes.markers
                        Dim page As Integer = _lastMarkerTabIndex.page
                        Dim tab As Integer = _lastMarkerTabIndex.tab
                        If page = 0 Then
                            If tab = 0 Then
                                _lastMarkerTabIndex = (0, 4)
                            End If
                            Me.TabControlPage1.SelectedIndex = _lastMarkerTabIndex.tab
                        Else
                            Me.TabControlPage2.SelectedIndex = If(5 < tab,
                                                                  0,
                                                                  _lastMarkerTabIndex.tab
                                                                 )
                            .Visible = False
                        End If
                    Case ServerDataIndexes.notificationHistory
                        .SelectedIndex = GetTabIndexFromName(NameOf(TabPage10NotificationHistory))
                    Case ServerDataIndexes.therapyAlgorithmState
                        .SelectedIndex = GetTabIndexFromName(NameOf(TabPage11TherapyAlgorithm))
                    Case ServerDataIndexes.pumpBannerState
                        .SelectedIndex = GetTabIndexFromName(NameOf(TabPage12BannerState))
                    Case ServerDataIndexes.basal
                        .SelectedIndex = GetTabIndexFromName(NameOf(TabPage13Basal))
                End Select
            End With
        End If
    End Sub

    Private Sub DgvSummary_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DgvSummary.ColumnAdded
        With e.Column
            e.DgvColumnAdded(
                cellStyle:=SummaryHelpers.GetCellStyle(.Name),
                wrapHeader:=False,
                forceReadOnly:=True,
                caption:=CType(CType(sender, DataGridView).DataSource, DataTable).Columns(.Index).Caption)
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

    End Sub

    Private Sub DgvSummary_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DgvSummary.DataError
        Stop
    End Sub

#End Region ' Dgv Summary Events

#End Region ' DataGridView Events

#Region "Form Events"

    Private Sub ActiveInsulinValue_Paint(sender As Object, e As PaintEventArgs) Handles ActiveInsulinValue.Paint
        ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, Color.LimeGreen, ButtonBorderStyle.Solid)
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.CleanUpNotificationIcon()
        If _webViewProcessId > 0 Then
            Dim webViewProcess As Process = Process.GetProcessById(_webViewProcessId)
            ' TODO: dispose of the WebView2 control
            'LoginDialog.WebView21.Dispose()
            webViewProcess.Kill()
            webViewProcess.WaitForExit(3_000)

            If Directory.Exists(WebViewCacheDirectory) Then
                Try
                    Directory.Delete(WebViewCacheDirectory, True)
                Catch
                    ' Ignore errors here
                End Try
            End If
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.UpgradeRequired Then
            My.Settings.Upgrade()
            My.Settings.UpgradeRequired = False
            My.Settings.Save()
        End If

        Dim currentAllUserLoginFile As String = GetUsersLoginInfoFileWithPath(True)
        If Not Directory.Exists(DirectoryForProjectData) Then
            Dim lastError As String = $"Can't create required project directories!"
            Directory.CreateDirectory(DirectoryForProjectData)
            Directory.CreateDirectory(SettingsDirectory)
            Try
                MoveIfExists(GetUsersLoginInfoFileWithPath(False), currentAllUserLoginFile, lastError)
                MoveIfExists(GetGraphColorsFileNameWithPath(False), GetGraphColorsFileNameWithPath(True), lastError)

                ' Move files with unique/variable names
                ' Error Reports
                lastError = $"Moving {BaseNameSavedErrorReport} files!"
                MoveFiles(MyDocuments, DirectoryForProjectData, $"{BaseNameSavedErrorReport}*.txt")
                lastError = $"Moving {BaseNameSavedLastDownload} files!"
                MoveFiles(MyDocuments, DirectoryForProjectData, $"{BaseNameSavedLastDownload}*.json")
                lastError = $"Moving {BaseNameSavedSnapshot} files!"
                MoveFiles(MyDocuments, DirectoryForProjectData, $"{BaseNameSavedSnapshot}*.json")
            Catch ex As Exception
                MsgBox($"Last error: {lastError}", ex.Message, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "Fatal Error")
                End
            End Try
        End If

        If Not Directory.Exists(SettingsDirectory) Then
            Directory.CreateDirectory(SettingsDirectory)
        End If

        If File.Exists(currentAllUserLoginFile) Then
            s_allUserSettingsData.LoadUserRecords(currentAllUserLoginFile)
        Else
            My.Settings.AutoLogin = False
        End If

        Me.InitializeDgvCareLinkUsers(Me.DgvCareLinkUsers)
        s_formLoaded = True
        Me.MenuOptionsAudioAlerts.Checked = My.Settings.SystemAudioAlertsEnabled
        Me.MenuOptionsSpeechRecognitionEnabled.Checked = My.Settings.SystemSpeechRecognitionThreshold < 1
        Me.SetSpeechRecognitionConfidenceThreshold()
        Me.MenuOptionsShowChartLegends.Checked = My.Settings.SystemShowLegends
        Me.MenuOptionsSpeechHelpShown.Checked = My.Settings.SystemSpeechHelpShown
        AddHandler My.Settings.SettingChanging, AddressOf Me.MySettings_SettingChanging

        If File.Exists(GetGraphColorsFileNameWithPath(True)) Then
            GetColorDictionaryFromFile()
        Else
            WriteColorDictionaryToFile()
        End If

        Me.InsulinTypeLabel.Text = s_insulinTypes.Keys(1)
        If String.IsNullOrWhiteSpace(WebViewCacheDirectory) Then
            s_webViewCacheDirectory = Path.Combine(s_projectWebCache, Guid.NewGuid().ToString)
            Directory.CreateDirectory(WebViewCacheDirectory)
        End If
    End Sub

    Private Sub Form1_Reseize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            Me.NotifyIcon1.Visible = True
            If Me.NotifyIcon1.BalloonTipText.Length > 0 Then
                Me.NotifyIcon1.ShowBalloonTip(1000)
            End If
        End If
    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Me.Fix(Me)

        Me.CurrentSgLabel.Parent = Me.SmartGuardShieldPictureBox
        Me.ShieldUnitsLabel.Parent = Me.SmartGuardShieldPictureBox
        Me.ShieldUnitsLabel.BackColor = Color.Transparent
        Me.SensorDaysLeftLabel.Parent = Me.SensorTimeLeftPictureBox
        Me.SensorMessageLabel.Parent = Me.SmartGuardShieldPictureBox
        Me.SensorDaysLeftLabel.BackColor = Color.Transparent
        s_useLocalTimeZone = My.Settings.UseLocalTimeZone
        Me.MenuOptionsUseLocalTimeZone.Checked = s_useLocalTimeZone
        CheckForUpdatesAsync(False)

        Dim caption As String = $"TIR Compliance, value in (){vbCrLf}Values<2 is Excellent and not shown{vbCrLf}CareLink<4 are considered OK, it is possible to improve{vbCrLf}Values>4 implies improvement needed{vbCrLf}"
        Me.ToolTip1.SetToolTip(Me.LowTirComplianceLabel, caption)
        Me.ToolTip1.SetToolTip(Me.HighTirComplianceLabel, caption)

        Me.NotifyIcon1.Visible = True
        Application.DoEvents()
        Me.NotifyIcon1.Visible = False
        Application.DoEvents()

        If DoOptionalLoginAndUpdateData(UpdateAllTabs:=False, fileToLoad:=FileToLoadOptions.NewUser) Then
            Me.UpdateAllTabPages(fromFile:=False)
        End If
    End Sub

    Private Sub SerialNumberButton_Click(sender As Object, e As EventArgs) Handles SerialNumberButton.Click
        Me.TabControlPage1.SelectedIndex = 3
        Me.TabControlPage1.Visible = True
        Dim dgv As DataGridView = CType(Me.TabControlPage1.TabPages(3).Controls(0), DataGridView)
        dgv.FirstDisplayedScrollingRowIndex = dgv.RowCount - 1
        For Each row As DataGridViewRow In dgv.Rows
            If row.Cells(1).FormattedValue.ToString = "medicalDeviceInformation" Then
                dgv.CurrentCell = row.Cells(2)
                Exit For
            End If
        Next
    End Sub

#End Region ' Form Events

#Region "Form Menu Events"

#Region "Start Here Menu Events"

    Private Sub MenuStartHere_DropDownOpening(sender As Object, e As EventArgs) Handles MenuStartHere.DropDownOpening
        Me.MenuStartHereLoadSavedDataFile.Enabled = AnyMatchingFiles(DirectoryForProjectData, $"CareLink*.json")
        Me.MenuStartHereSnapshotSave.Enabled = Not RecentDataEmpty()
        Me.MenuStartHereExceptionReportLoad.Visible = AnyMatchingFiles(DirectoryForProjectData, $"{BaseNameSavedErrorReport}*.txt")

        Dim userPdfExists As Boolean = Not (String.IsNullOrWhiteSpace(s_userName) OrElse Not AnyMatchingFiles(SettingsDirectory, $"{s_userName}Settings.pdf"))
        Me.MenuStartHereShowPumpSetup.Enabled = userPdfExists
        Me.MenuStartHereManuallyImportDeviceSettings.Enabled = Not userPdfExists
        Me.MenuStartHereCleanUpObsoleteFiles.Enabled = Process.GetProcessesByName(_processName).Length = 1
    End Sub

    Private Sub MenuStartHereCleanUpObsoleteFiles_Click(sender As Object, e As EventArgs) Handles MenuStartHereCleanUpObsoleteFiles.Click
        CleanupStaleFilesDialog.ShowDialog()
    End Sub

    Private Sub MenuStartHereExceptionReportLoad_Click(sender As Object, e As EventArgs) Handles MenuStartHereExceptionReportLoad.Click
        Dim fileList As String() = Directory.GetFiles(DirectoryForProjectData, $"{BaseNameSavedErrorReport}*.txt")
        Using openFileDialog1 As New OpenFileDialog With {
            .AddExtension = True,
            .AddToRecent = False,
            .CheckFileExists = True,
            .CheckPathExists = True,
            .DefaultExt = "txt",
            .FileName = If(fileList.Length > 0, Path.GetFileName(fileList(0)), "CareLink"),
            .Filter = $"Error files (*.txt)|{BaseNameSavedErrorReport}*.txt",
            .InitialDirectory = DirectoryForProjectData,
            .Multiselect = False,
            .ReadOnlyChecked = True,
            .RestoreDirectory = True,
            .ShowPreview = False,
            .SupportMultiDottedExtensions = False,
            .Title = $"Select CareLink™ saved snapshot to load",
            .ValidateNames = True}

            If openFileDialog1.ShowDialog(Me) = DialogResult.OK Then
                Try
                    Dim fileNameWithPath As String = openFileDialog1.FileName
                    StartOrStopServerUpdateTimer(False)
                    If File.Exists(fileNameWithPath) Then
                        RecentData?.Clear()
                        ExceptionHandlerDialog.ReportFileNameWithPath = fileNameWithPath
                        If ExceptionHandlerDialog.ShowDialog(Me) = DialogResult.OK Then
                            ExceptionHandlerDialog.ReportFileNameWithPath = ""
                            Try
                                RecentData = LoadIndexedItems(ExceptionHandlerDialog.LocalRawData)
                            Catch ex As Exception
                                MessageBox.Show($"Error reading date file. Original error: {ex.DecodeException()}")
                            End Try
                            CurrentDateCulture = openFileDialog1.FileName.ExtractCultureFromFileName($"CareLink", True)
                            Me.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                            Me.Text = $"{SavedTitle} Using file {Path.GetFileName(fileNameWithPath)}"
                            Dim epochDateTime As Date = s_lastMedicalDeviceDataUpdateServerEpoch.Epoch2PumpDateTime
                            SetLastUpdateTime(epochDateTime.ToShortDateTimeString, "from file", False, epochDateTime.IsDaylightSavingTime)
                            SetUpCareLinkUser(TestSettingsFileNameWithPath)

                            Try
                                FinishInitialization()
                            Catch ex As Exception
                                MessageBox.Show($"Error in {NameOf(FinishInitialization)}. Original error: {ex.Message}")
                            End Try
                            Try
                                Me.UpdateAllTabPages(True)
                            Catch ex As Exception
                                MessageBox.Show($"Error in {NameOf(UpdateAllTabPages)}. Original error: {ex.Message}")
                            End Try
                        End If
                    End If
                Catch ex As Exception
                    MessageBox.Show($"Cannot read file from disk. Original error: {ex.DecodeException()}")
                End Try
            End If
        End Using
    End Sub

    Private Sub MenuStartHereExit_Click(sender As Object, e As EventArgs) Handles MenuStartHereExit.Click
        Me.Close()
    End Sub

    Private Sub MenuStartHereLoadSavedDataFile_Click(sender As Object, e As EventArgs) Handles MenuStartHereLoadSavedDataFile.Click
        Dim di As New DirectoryInfo(DirectoryForProjectData)
        Dim fileList As String() = New DirectoryInfo(DirectoryForProjectData).
                                        EnumerateFiles($"CareLink*.json").
                                        OrderBy(Function(f As FileInfo) f.LastWriteTime).
                                        Select(Function(f As FileInfo) f.Name).ToArray
        Using openFileDialog1 As New OpenFileDialog With {
            .AddExtension = True,
            .AddToRecent = False,
            .CheckFileExists = True,
            .CheckPathExists = True,
            .DefaultExt = "json",
            .FileName = $"{s_userName}Settings.json",
            .Filter = $"json files (*.json)|CareLink*.json",
            .InitialDirectory = DirectoryForProjectData,
            .Multiselect = False,
            .ReadOnlyChecked = True,
            .RestoreDirectory = True,
            .ShowPreview = False,
            .SupportMultiDottedExtensions = False,
            .Title = $"Select CareLink™ saved snapshot to load",
            .ValidateNames = True}

            If openFileDialog1.ShowDialog(Me) = DialogResult.OK Then
                Try
                    If File.Exists(openFileDialog1.FileName) Then
                        StartOrStopServerUpdateTimer(False)
                        SetUpCareLinkUser(TestSettingsFileNameWithPath)
                        CurrentDateCulture = openFileDialog1.FileName.ExtractCultureFromFileName($"CareLink", True)
                        CurrentUICulture = CurrentDateCulture

                        RecentData = LoadIndexedItems(File.ReadAllText(openFileDialog1.FileName))
                        Me.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                        Me.Text = $"{SavedTitle} Using file {Path.GetFileName(openFileDialog1.FileName)}"
                        Dim fileDate As Date = File.GetLastWriteTime(openFileDialog1.FileName)
                        SetLastUpdateTime(fileDate.ToShortDateTimeString, " from file", False, fileDate.IsDaylightSavingTime)
                        FinishInitialization()
                        Me.UpdateAllTabPages(True)
                    End If
                Catch ex As Exception
                    MessageBox.Show($"Cannot read file from disk. Original error: {ex.DecodeException()}")
                End Try
            End If
        End Using
    End Sub

    Private Sub MenuStartHereLogin_Click(sender As Object, e As EventArgs) Handles MenuStartHereLogin.Click
        DoOptionalLoginAndUpdateData(UpdateAllTabs:=True, fileToLoad:=FileToLoadOptions.NewUser)
    End Sub

    Private Sub MenuStartHereManuallyImportDeviceSettings_Click(sender As Object, e As EventArgs) Handles MenuStartHereManuallyImportDeviceSettings.Click
        Dim downloadsPath As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) & "\Downloads\"
        Using openFileDialog1 As New OpenFileDialog With {
            .AddExtension = True,
            .AddToRecent = False,
            .CheckFileExists = True,
            .CheckPathExists = True,
            .DefaultExt = "pdf",
            .Filter = $"Settings file (*.pdf)|*.pdf",
            .InitialDirectory = downloadsPath,
            .Multiselect = False,
            .ReadOnlyChecked = True,
            .RestoreDirectory = True,
            .ShowPreview = False,
            .SupportMultiDottedExtensions = False,
            .Title = $"Select downloaded CareLink™ Settings file.",
            .ValidateNames = True}

            If openFileDialog1.ShowDialog(Me) = DialogResult.OK Then
                My.Computer.FileSystem.MoveFile(
                    sourceFileName:=openFileDialog1.FileName,
                    destinationFileName:=GetUserSettingsPdfFileNameWithPath(),
                    showUI:=FileIO.UIOption.AllDialogs,
                    onUserCancel:=FileIO.UICancelOption.DoNothing)
            End If
        End Using
    End Sub

    Private Sub MenuStartHereShowPumpSetup_Click(sender As Object, e As EventArgs) Handles MenuStartHereShowPumpSetup.Click
        Dim userSettingsPdfFile As String = GetUserSettingsPdfFileNameWithPath()

        If File.Exists(userSettingsPdfFile) Then
            If CurrentPdf.IsValid Then
                Using dialog As New PumpSetupDialog
                    StartOrStopServerUpdateTimer(False)
                    dialog.Pdf = CurrentPdf
                    dialog.ShowDialog(Me)
                End Using
            End If
            If Not CurrentPdf.IsValid Then
                MsgBox($"Device Setting PDF file is invalid", userSettingsPdfFile, MsgBoxStyle.OkOnly, "Invalid Settings PDF File")
            End If
        Else
            MsgBox($"Device Setting PDF file is missing!", userSettingsPdfFile, MsgBoxStyle.OkOnly, "Missing Settings PDF File")
        End If
        StartOrStopServerUpdateTimer(True)
    End Sub

    Private Sub MenuStartHereSnapshotSave_Click(sender As Object, e As EventArgs) Handles MenuStartHereSnapshotSave.Click
        If RecentDataEmpty() Then Exit Sub
        Using jd As JsonDocument = JsonDocument.Parse(RecentData.CleanUserData(), New JsonDocumentOptions)
            File.WriteAllTextAsync(GetUniqueDataFileName(BaseNameSavedSnapshot, CurrentDateCulture.Name, "json", True).withPath, JsonSerializer.Serialize(jd, s_jsonSerializerOptions))
        End Using

    End Sub

    Private Sub MenuStartHereUseLastSavedFile_Click(sender As Object, e As EventArgs) Handles MenuStartHereUseLastSavedFile.Click
        DoOptionalLoginAndUpdateData(UpdateAllTabs:=True, fileToLoad:=FileToLoadOptions.LastSaved)
        Me.MenuStartHereSnapshotSave.Enabled = False
    End Sub

    Private Sub MenuStartHereUseTestData_Click(sender As Object, e As EventArgs) Handles MenuStartHereUseTestData.Click
        DoOptionalLoginAndUpdateData(UpdateAllTabs:=True, fileToLoad:=FileToLoadOptions.TestData)
        Me.MenuStartHereSnapshotSave.Enabled = False
    End Sub

#End Region ' Start Here Menu Events

#Region "Option Menus"

    Private Function GetSpeechConfidenceValue() As Double
        For Each item As ToolStripMenuItem In Me.MenuOptionsSpeechRecognitionEnabled.DropDownItems
            If IsNumeric(item.Text) AndAlso item.Checked Then
                Return CDbl(item.Text)
            End If
        Next
        Return 100
    End Function

    Private Sub MenuOptionsAudioAlerts_Click(sender As Object, e As EventArgs) Handles MenuOptionsAudioAlerts.Click
        Dim playAudioAlerts As Boolean = Me.MenuOptionsAudioAlerts.Checked
        My.Settings.SystemAudioAlertsEnabled = playAudioAlerts
        My.Settings.Save()
        If playAudioAlerts Then
            Me.MenuOptionsSpeechRecognitionEnabled.Enabled = True
            Me.SetSpeechRecognitionConfidenceThreshold()
            If Not Me.MenuOptionsSpeechRecognitionDisabled.Checked Then
                InitializeSpeechRecognition()
            End If
        Else
            Me.MenuOptionsSpeechRecognitionEnabled.Checked = My.Settings.SystemSpeechRecognitionThreshold < 1
            Me.MenuOptionsSpeechRecognitionEnabled.Enabled = False
            CancelSpeechRecognition()
        End If
    End Sub

    Private Sub MenuOptionsAutoLogin_CheckedChanged(sender As Object, e As EventArgs) Handles MenuOptionsAutoLogin.CheckedChanged
        My.Settings.AutoLogin = Me.MenuOptionsAutoLogin.Checked
    End Sub

    Private Sub MenuOptionsColorPicker_Click(sender As Object, e As EventArgs) Handles MenuOptionsColorPicker.Click
        Using o As New OptionsDialog()
            o.ShowDialog(Me)
        End Using
    End Sub

    Private Sub MenuOptionsEditPumpSettings_Click(sender As Object, e As EventArgs) Handles MenuOptionsEditPumpSettings.Click
        SetUpCareLinkUser(GetUserSettingsJsonFileNameWithPath, True)
        Dim contents As String = File.ReadAllText(GetUserSettingsJsonFileNameWithPath)
        CurrentUser = JsonSerializer.Deserialize(Of CurrentUserRecord)(contents, s_jsonSerializerOptions)
    End Sub

    Private Sub MenuOptionsFilterRawJSONData_Click(sender As Object, e As EventArgs) Handles MenuOptionsFilterRawJSONData.Click
        s_filterJsonData = Me.MenuOptionsFilterRawJSONData.Checked

        Dim lastColumnIndex As Integer = Me.DgvAutoBasalDelivery.Columns.Count - 1
        For i As Integer = 0 To lastColumnIndex
            Dim c As DataGridViewColumn = Me.DgvAutoBasalDelivery.Columns(i)
            If i > 0 AndAlso String.IsNullOrWhiteSpace(c.DataPropertyName) Then
                Stop
            End If
            c.Visible = Not AutoBasalDeliveryHelpers.HideColumn(c.DataPropertyName)
            c.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Next

        lastColumnIndex = Me.DgvCareLinkUsers.Columns.Count - 1

        For i As Integer = 0 To lastColumnIndex
            Dim c As DataGridViewColumn = Me.DgvCareLinkUsers.Columns(i)
            If i > 0 AndAlso String.IsNullOrWhiteSpace(c.DataPropertyName) Then
                Stop
            End If
            c.Visible = Not CareLinkUserDataRecordHelpers.HideColumn(c.DataPropertyName)
            c.AutoSizeMode = If(i = lastColumnIndex, DataGridViewAutoSizeColumnMode.Fill, DataGridViewAutoSizeColumnMode.AllCells)
        Next

        lastColumnIndex = Me.DgvInsulin.Columns.Count - 1
        For i As Integer = 0 To lastColumnIndex
            Dim c As DataGridViewColumn = Me.DgvInsulin.Columns(i)
            c.Visible = Not InsulinHelpers.HideColumn(c.DataPropertyName)
        Next

        lastColumnIndex = Me.DgvMeal.Columns.Count - 1
        For i As Integer = 0 To lastColumnIndex
            Dim c As DataGridViewColumn = Me.DgvMeal.Columns()(i)
            c.AutoSizeMode = If(i = lastColumnIndex, DataGridViewAutoSizeColumnMode.Fill, DataGridViewAutoSizeColumnMode.AllCells)
        Next

        lastColumnIndex = Me.DgvSGs.Columns.Count - 1
        For i As Integer = 0 To lastColumnIndex
            Dim c As DataGridViewColumn = Me.DgvSGs.Columns()(i)
            c.Visible = Not SgHelpers.HideColumn(c.DataPropertyName)
            c.AutoSizeMode = If(c.HeaderText = "Sensor Message", DataGridViewAutoSizeColumnMode.Fill, DataGridViewAutoSizeColumnMode.AllCells)
        Next
    End Sub

    Private Sub MenuOptionsShowChartLegends_Click(sender As Object, e As EventArgs) Handles MenuOptionsShowChartLegends.Click
        Dim showLegend As Boolean = Me.MenuOptionsShowChartLegends.Checked
        _activeInsulinChartLegend.Enabled = showLegend
        _summaryChartLegend.Enabled = showLegend
        _treatmentMarkersChartLegend.Enabled = showLegend
        My.Settings.SystemShowLegends = showLegend
        My.Settings.Save()
    End Sub

    Private Sub MenuOptionsSpeechHelpShown_Click(sender As Object, e As EventArgs) Handles MenuOptionsSpeechHelpShown.Click
        If Me.MenuOptionsSpeechHelpShown.Checked Then

        End If
        My.Settings.SystemSpeechHelpShown = Me.MenuOptionsSpeechHelpShown.Checked
        My.Settings.Save()
    End Sub

    Private Sub MenuOptionsSpeechRecognition80_Click(sender As Object, e As EventArgs) Handles MenuOptionsSpeechRecognition80.Click
        My.Settings.SystemSpeechRecognitionThreshold = 0.8
        Me.SetSpeechRecognitionConfidenceThreshold()
        If Me.MenuOptionsSpeechRecognitionEnabled.Checked Then
            InitializeSpeechRecognition()
        Else
            CancelSpeechRecognition()
        End If
    End Sub

    Private Sub MenuOptionsSpeechRecognition85_Click(sender As Object, e As EventArgs) Handles MenuOptionsSpeechRecognition85.Click
        My.Settings.SystemSpeechRecognitionThreshold = 0.85
        Me.SetSpeechRecognitionConfidenceThreshold()
        If Me.MenuOptionsSpeechRecognitionEnabled.Checked Then
            InitializeSpeechRecognition()
        Else
            CancelSpeechRecognition()
        End If
    End Sub

    Private Sub MenuOptionsSpeechRecognition90_Click(sender As Object, e As EventArgs) Handles MenuOptionsSpeechRecognition90.Click
        My.Settings.SystemSpeechRecognitionThreshold = 0.9
        Me.SetSpeechRecognitionConfidenceThreshold()
        If Me.MenuOptionsSpeechRecognitionEnabled.Checked Then
            InitializeSpeechRecognition()
        Else
            CancelSpeechRecognition()
        End If
    End Sub

    Private Sub MenuOptionsSpeechRecognition95_Click(sender As Object, e As EventArgs) Handles MenuOptionsSpeechRecognition95.Click
        My.Settings.SystemSpeechRecognitionThreshold = 0.95
        Me.SetSpeechRecognitionConfidenceThreshold()
        If Me.MenuOptionsSpeechRecognitionEnabled.Checked Then
            InitializeSpeechRecognition()
        Else
            CancelSpeechRecognition()
        End If
    End Sub

    Private Sub MenuOptionsSpeechRecognitionDisabled_Click(sender As Object, e As EventArgs) Handles MenuOptionsSpeechRecognitionDisabled.Click
        My.Settings.SystemSpeechRecognitionThreshold = 1
        My.Settings.Save()
        Me.SetSpeechRecognitionConfidenceThreshold()
        If Me.MenuOptionsSpeechRecognitionEnabled.Checked Then
            InitializeSpeechRecognition()
        Else
            CancelSpeechRecognition()
        End If
    End Sub

    Private Sub MenuOptionsUseLocalTimeZone_Click(sender As Object, e As EventArgs) Handles MenuOptionsUseLocalTimeZone.Click
        Dim saveRequired As Boolean = Me.MenuOptionsUseLocalTimeZone.Checked <> My.Settings.UseLocalTimeZone
        If Me.MenuOptionsUseLocalTimeZone.Checked Then
            PumpTimeZoneInfo = TimeZoneInfo.Local
            My.Settings.UseLocalTimeZone = True
        Else
            PumpTimeZoneInfo = CalculateTimeZone(RecentData(NameOf(ServerDataIndexes.clientTimeZoneName)))
            My.Settings.UseLocalTimeZone = False
        End If
        If saveRequired Then My.Settings.Save()
    End Sub

    Private Sub SetSpeechRecognitionConfidenceThreshold()
        Select Case My.Settings.SystemSpeechRecognitionThreshold
            Case 1
                Me.MenuOptionsSpeechRecognitionDisabled.Checked = True
                Me.MenuOptionsSpeechRecognition80.Checked = False
                Me.MenuOptionsSpeechRecognition85.Checked = False
                Me.MenuOptionsSpeechRecognition90.Checked = False
                Me.MenuOptionsSpeechRecognition95.Checked = False
            Case 0.8
                Me.MenuOptionsSpeechRecognitionDisabled.Checked = False
                Me.MenuOptionsSpeechRecognition80.Checked = True
                Me.MenuOptionsSpeechRecognition85.Checked = False
                Me.MenuOptionsSpeechRecognition90.Checked = False
                Me.MenuOptionsSpeechRecognition95.Checked = False
            Case 0.85
                Me.MenuOptionsSpeechRecognitionDisabled.Checked = False
                Me.MenuOptionsSpeechRecognition80.Checked = False
                Me.MenuOptionsSpeechRecognition85.Checked = True
                Me.MenuOptionsSpeechRecognition90.Checked = False
                Me.MenuOptionsSpeechRecognition95.Checked = False
            Case 0.9
                Me.MenuOptionsSpeechRecognitionDisabled.Checked = False
                Me.MenuOptionsSpeechRecognition80.Checked = False
                Me.MenuOptionsSpeechRecognition85.Checked = False
                Me.MenuOptionsSpeechRecognition90.Checked = True
                Me.MenuOptionsSpeechRecognition95.Checked = False
            Case 0.95
                Me.MenuOptionsSpeechRecognitionDisabled.Checked = False
                Me.MenuOptionsSpeechRecognition80.Checked = False
                Me.MenuOptionsSpeechRecognition85.Checked = False
                Me.MenuOptionsSpeechRecognition90.Checked = False
                Me.MenuOptionsSpeechRecognition95.Checked = True
        End Select
        Me.MenuOptionsSpeechRecognitionEnabled.Checked = Me.MenuOptionsSpeechRecognitionDisabled.Checked = False
    End Sub

#End Region ' Option Menus

#Region "View Menu Events"

    Private Sub MenuShowMiniDisplay_Click(sender As Object, e As EventArgs) Handles MenuShowMiniDisplay.Click
        Me.Hide()
        _sgMiniDisplay.Show()
    End Sub

#End Region ' View Menu Events

#Region "Help Menu Events"

    Private Sub MenuHelpAbout_Click(sender As Object, e As EventArgs) Handles MenuHelpAbout.Click
        AboutBox1.ShowDialog(Me)
    End Sub

    Private Sub MenuHelpCheckForUpdates_Click(sender As Object, e As EventArgs) Handles MenuHelpCheckForUpdates.Click
        CheckForUpdatesAsync(True)
    End Sub

    Private Sub MenuHelpReportAnIssue_Click(sender As Object, e As EventArgs) Handles MenuHelpReportAnIssue.Click
        OpenUrlInBrowser($"{GitHubCareLinkUrl}issues")
    End Sub

#End Region ' Help Menu Events

#End Region 'Form Menu Events

#Region "NotifyIcon Events"

    Private Sub NotifyIcon1_DoubleClick(sender As Object, e As EventArgs) Handles NotifyIcon1.DoubleClick
        Me.ShowInTaskbar = True
        Me.WindowState = FormWindowState.Normal
    End Sub

#End Region

#Region "Settings Events"

    Private Sub MySettings_SettingChanging(sender As Object, e As SettingChangingEventArgs)
        If e.SettingName.StartsWith("System") Then Exit Sub

        Dim newValue As String = If(IsNothing(e.NewValue), "", e.NewValue.ToString)
        If My.Settings(e.SettingName).ToString.ToUpperInvariant.Equals(newValue.ToString.ToUpperInvariant, StringComparison.Ordinal) Then
            Exit Sub
        End If
        If e.SettingName = "CareLinkUserName" Then
            If s_allUserSettingsData?.ContainsKey(e.NewValue.ToString) Then
                Form1LoginHelpers.LoginDialog.LoggedOnUser = s_allUserSettingsData(e.NewValue.ToString)
                Exit Sub
            Else
                Dim userSettings As New CareLinkUserDataRecord(s_allUserSettingsData)
                userSettings.UpdateValue(e.SettingName, e.NewValue.ToString)
                s_allUserSettingsData.Add(userSettings)
            End If
        End If
        s_allUserSettingsData.SaveAllUserRecords(Form1LoginHelpers.LoginDialog.LoggedOnUser, e.SettingName, (e.NewValue?.ToString))
    End Sub

#End Region ' Settings Events

#Region "Summary Events"

    Private Sub CalibrationDueImage_MouseHover(sender As Object, e As EventArgs) Handles CalibrationDueImage.MouseHover
        If s_timeToNextCalibrationMinutes > 0 AndAlso s_timeToNextCalibrationMinutes < 1440 Then
            _calibrationToolTip.SetToolTip(Me.CalibrationDueImage, $"Calibration Due {PumpNow.AddMinutes(s_timeToNextCalibrationMinutes).ToShortTimeString}")
        End If
    End Sub

    Private Sub SensorDaysLeftLabel_MouseHover(sender As Object, e As EventArgs) Handles SensorDaysLeftLabel.MouseHover
        If s_sensorDurationHours < 24 Then
            _sensorLifeToolTip.SetToolTip(Me.CalibrationDueImage, $"Sensor will expire in {s_sensorDurationHours} hours")
        End If
    End Sub

#End Region ' Summary Events

#Region "Tab Events"

    Private Sub TabControlPage1_Selecting(sender As Object, e As TabControlCancelEventArgs) Handles TabControlPage1.Selecting

        Select Case e.TabPage.Name
            Case NameOf(TabPage14Markers)
                Me.DgvCareLinkUsers.InitializeDgv

                For Each c As DataGridViewColumn In Me.DgvCareLinkUsers.Columns
                    c.Visible = Not CareLinkUserDataRecordHelpers.HideColumn(c.DataPropertyName)
                Next
                Me.TabControlPage2.SelectedIndex = If(_lastMarkerTabIndex.page = 0,
                                                      0,
                                                      _lastMarkerTabIndex.tab
                                                     )
                Me.TabControlPage1.Visible = False
                Exit Sub
            Case NameOf(TabPage05Insulin)
                _lastMarkerTabIndex = (0, e.TabPageIndex)
            Case NameOf(TabPage06Meal)
                _lastMarkerTabIndex = (0, e.TabPageIndex)
        End Select
        _lastSummaryTabIndex = e.TabPageIndex
    End Sub

    Private Sub TabControlPage2_Selecting(sender As Object, e As TabControlCancelEventArgs) Handles TabControlPage2.Selecting
        Select Case e.TabPage.Name
            Case NameOf(TabPageBackToHomePage)
                Me.TabControlPage1.SelectedIndex = _lastSummaryTabIndex
                Me.TabControlPage1.Visible = True
                Exit Sub
            Case NameOf(TabPageAllUsers)
                Me.DgvCareLinkUsers.DataSource = s_allUserSettingsData
                For Each c As DataGridViewColumn In Me.DgvCareLinkUsers.Columns
                    c.Visible = Not CareLinkUserDataRecordHelpers.HideColumn(c.DataPropertyName)
                Next
            Case Else
                If e.TabPageIndex < Me.TabControlPage2.TabPages.Count - 2 Then
                    _lastMarkerTabIndex = (1, e.TabPageIndex)
                End If
        End Select
    End Sub

#End Region ' Tab Events

#Region "TableLayoutPanelTop Button Events"

    Private Sub TableLayoutPanelTopButton_Click(sender As Object, e As EventArgs) _
            Handles TableLayoutPanelActiveInsulinTop.ButtonClick,
                    TableLayoutPanelAutoBasalDeliveryTop.ButtonClick,
                    TableLayoutPanelAutoModeStatusTop.ButtonClick,
                    TableLayoutPanelBannerStateTop.ButtonClick,
                    TableLayoutPanelBasalTop.ButtonClick,
                    TableLayoutPanelCalibrationTop.ButtonClick,
                    TableLayoutPanelInsulinTop.ButtonClick,
                    TableLayoutPanelLastAlarmTop.ButtonClick,
                    TableLayoutPanelLastSgTop.ButtonClick,
                    TableLayoutPanelLimitsTop.ButtonClick,
                    TableLayoutPanelLowGlucoseSuspendedTop.ButtonClick,
                    TableLayoutPanelMealTop.ButtonClick,
                    TableLayoutPanelNotificationHistoryTop.ButtonClick,
                    TableLayoutPanelBgReadingsTop.ButtonClick,
                    TableLayoutPanelSgsTop.ButtonClick,
                    TableLayoutPanelTherapyAlgorithmTop.ButtonClick,
                    TableLayoutPanelTimeChangeTop.ButtonClick
        Me.TabControlPage1.Visible = True
        Dim topTable As TableLayoutPanelTopEx = CType(CType(sender, Button).Parent, TableLayoutPanelTopEx)
        Dim dgv As DataGridView = CType(Me.TabControlPage1.TabPages(3).Controls(0), DataGridView)
        Dim tabName As String = topTable.LabelText.Split(":")(0).Replace(" ", "")
        If tabName.Contains("Markers") Then
            tabName = "Markers"
        End If
        For Each row As DataGridViewRow In dgv.Rows
            If row.Cells(1).FormattedValue.ToString.Equals(tabName, StringComparison.OrdinalIgnoreCase) Then
                Me.TabControlPage1.SelectedIndex = 3
                dgv.CurrentCell = row.Cells(2)
                s_currentSummaryRow = row.Index
                Exit Sub
            End If
        Next
        Stop
    End Sub

#End Region ' TableLayoutPanelTop Button Events

#Region "Timer Events"

    Private Sub CursorTimer_Tick(sender As Object, e As EventArgs) Handles CursorTimer.Tick
        If Not Me.SummaryChart.ChartAreas(NameOf(ChartArea)).AxisX.ScaleView.IsZoomed Then
            Me.CursorTimer.Enabled = False
            Me.SummaryChart.ChartAreas(NameOf(ChartArea)).CursorX.Position = Double.NaN
        End If
    End Sub

    Private Sub PowerModeChanged(sender As Object, e As PowerModeChangedEventArgs)
        Debug.WriteLine($"PowerModeChange {e.Mode}")
        Select Case e.Mode
            Case PowerModes.Suspend
                StartOrStopServerUpdateTimer(False)
                s_shuttingDown = True
                SetLastUpdateTime("System Sleeping", "", True, Nothing)
            Case PowerModes.Resume
                SetLastUpdateTime("System Awake", "", True, Nothing)
                s_shuttingDown = False
                StartOrStopServerUpdateTimer(True, s_30SecondInMilliseconds \ 3)
                DebugPrint($"restarted after wake. {NameOf(ServerUpdateTimer)} started at {Now.ToLongTimeString}")
        End Select

    End Sub

    Private Sub ServerUpdateTimer_Tick(sender As Object, e As EventArgs) Handles ServerUpdateTimer.Tick
        StartOrStopServerUpdateTimer(False)
        Dim lastErrorMessage As String = ""
        SyncLock _updatingLock
            If Not _updating Then
                _updating = True
                lastErrorMessage = Client?.GetRecentData()
                If RecentDataEmpty() Then
                    If Client Is Nothing Then
                        Do While True
                            LoginDialog.LoginSourceAutomatic = FileToLoadOptions.Login
                            Dim result As DialogResult = LoginDialog.ShowDialog(My.Forms.Form1)
                            Select Case result
                                Case DialogResult.OK
                                    Exit Do
                                Case DialogResult.Cancel
                                    StartOrStopServerUpdateTimer(False)
                                    Return
                                Case DialogResult.Retry
                            End Select
                        Loop

                        Client = LoginDialog.Client
                    End If
                    lastErrorMessage = Client.GetRecentData()
                End If
                ReportLoginStatus(Me.LoginStatus, RecentDataEmpty, lastErrorMessage)

                Me.Cursor = Cursors.Default
                Application.DoEvents()
            End If
            _updating = False
        End SyncLock

        Dim lastMedicalDeviceDataUpdateServerEpochString As String = ""
        If Not RecentDataEmpty() Then
            If RecentData.TryGetValue(NameOf(ServerDataIndexes.lastMedicalDeviceDataUpdateServerTime), lastMedicalDeviceDataUpdateServerEpochString) Then
                If CLng(lastMedicalDeviceDataUpdateServerEpochString) = s_lastMedicalDeviceDataUpdateServerEpoch Then
                    Dim epochAsLocalDate As Date = lastMedicalDeviceDataUpdateServerEpochString.FromUnixTime.ToLocalTime
                    If epochAsLocalDate + s_05MinuteSpan < Now() Then
                        SetLastUpdateTime(Nothing, "", True, epochAsLocalDate.IsDaylightSavingTime)
                        _sgMiniDisplay.SetCurrentSgString("---", Single.NaN)
                    Else
                        SetLastUpdateTime(Nothing, "", False, epochAsLocalDate.IsDaylightSavingTime)
                        _sgMiniDisplay.SetCurrentSgString(s_lastSgRecord?.ToString, s_lastSgRecord.sg)
                    End If
                Else
                    Me.UpdateAllTabPages(False)
                End If
            Else
                Stop
            End If
        Else
            ReportLoginStatus(Me.LoginStatus, True, lastErrorMessage)
            _sgMiniDisplay.SetCurrentSgString("---", 0)
        End If
        StartOrStopServerUpdateTimer(True, s_1MinutesInMilliseconds)
    End Sub

#End Region ' Timer Events

#End Region ' Events

#Region "Initialize Charts"

#Region "Initialize Summary Charts"

    Friend Sub InitializeSummaryTabCharts()
        Me.SplitContainer3.Panel1.Controls.Clear()
        Me.SummaryChart = CreateChart(NameOf(SummaryChart))
        Dim summaryTitle As Title = CreateTitle(
            chartTitle:="Summary",
            name:=NameOf(summaryTitle),
            foreColor:=Me.SummaryChart.BackColor.GetContrastingColor())

        Dim summaryChartArea As ChartArea = CreateChartArea(Me.SummaryChart)
        Me.SummaryChart.ChartAreas.Add(summaryChartArea)
        _summaryChartLegend = CreateChartLegend(NameOf(_summaryChartLegend))

        Me.SummaryAutoCorrectionSeries = CreateSeriesBasal(
            SeriesName:=AutoCorrectionSeriesName,
            basalLegend:=_summaryChartLegend,
            legendText:="Auto Correction",
            YAxisType:=AxisType.Secondary)
        Me.SummaryBasalSeries = CreateSeriesBasal(BasalSeriesName, _summaryChartLegend, "Basal Series", AxisType.Secondary)
        Me.SummaryMinBasalSeries = CreateSeriesBasal(MinBasalSeriesName, _summaryChartLegend, "Min Basal", AxisType.Secondary)

        Me.SummaryHighLimitSeries = CreateSeriesLimitsAndTarget(_summaryChartLegend, HighLimitSeriesName)

        Me.SummarySuspendSeries = CreateSeriesSuspend(_summaryChartLegend)

        Me.SummaryTargetSgSeries = CreateSeriesLimitsAndTarget(_summaryChartLegend, TargetSgSeriesName)
        Me.SummarySgSeries = CreateSeriesSg(_summaryChartLegend)
        Me.SummaryLowLimitSeries = CreateSeriesLimitsAndTarget(_summaryChartLegend, LowLimitSeriesName)

        Me.SummaryMarkerSeries = CreateSeriesWithoutVisibleLegend(AxisType.Secondary)

        Me.SummaryTimeChangeSeries = CreateSeriesTimeChange(_summaryChartLegend)

        Me.SplitContainer3.Panel1.Controls.Add(Me.SummaryChart)
        Application.DoEvents()

        With Me.SummaryChart
            With .Series
                .Add(Me.SummarySuspendSeries)

                .Add(Me.SummaryHighLimitSeries)
                .Add(Me.SummaryTargetSgSeries)
                .Add(Me.SummaryLowLimitSeries)
                .Add(Me.SummaryTimeChangeSeries)

                .Add(Me.SummaryAutoCorrectionSeries)
                .Add(Me.SummaryBasalSeries)
                .Add(Me.SummaryMinBasalSeries)

                .Add(Me.SummarySgSeries)
                .Add(Me.SummaryMarkerSeries)

            End With
            With .Series(SgSeriesName).EmptyPointStyle
                .BorderWidth = 4
                .Color = Color.Transparent
            End With
            .Legends.Add(_summaryChartLegend)
            .Titles.Add(summaryTitle)
        End With
        Application.DoEvents()
    End Sub

    Friend Sub InitializeTimeInRangeArea()
        If Me.SplitContainer3.Panel2.Controls.Count > 16 Then
            Me.SplitContainer3.Panel2.Controls.RemoveAt(Me.SplitContainer3.Panel2.Controls.Count - 1)
        End If
        Dim width1 As Integer = Me.SplitContainer3.Panel2.Width - 94
        Dim splitPanelMidpoint As Integer = Me.SplitContainer3.Panel2.Width \ 2
        For Each control1 As Control In Me.SplitContainer3.Panel2.Controls
            Select Case control1.Name
                Case NameOf(Me.LowTirComplianceLabel)
                    Me.LowTirComplianceLabel.Left = splitPanelMidpoint - Me.LowTirComplianceLabel.Width
                Case NameOf(Me.HighTirComplianceLabel)
                    Me.HighTirComplianceLabel.Left = splitPanelMidpoint
                Case Else
                    DirectCast(control1, Label).AutoSize = True
                    control1.Left = splitPanelMidpoint - (control1.Width \ 2)
            End Select
        Next
        Me.TimeInRangeChart = New Chart With {
            .Anchor = AnchorStyles.Top,
            .BackColor = Color.Transparent,
            .BackGradientStyle = GradientStyle.None,
            .BackSecondaryColor = Color.Transparent,
            .BorderlineColor = Color.Transparent,
            .BorderlineWidth = 0,
            .Size = New Size(width1,
                             width1)
                            }

        With Me.TimeInRangeChart
            .BorderSkin.BackSecondaryColor = Color.Transparent
            .BorderSkin.SkinStyle = BorderSkinStyle.None
            Dim timeInRangeChartArea As New ChartArea With {
                    .Name = NameOf(timeInRangeChartArea),
                    .BackColor = Color.Black
                }
            .ChartAreas.Add(timeInRangeChartArea)
            .Location = New Point(Me.TimeInRangeChartLabel.FindHorizontalMidpoint - (.Width \ 2),
                                  CInt(Me.TimeInRangeChartLabel.FindVerticalMidpoint() - Math.Round(.Height / 2.5)))
            .Name = NameOf(TimeInRangeChart)
            Me.TimeInRangeSeries = New Series(NameOf(TimeInRangeSeries)) With {
                    .ChartArea = NameOf(timeInRangeChartArea),
                    .ChartType = SeriesChartType.Doughnut
                }
            .Series.Add(Me.TimeInRangeSeries)
            .Series(NameOf(TimeInRangeSeries))("DoughnutRadius") = "17"
        End With

        Me.SplitContainer3.Panel2.Controls.Add(Me.TimeInRangeChart)
        Application.DoEvents()
    End Sub

#End Region ' Initialize Home Tab Charts

#Region "Initialize Chart Tabs"

#Region "Initialize Active Insulin Chart"

    Friend Sub InitializeActiveInsulinTabChart()
        Me.SplitContainer1.Panel2.Controls.Clear()

        Me.ActiveInsulinChart = CreateChart(NameOf(ActiveInsulinChart))
        Dim activeInsulinChartArea As ChartArea = CreateChartArea(Me.ActiveInsulinChart)
        Dim labelColor As Color = Me.ActiveInsulinChart.BackColor.GetContrastingColor()
        Dim labelFont As New Font("Segoe UI", 12.0F, FontStyle.Bold)

        With activeInsulinChartArea.AxisY
            .Interval = 2
            .IsInterlaced = False
            With .LabelStyle
                .Font = labelFont
                .ForeColor = labelColor
                .Format = "{0}"
            End With
            With .MajorTickMark
                .Interval = 4
                .Enabled = False
            End With
            .Maximum = 25
            .Minimum = 0
            .Title = "Active Insulin"
            .TitleFont = New Font(labelFont.FontFamily, 14)
            .TitleForeColor = labelColor
        End With
        Me.ActiveInsulinChart.ChartAreas.Add(activeInsulinChartArea)
        _activeInsulinChartLegend = CreateChartLegend(NameOf(_activeInsulinChartLegend))
        Me.ActiveInsulinChartTitle = CreateTitle($"Running Insulin On Board (IOB)",
                                                 NameOf(ActiveInsulinChartTitle),
                                                 GetGraphLineColor("Active Insulin"))
        Me.ActiveInsulinActiveInsulinSeries = CreateSeriesActiveInsulin()
        Me.ActiveInsulinTargetSeries = CreateSeriesLimitsAndTarget(_activeInsulinChartLegend, TargetSgSeriesName)

        Me.ActiveInsulinAutoCorrectionSeries = CreateSeriesBasal(AutoCorrectionSeriesName, _activeInsulinChartLegend, "Auto Correction", AxisType.Secondary)
        Me.ActiveInsulinBasalSeries = CreateSeriesBasal(BasalSeriesName, _activeInsulinChartLegend, "Basal Series", AxisType.Secondary)
        Me.ActiveInsulinMinBasalSeries = CreateSeriesBasal(MinBasalSeriesName, _activeInsulinChartLegend, "Min Basal", AxisType.Secondary)

        Me.ActiveInsulinSuspendSeries = CreateSeriesSuspend(_activeInsulinChartLegend)

        Me.ActiveInsulinSgSeries = CreateSeriesSg(_activeInsulinChartLegend)
        Me.ActiveInsulinMarkerSeries = CreateSeriesWithoutVisibleLegend(AxisType.Secondary)
        Me.ActiveInsulinTimeChangeSeries = CreateSeriesTimeChange(_activeInsulinChartLegend)

        With Me.ActiveInsulinChart
            With .Series
                .Add(Me.ActiveInsulinTargetSeries)
                .Add(Me.ActiveInsulinTimeChangeSeries)

                .Add(Me.ActiveInsulinActiveInsulinSeries)

                .Add(Me.ActiveInsulinAutoCorrectionSeries)
                .Add(Me.ActiveInsulinBasalSeries)
                .Add(Me.ActiveInsulinMinBasalSeries)

                .Add(Me.ActiveInsulinSgSeries)
                .Add(Me.ActiveInsulinSuspendSeries)
                .Add(Me.ActiveInsulinMarkerSeries)
            End With
            .Series(SgSeriesName).EmptyPointStyle.BorderWidth = 4
            .Series(SgSeriesName).EmptyPointStyle.Color = Color.Transparent
            .Series(ActiveInsulinSeriesName).EmptyPointStyle.BorderWidth = 4
            .Series(ActiveInsulinSeriesName).EmptyPointStyle.Color = Color.Transparent
            .Legends.Add(_activeInsulinChartLegend)
        End With

        Me.ActiveInsulinChart.Titles.Add(Me.ActiveInsulinChartTitle)
        Me.SplitContainer1.Panel2.Controls.Add(Me.ActiveInsulinChart)
        Application.DoEvents()

    End Sub

#End Region ' Initialize Active Insulin Chart

#Region "Initialize Treatment Markers Chart"

    Private Sub InitializeTreatmentMarkersChart()
        Me.TabPage03TreatmentDetails.Controls.Clear()

        Me.TreatmentMarkersChart = CreateChart(NameOf(TreatmentMarkersChart))
        Dim treatmentMarkersChartArea As ChartArea = CreateChartArea(Me.TreatmentMarkersChart)

        Select Case MaxBasalPerDose
            Case < 0.5
                TreatmentInsulinRow = 0.5
            Case < 1
                TreatmentInsulinRow = 1
            Case < 1.5
                TreatmentInsulinRow = 1.5
            Case < 2
                TreatmentInsulinRow = 2
            Case Else
                TreatmentInsulinRow = (MaxBasalPerDose + 0.025!).RoundTo025
        End Select

        Dim labelColor As Color = Me.TreatmentMarkersChart.BackColor.GetContrastingColor()
        Dim labelFont As New Font("Segoe UI", 12.0F, FontStyle.Bold)

        With treatmentMarkersChartArea.AxisY
            Dim interval As Single = (TreatmentInsulinRow / 10).RoundSingle(3, False)
            .Interval = interval
            .IsInterlaced = False
            .IsMarginVisible = False
            With .LabelStyle
                .Font = labelFont
                .ForeColor = labelColor
                .Format = $"{{0{CurrentUICulture.NumberFormat.NumberDecimalSeparator}00}}"
            End With
            .LineColor = Color.FromArgb(64, labelColor)
            With .MajorTickMark
                .Enabled = True
                .Interval = interval
                .LineColor = Color.FromArgb(64, labelColor)
            End With
            .Maximum = TreatmentInsulinRow
            .Minimum = 0
            .Title = "Delivered Insulin"
            .TitleFont = New Font(labelFont.FontFamily, 14)
            .TitleForeColor = labelColor
        End With

        Me.TreatmentMarkersChart.ChartAreas.Add(treatmentMarkersChartArea)
        _treatmentMarkersChartLegend = CreateChartLegend(NameOf(_treatmentMarkersChartLegend))

        Me.TreatmentMarkersChartTitle = CreateTitle(
            chartTitle:="Treatment Details",
            name:=NameOf(TreatmentMarkersChartTitle),
            foreColor:=Me.TreatmentMarkersChart.BackColor.GetContrastingColor())
        Me.TreatmentTargetSeries = CreateSeriesLimitsAndTarget(
            limitsLegend:=_treatmentMarkersChartLegend,
            seriesName:=TargetSgSeriesName)
        Me.TreatmentMarkerAutoCorrectionSeries = CreateSeriesBasal(
            SeriesName:=AutoCorrectionSeriesName,
            basalLegend:=_treatmentMarkersChartLegend,
            legendText:="Auto Correction",
            YAxisType:=AxisType.Primary)
        Me.TreatmentMarkerBasalSeries = CreateSeriesBasal(
            SeriesName:=BasalSeriesName,
            basalLegend:=_treatmentMarkersChartLegend,
            legendText:="Basal Series",
            YAxisType:=AxisType.Primary)
        Me.TreatmentMarkerMinBasalSeries = CreateSeriesBasal(
            SeriesName:=MinBasalSeriesName,
            basalLegend:=_treatmentMarkersChartLegend,
            legendText:="Min Basal",
            YAxisType:=AxisType.Primary)

        Me.TreatmentMarkerSgSeries = CreateSeriesSg(_treatmentMarkersChartLegend)
        Me.TreatmentMarkerMarkersSeries = CreateSeriesWithoutVisibleLegend(AxisType.Primary)
        Me.TreatmentMarkerTimeChangeSeries = CreateSeriesTimeChange(_treatmentMarkersChartLegend)
        Me.TreatmentMarkerSuspendSeries = CreateSeriesSuspend(_treatmentMarkersChartLegend)

        With Me.TreatmentMarkersChart
            With .Series
                .Add(Me.TreatmentTargetSeries)
                .Add(Me.TreatmentMarkerSuspendSeries)
                .Add(Me.TreatmentMarkerTimeChangeSeries)

                .Add(Me.TreatmentMarkerAutoCorrectionSeries)
                .Add(Me.TreatmentMarkerBasalSeries)
                .Add(Me.TreatmentMarkerMinBasalSeries)

                .Add(Me.TreatmentMarkerSgSeries)
                .Add(Me.TreatmentMarkerMarkersSeries)
            End With
            .Legends.Add(_treatmentMarkersChartLegend)
            .Series(SgSeriesName).EmptyPointStyle.Color = Color.Transparent
            .Series(SgSeriesName).EmptyPointStyle.BorderWidth = 4
            .Series(BasalSeriesName).EmptyPointStyle.Color = Color.Transparent
            .Series(BasalSeriesName).EmptyPointStyle.BorderWidth = 4
            .Series(MarkerSeriesName).EmptyPointStyle.Color = Color.Transparent
            .Series(MarkerSeriesName).EmptyPointStyle.BorderWidth = 4
        End With

        Me.TreatmentMarkersChart.Titles.Add(Me.TreatmentMarkersChartTitle)
        Me.TabPage03TreatmentDetails.Controls.Add(Me.TreatmentMarkersChart)
        Application.DoEvents()
    End Sub

#End Region ' Initialize Treatment Markers Chart

#End Region ' Initialize Chart Tabs

#End Region ' Initialize Charts

#Region "NotifyIcon Support"

    Private Sub CleanUpNotificationIcon()
        Me.NotifyIcon1?.Dispose()
    End Sub

    <DebuggerNonUserCode()>
    Private Sub MenuOptions_DropDownOpening(sender As Object, e As EventArgs) Handles MenuOptions.DropDownOpening
        Me.MenuOptionsEditPumpSettings.Enabled = Debugger.IsAttached OrElse Not String.IsNullOrWhiteSpace(CurrentUser?.UserName)
    End Sub

    Private Sub UpdateNotifyIcon(lastSgString As String)
        Try
            Dim sg As Single = s_lastSgRecord.sg

            Using bitmapText As New Bitmap(16, 16)
                Using g As Graphics = Graphics.FromImage(bitmapText)
                    Dim backColor As Color
                    Select Case sg
                        Case <= TirLowLimit(NativeMmolL)
                            backColor = Color.Yellow
                            If _showBalloonTip Then
                                Me.NotifyIcon1.ShowBalloonTip(10000, $"CareLink™ Alert", $"SG below {TirLowLimitAsString(NativeMmolL)} {SgUnitsNativeString}", Me.ToolTip1.ToolTipIcon)
                            End If
                            _showBalloonTip = False
                        Case <= TirHighLimit(NativeMmolL)
                            backColor = Color.Green
                            _showBalloonTip = True
                        Case Else
                            backColor = Color.Red
                            If _showBalloonTip Then
                                Me.NotifyIcon1.ShowBalloonTip(
                                    timeout:=10000,
                                    tipTitle:=$"CareLink™ Alert",
                                    tipText:=$"SG above {TirHighLimitAsString(NativeMmolL)} {SgUnitsNativeString}",
                                    tipIcon:=Me.ToolTip1.ToolTipIcon)
                            End If
                            _showBalloonTip = False
                    End Select

                    Me.NotifyIcon1.Icon = CreateTextIcon(lastSgString.PadRight(totalWidth:=3).Substring(startIndex:=0, length:=3).Trim.PadLeft(totalWidth:=3), backColor)
                    Dim notStr As New StringBuilder(100)
                    notStr.AppendLine(Date.Now().ToShortDateTimeString.Replace($"{CultureInfo.CurrentUICulture.DateTimeFormat.DateSeparator}{Now.Year}", ""))
                    notStr.AppendLine($"Last SG {lastSgString} {SgUnitsNativeString}")
                    If s_pumpInRangeOfPhone Then
                        If s_lastSgValue = 0 Then
                            Me.LabelTrendValue.Text = ""
                            Me.LabelTrendValue.Visible = False
                            _sgMiniDisplay.SetCurrentDeltaValue(deltaString:="", delta:=0)
                        Else
                            Dim diffSg As Single = sg - s_lastSgValue
                            Me.LabelTrendValue.Text = If(Math.Abs(diffSg) < 0.001,
                                                         "0",
                                                         diffSg.ToString(GetSgFormat(withSign:=True), CultureInfo.InvariantCulture)
                                                        )
                            _sgMiniDisplay.SetCurrentDeltaValue(Me.LabelTrendValue.Text, diffSg)
                            Me.LabelTrendValue.ForeColor = backColor
                            notStr.AppendLine($"SG Trend { diffSg.ToString(GetSgFormat(True), CultureInfo.InvariantCulture)}")
                            Me.LabelTrendValue.Visible = True
                        End If
                    Else
                        Me.LabelTrendValue.Visible = False
                    End If
                    notStr.Append($"Active ins. {s_activeInsulin.amount:N3} U")
                    Me.NotifyIcon1.Text = notStr.ToString
                    Me.NotifyIcon1.Visible = True
                    s_lastSgValue = sg
                End Using
            End Using
        Catch ex As Exception
            Stop
            ' ignore errors
        End Try
    End Sub

#End Region 'NotifyIcon Support

#Region "Scale Split Containers"

    Private Sub Fix(sp As SplitContainer)
        ' Scale factor depends on orientation
        Dim sc As Single = If(sp.Orientation = Orientation.Vertical, _formScale.Width, _formScale.Height)
        If sp.FixedPanel = FixedPanel.Panel1 Then
            sp.SplitterDistance = CInt(Math.Truncate(Math.Round(sp.SplitterDistance * sc)))
        ElseIf sp.FixedPanel = FixedPanel.Panel2 Then
            Dim cs As Integer = If(sp.Orientation = Orientation.Vertical, sp.Panel2.ClientSize.Width, sp.Panel2.ClientSize.Height)
            sp.SplitterDistance -= CInt(Math.Truncate(cs * sc)) - cs
        End If
    End Sub

    ' Save the current scale value
    ' ScaleControl() is called during the Form'AiTimeInterval constructor
    Protected Overrides Sub ScaleControl(factor As SizeF, specified As BoundsSpecified)
        _formScale = New SizeF(_formScale.Width * factor.Width, _formScale.Height * factor.Height)
        MyBase.ScaleControl(factor, specified)
    End Sub

    ' Recursively search for SplitContainer controls
    Private Sub Fix(c As Control)
        For Each child As Control In c.Controls
            If TypeOf child Is SplitContainer Then
                Dim sp As SplitContainer = CType(child, SplitContainer)
                Me.Fix(sp)
                Me.Fix(sp.Panel1)
                Me.Fix(sp.Panel2)
            Else
                Me.Fix(child)
            End If
        Next child
    End Sub

#End Region ' Scale Split Containers

#Region "Update Home Tab"

    Private Sub UpdateActiveInsulin()
        Try
            If s_activeInsulin.amount >= 0 Then
                Dim activeInsulinStr As String = $"{s_activeInsulin.amount:N3}"
                If activeInsulinStr.ToCharArray.Last = "0" Then
                    activeInsulinStr = $"{s_activeInsulin.amount:N2}"
                End If
                Me.ActiveInsulinValue.Text = $"Active Insulin {activeInsulinStr} U"
                _sgMiniDisplay.ActiveInsulinTextBox.Text = $"Active Insulin {activeInsulinStr} U"
            Else
                Me.ActiveInsulinValue.Text = $"Active Insulin Unknown"
                _sgMiniDisplay.ActiveInsulinTextBox.Text = $"Active Insulin --- U"
            End If
        Catch ex As ArithmeticException
            Stop
            Throw New ArithmeticException($"{ex.DecodeException()} exception in {NameOf(UpdateActiveInsulin)}")
        Catch ex1 As Exception
            Stop
            Throw New Exception($"{ex1.DecodeException()} exception in {NameOf(UpdateActiveInsulin)}")
        End Try
    End Sub

    Private Sub UpdateActiveInsulinChart()
        If Not ProgramInitialized Then
            Exit Sub
        End If

        Try
            Me.TemporaryUseAdvanceAITDecayCheckBox.Checked = CurrentUser.UseAdvancedAitDecay = CheckState.Checked
            For Each s As Series In Me.ActiveInsulinChart?.Series
                s.Points.Clear()
            Next
            With Me.ActiveInsulinChart
                .Titles(NameOf(ActiveInsulinChartTitle)).Text = $"Running Insulin On Board (IOB) - {s_listOfManualBasal.GetSubTitle}"
                .ChartAreas(NameOf(ChartArea)).UpdateChartAreaSgAxisX()

                ' Order all markers by time
                Dim timeOrderedMarkers As New SortedDictionary(Of OADate, Single)

                Dim lastTimeChangeRecord As TimeChange = Nothing
                If s_markers.Count = 0 Then
                    Exit Sub
                End If

                For Each markerWithIndex As IndexClass(Of Marker) In s_markers.WithIndex()
                    Dim markerEntry As Marker = markerWithIndex.Value
                    Dim markerOADateTime As New OADate(markerEntry.GetMarkerTimestamp)
                    Select Case markerEntry.Type
                        Case "AUTO_BASAL_DELIVERY"
                            Dim bolusAmount As Single = markerEntry.GetSingleValueFromJson(NameOf(AutoBasalDelivery.bolusAmount))
                            If timeOrderedMarkers.ContainsKey(markerOADateTime) Then
                                timeOrderedMarkers(markerOADateTime) += bolusAmount
                            Else
                                timeOrderedMarkers.Add(markerOADateTime, bolusAmount)
                            End If
                        Case "MANUAL_BASAL_DELIVERY"
                            Dim bolusAmount As Single = markerEntry.GetSingleValueFromJson(NameOf(AutoBasalDelivery.bolusAmount))
                            If timeOrderedMarkers.ContainsKey(markerOADateTime) Then
                                timeOrderedMarkers(markerOADateTime) += bolusAmount
                            Else
                                timeOrderedMarkers.Add(markerOADateTime, bolusAmount)
                            End If
                        Case "INSULIN"
                            Dim bolusAmount As Single = markerEntry.GetSingleValueFromJson(NameOf(Insulin.deliveredFastAmount))
                            If timeOrderedMarkers.ContainsKey(markerOADateTime) Then
                                timeOrderedMarkers(markerOADateTime) += bolusAmount
                            Else
                                timeOrderedMarkers.Add(markerOADateTime, bolusAmount)
                            End If
                        Case "LOW_GLUCOSE_SUSPENDED"
                            If s_pumpInRangeOfTransmitter AndAlso CurrentPdf?.IsValid AndAlso Not InAutoMode Then
                                For Each kvp As KeyValuePair(Of OADate, Single) In GetManualBasalValues(markerWithIndex)
                                    If timeOrderedMarkers.ContainsKey(kvp.Key) Then
                                        timeOrderedMarkers(kvp.Key) += kvp.Value
                                    Else
                                        timeOrderedMarkers.Add(kvp.Key, kvp.Value)
                                    End If
                                Next
                            End If
                        Case "BG_READING"
                        Case "CALIBRATION"
                        Case "MEAL"
                        Case "TIME_CHANGE"
                        Case Else
                            Stop
                    End Select
                Next
                ' set up table that holds active insulin for every 5 minutes
                Dim remainingInsulinList As New List(Of RunningActiveInsulin)
                Dim currentMarker As Integer = 0

                For i As Integer = 0 To 287
                    Dim initialBolus As Single = 0
                    Dim firstNotSkippedOaTime As New OADate((s_listOfSgRecords(0).Timestamp + (s_05MinuteSpan * i)).RoundDownToMinute())
                    While currentMarker < timeOrderedMarkers.Count AndAlso timeOrderedMarkers.Keys(currentMarker) <= firstNotSkippedOaTime
                        initialBolus += timeOrderedMarkers.Values(currentMarker)
                        currentMarker += 1
                    End While
                    remainingInsulinList.Add(New RunningActiveInsulin(firstNotSkippedOaTime, initialBolus, CurrentUser))
                Next

                .ChartAreas(NameOf(ChartArea)).AxisY2.Maximum = GetYMaxValue(NativeMmolL)
                ' walk all markers, adjust active insulin and then add new markerWithIndex
                Dim maxActiveInsulin As Double = 0
                For i As Integer = 0 To remainingInsulinList.Count - 1
                    If i < CurrentUser.GetActiveInsulinIncrements Then
                        With Me.ActiveInsulinActiveInsulinSeries
                            .Points.AddXY(remainingInsulinList(i).OaDateTime, Double.NaN)
                            .Points.Last.IsEmpty = True
                        End With
                        If i > 0 Then
                            remainingInsulinList.AdjustList(0, i)
                        End If
                        Continue For
                    End If
                    Dim startIndex As Integer = i - CurrentUser.GetActiveInsulinIncrements + 1
                    Dim sum As Double = remainingInsulinList.ConditionalSum(startIndex, CurrentUser.GetActiveInsulinIncrements)
                    maxActiveInsulin = Math.Max(sum, maxActiveInsulin)
                    Me.ActiveInsulinActiveInsulinSeries.Points.AddXY(remainingInsulinList(i).OaDateTime, sum)
                    remainingInsulinList.AdjustList(startIndex, CurrentUser.GetActiveInsulinIncrements)
                Next

                .ChartAreas(NameOf(ChartArea)).AxisY.Maximum = Math.Ceiling(maxActiveInsulin) + 1
                .PlotSuspendArea(SuspendSeries:=Me.ActiveInsulinSuspendSeries)
                .PlotMarkers(
                    timeChangeSeries:=Me.ActiveInsulinTimeChangeSeries,
                    markerInsulinDictionary:=s_activeInsulinMarkerInsulinDictionary,
                    markerMealDictionary:=Nothing)
                .PlotSgSeries(GetYMinValue(NativeMmolL))
                .PlotHighLowLimitsAndTargetSg(True)
            End With
            Application.DoEvents()
        Catch ex As Exception
            Stop
            Throw New Exception($"{ex.DecodeException()} exception in {NameOf(UpdateActiveInsulinChart)}")
        End Try
    End Sub

    Private Sub UpdateAllSummarySeries()
        Try
            With Me.SummaryChart

                For Each s As Series In .Series
                    s.Points.Clear()
                Next
                .ChartAreas(NameOf(ChartArea)).UpdateChartAreaSgAxisX()
                .Titles(0).Text = $"Status - {s_listOfManualBasal.GetSubTitle}"
                .PlotSuspendArea(SuspendSeries:=Me.SummarySuspendSeries)
                .PlotMarkers(
                    timeChangeSeries:=Me.SummaryTimeChangeSeries,
                    markerInsulinDictionary:=s_summaryMarkerInsulinDictionary,
                    markerMealDictionary:=s_summaryMarkerMealDictionary)
                .PlotSgSeries(GetYMinValue(NativeMmolL))
                .PlotHighLowLimitsAndTargetSg(False)
                Application.DoEvents()
            End With
        Catch ex As Exception
            Stop
            Throw New Exception($"{ex.DecodeException()} exception while plotting Markers in {NameOf(UpdateAllSummarySeries)}")
        End Try

    End Sub

    Private Sub UpdateAutoModeShield()
        Try
            Me.LastSgOrExitTimeLabel.Text = s_lastSgRecord.Timestamp.ToShortTimeString
            Me.LastSgOrExitTimeLabel.BackColor = Color.Transparent
            Me.ShieldUnitsLabel.BackColor = Color.Transparent
            Me.ShieldUnitsLabel.Text = SgUnitsNativeString

            If InAutoMode Then
                Select Case s_sensorState
                    Case "CALIBRATING"
                        Me.SmartGuardShieldPictureBox.Image = My.Resources.Shield
                    Case "CALIBRATION_REQUIRED"
                        Me.SmartGuardShieldPictureBox.Image = My.Resources.Shield_Disabled
                    Case "NO_ERROR_MESSAGE"
                        Me.SmartGuardShieldPictureBox.Image = My.Resources.Shield
                    Case "WARM_UP"
                        Me.SmartGuardShieldPictureBox.Image = My.Resources.Shield_Disabled
                    Case "UNKNOWN"
                    Case Else
                        Me.SmartGuardShieldPictureBox.Image = My.Resources.Shield
                End Select
                Me.ShieldUnitsLabel.Visible = True
                Me.LastSgOrExitTimeLabel.Visible = True
            Else
                Me.SmartGuardShieldPictureBox.Image = Nothing
                Me.ShieldUnitsLabel.Visible = s_sensorState = "NO_ERROR_MESSAGE"
                Me.LastSgOrExitTimeLabel.Visible = False
            End If

            Dim message As String = ""
            If Not Single.IsNaN(s_lastSgRecord.sg) Then
                Me.CurrentSgLabel.Visible = True
                Dim sgString As String
                If s_autoModeReadinessState?.Value = "CALIBRATING" Then
                    ' TODO
                    sgString = s_lastSgRecord.ToString
                    Me.CurrentSgLabel.Text = sgString
                Else
                    sgString = s_lastSgRecord.ToString
                    Me.CurrentSgLabel.Text = sgString
                End If
                Me.UpdateNotifyIcon(sgString)
                _sgMiniDisplay.SetCurrentSgString(sgString, s_lastSgRecord.sg)
                Me.SensorMessageLabel.Visible = False
                If s_sensorMessages.TryGetValue(s_sensorState, message) Then
                    Dim splitMessage As String = message.Split(".")(0)
                    message = If(message.Contains("..."), $"{splitMessage}...", splitMessage)
                End If
            Else
                _sgMiniDisplay.SetCurrentSgString("---", s_lastSgRecord.sg)
                Me.CurrentSgLabel.Visible = False
                Me.LastSgOrExitTimeLabel.Visible = False
                Me.SensorMessageLabel.Visible = True
                Me.SensorMessageLabel.BackColor = Color.Transparent
                If s_sensorMessages.TryGetValue(s_sensorState, message) Then
                    Dim splitMessage As String = message.Split(".")(0)
                    message = If(message.Contains("..."), $"{splitMessage}...", splitMessage)
                Else
                    If Debugger.IsAttached Then
                        Stop
                        MsgBox($"{s_sensorState} is unknown sensor message", "", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, GetTitleFromStack(New StackFrame(0, True)))
                    End If

                    message = message.ToTitle
                End If

                Select Case s_sensorState
                    Case "UNKNOWN"
                        Me.SensorMessageLabel.Text = message
                    Case "WARM_UP"
                        Dim timeRemaining As String = ""
                        If s_systemStatusTimeRemaining.TotalMilliseconds > 0 Then
                            If s_systemStatusTimeRemaining.Hours > 1 Then
                                timeRemaining = $"{s_systemStatusTimeRemaining.Hours}:{s_systemStatusTimeRemaining.Minutes:D2} hrs"
                            ElseIf s_systemStatusTimeRemaining.Hours > 0 Then
                                timeRemaining = $"{s_systemStatusTimeRemaining.Hours}:{s_systemStatusTimeRemaining.Minutes:D2} hr"
                            ElseIf s_systemStatusTimeRemaining.Minutes > 1 Then
                                timeRemaining = $"{s_systemStatusTimeRemaining.Minutes} mins"
                            Else
                                timeRemaining = $"{s_systemStatusTimeRemaining.Minutes} min"
                            End If
                            Me.SensorMessageLabel.Text = $"{message.Replace("...", "")}{vbCrLf}{timeRemaining}"
                        Else
                            Me.SensorMessageLabel.Text = message
                        End If
                    Case "CALIBRATION_REQUIRED"
                    Case Else
                        Me.SensorMessageLabel.Text = message
                End Select
                Me.SensorMessageLabel.Visible = True
                Me.ShieldUnitsLabel.Visible = False
                Application.DoEvents()
            End If
        Catch ex As Exception
            Stop
            Throw New Exception($"{ex.DecodeException()} exception in {NameOf(UpdateAutoModeShield)}")
        End Try
        Application.DoEvents()
    End Sub

    Private Sub UpdateCalibrationTimeRemaining()
        Try
            If s_pumpInRangeOfPhone Then
                If s_timeToNextCalibrationHours > Byte.MaxValue Then
                    Me.CalibrationDueImage.Image = My.Resources.CalibrationDot.DrawCenteredArc(720)
                ElseIf s_timeToNextCalibrationHours = 0 Then
                    Me.CalibrationDueImage.Image = If(s_systemStatusMessage = "WAIT_TO_CALIBRATE" OrElse s_sensorState = "WARM_UP" OrElse s_sensorState = "CHANGE_SENSOR",
                    My.Resources.CalibrationNotReady,
                    My.Resources.CalibrationDotRed.DrawCenteredArc(s_timeToNextCalibrationMinutes))
                ElseIf s_timeToNextCalibrationMinutes = -1 Then
                    Me.CalibrationDueImage.Image = Nothing
                Else
                    Me.CalibrationDueImage.Image = My.Resources.CalibrationDot.DrawCenteredArc(s_timeToNextCalibrationMinutes)
                End If
            End If
            Me.CalibrationDueImage.Visible = s_pumpInRangeOfPhone
        Catch ex As Exception
            Stop
            Throw New Exception($"{ex.DecodeException()} exception in {NameOf(UpdateCalibrationTimeRemaining)}")
        End Try

        Application.DoEvents()
    End Sub

    Private Sub UpdateDosingAndCarbs()
        s_totalAutoCorrection = 0
        s_totalBasal = 0
        s_totalCarbs = 0
        s_totalDailyDose = 0
        s_totalManualBolus = 0

        For Each markerWithIndex As IndexClass(Of Marker) In s_markers.WithIndex()
            Dim marker As Marker = markerWithIndex.Value
            Select Case marker.Type
                Case "INSULIN"
                    Dim deliveredAmount As String = marker.GetSingleValueFromJson(NameOf(Insulin.deliveredFastAmount)).ToString
                    s_totalDailyDose += deliveredAmount.ParseSingle(3)
                    Select Case marker.GetStringValueFromJson(NameOf(Insulin.activationType))
                        Case "AUTOCORRECTION"
                            s_totalAutoCorrection += deliveredAmount.ParseSingle(3)
                        Case "MANUAL", "RECOMMENDED", "UNDETERMINED"
                            s_totalManualBolus += deliveredAmount.ParseSingle(3)
                    End Select

                Case "AUTO_BASAL_DELIVERY"
                    Dim amount As Single = marker.GetSingleValueFromJson(NameOf(AutoBasalDelivery.bolusAmount), 3)
                    s_totalBasal += amount
                    s_totalDailyDose += amount
                Case "MANUAL_BASAL_DELIVERY"
                    Dim amount As Single = marker.GetSingleValueFromJson(NameOf(AutoBasalDelivery.bolusAmount), 3)
                    s_totalBasal += amount
                    s_totalDailyDose += amount
                Case "MEAL"
                    s_totalCarbs += CInt(marker.GetSingleValueFromJson("amount"))
                Case "CALIBRATION"
                    ' IGNORE HERE
                Case "BG_READING"
                    ' IGNORE HERE
                Case "LOW_GLUCOSE_SUSPENDED"
                    ' IGNORE HERE
                Case "TIME_CHANGE"
                    ' IGNORE HERE
                Case Else
                    Stop
            End Select
        Next

        If s_totalBasal = 0 AndAlso CurrentPdf?.IsValid Then
            Dim activeBasalRecords As List(Of BasalRateRecord) = GetActiveBasalRateRecords()

            If activeBasalRecords.Count > 0 Then
                Dim startTime As TimeOnly
                Dim endTime As TimeOnly
                If activeBasalRecords.Count = 1 Then
                    s_totalBasal = activeBasalRecords(0).UnitsPerHr * 24
                    s_totalDailyDose += s_totalBasal
                Else
                    For Each e As IndexClass(Of BasalRateRecord) In activeBasalRecords.WithIndex
                        Dim basalRate As BasalRateRecord = e.Value
                        startTime = basalRate.Time
                        endTime = If(e.IsLast,
                                     New TimeOnly(23, 59, 59, 999),
                                     activeBasalRecords(e.Index + 1).Time
                                    )
                        Dim theTimeSpan As TimeSpan = endTime - startTime
                        s_totalBasal += CSng((theTimeSpan.Hours + (theTimeSpan.Minutes / 60) + (theTimeSpan.Seconds / 3600)) * basalRate.UnitsPerHr)
                    Next
                    s_totalDailyDose += s_totalBasal
                End If
            End If
        End If

        Dim totalPercent As String = If(s_totalDailyDose = 0,
                                        "???",
                                        $"{CInt(s_totalBasal / s_totalDailyDose * 100)}"
                                       )
        Me.Last24BasalUnitsLabel.Text = $"{s_totalBasal:F1} U"
        Me.Last24BasalPercentLabel.Text = $"{totalPercent}%"

        Me.Last24TotalInsulinUnitsLabel.Text = $"{s_totalDailyDose:F1} U"

        If s_totalAutoCorrection > 0 Then
            If s_totalDailyDose > 0 Then
                totalPercent = CInt(s_totalAutoCorrection / s_totalDailyDose * 100).ToString
            End If
            Me.Last24AutoCorrectionLabel.Visible = True
            Me.Last24AutoCorrectionUnitsLabel.Text = $"{s_totalAutoCorrection:F1} U"
            Me.Last24AutoCorrectionUnitsLabel.Visible = True
            Me.Last24AutoCorrectionPercentLabel.Text = $"{totalPercent}%"
            Me.Last24AutoCorrectionPercentLabel.Visible = True
            If s_totalDailyDose > 0 Then
                totalPercent = CInt(s_totalManualBolus / s_totalDailyDose * 100).ToString
            End If
            Me.Last24ManualBolusUnitsLabel.Text = $"{s_totalManualBolus:F1} U"
            Me.Last24ManualBolusPercentLabel.Text = $"{totalPercent}%"
        Else
            Me.Last24AutoCorrectionLabel.Visible = False
            Me.Last24AutoCorrectionUnitsLabel.Visible = False
            Me.Last24AutoCorrectionPercentLabel.Visible = False
            If s_totalDailyDose > 0 Then
                totalPercent = CInt(s_totalManualBolus / s_totalDailyDose * 100).ToString
            End If
            Me.Last24ManualBolusUnitsLabel.Text = $"{s_totalManualBolus:F1} U"
            Me.Last24ManualBolusPercentLabel.Text = $"{totalPercent}%"
        End If
        Me.Last24CarbsValueLabel.Text = $"{s_totalCarbs} {GetCarbDefaultUnit()}"
    End Sub

    Private Sub UpdateInsulinLevel()

        Me.InsulinLevelPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
        If Not s_pumpInRangeOfPhone Then
            Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(8)
            Me.RemainingInsulinUnits.Text = "???U"
        Else
            Me.RemainingInsulinUnits.Text = $"{s_listOfSummaryRecords.GetValue(Of String)(NameOf(ServerDataIndexes.reservoirRemainingUnits)).ParseSingle(1):N1} U"
            Select Case s_reservoirLevelPercent
                Case >= 85
                    Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(7)
                Case >= 71
                    Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(6)
                Case >= 57
                    Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(5)
                Case >= 43
                    Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(4)
                Case >= 29
                    Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(3)
                Case >= 15
                    Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(2)
                Case >= 1
                    Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(1)
                Case Else
                    Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(0)
            End Select
        End If
        Application.DoEvents()
    End Sub

    Private Sub UpdatePumpBattery()
        If Not s_pumpInRangeOfPhone Then
            Me.PumpBatteryPictureBox.Image = My.Resources.PumpConnectivityToPhoneNotOK
            Me.PumpBatteryRemainingLabel.Text = "Pump out"
            Me.PumpBatteryRemaining2Label.Text = "of range"
            Exit Sub
        End If

        Dim batteryLeftPercent As Integer = s_listOfSummaryRecords.GetValue(Of Integer)(NameOf(ServerDataIndexes.pumpBatteryLevelPercent))
        Me.PumpBatteryRemaining2Label.Text = $"{Math.Abs(batteryLeftPercent)}%"
        Select Case batteryLeftPercent
            Case > 90
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryFull
                Me.PumpBatteryRemainingLabel.Text = "Full"
            Case > 50
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryHigh
                Me.PumpBatteryRemainingLabel.Text = "High"
            Case > 25
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryMedium
                Me.PumpBatteryRemainingLabel.Text = $"Medium"
            Case > 10
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryLow
                Me.PumpBatteryRemainingLabel.Text = "Low"

            Case Else
                Me.PumpBatteryPictureBox.Image = My.Resources.PumpBatteryCritical
                Me.PumpBatteryRemainingLabel.Text = "Critical"
        End Select
    End Sub

    Private Sub UpdateSensorLife()
        If s_pumpInRangeOfPhone Then

            Select Case s_sensorDurationHours
                Case Is >= 255
                    Me.SensorDaysLeftLabel.Text = ""
                    Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorExpirationUnknown
                    Me.SensorTimeLeftLabel.Text = "Unknown"
                Case Is >= 168
                    Me.SensorDaysLeftLabel.Text = "~7"
                    Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorLifeOK
                    Me.SensorTimeLeftLabel.Text = "7 Days"

                Case Is >= 24
                    Me.SensorDaysLeftLabel.Text = Math.Ceiling(s_sensorDurationHours / 24).ToString()
                    Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorLifeOK
                    Me.SensorTimeLeftLabel.Text = $"{Me.SensorDaysLeftLabel.Text} Days"
                Case > 0
                    Me.SensorDaysLeftLabel.Text = $"<{Math.Ceiling(s_sensorDurationHours / 24)}"
                    Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorLifeNotOK
                    Me.SensorTimeLeftLabel.Text = $"{s_sensorDurationHours} Hours"
                Case 0
                    Dim sensorDurationMinutes As Integer = s_listOfSummaryRecords.GetValue(NameOf(ServerDataIndexes.sensorDurationMinutes), False, -1)
                    Select Case sensorDurationMinutes
                        Case > 0
                            Me.SensorDaysLeftLabel.Text = "0"
                            Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorLifeNotOK
                            Me.SensorTimeLeftLabel.Text = $"{sensorDurationMinutes} minutes"
                        Case 0
                            Me.SensorDaysLeftLabel.Text = ""
                            Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorExpired
                            Me.SensorTimeLeftLabel.Text = "Expired"
                        Case Else
                            Me.SensorDaysLeftLabel.Text = ""
                            Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorExpirationUnknown
                            Me.SensorTimeLeftLabel.Text = "Unknown"
                    End Select

                Case Else
                    Me.SensorDaysLeftLabel.Text = ""
                    Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorExpirationUnknown
                    Me.SensorTimeLeftLabel.Text = "Unknown"
            End Select
            Me.SensorDaysLeftLabel.Visible = True
        End If
        Me.SensorTimeLeftPanel.Visible = s_pumpInRangeOfPhone
    End Sub

    Private Sub UpdateTimeInRange()
        If Me.TimeInRangeChart Is Nothing Then
            Stop
            Exit Sub
        End If

        Me.TimeInRangeChartLabel.Text = GetTIR.ToString
        With Me.TimeInRangeChart
            With .Series(NameOf(TimeInRangeSeries)).Points
                .Clear()
                .AddXY($"{s_belowHypoLimit}% Below {TirLowLimitAsString(NativeMmolL)} {SgUnitsNativeString}", s_belowHypoLimit / 100)
                .Last().Color = Color.Red
                .Last().BorderColor = Color.Black
                .Last().BorderWidth = 2
                .AddXY($"{s_aboveHyperLimit}% Above {TirHighLimitAsString(NativeMmolL)} {SgUnitsNativeString}", s_aboveHyperLimit / 100)
                .Last().Color = Color.Yellow
                .Last().BorderColor = Color.Black
                .Last().BorderWidth = 2
                .AddXY($"{GetTIR()}% In Range", GetTIR() / 100)
                .Last().Color = Color.LimeGreen
                .Last().BorderColor = Color.Black
                .Last().BorderWidth = 2
            End With
            .Series(NameOf(TimeInRangeSeries))("PieLabelStyle") = "Disabled"
            .Series(NameOf(TimeInRangeSeries))("PieStartAngle") = "270"
        End With

        Me.AboveHighLimitValueLabel.Text = $"{s_aboveHyperLimit} %"
        Me.AboveHighLimitMessageLabel.Text = $"Above {TirHighLimitAsString(NativeMmolL)} {SgUnitsNativeString}"
        Me.TimeInRangeValueLabel.Text = $"{GetTIR()} %"
        Me.BelowLowLimitValueLabel.Text = $"{s_belowHypoLimit} %"
        Me.BelowLowLimitMessageLabel.Text = $"Below {TirLowLimitAsString(NativeMmolL)} {SgUnitsNativeString}"
        Dim averageSgStr As String = RecentData.GetStringValueOrEmpty(NameOf(ServerDataIndexes.averageSG))
        Me.AverageSGValueLabel.Text = If(NativeMmolL, averageSgStr.TruncateSingleString(2), averageSgStr)
        Me.AverageSGMessageLabel.Text = $"Average SG in {SgUnitsNativeString}"

        ' Calculate Time in AutoMode
        If s_listOfAutoModeStatusMarkers.Count = 0 Then
            Me.SmartGuardLabel.Text = "SmartGuard 0%"
        ElseIf s_listOfAutoModeStatusMarkers.Count = 1 AndAlso s_listOfAutoModeStatusMarkers.First.autoModeOn Then
            Me.SmartGuardLabel.Text = "SmartGuard 100%"
        Else
            Try
                ' need to figure out %
                Dim autoModeStartTime As New Date
                Dim timeInAutoMode As TimeSpan = s_0TicksSpan
                For Each r As IndexClass(Of AutoModeStatus) In s_listOfAutoModeStatusMarkers.WithIndex
                    If r.IsFirst Then
                        If r.Value.autoModeOn Then
                            autoModeStartTime = r.Value.Timestamp
                        Else

                        End If
                    Else
                        If r.Value.autoModeOn Then
                            If r.IsLast Then
                                timeInAutoMode += s_listOfAutoModeStatusMarkers.First.Timestamp.AddDays(1) - r.Value.Timestamp
                            Else
                                autoModeStartTime = r.Value.Timestamp
                            End If
                        Else
                            timeInAutoMode += r.Value.Timestamp - autoModeStartTime
                            autoModeStartTime = r.Value.Timestamp
                        End If
                    End If
                Next
                Me.SmartGuardLabel.Text = If(timeInAutoMode >= s_01DaySpan,
                                             "SmartGuard 100%",
                                             $"SmartGuard {CInt(timeInAutoMode / s_01DaySpan * 100)}%"
                                            )
            Catch ex As Exception
                Me.SmartGuardLabel.Text = "SmartGuard ???%"
            End Try
        End If

        ' Calculate deviations
        Dim highCount As Integer = 0
        Dim highDeviations As Double = 0
        Dim lowCount As Integer = 0
        Dim lowDeviations As Double = 0
        Dim elements As Integer = 0
        Dim highScale As Single = (GetYMaxValue(False) - TirHighLimit(False)) / (TirLowLimit(False) - GetYMinValue(False))
        For Each sg As SG In s_listOfSgRecords.Where(Function(entry As SG) Not Single.IsNaN(entry.sg))
            elements += 1
            If sg.sgMmDl < 70 Then
                lowCount += 1
                If NativeMmolL Then
                    lowDeviations += ((TirLowLimit(True) - sg.sgMmolL) * MmolLUnitsDivisor) ^ 2
                Else
                    lowDeviations += (TirLowLimit(False) - sg.sgMmDl) ^ 2
                End If
            ElseIf sg.sgMmDl > 180 Then
                highCount += 1
                If NativeMmolL Then
                    highDeviations += ((sg.sgMmolL - TirHighLimit(True)) * MmolLUnitsDivisor) ^ 2
                Else
                    highDeviations += (sg.sgMmDl - TirHighLimit(False)) ^ 2
                End If
            End If
        Next
        highDeviations /= 11

        If elements = 0 Then
            Me.LowTirComplianceLabel.Text = ""
            Me.HighTirComplianceLabel.Text = ""
        Else
            Dim lowDeviation As Single = CSng(Math.Sqrt(lowDeviations / (elements - highCount))).RoundSingle(1, False)
            Select Case True
                Case lowDeviation <= 2
                    Me.LowTirComplianceLabel.Text = $"Low{vbCrLf}Excellent"
                    Me.LowTirComplianceLabel.ForeColor = Color.LimeGreen
                Case lowDeviation <= 4
                    Me.LowTirComplianceLabel.Text = $"Low{vbCrLf}({lowDeviation}) OK"
                    Me.LowTirComplianceLabel.ForeColor = Color.Yellow
                Case Else
                    Me.LowTirComplianceLabel.Text = $"Low{vbCrLf}({lowDeviation}) Needs{vbCrLf}Improvement"
                    Me.LowTirComplianceLabel.ForeColor = Color.Red
            End Select

            Dim highDeviation As Single = CSng(Math.Sqrt(highDeviations / (elements - lowCount))).RoundSingle(1, False)
            Select Case True
                Case highDeviation <= 2
                    Me.HighTirComplianceLabel.Text = $"High{vbCrLf}Excellent"
                    Me.HighTirComplianceLabel.ForeColor = Color.LimeGreen
                Case highDeviation <= 4
                    Me.HighTirComplianceLabel.Text = $"High{vbCrLf}({highDeviation}) OK"
                    Me.HighTirComplianceLabel.ForeColor = Color.Yellow
                Case Else
                    Me.HighTirComplianceLabel.Text = $"High{vbCrLf}({highDeviation}) Needs{vbCrLf}Improvement "
                    Me.HighTirComplianceLabel.ForeColor = Color.Red
            End Select
        End If

        Dim splitPanelMidpoint As Integer = Me.SplitContainer3.Panel2.Width \ 2
        For Each control1 As Control In Me.SplitContainer3.Panel2.Controls
            If TypeOf control1 Is Label Then
                Select Case control1.Name
                    Case NameOf(Me.LowTirComplianceLabel)
                        Me.LowTirComplianceLabel.Left = splitPanelMidpoint - Me.LowTirComplianceLabel.Width
                    Case NameOf(Me.HighTirComplianceLabel)
                        Me.HighTirComplianceLabel.Left = splitPanelMidpoint
                    Case Else
                        DirectCast(control1, Label).AutoSize = True
                        control1.Left = splitPanelMidpoint - (control1.Width \ 2)
                End Select
            End If
        Next
    End Sub

    Private Sub UpdateTreatmentChart()
        If Not ProgramInitialized Then
            Exit Sub
        End If
        Try
            Me.InitializeTreatmentMarkersChart()
            Me.TreatmentMarkersChart.Titles(NameOf(TreatmentMarkersChartTitle)).Text = $"Treatment Details - {s_listOfManualBasal.GetSubTitle}"
            Me.TreatmentMarkersChart.ChartAreas(NameOf(ChartArea)).UpdateChartAreaSgAxisX()
            Me.TreatmentMarkersChart.PlotSuspendArea(Me.TreatmentMarkerSuspendSeries)
            Me.TreatmentMarkersChart.PlotTreatmentMarkers(Me.TreatmentMarkerTimeChangeSeries)
            Me.TreatmentMarkersChart.PlotSgSeries(GetYMinValue(NativeMmolL))
            Me.TreatmentMarkersChart.PlotHighLowLimitsAndTargetSg(True)
        Catch ex As Exception
            Stop
            Throw New Exception($"{ex.DecodeException()} exception in {NameOf(InitializeTreatmentMarkersChart)}")
        End Try
        Application.DoEvents()
    End Sub

    Private Sub UpdateTrendArrows()
        Dim rowValue As String = RecentData.GetStringValueOrEmpty(NameOf(ServerDataIndexes.lastSGTrend))
        If s_pumpInRangeOfPhone Then
            Dim arrows As String = Nothing
            If s_trends.TryGetValue(rowValue, arrows) Then
                Me.LabelTrendArrows.Font = If(rowValue = "NONE",
                    New Font("Segoe UI", 18.0F, FontStyle.Bold, GraphicsUnit.Point),
                    New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point))
                Me.LabelTrendArrows.Text = s_trends(rowValue)
            Else
                Me.LabelTrendArrows.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point)
                Me.LabelTrendArrows.Text = rowValue
            End If
        End If
        Me.LabelSgTrend.Visible = s_pumpInRangeOfPhone
        Me.LabelTrendValue.Visible = s_pumpInRangeOfPhone
        Me.LabelTrendArrows.Visible = s_pumpInRangeOfPhone
    End Sub

    Friend Sub UpdateAllTabPages(fromFile As Boolean)
        If RecentDataEmpty() Then
            DebugPrint($"exiting, {NameOf(RecentData)} has no data!")
            Exit Sub
        End If
        Dim lastMedicalDeviceDataUpdateServerTimeEpoch As String = ""
        If RecentData.TryGetValue(NameOf(ServerDataIndexes.lastMedicalDeviceDataUpdateServerTime), lastMedicalDeviceDataUpdateServerTimeEpoch) Then
            If CLng(lastMedicalDeviceDataUpdateServerTimeEpoch) = s_lastMedicalDeviceDataUpdateServerEpoch Then
                Exit Sub
            End If
        End If

        If RecentData.Count > ServerDataIndexes.sensorLifeIcon + 1 Then
            Stop
        End If

        CheckForUpdatesAsync(False)

        SyncLock _updatingLock
            _updating = True ' prevent paint
            _summaryChartAbsoluteRectangle = RectangleF.Empty
            _treatmentMarkerAbsoluteRectangle = RectangleF.Empty
            Me.MenuStartHere.Enabled = False
            If fromFile Then
                Me.LoginStatus.Text = "Login Status: N/A From Saved File"
            Else
                SetLastUpdateTime($"Last Update Time: {PumpNow.ToShortDateTimeString}", "", False, PumpNow.IsDaylightSavingTime)
            End If
            Me.CursorPanel.Visible = False

            Me.Cursor = Cursors.WaitCursor
            Application.DoEvents()
            UpdateDataTables()
            Application.DoEvents()
            Me.Cursor = Cursors.Default
            _updating = False
        End SyncLock

        FinishInitialization()
        Me.UpdateTrendArrows()
        UpdateSummaryTab(Me.DgvSummary)
        Me.UpdateActiveInsulinChart()
        Me.UpdateActiveInsulin()
        Me.UpdateAutoModeShield()
        Me.UpdateCalibrationTimeRemaining()
        Me.UpdateInsulinLevel()
        Me.UpdatePumpBattery()
        Me.UpdateSensorLife()
        Me.UpdateTimeInRange()
        UpdateTransmitterBattery()
        Me.UpdateAllSummarySeries()
        Me.UpdateDosingAndCarbs()

        Me.FullNameLabel.Text = $"{s_firstName} {RecentData.GetStringValueOrEmpty(NameOf(ServerDataIndexes.lastName))}"
        Me.ModelLabel.Text = $"{s_pumpModelNumber} HW Version = {s_pumpHardwareRevision}"
        Me.PumpNameLabel.Text = GetPumpName(s_pumpModelNumber)
        Dim nonZeroRecords As IEnumerable(Of SG) = s_listOfSgRecords.Where(Function(entry As SG) Not Single.IsNaN(entry.sg))
        Me.ReadingsLabel.Text = $"{nonZeroRecords.Count()}/288 SG Readings"

        Me.TableLayoutPanelLastSG.DisplayDataTableInDGV(
                              ClassCollectionToDataTable({s_lastSgRecord}.ToList),
                              NameOf(SG),
                              AddressOf SgHelpers.AttachHandlers,
                              ServerDataIndexes.lastSG,
                              True)

        Me.TableLayoutPanelLastAlarm.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(GetSummaryRecords(s_lastAlarmValue)),
                              NameOf(LastAlarmRecord),
                              AddressOf SummaryHelpers.AttachHandlers,
                              ServerDataIndexes.lastAlarm,
                              True)

        Me.TableLayoutPanelActiveInsulin.DisplayDataTableInDGV(
                              ClassCollectionToDataTable({s_activeInsulin}.ToList),
                              NameOf(ActiveInsulin),
                              AddressOf ActiveInsulinHelpers.AttachHandlers,
                              ServerDataIndexes.activeInsulin,
                              True)

        Me.TableLayoutPanelSgs.DisplayDataTableInDGV(
                              Me.DgvSGs,
                              ClassCollectionToDataTable(
                              classCollection:=s_listOfSgRecords.OrderByDescending(Function(x) x.RecordNumber).ToList()),
                              ServerDataIndexes.sgs)
        Me.DgvSGs.Columns(0).HeaderCell.SortGlyphDirection = SortOrder.Descending

        Me.TableLayoutPanelLimits.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(classCollection:=s_listOfLimitRecords),
                              NameOf(Limit),
                              AddressOf LimitsHelpers.AttachHandlers,
                              ServerDataIndexes.limits,
                              False)

        UpdateMarkerTabs()

        UpdateNotificationTab()

        Me.TableLayoutPanelTherapyAlgorithm.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(GetSummaryRecords(s_therapyAlgorithmStateValue)),
                              NameOf(SummaryRecord),
                              AddressOf SummaryHelpers.AttachHandlers,
                              ServerDataIndexes.therapyAlgorithmState,
                              True)

        UpdatePumpBannerStateTab()

        Me.TableLayoutPanelBasal.DisplayDataTableInDGV(
                              ClassCollectionToDataTable(s_listOfManualBasal.ToList),
                              NameOf(Basal),
                              AddressOf BasalHelpers.AttachHandlers,
                              ServerDataIndexes.basal,
                              True)

        Me.MenuStartHere.Enabled = True
        Me.UpdateTreatmentChart()

        Dim showLegend As Boolean = s_totalAutoCorrection > 0
        ShowHideLegendItem(showLegend,
                           "Auto Correction",
                           _activeInsulinChartLegend, _summaryChartLegend, _treatmentMarkersChartLegend)

        showLegend = s_listOfLowGlucoseSuspendedMarkers.Count > 0 AndAlso
                     Not (s_listOfLowGlucoseSuspendedMarkers.Count = 1 AndAlso
                     s_listOfLowGlucoseSuspendedMarkers(0).deliverySuspended = False)
        ShowHideLegendItem(showLegend,
                           "Suspend",
                           _activeInsulinChartLegend, _summaryChartLegend, _treatmentMarkersChartLegend)

        If s_listOfLowGlucoseSuspendedMarkers.Count = 1 AndAlso s_listOfLowGlucoseSuspendedMarkers(0).deliverySuspended = False Then
            Exit Sub
        End If

        If My.Settings.SystemAudioAlertsEnabled AndAlso My.Settings.SystemSpeechRecognitionThreshold <> 1 Then
            InitializeSpeechRecognition()
        Else
            CancelSpeechRecognition()
        End If
        Application.DoEvents()
    End Sub

#End Region ' Update Home Tab

End Class
