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

    Private ReadOnly _calibrationToolTip As New ToolTip()
    Private ReadOnly _carbRatio As New ToolTip()
    Private ReadOnly _processName As String = Process.GetCurrentProcess().ProcessName
    Private ReadOnly _sensorLifeToolTip As New ToolTip()
    Private ReadOnly _sgMiniDisplay As New SgMiniForm(form1:=Me)
    Private ReadOnly _updatingLock As New Object()

    Private _activeInsulinChartAbsoluteRectangle As RectangleF = RectangleF.Empty
    Private _dgvSummaryPrevRowIndex As Integer = -1
    Private _dgvSummaryPrevColIndex As Integer = -1
    Private _formScale As New SizeF(width:=1.0F, height:=1.0F)
    Private _inMouseMove As Boolean = False
    Private _lastMarkerTabLocation As (Page As Integer, Tab As Integer) = (Page:=0, Tab:=0)
    Private _lastSummaryTabIndex As Integer = 0
    Private _previousLoc As Point
    Private _remainingInsulinList As New List(Of RunningActiveInsulin)
    Private _showBalloonTip As Boolean = True
    Private _summaryChartAbsoluteRectangle As RectangleF
    Private _treatmentMarkerAbsoluteRectangle As RectangleF
    Private _timeInTightRange As (Uint As UInteger, Str As String)
    Private _updating As Boolean
    Private _webView2ProcessId As Integer = -1

    Public WriteOnly Property WebView2ProcessId As Integer
        Set
            _webView2ProcessId = Value
        End Set
    End Property

    Public Shared Property Client As Client2
        Get
            Return LoginHelpers.LoginDialog?.Client
        End Get
        Set(value As Client2)
            LoginHelpers.LoginDialog.Client = value
        End Set
    End Property

#Region "Overrides"

    ''' <summary>
    '''  Handles the <see cref="Form.HandleCreated"/> event.
    '''  Enables dark mode for the form and its controls.
    ''' </summary>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        EnableDarkMode(hwnd:=Me.Handle)
    End Sub

    ''' <summary>
    '''  Scales the control based on the <paramref name="factor"/>
    '''  and <paramref name="specified"/> bounds.
    '''  This method overrides the base method to adjust the form scale
    '''  and fix SplitContainer controls.
    ''' </summary>
    ''' <param name="factor">The scaling factor.</param>
    ''' <param name="specified">The bounds specified for scaling.</param>
    Protected Overrides Sub ScaleControl(factor As SizeF, specified As BoundsSpecified)
        _formScale = New SizeF(width:=_formScale.Width * factor.Width,
                               height:=_formScale.Height * factor.Height)
        MyBase.ScaleControl(factor, specified)
    End Sub

    ''' <summary>
    '''  Overloaded System Windows Handler.
    ''' </summary>
    ''' <param name="m">Message <see cref="Message"/> structure</param>
    <DebuggerNonUserCode()>
    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case WM_POWERBROADCAST
                Select Case m.WParam.ToInt32()
                    'value passed when system is going on standby / suspended
                    Case PBT_APMQUERYSUSPEND
                        Me.PowerModeChanged(
                            sender:=Nothing,
                            e:=New PowerModeChangedEventArgs(mode:=PowerModes.Suspend))

                        'value passed when system is resumed after suspension.
                    Case PBT_APMRESUMESUSPEND
                        Me.PowerModeChanged(
                            sender:=Nothing,
                            e:=New PowerModeChangedEventArgs(mode:=PowerModes.Resume))

                    'value passed when system Suspend Failed
                    Case PBT_APMQUERYSUSPENDFAILED
                        Me.PowerModeChanged(
                            sender:=Nothing,
                            e:=New PowerModeChangedEventArgs(mode:=PowerModes.Resume))

                    'value passed when system is suspended
                    Case PBT_APMSUSPEND
                        Me.PowerModeChanged(
                            sender:=Nothing,
                            e:=New PowerModeChangedEventArgs(mode:=PowerModes.Suspend))

                    'value passed when system is in standby
                    Case PBT_APMSTANDBY
                        Me.PowerModeChanged(
                            sender:=Nothing,
                            e:=New PowerModeChangedEventArgs(mode:=PowerModes.Suspend))

                        'value passed when system resumes from standby
                    Case PBT_APMRESUMESTANDBY
                        Me.PowerModeChanged(
                            sender:=Nothing,
                            e:=New PowerModeChangedEventArgs(mode:=PowerModes.Resume))

                        'value passed when system resumes from suspend
                    Case PBT_APMRESUMESUSPEND
                        Me.PowerModeChanged(
                            sender:=Nothing,
                            e:=New PowerModeChangedEventArgs(mode:=PowerModes.Resume))

                    'value passed when system is resumed automatically
                    Case PBT_APMRESUMEAUTOMATIC
                        Me.PowerModeChanged(
                            sender:=Nothing,
                            e:=New PowerModeChangedEventArgs(mode:=PowerModes.Resume))

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

#End Region 'Overrides

#Region "Chart Objects"

#Region "Chart"

    Private WithEvents ActiveInsulinChart As Chart
    Private WithEvents SummaryChart As Chart
    Private WithEvents TimeInRangeChart As Chart
    Private WithEvents TreatmentMarkersChart As Chart

#End Region

#Region "Legends"

    Friend _activeInsulinChartLegend As Legend
    Friend _summaryChartLegend As Legend
    Friend _treatmentMarkersChartLegend As Legend

    Private Sub ShowHideLegends()
        Dim showLegend As Boolean = s_totalAutoCorrection > 0
        ShowHideLegendItem(
            showLegend,
            legendString:="Auto Correction",
            activeInsulinChartLegend:=_activeInsulinChartLegend,
            homeChartLegend:=_summaryChartLegend,
            treatmentMarkersChartLegend:=_treatmentMarkersChartLegend)

        showLegend = s_suspendedMarkers.Any(predicate:=Function(s) s.deliverySuspended)

        ShowHideLegendItem(
            showLegend,
            legendString:="Suspend",
            activeInsulinChartLegend:=_activeInsulinChartLegend,
            homeChartLegend:=_summaryChartLegend,
            treatmentMarkersChartLegend:=_treatmentMarkersChartLegend)
    End Sub

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
    Private Property TreatmentAutoCorrectionSeries As Series
    Private Property TreatmentBasalSeries As Series
    Private Property TreatmentMarkersSeries As Series
    Private Property TreatmentMinBasalSeries As Series
    Private Property TreatmentSgSeries As Series
    Private Property TreatmentSuspendSeries As Series
    Private Property TreatmentTimeChangeSeries As Series
    Private Property TreatmentTargetSeries As Series

#End Region 'Common Series

#Region "Titles"

    Private WithEvents ActiveInsulinChartTitle As Title
    Private WithEvents TreatmentMarkersChartTitle As Title

#End Region ' Titles

#End Region ' Chart Objects

#Region "Events"

#Region "Chart Events"

    ''' <summary>
    '''  Handles the <see cref="Chart.CursorPositionChanging"/> event for
    '''  the <see cref="ActiveInsulinChart"/> and <see cref="SummaryChart"/>.
    '''  Starts the cursor timer when the cursor position is changing,
    '''  if the program is initialized.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="Chart"/> control.</param>
    ''' <param name="e">A <see cref="CursorEventArgs"/> that contains the event data.</param>
    Private Sub Chart_CursorPositionChanging(sender As Object, e As CursorEventArgs) _
        Handles ActiveInsulinChart.CursorPositionChanging, SummaryChart.CursorPositionChanging

        If Not ProgramInitialized Then
            Exit Sub
        End If

        Me.CursorTimer.Interval = ThirtySecondsInMilliseconds
        Me.CursorTimer.Start()
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Chart.MouseLeave"/> event for the
    '''  <see cref="ActiveInsulinChart"/>, <see cref="SummaryChart"/>,
    '''  and <see cref="TreatmentMarkersChart"/>. Hides the callout annotation
    '''  when the mouse leaves the chart area.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="Chart"/> control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub Chart_MouseLeave(sender As Object, e As EventArgs) Handles _
        ActiveInsulinChart.MouseLeave,
        SummaryChart.MouseLeave,
        TreatmentMarkersChart.MouseLeave

        Dim chart As Chart = CType(sender, Chart)
        With s_calloutAnnotations(key:=chart.Name)
            If .Visible Then
                .Visible = False
            End If
        End With
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Chart.MouseMove"/> event for
    '''  the <see cref="ActiveInsulinChart"/>, <see cref="SummaryChart"/>,
    '''  and <see cref="TreatmentMarkersChart"/>. Displays context-sensitive
    '''  information in a panel or callout when the mouse moves over a data point.
    '''  Shows details such as marker type, value, and time,
    '''  or sensor glucose information, depending on the series.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="Chart"/> control.</param>
    ''' <param name="e">A <see cref="MouseEventArgs"/> that contains the event data.</param>
    Private Sub Chart_MouseMove(sender As Object, e As MouseEventArgs) Handles _
        ActiveInsulinChart.MouseMove,
        SummaryChart.MouseMove,
        TreatmentMarkersChart.MouseMove

        If Not ProgramInitialized Then
            Exit Sub
        End If
        If e.Button <> MouseButtons.None OrElse
           e.Clicks > 0 OrElse
           e.Location = _previousLoc Then
            Return
        End If
        _inMouseMove = True
        _previousLoc = e.Location
        Dim yInPixels As Double
        Dim chart1 As Chart = CType(sender, Chart)
        Dim isHomePage As Boolean = chart1.Name = NameOf(SummaryChart)
        Try
            yInPixels =
                chart1.ChartAreas(name:=NameOf(ChartArea)).AxisY2 _
                    .ValueToPixelPosition(axisValue:=e.Y)
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

            Dim currentDataPoint As DataPoint = result.Series.Points(index:=result.PointIndex)

            If currentDataPoint.IsEmpty OrElse currentDataPoint.Color = Color.Transparent Then
                Me.CursorPanel.Visible = False
                Exit Sub
            End If

            Select Case result.Series.Name
                Case HighLimitSeriesName, LowLimitSeriesName, TargetSgSeriesName
                    Me.CursorPanel.Visible = False
                Case MarkerSeriesName, BasalSeriesName
                    Dim markerTags() As String = currentDataPoint.Tag.ToString.Split(separator:=":"c)
                    If markerTags.Length <= 1 Then
                        If chart1.Name = NameOf(TreatmentMarkersChart) Then
                            Dim callout As CalloutAnnotation = chart1.FindAnnotation(lastDataPoint:=currentDataPoint)
                            callout.BringToFront()
                        Else
                            Me.CursorPanel.Visible = True
                        End If
                        Exit Sub
                    End If
                    markerTags(0) = markerTags(0).Trim
                    If isHomePage Then
                        Dim xValue As Date = Date.FromOADate(currentDataPoint.XValue)
                        Me.CursorPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
                        Me.CursorPictureBox.Visible = True
                        Me.CursorMessage2Label.Font = New Font(FamilyName, emSize:=12.0F, style:=FontStyle.Bold)
                        Select Case markerTags.Length
                            Case 2
                                Me.CursorMessage1Label.Text = markerTags(0)
                                Me.CursorMessage1Label.Visible = True
                                Me.CursorMessage2Label.Text = markerTags(1).Trim
                                Me.CursorMessage2Label.Visible = True
                                Me.CursorMessage3Label.Text =
                                    Date.FromOADate(currentDataPoint.XValue).ToString(format:=s_timeWithMinuteFormat)
                                Me.CursorMessage3Label.Visible = True
                                Me.CursorMessage4Label.Visible = False
                                Select Case markerTags(0)
                                    Case "Auto Correction",
                                         "Auto Basal",
                                         "Manual Basal",
                                         "Basal",
                                         "Min Auto Basal"
                                        Me.CursorPictureBox.Image = My.Resources.InsulinVial
                                    Case "Bolus"
                                        Me.CursorPictureBox.Image = My.Resources.InsulinVial
                                    Case "Meal"
                                        Me.CursorPictureBox.Image =
                                            My.Resources.MealImageLarge
                                    Case Else
                                        Stop
                                        Me.CursorMessage1Label.Visible = False
                                        Me.CursorMessage2Label.Visible = False
                                        Me.CursorMessage3Label.Visible = False
                                        Me.CursorPictureBox.Image = Nothing
                                End Select
                                Me.CursorPanel.Visible = True
                            Case 3
                                Select Case markerTags(1).Trim
                                    Case "Calibration accepted", "Calibration not accepted"
                                        Me.CursorPictureBox.Image = My.Resources.CalibrationDotRed
                                    Case "Not used for calibration"
                                        Me.CursorPictureBox.Image = My.Resources.CalibrationDot
                                        Dim style As FontStyle = FontStyle.Bold
                                        Me.CursorMessage2Label.Font = New Font(FamilyName, emSize:=11.0F, style)
                                    Case Else
                                        Stop
                                End Select
                                Me.CursorMessage1Label.Text =
                                    $"{markerTags(0)}@{xValue.ToString(format:=s_timeWithMinuteFormat)}"
                                Me.CursorMessage1Label.Visible = True
                                Me.CursorMessage2Label.Text =
                                    markerTags(1).Replace(oldValue:="Calibration not", newValue:="Cal. not").Trim
                                Me.CursorMessage2Label.Visible = True
                                Me.CursorMessage3Label.Text = markerTags(2).Trim
                                Me.CursorMessage3Label.Visible = True
                                Dim sgValue As Single =
                                    markerTags(2).Trim.Split(separator:=" ")(0).ParseSingle(digits:=2)
                                Me.CursorMessage4Label.Text =
                                    If(NativeMmolL,
                                       $"{CInt(sgValue * MmolLUnitsDivisor)} mg/dL",
                                       $"{sgValue / MmolLUnitsDivisor:F1} mmol/L")

                                Me.CursorMessage4Label.Visible = True
                                Me.CursorPanel.Visible = True
                            Case Else
                                Stop
                                Me.CursorPanel.Visible = False
                        End Select
                    End If
                    chart1.SetUpCallout(currentDataPoint, markerTags)

                Case SgSeriesName
                    Me.CursorMessage1Label.Text = "Sensor Glucose"
                    Me.CursorMessage1Label.Visible = True
                    Me.CursorMessage2Label.Text = $"{currentDataPoint.YValues(0).RoundToSingle(digits:=3)} {BgUnits}"
                    Me.CursorMessage2Label.Visible = True
                    Me.CursorMessage3Label.Text =
                        If(NativeMmolL,
                           $"{CInt(currentDataPoint.YValues(0) * MmolLUnitsDivisor)} mg/dL",
                           $"{currentDataPoint.YValues(0) / MmolLUnitsDivisor:F1} mmol/L")

                    Me.CursorMessage3Label.Visible = True
                    Dim format As String = s_timeWithMinuteFormat
                    Me.CursorMessage4Label.Text = Date.FromOADate(currentDataPoint.XValue).ToString(format)
                    Me.CursorMessage4Label.Visible = True
                    Me.CursorPictureBox.Image = Nothing
                    Me.CursorPanel.Visible = True
                    chart1.SetupCallout(currentDataPoint, $"Sensor Glucose {Me.CursorMessage2Label.Text}")
                Case SuspendSeriesName, TimeChangeSeriesName
                    Me.CursorPanel.Visible = False
                Case ActiveInsulinSeriesName
                    Dim yValue As Single = currentDataPoint.YValues.FirstOrDefault().RoundToSingle(digits:=3)
                    chart1.SetupCallout(currentDataPoint, text:=$"Theoretical Active Insulin {yValue:F3} U")
                Case Else
                    Stop
            End Select
        Catch ex As Exception
            result = Nothing
        Finally
            _inMouseMove = False
        End Try
    End Sub

#Region "Post Paint Events"

    ''' <summary>
    '''  Handles the <see cref="Chart.PostPaint"/> event
    '''  for the <see cref="ActiveInsulinChart"/>.
    '''  Draws additional graphics or overlays after the chart is painted,
    '''  such as insulin and meal markers.
    '''  Skips painting if a mouse move is in progress, the chart is updating,
    '''  or the program is not initialized.
    ''' </summary>
    ''' <param name="sender">The source chart control.</param>
    ''' <param name="e">A <see cref="ChartPaintEventArgs"/> containing event data.</param>
    <DebuggerNonUserCode()>
    Private Sub ActiveInsulinChart_PostPaint(sender As Object, e As ChartPaintEventArgs) _
        Handles ActiveInsulinChart.PostPaint

        If _inMouseMove Then
            Exit Sub
        End If
        SyncLock _updatingLock
            If _updating Then
                Exit Sub
            End If
            If Not ProgramInitialized Then
                _activeInsulinChartAbsoluteRectangle = RectangleF.Empty
                Exit Sub
            End If
            e.PostPaintSupport(
                chartRelativePosition:=_activeInsulinChartAbsoluteRectangle,
                insulinDictionary:=s_activeInsulinMarkers,
                mealDictionary:=Nothing,
                offsetInsulinImage:=True,
                paintOnY2:=True)
        End SyncLock
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Chart.PostPaint"/> event for the <see cref="SummaryChart"/>.
    '''  Draws overlays such as insulin and meal markers after the summary chart is painted.
    '''  Skips painting if a mouse move is in progress,
    '''  the chart is updating, or the program is not initialized.
    ''' </summary>
    ''' <param name="sender">The source chart control.</param>
    ''' <param name="e">A <see cref="ChartPaintEventArgs"/> containing event data.</param>
    <DebuggerNonUserCode()>
    Private Sub SummaryChart_PostPaint(sender As Object, e As ChartPaintEventArgs) _
        Handles SummaryChart.PostPaint
        If _inMouseMove Then
            Exit Sub
        End If
        SyncLock _updatingLock
            If _updating Then
                Exit Sub
            End If
            If Not ProgramInitialized Then
                _activeInsulinChartAbsoluteRectangle = RectangleF.Empty
                Exit Sub
            End If
            e.PostPaintSupport(
                chartRelativePosition:=_summaryChartAbsoluteRectangle,
                insulinDictionary:=s_summaryMarkersInsulin,
                mealDictionary:=s_summaryMarkersMeal,
                offsetInsulinImage:=True,
                paintOnY2:=True)
        End SyncLock
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Chart.PostPaint"/> event
    '''  for the <see cref="TreatmentMarkersChart"/>.
    '''  Draws overlays such as insulin and meal markers after the
    '''  treatment markers chart is painted.
    '''  Skips painting if a mouse move is in progress, the chart is updating,
    '''  or the program is not initialized.
    ''' </summary>
    ''' <param name="sender">The source chart control.</param>
    ''' <param name="e">A <see cref="ChartPaintEventArgs"/> containing event data.</param>
    <DebuggerNonUserCode()>
    Private Sub TreatmentMarkersChart_PostPaint(sender As Object, e As ChartPaintEventArgs) _
        Handles TreatmentMarkersChart.PostPaint

        If _inMouseMove Then
            Exit Sub
        End If
        SyncLock _updatingLock
            If _updating Then
                Exit Sub
            End If
            If Not ProgramInitialized Then
                _activeInsulinChartAbsoluteRectangle = RectangleF.Empty
                Exit Sub
            End If
            e.PostPaintSupport(
                chartRelativePosition:=_treatmentMarkerAbsoluteRectangle,
                insulinDictionary:=s_treatmentMarkersInsulin,
                mealDictionary:=s_treatmentMarkersMeal,
                offsetInsulinImage:=False,
                paintOnY2:=False)
        End SyncLock
    End Sub

#End Region ' Post Paint Events

#End Region ' Chart Events

#Region "DGV Events"

#Region "DGV Global Event Helper"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellContextMenuStripNeeded"/> event
    '''  for multiple <see cref="DataGridView"/>. Assigns the context menu
    '''  for copying data if the row index is valid.
    ''' </summary>
    ''' <param name="sender">The DataGridView control.</param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewCellContextMenuStripNeededEventArgs"/> containing
    '''  event data.
    ''' </param>
    ''' <remarks>
    '''  Sets the context menu for copying data.
    ''' </remarks>
    Private Sub DgvCellContextMenuStripNeededWithExcel(
        sender As Object,
        e As DataGridViewCellContextMenuStripNeededEventArgs) Handles _
            DgvActiveInsulin.CellContextMenuStripNeeded,
            DgvAutoBasalDelivery.CellContextMenuStripNeeded,
            DgvAutoModeStatus.CellContextMenuStripNeeded,
            DgvPumpBannerState.CellContextMenuStripNeeded,
            DgvBasal.CellContextMenuStripNeeded,
            DgvBasalPerHour.CellContextMenuStripNeeded,
            DgvCalibration.CellContextMenuStripNeeded,
            DgvCareLinkUsers.CellContextMenuStripNeeded,
            DgvCurrentUser.CellContextMenuStripNeeded,
            DgvInsulin.CellContextMenuStripNeeded,
            DgvLastAlarm.CellContextMenuStripNeeded,
            DgvLastSensorGlucose.CellContextMenuStripNeeded,
            DgvLimits.CellContextMenuStripNeeded,
            DgvLowGlucoseSuspended.CellContextMenuStripNeeded,
            DgvMeal.CellContextMenuStripNeeded,
            DgvSensorBgReadings.CellContextMenuStripNeeded,
            DgvSGs.CellContextMenuStripNeeded,
            DgvSummary.CellContextMenuStripNeeded,
            DgvTherapyAlgorithmState.CellContextMenuStripNeeded,
            DgvTimeChange.CellContextMenuStripNeeded

        If e.RowIndex >= 0 Then
            e.ContextMenuStrip = Me.DgvCopyWithExcelMenuStrip
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellFormatting"/> event
    '''  for multiple <see cref="DataGridView"/>.
    '''  Formats the cell values based on their type and applies specific styles.
    ''' </summary>
    ''' <param name="sender">The DataGridView control.</param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewCellFormattingEventArgs"/> containing event data.
    ''' </param>
    ''' <remarks>
    '''  Applies formatting to cells based on their data type and content.
    ''' </remarks>
    Private Sub Dgv_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles _
        DgvActiveInsulin.CellFormatting,
        DgvAutoBasalDelivery.CellFormatting,
        DgvAutoModeStatus.CellFormatting,
        DgvPumpBannerState.CellFormatting,
        DgvBasal.CellFormatting,
        DgvBasalPerHour.CellFormatting,
        DgvCalibration.CellFormatting,
        DgvCareLinkUsers.CellFormatting,
        DgvCurrentUser.CellFormatting,
        DgvInsulin.CellFormatting,
        DgvLastAlarm.CellFormatting,
        DgvLastSensorGlucose.CellFormatting,
        DgvLimits.CellFormatting,
        DgvLowGlucoseSuspended.CellFormatting,
        DgvMeal.CellFormatting,
        DgvSensorBgReadings.CellFormatting,
        DgvSGs.CellFormatting,
        DgvTimeChange.CellFormatting

        If e.Value Is Nothing OrElse e.Value Is DBNull.Value Then
            e.Value = String.Empty
        End If
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim columnName As String = dgv.Columns(index:=e.ColumnIndex).Name
        Try
            Select Case columnName
                Case NameOf(Insulin.ActivationType)
                    Select Case Convert.ToString(e.Value)
                        Case "AUTOCORRECTION"
                            e.Value = "Auto Correction"
                            Dim textColor As Color = GetGraphLineColor(key:="Auto Correction")
                            dgv.CellFormattingApplyBoldColor(e, textColor)
                        Case "FAST", "RECOMMENDED", "UNDETERMINED"
                            dgv.CellFormattingToTitle(e)
                        Case Else
                            dgv.CellFormattingSetForegroundColor(e)
                    End Select
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                Case "Amount"
                    Select Case dgv.Name
                        Case NameOf(DgvActiveInsulin)
                            dgv.CellFormattingSingleValue(e, digits:=3, TrailingText:=" U")
                        Case NameOf(DgvMeal)
                            dgv.CellFormattingInteger(e, message:=GetCarbDefaultUnit)
                    End Select

                Case NameOf(BasalPerHour.BasalRate), NameOf(BasalPerHour.BasalRate2)
                    If dgv.Name = NameOf(DgvBasalPerHour) Then
                        dgv.CellFormattingSingleValue(e, digits:=3, TrailingText:=" U/h")
                        e.CellStyle.Font = New Font(FamilyName, emSize:=12.0F, style:=FontStyle.Regular)
                    End If

                Case NameOf(Calibration.bgUnits)
                    Dim key As String = Convert.ToString(e.Value)
                    Try
                        e.Value = UnitsStrings(key)
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Catch ex As Exception
                        e.Value = key ' Key becomes value if its unknown
                    End Try
                    dgv.CellFormattingSetForegroundColor(e)

                Case NameOf(AutoBasalDelivery.BolusAmount)
                    If dgv.CellFormattingSingleValue(e, digits:=3).IsMinBasal Then
                        dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Red)
                    Else
                        dgv.CellFormattingSetForegroundColor(e)
                    End If

                Case NameOf(Insulin.BolusType), NameOf(Insulin.InsulinType)
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    dgv.CellFormattingToTitle(e)

                Case NameOf(ActiveInsulin.DateTime),
                     NameOf(AutoModeStatus.DisplayTime),
                     NameOf(AutoModeStatus.Timestamp)
                    dgv.CellFormattingDateTime(e)

                Case NameOf(Limit.HighLimit),
                     NameOf(Limit.HighLimitMgdL),
                     NameOf(Limit.HighLimitMmolL)
                    dgv.CellFormattingSgValue(e, partialKey:=NameOf(Limit.HighLimit))

                Case NameOf(Limit.LowLimit),
                     NameOf(Limit.lowLimitMgdL),
                     NameOf(Limit.lowLimitMmolL)
                    dgv.CellFormattingSgValue(e, partialKey:=NameOf(Limit.LowLimit))

                Case NameOf(InsulinPerHour.Hour),
                     NameOf(InsulinPerHour.Hour2)
                    Dim hour As Integer = TimeSpan.FromHours(CInt(e.Value)).Hours
                    Dim time As New DateTime(
                        year:=1,
                        month:=1,
                        day:=1,
                        hour,
                        minute:=0,
                        second:=0)
                    e.Value = time.ToString(format:=s_timeWithoutMinuteFormat)
                    e.CellStyle.Font = New Font(FamilyName, emSize:=12.0F, style:=FontStyle.Regular)

                Case NameOf(BannerState.Message)
                    Select Case dgv.Name
                        Case NameOf(DgvPumpBannerState)
                            dgv.CellFormattingToTitle(e)
                        Case NameOf(DgvSGs)
                            e.Value =
                                Convert.ToString(e.Value).
                                    Replace(oldValue:=vbCrLf, newValue:=" ")
                            dgv.CellFormattingSetForegroundColor(e)
                        Case Else
                            e.Value =
                                Convert.ToString(e.Value).Replace(oldValue:=vbCrLf, newValue:=" ")
                            dgv.CellFormattingSetForegroundColor(e)
                    End Select
                Case NameOf(ActiveInsulin.Precision)
                    dgv.CellFormattingToTitle(e)
                Case NameOf(Insulin.SafeMealReduction)
                    If dgv.CellFormattingSingleValue(e, digits:=3) >= 0.0025 Then
                        dgv.CellFormattingApplyBoldColor(e, textColor:=Color.OrangeRed)
                    Else
                        e.Value = ""
                        dgv.CellFormattingSetForegroundColor(e)
                    End If
                Case NameOf(SG.SensorState)
                    If Equals(e.Value, "NO_ERROR_MESSAGE") Then
                        dgv.CellFormattingToTitle(e)
                    Else
                        dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Red)
                        dgv.CellFormattingToTitle(e)
                    End If

                Case NameOf(SG.sg), NameOf(SG.sgMmolL), NameOf(SG.sgMgdL)
                    dgv.CellFormattingSgValue(e, partialKey:=NameOf(SG.sg))

                Case NameOf(BannerState.TimeRemaining)
                    e.CellFormatting0Value()

                Case NameOf(SG.Timestamp)
                    dgv.CellFormattingDateTime(e)

                Case NameOf(SG.sg), NameOf(SG.sgMmolL), NameOf(SG.sgMgdL)
                    dgv.CellFormattingSgValue(e, partialKey:=NameOf(SG.sg))

                Case NameOf(Calibration.UnitValue),
                     NameOf(Calibration.UnitValueMgdL),
                     NameOf(Calibration.UnitValueMmolL)
                    dgv.CellFormattingSgValue(e, partialKey:=NameOf(Calibration.UnitValue))
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dgv.CellFormattingSetForegroundColor(e)

                Case Else
                    Dim valueType As Type = dgv.Columns(index:=e.ColumnIndex).ValueType
                    If valueType = GetType(Single) Then
                        dgv.CellFormattingSingleValue(e, digits:=3)
                    ElseIf valueType = GetType(String) AndAlso
                        dgv.Name <> NameOf(DgvLastAlarm) Then

                        dgv.CellFormattingSingleWord(e)
                    Else
                        dgv.CellFormattingSetForegroundColor(e)
                    End If
            End Select
        Catch ex As Exception
            Stop
        End Try
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.DataBindingComplete"/> event.
    '''  This event is raised when the data binding operation is complete.
    '''  It clears the selection of all DataGridViews to ensure no cells
    '''  are selected after data binding.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  The <see cref="DataGridViewBindingCompleteEventArgs"/> containing the event data.
    ''' </param>
    ''' <remarks>
    '''  This event is used to customize the appearance of DataGridViews
    '''  after data binding is complete.
    ''' </remarks>
    Private Sub Dgv_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles _
        DgvActiveInsulin.DataBindingComplete,
        DgvAutoBasalDelivery.DataBindingComplete,
        DgvAutoModeStatus.DataBindingComplete,
        DgvPumpBannerState.DataBindingComplete,
        DgvBasal.DataBindingComplete,
        DgvBasalPerHour.DataBindingComplete,
        DgvCalibration.DataBindingComplete,
        DgvCareLinkUsers.DataBindingComplete,
        DgvCurrentUser.DataBindingComplete,
        DgvLastAlarm.DataBindingComplete,
        DgvLastSensorGlucose.DataBindingComplete,
        DgvLimits.DataBindingComplete,
        DgvLowGlucoseSuspended.DataBindingComplete,
        DgvMeal.DataBindingComplete,
        DgvSensorBgReadings.DataBindingComplete,
        DgvSummary.DataBindingComplete,
        DgvTherapyAlgorithmState.DataBindingComplete,
        DgvTimeChange.DataBindingComplete

        Dim dgv As DataGridView = CType(sender, DataGridView)
        If dgv.ColumnCount > 0 Then
            Dim lastColumnIndex As Integer = dgv.ColumnCount - 1
            For Each column As DataGridViewColumn In dgv.Columns
                With dgv.Columns(column.Index)
                    If .Index = lastColumnIndex Then
                        dgv.ColumnHeadersDefaultCellStyle.WrapMode =
                            DataGridViewTriState.False
                        .DefaultCellStyle.WrapMode = DataGridViewTriState.True
                        If .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill Then
                            .DefaultCellStyle.WrapMode = DataGridViewTriState.True
                        End If
                    Else
                        If s_wrappedDataGridView.Contains(item:=dgv.Name) Then
                            Dim result As String = String.Empty
                            If s_wrappedStrings.TryGetPrefixMatch(.HeaderText, result) Then
                                Dim trimChars As Char() = {" "c, NonBreakingSpace}
                                .HeaderText = .HeaderText.Replace(
                                    oldValue:=result,
                                    newValue:=$"{result.TrimEnd(trimChars)}{vbCrLf}")
                                .HeaderCell.Style.WrapMode = DataGridViewTriState.True
                                .DefaultCellStyle.WrapMode = DataGridViewTriState.True
                                If result.StartsWithNoCase(value:="Timestamp") Then
                                    .MinimumWidth = 130
                                End If
                            Else
                                column.HeaderCell.Style.WrapMode = DataGridViewTriState.False
                                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                            End If
                        End If
                    End If
                End With
            Next
        End If

        If dgv.Name = NameOf(DgvSummary) AndAlso
            _dgvSummaryPrevRowIndex > 0 AndAlso
            _dgvSummaryPrevRowIndex < dgv.RowCount AndAlso
            _dgvSummaryPrevColIndex > 0 AndAlso
            _dgvSummaryPrevColIndex < dgv.ColumnCount Then

            ' Restore the previous selection in the Summary DataGridView
            ' if its not empty or Row(0).Cell(0).
            dgv.CurrentCell =
                dgv.Rows(index:=_dgvSummaryPrevRowIndex).Cells(index:=_dgvSummaryPrevColIndex)
            dgv.Rows(index:=_dgvSummaryPrevRowIndex).Selected = True
            dgv.FirstDisplayedScrollingRowIndex = _dgvSummaryPrevRowIndex
        Else
            ' Clear the selection of all DataGridViews except Summary DataGridView.
            dgv.ClearSelection()
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.DataError"/> event
    '''  for all DataGridViews in the form. This event is raised when an
    '''  exception occurs during data operations such as
    '''  formatting, parsing, or committing a cell value.
    '''  The handler currently stops execution for debugging purposes.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewDataErrorEventArgs"/> that contains the event data,
    '''  including the exception and context.
    ''' </param>
    ''' <remarks>
    '''  This method is intended for debugging and should be updated to provide
    '''  user-friendly error handling in production.
    ''' </remarks>
    Private Sub Dgv_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles _
        DgvActiveInsulin.DataError,
        DgvAutoBasalDelivery.DataError,
        DgvAutoModeStatus.DataError,
        DgvPumpBannerState.DataError,
        DgvBasal.DataError,
        DgvBasalPerHour.DataError,
        DgvCalibration.DataError,
        DgvCareLinkUsers.DataError,
        DgvCurrentUser.DataError,
        DgvInsulin.DataError,
        DgvLastAlarm.DataError,
        DgvLastSensorGlucose.DataError,
        DgvLimits.DataError,
        DgvLowGlucoseSuspended.DataError,
        DgvMeal.DataError,
        DgvSensorBgReadings.DataError,
        DgvSGs.DataError,
        DgvSummary.DataError,
        DgvTherapyAlgorithmState.DataError,
        DgvTimeChange.DataError

        Dim dgv As DataGridView = CType(sender, DataGridView)
        ' Log the error (to file, event log, etc.)
        Debug.WriteLine($"DataError in {dgv.Name}: {e.Exception.Message}")

        ' Show a user-friendly message
        Dim dgvName As String = dgv.Name.Remove(s:="dgv")
        Dim text As String =
            $"An error {e.Exception.Message} occurred while processing {dgvName}. " &
            $"Please check your data and try again."
        MessageBox.Show(
            text,
            caption:="Input Error",
            buttons:=MessageBoxButtons.OK,
            icon:=MessageBoxIcon.Warning)

        Debug.WriteLine(message:=$"Context: {e.Context}")

        ' Prevent the exception from being thrown again
        e.ThrowException = False
        Stop
    End Sub

#Region "ContextMenuStrip Menu Events"

    Private WithEvents DgvCopyWithExcelMenuStrip As New ContextMenuStrip
    Friend WithEvents DgvCopyWithoutExcelMenuStrip As New ContextMenuStrip

    ''' <summary>
    '''  Handles the <see cref="DgvCopyWithExcelMenuStrip.Opening"/> event
    '''  for the copy-with-Excel context menu.
    '''  Populates the context menu with options to copy data with or without headers,
    '''  or save to Excel.
    ''' </summary>
    ''' <param name="sender">The context menu strip.</param>
    ''' <param name="e">A <see cref="CancelEventArgs"/> containing event data.</param>
    Private Sub DgvCopyWithExcelMenuStrip_Opening(sender As Object, e As CancelEventArgs) _
        Handles DgvCopyWithExcelMenuStrip.Opening

        ' Acquire references to the owning control and item.
        Dim mnuStrip As ContextMenuStrip = CType(sender, ContextMenuStrip)
        mnuStrip.Tag = CType(mnuStrip.SourceControl, DataGridView)

        ' Clear the ContextMenuStrip control's Items collection.
        mnuStrip.Items.Clear()

        ' Populate the ContextMenuStrip control with its default items.
        mnuStrip.Items.Add(
            text:="Copy with Header",
            image:=My.Resources.Copy,
            onClick:=AddressOf DgvExportToClipBoardWithHeaders)
        mnuStrip.Items.Add(
            text:="Copy without Header",
            image:=My.Resources.Copy,
            onClick:=AddressOf DgvExportToClipBoardWithoutHeaders)
        mnuStrip.Items.Add(
            text:="Save To Excel",
            image:=My.Resources.ExportData,
            onClick:=AddressOf DgvExportToExcel)

        ' Set Cancel to false.
        ' It is optimized to true based on empty key.
        e.Cancel = False
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DgvCopyWithExcelMenuStrip.Opening"/> event
    '''  for the copy-without-Excel context menu.
    '''  Populates the context menu with options to copy selected cells with
    '''  or without headers.
    ''' </summary>
    ''' <param name="sender">The context menu strip.</param>
    ''' <param name="e">A <see cref="CancelEventArgs"/> containing event data.</param>
    Private Sub DgvCopyWithoutExcelMenuStrip_Opening(sender As Object, e As CancelEventArgs) _
        Handles DgvCopyWithoutExcelMenuStrip.Opening

        ' Acquire references to the owning control and item.
        Dim mnuStrip As ContextMenuStrip = CType(sender, ContextMenuStrip)
        mnuStrip.Tag = CType(Me.DgvCopyWithExcelMenuStrip.SourceControl, DataGridView)

        ' Clear the ContextMenuStrip control's Items collection.
        mnuStrip.Items.Clear()

        ' Populate the ContextMenuStrip control with its default items.
        mnuStrip.Items.Add(
            text:="Copy Selected Cells with Header",
            image:=My.Resources.Copy,
            onClick:=AddressOf DgvCopySelectedCellsToClipBoardWithHeaders)
        mnuStrip.Items.Add(
            text:="Copy Selected Cells without headers",
            image:=My.Resources.Copy,
            onClick:=AddressOf DgvCopySelectedCellsToClipBoardWithoutHeaders)

        ' Set Cancel to false.
        ' It is optimized to true based on empty key.
        e.Cancel = False
    End Sub

#End Region 'ContextMenuStrip Events

#End Region ' DGV Global Event Helper

#Region "Dgv Active Insulin Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event
    '''  for the <see cref="DgvActiveInsulin"/> DataGridView.
    '''  This event is raised when a new column is added to the <see cref="DataGridView"/>.
    '''  It sets the properties of the newly added column,
    '''  such as sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvActiveInsulin_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvActiveInsulin.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            .Visible = Not HideColumn(Of ActiveInsulin)(item:= .Name)
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(Of ActiveInsulin)(.Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Active Insulin Events

#Region "Dgv Auto Basal Delivery (Basal) Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event
    '''  for the <see cref="DgvAutoBasalDelivery"/>.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode,
    '''  visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvAutoBasalDelivery_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvAutoBasalDelivery.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If HideColumn(Of AutoBasalDelivery)(item:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(Of AutoBasalDelivery)(.Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Auto Basal Delivery (Basal) Events

#Region "Dgv AutoMode Status Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event
    '''  for the <see cref="DgvAutoModeStatus"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode,
    '''  visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvAutoModeStatus_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvAutoModeStatus.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If HideColumn(Of AutoModeStatus)(item:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(Of AutoModeStatus)(.Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv AutoMode Status Events

#Region "Dgv Pump Banner State Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for
    '''  the <see cref="DgvPumpBannerState"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as
    '''  sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvBannerState_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvPumpBannerState.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If HideColumn(Of BannerState)(item:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(Of BannerState)(.Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Banner State Events

#Region "Dgv Basal Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event
    '''  for the <see cref="DgvBasal"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode,
    '''  visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvBasal_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvBasal.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If HideColumn(Of Basal)(item:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(Of Basal)(.Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Basal Events

#Region "Dgv Basal Per Hour Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event
    '''  for the <see cref="DgvBasalPerHour"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode,
    '''  visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvBasalPerHour_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvBasalPerHour.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(Of InsulinPerHour)(.Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Basal Per Hour Events

#Region "Dgv Calibration Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event
    '''  for the <see cref="DgvCalibration"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the column properties such as SortMode, visibility,
    '''  cell style, and caption.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvCalibration_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvCalibration.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If HideColumn(Of Calibration)(item:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(Of Calibration)(.Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv CalibrationHelpers Events

#Region "Dgv CareLink™ Users Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellBeginEdit"/> event
    '''  for the <see cref="DgvCareLinkUsers"/> DataGridView.
    '''  This event is raised when a cell enters edit mode.
    '''  Saves the current value of the cell being edited to the
    '''  DataGridView's <see cref="DataGridView.Tag"/> property.
    '''  This allows comparison with the new value after editing is complete,
    '''  for change detection or validation.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewCellCancelEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvCareLinkUsers_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) _
        Handles DgvCareLinkUsers.CellBeginEdit

        Dim dgv As DataGridView = CType(sender, DataGridView)
        'Here we save a current value of cell to some variable,
        'that later we can compare with a new value
        'For example using of dgv.Tag property
        If e.RowIndex >= 0 AndAlso e.ColumnIndex > 0 Then
            dgv.Tag = dgv.CurrentCell.Value.ToString
        End If

    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellContentClick"/> event
    '''  for the <see cref="DgvCareLinkUsers"/> DataGridView.
    '''  This event is raised when a cell's content is clicked,
    '''  specifically for delete button cells.
    '''  It removes the corresponding user record from the data source and
    '''  updates the DataGridView.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewCellEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvCareLinkUsers_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) _
        Handles DgvCareLinkUsers.CellContentClick

        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim cell As DataGridViewCell = dgv.Rows(index:=e.RowIndex).Cells(index:=e.ColumnIndex)
        Dim dataGridViewDisableButtonCell As DataGridViewDisableButtonCell =
            TryCast(cell, DataGridViewDisableButtonCell)
        If dataGridViewDisableButtonCell IsNot Nothing Then
            If Not dataGridViewDisableButtonCell.Enabled Then
                Exit Sub
            End If

            dgv.DataSource = Nothing
            s_allUserSettingsData.RemoveAt(index:=e.RowIndex)
            dgv.DataSource = s_allUserSettingsData
            s_allUserSettingsData.SaveAllUserRecords()
        End If

    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellEndEdit"/> event
    '''  for the <see cref="DgvCareLinkUsers"/> DataGridView.
    '''  This event is raised after a cell edit is completed.
    '''  Intended for post-edit logic, such as validation or saving changes.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewCellEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvCareLinkUsers_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) _
        Handles DgvCareLinkUsers.CellEndEdit

        Try
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If e.RowIndex < 0 OrElse e.ColumnIndex <= 0 Then
                Exit Sub
            End If
            Dim currentValue As String = dgv.CurrentCell.Value.ToString
            If currentValue = dgv.Tag.ToString Then
                Exit Sub ' No change, exit early
            End If
            Dim userRecord As CareLinkUserDataRecord =
                s_allUserSettingsData(index:=e.RowIndex)

            Select Case dgv.Columns(index:=e.ColumnIndex).Name
                Case NameOf(CareLinkUserDataRecord.CareLinkUserName)
                    userRecord.CareLinkUserName = currentValue
                Case NameOf(CareLinkUserDataRecord.CareLinkPassword)
                    userRecord.CareLinkPassword = currentValue
                Case NameOf(CareLinkUserDataRecord.CountryCode)
                    userRecord.CountryCode = currentValue
                Case NameOf(CareLinkUserDataRecord.UseLocalTimeZone)
                    userRecord.UseLocalTimeZone = CBool(currentValue)
                Case Else
                    Exit Sub ' Unsupported column, exit early
            End Select
            s_allUserSettingsData.SaveAllUserRecords()
        Catch ex As Exception
            MessageBox.Show(text:=ex.DecodeException())
        End Try
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellValidating"/> event
    '''  for the <see cref="DgvCareLinkUsers"/> DataGridView.
    '''  Used to validate cell values before committing changes.
    '''  Currently skips validation for the first column (index 0).
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewCellValidatingEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvCareLinkUsers_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) _
        Handles DgvCareLinkUsers.CellValidating

        If e.ColumnIndex = 0 Then
            Exit Sub
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for the
    '''  <see cref="DgvCareLinkUsers"/> DataGridView.
    '''  Configures new columns, including sort mode, visibility, cell style, and caption.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvCareLinkUsers_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvCareLinkUsers.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            Dim value As String = dgv.Columns(.Index).HeaderText
            If String.IsNullOrWhiteSpace(value) Then
                value = .DataPropertyName.Remove(s:="DgvCareLinkUsers")
            End If
            If value.ContainsNoCase(value:="DeleteRow") Then
                value = ""
            Else
                If .Index > 0 AndAlso
                    String.IsNullOrWhiteSpace(value:= .DataPropertyName) AndAlso
                    String.IsNullOrWhiteSpace(value) Then

                    .DataPropertyName = s_headerColumns(index:= .Index - 2)
                End If
            End If
            Dim forceReadOnly As Boolean
            If HideColumn(Of CareLinkUserDataRecord)(.DataPropertyName) Then
                .Visible = False
            Else
                forceReadOnly = True
            End If
            e.DgvColumnAdded(
                cellStyle:=CareLinkUserDataRecordHelpers.GetCellStyleForCareLinkUser(
                    columnName:= .DataPropertyName),
                    forceReadOnly,
                    caption:=value)
        End With
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.RowsAdded"/> event
    '''  for the <see cref="DgvCareLinkUsers"/> DataGridView.
    '''  Enables or disables the delete button cell based on whether
    '''  the row belongs to the logged-on user.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewRowsAddedEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvCareLinkUsers_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) _
        Handles DgvCareLinkUsers.RowsAdded

        If s_allUserSettingsData.Count = 0 Then Exit Sub
        Dim dgv As DataGridView = CType(sender, DataGridView)
        For i As Integer = e.RowIndex To e.RowIndex + (e.RowCount - 1)
            Const columnName As String = "DgvCareLinkUsersDeleteRow"
            Dim disableButtonCell As DataGridViewDisableButtonCell =
                CType(dgv.Rows(index:=i).Cells(columnName), DataGridViewDisableButtonCell)

            Dim careLinkUserName As String =
                LoginHelpers.LoginDialog.LoggedOnUser.CareLinkUserName
            disableButtonCell.Enabled =
                s_allUserSettingsData(index:=i).CareLinkUserName <> careLinkUserName
        Next
    End Sub

    ''' <summary>
    '''  Initializes the <see cref="DgvCareLinkUsers"/> DataGridView
    '''  with columns and sets its data source.
    '''  Adds columns for user ID, delete row button, username, password, country code,
    '''  time zone, auto login, partner, and patient user ID.
    ''' </summary>
    ''' <param name="dgv">The <see cref="DataGridView"/> to initialize.</param>
    Private Sub InitializeDgvCareLinkUsers(dgv As DataGridView)
        Dim dgvCareLinkUsersID As New DataGridViewTextBoxColumn With {
            .DataPropertyName = "ID",
            .HeaderText = "ID",
            .Name = NameOf(dgvCareLinkUsersID),
            .ReadOnly = True,
            .Width = 43}

        Dim dgvCareLinkUsersDeleteRow As New DataGridViewDisableButtonColumn With {
            .DataPropertyName = "DeleteRow",
            .HeaderText = "",
            .Name = NameOf(dgvCareLinkUsersDeleteRow),
            .ReadOnly = True,
            .Text = "Delete Row",
            .UseColumnTextForButtonValue = True,
            .Width = 5}

        Dim dgvCareLinkUsersCareLinkUserName As New DataGridViewTextBoxColumn With {
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .DataPropertyName = "CareLinkUserName",
            .HeaderText = $"CareLink™ UserName",
            .MinimumWidth = 125,
            .Name = NameOf(dgvCareLinkUsersCareLinkUserName),
            .Width = 125}

        Dim dgvCareLinkUsersCareLinkPassword As New DataGridViewTextBoxColumn With {
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .DataPropertyName = "CareLinkPassword",
            .HeaderText = $"CareLink™ Password",
            .Name = NameOf(dgvCareLinkUsersCareLinkPassword),
            .Width = 120}

        Dim dgvCareLinkUsersCountryCode As New DataGridViewTextBoxColumn With {
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .DataPropertyName = "CountryCode",
            .HeaderText = "Country Code",
            .Name = NameOf(dgvCareLinkUsersCountryCode),
            .Width = 97}

        Dim dgvCareLinkUsersUseLocalTimeZone As New DataGridViewCheckBoxColumn With {
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .DataPropertyName = "UseLocalTimeZone",
            .HeaderText = $"Use Local{vbCrLf} Time Zone",
            .Name = NameOf(dgvCareLinkUsersUseLocalTimeZone),
            .Width = 86}

        Dim dgvCareLinkUsersAutoLogin As New DataGridViewCheckBoxColumn With {
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .DataPropertyName = "AutoLogin",
            .HeaderText = "Auto Login",
            .Name = NameOf(dgvCareLinkUsersAutoLogin),
            .Width = 65}

        Dim dgvCareLinkUsersCareLinkPartner As New DataGridViewCheckBoxColumn With {
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .DataPropertyName = "CareLinkPartner",
            .HeaderText = $"CareLink™ Partner",
            .Name = NameOf(dgvCareLinkUsersCareLinkPartner),
            .Width = 86}

        Dim dgvCareLinkPatientUserID As New DataGridViewTextBoxColumn With {
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .DataPropertyName = "CareLinkPatientUserID",
            .HeaderText = $"CareLink™ Patient UserID",
            .Name = NameOf(dgvCareLinkPatientUserID),
            .Width = 97}

        Dim dataGridViewColumns As DataGridViewColumn() = New DataGridViewColumn() {
            dgvCareLinkUsersID,
            dgvCareLinkUsersDeleteRow,
            dgvCareLinkUsersCareLinkUserName,
            dgvCareLinkUsersCareLinkPassword,
            dgvCareLinkUsersCountryCode,
            dgvCareLinkUsersUseLocalTimeZone,
            dgvCareLinkUsersAutoLogin,
            dgvCareLinkUsersCareLinkPartner,
            dgvCareLinkPatientUserID}
        dgv.Columns.AddRange(dataGridViewColumns)
        dgv.DataSource = Me.CareLinkUserDataRecordBindingSource
    End Sub

#End Region ' Dgv CareLink™ Users Events

#Region "Dgv Current User Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event
    '''  for <see cref="DgvCurrentUser"/> DataGridView.
    '''  This event is raised when the data binding operation is complete.
    '''  It clears the selection of all DataGridViews to ensure
    '''  no cells are selected after data binding.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvCurrentUser_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvCurrentUser.ColumnAdded

        e.Column.SortMode = DataGridViewColumnSortMode.NotSortable
        Dim alignment As DataGridViewContentAlignment =
            DataGridViewContentAlignment.MiddleLeft

        Dim padding As New Padding(all:=1)
        e.DgvColumnAdded(
            cellStyle:=New DataGridViewCellStyle().SetCellStyle(alignment, padding),
            forceReadOnly:=True,
            caption:=Nothing)
    End Sub

#End Region ' Dgv Current User Events

#Region "Dgv Insulin Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event
    '''  for the <see cref="DgvInsulin"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode,
    '''  visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvInsulin_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvInsulin.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            If HideColumn(Of Insulin)(item:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(Of Insulin)(.Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

    ''' <summary>
    '''  Handles the DataGridView's DataBindingComplete event.
    '''  This event is raised when the data binding operation is complete.
    '''  It clears the selection of all DataGridViews to ensure no cells
    '''  are selected after data binding.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  The DataGridViewBindingCompleteEventArgs containing the event data.
    ''' </param>
    ''' <remarks>
    '''  This event is used to customize the appearance of DataGridViews
    '''  after data binding is complete.
    ''' </remarks>
    Private Sub DgvInsulin_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) _
        Handles DgvInsulin.DataBindingComplete

        Dim dgv As DataGridView = CType(sender, DataGridView)
        HideUnneededColumns(dgv, columnName:=NameOf(Insulin.DeliveredExtendedAmount), value:="NaN")
        HideUnneededColumns(dgv, columnName:=NameOf(Insulin.ProgrammedExtendedAmount), value:="NaN")
        HideUnneededColumns(dgv, columnName:=NameOf(Insulin.ProgrammedDuration), value:="0")
        HideUnneededColumns(dgv, columnName:=NameOf(Insulin.EffectiveDuration), value:="0")
        Me.Dgv_DataBindingComplete(sender, e)
        dgv.ClearSelection()
    End Sub

#End Region ' Dgv Insulin Events

#Region "Dgv Last Alarm Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event
    '''  for the <see cref="DgvLastAlarm"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode,
    '''  visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Friend Sub DgvLastAlarm_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvLastAlarm.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If HideColumn(Of LastAlarm)(item:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(Of LastAlarm)(.Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Last Alarm Events

#Region "Dgv Limits Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event
    '''  for the <see cref="DgvLimits"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column,
    '''  such as sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvLimits.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If HideColumn(Of Limit)(item:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(Of Limit)(.Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Limits Events

#Region "Dgv Low Glucose Suspended Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for
    '''  the <see cref="DgvLowGlucoseSuspended"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode,
    '''  visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Friend Sub DgvLowGlucoseSuspended_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvLowGlucoseSuspended.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If HideColumn(Of LowGlucoseSuspended)(item:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(Of LowGlucoseSuspended)(.Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Low Glucose Suspended Events

#Region "Dgv Meal Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event
    '''  for the <see cref="DgvMeal"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column,
    '''  such as sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvMeal_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvMeal.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If HideColumn(Of Meal)(item:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(Of Meal)(.Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Meal Events

#Region "Dgv Sensor Bg Readings Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event
    '''  for the <see cref="DgvSensorBgReadings"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode, visibility,
    '''  and cell style.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvSensorBgReadings_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvSensorBgReadings.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If HideColumn(Of BgReading)(item:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(Of BgReading)(.Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Sensor Bg Readings Events

#Region "Dgv SGs Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellPainting"/> event
    '''  for the <see cref="DgvSGs"/> DataGridView.
    '''  This event is raised when a cell is painted, allowing custom rendering
    '''  of the cell's content.
    '''  Specifically, it draws a custom sort glyph in the header cells
    '''  of sortable columns.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewCellPaintingEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvSGs_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) _
        Handles DgvSGs.CellPainting

        Dim dgv As DataGridView = CType(sender, DataGridView)
        ' Only handle header cell 0
        If e.RowIndex = -1 AndAlso e.ColumnIndex = 0 Then
            Dim col As DataGridViewColumn = dgv.Columns(index:=e.ColumnIndex)
            ' Only for sortable columns
            If col.SortMode <> DataGridViewColumnSortMode.NotSortable Then
                e.PaintBackground(e.ClipBounds, cellsPaintSelectionBackground:=True)
                e.PaintContent(e.ClipBounds)

                ' Determine if this column is sorted and which direction
                Dim glyphDir As SortOrder = col.HeaderCell.SortGlyphDirection
                If glyphDir <> SortOrder.None Then
                    Dim color As Color = Color.White
                    Dim x As Integer = e.CellBounds.Right - 18
                    Dim y As Integer = e.CellBounds.Top + (e.CellBounds.Height \ 2) - 4
                    Dim points() As Point =
                        If(glyphDir = SortOrder.Ascending,
                           {New Point(x, y:=y + 6), New Point(x:=x + 8, y:=y + 6), New Point(x:=x + 4, y)},
                           {New Point(x, y), New Point(x:=x + 8, y), New Point(x:=x + 4, y:=y + 6)})

                    Dim g As Graphics = e.Graphics
                    g.FillPolygon(brush:=New SolidBrush(color), points)
                End If
                e.Handled = True
            End If
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event
    '''  for the <see cref="DgvSGs"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode,
    '''  visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvSGs_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvLastSensorGlucose.ColumnAdded, DgvSGs.ColumnAdded

        With e.Column
            .AutoSizeMode = If(e.Column.Name = "Message",
                               DataGridViewAutoSizeColumnMode.Fill,
                               DataGridViewAutoSizeColumnMode.AllCells)

            If HideColumn(Of SG)(item:= .Name) Then
                .Visible = False
            End If
            Dim dgv As DataGridView = CType(sender, DataGridView)
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(Of SG)(.Name),
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

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnHeaderMouseClick"/> event for the
    '''  <see cref="DgvSGs"/> DataGridView. This event is raised when a column header
    '''  is clicked, and sorts the DataGridView by the clicked column.
    '''  Only sorts when the first column header is clicked.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewCellMouseEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvSGs_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) _
        Handles DgvSGs.ColumnHeaderMouseClick

        If e.ColumnIndex <> 0 Then Exit Sub
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim dataGridViewColumn As DataGridViewColumn = dgv.Columns(index:=e.ColumnIndex)
        Dim direction As ListSortDirection = ListSortDirection.Ascending

        Select Case dataGridViewColumn.HeaderCell.SortGlyphDirection
            Case SortOrder.None, SortOrder.Ascending
                direction = ListSortDirection.Descending
            Case SortOrder.Descending
                direction = ListSortDirection.Ascending
        End Select

        dgv.Sort(dataGridViewColumn, direction)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.DataBindingComplete"/> event for the
    '''  <see cref="DgvSGs"/> DataGridView.
    '''  This event is raised when the data binding operation is complete.
    '''  It sets the column autosize modes and sort glyphs,
    '''  and ensures the last column is filled and wrapped.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewBindingCompleteEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvSGs_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) _
        Handles DgvSGs.DataBindingComplete

        Me.Dgv_DataBindingComplete(sender, e)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim lastColumnIndex As Integer = dgv.Columns.Count - 1
        dgv.Columns(index:=lastColumnIndex).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgv.Columns(index:=lastColumnIndex).DefaultCellStyle.WrapMode =
            DataGridViewTriState.True
        dgv.Columns(index:=0).HeaderCell.SortGlyphDirection =
            If(dgv.RowCount > 0,
               If(String.Equals(dgv.Rows(index:=0).Cells(index:=0).Value.ToString(), "1"),
                  SortOrder.Ascending,
                  SortOrder.Descending),
               SortOrder.None)

        dgv.ClearSelection()
    End Sub

#End Region ' Dgv SGs Events

#Region "Dgv Summary Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellFormatting"/> event
    '''  for the <see cref="DgvSummary"/> DataGridView.
    '''  This event is raised when a cell's value needs to be formatted for display.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewCellFormattingEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvSummary_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) _
        Handles DgvSummary.CellFormatting

        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim key As String =
            dgv.Rows(index:=e.RowIndex).Cells(columnName:="key").Value.ToString
        Dim eValue As String = Convert.ToString(e.Value)

        Select Case e.ColumnIndex
            Case 0
                Dim singleValue As Single = eValue.ParseSingleInvariant()
                If singleValue.IsSingleEqualToInteger(integerValue:=CInt(e.Value)) Then
                    dgv.CellFormattingSingleValue(e, digits:=0)
                Else
                    dgv.CellFormattingSingleValue(e, digits:=1)
                End If
            Case 1
                e.Value = eValue.Replace(oldValue:=":", newValue:=" : ")
            Case 2
                If e.Value IsNot Nothing Then
                    Select Case GetItemIndex(key)
                        ' Not Clickable Cells - Left
                        Case ServerDataEnum.conduitSerialNumber,
                             ServerDataEnum.lastConduitDateTime,
                             ServerDataEnum.systemStatusMessage,
                             ServerDataEnum.sensorState,
                             ServerDataEnum.timeFormat,
                             ServerDataEnum.bgUnits,
                             ServerDataEnum.lastSGTrend,
                             ServerDataEnum.sensorLifeText,
                             ServerDataEnum.sensorLifeIcon
                            e.CellStyle = e.CellStyle.SetCellStyle(
                                alignment:=DataGridViewContentAlignment.MiddleLeft,
                                padding:=New Padding(all:=1))

                        ' Not Clickable Cells - Center
                        Case ServerDataEnum.clientTimeZoneName,
                             ServerDataEnum.lastName,
                             ServerDataEnum.firstName,
                             ServerDataEnum.appModelType,
                             ServerDataEnum.conduitBatteryStatus,
                             ServerDataEnum.medicalDeviceFamily,
                             ServerDataEnum.medicalDeviceInformation,
                             ServerDataEnum.cgmInfo,
                             ServerDataEnum.approvedForTreatment,
                             ServerDataEnum.calibStatus,
                             ServerDataEnum.calFreeSensor,
                             ServerDataEnum.calibrationIconId,
                             ServerDataEnum.finalCalibration,
                             ServerDataEnum.pumpSuspended,
                             ServerDataEnum.conduitInRange,
                             ServerDataEnum.conduitMedicalDeviceInRange,
                             ServerDataEnum.conduitSensorInRange,
                             ServerDataEnum.gstCommunicationState,
                             ServerDataEnum.pumpCommunicationState
                            e.CellStyle = e.CellStyle.SetCellStyle(
                                alignment:=DataGridViewContentAlignment.MiddleCenter,
                                padding:=New Padding(all:=1))

                        ' Not Clickable - Data Dependent
                        Case ServerDataEnum.appModelNumber,
                             ServerDataEnum.transmitterPairedTime
                            If eValue = "NA" Then
                                e.CellStyle = e.CellStyle.SetCellStyle(
                                    alignment:=DataGridViewContentAlignment.MiddleCenter,
                                    padding:=New Padding(all:=1))
                                e.Value = "N/A"
                            Else
                                e.CellStyle =
                                    e.CellStyle.SetCellStyle(
                                    alignment:=DataGridViewContentAlignment.MiddleRight,
                                    padding:=New Padding(all:=1))
                            End If

                        ' Not Clickable Cells - Right
                        Case ServerDataEnum.currentServerTime,
                             ServerDataEnum.conduitBatteryLevel,
                             ServerDataEnum.lastConduitUpdateServerDateTime,
                             ServerDataEnum.medicalDeviceTime,
                             ServerDataEnum.lastMedicalDeviceDataUpdateServerTime,
                             ServerDataEnum.timeToNextCalibrationMinutes,
                             ServerDataEnum.timeToNextCalibrationRecommendedMinutes,
                             ServerDataEnum.timeToNextCalibHours,
                             ServerDataEnum.sensorDurationHours,
                             ServerDataEnum.systemStatusTimeRemaining,
                             ServerDataEnum.gstBatteryLevel,
                             ServerDataEnum.reservoirLevelPercent,
                             ServerDataEnum.reservoirAmount,
                             ServerDataEnum.pumpBatteryLevelPercent,
                             ServerDataEnum.reservoirRemainingUnits,
                             ServerDataEnum.maxAutoBasalRate,
                             ServerDataEnum.maxBolusAmount,
                             ServerDataEnum.sgBelowLimit,
                             ServerDataEnum.lastSensorTime,
                             ServerDataEnum.averageSGFloat,
                             ServerDataEnum.averageSG,
                             ServerDataEnum.belowHypoLimit,
                             ServerDataEnum.aboveHyperLimit,
                             ServerDataEnum.timeInRange
                            e.CellStyle = e.CellStyle.SetCellStyle(
                                alignment:=DataGridViewContentAlignment.MiddleRight,
                                padding:=New Padding(all:=1))

                         ' Not Clickable Cells - Integer with comma, align Right
                        Case ServerDataEnum.timeToNextEarlyCalibrationMinutes,
                             ServerDataEnum.sensorDurationMinutes
                            e.Value = $"{CInt(e.Value):N0}"
                            e.CellStyle = e.CellStyle.SetCellStyle(
                                alignment:=DataGridViewContentAlignment.MiddleRight,
                                padding:=New Padding(left:=0, top:=1, right:=1, bottom:=1))

                            ' Clickable Cells - Center
                        Case ServerDataEnum.pumpBannerState,
                             ServerDataEnum.therapyAlgorithmState,
                             ServerDataEnum.lastAlarm,
                             ServerDataEnum.activeInsulin,
                             ServerDataEnum.basal,
                             ServerDataEnum.lastSG,
                             ServerDataEnum.limits,
                             ServerDataEnum.markers,
                             ServerDataEnum.sgs,
                             ServerDataEnum.notificationHistory
                            e.CellStyle = e.CellStyle.SetCellStyle(
                                alignment:=DataGridViewContentAlignment.MiddleCenter,
                                padding:=New Padding(all:=1))
                            dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Black, emIncrease:=1)
                        Case Else
                            Stop
                    End Select
                End If
            Case Else
        End Select
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellMouseClick"/> event for the
    '''  <see cref="DgvSummary"/> DataGridView.
    '''  When a cell is clicked, checks if the value starts with <c>ClickToShowDetails</c>.
    '''  If so, navigates to the appropriate tab or page in the UI based
    '''  on the key in the clicked row.
    '''  This allows users to quickly jump to detailed views for items such as
    '''  last sensor glucose, alarms, insulin, sensor glucose values, limits, markers,
    '''  notification history, therapy algorithm state, pump banner state, or basal.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, typically the <see cref="DgvSummary"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewCellMouseEventArgs"/> that contains the event data,
    '''  including the row and column indices.
    ''' </param>
    Private Sub DgvSummary_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) _
        Handles DgvSummary.CellMouseClick

        If e.RowIndex < 0 OrElse _updating Then Exit Sub
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim value As String =
            dgv.Rows(index:=e.RowIndex).Cells(index:=e.ColumnIndex).Value.ToString
        If value.StartsWith(value:=ClickToShowDetails) Then
            With Me.TabControlPage1
                Dim key As String =
                    dgv.Rows(index:=e.RowIndex).Cells(columnName:="key").Value.ToString
                Select Case key.GetItemIndex()
                    Case ServerDataEnum.activeInsulin
                        .SelectedIndex = GetTabIndexFromName(tabPageName:=NameOf(TabPage05ActiveInsulin))
                    Case ServerDataEnum.basal
                        .SelectedIndex = GetTabIndexFromName(tabPageName:=NameOf(TabPage06Basal))
                    Case ServerDataEnum.lastAlarm
                        .SelectedIndex = GetTabIndexFromName(tabPageName:=NameOf(TabPage07LastAlarm))
                    Case ServerDataEnum.lastSG
                        .SelectedIndex = GetTabIndexFromName(tabPageName:=NameOf(TabPage08LastSG))
                    Case ServerDataEnum.limits
                        .SelectedIndex = GetTabIndexFromName(tabPageName:=NameOf(TabPage09Limits))
                    Case ServerDataEnum.notificationHistory
                        .SelectedIndex =
                            If(key = "activeNotification",
                               GetTabIndexFromName(tabPageName:=NameOf(TabPage10NotificationActive)),
                               GetTabIndexFromName(tabPageName:=NameOf(TabPage11NotificationsCleared)))

                    Case ServerDataEnum.pumpBannerState
                        .SelectedIndex = GetTabIndexFromName(tabPageName:=NameOf(TabPage12PumpBannerState))
                    Case ServerDataEnum.sgs
                        .SelectedIndex = GetTabIndexFromName(tabPageName:=NameOf(TabPage13SensorGlucose))
                    Case ServerDataEnum.therapyAlgorithmState
                        .SelectedIndex = GetTabIndexFromName(tabPageName:=NameOf(TabPage14TherapyAlgorithmState))
                    Case ServerDataEnum.markers
                        Dim page As Integer = _lastMarkerTabLocation.Page
                        Dim tab As Integer = _lastMarkerTabLocation.Tab
                        If page = 0 Then
                            _lastMarkerTabLocation = New TabLocation(page:=1, tab:=0)
                        End If
                        Me.TabControlPage2.SelectedIndex = _lastMarkerTabLocation.Tab
                        .Visible = False
                End Select
            End With
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event
    '''  for the <see cref="DgvSummary"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as
    '''  sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvSummary_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvSummary.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(Of SummaryRecord)(.Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.SelectionChanged"/> event
    '''  for the <see cref="DgvSummary"/> DataGridView.
    '''  This event is raised when the selection in the DataGridView changes.
    '''  It updates the previous row and column indices to track the last selected cell.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">A <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub DgvSummary_SelectionChanged(sender As Object, e As EventArgs) _
        Handles DgvSummary.SelectionChanged
        Dim dgv As DataGridView = DirectCast(sender, DataGridView)
        If dgv.CurrentCell IsNot Nothing AndAlso
            dgv.CurrentCell.RowIndex >= 0 AndAlso
            dgv.CurrentCell.ColumnIndex > 0 Then
            _dgvSummaryPrevRowIndex = dgv.CurrentCell.RowIndex
            _dgvSummaryPrevColIndex = dgv.CurrentCell.ColumnIndex
        End If
    End Sub

#End Region ' Dgv Summary Events

#Region "Dgv Therapy Algorithm State Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellFormatting"/> event for
    '''  the <see cref="DgvTherapyAlgorithmState"/> DataGridView.
    '''  This event is raised when a cell's value needs to be formatted for display.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewCellFormattingEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvTherapyAlgorithmState_CellFormatting(
        sender As Object,
        e As DataGridViewCellFormattingEventArgs) _
        Handles DgvTherapyAlgorithmState.CellFormatting

        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim key As String
        If e.Value IsNot Nothing AndAlso e.ColumnIndex = 2 Then
            key = dgv.Rows(index:=e.RowIndex).Cells(columnName:="key").Value.ToString
            Select Case key
                Case NameOf(TherapyAlgorithmState.AutoModeReadinessState),
                     NameOf(TherapyAlgorithmState.AutoModeShieldState),
                     NameOf(TherapyAlgorithmState.PlgmLgsState)

                    e.CellStyle = e.CellStyle.SetCellStyle(
                        alignment:=DataGridViewContentAlignment.MiddleLeft,
                        padding:=New Padding(all:=1))

                Case NameOf(TherapyAlgorithmState.SafeBasalDuration),
                     NameOf(TherapyAlgorithmState.WaitToCalibrateDuration)
                    Dim totalMinutes As Integer = CInt(e.Value)
                    Dim hours As Integer = totalMinutes \ 60
                    Dim minutes As Integer = totalMinutes Mod 60
                    e.Value = $"{hours}:{minutes:D2}"
                    e.CellStyle = e.CellStyle.SetCellStyle(
                        alignment:=DataGridViewContentAlignment.MiddleRight,
                        padding:=New Padding(left:=0, top:=1, right:=1, bottom:=1))

                Case Else
                    Stop
            End Select
        End If
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for the
    '''  <see cref="DgvTherapyAlgorithmState"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode,
    '''  visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvTherapyAlgorithmState_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvTherapyAlgorithmState.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If HideColumn(Of TherapyAlgorithmState)(item:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(Of BgReading)(.Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Therapy Algorithm State Events

#Region "Dgv Time Change Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for
    '''  the <see cref="DgvTimeChange"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode,
    '''  visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    ''' </param>
    Private Sub DgvTimeChange_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) _
        Handles DgvTimeChange.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If HideColumn(Of TimeChange)(item:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=GetCellStyle(Of TimeChange)(.Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Time Change Events

#End Region ' DataGridView Events

#Region "Form Events"

    ''' <summary>
    '''  Handles the <see cref="Form.FormClosing"/> event for the main form.
    '''  Performs cleanup tasks such as disposing of the notification icon,
    '''  terminating the <see cref="WebView2"/> process, and deleting the WebView2
    '''  cache directory when the form is closing.
    ''' </summary>
    ''' <param name="sender">The source of the event, <see cref="Form"/>.</param>
    ''' <param name="e">
    '''  A <see cref="FormClosingEventArgs"/> that contains the event data.
    ''' </param>
    ''' <remarks>
    '''  Ensures proper resource cleanup before the application exits,
    '''  including killing the WebView2 process
    '''  and removing its cache directory if present.
    ''' </remarks>
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) _
        Handles MyBase.FormClosing

        Me.NotifyIcon1?.Dispose()
        If _webView2ProcessId > 0 Then
            Dim webViewProcess As Process =
                Process.GetProcessById(processId:=_webView2ProcessId)
            ' TODO: dispose of the WebView2 control
            'LoginDialog.WebView21.Dispose()
            webViewProcess.Kill()
            webViewProcess.WaitForExit(milliseconds:=3_000)
        End If

        If Directory.Exists(path:=GetWebViewDirectory()) Then
            Try
                Directory.Delete(path:=GetWebViewDirectory(), recursive:=True)
            Catch
                Stop
                ' Ignore errors here
            End Try
        End If
    End Sub

    ''' <summary>
    '''  Main form for the CareLink™ application.
    '''  Handles initialization, event wiring, chart setup, DataGridView formatting,
    '''  and user interaction logic.
    ''' </summary>
    ''' <param name="sender">The source of the event, <see cref="Form"/>.</param>
    ''' <param name="e">The EventArgs containing the event data.</param>
    ''' <remarks>
    '''  This form manages the primary UI, including charts, data grids,
    '''  user settings, and notification icons.
    '''  It coordinates loading and saving user data, updating UI elements,
    '''  and responding to user and system events.
    ''' </remarks>
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.UpgradeRequired Then
            My.Settings.Upgrade()
            My.Settings.UpgradeRequired = False
            My.Settings.Save()
        End If
        Encoding.RegisterProvider(provider:=CodePagesEncodingProvider.Instance)
        If Not Directory.Exists(path:=GetProjectDataDirectory()) Then
            Dim lastError As String = $"Can't create required project directories!"
            Directory.CreateDirectory(path:=GetProjectDataDirectory())
            Directory.CreateDirectory(path:=GetSettingsDirectory())
        End If

        If Not Directory.Exists(path:=GetSettingsDirectory()) Then
            Directory.CreateDirectory(path:=GetSettingsDirectory())
        End If

        If File.Exists(path:=GetAllUsersCsvPath()) Then
            s_allUserSettingsData.LoadUserRecords()
        Else
            My.Settings.AutoLogin = False
        End If

        Me.MenuOptionsShowChartLegends.Checked = My.Settings.SystemShowLegends
        Me.MenuOptionsSpeechHelpShown.Checked = My.Settings.SystemSpeechHelpShown
        Me.InitializeDgvCareLinkUsers(dgv:=Me.DgvCareLinkUsers)
        s_formLoaded = True
        Me.MenuOptionsAudioAlerts.Checked = My.Settings.SystemAudioAlertsEnabled
        Me.MenuOptionsSpeechRecognitionEnabled.Checked =
            My.Settings.SystemSpeechRecognitionThreshold < 1
        Me.SetSpeechRecognitionConfidenceThreshold()
        Me.MenuOptionsConfigureTiTR.Text =
            $"Configure TiTR ({My.Forms.OptionsConfigureTiTR.GetTiTrMsg()})..."
        AddHandler My.Settings.SettingChanging, AddressOf Me.MySettings_SettingChanging

        If File.Exists(path:=GetGraphColorsFileNameWithPath()) Then
            GetColorDictionaryFromFile()
        Else
            WriteColorDictionaryToFile()
        End If

        Me.InsulinTypeLabel.Text = s_insulinTypes.Keys(index:=1)
        If String.IsNullOrWhiteSpace(value:=GetWebViewDirectory()) Then
            s_webView2CacheDirectory =
                Path.Join(GetProjectWebCache(), Guid.NewGuid().ToString())
            Directory.CreateDirectory(path:=s_webView2CacheDirectory)
        End If

        Dim style As FontStyle = FontStyle.Bold
        Dim emSize As Single = 12.0F
        Me.DgvBasalPerHour.Font = New Font(FamilyName, emSize, style)
        Dim currentHeaderStyle As DataGridViewCellStyle =
            Me.DgvBasalPerHour.ColumnHeadersDefaultCellStyle.Clone
        currentHeaderStyle.Font = New Font(FamilyName, emSize, style)
        Me.DgvBasalPerHour.ColumnHeadersDefaultCellStyle = currentHeaderStyle
        Me.DgvBasalPerHour.DefaultCellStyle = New DataGridViewCellStyle With {
            .Font = New Font(FamilyName, emSize, style:=FontStyle.Regular)}
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Form.Resize"/> event for the main form.
    '''  This event is raised when the form is resized.
    '''  It checks if the form is minimized and shows a notification icon if it is.
    ''' </summary>
    ''' <param name="sender">The source of the event, <see cref="Form"/>.</param>
    ''' <param name="e">The EventArgs containing the event data.</param>
    ''' <remarks>
    '''  This event is used to manage the visibility of the notification
    '''  icon when the form is minimized.
    ''' </remarks>
    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            Me.NotifyIcon1.Visible = True
            If Me.NotifyIcon1.BalloonTipText.Length > 0 Then
                Me.NotifyIcon1.ShowBalloonTip(timeout:=1000)
            End If
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Form.Shown"/> event for the main form.
    '''  This event is raised when the form is first shown.
    '''  It performs additional initialization tasks such as setting up labels,
    '''  checking for updates, and loading data.
    ''' </summary>
    ''' <param name="sender">The source of the event, <see cref="Form"/>.</param>
    ''' <param name="e">The EventArgs containing the event data.</param>
    ''' <remarks>
    '''  This event is used to finalize the setup of the form after it has been
    '''  displayed to the user.
    ''' </remarks>
    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Me.Fix(c:=Me)

        Me.CurrentSgLabel.Parent = Me.SmartGuardShieldPictureBox
        Me.ShieldUnitsLabel.Parent = Me.SmartGuardShieldPictureBox
        Me.ShieldUnitsLabel.BackColor = Color.Transparent
        Me.SensorDaysLeftLabel.Parent = Me.SensorTimeLeftPictureBox
        Me.SensorMessageLabel.Parent = Me.SmartGuardShieldPictureBox
        Me.SensorDaysLeftLabel.BackColor = Color.Transparent
        s_useLocalTimeZone = My.Settings.UseLocalTimeZone
        Me.MenuOptionsUseLocalTimeZone.Checked = s_useLocalTimeZone
        CheckForUpdatesAsync(reportSuccessfulResult:=False)

        Me.ToolTip1.SetToolTip(control:=Me.TirComplianceLabel, caption:=CheckComplianceValues)
        Me.ToolTip1.SetToolTip(control:=Me.LowTirComplianceLabel, caption:=TirToolTip)
        Me.ToolTip1.SetToolTip(control:=Me.HighTirComplianceLabel, caption:=TirToolTip)

        Me.SetDgvCustomHeadersVisualStyles()

#Region "Status Strip Colors"

        Me.StatusStrip1.BackColor = Me.MenuStrip1.BackColor

        Me.LastUpdateTimeToolStripStatusLabel.BackColor = Me.MenuStrip1.BackColor
        Me.LastUpdateTimeToolStripStatusLabel.ForeColor = Me.MenuStrip1.ForeColor

        Me.LoginStatus.BackColor = Me.MenuStrip1.BackColor
        Me.LoginStatus.ForeColor = Me.MenuStrip1.ForeColor

        Me.StatusStripSpacerRight.BackColor = Me.MenuStrip1.BackColor

        Me.StatusStripSpeech.BackColor = Me.MenuStrip1.BackColor
        Me.StatusStripSpeech.ForeColor = Me.MenuStrip1.ForeColor

        Me.TimeZoneToolStripStatusLabel.BackColor = Me.MenuStrip1.BackColor
        Me.TimeZoneToolStripStatusLabel.ForeColor = Me.MenuStrip1.ForeColor

        Me.UpdateAvailableStatusStripLabel.BackColor = Me.MenuStrip1.BackColor
        Me.UpdateAvailableStatusStripLabel.ForeColor = Color.Red

#End Region ' Status Strip Colors

        Me.PositionControlsInPanel()

#If NET9_0 Then

#Region "Tab Page Colors"

        Me.TabControlPage1.DrawMode = TabDrawMode.OwnerDrawFixed
        Me.TabControlPage2.DrawMode = TabDrawMode.OwnerDrawFixed

#End Region ' Tab Page Colors

#End If ' NET9_0

        Me.NotifyIcon1.Visible = True
        Application.DoEvents()
        Me.NotifyIcon1.Visible = False
        Application.DoEvents()

        If DoOptionalLoginAndUpdateData(
            owner:=Me,
            updateAllTabs:=False,
            fileToLoad:=FileToLoadOptions.NewUser) Then

            Me.UpdateAllTabPages(fromFile:=False)
        End If

    End Sub

    ''' <summary>
    '''  Handles the <see cref="Button.Click"/> event for
    '''  the <see cref="SerialNumberButton"/> control.
    '''  It switches to the Serial Number tab and scrolls to the last row
    '''  in the DataGridView. Highlights the row containing
    '''  "<see cref="medicalDeviceInformation"/>" in the second column.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, typically the SerialNumberButton.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    ''' <remarks>
    '''  This event is used to navigate to the Serial Number tab and
    '''  focus on the relevant data.
    ''' </remarks>
    Private Sub SerialNumberButton_Click(sender As Object, e As EventArgs) _
        Handles SerialNumberButton.Click

        Me.TabControlPage1.SelectedIndex = 3
        Me.TabControlPage1.Visible = True
        Dim dgv As DataGridView =
            CType(Me.TabControlPage1.TabPages(index:=3).Controls(index:=0), DataGridView)
        For Each row As DataGridViewRow In dgv.Rows
            If row.Cells(index:=1).FormattedValue.ToString _
                  .StartsWith(value:="medicalDeviceInformation") Then
                dgv.CurrentCell = dgv.Rows(row.Index).Cells(index:=1)
                _dgvSummaryPrevRowIndex = dgv.CurrentCell.RowIndex
                _dgvSummaryPrevColIndex = dgv.CurrentCell.ColumnIndex
                dgv.Rows(row.Index).Selected = True
                dgv.FirstDisplayedScrollingRowIndex = row.Index
                Exit For
            End If
        Next
    End Sub

#End Region ' Form Events

#Region "Form1 Menu Events"

#Region "Start Here Menu Events"

    ''' <summary>
    '''  Handles the <see cref="MenuStartHere.DropDownOpening"/> event for
    '''  the Start Here menu. This event is raised when the Start Here menu
    '''  is about to be displayed. It enables or disables menu items
    '''  based on the current state of the application.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a ToolStripMenuItem control.
    ''' </param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    Private Sub MenuStartHere_DropDownOpening(sender As Object, e As EventArgs) _
        Handles MenuStartHere.DropDownOpening

        Me.MenuStartHereLoadSavedDataFile.Enabled =
            AnyMatchingFiles(path:=GetProjectDataDirectory(),
            searchPattern:=$"CareLink*.json")
        Me.MenuStartHereSaveSnapshotFile.Enabled = Not RecentDataEmpty()
        Me.MenuStartHereUseExceptionReport.Visible = AnyMatchingFiles(
            path:=GetProjectDataDirectory(),
            searchPattern:=$"{BaseErrorReportName}*.txt")

        Dim userPdfExists As Boolean =
            Not (String.IsNullOrWhiteSpace(s_userName) OrElse
            Not AnyMatchingFiles(
                path:=GetSettingsDirectory(),
                searchPattern:=$"{s_userName}Settings.pdf"))

        Me.MenuStartHereShowPumpSetup.Enabled = userPdfExists AndAlso
                                                CurrentPdf IsNot Nothing AndAlso
                                                CurrentPdf.IsValid

        Dim settingExist As Boolean = Directory.Exists(path:=GetSettingsDirectory)

        Dim downloadFilesExists As Boolean =
           Directory.GetFiles(path:=GetDownloadsDirectory(),
                              searchPattern:=$"*.pdf").Length > 0

        Me.MenuStartHereManuallyImportDeviceSettings.Enabled = downloadFilesExists

        ' The menu item For cleaning up obsolete files
        ' (MenuStartHereCleanUpObsoleteFiles) is only enabled,
        ' when the application Is the only instance running, as a safety precaution.
        Me.MenuStartHereCleanUpObsoleteFiles.Enabled =
            Process.GetProcessesByName(processName:=_processName).Length = 1
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Form.Click"/> event for the Start Here menu item.
    '''  This event is raised when the Start Here menu item is clicked.
    '''  It opens the Start Here dialog to guide the user through the initial setup process.
    ''' </summary>
    ''' <param name="sender">The source of the event, a ToolStripMenuItem control.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    Private Sub MenuStartHereCleanUpObsoleteFiles_Click(sender As Object, e As EventArgs) _
        Handles MenuStartHereCleanUpObsoleteFiles.Click

        ' Opens a dialog to clean up obsolete files.
        Using dialog As New CleanupStaleFilesDialog With {.Text = $"Cleanup Obsolete Files"}
            dialog.ShowDialog()
        End Using
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Form.Click"/> event for the Exit menu item.
    '''  This event is raised when the Exit menu item is clicked.
    '''  It closes the application.
    ''' </summary>
    ''' <param name="sender">The source of the event, a ToolStripMenuItem control.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    Private Sub MenuStartHereExit_Click(sender As Object, e As EventArgs) _
        Handles MenuStartHereExit.Click

        Me.Close()
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Form.Click"/> event for the
    '''  Manually Import Device Settings menu item.
    '''  This event is used to prompt the user to select a settings PDF file for import.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="ToolStripMenuItem"/> control.
    ''' </param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    Private Sub MenuStartHereManuallyImportDeviceSettings_Click(sender As Object, e As EventArgs) _
        Handles MenuStartHereManuallyImportDeviceSettings.Click

        Dim folder As String =
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
        Dim initialDirectory As String = $"{folder}\Downloads\"
        Using openFileDialog1 As New System.Windows.Forms.OpenFileDialog With {
            .AddExtension = True,
            .AddToRecent = False,
            .CheckFileExists = True,
            .CheckPathExists = True,
            .DefaultExt = "pdf",
            .Filter = $"Settings file (*.pdf)|*.pdf",
            .InitialDirectory = initialDirectory,
            .Multiselect = False,
            .ReadOnlyChecked = True,
            .RestoreDirectory = True,
            .ShowPreview = False,
            .SupportMultiDottedExtensions = False,
            .Title = $"Select downloaded CareLink™ Settings file.",
            .ValidateNames = True}

            If openFileDialog1.ShowDialog(owner:=Me) <> DialogResult.OK Then
                Return
            End If
            Try
                Me.Cursor = Cursors.WaitCursor
                Application.DoEvents()
                Dim pdfSettingsRecord As New PdfSettingsRecord(openFileDialog1.FileName)
                Me.Cursor = Cursors.Default
                Application.DoEvents()

                If pdfSettingsRecord.IsValid Then
                    File.Move(
                        sourceFileName:=openFileDialog1.FileName,
                        destFileName:=GetUserPdfPath(),
                        overwrite:=True)
                    Exit Sub
                Else
                    MsgBox(
                        heading:=$"Device Setting PDF file Is invalid",
                        prompt:=GetUserPdfPath(),
                        buttonStyle:=MsgBoxStyle.OkOnly,
                        title:="Invalid Settings PDF File")
                End If
            Catch ex As Exception
                ' Ignore errors here
            End Try
        End Using
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Form.Click"/> event for the Show Pump Setup menu item.
    '''  This event is raised when the Show Pump Setup menu item is clicked.
    '''  It opens a dialog to display the pump setup information from
    '''  the user's settings PDF file.
    ''' </summary>
    ''' <param name="sender">The source of the event, a ToolStripMenuItem control.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    Private Sub MenuStartHereShowPumpSetup_Click(sender As Object, e As EventArgs) _
        Handles MenuStartHereShowPumpSetup.Click

        If File.Exists(path:=GetUserPdfPath()) Then
            If CurrentPdf.IsValid Then
                SetServerUpdateTimer(Start:=False)
                Using dialog As New PumpSetupDialog
                    dialog.Pdf = CurrentPdf
                    dialog.ShowDialog(owner:=Me)
                End Using
            End If

            ' If the PDF file is not valid after setup, show a message box to the user.
            If CurrentPdf.IsValid Then
                SetServerUpdateTimer(Start:=True)
            Else
                MsgBox(
                    heading:=$"Device Setting PDF file Is invalid",
                    prompt:=GetUserPdfPath(),
                    buttonStyle:=MsgBoxStyle.OkOnly,
                    title:="Invalid Settings PDF File")
            End If
        Else
            MsgBox(
                heading:=$"Device Setting PDF file Is missing!",
                prompt:=GetUserPdfPath(),
                buttonStyle:=MsgBoxStyle.OkOnly,
                title:="Missing Settings PDF File")
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Form.Click"/> event for the Save Snapshot File menu item.
    '''  This event is raised when the Save Snapshot File menu item is clicked.
    '''  It saves the current patient data to a JSON file.
    ''' </summary>
    ''' <param name="sender">The source of the event, a ToolStripMenuItem control.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    ''' <remarks>
    '''  The saved file will have a unique name based on the current date and time.
    ''' </remarks>
    Private Sub MenuStartHereSnapshotSave_Click(sender As Object, e As EventArgs) _
        Handles MenuStartHereSaveSnapshotFile.Click

        If RecentDataEmpty() Then Exit Sub
        Dim path As String = GetUniqueDataFileName(
            baseName:=BaseSnapshotName,
            cultureName:=CurrentDateCulture.Name,
            extension:="json",
            mustBeUnique:=True).withPath
        File.WriteAllTextAsync(path, contents:=CleanPatientData())
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Form.Click"/> event for the Use Exception Report menu item.
    '''  This event is raised when the Use Exception Report menu item is clicked.
    '''  It allows the user to load a saved error report file and process it.
    ''' </summary>
    ''' <param name="sender">The source of the event, a ToolStripMenuItem control.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    ''' <remarks>
    '''  The user can select an error report file to load and process.
    ''' </remarks>
    Private Sub MenuStartHereUseExceptionReport_Click(sender As Object, e As EventArgs) _
        Handles MenuStartHereUseExceptionReport.Click

        Dim fileList As String() = Directory.GetFiles(
            path:=GetProjectDataDirectory(),
            searchPattern:=$"{BaseErrorReportName}*.txt")
        Using openFileDialog1 As New System.Windows.Forms.OpenFileDialog With {
            .AddExtension = True,
            .AddToRecent = False,
            .CheckFileExists = True,
            .CheckPathExists = True,
            .DefaultExt = "txt",
            .FileName = If(fileList.Length > 0,
                           Path.GetFileName(path:=fileList(0)),
                           "CareLink"),
            .Filter = $"Error files (*.txt)|{BaseErrorReportName}*.txt",
            .InitialDirectory = GetProjectDataDirectory(),
            .Multiselect = False,
            .ReadOnlyChecked = True,
            .RestoreDirectory = True,
            .ShowPreview = False,
            .SupportMultiDottedExtensions = False,
            .Title = $"Select CareLink™ saved snapshot to load",
            .ValidateNames = True}

            If openFileDialog1.ShowDialog(owner:=Me) = DialogResult.OK Then
                Try
                    Dim fileNameWithPath As String = openFileDialog1.FileName
                    SetServerUpdateTimer(Start:=False)
                    If File.Exists(fileNameWithPath) Then
                        RecentData = New Dictionary(Of String, String)
                        ExceptionHandlerDialog.ReportFileNameWithPath = fileNameWithPath
                        If ExceptionHandlerDialog.ShowDialog(owner:=Me) = DialogResult.OK Then
                            ExceptionHandlerDialog.ReportFileNameWithPath = ""
                            Try
                                Dim json As String = ExceptionHandlerDialog.LocalRawData
                                PatientDataElement =
                                    JsonSerializer.Deserialize(Of JsonElement)(json)
                                DeserializePatientElement()
                                Me.TabControlPage2.Visible = True
                                Me.TabControlPage1.Visible = True
                            Catch ex As Exception
                                Dim str As String = ex.DecodeException()
                                MessageBox.Show(
                                    text:=$"Error reading data file. Original error: {str}")
                            End Try
                            CurrentDateCulture =
                                openFileDialog1.FileName.ExtractCulture(
                                    FixedPart:="CareLink",
                                    fuzzy:=True)
                            Me.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                            Dim file As String =
                                Path.GetFileName(fileNameWithPath)
                            Me.Text = $"{SavedTitle} Using file {file}"
                            Dim epochDateTime As Date =
                                s_lastMedicalDeviceDataUpdateServerEpoch.Epoch2PumpDateTime
                            Me.SetLastUpdateTime(
                                msg:=epochDateTime.ToShortDateTimeString,
                                suffixMessage:="from file",
                                highLight:=False,
                                isDaylightSavingTime:=epochDateTime.IsDaylightSavingTime)
                            SetUpCareLinkUser()

                            Dim subName As String = NameOf(UpdateAllTabPages)
                            Try
                                Me.UpdateAllTabPages(fromFile:=True)
                            Catch ex As Exception
                                MessageBox.Show(
                                    text:=$"Error in {subName}. Original error: {ex.Message}")
                            End Try
                            Try
                                Me.UpdateAllTabPages(fromFile:=True)
                            Catch ex As Exception
                                MessageBox.Show(
                                    text:=$"Error in {subName}. Original error: {ex.Message}")
                            End Try
                        End If
                    End If
                Catch ex As Exception
                    Dim str As String = ex.DecodeException()
                    MessageBox.Show(
                        text:=$"Cannot read file from disk. Original error: {str}")
                End Try
            End If
        End Using
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Form.Click"/> event for the Use Last Saved File menu item.
    '''  This event is raised when the Use Last Saved File menu item is clicked.
    '''  It loads the last saved data file and updates the application state accordingly.
    ''' </summary>
    ''' <param name="sender">The source of the event, a ToolStripMenuItem control.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    ''' <remarks>
    '''  The last saved file will be loaded and processed to update the application state.
    ''' </remarks>
    Private Sub MenuStartHereUseLastSavedFile_Click(sender As Object, e As EventArgs) _
        Handles MenuStartHereUseLastSavedFile.Click

        Dim success As Boolean = DoOptionalLoginAndUpdateData(
            owner:=Me,
            updateAllTabs:=True,
            fileToLoad:=FileToLoadOptions.LastSaved)
        Me.MenuStartHereSaveSnapshotFile.Enabled = Not success
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Form.Click"/> event for the User Login menu item.
    '''  This event is raised when the User Login menu item is clicked.
    '''  It allows the user to log in to their CareLink™ account and
    '''  update the application state accordingly.
    ''' </summary>
    ''' <param name="sender">The source of the event, a ToolStripMenuItem control.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    ''' <remarks>
    '''  The user will be prompted to log in, and their data will be updated
    '''  based on their account information.
    ''' </remarks>
    Private Sub MenuStartHereUserLogin_Click(sender As Object, e As EventArgs) _
        Handles MenuStartHereUserLogin.Click

        Dim success As Boolean = DoOptionalLoginAndUpdateData(
            owner:=Me,
            updateAllTabs:=True,
            fileToLoad:=FileToLoadOptions.NewUser)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Form.Click"/> event for the Use Saved Data File menu item.
    '''  This event is raised when the Use Saved Data File menu item is clicked.
    '''  It allows the user to load a saved data file and update the application
    '''  state accordingly.
    ''' </summary>
    ''' <param name="sender">The source of the event, a ToolStripMenuItem control.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    ''' <remarks>
    '''  The user can select a saved data file to load and process.
    ''' </remarks>
    Private Sub MenuStartHereUseSavedDataFile_Click(sender As Object, e As EventArgs) _
        Handles MenuStartHereLoadSavedDataFile.Click

        Dim success As Boolean = DoOptionalLoginAndUpdateData(
            owner:=Me,
            updateAllTabs:=True,
            fileToLoad:=FileToLoadOptions.Snapshot)
        Me.MenuStartHereLoadSavedDataFile.Enabled = Not success
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Form.Click"/> event for the Use Test Data menu item.
    '''  This event is raised when the Use Test Data menu item is clicked.
    '''  It loads test data into the application and updates the state accordingly.
    ''' </summary>
    ''' <param name="sender">The source of the event, a ToolStripMenuItem control.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    ''' <remarks>
    '''  The test data will be loaded and processed to simulate a CareLink™ environment.
    ''' </remarks>
    Private Sub MenuStartHereUseTestData_Click(sender As Object, e As EventArgs) _
        Handles MenuStartHereUseTestData.Click

        Dim success As Boolean = DoOptionalLoginAndUpdateData(
            owner:=Me,
            updateAllTabs:=True,
            fileToLoad:=FileToLoadOptions.TestData)
        Me.MenuStartHereSaveSnapshotFile.Enabled = Not success
    End Sub

#End Region ' Start Here Menu Events

#Region "Menus Options"

    ''' <summary>
    '''  Gets the selected speech recognition minimum confidence value from the menu.
    ''' </summary>
    ''' <returns>
    '''  The selected confidence value as a <see cref="Double"/>.
    '''  Returns 100 if no item is checked or no numeric value is found.
    ''' </returns>
    Private Function GetSpeechConfidenceValue() As Double
        For Each item As ToolStripMenuItem In
            Me.MenuOptionsSpeechRecognitionEnabled.DropDownItems

            If IsNumeric(Expression:=item.Text) AndAlso item.Checked Then
                Return CDbl(item.Text)
            End If
        Next
        Return 100
    End Function

    ''' <summary>
    '''  Handles the <see cref="MenuOptions.DropDownOpening"/> event
    '''  for the <see cref="MenuOptions"/> menu. Enables or disables the Edit Pump Settings
    '''  menu item based on debugger state or user name.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The event data.</param>
    <DebuggerNonUserCode()>
    Private Sub MenuOptions_DropDownOpening(sender As Object, e As EventArgs) _
    Handles MenuOptions.DropDownOpening

        Me.MenuOptionsEditPumpSettings.Enabled =
            Debugger.IsAttached OrElse
            Not String.IsNullOrWhiteSpace(value:=CurrentUser?.UserName)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="MenuOptionsAudioAlerts.Click"/> event.
    '''  This event is raised when the Audio Alerts menu item is clicked.
    '''  It toggles the audio alerts setting and initializes or
    '''  cancels speech recognition accordingly.
    ''' </summary>
    ''' <param name="sender">The source of the event, a ToolStripMenuItem control.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    Private Sub MenuOptionsAudioAlerts_Click(sender As Object, e As EventArgs) _
        Handles MenuOptionsAudioAlerts.Click
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
            Me.MenuOptionsSpeechRecognitionEnabled.Checked =
                My.Settings.SystemSpeechRecognitionThreshold < 1
            Me.MenuOptionsSpeechRecognitionEnabled.Enabled = False
            CancelSpeechRecognition()
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="CheckBox.CheckedChanged"/> event
    '''  for the <see cref="MenuOptionsAutoLogin"/> checkbox.
    '''  Updates the application's AutoLogin setting based on the checkbox state.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="CheckBox"/> control.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuOptionsAutoLogin_CheckedChanged(sender As Object, e As EventArgs) _
        Handles MenuOptionsAutoLogin.CheckedChanged

        My.Settings.AutoLogin = Me.MenuOptionsAutoLogin.Checked
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event
    '''  for the <see cref="MenuOptionsColorPicker"/> menu item.
    '''  Opens the <see cref="OptionsColorPickerDialog"/> for color selection.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="ToolStripMenuItem"/> control.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuOptionsColorPicker_Click(sender As Object, e As EventArgs) _
        Handles MenuOptionsColorPicker.Click

        Using o As New OptionsColorPickerDialog()
            o.ShowDialog(owner:=Me)
        End Using
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event
    '''  for the <see cref="MenuOptionsConfigureTiTR_Click"/> menu item.
    '''  Opens the <see cref="OptionsConfigureTiTR"/> for configuration of
    '''  Time in Tight Range (TiTR).
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="ToolStripMenuItem"/> control.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuOptionsConfigureTiTR_Click(sender As Object, e As EventArgs) _
        Handles MenuOptionsConfigureTiTR.Click

        Dim result As DialogResult = OptionsConfigureTiTR.ShowDialog(owner:=Me)
        If result = DialogResult.OK Then
            Me.MenuOptionsConfigureTiTR.Text =
                $"Configure TiTR ({OptionsConfigureTiTR.GetTiTrMsg()})..."
            Me.TiTRMgsLabel2.Text = OptionsConfigureTiTR.GetTiTrMsg()

            ' Update the TiTR compliance values based on the user's configuration.
            Me.UpdateTimeInRange()
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event
    '''  for the <see cref="MenuOptionsEditPumpSettings"/> menu item.
    '''  Loads and deserializes the current user's pump settings from JSON.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="ToolStripMenuItem"/> control.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuOptionsEditPumpSettings_Click(sender As Object, e As EventArgs) _
        Handles MenuOptionsEditPumpSettings.Click

        SetUpCareLinkUser(forceUI:=True)
        Dim json As String = File.ReadAllText(path:=GetUserSettingsPath())
        CurrentUser = JsonSerializer.Deserialize(Of CurrentUserRecord)(json, options:=s_jsonSerializerOptions)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event for
    '''  the <see cref="MenuOptionsFilterRawJSONData"/> menu item.
    '''  Toggles the filtering of raw JSON data in all <see cref="DataGridView"/> Controls
    '''  by hiding columns as defined by their respective helpers.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="ToolStripMenuItem"/> control.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuOptionsFilterRawJSONData_Click(sender As Object, e As EventArgs) _
        Handles MenuOptionsFilterRawJSONData.Click

        s_filterJsonData = Me.MenuOptionsFilterRawJSONData.Checked
        HideDataGridViewColumnsByName(Of ActiveInsulin)(dgv:=Me.DgvActiveInsulin)
        HideDataGridViewColumnsByName(Of AutoBasalDelivery)(dgv:=Me.DgvAutoBasalDelivery)
        HideDataGridViewColumnsByName(Of AutoModeStatus)(dgv:=Me.DgvAutoModeStatus)
        HideDataGridViewColumnsByName(Of BannerState)(dgv:=Me.DgvPumpBannerState)
        HideDataGridViewColumnsByName(Of Basal)(dgv:=Me.DgvBasal)
        HideDataGridViewColumnsByName(Of BasalPerHour)(dgv:=Me.DgvBasalPerHour)
        HideDataGridViewColumnsByName(Of Calibration)(dgv:=Me.DgvCalibration)
        HideDataGridViewColumnsByName(Of CareLinkUserDataRecord)(dgv:=Me.DgvCareLinkUsers)
        HideDataGridViewColumnsByName(Of CurrentUserRecord)(dgv:=Me.DgvCurrentUser)
        HideDataGridViewColumnsByName(Of Insulin)(dgv:=Me.DgvInsulin)
        HideDataGridViewColumnsByName(Of LastAlarm)(dgv:=Me.DgvLastAlarm)
        HideDataGridViewColumnsByName(Of SG)(dgv:=Me.DgvLastSensorGlucose)
        HideDataGridViewColumnsByName(Of Limit)(dgv:=Me.DgvLimits)
        HideDataGridViewColumnsByName(Of LowGlucoseSuspended)(dgv:=Me.DgvLowGlucoseSuspended)
        HideDataGridViewColumnsByName(Of Meal)(dgv:=Me.DgvMeal)
        HideDataGridViewColumnsByName(Of SG)(dgv:=Me.DgvSensorBgReadings)
        HideDataGridViewColumnsByName(Of SG)(dgv:=Me.DgvSGs)
        HideDataGridViewColumnsByName(Of TherapyAlgorithmState)(
            dgv:=Me.DgvTherapyAlgorithmState)
        HideDataGridViewColumnsByName(Of TimeChange)(dgv:=Me.DgvTimeChange)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event for
    '''  the <see cref="MenuOptionsShowChartLegends"/> menu item.
    '''  Toggles the visibility of chart legends for all main charts and updates
    '''  the application settings.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="ToolStripMenuItem"/> control.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuOptionsShowChartLegends_Click(sender As Object, e As EventArgs) _
        Handles MenuOptionsShowChartLegends.Click

        Dim showLegend As Boolean = Me.MenuOptionsShowChartLegends.Checked
        _activeInsulinChartLegend.Enabled = showLegend
        _summaryChartLegend.Enabled = showLegend
        _treatmentMarkersChartLegend.Enabled = showLegend
        My.Settings.SystemShowLegends = showLegend
        My.Settings.Save()
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event
    '''  for the <see cref="MenuOptionsSpeechHelpShown"/> menu item.
    '''  Updates the <see cref="My.Settings.SystemSpeechHelpShown"/> setting
    '''  based on the menu item's checked state.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="ToolStripMenuItem"/> control.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuOptionsSpeechHelpShown_Click(sender As Object, e As EventArgs) _
        Handles MenuOptionsSpeechHelpShown.Click

        My.Settings.SystemSpeechHelpShown = Me.MenuOptionsSpeechHelpShown.Checked
        My.Settings.Save()
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event for
    '''  the <see cref="MenuOptionsSpeechRecognition80"/> menu item.
    '''  Sets the speech recognition confidence threshold to 0.8 and
    '''  updates the UI and speech recognition state.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="ToolStripMenuItem"/> control.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuOptionsSpeechRecognition80_Click(sender As Object, e As EventArgs) _
        Handles MenuOptionsSpeechRecognition80.Click,
                MenuOptionsSpeechRecognition85.Click,
                MenuOptionsSpeechRecognition90.Click,
                MenuOptionsSpeechRecognition95.Click,
                MenuOptionsSpeechRecognitionDisabled.Click

        Dim menuItem As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Select Case menuItem.Name
            Case "MenuOptionsSpeechRecognition80"
                My.Settings.SystemSpeechRecognitionThreshold = 0.8
            Case "MenuOptionsSpeechRecognition85"
                My.Settings.SystemSpeechRecognitionThreshold = 0.85
            Case "MenuOptionsSpeechRecognition90"
                My.Settings.SystemSpeechRecognitionThreshold = 0.9
            Case "MenuOptionsSpeechRecognition95"
                My.Settings.SystemSpeechRecognitionThreshold = 0.95
            Case "MenuOptionsSpeechRecognitionDisabled"
                My.Settings.SystemSpeechRecognitionThreshold = 1.0
        End Select

        My.Settings.Save()
        Me.SetSpeechRecognitionConfidenceThreshold()
        If Me.MenuOptionsSpeechRecognitionEnabled.Checked Then
            InitializeSpeechRecognition()
        Else
            CancelSpeechRecognition()
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event for
    '''  the <see cref="MenuOptionsUseLocalTimeZone"/> menu item.
    '''  Sets the application's time zone to local or server based
    '''  on the menu item's checked state, and updates the
    '''  corresponding setting if changed.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="ToolStripMenuItem"/> control.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuOptionsUseLocalTimeZone_Click(sender As Object, e As EventArgs) _
        Handles MenuOptionsUseLocalTimeZone.Click

        ' Toggle the UseLocalTimeZone setting and update the PumpTimeZoneInfo accordingly.
        Dim saveRequired As Boolean =
            Me.MenuOptionsUseLocalTimeZone.Checked <> My.Settings.UseLocalTimeZone
        If Me.MenuOptionsUseLocalTimeZone.Checked Then
            PumpTimeZoneInfo = TimeZoneInfo.Local
            My.Settings.UseLocalTimeZone = True
        Else
            Const key As String = NameOf(ServerDataEnum.clientTimeZoneName)
            Dim timeZoneName As String = RecentData(key)
            PumpTimeZoneInfo = CalculateTimeZone(timeZoneName)
            My.Settings.UseLocalTimeZone = False
        End If
        If saveRequired Then My.Settings.Save()
    End Sub

    ''' <summary>
    '''  Sets the checked state of speech recognition confidence threshold
    '''  menu items based on the current value in
    '''  <see cref="My.Settings.SystemSpeechRecognitionThreshold"/>.
    '''  Ensures only the selected threshold is checked.
    ''' </summary>
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
        Me.MenuOptionsSpeechRecognitionEnabled.Checked =
            Me.MenuOptionsSpeechRecognitionDisabled.Checked = False
    End Sub

#End Region ' Menus Options

#Region "View Menu Events"

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event for
    '''  the <see cref="MenuShowMiniDisplay"/> menu item.
    '''  Hides the main form and displays the mini SmartGuard display window.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="ToolStripMenuItem"/> control.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuShowMiniDisplay_Click(sender As Object, e As EventArgs) _
        Handles MenuShowMiniDisplay.Click

        Me.Hide()
        _sgMiniDisplay.Show()
    End Sub

#End Region ' View Menu Events

#Region "Help Menu Events"

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event for
    '''  the <see cref="MenuHelpAbout"/> menu item.
    '''  Displays the About dialog box for the application.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="ToolStripMenuItem"/> control.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuHelpAbout_Click(sender As Object, e As EventArgs) _
        Handles MenuHelpAbout.Click

        AboutBox1.ShowDialog(owner:=Me)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event
    '''  for the <see cref="MenuHelpCheckForUpdates"/> menu item.
    '''  Initiates an asynchronous check for application updates and reports the result.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="ToolStripMenuItem"/> control.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuHelpCheckForUpdates_Click(sender As Object, e As EventArgs) _
        Handles MenuHelpCheckForUpdates.Click

        CheckForUpdatesAsync(reportSuccessfulResult:=True)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event for the
    '''  <see cref="MenuHelpReportAnIssue"/> menu item.
    '''  Opens the GitHub issues page for the CareLink™ project in the default web browser.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="ToolStripMenuItem"/> control.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuHelpReportAnIssue_Click(sender As Object, e As EventArgs) _
        Handles MenuHelpReportAnIssue.Click

        OpenUrlInBrowser(url:=$"{GitHubCareLinkUrl}issues")
    End Sub

#End Region ' Help Menu Events

#End Region 'Form1 Menu Events

#Region "Form Misc Events"

    ''' <summary>
    '''  Handles the <see cref="Label.Paint"/> event
    '''  for the <see cref="ActiveInsulinValue"/> control.
    '''  Draws a solid lime green border around the ActiveInsulinValue label.
    ''' </summary>
    ''' <param name="sender">The source of the event, the ActiveInsulinValue label.</param>
    ''' <param name="e">A <see cref="PaintEventArgs"/> that contains the event data.</param>
    Private Sub ActiveInsulinValue_Paint(sender As Object, e As PaintEventArgs) _
        Handles ActiveInsulinValue.Paint

        ControlPaint.DrawBorder(
            e.Graphics,
            bounds:=e.ClipRectangle,
            leftColor:=Color.LimeGreen, leftWidth:=3, leftStyle:=ButtonBorderStyle.Solid,
            topColor:=Color.LimeGreen, topWidth:=3, topStyle:=ButtonBorderStyle.Solid,
            rightColor:=Color.LimeGreen, rightWidth:=3, rightStyle:=ButtonBorderStyle.Solid,
            bottomColor:=Color.LimeGreen, bottomWidth:=3, bottomStyle:=ButtonBorderStyle.Solid)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="CheckBox.CheckedChanged"/> event for
    '''  the <see cref="TemporaryUseAdvanceAITDecayCheckBox"/>.
    '''  Updates the checkbox text and the
    '''  <see cref="CurrentUser.UseAdvancedAitDecay"/> property, then updates
    '''  the active insulin chart.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="CheckBox"/> control.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub TemporaryUseAdvanceAITDecayCheckBox_CheckedChanged(sender As Object, e As EventArgs) _
        Handles TemporaryUseAdvanceAITDecayCheckBox.CheckedChanged

        ' Update the checkbox text based on the current state and insulin type.
        With CurrentUser
            Dim checkState As CheckState = Me.TemporaryUseAdvanceAITDecayCheckBox.CheckState
            Dim whileUsing As String = $" hours, while using { .InsulinTypeName}"
            Me.TemporaryUseAdvanceAITDecayCheckBox.Text =
                If(checkState = CheckState.Checked,
                   $"Advanced Decay, AIT will decay over { .InsulinRealAit}{ whileUsing}",
                   $"AIT will decay over { .PumpAit.ToHoursMinutes}{ whileUsing}")

            CurrentUser.UseAdvancedAitDecay = checkState
        End With
        If _remainingInsulinList.Count = 0 Then Exit Sub
        Me.UpdateActiveInsulinChart()
    End Sub

#End Region ' Form Misc Events

#Region "NotifyIcon Events"

    ''' <summary>
    '''  Handles the <see cref="NotifyIcon.DoubleClick"/> event
    '''  for the application's notification icon. Restores the main window
    '''  to the taskbar and sets its state to normal when the icon is double-clicked.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="NotifyIcon"/> control.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub NotifyIcon1_DoubleClick(sender As Object, e As EventArgs) _
        Handles NotifyIcon1.DoubleClick

        Me.ShowInTaskbar = True
        Me.WindowState = FormWindowState.Normal
    End Sub

#End Region ' NotifyIcon Events

#Region "Settings Events"

    ''' <summary>
    '''  Handles the <see cref="ApplicationSettingsBase.SettingChanging"/> event
    '''  for application settings.
    '''  This method is called whenever a setting is about to change,
    '''  except for settings whose names start with "System".
    '''  It checks if the new value is different from the current value (case-insensitive).
    '''  If the setting being changed is "CareLinkUserName", it updates the logged-on user
    '''  in <see cref="LoginHelpers.LoginDialog"/>.
    '''  If the user does not exist in <see cref="s_allUserSettingsData"/>,
    '''  a new <see cref="CareLinkUserDataRecord"/> is created and added.
    '''  Finally, all user records are saved with the updated value.
    ''' </summary>
    ''' <param name="sender">The source of the event, typically the settings object.</param>
    ''' <param name="e">
    '''  A <see cref="SettingChangingEventArgs"/> containing the event data,
    '''  including the setting name and new value.
    ''' </param>
    Private Sub MySettings_SettingChanging(sender As Object, e As SettingChangingEventArgs)
        If e.SettingName.StartsWith(value:="System") Then Exit Sub

        Dim value As String = Convert.ToString(value:=e.NewValue)
        If EqualsNoCase(My.Settings(propertyName:=e.SettingName), value) Then Exit Sub

        If e.SettingName = "CareLinkUserName" Then
            If s_allUserSettingsData?.ContainsKey(key:=value) Then
                LoginHelpers.LoginDialog.LoggedOnUser = s_allUserSettingsData(itemName:=value)
                Exit Sub
            End If
            Dim userSettings As New CareLinkUserDataRecord(parent:=s_allUserSettingsData)
            userSettings.UpdateValue(key:=e.SettingName, value)
            s_allUserSettingsData.Add(value:=userSettings)
        End If
        s_allUserSettingsData.SaveAllUserRecords(LoginHelpers.LoginDialog.LoggedOnUser, key:=e.SettingName, value)
    End Sub

#End Region ' Settings Events

#Region "Summary Events"

    ''' <summary>
    '''  Handles the <see cref="MouseHover"/> event for the CalibrationDueImage control.
    '''  Displays a tooltip with the time of the next calibration if it is
    '''  due within the next 24 hours.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, typically the CalibrationDueImage control.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub CalibrationDueImage_MouseHover(sender As Object, e As EventArgs) _
        Handles CalibrationDueImage.MouseHover

        If s_timeToNextCalibrationMinutes > 0 AndAlso s_timeToNextCalibrationMinutes < 1440 Then
            Dim due As String = PumpNow.AddMinutes(value:=s_timeToNextCalibrationMinutes).ToShortTimeString()
            Dim caption As String = $"Calibration Due {due}"
            _calibrationToolTip.SetToolTip(control:=Me.CalibrationDueImage, caption)
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="MouseHover"/> event for the SensorDaysLeftLabel control.
    '''  Displays a tooltip with the remaining sensor duration in hours
    '''  if it is less than 24 hours.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, typically the SensorDaysLeftLabel control.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub Last24HrCarbLabel_MouseHover(sender As Object, e As EventArgs) _
        Handles Last24HrCarbsLabel.MouseHover, Last24HrCarbsValueLabel.MouseHover

        _carbRatio.SetToolTip(
            control:=DirectCast(sender, Label),
            caption:=$"Carb Ratio {CDbl(s_totalCarbs / s_totalManualBolus):N1}")
    End Sub

    ''' <summary>
    '''  Handles the <see cref="MouseHover"/> event for the SensorDaysLeftLabel control.
    '''  Displays a tooltip with the remaining sensor duration in hours
    '''  if it is less than 24 hours.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, <see cref="SensorDaysLeftLabel"/> control.
    ''' </param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub SensorDaysLeftLabel_MouseHover(sender As Object, e As EventArgs) _
        Handles SensorDaysLeftLabel.MouseHover

        If PatientData.SensorDurationHours < 24 Then
            _sensorLifeToolTip.SetToolTip(
                control:=Me.SensorDaysLeftLabel,
                caption:=$"Sensor will expire In {PatientData.SensorDurationHours} hours")
        End If
    End Sub

#End Region ' Summary Events

#Region "Tab Events"

#If NET9_0 Then

    ''' <summary>
    '''  Handles the <see cref="TabControl.DrawItem"/> event
    '''  for the main and secondary tab controls.
    '''  Customizes the drawing of the tab pages to highlight the selected tab with a
    '''  different background and foreground color.
    ''' </summary>
    ''' <param name="sender">The source of the event, typically a TabControl control.</param>
    ''' <param name="e">
    '''  A <see cref="DrawItemEventArgs"/> that contains the event data,
    '''  including the index of the tab page being drawn.
    ''' </param>
    ''' <remarks>
    '''  This method is used to provide a custom appearance for the tab
    '''  pages in the application. It fills the background with a solid color
    '''  and draws the tab text centered within the tab rectangle.
    ''' </remarks>
    Private Sub TabControl_DrawItem(sender As Object, e As DrawItemEventArgs) Handles _
        TabControlPage1.DrawItem,
        TabControlPage2.DrawItem

        Dim tabControl As TabControl = CType(sender, TabControl)
        Dim rect As Rectangle = tabControl.GetTabRect(e.Index)
        Dim format As New StringFormat With {
            .Alignment = StringAlignment.Center,
            .LineAlignment = StringAlignment.Center}

        ' Highlight selected tab
        Dim color As Color = If(tabControl.SelectedIndex = e.Index,
                                Color.Black,
                                SystemColors.ControlDark)

        Using brush As New SolidBrush(color), textBrush As New SolidBrush(color:=Color.White)
            Using g As Graphics = e.Graphics
                g.FillRectangle(brush, rect)
                Dim s As String = tabControl.TabPages(e.Index).Text
                g.DrawString(s, tabControl.Font, brush:=textBrush, layoutRectangle:=rect, format)
            End Using
        End Using
    End Sub

#End If ' NET9_0

    ''' <summary>
    '''  Handles the <see cref="TabControl.Selecting"/> event for the main tab control.
    '''  Updates the cursor and last selected tab index based on the selected tab page.
    ''' </summary>
    ''' <param name="sender">The source of the event, typically a TabControl control.</param>
    ''' <param name="e">
    '''  A <see cref="TabControlCancelEventArgs"/> that contains the event data.
    ''' </param>
    ''' <remarks>
    '''  This method is used to manage cursor visibility and
    '''  last selected tab index for navigation.
    ''' </remarks>
    Private Sub TabControlPage1_Selecting(sender As Object, e As TabControlCancelEventArgs) _
        Handles TabControlPage1.Selecting

        Select Case e.TabPage.Name
            Case NameOf(TabPage15More)
                Me.DgvCareLinkUsers.InitializeDgv

                For Each c As DataGridViewColumn In Me.DgvCareLinkUsers.Columns
                    c.Visible =
                        Not HideColumn(Of CareLinkUserDataRecord)(c.DataPropertyName)
                Next
                Me.TabControlPage2.SelectedIndex =
                    If(_lastMarkerTabLocation.Page = 0,
                       0,
                       _lastMarkerTabLocation.Tab)
                Me.TabControlPage1.Visible = False
                Exit Sub
        End Select
        _lastSummaryTabIndex = e.TabPageIndex
    End Sub

    ''' <summary>
    '''  Handles the <see cref="TabControlPage2.Selecting"/> event for the
    '''  secondary tab control. Updates the selected index and
    '''  visibility of the main tab control based on the selected tab page.
    ''' </summary>
    ''' <param name="sender">The source of the event, typically a TabControl control.</param>
    ''' <param name="e">
    '''  A <see cref="TabControlCancelEventArgs"/> that contains the event data.
    ''' </param>
    ''' <remarks>
    '''  This method is used to manage navigation between different summary tabs
    '''  and user settings.
    ''' </remarks>
    Private Sub TabControlPage2_Selecting(sender As Object, e As TabControlCancelEventArgs) _
        Handles TabControlPage2.Selecting

        Select Case e.TabPage.Name
            Case NameOf(TabPage12BackToHomePage)
                Me.TabControlPage1.SelectedIndex = _lastSummaryTabIndex
                Me.TabControlPage1.Visible = True
                Exit Sub
            Case NameOf(TabPage11AllUsers)
                Me.DgvCareLinkUsers.DataSource = s_allUserSettingsData
                For Each c As DataGridViewColumn In Me.DgvCareLinkUsers.Columns
                    c.Visible = Not HideColumn(Of CareLinkUserDataRecord)(item:=c.DataPropertyName)
                Next
            Case Else
                Const tabPageName As String = NameOf(TabPage09BasalPerHour)
                If e.TabPageIndex <= GetTabIndexFromName(tabPageName) Then
                    _lastMarkerTabLocation = (Page:=1, Tab:=e.TabPageIndex)
                End If
        End Select
    End Sub

#End Region ' Tab Events

#Region "TableLayoutPanelTop Button Events"

    ''' <summary>
    '''  Handles the <see cref="Button.Click"/> event for buttons
    '''  in the top TableLayoutPanel controls.
    '''  This method is used to navigate to the corresponding
    '''  summary tab based on the button clicked.
    ''' </summary>
    ''' <param name="sender">The source of the event, typically a Button control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    ''' <remarks>
    '''  The method identifies the tab name from the button's parent TableLayoutPanel and
    '''  selects the appropriate tab in the main tab control.
    ''' </remarks>
    Private Sub TableLayoutPanelTopButton_Click(sender As Object, e As EventArgs) Handles _
        TlpActiveInsulinTop.ButtonClick,
        TlpAutoBasalDeliveryTop.ButtonClick,
        TlpAutoModeStatusTop.ButtonClick,
        TlpPumpBannerStateTop.ButtonClick,
        TlpBasalTop.ButtonClick,
        TlpBgReadingsTop.ButtonClick,
        TlpCalibrationTop.ButtonClick,
        TlpInsulinTop.ButtonClick,
        TlpLastAlarmTop.ButtonClick,
        TlpLastSgTop.ButtonClick,
        TlpLimitsTop.ButtonClick,
        TlpLowGlucoseSuspendedTop.ButtonClick,
        TlpMealTop.ButtonClick,
        TlpNotificationActiveTop.ButtonClick,
        TlpNotificationsClearedTop.ButtonClick,
        TlpSgsTop.ButtonClick,
        TlpTherapyAlgorithmStateTop.ButtonClick,
        TlpTimeChangeTop.ButtonClick

        Me.TabControlPage1.Visible = True
        Dim button As Button = CType(sender, Button)
        Dim panelTop As TableLayoutPanelTopEx = CType(button.Parent, TableLayoutPanelTopEx)
        Dim tabName As String = panelTop.LabelText.Split(separator:=":")(0).Remove(s:=" ")
        If tabName.Contains(value:="Marker") Then
            tabName = "Markers"
        ElseIf tabName = "NotificationHistory" Then
            tabName = If(panelTop.Name.Contains(value:="Active"),
                         NameOf(ActiveNotification),
                         NameOf(ClearedNotifications))

        ElseIf tabName = "LastSensorGlucose" Then
            tabName = "LastSG"
        ElseIf tabName = "SensorGlucoseValues" Then
            tabName = "Sgs"
        End If
        Dim dgv As DataGridView =
            CType(Me.TabControlPage1.TabPages(index:=3).Controls(index:=0), DataGridView)
        For index As Integer = 0 To dgv.RowCount - 1
            Dim row As DataGridViewRow = dgv.Rows(index)
            Dim message As String = row.Cells(index:=1).FormattedValue.ToString
            Debug.WriteLine(message)
            If message.EqualsNoCase(tabName) Then
                Me.TabControlPage1.SelectedIndex = 3
                dgv.CurrentCell = dgv.Rows(index).Cells(index:=2)
                s_currentSummaryRow = index
                Exit Sub
            End If
        Next
        Stop
    End Sub

#End Region ' TableLayoutPanelTop Button Events

#Region "Timer Events"

    ''' <summary>
    '''  Handles the <see cref="Timer.Tick"/> event for the cursor timer.
    '''  This method is called periodically to reset the cursor position
    '''  if the chart is not zoomed.
    ''' </summary>
    ''' <param name="sender">The source of the event, typically a Timer control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    ''' <remarks>
    '''  The timer is stopped if the chart is not zoomed,
    '''  and the cursor position is set to NaN.
    ''' </remarks>
    Private Sub CursorTimer_Tick(sender As Object, e As EventArgs) Handles CursorTimer.Tick
        If Not Me.SummaryChart.ChartAreas(name:=NameOf(ChartArea)).AxisX.ScaleView.IsZoomed Then
            Me.CursorTimer.Enabled = False
            Me.SummaryChart.ChartAreas(name:=NameOf(ChartArea)).CursorX.Position = Double.NaN
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="PowerModeChanged"/> event for system power mode changes.
    '''  This method is called when the system enters or resumes from a sleep state.
    '''  It manages the server update timer and updates the last update time accordingly.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, typically the application or system.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="PowerModeChangedEventArgs"/> that contains the event data.
    ''' </param>
    ''' <remarks>
    '''  The method stops the server update timer on suspend and restarts it on resume.
    ''' </remarks>
    Private Sub PowerModeChanged(sender As Object, e As PowerModeChangedEventArgs)
        Debug.WriteLine(message:=$"PowerModeChange {e.Mode}")
        Select Case e.Mode
            Case PowerModes.Suspend
                SetServerUpdateTimer(Start:=False)
                s_shuttingDown = True
                Me.SetLastUpdateTime(
                    msg:="System Sleeping",
                    suffixMessage:="",
                    highLight:=True,
                    isDaylightSavingTime:=Nothing)
            Case PowerModes.Resume
                Me.SetLastUpdateTime(
                    msg:="System Awake",
                    suffixMessage:="",
                    highLight:=True,
                    isDaylightSavingTime:=Nothing)
                s_shuttingDown = False
                SetServerUpdateTimer(Start:=True, interval:=TwentySecondsInMilliseconds)
                Dim message As String = $"restarted after wake. {NameOf(ServerUpdateTimer)} started at {Now:T}"
                DebugPrint(message)
        End Select

    End Sub

    ''' <summary>
    '''  Handles the <see cref="Timer.Tick"/> event for the server update timer.
    '''  This method is called periodically to update the server data and refresh the UI.
    ''' </summary>
    ''' <param name="sender">The source of the event, typically a Timer control.</param>
    ''' <param name="e">
    '''  An <see cref="EventArgs"/> that contains the event data.
    ''' </param>
    ''' <remarks>
    '''  The method checks if updates are in progress, retrieves recent data,
    '''  and updates the UI accordingly.
    ''' </remarks>
    Private Sub ServerUpdateTimer_Tick(sender As Object, e As EventArgs) _
        Handles ServerUpdateTimer.Tick

        SetServerUpdateTimer(Start:=False)
        Dim lastErrorMessage As String = String.Empty
        SyncLock _updatingLock
            If _updating Then
                Stop
            Else
                _updating = True
                lastErrorMessage = Client?.GetRecentData()
                If RecentDataEmpty() Then
                    If Client Is Nothing Then
                        Do While True
                            LoginDialog.LoginSourceAutomatic = FileToLoadOptions.Login
                            Dim result As DialogResult = LoginDialog.ShowDialog(owner:=Me)
                            Select Case result
                                Case DialogResult.OK
                                    Exit Do
                                Case DialogResult.Cancel
                                    SetServerUpdateTimer(Start:=False)
                                    Return
                                Case DialogResult.Retry
                            End Select
                        Loop

                        Client = LoginDialog.Client
                    End If
                    lastErrorMessage = Client.GetRecentData()
                End If
                ReportLoginStatus(
                    Me.LoginStatus,
                    hasErrors:=RecentDataEmpty,
                    lastErrorMessage)

                Me.Cursor = Cursors.Default
                Application.DoEvents()
            End If
            _updating = False
        End SyncLock

        Dim lastMedicalDeviceDataUpdateServerEpochString As String = ""
        If Not RecentDataEmpty() Then
            If RecentData.TryGetValue(
                    key:=NameOf(ServerDataEnum.lastMedicalDeviceDataUpdateServerTime),
                    value:=lastMedicalDeviceDataUpdateServerEpochString) Then
                If CLng(lastMedicalDeviceDataUpdateServerEpochString) =
                    s_lastMedicalDeviceDataUpdateServerEpoch Then
                    Dim epochAsLocalDate As Date =
                        lastMedicalDeviceDataUpdateServerEpochString.FromUnixTime.ToLocalTime
                    If epochAsLocalDate + FiveMinuteSpan < Now() Then
                        Me.SetLastUpdateTime(
                            msg:=Nothing,
                            suffixMessage:="",
                            highLight:=True,
                            isDaylightSavingTime:=epochAsLocalDate.IsDaylightSavingTime)
                        _sgMiniDisplay.SetCurrentSgString(
                            sgString:="---",
                            f:=Single.NaN)
                    Else
                        Me.SetLastUpdateTime(
                            msg:=Nothing,
                            suffixMessage:="",
                            highLight:=False,
                            isDaylightSavingTime:=epochAsLocalDate.IsDaylightSavingTime)
                        _sgMiniDisplay.SetCurrentSgString(
                            sgString:=s_lastSg?.ToString,
                            f:=s_lastSg.sg)
                    End If
                Else
                    Me.UpdateAllTabPages(fromFile:=False)
                End If
            Else
                Stop
            End If
        Else
            ReportLoginStatus(Me.LoginStatus, hasErrors:=True, lastErrorMessage)
            _sgMiniDisplay.SetCurrentSgString(sgString:="---", f:=0)
        End If
        SetServerUpdateTimer(Start:=True, interval:=OneMinuteInMilliseconds)
    End Sub

#End Region ' Timer Events

#End Region ' Events

#Region "Initialize Charts"

#Region "Initialize Summary Charts"

    ''' <summary>
    '''  Initializes the summary tab charts, including setting up
    '''  chart areas, series, and legends.
    '''  This method is called to prepare the summary chart for displaying
    '''  data related to insulin therapy.
    ''' </summary>
    Friend Sub InitializeSummaryTabCharts()
        Me.SplitContainer3.Panel1.Controls.Clear()
        Me.SummaryChart = CreateChart(NameOf(SummaryChart))
        Dim summaryTitle As Title = CreateTitle(
            chartTitle:="Summary",
            name:=NameOf(summaryTitle),
            foreColor:=Me.SummaryChart.BackColor.ContrastingColor())

        Dim summaryChartArea As ChartArea = CreateChartArea(containingChart:=Me.SummaryChart)
        Me.SummaryChart.ChartAreas.Add(item:=summaryChartArea)
        _summaryChartLegend = CreateChartLegend(legendName:=NameOf(_summaryChartLegend))

        Me.SummaryAutoCorrectionSeries = CreateSeriesBasal(
            name:=AutoCorrectionSeriesName,
            basalLegend:=_summaryChartLegend,
            legendText:="Auto Correction",
            yAxisType:=AxisType.Secondary)
        Me.SummaryBasalSeries = CreateSeriesBasal(
            name:=BasalSeriesName,
            basalLegend:=_summaryChartLegend,
            legendText:="Basal Series",
            yAxisType:=AxisType.Secondary)
        Me.SummaryMinBasalSeries = CreateSeriesBasal(
            name:=MinBasalSeriesName,
            basalLegend:=_summaryChartLegend,
            legendText:="Min Basal",
            yAxisType:=AxisType.Secondary)
        Me.SummaryHighLimitSeries = CreateSeriesLimitsAndTarget(
            limitsLegend:=_summaryChartLegend,
            seriesName:=HighLimitSeriesName)
        Me.SummarySuspendSeries = CreateSeriesSuspend(basalLegend:=_summaryChartLegend)
        Me.SummaryTargetSgSeries = CreateSeriesLimitsAndTarget(
            limitsLegend:=_summaryChartLegend,
            seriesName:=TargetSgSeriesName)
        Me.SummarySgSeries = CreateSeriesSg(sgLegend:=_summaryChartLegend)
        Me.SummaryLowLimitSeries = CreateSeriesLimitsAndTarget(
            limitsLegend:=_summaryChartLegend,
            seriesName:=LowLimitSeriesName)
        Me.SummaryMarkerSeries =
            CreateSeriesWithoutVisibleLegend(YAxisType:=AxisType.Secondary)
        Me.SummaryTimeChangeSeries = CreateSeriesTimeChange(basalLegend:=_summaryChartLegend)

        Me.SplitContainer3.Panel1.Controls.Add(Me.SummaryChart)
        Application.DoEvents()

        With Me.SummaryChart
            With .Series
                .Add(item:=Me.SummarySuspendSeries)

                .Add(item:=Me.SummaryHighLimitSeries)
                .Add(item:=Me.SummaryTargetSgSeries)
                .Add(item:=Me.SummaryLowLimitSeries)
                .Add(item:=Me.SummaryTimeChangeSeries)

                .Add(item:=Me.SummaryAutoCorrectionSeries)
                .Add(item:=Me.SummaryBasalSeries)
                .Add(item:=Me.SummaryMinBasalSeries)

                .Add(item:=Me.SummarySgSeries)
                .Add(item:=Me.SummaryMarkerSeries)

            End With
            With .Series(name:=SgSeriesName).EmptyPointStyle
                .BorderWidth = 4
                .Color = Color.Transparent
            End With
            .Legends.Add(item:=_summaryChartLegend)
            .Titles.Add(item:=summaryTitle)
        End With
        Application.DoEvents()
    End Sub

    ''' <summary>
    '''  Initializes the Time in Range area of the home tab,
    '''  setting up labels and a chart for displaying time in range data.
    '''  This method is called to prepare the Time in Range chart and
    '''  labels for displaying compliance information.
    ''' </summary>
    Friend Sub InitializeTimeInRangeArea()
        Dim c As Control = Me.SplitContainer3.Panel2.Controls.FindControlByName(NameOf(Me.TimeInRangeChart))
        If c Is Nothing Then
            Dim size As Integer = Me.SplitContainer3.Panel2.Width - 94
            Me.TimeInRangeChart = New Chart With {
                .Anchor = AnchorStyles.Top,
                .BackColor = Color.Transparent,
                .BackGradientStyle = GradientStyle.None,
                .BackSecondaryColor = Color.Transparent,
                .BorderlineColor = Color.Transparent,
                .BorderlineWidth = 0,
                .Size = New Size(width:=size, height:=size)}

            With Me.TimeInRangeChart
                .BorderSkin.BackSecondaryColor = Color.Transparent
                .BorderSkin.SkinStyle = BorderSkinStyle.None
                Dim timeInRangeChartArea As New ChartArea With {
                    .Name = NameOf(timeInRangeChartArea),
                    .BackColor = Color.Black}
                .ChartAreas.Add(timeInRangeChartArea)
                Dim chartLabel As Label = Me.TimeInRangeChartLabel
                Dim x As Integer = chartLabel.FindHorizontalMidpoint - (.Width \ 2)
                Dim y As Integer = CInt(chartLabel.FindVerticalMidpoint() - Math.Round(.Height / 2.5))
                .Location = New Point(x, y)
                .Name = NameOf(TimeInRangeChart)
                Me.TimeInRangeSeries = New Series(NameOf(TimeInRangeSeries)) With {
                    .ChartArea = NameOf(timeInRangeChartArea),
                    .ChartType = SeriesChartType.Doughnut}
                .Series.Add(item:=Me.TimeInRangeSeries)
                .Series(name:=NameOf(TimeInRangeSeries))(name:="DoughnutRadius") = "17"
            End With
            Me.SplitContainer3.Panel2.Controls.Add(Me.TimeInRangeChart)
        End If
        Me.PositionControlsInPanel()
    End Sub

#End Region ' Initialize Home Tab Charts

#Region "Initialize Chart Tabs"

#Region "Initialize Active Insulin Chart"

    ''' <summary>
    '''  Initializes the Active Insulin Tab chart, including chart area, axes, series,
    '''  and legend. This method sets up the chart for displaying active
    '''  insulin on board (IOB) and related data.
    ''' </summary>
    ''' <remarks>
    '''  - Clears any existing controls from the panel.
    '''  - Creates and configures the chart and its area, including axis labels,
    '''    intervals, and colors.
    ''' - Adds all required series for displaying active insulin, basal,
    '''   auto correction, min basal, suspend, sensor glucose, markers, and time change.
    ''' - Adds the chart legend and title.
    ''' - Adds the chart to the panel and processes UI events.
    ''' </remarks>
    Friend Sub InitializeActiveInsulinTabChart()
        Me.SplitContainer1.Panel2.Controls.Clear()
        Me.ActiveInsulinChart = CreateChart(NameOf(ActiveInsulinChart))
        Dim activeInsulinChartArea As ChartArea =
            CreateChartArea(containingChart:=Me.ActiveInsulinChart)
        Dim labelColor As Color = Me.ActiveInsulinChart.BackColor.ContrastingColor()
        Dim labelFont As New Font(FamilyName, emSize:=12.0F, style:=FontStyle.Bold)

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
            .TitleFont = New Font(family:=labelFont.FontFamily, emSize:=14)
            .TitleForeColor = labelColor
        End With
        Me.ActiveInsulinChart.ChartAreas.Add(item:=activeInsulinChartArea)
        _activeInsulinChartLegend =
            CreateChartLegend(legendName:=NameOf(_activeInsulinChartLegend))
        Me.ActiveInsulinChartTitle = CreateTitle(
            chartTitle:=$"Running Insulin On Board (IOB)",
            name:=NameOf(ActiveInsulinChartTitle),
            foreColor:=GetGraphLineColor(key:="Active Insulin"))
        Me.ActiveInsulinActiveInsulinSeries = CreateSeriesActiveInsulin()
        Me.ActiveInsulinTargetSeries = CreateSeriesLimitsAndTarget(
            limitsLegend:=_activeInsulinChartLegend,
            seriesName:=TargetSgSeriesName)

        Me.ActiveInsulinAutoCorrectionSeries = CreateSeriesBasal(
            name:=AutoCorrectionSeriesName,
            basalLegend:=_activeInsulinChartLegend,
            legendText:="Auto Correction",
            yAxisType:=AxisType.Secondary)
        Me.ActiveInsulinBasalSeries = CreateSeriesBasal(
            name:=BasalSeriesName,
            basalLegend:=_activeInsulinChartLegend,
            legendText:="Basal Series",
            yAxisType:=AxisType.Secondary)
        Me.ActiveInsulinMinBasalSeries = CreateSeriesBasal(
            name:=MinBasalSeriesName,
            basalLegend:=_activeInsulinChartLegend,
            legendText:="Min Basal",
            yAxisType:=AxisType.Secondary)

        Me.ActiveInsulinSuspendSeries =
            CreateSeriesSuspend(basalLegend:=_activeInsulinChartLegend)

        Me.ActiveInsulinSgSeries = CreateSeriesSg(sgLegend:=_activeInsulinChartLegend)
        Me.ActiveInsulinMarkerSeries =
            CreateSeriesWithoutVisibleLegend(YAxisType:=AxisType.Secondary)
        Me.ActiveInsulinTimeChangeSeries =
            CreateSeriesTimeChange(basalLegend:=_activeInsulinChartLegend)

        With Me.ActiveInsulinChart
            With .Series
                .Add(item:=Me.ActiveInsulinTargetSeries)
                .Add(item:=Me.ActiveInsulinTimeChangeSeries)

                .Add(item:=Me.ActiveInsulinActiveInsulinSeries)

                .Add(item:=Me.ActiveInsulinAutoCorrectionSeries)
                .Add(item:=Me.ActiveInsulinBasalSeries)
                .Add(item:=Me.ActiveInsulinMinBasalSeries)

                .Add(item:=Me.ActiveInsulinSgSeries)
                .Add(item:=Me.ActiveInsulinSuspendSeries)
                .Add(item:=Me.ActiveInsulinMarkerSeries)
            End With
            .Series(name:=SgSeriesName).EmptyPointStyle.BorderWidth = 4
            .Series(name:=SgSeriesName).EmptyPointStyle.Color = Color.Transparent
            .Series(name:=ActiveInsulinSeriesName).EmptyPointStyle.BorderWidth = 4
            .Series(name:=ActiveInsulinSeriesName).EmptyPointStyle.Color = Color.Transparent
            .Legends.Add(item:=_activeInsulinChartLegend)
        End With

        Me.ActiveInsulinChart.Titles.Add(item:=Me.ActiveInsulinChartTitle)
        Me.SplitContainer1.Panel2.Controls.Add(value:=Me.ActiveInsulinChart)
        Application.DoEvents()

    End Sub

#End Region ' Initialize Active Insulin Chart

#Region "Initialize Treatment Markers Chart"

    ''' <summary>
    '''  Initializes the Treatment Markers tab chart, including chart area,
    '''  axes, series, and legend. This method sets up the chart for
    '''  displaying treatment details such as insulin delivery and sensor glucose readings.
    ''' </summary>
    ''' <remarks>
    '''  <list type="bullet">
    '''   <item>Clears any existing controls from the treatment details tab page.</item>
    '''   <item>
    '''    Creates and configures the chart and its area,
    '''   including axis labels, intervals, and colors.
    '''  </item>
    '''  <item>
    '''   Sets the maximum insulin delivery row based on the maximum basal per dose.
    '''    Adjusts the interval and label style for the Y-axis to
    '''    display insulin delivery values.
    '''  </item>
    '''  <item>
    '''    Adds all required series for displaying treatment markers. This includes
    '''    target sensor glucose, auto correction, basal series, min basal, suspend,
    '''    sensor glucose, markers, and time change.
    '''   </item>
    '''   <item>Sets the legend and title for the chart.</item>
    '''   <item>
    '''    Adds the chart to the treatment details tab page and processes UI events.
    '''   </item>
    '''  </list>
    ''' </remarks>
    Private Sub InitializeTreatmentMarkersChart()
        Me.TabPage03TreatmentDetails.Controls.Clear()

        Me.TreatmentMarkersChart = CreateChart(key:=NameOf(TreatmentMarkersChart))
        Dim treatmentMarkersChartArea As ChartArea =
            CreateChartArea(containingChart:=Me.TreatmentMarkersChart)

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
                TreatmentInsulinRow = (MaxBasalPerDose + 0.025).RoundTo025
        End Select

        Dim baseColor As Color = Me.TreatmentMarkersChart.BackColor.ContrastingColor()
        Dim labelFont As New Font(FamilyName, emSize:=12.0F, style:=FontStyle.Bold)

        With treatmentMarkersChartArea.AxisY
            Dim interval As Single = (TreatmentInsulinRow / 10).RoundToSingle(digits:=3)
            .Interval = interval
            .IsInterlaced = False
            .IsMarginVisible = False
            With .LabelStyle
                .Font = labelFont
                .ForeColor = baseColor
                .Format = $"{{0{DecimalSeparator}00}}"
            End With
            .LineColor = Color.FromArgb(alpha:=64, baseColor)
            With .MajorTickMark
                .Enabled = True
                .Interval = interval
                .LineColor = Color.FromArgb(alpha:=64, baseColor)
            End With
            .Maximum = TreatmentInsulinRow
            .Minimum = 0
            .Title = "Delivered Insulin"
            .TitleFont = New Font(family:=labelFont.FontFamily, emSize:=14)
            .TitleForeColor = baseColor
        End With

        Me.TreatmentMarkersChart.ChartAreas.Add(item:=treatmentMarkersChartArea)
        _treatmentMarkersChartLegend = CreateChartLegend(
            legendName:=NameOf(_treatmentMarkersChartLegend))

        Me.TreatmentMarkersChartTitle = CreateTitle(
            chartTitle:="Treatment Details",
            name:=NameOf(TreatmentMarkersChartTitle),
            foreColor:=Me.TreatmentMarkersChart.BackColor.ContrastingColor())
        Me.TreatmentTargetSeries = CreateSeriesLimitsAndTarget(
            limitsLegend:=_treatmentMarkersChartLegend,
            seriesName:=TargetSgSeriesName)
        Me.TreatmentAutoCorrectionSeries = CreateSeriesBasal(
            name:=AutoCorrectionSeriesName,
            basalLegend:=_treatmentMarkersChartLegend,
            legendText:="Auto Correction",
            yAxisType:=AxisType.Primary)
        Me.TreatmentBasalSeries = CreateSeriesBasal(
            name:=BasalSeriesName,
            basalLegend:=_treatmentMarkersChartLegend,
            legendText:="Basal Series",
            yAxisType:=AxisType.Primary)
        Me.TreatmentMinBasalSeries = CreateSeriesBasal(
            name:=MinBasalSeriesName,
            basalLegend:=_treatmentMarkersChartLegend,
            legendText:="Min Basal",
            yAxisType:=AxisType.Primary)

        Me.TreatmentSgSeries = CreateSeriesSg(sgLegend:=_treatmentMarkersChartLegend)
        Me.TreatmentMarkersSeries =
            CreateSeriesWithoutVisibleLegend(YAxisType:=AxisType.Primary)
        Me.TreatmentTimeChangeSeries =
            CreateSeriesTimeChange(basalLegend:=_treatmentMarkersChartLegend)
        Me.TreatmentSuspendSeries =
            CreateSeriesSuspend(basalLegend:=_treatmentMarkersChartLegend)

        With Me.TreatmentMarkersChart
            With .Series
                .Add(item:=Me.TreatmentTargetSeries)
                .Add(item:=Me.TreatmentSuspendSeries)
                .Add(item:=Me.TreatmentTimeChangeSeries)

                .Add(item:=Me.TreatmentAutoCorrectionSeries)
                .Add(item:=Me.TreatmentBasalSeries)
                .Add(item:=Me.TreatmentMinBasalSeries)

                .Add(item:=Me.TreatmentSgSeries)
                .Add(item:=Me.TreatmentMarkersSeries)
            End With
            .Legends.Add(item:=_treatmentMarkersChartLegend)
            .Series(name:=SgSeriesName).EmptyPointStyle.Color = Color.Transparent
            .Series(name:=SgSeriesName).EmptyPointStyle.BorderWidth = 4
            .Series(name:=BasalSeriesName).EmptyPointStyle.Color = Color.Transparent
            .Series(name:=BasalSeriesName).EmptyPointStyle.BorderWidth = 4
            .Series(name:=MarkerSeriesName).EmptyPointStyle.Color = Color.Transparent
            .Series(name:=MarkerSeriesName).EmptyPointStyle.BorderWidth = 4
        End With

        Me.TreatmentMarkersChart.Titles.Add(item:=Me.TreatmentMarkersChartTitle)
        Me.TabPage03TreatmentDetails.Controls.Add(value:=Me.TreatmentMarkersChart)
        Application.DoEvents()
    End Sub

#End Region ' Initialize Treatment Markers Chart

#End Region ' Initialize Chart Tabs

#End Region ' Initialize Charts

#Region "NotifyIcon Support"

    ''' <summary>
    '''  Updates the notification icon with the latest sensor glucose value
    '''  and displays a balloon tip if necessary.
    '''  This method is called to refresh the notification icon based on
    '''  the latest sensor glucose reading.
    ''' </summary>
    ''' <param name="sgString">The last sensor glucose value as a string.</param>
    Private Sub UpdateNotifyIcon(sgString As String)
        Try
            Dim sg As Single = s_lastSg.sg
            Dim tipText As String
            Using bitmapText As New Bitmap(width:=16, height:=16)
                Using g As Graphics = Graphics.FromImage(bitmapText)
                    Dim backColor As Color
                    Select Case sg
                        Case <= GetTirLowLimit()
                            backColor = Color.Yellow
                            tipText = $"SG below {GetTirLowLimitWithUnits()} {BgUnits}"
                            If _showBalloonTip Then
                                Me.NotifyIcon1.ShowBalloonTip(
                                    timeout:=10000,
                                    tipTitle:=$"CareLink™ Alert",
                                    tipText,
                                    tipIcon:=Me.ToolTip1.ToolTipIcon)
                            End If
                            _showBalloonTip = False
                        Case <= GetTirHighLimit()
                            backColor = Color.Green
                            _showBalloonTip = True
                        Case Else
                            backColor = Color.Red
                            If _showBalloonTip Then
                                tipText = $"SG above {GetTirHighLimitWithUnits()} {BgUnits}"
                                Me.NotifyIcon1.ShowBalloonTip(
                                    timeout:=10000,
                                    tipTitle:=$"CareLink™ Alert",
                                    tipText,
                                    tipIcon:=Me.ToolTip1.ToolTipIcon)
                            End If
                            _showBalloonTip = False
                    End Select

                    Dim s As String =
                        sgString.PadRight(totalWidth:=3) _
                                .Substring(startIndex:=0, length:=3).Trim _
                                .PadLeft(totalWidth:=3)

                    Me.NotifyIcon1.Icon = CreateTextIcon(s, backColor)
                    Dim strBuilder As New StringBuilder(capacity:=100)
                    Dim dateSeparator As String =
                        CultureInfo.CurrentUICulture.DateTimeFormat.DateSeparator
                    strBuilder.AppendLine(
                        value:=Date.Now().ToShortDateTimeString _
                                   .Remove(s:=$"{dateSeparator}{Now.Year}"))
                    strBuilder.AppendLine(value:=$"Last SG {sgString} {BgUnits}")
                    If PatientData.ConduitInRange Then
                        If s_lastSgValue.IsSgInvalid Then
                            Me.TrendValueLabel.Text = ""
                            Me.TrendValueLabel.Visible = False
                            _sgMiniDisplay.SetCurrentDeltaValue(deltaString:="", delta:=0)
                        Else
                            Dim delta As Single = sg - s_lastSgValue
                            Dim deltaString As String = ""
                            Dim provider As CultureInfo = CultureInfo.InvariantCulture

                            If sg.IsSgInvalid Then
                                Me.TrendValueLabel.Text = ""
                                _sgMiniDisplay.SetCurrentDeltaValue(deltaString, delta:=0)
                            Else
                                deltaString =
                                    If(Math.Abs(value:=delta) < 0.001,
                                       "0",
                                       delta.ToString(format:=GetSgFormat(withSign:=True), provider))

                                Me.TrendValueLabel.Text = deltaString
                                _sgMiniDisplay.SetCurrentDeltaValue(deltaString, delta)
                            End If
                            Me.TrendValueLabel.ForeColor = backColor
                            strBuilder.AppendLine($"SG Trend { deltaString}")
                            Me.TrendValueLabel.Visible = True
                        End If
                    Else
                        Me.TrendValueLabel.Visible = False
                    End If
                    strBuilder.Append(
                        value:=$"Active ins. {PatientData.ActiveInsulin?.Amount:N3} U")
                    Me.NotifyIcon1.Text = strBuilder.ToString()
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

    ''' <summary>
    '''  Fixes the <see cref="SplitContainer"/> control's SplitterDistance
    '''  based on the current form scale.
    '''  This method is called to ensure that the SplitContainer's
    '''  splitter distance is correctly scaled when the form is resized or scaled.
    ''' </summary>
    ''' <param name="sp">The SplitContainer control to fix.</param>
    ''' <remarks>
    '''  The method adjusts the SplitterDistance based on the orientation
    '''  and fixed panel of the SplitContainer.
    ''' </remarks>
    Private Sub Fix(sp As SplitContainer)
        ' Scale factor depends on orientation
        Dim sc As Single = If(sp.Orientation = Orientation.Vertical,
                              _formScale.Width,
                              _formScale.Height)

        If sp.FixedPanel = FixedPanel.Panel1 Then
            sp.SplitterDistance =
                CInt(Math.Truncate(Math.Round(sp.SplitterDistance * sc)))
        ElseIf sp.FixedPanel = FixedPanel.Panel2 Then
            Dim cs As Integer = If(sp.Orientation = Orientation.Vertical,
                                   sp.Panel2.ClientSize.Width,
                                   sp.Panel2.ClientSize.Height)

            sp.SplitterDistance -= CInt(Math.Truncate(cs * sc)) - cs
        End If
    End Sub

    ''' <summary>
    '''  Recursively fixes the SplitContainer controls within the specified control.
    '''  This method is called to ensure that all SplitContainer controls
    '''  in the control hierarchy are fixed.
    ''' </summary>
    ''' <param name="c">The control containing SplitContainer controls to fix.</param>
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

    ''' <summary>
    '''  Returns a subtitle string for the summary chart based on the
    '''  current AutoMode state and basal pattern. If in AutoMode, the subtitle
    '''  reflects the AutoMode state and remaining time if in Safe Basal.
    '''  Otherwise, it returns the active basal pattern and rate.
    ''' </summary>
    ''' <returns>
    '''  A string representing the current subtitle for the summary chart.
    ''' </returns>
    Private Function GetSubTitle() As String
        Dim title As String = ""
        If InAutoMode Then
            Dim autoModeState As String =
                s_therapyAlgorithmStateValue(
                    key:=NameOf(TherapyAlgorithmState.AutoModeShieldState))
            Select Case autoModeState
                Case "AUTO_BASAL"
                    title = If(Is700Series(),
                               "AutoMode",
                               "SmartGuard")

                Case "SAFE_BASAL"
                    title = autoModeState.ToTitle
                    Dim key As String = NameOf(TherapyAlgorithmState.SafeBasalDuration)
                    Dim safeBasalDuration As UInteger =
                        CUInt(s_therapyAlgorithmStateValue(key))
                    If safeBasalDuration > 0 Then
                        title &= $", {TimeSpan.FromMinutes(safeBasalDuration):h\:mm} left."
                    End If
            End Select
        Else
            Dim pattern As String = s_basalList.ActiveBasalPattern
            Return $"{pattern} rate = {s_basalList.GetBasalPerHour} U Per Hour".CleanSpaces
        End If
        Return title
    End Function

    ''' <summary>
    '''  Updates the Active Insulin value display on the main form
    '''  and mini display.
    ''' </summary>
    ''' <remarks>
    '''  If the active insulin value is available and non-negative,
    '''  it is formatted and displayed.
    '''  Otherwise, "Unknown" or "---" is shown.
    ''' </remarks>
    ''' <exception cref="ArithmeticException">
    '''  Thrown if an arithmetic error occurs while updating the active insulin value.
    ''' </exception>
    ''' <exception cref="ApplicationException">
    '''  Thrown if a general error occurs while updating the active insulin value.
    ''' </exception>
    Private Sub UpdateActiveInsulin()
        Try
            If PatientData.ActiveInsulin IsNot Nothing AndAlso
               PatientData.ActiveInsulin.Amount >= 0 Then

                Dim activeInsulinStr As String =
                    $"Active Insulin {$"{PatientData.ActiveInsulin.Amount:N3}"} U"
                Me.ActiveInsulinValue.Text = activeInsulinStr
                _sgMiniDisplay.ActiveInsulinTextBox.Text = activeInsulinStr
            Else
                Me.ActiveInsulinValue.Text = $"Active Insulin Unknown"
                _sgMiniDisplay.ActiveInsulinTextBox.Text = $"Active Insulin --- U"
            End If
        Catch ex As ArithmeticException
            Stop
            Throw New ArithmeticException(
                message:=$"{ex.DecodeException()} exception in {NameOf(UpdateActiveInsulin)}")
        Catch innerException As Exception
            Stop
            Throw New ApplicationException(
                message:=$"{innerException.DecodeException()} exception in {NameOf(UpdateActiveInsulin)}",
                innerException)
        End Try
    End Sub

    ''' <summary>
    '''  Updates the Active Insulin chart with the latest data.
    ''' </summary>
    ''' <remarks>
    '''  Clears and repopulates the chart series with new data points
    '''  based on the current markers and user settings.
    '''  Calculates active insulin on board (IOB) for each 5-minute interval,
    '''  using all relevant markers.
    '''  Handles auto basal, manual basal, insulin, and low glucose suspended markers.
    '''  Updates the chart's Y axis maximum, plots suspend areas,
    '''  markers, sensor glucose series, and high/low/target limits.
    ''' </remarks>
    ''' <exception cref="ApplicationException">
    '''  Thrown if an error occurs while updating the chart.
    ''' </exception>
    Private Sub UpdateActiveInsulinChart()
        If Not ProgramInitialized Then
            Exit Sub
        End If

        Try
            Me.TemporaryUseAdvanceAITDecayCheckBox.Checked = CurrentUser.UseAdvancedAitDecay = CheckState.Checked
            If Me.ActiveInsulinChart Is Nothing Then
                Return
            End If
            For Each s As Series In Me.ActiveInsulinChart?.Series
                s.Points.Clear()
            Next

            With Me.ActiveInsulinChart
                .Titles(name:=NameOf(ActiveInsulinChartTitle)).Text =
                    $"Running Insulin On Board (IOB){s_basalList.Subtitle()}"
                .ChartAreas(name:=NameOf(ChartArea)).UpdateChartAreaSgAxisX()

                If s_markers.Count = 0 Then
                    Exit Sub
                End If

                ' Order all markers by time and sum bolus amounts for the same key
                Dim timeOrderedMarkers As New SortedDictionary(Of OADate, Single)

                For Each markerWithIndex As IndexClass(Of Marker) In s_markers.WithIndex()
                    Dim marker As Marker = markerWithIndex.Value
                    Dim key As New OADate(asDate:=marker.GetMarkerTimestamp)

                    Dim bolusAmount As Single = 0
                    Dim shouldAdd As Boolean = False

                    Select Case marker.Type
                        Case "AUTO_BASAL_DELIVERY", "MANUAL_BASAL_DELIVERY"
                            bolusAmount = marker.GetSingleFromJson(NameOf(AutoBasalDelivery.BolusAmount))
                            shouldAdd = True

                        Case "INSULIN"
                            bolusAmount = marker.GetSingleFromJson(NameOf(Insulin.DeliveredFastAmount))
                            shouldAdd = True

                        Case "LOW_GLUCOSE_SUSPENDED"
                            If PatientData.ConduitSensorInRange AndAlso
                                CurrentPdf?.IsValid AndAlso
                                Not InAutoMode Then

                                For Each kvp As KeyValuePair(Of OADate, Single) In GetManualBasalValues(markerWithIndex)
                                    If timeOrderedMarkers.ContainsKey(kvp.Key) Then
                                        timeOrderedMarkers(kvp.Key) += kvp.Value
                                    Else
                                        timeOrderedMarkers.Add(kvp.Key, kvp.Value)
                                    End If
                                Next
                            End If

                        Case "BG_READING", "CALIBRATION", "MEAL", "TIME_CHANGE"
                            ' Ignored marker types

                        Case Else
                            Stop
                    End Select

                    If shouldAdd Then
                        If timeOrderedMarkers.ContainsKey(key) Then
                            timeOrderedMarkers(key) += bolusAmount
                        Else
                            timeOrderedMarkers.Add(key, bolusAmount)
                        End If
                    End If
                Next
                Dim upCount As Integer = s_insulinTypes(key:=CurrentUser.InsulinTypeName).UpCount
                Dim windowSize As Integer = CInt(s_insulinTypes(key:=CurrentUser.InsulinTypeName).AitHours * 12)
                Dim timestamp As Date = s_sgRecords(index:=0).Timestamp
                Dim insulinIncrements As Integer = CurrentUser.GetActiveInsulinIncrements
                ' set up table that holds active insulin for every 5 minutes
                If _remainingInsulinList.Count >= 288 Then
                    If _remainingInsulinList.Count > 288 + insulinIncrements Then
                        _remainingInsulinList.RemoveAt(index:=0)
                    End If
                    Dim n As Integer = _remainingInsulinList.Count - 287
                    _remainingInsulinList = _remainingInsulinList.Take(count:=n).ToList()
                End If
                Dim currentMarker As Integer = 0
                For i As Integer = 0 To 287
                    Dim initialInsulinLevel As Single = 0
                    Dim timeSpan As TimeSpan = FiveMinuteSpan * i
                    Dim firstValidOaTime As _
                        New OADate(asDate:=(timestamp + timeSpan).RoundDownToMinute())
                    While currentMarker < timeOrderedMarkers.Count AndAlso
                        timeOrderedMarkers.Keys(index:=currentMarker) <= firstValidOaTime

                        initialInsulinLevel += timeOrderedMarkers.Values(index:=currentMarker)
                        currentMarker += 1
                    End While
                    Dim item As New RunningActiveInsulin(
                        firstValidOaTime,
                        initialInsulinLevel,
                        insulinIncrements,
                        upCount)
                    _remainingInsulinList.Add(item)
                Next

                .ChartAreas(name:=NameOf(ChartArea)).AxisY2.Maximum = GetYMaxValueFromNativeMmolL()
                ' walk all markers, adjust active insulin and then add new markerWithIndex
                Dim maxActiveInsulin As Double = 0
                Dim count As Integer = CurrentUser.GetActiveInsulinIncrements
                Dim startIndex As Integer = _remainingInsulinList.Count - 288
                For i As Integer = startIndex To startIndex + 287
                    If i < windowSize - 1 Then
                        With Me.ActiveInsulinActiveInsulinSeries
                            .Points.AddXY(
                                xValue:=_remainingInsulinList(index:=i).OaDateTime,
                                yValue:=Double.NaN)
                            .Points.Last.IsEmpty = True
                        End With
                        If i > 0 Then
                            _remainingInsulinList.AdjustList(start:=0, count)
                        End If
                        Continue For
                    End If
                    Dim start As Integer = i - count + 1
                    Dim sum As Double = _remainingInsulinList.ConditionalSum(index:=start, count)
                    maxActiveInsulin = Math.Max(sum, maxActiveInsulin)
                    Me.ActiveInsulinActiveInsulinSeries.Points.AddXY(
                        xValue:=_remainingInsulinList(index:=i).OaDateTime,
                        yValue:=sum)
                    _remainingInsulinList.AdjustList(start, count)
                Next

                .ChartAreas(name:=NameOf(ChartArea)).AxisY.Maximum = Math.Ceiling(maxActiveInsulin) + 1
                .PlotSuspendArea(SuspendSeries:=Me.ActiveInsulinSuspendSeries)
                .PlotMarkers(
                    timeChangeSeries:=Me.ActiveInsulinTimeChangeSeries,
                    markerInsulinDictionary:=s_activeInsulinMarkers,
                    markerMealDictionary:=Nothing)
                .PlotSgSeries(HomePageMealRow:=GetYMinValueFromNativeMmolL())
                .PlotHighLowLimitsAndTargetSg(targetSgOnly:=True)
            End With
            Application.DoEvents()
        Catch innerException As Exception
            Stop
            Throw New ApplicationException(
                message:=$"{innerException.DecodeException()} exception in {NameOf(UpdateActiveInsulinChart)}",
                innerException)
        End Try
    End Sub

    ''' <summary>
    '''  Updates all summary chart series with the latest data and settings.
    ''' </summary>
    ''' <remarks>
    '''  <para>
    '''   Clears and repopulates the chart series for the summary chart,
    '''   including suspend areas, markers, sensor glucose series, and high/low/target limits.
    ''' </para>
    ''' <para>
    '''  The chart title is updated with the current subtitle.
    '''  This method should be called after data changes to refresh the summary chart display.
    ''' </para>
    ''' </remarks>
    ''' <exception cref="ApplicationException">
    '''  Thrown if an error occurs while plotting markers in the summary chart.
    ''' </exception>
    Private Sub UpdateAllSummarySeries()
        Try
            With Me.SummaryChart

                For Each s As Series In .Series
                    s.Points.Clear()
                Next
                .ChartAreas(NameOf(ChartArea)).UpdateChartAreaSgAxisX()
                .Titles(index:=0).Text = $"Status - {Me.GetSubTitle()}"
                .PlotSuspendArea(SuspendSeries:=Me.SummarySuspendSeries)
                .PlotMarkers(
                    timeChangeSeries:=Me.SummaryTimeChangeSeries,
                    markerInsulinDictionary:=s_summaryMarkersInsulin,
                    markerMealDictionary:=s_summaryMarkersMeal)
                .PlotSgSeries(HomePageMealRow:=GetYMinValueFromNativeMmolL())
                .PlotHighLowLimitsAndTargetSg(targetSgOnly:=False)
                Application.DoEvents()
            End With
        Catch innerException As Exception
            Stop
            Dim str As String = innerException.DecodeException()
            Dim message As String =
                $"{str} exception while plotting Markers in {NameOf(UpdateAllSummarySeries)}"
            Throw New ApplicationException(message, innerException)
        End Try

    End Sub

    ''' <summary>
    '''  Updates the Auto Mode shield display on the home tab.
    '''  This method updates the shield image, last sensor glucose time,
    '''  and shield units label based on the current sensor state.
    ''' </summary>
    ''' <remarks>
    '''  The shield image is set based on the sensor state, and the last
    '''  sensor glucose time and shield units are displayed accordingly.
    ''' </remarks>
    Private Sub UpdateAutoModeShield()
        Try
            Dim lastSgTimestamp As String = s_lastSg.Timestamp.ToString(format:=s_timeWithMinuteFormat)
            Me.LastSgOrExitTimeLabel.Text = lastSgTimestamp
            Me.ShieldUnitsLabel.Text = BgUnits

            If InAutoMode Then
                Select Case PatientData.SensorState
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
                Me.ShieldUnitsLabel.Visible = PatientData.SensorState = "NO_ERROR_MESSAGE"
                Me.LastSgOrExitTimeLabel.Visible = False
            End If

            Dim message As String = ""
            If s_lastSg.sg.IsSgValid Then
                Me.SensorMessageLabel.Visible = False
                Dim sgString As String = New SG(PatientData.LastSG).ToString()
                Me.CurrentSgLabel.Text = sgString
                Me.CurrentSgLabel.CenterXYOnParent(verticalOffset:=-Me.ShieldUnitsLabel.Height)
                Me.CurrentSgLabel.Visible = True
                Me.ShieldUnitsLabel.CenterLabelXOnParent()
                Me.ShieldUnitsLabel.Top = Me.CurrentSgLabel.Bottom + 2
                Me.UpdateNotifyIcon(sgString)
                _sgMiniDisplay.SetCurrentSgString(sgString, f:=s_lastSg.sg)
                message = SG.FormatSensorMessage(key:=PatientData.SensorState, truncate:=True)
            Else
                _sgMiniDisplay.SetCurrentSgString(sgString:="---", f:=s_lastSg.sg)
                Me.CurrentSgLabel.Visible = False
                Me.LastSgOrExitTimeLabel.Visible = False
                Me.SensorMessageLabel.Visible = True
                Me.SensorMessageLabel.BackColor = Color.Transparent

                message = SG.FormatSensorMessage(key:=PatientData.SensorState, truncate:=True)
                Select Case PatientData.SensorState
                    Case "UNKNOWN"
                        Me.SensorMessageLabel.Text = message
                        Me.SensorMessageLabel.CenterXYOnParent(verticalOffset:=-5)
                    Case "WARM_UP"
                        Dim timeRemaining As String = ""
                        If s_systemStatusTimeRemaining.TotalMilliseconds > 0 Then
                            timeRemaining = s_systemStatusTimeRemaining.ToFormattedTimeSpan(unit:="hr")
                            Me.SensorMessageLabel.Text = $"{message.Remove(s:="...")}{vbCrLf}{timeRemaining}"
                            Me.SensorMessageLabel.CenterXYOnParent(verticalOffset:=-5)
                        Else
                            Me.SensorMessageLabel.Text = message
                            Me.SensorMessageLabel.CenterXYOnParent(verticalOffset:=-5)
                        End If
                    Case "CALIBRATION_REQUIRED"
                    Case Else
                        Me.SensorMessageLabel.Text = message
                End Select
                Me.SensorMessageLabel.Visible = True
                Me.ShieldUnitsLabel.Visible = False
                Application.DoEvents()
            End If
        Catch innerException As Exception
            Stop
            Throw New ApplicationException(
                message:=$"{innerException.DecodeException()} exception in {NameOf(UpdateAutoModeShield)}",
                innerException)
        End Try
        Application.DoEvents()
    End Sub

    ''' <summary>
    '''  Updates the calibration time remaining display on the home tab.
    '''  This method updates the calibration due image based on the current sensor state
    '''  and time to next calibration.
    ''' </summary>
    ''' <remarks>
    '''  The calibration due image is set based on the time remaining for calibration
    '''  and the sensor state.
    '''  If the sensor is in range, the image is updated to reflect the calibration status:
    '''  - If the time to next calibration is unknown (>= Byte.MaxValue),
    '''    a default arc is shown.
    '''  - If calibration is due now (0 hours), the image reflects whether
    '''    calibration is not ready or required.
    '''  - If the time to next calibration is -1, the image is cleared.
    '''  - Otherwise, the image shows a progress arc for the remaining minutes.
    '''  The image is only visible if the sensor is in range.
    ''' </remarks>
    Private Sub UpdateCalibrationTimeRemaining()
        Try
            If PatientData.ConduitInRange Then
                If PatientData.TimeToNextCalibHours >= Byte.MaxValue Then
                    Dim calibrationDot As Bitmap = My.Resources.CalibrationDot
                    Me.CalibrationDueImage.Image =
                        calibrationDot.DrawCenteredArc(minutesToNextCalibration:=720)
                ElseIf PatientData.TimeToNextCalibHours = 0 Then
                    Dim notReady As Boolean =
                        PatientData.SystemStatusMessage = "WAIT_TO_CALIBRATE" OrElse
                        PatientData.SensorState = "WARM_UP" OrElse
                        PatientData.SensorState = "CHANGE_SENSOR"
                    If notReady Then
                        Me.CalibrationDueImage.Image = My.Resources.CalibrationNotReady
                    Else
                        Dim minutesToNextCalibration As Short = s_timeToNextCalibrationMinutes
                        Dim calibrationDotRed As Bitmap = My.Resources.CalibrationDotRed
                        Me.CalibrationDueImage.Image =
                            calibrationDotRed.DrawCenteredArc(minutesToNextCalibration)
                    End If
                ElseIf s_timeToNextCalibrationMinutes = -1 Then
                    Me.CalibrationDueImage.Image = Nothing
                Else
                    Dim minutesToNextCalibration As Short = s_timeToNextCalibrationMinutes
                    Me.CalibrationDueImage.Image =
                        My.Resources.CalibrationDot.DrawCenteredArc(minutesToNextCalibration)
                End If
            End If
            Me.CalibrationDueImage.Visible = PatientData.ConduitInRange
        Catch innerException As Exception
            Stop
            Throw New ApplicationException(
                message:=$"{innerException.DecodeException()} exception in {NameOf(UpdateCalibrationTimeRemaining)}",
                innerException)
        End Try

        Application.DoEvents()
    End Sub

    ''' <summary>
    '''  Updates the dosing and carbs information on the home tab.
    '''  This method calculates and displays the total insulin
    '''  doses, basal rates, auto corrections, manual boluses,
    '''  and carbs for the last 24 hours.
    ''' </summary>
    ''' <remarks>
    '''  The method iterates through markers to calculate total
    '''  doses and updates the corresponding labels on the form.
    ''' </remarks>
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
                    Dim deliveredAmount As String = marker.GetSingleFromJson(
                        key:=NameOf(Insulin.DeliveredFastAmount)).ToString
                    s_totalDailyDose += deliveredAmount.ParseSingle(digits:=3)
                    Select Case marker.GetStringFromJson(key:=NameOf(Insulin.ActivationType))
                        Case "AUTOCORRECTION"
                            s_totalAutoCorrection += deliveredAmount.ParseSingle(digits:=3)
                        Case "MANUAL", "RECOMMENDED", "UNDETERMINED"
                            s_totalManualBolus += deliveredAmount.ParseSingle(digits:=3)
                    End Select

                Case "AUTO_BASAL_DELIVERY"
                    Dim amount As Single = marker.GetSingleFromJson(
                        key:=NameOf(AutoBasalDelivery.BolusAmount),
                        digits:=3)
                    s_totalBasal += amount
                    s_totalDailyDose += amount
                Case "MANUAL_BASAL_DELIVERY"
                    Dim amount As Single = marker.GetSingleFromJson(
                        key:=NameOf(AutoBasalDelivery.BolusAmount),
                        digits:=3)
                    s_totalBasal += amount
                    s_totalDailyDose += amount
                Case "MEAL"
                    s_totalCarbs += CInt(marker.GetSingleFromJson(key:="amount"))
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
                    s_totalBasal = activeBasalRecords(index:=0).UnitsPerHr * 24
                    s_totalDailyDose += s_totalBasal
                Else
                    For Each e As IndexClass(Of BasalRateRecord) In
                        activeBasalRecords.WithIndex

                        Dim basalRate As BasalRateRecord = e.Value
                        startTime = basalRate.Time
                        endTime = If(e.IsLast,
                                     New TimeOnly(
                                        hour:=23,
                                        minute:=59,
                                        second:=59,
                                        millisecond:=999),
                                     activeBasalRecords(index:=e.Index + 1).Time)

                        Dim theTimeSpan As TimeSpan = endTime - startTime
                        Dim periodInHours As Double =
                            theTimeSpan.Hours +
                            (theTimeSpan.Minutes / 60) +
                            (theTimeSpan.Seconds / 3600)

                        s_totalBasal += CSng(periodInHours * basalRate.UnitsPerHr)
                    Next
                    s_totalDailyDose += s_totalBasal
                End If
            End If
        End If

        Dim provider As CultureInfo = CultureInfo.CurrentUICulture
        Dim totalPercent As String = If(s_totalDailyDose = 0,
                                        "???",
                                        $"{CInt(s_totalBasal / s_totalDailyDose * 100)}")

        Me.Last24HrBasalUnitsLabel.Text = String.Format(provider, format:=$"{s_totalBasal:F1} U")
        Me.Last24HrBasalPercentLabel.Text = $"{totalPercent}%"

        Me.Last24HrTotalInsulinUnitsLabel.Text = String.Format(provider, format:=$"{s_totalDailyDose:F1} U")

        If s_totalAutoCorrection > 0 Then
            Me.Last24HrAutoCorrectionLabel.Visible = True
            Me.Last24HrAutoCorrectionUnitsLabel.ForeColor = Color.LightGray
            Me.Last24HrAutoCorrectionUnitsLabel.Text =
                String.Format(provider, format:=$"{s_totalAutoCorrection:F1} U")
            Me.Last24HrAutoCorrectionLabel.ForeColor = Color.Gray
            Me.Last24HrAutoCorrectionUnitsLabel.Visible = True
            If s_totalDailyDose > 0 Then
                totalPercent = CInt(s_totalAutoCorrection / s_totalDailyDose * 100).ToString
            End If
            Me.Last24HrAutoCorrectionPercentLabel.ForeColor = Color.LightGray
            Me.Last24HrAutoCorrectionPercentLabel.Visible = True
            Me.Last24HrAutoCorrectionPercentLabel.Text = $"{totalPercent}%"
            Me.Last24HrAutoCorrectionPercentLabel.ForeColor = Color.LightGray
            If s_totalDailyDose > 0 Then
                totalPercent = CInt(s_totalManualBolus / s_totalDailyDose * 100).ToString
            End If
            Me.Last24HrMealBolusUnitsLabel.Text =
                String.Format(provider, format:=$"{s_totalManualBolus:F1} U")
            Me.Last24HrMealBolusPercentLabel.Text = $"{totalPercent}%"
        Else
            Me.Last24HrAutoCorrectionLabel.ForeColor = Color.FromArgb(red:=64, green:=64, blue:=64)
            If s_autoModeStatusMarkers.Count = 0 Then
                Me.Last24HrAutoCorrectionUnitsLabel.Visible = False
                Me.Last24HrAutoCorrectionPercentLabel.Visible = False
            Else
                Me.Last24HrAutoCorrectionUnitsLabel.ForeColor = Color.DarkGray
                Me.Last24HrAutoCorrectionPercentLabel.ForeColor = Color.DarkGray
                Me.Last24HrAutoCorrectionPercentLabel.Text = "0%"
            End If
            If s_totalDailyDose > 0 Then
                totalPercent = CInt(s_totalManualBolus / s_totalDailyDose * 100).ToString
            End If
            Me.Last24HrMealBolusUnitsLabel.Text =
                String.Format(provider, format:=$"{s_totalManualBolus:F1} U")
            Me.Last24HrMealBolusPercentLabel.Text = $"{totalPercent}%"
        End If
        Me.Last24HrAutoCorrectionUnitsLabel.Text =
                String.Format(provider, format:=$"{s_totalAutoCorrection:F1} U")
        Me.Last24HrCarbsValueLabel.Text =
            $"{s_totalCarbs} {GetCarbDefaultUnit()}{Superscript3}"
    End Sub

    ''' <summary>
    '''  Updates the insulin level display on the home tab.
    '''  This method updates the insulin level picture box and remaining
    '''  insulin units label based on the current reservoir level.
    ''' </summary>
    ''' <remarks>
    '''  The insulin level picture box is set based on the reservoir
    '''  level percentage, and the remaining insulin units are displayed accordingly.
    ''' </remarks>
    Private Sub UpdateInsulinLevel()
        ' This function is subject to crash if the ImageList is disposed on exit.
        Try
            Me.InsulinLevelPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
            If Not PatientData.ConduitInRange Then
                Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(index:=8)
                Me.RemainingInsulinUnits.Text = "???U"
            Else
                Dim remainingUnits As Double = PatientData.ReservoirRemainingUnits
                Me.RemainingInsulinUnits.Text = $"{remainingUnits:N1} U"
                Select Case PatientData.ReservoirLevelPercent
                    Case >= 85
                        Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(index:=7)
                    Case >= 71
                        Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(index:=6)
                    Case >= 57
                        Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(index:=5)
                    Case >= 43
                        Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(index:=4)
                    Case >= 29
                        Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(index:=3)
                    Case >= 15
                        Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(index:=2)
                    Case >= 1
                        Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(index:=1)
                    Case Else
                        Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(index:=0)
                End Select
            End If
        Finally
            Application.DoEvents()
        End Try
    End Sub

    ''' <summary>
    '''  Updates the pump battery status display on the home tab.
    '''  This method updates the pump battery picture box and
    '''  remaining battery percentage label based on the current pump battery level.
    ''' </summary>
    ''' <remarks>
    '''  The pump battery picture box is set based on the battery level percentage,
    '''  and the remaining battery percentage is displayed accordingly.
    ''' </remarks>
    Private Sub UpdatePumpBattery()
        If Not PatientData.ConduitInRange Then
            Me.PumpBatteryPictureBox.Image = My.Resources.PumpConnectivityToPhoneNotOK
            Me.PumpBatteryRemainingLabel.Text = "Pump out"
            Me.PumpBatteryRemaining2Label.Text = "of range"
            Exit Sub
        End If

        Dim batteryLeftPercent As Integer = PatientData.PumpBatteryLevelPercent
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

    ''' <summary>
    '''  Updates the sensor life display on the home tab.
    '''  This method updates the sensor days left label and sensor time left
    '''  picture box based on the current sensor duration hours.
    ''' </summary>
    ''' <remarks>
    '''  The sensor days left label is set based on the remaining sensor duration,
    '''  and the sensor time left picture box is updated accordingly.
    ''' </remarks>
    Private Sub UpdateSensorLife()
        If PatientData.ConduitInRange Then

            Select Case PatientData.SensorDurationHours
                Case Is >= 255
                    Me.SensorDaysLeftLabel.Text = ""
                    Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorExpirationUnknown
                    Me.SensorTimeLeftLabel.Text = "Unknown"
                Case Is >= 168
                    Me.SensorDaysLeftLabel.Text = "~7"
                    Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorLifeOK
                    Me.SensorTimeLeftLabel.Text = "7 Days"

                Case Is >= 24
                    Me.SensorDaysLeftLabel.Text = Math.Ceiling(PatientData.SensorDurationHours / 24).ToString()
                    Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorLifeOK
                    Me.SensorTimeLeftLabel.Text = $"{Me.SensorDaysLeftLabel.Text} Days"
                Case Is > 0
                    Me.SensorDaysLeftLabel.Text = $"<{Math.Ceiling(PatientData.SensorDurationHours / 24)}"
                    Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorLifeNotOK
                    Me.SensorTimeLeftLabel.Text = $"{PatientData.SensorDurationHours} Hours"
                Case Is = 0
                    Dim sensorDurationMinutes As Integer = If(PatientData.SensorDurationMinutes Is Nothing,
                                                              -1,
                                                              CInt(PatientData.SensorDurationMinutes))
                    Select Case sensorDurationMinutes
                        Case Is > 0
                            Me.SensorDaysLeftLabel.Text = "0"
                            Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorLifeNotOK
                            Me.SensorTimeLeftLabel.Text = $"{sensorDurationMinutes} minutes"
                        Case Is = 0
                            Me.SensorDaysLeftLabel.Text = ""
                            Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorExpired
                            Me.SensorTimeLeftLabel.Text = "Expired"
                        Case Else
                            Me.SensorDaysLeftLabel.Text = ""
                            Me.SensorTimeLeftPictureBox.Image =
                                My.Resources.SensorExpirationUnknown
                            Me.SensorTimeLeftLabel.Text = "Unknown"
                    End Select

                Case Else
                    Me.SensorDaysLeftLabel.Text = ""
                    Me.SensorTimeLeftPictureBox.Image = My.Resources.SensorExpirationUnknown
                    Me.SensorTimeLeftLabel.Text = "Unknown"
            End Select
            Me.SensorDaysLeftLabel.Visible = True
        End If
        Me.SensorTimeLeftPanel.Visible = PatientData.ConduitInRange
    End Sub

    ''' <summary>
    '''  Updates the Time in Range chart with the latest data and settings.
    ''' </summary>
    ''' <remarks>
    '''  Clears and repopulates the chart series with new data points based
    '''  on the current patient data and user settings.
    ''' </remarks>
    Private Sub UpdateTimeInRange()
        If Me.TimeInRangeChart Is Nothing Then
            Stop
            Exit Sub
        End If

        _timeInTightRange = GetTIR(tight:=True)
        Me.TimeInRangeChartLabel.Text = GetTIR.AsString
        With Me.TimeInRangeChart
            With .Series(name:=NameOf(TimeInRangeSeries)).Points
                .Clear()
                .AddXY(
                    $"{GetBelowHypoLimit.Str}% Below {GetTirLowLimitWithUnits()}",
                    PatientData.BelowHypoLimit.RoundToSingle(digits:=1) / 100)
                .Last().Color = Color.Red
                .Last().BorderColor = Color.Black
                .Last().BorderWidth = 2
                .AddXY(
                    $"{GetAboveHyperLimit.Str}% Above {GetTirHighLimitWithUnits()}",
                    PatientData.AboveHyperLimit.RoundToSingle(digits:=1) / 100)
                .Last().Color = Color.Yellow
                .Last().BorderColor = Color.Black
                .Last().BorderWidth = 2
                Dim tir As UInteger = GetTIR.Percent
                If _timeInTightRange.Uint = tir Then
                    .AddXY(
                        $"{_timeInTightRange.Str}% In Tight Range = TIR",
                        _timeInTightRange.Uint / 100)
                    .Last().Color = Color.LimeGreen
                    .Last().BorderColor = Color.Black
                    .Last().BorderWidth = 2
                ElseIf _timeInTightRange.Uint < tir Then
                    .AddXY(
                        $"{GetTIR.AsString}% In Range",
                        (tir - _timeInTightRange.Uint) / 100)
                    .Last().Color = Color.Green
                    .Last().BorderColor = Color.Black
                    .Last().BorderWidth = 2

                    .AddXY(
                        $"{_timeInTightRange.Str}% In Tight Range",
                        _timeInTightRange.Uint / 100)
                    .Last().Color = Color.LimeGreen
                    .Last().BorderColor = Color.Black
                    .Last().BorderWidth = 2
                Else
                    .AddXY(
                        $"{_timeInTightRange.Str}% In Tight Range",
                        _timeInTightRange.Uint / 100)
                    .Last().Color = Color.LimeGreen
                    .Last().BorderColor = Color.Black
                    .Last().BorderWidth = 2

                    .AddXY(
                        $"{GetTIR.AsString}% In Range",
                        (_timeInTightRange.Uint - tir) / 100)
                    .Last().Color = Color.Green
                    .Last().BorderColor = Color.Black
                    .Last().BorderWidth = 2

                End If
            End With
            .Series(NameOf(TimeInRangeSeries))("PieLabelStyle") = "Disabled"
            .Series(NameOf(TimeInRangeSeries))("PieStartAngle") = "270"
        End With

        Me.AboveHighLimitValueLabel.Text = $"{GetAboveHyperLimit.Str}%"
        Me.AboveHighLimitMessageLabel.Text = $"Above {GetTirHighLimitWithUnits()} {BgUnits}"

        Me.TimeInRangeValueLabel.Text = $"{GetTIR.AsString}%"
        If GetTIR.Percent >= 70 Then
            Me.TimeInRangeMessageLabel.ForeColor = Color.DarkGreen
            Me.TimeInRangeValueLabel.ForeColor = Color.DarkGreen
        Else
            Me.TimeInRangeMessageLabel.ForeColor = Color.Red
            Me.TimeInRangeValueLabel.ForeColor = Color.Red
        End If

        Me.TimeInTightRangeValueLabel.Text = $"{_timeInTightRange.Str}%"
        Me.TiTRMgsLabel2.Text = My.Forms.OptionsConfigureTiTR.GetTiTrMsg()
        If _timeInTightRange.Uint >= My.Settings.TiTrTreatmentTargetPercent Then
            Me.TiTRMgsLabel.ForeColor = Color.LimeGreen
            Me.TiTRMgsLabel2.ForeColor = Color.LimeGreen
            Me.TimeInTightRangeValueLabel.ForeColor = Color.LimeGreen
        Else
            Me.TiTRMgsLabel.ForeColor = Color.Red
            Me.TiTRMgsLabel2.ForeColor = Color.Red
            Me.TimeInTightRangeValueLabel.ForeColor = Color.Red
        End If

        Me.BelowLowLimitValueLabel.Text = $"{GetBelowHypoLimit.Str}%"
        Me.BelowLowLimitMessageLabel.Text = $"Below {GetTirLowLimitWithUnits()} {BgUnits}"
        Dim averageSgStr As String =
            RecentData.GetStringValueOrEmpty(NameOf(ServerDataEnum.averageSG))
        Me.AverageSGValueLabel.Text = If(NativeMmolL,
                                         averageSgStr.TruncateSingle(digits:=2),
                                         averageSgStr)

        Me.AverageSGMessageLabel.Text = $"Average SG in {BgUnits}"

        ' Calculate Time in AutoMode
        If s_autoModeStatusMarkers.Count = 0 Then
            Me.SmartGuardLabel.Text = "SmartGuard 0%"
        ElseIf s_autoModeStatusMarkers.Count = 1 AndAlso
               s_autoModeStatusMarkers.First.AutoModeOn Then

            Me.SmartGuardLabel.Text = "SmartGuard 100%"
        Else
            Try
                ' need to figure out %
                Dim autoModeStartTime As New Date
                Dim timeInAutoMode As TimeSpan = ZeroTickSpan
                Dim timestamp As Date
                For Each r As IndexClass(Of AutoModeStatus) In
                    s_autoModeStatusMarkers.WithIndex

                    If r.IsFirst Then
                        If r.Value.AutoModeOn OrElse s_autoModeStatusMarkers.Count = 1 Then
                            autoModeStartTime = r.Value.Timestamp
                            timestamp = s_autoModeStatusMarkers.First.Timestamp
                            timeInAutoMode += timestamp.AddDays(value:=1) - autoModeStartTime
                        Else

                        End If
                    Else
                        If r.Value.AutoModeOn Then
                            If r.IsLast Then
                                timestamp = s_autoModeStatusMarkers.First.Timestamp
                                timeInAutoMode +=
                                    timestamp.AddDays(value:=1) - r.Value.Timestamp
                            Else
                                autoModeStartTime = r.Value.Timestamp
                            End If
                        Else
                            timeInAutoMode += r.Value.Timestamp - autoModeStartTime
                            autoModeStartTime = r.Value.Timestamp
                        End If
                    End If
                Next
                Me.SmartGuardLabel.Text = If(timeInAutoMode >= OneDaySpan,
                                             "SmartGuard 100%",
                                             $"SmartGuard {CInt(timeInAutoMode / OneDaySpan * 100)}%")
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
        Dim highScale As Single =
            (GetYMaxValueFromNativeMmolL() - GetTirHighLimit()) /
            (GetTirLowLimit() - GetYMinValueFromNativeMmolL())

        For Each sg As SG In GetValidSgRecords()
            elements += 1
            If sg.sgMgdL < 70 Then
                lowCount += 1
                If NativeMmolL Then
                    lowDeviations +=
                        ((GetTirLowLimit() - sg.sgMmolL) * MmolLUnitsDivisor) ^ 2
                Else
                    lowDeviations += (GetTirLowLimit() - sg.sgMgdL) ^ 2
                End If
            ElseIf sg.sgMgdL > 180 Then
                highCount += 1
                If NativeMmolL Then
                    highDeviations +=
                        ((sg.sgMmolL - GetTirHighLimit()) * MmolLUnitsDivisor) ^ 2
                Else
                    highDeviations += (sg.sgMgdL - GetTirHighLimit()) ^ 2
                End If
            End If
        Next
        highDeviations /= 11

        If elements = 0 Then
            Me.LowTirComplianceLabel.Text = ""
            Me.HighTirComplianceLabel.Text = ""
        Else
            Dim lowDeviation As Single =
                Math.Sqrt(lowDeviations / (elements - highCount)).RoundToSingle(digits:=1)
            Select Case True
                Case lowDeviation <= 2
                    Me.LowTirComplianceLabel.Text = $"Low{vbCrLf}Excellent{Superscript2}"
                    Me.LowTirComplianceLabel.ForeColor = Color.LimeGreen
                Case lowDeviation <= 4
                    Me.LowTirComplianceLabel.Text =
                        $"Low{vbCrLf}({lowDeviation}) OK{Superscript2}"
                    Me.LowTirComplianceLabel.ForeColor = Color.Yellow
                Case Else
                    Me.LowTirComplianceLabel.Text =
                        $"Low{vbCrLf}({lowDeviation}) Needs{vbCrLf}Improvement{Superscript2}"
                    Me.LowTirComplianceLabel.ForeColor = Color.Red
            End Select

            Dim highDeviation As Single =
                Math.Sqrt(highDeviations / (elements - lowCount)).RoundToSingle(digits:=1)
            Select Case True
                Case highDeviation <= 2
                    Me.HighTirComplianceLabel.Text =
                        $"High{vbCrLf}Excellent{Superscript2}"
                    Me.HighTirComplianceLabel.ForeColor = Color.LimeGreen
                Case highDeviation <= 4
                    Me.HighTirComplianceLabel.Text =
                        $"High{vbCrLf}({highDeviation}) OK{Superscript2}"
                    Me.HighTirComplianceLabel.ForeColor = Color.Yellow
                Case Else
                    Me.HighTirComplianceLabel.Text =
                        $"High{vbCrLf}({highDeviation}) " &
                        $"Needs{vbCrLf}Improvement{Superscript2}"
                    Me.HighTirComplianceLabel.ForeColor = Color.Red
            End Select
        End If
        Me.PositionControlsInPanel()
    End Sub

    ''' <summary>
    '''  Positions the controls in the panel of the home tab.
    ''' </summary>
    ''' <remarks>
    '''  This method centers the labels in the panel based on their names
    '''  and adjusts their positions accordingly.
    ''' </remarks>
    Private Sub PositionControlsInPanel()
        For Each ctrl As Control In Me.SplitContainer3.Panel2.Controls
            Dim parent As Control = ctrl.Parent
            If parent Is Nothing Then
                If Not Debugger.IsAttached Then
                    Exit Sub
                End If
                Const message As String = "The control must have a parent to center it."
                Throw New InvalidOperationException(message)
            End If
            If TypeOf ctrl Is Label Then
                Select Case ctrl.Name
                    Case NameOf(Me.LowTirComplianceLabel)
                        ctrl.CenterXOnParent(onLeftHalf:=True)

                    Case NameOf(Me.HighTirComplianceLabel)
                        ctrl.CenterXOnParent(onLeftHalf:=False)

                    Case NameOf(Me.TimeInRangeMessageLabel)
                        ctrl.CenterXOnParent(onLeftHalf:=True)

                    Case NameOf(Me.TimeInRangeValueLabel)
                        ctrl.CenterXOnParent(onLeftHalf:=True)

                    Case NameOf(Me.TiTRMgsLabel)
                        ctrl.CenterXOnParent(onLeftHalf:=False)

                    Case NameOf(Me.TimeInTightRangeValueLabel)
                        ctrl.CenterXOnParent(onLeftHalf:=False)

                    Case NameOf(Me.TiTRMgsLabel2)
                        With Me.TiTRMgsLabel2
                            .Left = parent.Width - .Width - .Margin.Right
                        End With
                    Case Else
                        ctrl.CenterXOnParent()
                End Select
            End If
        Next
    End Sub

    ''' <summary>
    '''  Updates the Treatment Markers chart with the latest data and settings.
    ''' </summary>
    ''' <remarks>
    '''  This method reinitializes the Treatment Markers chart,
    '''  updates its title, axes, and series. It plots suspend areas,
    '''  treatment markers, sensor glucose series, and high/low/target limits.
    '''  If the program is not initialized, the method exits without making changes.
    '''  Any exceptions encountered during the update are caught, the debugger is stopped,
    '''  and an <see cref="ApplicationException"/> is thrown.
    ''' </remarks>
    Private Sub UpdateTreatmentChart()
        If Not ProgramInitialized Then
            Exit Sub
        End If
        Try
            Me.InitializeTreatmentMarkersChart()
            With Me.TreatmentMarkersChart
                .Titles(name:=NameOf(TreatmentMarkersChartTitle)).Text = $"Treatment Details{s_basalList.Subtitle()}"
                .ChartAreas(name:=NameOf(ChartArea)).UpdateChartAreaSgAxisX()
                .PlotSuspendArea(SuspendSeries:=Me.TreatmentSuspendSeries)
                .PlotTreatmentMarkers(Me.TreatmentTimeChangeSeries)
                .PlotSgSeries(HomePageMealRow:=GetYMinValueFromNativeMmolL())
                .PlotHighLowLimitsAndTargetSg(targetSgOnly:=True)
            End With
        Catch innerException As Exception
            Stop
            Throw New ApplicationException(
                message:=$"{innerException.DecodeException()} exception in {NameOf(UpdateTreatmentChart)}",
                innerException)
        End Try
        Application.DoEvents()
    End Sub

    ''' <summary>
    '''  Updates the trend arrows display on the home tab.
    '''  This method updates the trend arrows label based on the
    '''  current sensor glucose trend value.
    ''' </summary>
    ''' <remarks>
    '''  The trend arrows label is set based on the last sensor glucose trend
    '''  value and is styled accordingly.
    ''' </remarks>
    Private Sub UpdateTrendArrows()
        Dim key As String =
            RecentData.GetStringValueOrEmpty(NameOf(ServerDataEnum.lastSGTrend))
        If PatientData.ConduitInRange Then
            Dim value As String = Nothing
            If s_trends.TryGetValue(key, value) Then
                Me.TrendArrowsLabel.Font = If(key = "NONE",
                                              New Font(FamilyName, emSize:=18.0F, style:=FontStyle.Bold),
                                              New Font(FamilyName, emSize:=14.25F, style:=FontStyle.Bold))

                Me.TrendArrowsLabel.Text = s_trends(key)
            Else
                Me.TrendArrowsLabel.Font = New Font(FamilyName, emSize:=14.25F, style:=FontStyle.Bold)
                Me.TrendArrowsLabel.Text = key
            End If
        End If
        Me.SgTrendLabel.Visible = PatientData.ConduitInRange
        Me.TrendValueLabel.Visible = PatientData.ConduitInRange
        Me.TrendArrowsLabel.Visible = PatientData.ConduitInRange
    End Sub

    ''' <summary>
    '''  Updates all tab pages with the latest data and settings.
    ''' </summary>
    ''' <param name="fromFile">Indicates whether the update is from a saved file.</param>
    ''' <remarks>
    '''  This method updates various components of the main form,
    '''  including data tables, charts, and labels,
    '''  based on the current patient data and system status.
    ''' </remarks>
    Friend Sub UpdateAllTabPages(fromFile As Boolean)
        If RecentDataEmpty() Then
            DebugPrint($"exiting, {NameOf(RecentData)} has no data!")
            Exit Sub
        End If
        Dim lastMedicalDeviceDataUpdateServerTimeEpoch As String = ""
        Dim key As String = NameOf(ServerDataEnum.lastMedicalDeviceDataUpdateServerTime)
        If RecentData.TryGetValue(key, value:=lastMedicalDeviceDataUpdateServerTimeEpoch) Then
            If CLng(lastMedicalDeviceDataUpdateServerTimeEpoch) =
                s_lastMedicalDeviceDataUpdateServerEpoch Then
                Exit Sub
            Else
                s_lastMedicalDeviceDataUpdateServerEpoch =
                    CLng(lastMedicalDeviceDataUpdateServerTimeEpoch)
            End If
        End If

        If RecentData.Count > ServerDataEnum.sensorLifeIcon + 1 Then
            Stop
        End If

        CheckForUpdatesAsync(reportSuccessfulResult:=False)

        SyncLock _updatingLock
            _updating = True ' prevent paint
            Me.MenuStartHere.Enabled = False
            If fromFile Then
                Me.LoginStatus.Text = "Login Status: N/A From Saved File"
            Else
                Me.SetLastUpdateTime(
                    msg:=$"Last Update Time: {PumpNow()}",
                    suffixMessage:="",
                    highLight:=False,
                    isDaylightSavingTime:=PumpNow.IsDaylightSavingTime)
            End If
            Me.CursorPanel.Visible = False

            Me.Cursor = Cursors.WaitCursor
            Application.DoEvents()
            UpdateDataTables(mainForm:=Me)
            Application.DoEvents()
            Me.Cursor = Cursors.Default
            _updating = False
        End SyncLock

        FinishInitialization(mainForm:=Me)
        Me.UpdateTrendArrows()
        UpdateSummaryTab(dgv:=Me.DgvSummary, classCollection:=s_listOfSummaryRecords, sort:=True)
        Me.UpdateActiveInsulin()
        Me.UpdateAutoModeShield()
        Me.UpdateCalibrationTimeRemaining()
        Me.UpdateInsulinLevel()
        Me.UpdatePumpBattery()
        Me.UpdateSensorLife()
        UpdateSensorData()
        Me.UpdateTimeInRange()
        Me.UpdateAllSummarySeries()
        Me.UpdateDosingAndCarbs()

        key = NameOf(ServerDataEnum.lastName)
        Me.FullNameLabel.Text =
            $"{PatientData.FirstName} {RecentData.GetStringValueOrEmpty(key)}"

        Dim mdi As MedicalDeviceInformation = PatientData.MedicalDeviceInformation
        Me.ModelLabel.Text = $"{mdi.ModelNumber} HW Version = {mdi.HardwareRevision}"
        Me.PumpNameLabel.Text = GetPumpName(mdi.ModelNumber)

        Me.ReadingsLabel.Text = $"{GetValidSgRecords().Count()}/{288} SG Readings"

        Me.TlpLastSG.DisplayDataTableInDGV(
            table:=ClassCollectionToDataTable(classCollection:={s_lastSg}.ToList),
            className:=NameOf(LastSG), rowIndex:=ServerDataEnum.lastSG,
            hideRecordNumberColumn:=True)

        UpdateSummaryTab(
            dgv:=Me.DgvLastAlarm,
            classCollection:=GetSummaryRecords(jsonDictionary:=s_lastAlarmValue),
            sort:=True)
        Me.DgvLastAlarm.Columns(index:=0).Visible = False

        Me.TlpActiveInsulin.DisplayDataTableInDGV(
            table:=ClassCollectionToDataTable(classCollection:={s_activeInsulin}.ToList),
            className:=NameOf(ActiveInsulin), rowIndex:=ServerDataEnum.activeInsulin,
            hideRecordNumberColumn:=True)

        Dim keySelector As Func(Of SG, Integer) =
            Function(x As SG) As Integer
                Return x.RecordNumber
            End Function
        Dim classCollection As List(Of SG) =
            s_sgRecords.OrderByDescending(keySelector).ToList()
        Me.TlpSgs.DisplayDataTableInDGV(
            table:=ClassCollectionToDataTable(classCollection),
            dgv:=Me.DgvSGs,
            rowIndex:=ServerDataEnum.sgs)
        Me.DgvSGs.AutoSize = True
        Me.DgvSGs.Columns(index:=0).HeaderCell.SortGlyphDirection = SortOrder.Descending

        Me.TlpLimits.DisplayDataTableInDGV(
            table:=ClassCollectionToDataTable(classCollection:=s_limitRecords),
            className:=NameOf(Limit), rowIndex:=ServerDataEnum.limits)

        UpdateSummaryTab(
            dgv:=Me.DgvTherapyAlgorithmState,
            classCollection:=GetSummaryRecords(jsonDictionary:=s_therapyAlgorithmStateValue),
            sort:=False)
        Me.DgvTherapyAlgorithmState.Columns(index:=0).Visible = False

        Me.DgvLastAlarm.Columns(index:=0).Visible = False
        Me.TlpBasal.DisplayDataTableInDGV(
            table:=ClassCollectionToDataTable(s_basalList.ClassCollection),
            className:=NameOf(Basal), rowIndex:=ServerDataEnum.basal,
            hideRecordNumberColumn:=True)

        UpdateMarkerTabs(mainForm:=Me)
        UpdateNotificationTabs(mainForm:=Me)
        UpdatePumpBannerStateTab(mainForm:=Me)

        Me.MenuStartHere.Enabled = True
        ProgramInitialized = True
        Me.UpdateTreatmentChart()
        Me.UpdateActiveInsulinChart()

        Me.ShowHideLegends()

        If My.Settings.SystemAudioAlertsEnabled AndAlso
           My.Settings.SystemSpeechRecognitionThreshold <> 1 Then
            InitializeSpeechRecognition()
        Else
            CancelSpeechRecognition()
        End If
        Application.DoEvents()
    End Sub

#End Region ' Update Home Tab

End Class
