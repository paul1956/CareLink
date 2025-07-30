' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Configuration
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices
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
    Private ReadOnly _sgMiniDisplay As New SgMiniForm(Me)
    Private ReadOnly _updatingLock As New Object()

    Private _activeInsulinChartAbsoluteRectangle As RectangleF = RectangleF.Empty
    Private _dgvSummaryPrevRowIndex As Integer = -1
    Private _dgvSummaryPrevColIndex As Integer = -1
    Private _formScale As New SizeF(width:=1.0F, height:=1.0F)
    Private _inMouseMove As Boolean = False
    Private _lastMarkerTabLocation As (page As Integer, tab As Integer) = (0, 0)
    Private _lastSummaryTabIndex As Integer = 0
    Private _previousLoc As Point
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
    '''  Enables dark mode for the form and its controls.
    '''  Sets the window attributes for immersive dark mode and border color.
    ''' </summary>
    ''' <remarks>
    '''  This method uses the DWM API to enable dark mode and set the border color.
    ''' </remarks>
    Private Sub EnableDarkMode()
        ' Enable immersive dark mode
        Dim useDarkMode As Integer = 1
        Dim result As Integer = DwmSetWindowAttribute(
            hwnd:=Me.Handle,
            attr:=DWMWA_USE_IMMERSIVE_DARK_MODE,
            attrValue:=useDarkMode,
            attrSize:=Marshal.SizeOf([structure]:=useDarkMode))

        If result <> 0 Then
            ' Handle error if dark mode could not be enabled
            MessageBox.Show(
                text:="Failed to enable dark mode.",
                caption:="Error",
                buttons:=MessageBoxButtons.OK,
                icon:=MessageBoxIcon.Error)
            Return
        End If
        ' Set border color (BGR format, e.g., &H202020 for dark gray)
        Dim borderColor As Integer = &H202020
        result = DwmSetWindowAttribute(
            hwnd:=Me.Handle,
            attr:=DWMWA_BORDER_COLOR,
            attrValue:=borderColor,
            attrSize:=Marshal.SizeOf([structure]:=borderColor))
        If result <> 0 Then
            MessageBox.Show(
                text:="Failed to set border color.",
                caption:="Error",
                buttons:=MessageBoxButtons.OK,
                icon:=MessageBoxIcon.Error)
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Form.HandleCreated"/> event.
    '''  Enables dark mode for the form and its controls.
    ''' </summary>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        Me.EnableDarkMode()
    End Sub

    ''' <summary>
    '''  Scales the control based on the <paramref name="factor"/> and <paramref name="specified"/> bounds.
    '''  This method overrides the base method to adjust the form scale and fix SplitContainer controls.
    ''' </summary>
    ''' <param name="factor">The scaling factor.</param>
    ''' <param name="specified">The bounds specified for scaling.</param>
    Protected Overrides Sub ScaleControl(factor As SizeF, specified As BoundsSpecified)
        _formScale = New SizeF(width:=_formScale.Width * factor.Width, height:=_formScale.Height * factor.Height)
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
                        Me.PowerModeChanged(sender:=Nothing, New PowerModeChangedEventArgs(PowerModes.Suspend))

                        'value passed when system is resumed after suspension.
                    Case PBT_APMRESUMESUSPEND
                        Me.PowerModeChanged(sender:=Nothing, New PowerModeChangedEventArgs(PowerModes.Resume))

                    'value passed when system Suspend Failed
                    Case PBT_APMQUERYSUSPENDFAILED
                        Me.PowerModeChanged(sender:=Nothing, New PowerModeChangedEventArgs(PowerModes.Resume))

                    'value passed when system is suspended
                    Case PBT_APMSUSPEND
                        Me.PowerModeChanged(sender:=Nothing, New PowerModeChangedEventArgs(PowerModes.Suspend))

                    'value passed when system is in standby
                    Case PBT_APMSTANDBY
                        Me.PowerModeChanged(sender:=Nothing, New PowerModeChangedEventArgs(PowerModes.Suspend))

                        'value passed when system resumes from standby
                    Case PBT_APMRESUMESTANDBY
                        Me.PowerModeChanged(sender:=Nothing, New PowerModeChangedEventArgs(PowerModes.Resume))

                        'value passed when system resumes from suspend
                    Case PBT_APMRESUMESUSPEND
                        Me.PowerModeChanged(sender:=Nothing, New PowerModeChangedEventArgs(PowerModes.Resume))

                    'value passed when system is resumed automatically
                    Case PBT_APMRESUMEAUTOMATIC
                        Me.PowerModeChanged(sender:=Nothing, New PowerModeChangedEventArgs(PowerModes.Resume))

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

    Private WithEvents ActiveInsulinChartTitle As Title
    Private WithEvents TreatmentMarkersChartTitle As Title

#End Region ' Titles

#End Region ' Chart Objects

#Region "Events"

#Region "Chart Events"

    ''' <summary>
    '''  Handles the <see cref="Chart.CursorPositionChanging"/> event for
    '''  the <see cref="ActiveInsulinChart"/> and <see cref="SummaryChart"/>.
    '''  Starts the cursor timer when the cursor position is changing, if the program is initialized.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="Chart"/> control.</param>
    ''' <param name="e">A <see cref="CursorEventArgs"/> that contains the event data.</param>
    Private Sub Chart_CursorPositionChanging(sender As Object, e As CursorEventArgs) Handles _
        ActiveInsulinChart.CursorPositionChanging,
        SummaryChart.CursorPositionChanging

        If Not ProgramInitialized Then
            Exit Sub
        End If

        Me.CursorTimer.Interval = ThirtySecondInMilliseconds
        Me.CursorTimer.Start()
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Chart.MouseLeave"/> event for
    '''  the <see cref="ActiveInsulinChart"/>, <see cref="SummaryChart"/>, and <see cref="TreatmentMarkersChart"/>.
    '''  Hides the callout annotation when the mouse leaves the chart area.
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
    '''  the <see cref="ActiveInsulinChart"/>, <see cref="SummaryChart"/>, and <see cref="TreatmentMarkersChart"/>.
    '''  Displays context-sensitive information in a panel or callout when the mouse moves over a data point.
    '''  Shows details such as marker type, value, and time, or sensor glucose information, depending on the series.
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
        If e.Button <> MouseButtons.None OrElse e.Clicks > 0 OrElse e.Location = _previousLoc Then
            Return
        End If
        _inMouseMove = True
        _previousLoc = e.Location
        Dim yInPixels As Double
        Dim chart1 As Chart = CType(sender, Chart)
        Dim isHomePage As Boolean = chart1.Name = NameOf(SummaryChart)
        Try
            yInPixels = chart1.ChartAreas(name:=NameOf(ChartArea)).AxisY2.ValueToPixelPosition(axisValue:=e.Y)
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

            Dim annotationText As String
            Select Case result.Series.Name
                Case HighLimitSeriesName, LowLimitSeriesName, TargetSgSeriesName
                    Me.CursorPanel.Visible = False
                Case MarkerSeriesName, BasalSeriesName
                    Dim markerTag() As String = currentDataPoint.Tag.ToString.Split(separator:=":"c)
                    If markerTag.Length <= 1 Then
                        If chart1.Name = NameOf(TreatmentMarkersChart) Then
                            Dim callout As CalloutAnnotation = chart1.FindAnnotation(lastDataPoint:=currentDataPoint)
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
                        Me.CursorMessage2Label.Font = New Font(FamilyName, emSize:=12.0F, style:=FontStyle.Bold)
                        Select Case markerTag.Length
                            Case 2
                                Me.CursorMessage1Label.Text = markerTag(0)
                                Me.CursorMessage1Label.Visible = True
                                Me.CursorMessage2Label.Text = markerTag(1).Trim
                                Me.CursorMessage2Label.Visible = True
                                Me.CursorMessage3Label.Text =
                                    Date.FromOADate(currentDataPoint.XValue).ToString(format:=s_timeWithMinuteFormat)
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
                                        Dim style As FontStyle = FontStyle.Bold
                                        Me.CursorMessage2Label.Font = New Font(FamilyName, emSize:=11.0F, style)
                                    Case Else
                                        Stop
                                End Select
                                Me.CursorMessage1Label.Text =
                                    $"{markerTag(0)}@{xValue.ToString(format:=s_timeWithMinuteFormat)}"
                                Me.CursorMessage1Label.Visible = True
                                Me.CursorMessage2Label.Text = markerTag(1).Replace(
                                    oldValue:="Calibration not",
                                    newValue:="Cal. not").Trim
                                Me.CursorMessage2Label.Visible = True
                                Dim sgValue As Single = markerTag(2).Trim.Split(separator:=" ")(0).ParseSingle(digits:=2)
                                Me.CursorMessage3Label.Text = markerTag(2).Trim
                                Me.CursorMessage3Label.Visible = True
                                Me.CursorMessage4Label.Text = If(NativeMmolL,
                                                                 $"{CInt(sgValue * MmolLUnitsDivisor)} mg/dL",
                                                                 $"{sgValue / MmolLUnitsDivisor:F1} mmol/L")
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
                    Me.CursorMessage2Label.Text = $"{currentDataPoint.YValues(0).RoundToSingle(digits:=3)} {GetBgUnitsString()}"
                    Me.CursorMessage2Label.Visible = True
                    Me.CursorMessage3Label.Text = If(NativeMmolL,
                                                     $"{CInt(currentDataPoint.YValues(0) * MmolLUnitsDivisor)} mg/dL",
                                                     $"{currentDataPoint.YValues(0) / MmolLUnitsDivisor:F1} mmol/L")
                    Me.CursorMessage3Label.Visible = True
                    Me.CursorMessage4Label.Text = Date.FromOADate(currentDataPoint.XValue).ToString(format:=s_timeWithMinuteFormat)
                    Me.CursorMessage4Label.Visible = True
                    Me.CursorPictureBox.Image = Nothing
                    Me.CursorPanel.Visible = True
                    annotationText = $"Sensor Glucose {Me.CursorMessage2Label.Text}"
                    chart1.SetupCallout(currentDataPoint, annotationText)
                Case SuspendSeriesName, TimeChangeSeriesName
                    Me.CursorPanel.Visible = False
                Case ActiveInsulinSeriesName
                    annotationText = $"Theoretical Active Insulin {currentDataPoint.YValues.FirstOrDefault:F3} U"
                    chart1.SetupCallout(currentDataPoint, annotationText)
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
    '''  Handles the <see cref="Chart.PostPaint"/> event for the <see cref="ActiveInsulinChart"/>.
    '''  Draws additional graphics or overlays after the chart is painted, such as insulin and meal markers.
    '''  Skips painting if a mouse move is in progress, the chart is updating, or the program is not initialized.
    ''' </summary>
    ''' <param name="sender">The source chart control.</param>
    ''' <param name="e">A <see cref="ChartPaintEventArgs"/> containing event data.</param>
    <DebuggerNonUserCode()>
    Private Sub ActiveInsulinChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles _
        ActiveInsulinChart.PostPaint

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
                insulinDictionary:=s_activeInsulinMarkerInsulinDictionary,
                mealDictionary:=Nothing,
                offsetInsulinImage:=True,
                paintOnY2:=True)
        End SyncLock
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Chart.PostPaint"/> event for the <see cref="SummaryChart"/>.
    '''  Draws overlays such as insulin and meal markers after the summary chart is painted.
    '''  Skips painting if a mouse move is in progress, the chart is updating, or the program is not initialized.
    ''' </summary>
    ''' <param name="sender">The source chart control.</param>
    ''' <param name="e">A <see cref="ChartPaintEventArgs"/> containing event data.</param>
    <DebuggerNonUserCode()>
    Private Sub SummaryChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles SummaryChart.PostPaint
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
                insulinDictionary:=s_summaryMarkerInsulinDictionary,
                mealDictionary:=s_summaryMarkerMealDictionary,
                offsetInsulinImage:=True,
                paintOnY2:=True)
        End SyncLock
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Chart.PostPaint"/> event for the <see cref="TreatmentMarkersChart"/>.
    '''  Draws overlays such as insulin and meal markers after the treatment markers chart is painted.
    '''  Skips painting if a mouse move is in progress, the chart is updating, or the program is not initialized.
    ''' </summary>
    ''' <param name="sender">The source chart control.</param>
    ''' <param name="e">A <see cref="ChartPaintEventArgs"/> containing event data.</param>
    <DebuggerNonUserCode()>
    Private Sub TreatmentMarkersChart_PostPaint(sender As Object, e As ChartPaintEventArgs) Handles _
        TreatmentMarkersChart.PostPaint

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
                insulinDictionary:=s_treatmentMarkerInsulinDictionary,
                mealDictionary:=s_treatmentMarkerMealDictionary,
                offsetInsulinImage:=False,
                paintOnY2:=False)
        End SyncLock
    End Sub

#End Region ' Post Paint Events

#End Region ' Chart Events

#Region "DGV Events"

#Region "DGV Global Event Helper"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellContextMenuStripNeeded"/> event for multiple <see cref="DataGridView"/>.
    '''  Assigns the context menu for copying data if the row index is valid.
    ''' </summary>
    ''' <param name="sender">The DataGridView control.</param>
    ''' <param name="e">A <see cref="DataGridViewCellContextMenuStripNeededEventArgs"/> containing event data.</param>
    ''' <remarks>
    '''  Sets the context menu for copying data.
    ''' </remarks>
    Public Sub DgvCellContextMenuStripNeededWithExcel(sender As Object, e As DataGridViewCellContextMenuStripNeededEventArgs) Handles _
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
    '''  Handles the <see cref="DataGridView.CellFormatting"/> event for multiple <see cref="DataGridView"/>.
    '''  Formats the cell values based on their type and applies specific styles.
    ''' </summary>
    ''' <param name="sender">The DataGridView control.</param>
    ''' <param name="e">A <see cref="DataGridViewCellFormattingEventArgs"/> containing event data.</param>
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
                            Dim textColor As Color = GetGraphLineColor(legendText:="Auto Correction")
                            dgv.CellFormattingApplyBoldColor(e, textColor, isUri:=False)
                        Case "FAST", "RECOMMENDED", "UNDETERMINED"
                            dgv.CellFormattingToTitle(e)
                        Case Else
                            dgv.CellFormattingSetForegroundColor(e)
                    End Select
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                Case "Amount"
                    Select Case dgv.Name
                        Case NameOf(DgvActiveInsulin)
                            e.Value = $"{dgv.CellFormattingSingleValue(e, digits:=3)} U"
                        Case NameOf(DgvMeal)
                            dgv.CellFormattingInteger(e, message:=GetCarbDefaultUnit)
                    End Select
                Case NameOf(BasalPerHour.BasalRate), NameOf(BasalPerHour.BasalRate2)
                    If dgv.Name = NameOf(DgvBasalPerHour) Then
                        e.Value = $"{dgv.CellFormattingSingleValue(e, digits:=3)} U/h"
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
                        dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Red, isUri:=False)
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
                Case NameOf(Limit.HighLimit), NameOf(Limit.HighLimitMgdL), NameOf(Limit.HighLimitMmolL)
                    dgv.CellFormattingSgValue(e, partialKey:=NameOf(Limit.HighLimit))
                Case NameOf(Limit.LowLimit), NameOf(Limit.lowLimitMgdL), NameOf(Limit.lowLimitMmolL)
                    dgv.CellFormattingSgValue(e, partialKey:=NameOf(Limit.LowLimit))
                Case NameOf(InsulinPerHour.Hour), NameOf(InsulinPerHour.Hour2)
                    Dim hour As Integer = TimeSpan.FromHours(CInt(e.Value)).Hours
                    Dim time As New DateTime(year:=1, month:=1, day:=1, hour, minute:=0, second:=0)
                    e.Value = time.ToString(s_timeWithoutMinuteFormat)
                    e.CellStyle.Font = New Font(FamilyName, emSize:=12.0F, style:=FontStyle.Regular)
                Case NameOf(BannerState.Message)
                    Select Case dgv.Name
                        Case NameOf(DgvPumpBannerState)
                            dgv.CellFormattingToTitle(e)
                        Case NameOf(DgvSGs)
                            e.Value = Convert.ToString(e.Value).Replace(oldValue:=vbCrLf, newValue:=" ")
                            dgv.CellFormattingSetForegroundColor(e)
                        Case Else
                            e.Value = Convert.ToString(e.Value).Replace(oldValue:=vbCrLf, newValue:=" ")
                            dgv.CellFormattingToTitle(e)
                    End Select
                Case NameOf(ActiveInsulin.Precision)
                    dgv.CellFormattingToTitle(e)
                Case NameOf(Insulin.SafeMealReduction)
                    If dgv.CellFormattingSingleValue(e, digits:=3) >= 0.0025 Then
                        dgv.CellFormattingApplyBoldColor(e, textColor:=Color.OrangeRed, isUri:=False)
                    Else
                        e.Value = ""
                        dgv.CellFormattingSetForegroundColor(e)
                    End If
                Case NameOf(SG.SensorState)
                    If Equals(e.Value, "NO_ERROR_MESSAGE") Then
                        dgv.CellFormattingToTitle(e)
                    Else
                        dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Red, isUri:=False)
                        dgv.CellFormattingToTitle(e)
                    End If
                Case NameOf(SG.sg), NameOf(SG.sgMmolL), NameOf(SG.sgMgdL)
                    dgv.CellFormattingSgValue(e, partialKey:=NameOf(SG.sg))
                Case NameOf(BannerState.TimeRemaining)
                    CellFormatting0Value(e)
                Case NameOf(SG.Timestamp)
                    dgv.CellFormattingDateTime(e)
                Case NameOf(SG.sg), NameOf(SG.sgMmolL), NameOf(SG.sgMgdL)
                    dgv.CellFormattingSgValue(e, partialKey:=NameOf(SG.sg))
                Case NameOf(Calibration.UnitValue), NameOf(Calibration.UnitValueMgdL), NameOf(Calibration.UnitValueMmolL)
                    dgv.CellFormattingSgValue(e, partialKey:=NameOf(Calibration.UnitValue))
                    e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    dgv.CellFormattingSetForegroundColor(e)
                Case Else
                    If dgv.Columns(index:=e.ColumnIndex).ValueType = GetType(Single) Then
                        dgv.CellFormattingSingleValue(e, digits:=3)
                    ElseIf dgv.Columns(index:=e.ColumnIndex).ValueType = GetType(String) AndAlso dgv.Name <> NameOf(DgvLastAlarm) Then
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
    '''  Handles the <see cref="DataGridView.DataError"/> event for all DataGridViews in the form.
    '''  This event is raised when an exception occurs during data operations such as
    '''  formatting, parsing, or committing a cell value.
    '''  The handler currently stops execution for debugging purposes.
    ''' </summary>
    ''' <param name="sender">
    '''  The source of the event, a <see cref="DataGridView"/> control.
    ''' </param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewDataErrorEventArgs"/> that contains the event data, including the exception and context.
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
        Dim dgvName As String = dgv.Name.Remove(oldValue:="dgv")
        Dim text As String = $"An error {e.Exception.Message} occurred while processing {dgvName}. " &
            $"Please check your data and try again."
        MessageBox.Show(text, caption:="Input Error", buttons:=MessageBoxButtons.OK, icon:=MessageBoxIcon.Warning)

        Debug.WriteLine($"Context: {e.Context}")

        ' Prevent the exception from being thrown again
        e.ThrowException = False
        Stop
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.DataBindingComplete"/> event.
    '''  This event is raised when the data binding operation is complete.
    '''  It clears the selection of all DataGridViews to ensure no cells are selected after data binding.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">The <see cref="DataGridViewBindingCompleteEventArgs"/> containing the event data.</param>
    ''' <remarks>
    '''  This event is used to customize the appearance of DataGridViews after data binding is complete.
    ''' </remarks>
    Public Sub Dgv_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles _
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
                        dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False
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
                                If result.StartsWithIgnoreCase(value:="Timestamp") Then
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

            ' Restore the previous selection in the Summary DataGridView if its not empty or Row(0).Cell(0).
            dgv.CurrentCell = dgv.Rows(index:=_dgvSummaryPrevRowIndex).Cells(index:=_dgvSummaryPrevColIndex)
            dgv.Rows(index:=_dgvSummaryPrevRowIndex).Selected = True
            dgv.FirstDisplayedScrollingRowIndex = _dgvSummaryPrevRowIndex
        Else
            ' Clear the selection of all DataGridViews except Summary DataGridView.
            dgv.ClearSelection()
        End If
    End Sub

#Region "ContextMenuStrip Menu Events"

    Private WithEvents DgvCopyWithExcelMenuStrip As New ContextMenuStrip
    Friend WithEvents DgvCopyWithoutExcelMenuStrip As New ContextMenuStrip

    ''' <summary>
    '''  Handles the <see cref="DgvCopyWithExcelMenuStrip.Opening"/> event for the copy-with-Excel context menu.
    '''  Populates the context menu with options to copy data with or without headers, or save to Excel.
    ''' </summary>
    ''' <param name="sender">The context menu strip.</param>
    ''' <param name="e">A <see cref="CancelEventArgs"/> containing event data.</param>
    Private Sub DgvCopyWithExcelMenuStrip_Opening(sender As Object, e As CancelEventArgs) Handles _
        DgvCopyWithExcelMenuStrip.Opening

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
        ' It is optimized to true based on empty entry.
        e.Cancel = False
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DgvCopyWithExcelMenuStrip.Opening"/> event for the copy-without-Excel context menu.
    '''  Populates the context menu with options to copy selected cells with or without headers.
    ''' </summary>
    ''' <param name="sender">The context menu strip.</param>
    ''' <param name="e">A <see cref="CancelEventArgs"/> containing event data.</param>
    Private Sub DgvCopyWithoutExcelMenuStrip_Opening(sender As Object, e As CancelEventArgs) Handles _
        DgvCopyWithoutExcelMenuStrip.Opening

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
        ' It is optimized to true based on empty entry.
        e.Cancel = False
    End Sub

#End Region 'ContextMenuStrip Events

#End Region ' DGV Global Event Helper

#Region "Dgv Active Insulin Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for the <see cref="DgvActiveInsulin"/> DataGridView.
    '''  This event is raised when a new column is added to the <see cref="DataGridView"/>.
    '''  It sets the properties of the newly added column, such as sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e"> A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.
    Private Sub DgvActiveInsulin_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles _
        DgvActiveInsulin.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            .Visible = Not DataGridViewHelpers.HideColumn(Of ActiveInsulin)(dataPropertyName:= .Name)
            e.DgvColumnAdded(
                cellStyle:=DataGridViewHelpers.GetCellStyle(Of ActiveInsulin)(columnName:= .Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Active Insulin Events

#Region "Dgv Auto Basal Delivery (Basal) Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for the <see cref="DgvAutoBasalDelivery"/>.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    Private Sub DgvAutoBasalDelivery_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles _
        DgvAutoBasalDelivery.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If DataGridViewHelpers.HideColumn(Of AutoBasalDelivery)(dataPropertyName:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=DataGridViewHelpers.GetCellStyle(Of AutoBasalDelivery)(columnName:= .Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Auto Basal Delivery (Basal) Events

#Region "Dgv AutoMode Status Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for the <see cref="DgvAutoModeStatus"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    Private Sub DgvAutoModeStatus_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles _
        DgvAutoModeStatus.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If DataGridViewHelpers.HideColumn(Of AutoModeStatus)(dataPropertyName:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=DataGridViewHelpers.GetCellStyle(Of AutoModeStatus)(columnName:= .Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv AutoMode Status Events

#Region "Dgv Pump Banner State Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for the <see cref="DgvPumpBannerState"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    Private Sub DgvBannerState_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles _
        DgvPumpBannerState.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If DataGridViewHelpers.HideColumn(Of BannerState)(dataPropertyName:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=DataGridViewHelpers.GetCellStyle(Of BannerState)(columnName:= .Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Banner State Events

#Region "Dgv Basal Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for the <see cref="DgvBasal"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    Private Sub DgvBasal_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles _
        DgvBasal.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If DataGridViewHelpers.HideColumn(Of Basal)(dataPropertyName:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=DataGridViewHelpers.GetCellStyle(Of Basal)(columnName:= .Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Basal Events

#Region "Dgv Basal Per Hour Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for the <see cref="DgvBasalPerHour"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    Friend Sub DgvBasalPerHour_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles _
        DgvBasalPerHour.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            e.DgvColumnAdded(
                cellStyle:=DataGridViewHelpers.GetCellStyle(Of InsulinPerHour)(columnName:= .Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Basal Per Hour Events

#Region "Dgv Calibration Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for the <see cref="DgvCalibration"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the column properties such as SortMode, visibility, cell style, and caption.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    Private Sub DgvCalibration_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles _
        DgvCalibration.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If DataGridViewHelpers.HideColumn(Of Calibration)(dataPropertyName:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=DataGridViewHelpers.GetCellStyle(Of Calibration)(columnName:= .Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv CalibrationHelpers Events

#Region "Dgv CareLink Users Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellBeginEdit"/> event for the <see cref="DgvCareLinkUsers"/> DataGridView.
    '''  Saves the current value of the cell being edited to the DataGridView's <see cref="DataGridView.Tag"/> property.
    '''  This allows comparison with the new value after editing is complete, for change detection or validation.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewCellCancelEventArgs"/> that contains the event data.</param>
    Private Sub DgvCareLinkUsers_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles _
        DgvCareLinkUsers.CellBeginEdit

        Dim dgv As DataGridView = CType(sender, DataGridView)
        'Here we save a current value of cell to some variable, that later we can compare with a new value
        'For example using of dgv.Tag property
        If e.RowIndex >= 0 AndAlso e.ColumnIndex > 0 Then
            dgv.Tag = dgv.CurrentCell.Value.ToString
        End If

    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellContentClick"/> event for the <see cref="DgvCareLinkUsers"/> DataGridView.
    '''  This event is raised when a cell's content is clicked, specifically for delete button cells.
    '''  It removes the corresponding user record from the data source and updates the DataGridView.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewCellEventArgs"/> that contains the event data.</param>
    Private Sub DgvCareLinkUsers_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles _
        DgvCareLinkUsers.CellContentClick

        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim dataGridViewDisableButtonCell As DataGridViewDisableButtonCell =
            TryCast(dgv.Rows(index:=e.RowIndex).Cells(index:=e.ColumnIndex), DataGridViewDisableButtonCell)
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
    '''  Handles the <see cref="DataGridView.CellEndEdit"/> event for the <see cref="DgvCareLinkUsers"/> DataGridView.
    '''  This event is raised after a cell edit is completed.
    '''  Intended for post-edit logic, such as validation or saving changes.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewCellEventArgs"/> that contains the event data.</param>
    Private Sub DgvCareLinkUsers_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles _
        DgvCareLinkUsers.CellEndEdit

        Try
            Dim dgv As DataGridView = CType(sender, DataGridView)
            If e.RowIndex < 0 OrElse e.ColumnIndex <= 0 Then
                Exit Sub
            End If
            Dim currentValue As String = dgv.CurrentCell.Value.ToString
            If currentValue = dgv.Tag.ToString Then
                Exit Sub ' No change, exit early
            End If
            Dim userRecord As CareLinkUserDataRecord = s_allUserSettingsData(index:=e.RowIndex)
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
    '''  Handles the <see cref="DataGridView.CellValidating"/> event for the <see cref="DgvCareLinkUsers"/> DataGridView.
    '''  Used to validate cell values before committing changes.
    '''  Currently skips validation for the first column (index 0).
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewCellValidatingEventArgs"/> that contains the event data.</param>
    Private Sub DgvCareLinkUsers_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles _
        DgvCareLinkUsers.CellValidating
        If e.ColumnIndex = 0 Then
            Exit Sub
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for the <see cref="DgvCareLinkUsers"/> DataGridView.
    '''  Configures new columns, including sort mode, visibility, cell style, and caption.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    Private Sub DgvCareLinkUsers_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles _
        DgvCareLinkUsers.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            Dim value As String = dgv.Columns(.Index).HeaderText
            If String.IsNullOrWhiteSpace(value) Then
                value = .DataPropertyName.Remove(oldValue:="DgvCareLinkUsers")
            End If
            If value.ContainsIgnoreCase(value:="DeleteRow") Then
                value = ""
            Else
                If .Index > 0 AndAlso
                    String.IsNullOrWhiteSpace(value:= .DataPropertyName) AndAlso
                    String.IsNullOrWhiteSpace(value) Then

                    .DataPropertyName = s_headerColumns(index:= .Index - 2)
                End If
            End If
            Dim forceReadOnly As Boolean
            If DataGridViewHelpers.HideColumn(Of CareLinkUserDataRecord)(.DataPropertyName) Then
                .Visible = False
            Else
                forceReadOnly = True
            End If
            e.DgvColumnAdded(
                cellStyle:=CareLinkUserDataRecordHelpers.GetCellStyle(
                    columnName:= .DataPropertyName),
                    forceReadOnly,
                    caption:=value)
        End With
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.RowsAdded"/> event for the <see cref="DgvCareLinkUsers"/> DataGridView.
    '''  Enables or disables the delete button cell based on whether the row belongs to the logged-on user.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewRowsAddedEventArgs"/> that contains the event data.</param>
    Private Sub DgvCareLinkUsers_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles _
        DgvCareLinkUsers.RowsAdded

        If s_allUserSettingsData.Count = 0 Then Exit Sub
        Dim dgv As DataGridView = CType(sender, DataGridView)
        For i As Integer = e.RowIndex To e.RowIndex + (e.RowCount - 1)
            Const columnName As String = "DgvCareLinkUsersDeleteRow"
            Dim disableButtonCell As DataGridViewDisableButtonCell =
                CType(dgv.Rows(index:=i).Cells(columnName), DataGridViewDisableButtonCell)

            Dim careLinkUserName As String = LoginHelpers.LoginDialog.LoggedOnUser.CareLinkUserName
            disableButtonCell.Enabled = s_allUserSettingsData(index:=i).CareLinkUserName <> careLinkUserName
        Next
    End Sub

    ''' <summary>
    '''  Initializes the <see cref="DgvCareLinkUsers"/> DataGridView with columns and sets its data source.
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

#End Region ' Dgv CareLink Users Events

#Region "Dgv Current User Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/>  event for <see cref="DgvCurrentUser"/> DataGridView.
    '''  This event is raised when the data binding operation is complete.
    '''  It clears the selection of all DataGridViews to ensure no cells are selected after data binding.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    Private Sub DgvCurrentUser_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles _
        DgvCurrentUser.ColumnAdded

        e.Column.SortMode = DataGridViewColumnSortMode.NotSortable
        Dim alignment As DataGridViewContentAlignment = DataGridViewContentAlignment.MiddleLeft
        e.DgvColumnAdded(
            cellStyle:=New DataGridViewCellStyle().SetCellStyle(alignment, padding:=New Padding(all:=1)),
            forceReadOnly:=True,
            caption:=Nothing)
    End Sub

#End Region ' Dgv Current User Events

#Region "Dgv Insulin Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for the <see cref="DgvInsulin"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    Private Sub DgvInsulin_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles _
        DgvInsulin.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            If DataGridViewHelpers.HideColumn(Of Insulin)(dataPropertyName:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=DataGridViewHelpers.GetCellStyle(Of Insulin)(columnName:= .Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

    ''' <summary>
    '''  Handles the DataGridView's DataBindingComplete event.
    '''  This event is raised when the data binding operation is complete.
    '''  It clears the selection of all DataGridViews to ensure no cells are selected after data binding.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">
    '''  The DataGridViewBindingCompleteEventArgs containing the event data.
    ''' </param>
    ''' <remarks>
    '''  This event is used to customize the appearance of DataGridViews after data binding is complete.
    ''' </remarks>
    Public Sub DgvInsulinDataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles _
        DgvInsulin.DataBindingComplete

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
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for the <see cref="DgvLastAlarm"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    Friend Sub DgvLastAlarm_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles _
        DgvLastAlarm.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If DataGridViewHelpers.HideColumn(Of LastAlarm)(dataPropertyName:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=DataGridViewHelpers.GetCellStyle(Of LastAlarm)(columnName:= .Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Last Alarm Events

#Region "Dgv Limits Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for the <see cref="DgvLimits"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    Private Sub DataGridView_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles _
        DgvLimits.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If DataGridViewHelpers.HideColumn(Of Limit)(dataPropertyName:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=DataGridViewHelpers.GetCellStyle(Of Limit)(columnName:= .Name),
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
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    Friend Sub DgvLowGlucoseSuspended_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles _
        DgvLowGlucoseSuspended.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If DataGridViewHelpers.HideColumn(Of LowGlucoseSuspended)(dataPropertyName:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=DataGridViewHelpers.GetCellStyle(Of LowGlucoseSuspended)(columnName:= .Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Low Glucose Suspended Events

#Region "Dgv Meal Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for the <see cref="DgvMeal"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    Private Sub DgvMeal_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles DgvMeal.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If DataGridViewHelpers.HideColumn(Of Meal)(dataPropertyName:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=DataGridViewHelpers.GetCellStyle(Of Meal)(columnName:= .Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Meal Events

#Region "Dgv Sensor Bg Readings Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for the <see cref="DgvSensorBgReadings"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    Friend Sub DgvSensorBgReadings_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles _
        DgvSensorBgReadings.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If DataGridViewHelpers.HideColumn(Of BgReading)(dataPropertyName:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=DataGridViewHelpers.GetCellStyle(Of BgReading)(columnName:= .Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Sensor Bg Readings Events

#Region "Dgv SGs Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellPainting"/> event for the <see cref="DgvSGs"/> DataGridView.
    '''  This event is raised when a cell is painted, allowing custom rendering of the cell's content.
    '''  Specifically, it draws a custom sort glyph in the header cells of sortable columns.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewCellPaintingEventArgs"/> that contains the event data.
    Private Sub DgvSGs_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles _
        DgvSGs.CellPainting

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
                    Dim points() As Point = If(glyphDir = SortOrder.Ascending,
                                               {New Point(x, y:=y + 6),
                                                New Point(x:=x + 8, y:=y + 6),
                                                New Point(x:=x + 4, y)},
                                               {New Point(x, y),
                                                New Point(x:=x + 8, y),
                                                New Point(x:=x + 4, y:=y + 6)})

                    Dim g As Graphics = e.Graphics
                    g.FillPolygon(New SolidBrush(color), points)
                End If
                e.Handled = True
            End If
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for the <see cref="DgvSGs"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    Private Sub DgvSGs_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles _
        DgvLastSensorGlucose.ColumnAdded,
        DgvSGs.ColumnAdded

        With e.Column
            .AutoSizeMode = If(e.Column.Name = "Message",
                               DataGridViewAutoSizeColumnMode.Fill,
                               DataGridViewAutoSizeColumnMode.AllCells)

            If DataGridViewHelpers.HideColumn(Of SG)(dataPropertyName:= .Name) Then
                .Visible = False
            End If
            Dim dgv As DataGridView = CType(sender, DataGridView)
            e.DgvColumnAdded(
                cellStyle:=DataGridViewHelpers.GetCellStyle(Of SG)(columnName:= .Name),
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
    '''  Handles the <see cref="DataGridView.ColumnHeaderMouseClick"/> event for the <see cref="DgvSGs"/> DataGridView.
    '''  This event is raised when a column header is clicked, and sorts the DataGridView by the clicked column.
    '''  Only sorts when the first column header is clicked.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewCellMouseEventArgs"/> that contains the event data.</param>
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
    '''  Handles the <see cref="DataGridView.DataBindingComplete"/> event for the <see cref="DgvSGs"/> DataGridView.
    '''  This event is raised when the data binding operation is complete.
    '''  It sets the column autosize modes and sort glyphs, and ensures the last column is filled and wrapped.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewBindingCompleteEventArgs"/> that contains the event data.</param>
    Private Sub DgvSGs_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles _
        DgvSGs.DataBindingComplete

        Me.Dgv_DataBindingComplete(sender, e)
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim lastColumnIndex As Integer = dgv.Columns.Count - 1
        dgv.Columns(index:=lastColumnIndex).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgv.Columns(index:=lastColumnIndex).DefaultCellStyle.WrapMode = DataGridViewTriState.True
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
    '''  Handles the <see cref="DataGridView.CellFormatting"/> event for the <see cref="DgvSummary"/> DataGridView.
    '''  This event is raised when a cell's value needs to be formatted for display.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewCellFormattingEventArgs"/> that contains the event data.</param>
    Private Sub DgvSummary_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles _
        DgvSummary.CellFormatting

        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim key As String = dgv.Rows(index:=e.RowIndex).Cells(columnName:="key").Value.ToString
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
                        Case ServerDataIndexes.conduitSerialNumber, ServerDataIndexes.lastConduitDateTime,
                             ServerDataIndexes.systemStatusMessage,
                             ServerDataIndexes.sensorState, ServerDataIndexes.timeFormat,
                             ServerDataIndexes.bgUnits,
                             ServerDataIndexes.lastSGTrend, ServerDataIndexes.sensorLifeText,
                             ServerDataIndexes.sensorLifeIcon
                            e.CellStyle = e.CellStyle.SetCellStyle(
                                alignment:=DataGridViewContentAlignment.MiddleLeft,
                                padding:=New Padding(all:=1))

                        ' Not Clickable Cells - Center
                        Case ServerDataIndexes.clientTimeZoneName, ServerDataIndexes.lastName,
                             ServerDataIndexes.firstName, ServerDataIndexes.appModelType,
                             ServerDataIndexes.conduitBatteryStatus, ServerDataIndexes.medicalDeviceFamily,
                             ServerDataIndexes.medicalDeviceInformation, ServerDataIndexes.cgmInfo,
                             ServerDataIndexes.approvedForTreatment, ServerDataIndexes.calibStatus,
                             ServerDataIndexes.calFreeSensor, ServerDataIndexes.calibrationIconId,
                             ServerDataIndexes.finalCalibration,
                             ServerDataIndexes.pumpSuspended, ServerDataIndexes.conduitInRange,
                             ServerDataIndexes.conduitMedicalDeviceInRange, ServerDataIndexes.conduitSensorInRange,
                             ServerDataIndexes.gstCommunicationState, ServerDataIndexes.pumpCommunicationState
                            e.CellStyle = e.CellStyle.SetCellStyle(
                                alignment:=DataGridViewContentAlignment.MiddleCenter,
                                padding:=New Padding(all:=1))

                        ' Not Clickable - Data Dependent
                        Case ServerDataIndexes.appModelNumber, ServerDataIndexes.transmitterPairedTime
                            If eValue = "NA" Then
                                e.CellStyle = e.CellStyle.SetCellStyle(
                                    alignment:=DataGridViewContentAlignment.MiddleCenter,
                                    padding:=New Padding(all:=1))
                                e.Value = "N/A"
                            Else
                                e.CellStyle = e.CellStyle.SetCellStyle(
                                    alignment:=DataGridViewContentAlignment.MiddleRight,
                                    padding:=New Padding(left:=0, top:=1, right:=1, bottom:=1))
                            End If

                        ' Not Clickable Cells - Right
                        Case ServerDataIndexes.currentServerTime, ServerDataIndexes.conduitBatteryLevel,
                             ServerDataIndexes.lastConduitUpdateServerDateTime, ServerDataIndexes.medicalDeviceTime,
                             ServerDataIndexes.lastMedicalDeviceDataUpdateServerTime,
                             ServerDataIndexes.timeToNextCalibrationMinutes,
                             ServerDataIndexes.timeToNextCalibrationRecommendedMinutes,
                             ServerDataIndexes.timeToNextCalibHours,
                             ServerDataIndexes.sensorDurationHours, ServerDataIndexes.systemStatusTimeRemaining,
                             ServerDataIndexes.gstBatteryLevel, ServerDataIndexes.reservoirLevelPercent,
                             ServerDataIndexes.reservoirAmount, ServerDataIndexes.pumpBatteryLevelPercent,
                             ServerDataIndexes.reservoirRemainingUnits,
                             ServerDataIndexes.maxAutoBasalRate, ServerDataIndexes.maxBolusAmount,
                             ServerDataIndexes.sgBelowLimit, ServerDataIndexes.lastSensorTime,
                             ServerDataIndexes.averageSGFloat, ServerDataIndexes.averageSG,
                             ServerDataIndexes.belowHypoLimit, ServerDataIndexes.aboveHyperLimit,
                             ServerDataIndexes.timeInRange
                            e.CellStyle = e.CellStyle.SetCellStyle(
                                alignment:=DataGridViewContentAlignment.MiddleRight,
                                padding:=New Padding(left:=0, top:=1, right:=1, bottom:=1))

                         ' Not Clickable Cells - Integer with comma, align Right
                        Case ServerDataIndexes.timeToNextEarlyCalibrationMinutes,
                             ServerDataIndexes.sensorDurationMinutes
                            e.Value = $"{CInt(e.Value):N0}"
                            e.CellStyle = e.CellStyle.SetCellStyle(
                                alignment:=DataGridViewContentAlignment.MiddleRight,
                                padding:=New Padding(left:=0, top:=1, right:=1, bottom:=1))

                            ' Clickable Cells - Center
                        Case ServerDataIndexes.pumpBannerState, ServerDataIndexes.therapyAlgorithmState,
                             ServerDataIndexes.lastAlarm, ServerDataIndexes.activeInsulin,
                             ServerDataIndexes.basal, ServerDataIndexes.lastSG,
                             ServerDataIndexes.limits, ServerDataIndexes.markers,
                             ServerDataIndexes.sgs, ServerDataIndexes.notificationHistory
                            e.CellStyle = e.CellStyle.SetCellStyle(
                                alignment:=DataGridViewContentAlignment.MiddleCenter,
                                padding:=New Padding(all:=1))
                            dgv.CellFormattingApplyBoldColor(e, textColor:=Color.Black, isUri:=False, emIncrease:=1)
                        Case Else
                            Stop
                    End Select
                End If
            Case Else
        End Select
        dgv.CellFormattingSetForegroundColor(e)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.CellMouseClick"/> event for the <see cref="DgvSummary"/> DataGridView.
    '''  When a cell is clicked, checks if the value starts with <c>ClickToShowDetails</c>.
    '''  If so, navigates to the appropriate tab or page in the UI based on the key in the clicked row.
    '''  This allows users to quickly jump to detailed views for items such as last sensor glucose, alarms, insulin,
    '''  sensor glucose values, limits, markers, notification history, therapy algorithm state,
    '''  pump banner state, or basal.
    ''' </summary>
    ''' <param name="sender">The source of the event, typically the <see cref="DgvSummary"/> control.</param>
    ''' <param name="e">
    '''  A <see cref="DataGridViewCellMouseEventArgs"/> that contains the event data,
    '''  including the row and column indices.
    ''' </param>
    Private Sub DgvSummary_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles _
        DgvSummary.CellMouseClick

        If e.RowIndex < 0 OrElse _updating Then Exit Sub
        Dim dgv As DataGridView = CType(sender, DataGridView)
        Dim value As String = dgv.Rows(index:=e.RowIndex).Cells(index:=e.ColumnIndex).Value.ToString
        If value.StartsWith(value:=ClickToShowDetails) Then
            With Me.TabControlPage1
                Dim key As String = dgv.Rows(e.RowIndex).Cells(columnName:="key").Value.ToString
                Select Case key.GetItemIndex()
                    Case ServerDataIndexes.activeInsulin
                        .SelectedIndex = GetTabIndexFromName(tabPageName:=NameOf(TabPage05ActiveInsulin))
                    Case ServerDataIndexes.basal
                        .SelectedIndex = GetTabIndexFromName(tabPageName:=NameOf(TabPage06Basal))
                    Case ServerDataIndexes.lastAlarm
                        .SelectedIndex = GetTabIndexFromName(tabPageName:=NameOf(TabPage07LastAlarm))
                    Case ServerDataIndexes.lastSG
                        .SelectedIndex = GetTabIndexFromName(tabPageName:=NameOf(TabPage08LastSG))
                    Case ServerDataIndexes.limits
                        .SelectedIndex = GetTabIndexFromName(tabPageName:=NameOf(TabPage09Limits))
                    Case ServerDataIndexes.notificationHistory
                        .SelectedIndex = If(key = "activeNotification",
                            GetTabIndexFromName(tabPageName:=NameOf(TabPage10NotificationActive)),
                            GetTabIndexFromName(tabPageName:=NameOf(TabPage11NotificationsCleared)))
                    Case ServerDataIndexes.pumpBannerState
                        .SelectedIndex = GetTabIndexFromName(tabPageName:=NameOf(TabPage12PumpBannerState))
                    Case ServerDataIndexes.sgs
                        .SelectedIndex = GetTabIndexFromName(tabPageName:=NameOf(TabPage13SensorGlucose))
                    Case ServerDataIndexes.therapyAlgorithmState
                        .SelectedIndex = GetTabIndexFromName(tabPageName:=NameOf(TabPage14TherapyAlgorithmState))
                    Case ServerDataIndexes.markers
                        Dim page As Integer = _lastMarkerTabLocation.page
                        Dim tab As Integer = _lastMarkerTabLocation.tab
                        If page = 0 Then
                            _lastMarkerTabLocation = New TabLocation(page:=1, tab:=0)
                        End If
                        Me.TabControlPage2.SelectedIndex = _lastMarkerTabLocation.tab
                        .Visible = False
                End Select
            End With
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for the <see cref="DgvSummary"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    Private Sub DgvSummary_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles _
        DgvSummary.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            e.DgvColumnAdded(
                cellStyle:=DataGridViewHelpers.GetCellStyle(Of SummaryRecord)(columnName:= .Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

    ''' <summary>
    '''  Handles the <see cref="DataGridView.SelectionChanged"/> event for the <see cref="DgvSummary"/> DataGridView.
    '''  This event is raised when the selection in the DataGridView changes.
    '''  It updates the previous row and column indices to track the last selected cell.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub DgvSummary_SelectionChanged(sender As Object, e As EventArgs) Handles DgvSummary.SelectionChanged
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
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewCellFormattingEventArgs"/> that contains the event data.</param>
    Private Sub DgvTherapyAlgorithmState_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles _
        DgvTherapyAlgorithmState.CellFormatting

        Dim dgv As DataGridView = CType(sender, DataGridView)
        If e.Value IsNot Nothing AndAlso e.ColumnIndex = 2 Then
            Dim key As String = dgv.Rows(index:=e.RowIndex).Cells(columnName:="key").Value.ToString
            Select Case key
                Case NameOf(TherapyAlgorithmState.AutoModeReadinessState),
                     NameOf(TherapyAlgorithmState.AutoModeShieldState),
                     NameOf(TherapyAlgorithmState.PlgmLgsState)

                    e.CellStyle = e.CellStyle.SetCellStyle(
                        alignment:=DataGridViewContentAlignment.MiddleLeft,
                        padding:=New Padding(1))

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
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    Friend Sub DgvTherapyAlgorithmState_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles _
        DgvTherapyAlgorithmState.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If DataGridViewHelpers.HideColumn(Of TherapyAlgorithmState)(dataPropertyName:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=DataGridViewHelpers.GetCellStyle(Of BgReading)(columnName:= .Name),
                forceReadOnly:=True,
                caption:=CType(dgv.DataSource, DataTable).Columns(.Index).Caption)
        End With
    End Sub

#End Region ' Dgv Therapy Algorithm State Events

#Region "Dgv Time Change Events"

    ''' <summary>
    '''  Handles the <see cref="DataGridView.ColumnAdded"/> event for the <see cref="DgvTimeChange"/> DataGridView.
    '''  This event is raised when a new column is added to the DataGridView.
    '''  It sets the properties of the newly added column, such as sort mode, visibility, and cell style.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="DataGridView"/> control.</param>
    ''' <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    Friend Sub DgvTimeChange_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles _
        DgvTimeChange.ColumnAdded

        Dim dgv As DataGridView = CType(sender, DataGridView)
        With e.Column
            .SortMode = DataGridViewColumnSortMode.NotSortable
            If DataGridViewHelpers.HideColumn(Of TimeChange)(dataPropertyName:= .Name) Then
                .Visible = False
            End If
            e.DgvColumnAdded(
                cellStyle:=DataGridViewHelpers.GetCellStyle(Of TimeChange)(columnName:= .Name),
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
    ''' <param name="e">A <see cref="FormClosingEventArgs"/> that contains the event data.</param>
    ''' <remarks>
    '''  Ensures proper resource cleanup before the application exits, including killing the WebView2 process
    '''  and removing its cache directory if present.
    ''' </remarks>
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.NotifyIcon1?.Dispose()
        If _webView2ProcessId > 0 Then
            Dim webViewProcess As Process = Process.GetProcessById(_webView2ProcessId)
            ' TODO: dispose of the WebView2 control
            'LoginDialog.WebView21.Dispose()
            webViewProcess.Kill()
            webViewProcess.WaitForExit(3_000)
        End If

        If Directory.Exists(GetWebViewCacheDirectory()) Then
            Try
                Directory.Delete(GetWebViewCacheDirectory(), recursive:=True)
            Catch
                Stop
                ' Ignore errors here
            End Try
        End If
    End Sub

    ''' <summary>
    '''  Main form for the CareLink application.
    '''  Handles initialization, event wiring, chart setup, DataGridView formatting, and user interaction logic.
    ''' </summary>
    ''' <param name="sender">The source of the event, <see cref="Form"/>.</param>
    ''' <param name="e">The EventArgs containing the event data.</param>
    ''' <remarks>
    '''  This form manages the primary UI, including charts, data grids, user settings, and notification icons.
    '''  It coordinates loading and saving user data, updating UI elements, and responding to user and system events.
    ''' </remarks>
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.UpgradeRequired Then
            My.Settings.Upgrade()
            My.Settings.UpgradeRequired = False
            My.Settings.Save()
        End If
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
        If Not Directory.Exists(DirectoryForProjectData) Then
            Dim lastError As String = $"Can't create required project directories!"
            Directory.CreateDirectory(DirectoryForProjectData)
            Directory.CreateDirectory(GetSettingsDirectory())
        End If

        If Not Directory.Exists(path:=GetSettingsDirectory()) Then
            Directory.CreateDirectory(path:=GetSettingsDirectory())
        End If

        If File.Exists(UserSettingsCsvFileWithPath) Then
            s_allUserSettingsData.LoadUserRecords(UserSettingsCsvFileWithPath)
        Else
            My.Settings.AutoLogin = False
        End If

        Me.MenuOptionsShowChartLegends.Checked = My.Settings.SystemShowLegends
        Me.MenuOptionsSpeechHelpShown.Checked = My.Settings.SystemSpeechHelpShown
        My.Forms.OptionsConfigureTiTR.TreatmentTargetPercent = My.Settings.TiTrTreatmentTargetPercent
        My.Forms.OptionsConfigureTiTR.LowThreshold = My.Settings.TiTrLowThreshold
        Me.InitializeDgvCareLinkUsers(dgv:=Me.DgvCareLinkUsers)
        s_formLoaded = True
        Me.MenuOptionsAudioAlerts.Checked = My.Settings.SystemAudioAlertsEnabled
        Me.MenuOptionsSpeechRecognitionEnabled.Checked = My.Settings.SystemSpeechRecognitionThreshold < 1
        Me.SetSpeechRecognitionConfidenceThreshold()
        Me.MenuOptionsConfigureTiTR.Text = $"Configure TiTR ({My.Forms.OptionsConfigureTiTR.GetTiTrMsg()})..."
        AddHandler My.Settings.SettingChanging, AddressOf Me.MySettings_SettingChanging

        If File.Exists(GraphColorsFileNameWithPath) Then
            GetColorDictionaryFromFile()
        Else
            WriteColorDictionaryToFile()
        End If

        Me.InsulinTypeLabel.Text = s_insulinTypes.Keys(1)
        If String.IsNullOrWhiteSpace(value:=GetWebViewCacheDirectory()) Then
            s_webView2CacheDirectory = Path.Join(ProjectWebCache, Guid.NewGuid().ToString())
            Directory.CreateDirectory(path:=s_webView2CacheDirectory)
        End If

        Dim style As FontStyle = FontStyle.Bold
        Dim emSize As Single = 12.0F
        Me.DgvBasalPerHour.Font = New Font(FamilyName, emSize, style)
        Dim currentHeaderStyle As DataGridViewCellStyle = Me.DgvBasalPerHour.ColumnHeadersDefaultCellStyle.Clone
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
    '''  This event is used to manage the visibility of the notification icon when the form is minimized.
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
    '''  It performs additional initialization tasks such as setting up labels, checking for updates, and loading data.
    ''' </summary>
    ''' <param name="sender">The source of the event, <see cref="Form"/>.</param>
    ''' <param name="e">The EventArgs containing the event data.</param>
    ''' <remarks>
    '''  This event is used to finalize the setup of the form after it has been displayed to the user.
    ''' </remarks>
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
        CheckForUpdatesAsync(reportSuccessfulResult:=False)

        Me.ToolTip1.SetToolTip(control:=Me.TirComplianceLabel, caption:=UserMessageConstants.CheckComplianceValues)
        Me.ToolTip1.SetToolTip(control:=Me.LowTirComplianceLabel, caption:=UserMessageConstants.TirToolTip)
        Me.ToolTip1.SetToolTip(control:=Me.HighTirComplianceLabel, caption:=UserMessageConstants.TirToolTip)

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

        Me.NotifyIcon1.Visible = True
        Application.DoEvents()
        Me.NotifyIcon1.Visible = False
        Application.DoEvents()

        If DoOptionalLoginAndUpdateData(owner:=Me, updateAllTabs:=False, fileToLoad:=FileToLoadOptions.NewUser) Then
            Me.UpdateAllTabPages(fromFile:=False)
        End If

    End Sub

    ''' <summary>
    '''  Handles the <see cref="Button.Click"/> event for the <see cref="SerialNumberButton"/> control.
    '''  It switches to the Serial Number tab and scrolls to the last row in the DataGridView.
    '''  Highlights the row containing "medicalDeviceInformation" in the second column.
    ''' </summary>
    ''' <param name="sender">The source of the event, typically the SerialNumberButton.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    ''' <remarks>
    '''  This event is used to navigate to the Serial Number tab and focus on the relevant data.
    ''' </remarks>
    Private Sub SerialNumberButton_Click(sender As Object, e As EventArgs) Handles SerialNumberButton.Click
        Me.TabControlPage1.SelectedIndex = 3
        Me.TabControlPage1.Visible = True
        Dim dgv As DataGridView = CType(Me.TabControlPage1.TabPages(3).Controls(0), DataGridView)
        For Each row As DataGridViewRow In dgv.Rows
            If row.Cells(1).FormattedValue.ToString.StartsWith("medicalDeviceInformation") Then
                dgv.CurrentCell = dgv.Rows(row.Index).Cells(1)
                _dgvSummaryPrevRowIndex = dgv.CurrentCell.RowIndex
                _dgvSummaryPrevColIndex = dgv.CurrentCell.ColumnIndex
                dgv.Rows(row.Index).Selected = True
                dgv.FirstDisplayedScrollingRowIndex = row.Index
                Exit For
            End If
        Next
    End Sub

#End Region ' Form Events

#Region "Form Menu Events"

#Region "Start Here Menu Events"

    ''' <summary>
    '''  Handles the <see cref="MenuStartHere.DropDownOpening"/> event for the Start Here menu.
    '''  This event is raised when the Start Here menu is about to be displayed.
    '''  It enables or disables menu items based on the current state of the application.
    ''' </summary>
    ''' <param name="sender">The source of the event, a ToolStripMenuItem control.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    Private Sub MenuStartHere_DropDownOpening(sender As Object, e As EventArgs) Handles MenuStartHere.DropDownOpening
        Me.MenuStartHereLoadSavedDataFile.Enabled = AnyMatchingFiles(
            path:=DirectoryForProjectData,
            searchPattern:=$"CareLink*.json")
        Me.MenuStartHereSaveSnapshotFile.Enabled = Not RecentDataEmpty()
        Me.MenuStartHereUseExceptionReport.Visible = AnyMatchingFiles(
            path:=DirectoryForProjectData,
            searchPattern:=$"{BaseNameSavedErrorReport}*.txt")

        Dim userPdfExists As Boolean = Not (String.IsNullOrWhiteSpace(s_userName) OrElse
            Not AnyMatchingFiles(path:=GetSettingsDirectory(), searchPattern:=$"{s_userName}Settings.pdf"))

        Me.MenuStartHereShowPumpSetup.Enabled = userPdfExists AndAlso
                                                CurrentPdf IsNot Nothing AndAlso
                                                CurrentPdf.IsValid

        Me.MenuStartHereManuallyImportDeviceSettings.Enabled = Not userPdfExists
        ' The menu item For cleaning up obsolete files (MenuStartHereCleanUpObsoleteFiles) Is only enabled,
        ' when the application Is the only instance running, as a safety precaution.
        Me.MenuStartHereCleanUpObsoleteFiles.Enabled = Process.GetProcessesByName(processName:=_processName).Length = 1
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Form.Click"/> event for the Start Here menu item.
    '''  This event is raised when the Start Here menu item is clicked.
    '''  It opens the Start Here dialog to guide the user through the initial setup process.
    ''' </summary>
    ''' <param name="sender">The source of the event, a ToolStripMenuItem control.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    Private Sub MenuStartHereCleanUpObsoleteFiles_Click(sender As Object, e As EventArgs) Handles _
        MenuStartHereCleanUpObsoleteFiles.Click
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
    Private Sub MenuStartHereExit_Click(sender As Object, e As EventArgs) Handles MenuStartHereExit.Click
        Me.Close()
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Form.Click"/> event for the Manually Import Device Settings menu item.
    '''  This event is used to prompt the user to select a settings PDF file for import.
    Private Sub MenuStartHereManuallyImportDeviceSettings_Click(sender As Object, e As EventArgs) Handles _
        MenuStartHereManuallyImportDeviceSettings.Click

        Dim folder As String = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
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

            If openFileDialog1.ShowDialog(Me) = DialogResult.OK Then
                Dim directory As String = Path.GetDirectoryName(openFileDialog1.FileName)
                Dim destinationPath As String = Path.Combine(directory, UserSettingsPdfFileWithPath)
                File.Move(openFileDialog1.FileName, destinationPath)
                My.Computer.FileSystem.MoveFile(
                    sourceFileName:=destinationPath,
                    destinationFileName:=UserSettingsPdfFileWithPath,
                    showUI:=FileIO.UIOption.AllDialogs,
                    onUserCancel:=FileIO.UICancelOption.DoNothing)
            End If
        End Using
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Form.Click"/> event for the Show Pump Setup menu item.
    '''  This event is raised when the Show Pump Setup menu item is clicked.
    '''  It opens a dialog to display the pump setup information from the user's settings PDF file.
    ''' </summary>
    ''' <param name="sender">The source of the event, a ToolStripMenuItem control.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    Private Sub MenuStartHereShowPumpSetup_Click(sender As Object, e As EventArgs) Handles _
        MenuStartHereShowPumpSetup.Click

        If File.Exists(UserSettingsPdfFileWithPath) Then
            If CurrentPdf.IsValid Then
                StartOrStopServerUpdateTimer(Start:=False)
                Using dialog As New PumpSetupDialog
                    dialog.Pdf = CurrentPdf
                    dialog.ShowDialog(Me)
                End Using
            End If

            ' If the PDF file is not valid after setup, show a message box to the user.
            If CurrentPdf.IsValid Then
                StartOrStopServerUpdateTimer(Start:=True)
            Else
                MsgBox(
                    heading:=$"Device Setting PDF file Is invalid",
                    text:=UserSettingsPdfFileWithPath,
                    buttonStyle:=MsgBoxStyle.OkOnly,
                    title:="Invalid Settings PDF File")
            End If
        Else
            MsgBox(
                heading:=$"Device Setting PDF file Is missing!",
                text:=UserSettingsPdfFileWithPath,
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
    Private Sub MenuStartHereSnapshotSave_Click(sender As Object, e As EventArgs) Handles _
        MenuStartHereSaveSnapshotFile.Click

        If RecentDataEmpty() Then Exit Sub
        Dim path As String = GetUniqueDataFileName(
            baseName:=BaseNameSavedSnapshot,
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
    Private Sub MenuStartHereUseExceptionReport_Click(sender As Object, e As EventArgs) Handles _
        MenuStartHereUseExceptionReport.Click

        Dim fileList As String() = Directory.GetFiles(
            path:=DirectoryForProjectData,
            searchPattern:=$"{BaseNameSavedErrorReport}*.txt")
        Using openFileDialog1 As New System.Windows.Forms.OpenFileDialog With {
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
                    StartOrStopServerUpdateTimer(Start:=False)
                    If File.Exists(fileNameWithPath) Then
                        RecentData = New Dictionary(Of String, String)
                        ExceptionHandlerDialog.ReportFileNameWithPath = fileNameWithPath
                        If ExceptionHandlerDialog.ShowDialog(owner:=Me) = DialogResult.OK Then
                            ExceptionHandlerDialog.ReportFileNameWithPath = ""
                            Try
                                PatientDataElement = JsonSerializer.Deserialize(Of JsonElement)(
                                    json:=ExceptionHandlerDialog.LocalRawData)
                                DeserializePatientElement()
                                Me.TabControlPage2.Visible = True
                                Me.TabControlPage1.Visible = True
                            Catch ex As Exception
                                MessageBox.Show(text:=$"Error reading data file. Original error: {ex.DecodeException()}")
                            End Try
                            CurrentDateCulture = openFileDialog1.FileName.ExtractCultureFromFileName(
                                FixedPart:="CareLink",
                                fuzzy:=True)
                            Me.MenuShowMiniDisplay.Visible = Debugger.IsAttached
                            Me.Text = $"{SavedTitle} Using file {Path.GetFileName(fileNameWithPath)}"
                            Dim epochDateTime As Date = s_lastMedicalDeviceDataUpdateServerEpoch.Epoch2PumpDateTime
                            Me.SetLastUpdateTime(
                                msg:=epochDateTime.ToShortDateTimeString,
                                suffixMessage:="from file",
                                highLight:=False,
                                isDaylightSavingTime:=epochDateTime.IsDaylightSavingTime)
                            SetUpCareLinkUser()

                            Try
                                Me.UpdateAllTabPages(fromFile:=True)
                            Catch ex As Exception
                                MessageBox.Show(text:=$"Error in {NameOf(UpdateAllTabPages)}. Original error: {ex.Message}")
                            End Try
                            Try
                                Me.UpdateAllTabPages(fromFile:=True)
                            Catch ex As Exception
                                MessageBox.Show(text:=$"Error in {NameOf(UpdateAllTabPages)}. Original error: {ex.Message}")
                            End Try
                        End If
                    End If
                Catch ex As Exception
                    MessageBox.Show(text:=$"Cannot read file from disk. Original error: {ex.DecodeException()}")
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
    Private Sub MenuStartHereUseLastSavedFile_Click(sender As Object, e As EventArgs) Handles _
        MenuStartHereUseLastSavedFile.Click

        Dim success As Boolean = DoOptionalLoginAndUpdateData(
            owner:=Me,
            updateAllTabs:=True,
            fileToLoad:=FileToLoadOptions.LastSaved)
        Me.MenuStartHereSaveSnapshotFile.Enabled = Not success
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Form.Click"/> event for the User Login menu item.
    '''  This event is raised when the User Login menu item is clicked.
    '''  It allows the user to log in to their CareLink™ account and update the application state accordingly.
    ''' </summary>
    ''' <param name="sender">The source of the event, a ToolStripMenuItem control.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    ''' <remarks>
    '''  The user will be prompted to log in, and their data will be updated based on their account information.
    ''' </remarks>
    Private Sub MenuStartHereUserLogin_Click(sender As Object, e As EventArgs) Handles _
        MenuStartHereUserLogin.Click

        Dim success As Boolean = DoOptionalLoginAndUpdateData(
            owner:=Me,
            updateAllTabs:=True,
            fileToLoad:=FileToLoadOptions.NewUser)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="Form.Click"/> event for the Use Saved Data File menu item.
    '''  This event is raised when the Use Saved Data File menu item is clicked.
    '''  It allows the user to load a saved data file and update the application state accordingly.
    ''' </summary>
    ''' <param name="sender">The source of the event, a ToolStripMenuItem control.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    ''' <remarks>
    '''  The user can select a saved data file to load and process.
    ''' </remarks>
    Private Sub MenuStartHereUseSavedDataFile_Click(sender As Object, e As EventArgs) Handles _
        MenuStartHereLoadSavedDataFile.Click

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
    Private Sub MenuStartHereUseTestData_Click(sender As Object, e As EventArgs) Handles MenuStartHereUseTestData.Click
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
        For Each item As ToolStripMenuItem In Me.MenuOptionsSpeechRecognitionEnabled.DropDownItems
            If IsNumeric(item.Text) AndAlso item.Checked Then
                Return CDbl(item.Text)
            End If
        Next
        Return 100
    End Function

    ''' <summary>
    '''  Handles the <see cref="MenuOptions.DropDownOpening"/> event for the <see cref="MenuOptions"/> menu.
    '''  Enables or disables the Edit Pump Settings menu item based on debugger state or user name.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The event data.</param>
    <DebuggerNonUserCode()>
    Private Sub MenuOptions_DropDownOpening(sender As Object, e As EventArgs) Handles MenuOptions.DropDownOpening
        Me.MenuOptionsEditPumpSettings.Enabled = Debugger.IsAttached OrElse
            Not String.IsNullOrWhiteSpace(CurrentUser?.UserName)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="MenuOptionsAudioAlerts.Click"/> event.
    '''  This event is raised when the Audio Alerts menu item is clicked.
    '''  It toggles the audio alerts setting and initializes or cancels speech recognition accordingly.
    ''' </summary>
    ''' <param name="sender">The source of the event, a ToolStripMenuItem control.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
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

    ''' <summary>
    '''  Handles the <see cref="CheckBox.CheckedChanged"/> event for the <see cref="MenuOptionsAutoLogin"/> checkbox.
    '''  Updates the application's AutoLogin setting based on the checkbox state.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="CheckBox"/> control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuOptionsAutoLogin_CheckedChanged(sender As Object, e As EventArgs) Handles _
        MenuOptionsAutoLogin.CheckedChanged

        My.Settings.AutoLogin = Me.MenuOptionsAutoLogin.Checked
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event for the <see cref="MenuOptionsColorPicker"/> menu item.
    '''  Opens the <see cref="OptionsColorPickerDialog"/> for color selection.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="ToolStripMenuItem"/> control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuOptionsColorPicker_Click(sender As Object, e As EventArgs) Handles MenuOptionsColorPicker.Click
        Using o As New OptionsColorPickerDialog()
            o.ShowDialog(Me)
        End Using
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event for the <see cref="MenuOptionsConfigureTiTR_Click"/> menu item.
    '''  Opens the <see cref="OptionsConfigureTiTR"/> for configuration of Time in Tight Range (TiTR).
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="ToolStripMenuItem"/> control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuOptionsConfigureTiTR_Click(sender As Object, e As EventArgs) Handles MenuOptionsConfigureTiTR.Click
        Dim result As DialogResult = OptionsConfigureTiTR.ShowDialog(owner:=Me)
        If result = DialogResult.OK Then
            Me.MenuOptionsConfigureTiTR.Text = $"Configure TiTR ({OptionsConfigureTiTR.GetTiTrMsg()})..."
            Me.TiTRMgsLabel2.Text = OptionsConfigureTiTR.GetTiTrMsg()

            ' Update the TiTR compliance values based on the user's configuration.
            Me.UpdateTimeInRange()
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event for the <see cref="MenuOptionsEditPumpSettings"/> menu item.
    '''  Loads and deserializes the current user's pump settings from JSON.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="ToolStripMenuItem"/> control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuOptionsEditPumpSettings_Click(sender As Object, e As EventArgs) Handles _
        MenuOptionsEditPumpSettings.Click

        SetUpCareLinkUser(forceUI:=True)
        Dim contents As String = File.ReadAllText(UserSettingsFileWithPath)
        CurrentUser = JsonSerializer.Deserialize(Of CurrentUserRecord)(contents, s_jsonSerializerOptions)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event for the <see cref="MenuOptionsFilterRawJSONData"/> menu item.
    '''  Toggles the filtering of raw JSON data in all DataGridViews by hiding columns as defined by their respective helpers.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="ToolStripMenuItem"/> control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuOptionsFilterRawJSONData_Click(sender As Object, e As EventArgs) Handles MenuOptionsFilterRawJSONData.Click
        s_filterJsonData = Me.MenuOptionsFilterRawJSONData.Checked
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvActiveInsulin,
            hideColFunc:=Function(dataPropertyName)
                             Return DataGridViewHelpers.HideColumn(Of ActiveInsulin)(dataPropertyName)
                         End Function)
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvAutoBasalDelivery,
            hideColFunc:=Function(dataPropertyName)
                             Return DataGridViewHelpers.HideColumn(Of AutoBasalDelivery)(dataPropertyName)
                         End Function)
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvAutoModeStatus,
            hideColFunc:=Function(dataPropertyName)
                             Return DataGridViewHelpers.HideColumn(Of AutoModeStatus)(dataPropertyName)
                         End Function)
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvPumpBannerState,
            hideColFunc:=Function(dataPropertyName)
                             Return DataGridViewHelpers.HideColumn(Of BannerState)(dataPropertyName)
                         End Function)
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvBasal,
            hideColFunc:=Function(dataPropertyName) DataGridViewHelpers.HideColumn(Of Basal)(dataPropertyName))
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvBasalPerHour,
            hideColFunc:=Function(dataPropertyName)
                             Return DataGridViewHelpers.HideColumn(Of InsulinPerHour)(dataPropertyName)
                         End Function)
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvCalibration,
            hideColFunc:=Function(dataPropertyName)
                             Return DataGridViewHelpers.HideColumn(Of Calibration)(dataPropertyName)
                         End Function)
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvCareLinkUsers,
            hideColFunc:=Function(dataPropertyName)
                             Return DataGridViewHelpers.HideColumn(Of CurrentUserRecord)(dataPropertyName)
                         End Function)
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvCurrentUser,
            hideColFunc:=Function(dataPropertyName)
                             Return DataGridViewHelpers.HideColumn(Of CareLinkUserDataRecord)(dataPropertyName)
                         End Function)
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvInsulin,
            hideColFunc:=Function(dataPropertyName)
                             Return DataGridViewHelpers.HideColumn(Of Insulin)(dataPropertyName)
                         End Function)
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvLastAlarm,
            hideColFunc:=Function(dataPropertyName)
                             Return DataGridViewHelpers.HideColumn(Of LastAlarm)(dataPropertyName)
                         End Function)
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvLastSensorGlucose,
            hideColFunc:=Function(dataPropertyName)
                             Return DataGridViewHelpers.HideColumn(Of LastSG)(dataPropertyName)
                         End Function)
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvLimits,
            hideColFunc:=Function(dataPropertyName)
                             Return DataGridViewHelpers.HideColumn(Of Limit)(dataPropertyName)
                         End Function)
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvLowGlucoseSuspended,
            hideColFunc:=Function(dataPropertyName)
                             Return DataGridViewHelpers.HideColumn(Of LowGlucoseSuspended)(dataPropertyName)
                         End Function)
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvMeal,
            hideColFunc:=Function(dataPropertyName)
                             Return DataGridViewHelpers.HideColumn(Of Meal)(dataPropertyName)
                         End Function)
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvSensorBgReadings,
            hideColFunc:=Function(dataPropertyName)
                             Return DataGridViewHelpers.HideColumn(Of BgReading)(dataPropertyName)
                         End Function)
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvSGs,
            hideColFunc:=Function(dataPropertyName)
                             Return DataGridViewHelpers.HideColumn(Of SG)(dataPropertyName)
                         End Function)
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvTherapyAlgorithmState,
            hideColFunc:=Function(dataPropertyName)
                             Return DataGridViewHelpers.HideColumn(Of TherapyAlgorithmState)(dataPropertyName)
                         End Function)
        HideDataGridViewColumnsByName(
            dgv:=Me.DgvTimeChange,
            hideColFunc:=Function(dataPropertyName)
                             Return DataGridViewHelpers.HideColumn(Of TimeChange)(dataPropertyName)
                         End Function)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event for the <see cref="MenuOptionsShowChartLegends"/> menu item.
    '''  Toggles the visibility of chart legends for all main charts and updates the application settings.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="ToolStripMenuItem"/> control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuOptionsShowChartLegends_Click(sender As Object, e As EventArgs) Handles _
        MenuOptionsShowChartLegends.Click

        Dim showLegend As Boolean = Me.MenuOptionsShowChartLegends.Checked
        _activeInsulinChartLegend.Enabled = showLegend
        _summaryChartLegend.Enabled = showLegend
        _treatmentMarkersChartLegend.Enabled = showLegend
        My.Settings.SystemShowLegends = showLegend
        My.Settings.Save()
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event for the <see cref="MenuOptionsSpeechHelpShown"/> menu item.
    '''  Updates the <see cref="My.Settings.SystemSpeechHelpShown"/> setting based on the menu item's checked state.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="ToolStripMenuItem"/> control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuOptionsSpeechHelpShown_Click(sender As Object, e As EventArgs) Handles _
        MenuOptionsSpeechHelpShown.Click

        My.Settings.SystemSpeechHelpShown = Me.MenuOptionsSpeechHelpShown.Checked
        My.Settings.Save()
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event for the <see cref="MenuOptionsSpeechRecognition80"/> menu item.
    '''  Sets the speech recognition confidence threshold to 0.8 and updates the UI and speech recognition state.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="ToolStripMenuItem"/> control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuOptionsSpeechRecognition80_Click(sender As Object, e As EventArgs) Handles _
        MenuOptionsSpeechRecognition80.Click,
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
    '''  Sets the application's time zone to local or server based on the menu item's checked state,
    '''  and updates the corresponding setting if changed.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="ToolStripMenuItem"/> control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuOptionsUseLocalTimeZone_Click(sender As Object, e As EventArgs) Handles _
        MenuOptionsUseLocalTimeZone.Click

        ' Toggle the UseLocalTimeZone setting and update the PumpTimeZoneInfo accordingly.
        Dim saveRequired As Boolean = Me.MenuOptionsUseLocalTimeZone.Checked <> My.Settings.UseLocalTimeZone
        If Me.MenuOptionsUseLocalTimeZone.Checked Then
            PumpTimeZoneInfo = TimeZoneInfo.Local
            My.Settings.UseLocalTimeZone = True
        Else
            PumpTimeZoneInfo = CalculateTimeZone(timeZoneName:=RecentData(key:=NameOf(ServerDataIndexes.clientTimeZoneName)))
            My.Settings.UseLocalTimeZone = False
        End If
        If saveRequired Then My.Settings.Save()
    End Sub

    ''' <summary>
    '''  Sets the checked state of speech recognition confidence threshold menu items
    '''  based on the current value in <see cref="My.Settings.SystemSpeechRecognitionThreshold"/>.
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
        Me.MenuOptionsSpeechRecognitionEnabled.Checked = Me.MenuOptionsSpeechRecognitionDisabled.Checked = False
    End Sub

#End Region ' Menus Options

#Region "View Menu Events"

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event for the <see cref="MenuShowMiniDisplay"/> menu item.
    '''  Hides the main form and displays the mini SmartGuard display window.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="ToolStripMenuItem"/> control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuShowMiniDisplay_Click(sender As Object, e As EventArgs) Handles MenuShowMiniDisplay.Click
        Me.Hide()
        _sgMiniDisplay.Show()
    End Sub

#End Region ' View Menu Events

#Region "Help Menu Events"

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event for the <see cref="MenuHelpAbout"/> menu item.
    '''  Displays the About dialog box for the application.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="ToolStripMenuItem"/> control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuHelpAbout_Click(sender As Object, e As EventArgs) Handles MenuHelpAbout.Click
        AboutBox1.ShowDialog(Me)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event for the <see cref="MenuHelpCheckForUpdates"/> menu item.
    '''  Initiates an asynchronous check for application updates and reports the result.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="ToolStripMenuItem"/> control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuHelpCheckForUpdates_Click(sender As Object, e As EventArgs) Handles MenuHelpCheckForUpdates.Click
        CheckForUpdatesAsync(reportSuccessfulResult:=True)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="ToolStripMenuItem.Click"/> event for the <see cref="MenuHelpReportAnIssue"/> menu item.
    '''  Opens the GitHub issues page for the CareLink project in the default web browser.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="ToolStripMenuItem"/> control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub MenuHelpReportAnIssue_Click(sender As Object, e As EventArgs) Handles MenuHelpReportAnIssue.Click
        OpenUrlInBrowser($"{GitHubCareLinkUrl}issues")
    End Sub

#End Region ' Help Menu Events

#End Region 'Form Menu Events

#Region "Form Misc Events"

    ''' <summary>
    '''  Handles the <see cref="Label.Paint"/> event for the <see cref="ActiveInsulinValue"/> control.
    '''  Draws a solid lime green border around the ActiveInsulinValue label.
    ''' </summary>
    ''' <param name="sender">The source of the event, the ActiveInsulinValue label.</param>
    ''' <param name="e">A <see cref="PaintEventArgs"/> that contains the event data.</param>
    Private Sub ActiveInsulinValue_Paint(sender As Object, e As PaintEventArgs) Handles ActiveInsulinValue.Paint
        ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, Color.LimeGreen, ButtonBorderStyle.Solid)
    End Sub

    ''' <summary>
    '''  Handles the <see cref="CheckBox.CheckedChanged"/> event for
    '''  the <see cref="TemporaryUseAdvanceAITDecayCheckBox"/>.
    '''  Updates the checkbox text and the <see cref="CurrentUser.UseAdvancedAitDecay"/> property,
    '''  then updates the active insulin chart.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="CheckBox"/> control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub TemporaryUseAdvanceAITDecayCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles _
        TemporaryUseAdvanceAITDecayCheckBox.CheckedChanged

        ' Update the checkbox text based on the current state and insulin type.
        With CurrentUser
            Me.TemporaryUseAdvanceAITDecayCheckBox.Text = If(
                Me.TemporaryUseAdvanceAITDecayCheckBox.CheckState = CheckState.Checked,
                $"Advanced Decay, AIT will decay over { .InsulinRealAit} hours While Using { .InsulinTypeName}",
                $"AIT will decay over { .PumpAit.ToHoursMinutes} While Using { .InsulinTypeName}")
            CurrentUser.UseAdvancedAitDecay = Me.TemporaryUseAdvanceAITDecayCheckBox.CheckState
        End With
        Me.UpdateActiveInsulinChart()
    End Sub

#End Region ' Form Misc Events

#Region "NotifyIcon Events"

    ''' <summary>
    '''  Handles the <see cref="NotifyIcon.DoubleClick"/> event for the application's notification icon.
    '''  Restores the main window to the taskbar and sets its state to normal when the icon is double-clicked.
    ''' </summary>
    ''' <param name="sender">The source of the event, a <see cref="NotifyIcon"/> control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub NotifyIcon1_DoubleClick(sender As Object, e As EventArgs) Handles NotifyIcon1.DoubleClick
        Me.ShowInTaskbar = True
        Me.WindowState = FormWindowState.Normal
    End Sub

#End Region ' NotifyIcon Events

#Region "Settings Events"

    ''' <summary>
    '''  Handles the <see cref="ApplicationSettingsBase.SettingChanging"/> event for application settings.
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
    '''  A <see cref="SettingChangingEventArgs"/> containing the event data, including the setting name and new value.
    ''' </param>
    Private Sub MySettings_SettingChanging(sender As Object, e As SettingChangingEventArgs)
        If e.SettingName.StartsWith(value:="System") Then Exit Sub

        Dim value As String = If(IsNothing(Expression:=e.NewValue),
                                 "",
                                 e.NewValue.ToString)
        If EqualsIgnoreCase(My.Settings(propertyName:=e.SettingName), value) Then
            Exit Sub
        End If
        If e.SettingName = "CareLinkUserName" Then
            If s_allUserSettingsData?.ContainsKey(key:=e.NewValue.ToString) Then
                LoginHelpers.LoginDialog.LoggedOnUser = s_allUserSettingsData(itemName:=e.NewValue.ToString)
                Exit Sub
            Else
                Dim userSettings As New CareLinkUserDataRecord(parent:=s_allUserSettingsData)
                userSettings.UpdateValue(key:=e.SettingName, value:=e.NewValue.ToString)
                s_allUserSettingsData.Add(value:=userSettings)
            End If
        End If
        s_allUserSettingsData.SaveAllUserRecords(
            LoginHelpers.LoginDialog.LoggedOnUser,
            key:=e.SettingName,
            value:=(e.NewValue?.ToString))
    End Sub

#End Region ' Settings Events

#Region "Summary Events"

    ''' <summary>
    '''  Handles the <see cref="MouseHover"/> event for the CalibrationDueImage control.
    '''  Displays a tooltip with the time of the next calibration if it is due within the next 24 hours.
    ''' </summary>
    ''' <param name="sender">The source of the event, typically the CalibrationDueImage control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub CalibrationDueImage_MouseHover(sender As Object, e As EventArgs) Handles CalibrationDueImage.MouseHover
        If s_timeToNextCalibrationMinutes > 0 AndAlso s_timeToNextCalibrationMinutes < 1440 Then
            Dim caption As String =
                $"Calibration Due {PumpNow.AddMinutes(value:=s_timeToNextCalibrationMinutes).ToShortTimeString()}"

            _calibrationToolTip.SetToolTip(
                control:=Me.CalibrationDueImage,
                caption)
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="MouseHover"/> event for the SensorDaysLeftLabel control.
    '''  Displays a tooltip with the remaining sensor duration in hours if it is less than 24 hours.
    ''' </summary>
    ''' <param name="sender">The source of the event, typically the SensorDaysLeftLabel control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub Last24HrCarbLabel_MouseHover(sender As Object, e As EventArgs) Handles _
        Last24HrCarbsLabel.MouseHover, Last24HrCarbsValueLabel.MouseHover

        _carbRatio.SetToolTip(
            control:=DirectCast(sender, Label),
            caption:=$"Carb Ratio {CDbl(s_totalCarbs / s_totalManualBolus):N1}")
    End Sub

    ''' <summary>
    '''  Handles the <see cref="MouseHover"/> event for the SensorDaysLeftLabel control.
    '''  Displays a tooltip with the remaining sensor duration in hours if it is less than 24 hours.
    ''' </summary>
    ''' <param name="sender">The source of the event, typically the SensorDaysLeftLabel control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub SensorDaysLeftLabel_MouseHover(sender As Object, e As EventArgs) Handles SensorDaysLeftLabel.MouseHover
        If PatientData.SensorDurationHours < 24 Then
            _sensorLifeToolTip.SetToolTip(
                control:=Me.SensorDaysLeftLabel,
                caption:=$"Sensor will expire In {PatientData.SensorDurationHours} hours")
        End If
    End Sub

#End Region ' Summary Events

#Region "Tab Events"

    ''' <summary>
    '''  Handles the <see cref="TabControl.Selecting"/> event for the main tab control.
    '''  Updates the cursor and last selected tab index based on the selected tab page.
    ''' </summary>
    ''' <param name="sender">The source of the event, typically a TabControl control.</param>
    ''' <param name="e">A <see cref="TabControlCancelEventArgs"/> that contains the event data.</param>
    ''' <remarks>
    '''  This method is used to manage cursor visibility and last selected tab index for navigation.
    ''' </remarks>
    Private Sub TabControlPage1_Selecting(sender As Object, e As TabControlCancelEventArgs) Handles _
        TabControlPage1.Selecting

        Select Case e.TabPage.Name
            Case NameOf(TabPage15More)
                Me.DgvCareLinkUsers.InitializeDgv

                For Each c As DataGridViewColumn In Me.DgvCareLinkUsers.Columns
                    c.Visible = Not DataGridViewHelpers.HideColumn(Of CareLinkUserDataRecord)(c.DataPropertyName)
                Next
                Me.TabControlPage2.SelectedIndex = If(_lastMarkerTabLocation.page = 0,
                                                      0,
                                                      _lastMarkerTabLocation.tab)
                Me.TabControlPage1.Visible = False
                Exit Sub
        End Select
        _lastSummaryTabIndex = e.TabPageIndex
    End Sub

    ''' <summary>
    '''  Handles the <see cref="TabControlPage2.Selecting"/> event for the secondary tab control.
    '''  Updates the selected index and visibility of the main tab control based on the selected tab page.
    ''' </summary>
    ''' <param name="sender">The source of the event, typically a TabControl control.</param>
    ''' <param name="e">A <see cref="TabControlCancelEventArgs"/> that contains the event data.</param>
    ''' <remarks>
    '''  This method is used to manage navigation between different summary tabs and user settings.
    ''' </remarks>
    Private Sub TabControlPage2_Selecting(sender As Object, e As TabControlCancelEventArgs) Handles _
        TabControlPage2.Selecting

        Select Case e.TabPage.Name
            Case NameOf(TabPage12BackToHomePage)
                Me.TabControlPage1.SelectedIndex = _lastSummaryTabIndex
                Me.TabControlPage1.Visible = True
                Exit Sub
            Case NameOf(TabPage11AllUsers)
                Me.DgvCareLinkUsers.DataSource = s_allUserSettingsData
                For Each c As DataGridViewColumn In Me.DgvCareLinkUsers.Columns
                    c.Visible = Not DataGridViewHelpers.HideColumn(Of CareLinkUserDataRecord)(c.DataPropertyName)
                Next
            Case Else
                If e.TabPageIndex <= GetTabIndexFromName(NameOf(TabPage09BasalPerHour)) Then
                    _lastMarkerTabLocation = (page:=1, tab:=e.TabPageIndex)
                End If
        End Select
    End Sub

#End Region ' Tab Events

#Region "TableLayoutPanelTop Button Events"

    ''' <summary>
    '''  Handles the <see cref="Button.Click"/> event for buttons in the top TableLayoutPanel controls.
    '''  This method is used to navigate to the corresponding summary tab based on the button clicked.
    ''' </summary>
    ''' <param name="sender">The source of the event, typically a Button control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    ''' <remarks>
    '''  The method identifies the tab name from the button's parent TableLayoutPanel and
    '''  selects the appropriate tab in the main tab control.
    ''' </remarks>
    Private Sub TableLayoutPanelTopButton_Click(sender As Object, e As EventArgs) Handles _
        TableLayoutPanelActiveInsulinTop.ButtonClick,
        TableLayoutPanelAutoBasalDeliveryTop.ButtonClick,
        TableLayoutPanelAutoModeStatusTop.ButtonClick,
        TableLayoutPanelPumpBannerStateTop.ButtonClick,
        TableLayoutPanelBasalTop.ButtonClick,
        TableLayoutPanelBgReadingsTop.ButtonClick,
        TableLayoutPanelCalibrationTop.ButtonClick,
        TableLayoutPanelInsulinTop.ButtonClick,
        TableLayoutPanelLastAlarmTop.ButtonClick,
        TableLayoutPanelLastSgTop.ButtonClick,
        TableLayoutPanelLimitsTop.ButtonClick,
        TableLayoutPanelLowGlucoseSuspendedTop.ButtonClick,
        TableLayoutPanelMealTop.ButtonClick,
        TableLayoutPanelNotificationActiveTop.ButtonClick,
        TableLayoutPanelNotificationsClearedTop.ButtonClick,
        TableLayoutPanelSgsTop.ButtonClick,
        TableLayoutPanelTherapyAlgorithmStateTop.ButtonClick,
        TableLayoutPanelTimeChangeTop.ButtonClick

        Me.TabControlPage1.Visible = True
        Dim button As Button = CType(sender, Button)
        Dim panelTop As TableLayoutPanelTopEx = CType(button.Parent, TableLayoutPanelTopEx)
        Dim tabName As String = panelTop.LabelText.Split(separator:=":")(0).Remove(oldValue:=" ")
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
        Dim dgv As DataGridView = CType(Me.TabControlPage1.TabPages(index:=3).Controls(index:=0), DataGridView)
        For index As Integer = 0 To dgv.RowCount - 1
            Dim row As DataGridViewRow = dgv.Rows(index)
            Dim message As String = row.Cells(index:=1).FormattedValue.ToString
            Debug.WriteLine(message)
            If message.EqualsIgnoreCase(tabName) Then
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
    '''  This method is called periodically to reset the cursor position if the chart is not zoomed.
    ''' </summary>
    ''' <param name="sender">The source of the event, typically a Timer control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    ''' <remarks>
    '''  The timer is stopped if the chart is not zoomed, and the cursor position is set to NaN.
    ''' </remarks>
    Private Sub CursorTimer_Tick(sender As Object, e As EventArgs) Handles CursorTimer.Tick
        If Not Me.SummaryChart.ChartAreas(NameOf(ChartArea)).AxisX.ScaleView.IsZoomed Then
            Me.CursorTimer.Enabled = False
            Me.SummaryChart.ChartAreas(NameOf(ChartArea)).CursorX.Position = Double.NaN
        End If
    End Sub

    ''' <summary>
    '''  Handles the <see cref="PowerModeChanged"/> event for system power mode changes.
    '''  This method is called when the system enters or resumes from a sleep state.
    '''  It manages the server update timer and updates the last update time accordingly.
    ''' </summary>
    ''' <param name="sender">The source of the event, typically the application or system.</param>
    ''' <param name="e">A <see cref="PowerModeChangedEventArgs"/> that contains the event data.</param>
    ''' <remarks>
    '''  The method stops the server update timer on suspend and restarts it on resume.
    ''' </remarks>
    Private Sub PowerModeChanged(sender As Object, e As PowerModeChangedEventArgs)
        Debug.WriteLine($"PowerModeChange {e.Mode}")
        Select Case e.Mode
            Case PowerModes.Suspend
                StartOrStopServerUpdateTimer(Start:=False)
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
                StartOrStopServerUpdateTimer(Start:=True, interval:=ThirtySecondInMilliseconds \ 3)
                Dim message As String =
                    $"restarted after wake. {NameOf(ServerUpdateTimer)} started at {Now.ToLongTimeString}"
                DebugPrint(message)
        End Select

    End Sub

    ''' <summary>
    '''  Handles the <see cref="Timer.Tick"/> event for the server update timer.
    '''  This method is called periodically to update the server data and refresh the UI.
    ''' </summary>
    ''' <param name="sender">The source of the event, typically a Timer control.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    ''' <remarks>
    '''  The method checks if updates are in progress, retrieves recent data, and updates the UI accordingly.
    ''' </remarks>
    Private Sub ServerUpdateTimer_Tick(sender As Object, e As EventArgs) Handles ServerUpdateTimer.Tick
        StartOrStopServerUpdateTimer(Start:=False)
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
                            Dim result As DialogResult = LoginDialog.ShowDialog(Me)
                            Select Case result
                                Case DialogResult.OK
                                    Exit Do
                                Case DialogResult.Cancel
                                    StartOrStopServerUpdateTimer(Start:=False)
                                    Return
                                Case DialogResult.Retry
                            End Select
                        Loop

                        Client = LoginDialog.Client
                    End If
                    lastErrorMessage = Client.GetRecentData()
                End If
                ReportLoginStatus(Me.LoginStatus, hasErrors:=RecentDataEmpty, lastErrorMessage)

                Me.Cursor = Cursors.Default
                Application.DoEvents()
            End If
            _updating = False
        End SyncLock

        Dim lastMedicalDeviceDataUpdateServerEpochString As String = ""
        If Not RecentDataEmpty() Then
            If RecentData.TryGetValue(
                    key:=NameOf(ServerDataIndexes.lastMedicalDeviceDataUpdateServerTime),
                    value:=lastMedicalDeviceDataUpdateServerEpochString) Then
                If CLng(lastMedicalDeviceDataUpdateServerEpochString) = s_lastMedicalDeviceDataUpdateServerEpoch Then
                    Dim epochAsLocalDate As Date = lastMedicalDeviceDataUpdateServerEpochString.FromUnixTime.ToLocalTime
                    If epochAsLocalDate + FiveMinuteSpan < Now() Then
                        Me.SetLastUpdateTime(
                            msg:=Nothing,
                            suffixMessage:="",
                            highLight:=True,
                            isDaylightSavingTime:=epochAsLocalDate.IsDaylightSavingTime)
                        _sgMiniDisplay.SetCurrentSgString(sgString:="---", sgValue:=Single.NaN)
                    Else
                        Me.SetLastUpdateTime(
                            msg:=Nothing,
                            suffixMessage:="",
                            highLight:=False,
                            isDaylightSavingTime:=epochAsLocalDate.IsDaylightSavingTime)
                        _sgMiniDisplay.SetCurrentSgString(s_lastSg?.ToString, s_lastSg.sg)
                    End If
                Else
                    Me.UpdateAllTabPages(fromFile:=False)
                End If
            Else
                Stop
            End If
        Else
            ReportLoginStatus(Me.LoginStatus, hasErrors:=True, lastErrorMessage)
            _sgMiniDisplay.SetCurrentSgString(sgString:="---", sgValue:=0)
        End If
        StartOrStopServerUpdateTimer(Start:=True, interval:=OneMinutesInMilliseconds)
    End Sub

#End Region ' Timer Events

#End Region ' Events

#Region "Initialize Charts"

#Region "Initialize Summary Charts"

    ''' <summary>
    '''  Initializes the summary tab charts, including setting up chart areas, series, and legends.
    '''  This method is called to prepare the summary chart for displaying data related to insulin therapy.
    ''' </summary>
    Friend Sub InitializeSummaryTabCharts()
        Me.SplitContainer3.Panel1.Controls.Clear()
        Me.SummaryChart = CreateChart(NameOf(SummaryChart))
        Dim summaryTitle As Title = CreateTitle(
            chartTitle:="Summary",
            name:=NameOf(summaryTitle),
            foreColor:=Me.SummaryChart.BackColor.ContrastingColor())

        Dim summaryChartArea As ChartArea = CreateChartArea(Me.SummaryChart)
        Me.SummaryChart.ChartAreas.Add(summaryChartArea)
        _summaryChartLegend = CreateChartLegend(NameOf(_summaryChartLegend))

        Me.SummaryAutoCorrectionSeries = CreateSeriesBasal(
            seriesName:=AutoCorrectionSeriesName,
            basalLegend:=_summaryChartLegend,
            legendText:="Auto Correction",
            yAxisType:=AxisType.Secondary)
        Me.SummaryBasalSeries = CreateSeriesBasal(
            seriesName:=BasalSeriesName,
            basalLegend:=_summaryChartLegend,
            legendText:="Basal Series",
            yAxisType:=AxisType.Secondary)
        Me.SummaryMinBasalSeries = CreateSeriesBasal(
            seriesName:=MinBasalSeriesName,
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
        Me.SummaryMarkerSeries = CreateSeriesWithoutVisibleLegend(YAxisType:=AxisType.Secondary)
        Me.SummaryTimeChangeSeries = CreateSeriesTimeChange(basalLegend:=_summaryChartLegend)

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

    ''' <summary>
    '''  Initializes the Time in Range area of the home tab, setting up labels and a chart for displaying time in range data.
    '''  This method is called to prepare the Time in Range chart and labels for displaying compliance information.
    ''' </summary>
    Friend Sub InitializeTimeInRangeArea()
        If Me.SplitContainer3.Panel2.Controls.FindControlByName(NameOf(Me.TimeInRangeChart)) Is Nothing Then
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
                .Series.Add(Me.TimeInRangeSeries)
                .Series(NameOf(TimeInRangeSeries))("DoughnutRadius") = "17"
            End With
            Me.SplitContainer3.Panel2.Controls.Add(Me.TimeInRangeChart)
        End If
        Me.PositionControlsInPanel()
    End Sub

#End Region ' Initialize Home Tab Charts

#Region "Initialize Chart Tabs"

#Region "Initialize Active Insulin Chart"

    ''' <summary>
    '''  Initializes the Active Insulin tab chart, including chart area, axes, series, and legend.
    '''  This method sets up the chart for displaying active insulin on board (IOB) and related data.
    ''' </summary>
    ''' <remarks>
    ''' - Clears any existing controls from the panel.
    ''' - Creates and configures the chart and its area, including axis labels, intervals, and colors.
    ''' - Adds all required series for displaying active insulin, basal, auto correction, min basal, suspend, sensor glucose, markers, and time change.
    ''' - Adds the chart legend and title.
    ''' - Adds the chart to the panel and processes UI events.
    ''' </remarks>
    Friend Sub InitializeActiveInsulinTabChart()
        Me.SplitContainer1.Panel2.Controls.Clear()
        Me.ActiveInsulinChart = CreateChart(NameOf(ActiveInsulinChart))
        Dim activeInsulinChartArea As ChartArea = CreateChartArea(containingChart:=Me.ActiveInsulinChart)
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
        Me.ActiveInsulinChart.ChartAreas.Add(activeInsulinChartArea)
        _activeInsulinChartLegend = CreateChartLegend(legendName:=NameOf(_activeInsulinChartLegend))
        Me.ActiveInsulinChartTitle = CreateTitle(
            chartTitle:=$"Running Insulin On Board (IOB)",
            name:=NameOf(ActiveInsulinChartTitle),
            foreColor:=GetGraphLineColor("Active Insulin"))
        Me.ActiveInsulinActiveInsulinSeries = CreateSeriesActiveInsulin()
        Me.ActiveInsulinTargetSeries = CreateSeriesLimitsAndTarget(
            limitsLegend:=_activeInsulinChartLegend,
            seriesName:=TargetSgSeriesName)

        Me.ActiveInsulinAutoCorrectionSeries = CreateSeriesBasal(
            seriesName:=AutoCorrectionSeriesName,
            basalLegend:=_activeInsulinChartLegend,
            legendText:="Auto Correction",
            yAxisType:=AxisType.Secondary)
        Me.ActiveInsulinBasalSeries = CreateSeriesBasal(
            seriesName:=BasalSeriesName,
            basalLegend:=_activeInsulinChartLegend,
            legendText:="Basal Series",
            yAxisType:=AxisType.Secondary)
        Me.ActiveInsulinMinBasalSeries = CreateSeriesBasal(
            seriesName:=MinBasalSeriesName,
            basalLegend:=_activeInsulinChartLegend,
            legendText:="Min Basal",
            yAxisType:=AxisType.Secondary)

        Me.ActiveInsulinSuspendSeries = CreateSeriesSuspend(basalLegend:=_activeInsulinChartLegend)

        Me.ActiveInsulinSgSeries = CreateSeriesSg(sgLegend:=_activeInsulinChartLegend)
        Me.ActiveInsulinMarkerSeries = CreateSeriesWithoutVisibleLegend(YAxisType:=AxisType.Secondary)
        Me.ActiveInsulinTimeChangeSeries = CreateSeriesTimeChange(basalLegend:=_activeInsulinChartLegend)

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

    ''' <summary>
    '''  Initializes the Treatment Markers tab chart, including chart area, axes, series, and legend.
    '''  This method sets up the chart for displaying treatment details
    '''  such as insulin delivery and sensor glucose readings.
    ''' </summary>
    ''' <remarks>
    '''  <list type="bullet">
    '''   <item>Clears any existing controls from the treatment details tab page.</item>
    '''   <item>Creates and configures the chart and its area, including axis labels, intervals, and colors.</item>
    '''   <item>Sets the maximum insulin delivery row based on the maximum basal per dose.
    '''     Adjusts the interval and label style for the Y-axis to display insulin delivery values.
    '''   </item>
    '''   <item>Adds all required series for displaying treatment markers.
    '''     This includes target sensor glucose, auto correction, basal series, min basal, suspend,
    '''     sensor glucose, markers, and time change.
    '''   </item>
    '''   <item>Sets the legend and title for the chart.</item>
    '''   <item>Adds the chart to the treatment details tab page and processes UI events.</item>
    '''  </list>
    ''' </remarks>
    Private Sub InitializeTreatmentMarkersChart()
        Me.TabPage03TreatmentDetails.Controls.Clear()

        Me.TreatmentMarkersChart = CreateChart(key:=NameOf(TreatmentMarkersChart))
        Dim treatmentMarkersChartArea As ChartArea = CreateChartArea(containingChart:=Me.TreatmentMarkersChart)

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
            Dim interval As Single = (TreatmentInsulinRow / 10).RoundSingle(digits:=3, considerValue:=False)
            .Interval = interval
            .IsInterlaced = False
            .IsMarginVisible = False
            With .LabelStyle
                .Font = labelFont
                .ForeColor = baseColor
                .Format = $"{{0{Provider.NumberFormat.NumberDecimalSeparator}00}}"
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
        Me.TreatmentMarkerAutoCorrectionSeries = CreateSeriesBasal(
            seriesName:=AutoCorrectionSeriesName,
            basalLegend:=_treatmentMarkersChartLegend,
            legendText:="Auto Correction",
            yAxisType:=AxisType.Primary)
        Me.TreatmentMarkerBasalSeries = CreateSeriesBasal(
            seriesName:=BasalSeriesName,
            basalLegend:=_treatmentMarkersChartLegend,
            legendText:="Basal Series",
            yAxisType:=AxisType.Primary)
        Me.TreatmentMarkerMinBasalSeries = CreateSeriesBasal(
            seriesName:=MinBasalSeriesName,
            basalLegend:=_treatmentMarkersChartLegend,
            legendText:="Min Basal",
            yAxisType:=AxisType.Primary)

        Me.TreatmentMarkerSgSeries = CreateSeriesSg(sgLegend:=_treatmentMarkersChartLegend)
        Me.TreatmentMarkerMarkersSeries = CreateSeriesWithoutVisibleLegend(YAxisType:=AxisType.Primary)
        Me.TreatmentMarkerTimeChangeSeries = CreateSeriesTimeChange(basalLegend:=_treatmentMarkersChartLegend)
        Me.TreatmentMarkerSuspendSeries = CreateSeriesSuspend(basalLegend:=_treatmentMarkersChartLegend)

        With Me.TreatmentMarkersChart
            With .Series
                .Add(item:=Me.TreatmentTargetSeries)
                .Add(item:=Me.TreatmentMarkerSuspendSeries)
                .Add(item:=Me.TreatmentMarkerTimeChangeSeries)

                .Add(item:=Me.TreatmentMarkerAutoCorrectionSeries)
                .Add(item:=Me.TreatmentMarkerBasalSeries)
                .Add(item:=Me.TreatmentMarkerMinBasalSeries)

                .Add(item:=Me.TreatmentMarkerSgSeries)
                .Add(item:=Me.TreatmentMarkerMarkersSeries)
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
    '''  Updates the notification icon with the latest sensor glucose value and displays a balloon tip if necessary.
    '''  This method is called to refresh the notification icon based on the latest sensor glucose reading.
    ''' </summary>
    ''' <param name="sgString">The last sensor glucose value as a string.</param>
    Private Sub UpdateNotifyIcon(sgString As String)
        Try
            Dim sg As Single = s_lastSg.sg
            Using bitmapText As New Bitmap(width:=16, height:=16)
                Using g As Graphics = Graphics.FromImage(bitmapText)
                    Dim backColor As Color
                    Select Case sg
                        Case <= GetTirLowLimit()
                            backColor = Color.Yellow
                            If _showBalloonTip Then
                                Me.NotifyIcon1.ShowBalloonTip(
                                    timeout:=10000,
                                    tipTitle:=$"CareLink™ Alert",
                                    tipText:=$"SG below {GetTirLowLimitWithUnits()} {GetBgUnitsString()}",
                                    tipIcon:=Me.ToolTip1.ToolTipIcon)
                            End If
                            _showBalloonTip = False
                        Case <= GetTirHighLimit()
                            backColor = Color.Green
                            _showBalloonTip = True
                        Case Else
                            backColor = Color.Red
                            If _showBalloonTip Then
                                Me.NotifyIcon1.ShowBalloonTip(
                                    timeout:=10000,
                                    tipTitle:=$"CareLink™ Alert",
                                    tipText:=$"SG above {GetTirHighLimitWithUnits()} {GetBgUnitsString()}",
                                    tipIcon:=Me.ToolTip1.ToolTipIcon)
                            End If
                            _showBalloonTip = False
                    End Select

                    Dim s As String = sgString.PadRight(totalWidth:=3) _
                                              .Substring(startIndex:=0, length:=3).Trim _
                                              .PadLeft(totalWidth:=3)
                    Me.NotifyIcon1.Icon = CreateTextIcon(s, backColor)
                    Dim strBuilder As New StringBuilder(100)
                    Dim dateSeparator As String = CultureInfo.CurrentUICulture.DateTimeFormat.DateSeparator
                    strBuilder.AppendLine(
                        value:=Date.Now().ToShortDateTimeString.Remove(oldValue:=$"{dateSeparator}{Now.Year}"))
                    strBuilder.AppendLine(value:=$"Last SG {sgString} {GetBgUnitsString()}")
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
                                deltaString = If(Math.Abs(value:=delta) < 0.001,
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
                    strBuilder.Append(value:=$"Active ins. {PatientData.ActiveInsulin?.Amount:N3} U")
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
    '''  Fixes the <see cref="SplitContainer"/> control's SplitterDistance based on the current form scale.
    '''  This method is called to ensure that the SplitContainer's splitter distance is correctly scaled
    '''  when the form is resized or scaled.
    ''' </summary>
    ''' <param name="sp">The SplitContainer control to fix.</param>
    ''' <remarks>
    '''  The method adjusts the SplitterDistance based on the orientation and fixed panel of the SplitContainer.
    ''' </remarks>
    Private Sub Fix(sp As SplitContainer)
        ' Scale factor depends on orientation
        Dim sc As Single = If(sp.Orientation = Orientation.Vertical,
                              _formScale.Width,
                              _formScale.Height)
        If sp.FixedPanel = FixedPanel.Panel1 Then
            sp.SplitterDistance = CInt(Math.Truncate(Math.Round(sp.SplitterDistance * sc)))
        ElseIf sp.FixedPanel = FixedPanel.Panel2 Then
            Dim cs As Integer = If(sp.Orientation = Orientation.Vertical,
                                   sp.Panel2.ClientSize.Width,
                                   sp.Panel2.ClientSize.Height)
            sp.SplitterDistance -= CInt(Math.Truncate(cs * sc)) - cs
        End If
    End Sub

    ''' <summary>
    '''  Recursively fixes the SplitContainer controls within the specified control.
    '''  This method is called to ensure that all SplitContainer controls in the control hierarchy are fixed.
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
    '''  Returns a subtitle string for the summary chart based on the current AutoMode state and basal pattern.
    '''  If in AutoMode, the subtitle reflects the automode state and remaining time if in Safe Basal.
    '''  Otherwise, it returns the active basal pattern and rate.
    ''' </summary>
    ''' <returns>
    '''  A string representing the current subtitle for the summary chart.
    ''' </returns>
    Private Function GetSubTitle() As String
        Dim title As String = ""
        If InAutoMode Then
            Dim automodeState As String = s_therapyAlgorithmStateValue(NameOf(TherapyAlgorithmState.AutoModeShieldState))
            Select Case automodeState
                Case "AUTO_BASAL"
                    title = If(Is700Series(), "AutoMode", "SmartGuard")
                Case "SAFE_BASAL"
                    title = automodeState.ToTitle
                    Dim safeBasalDuration As UInteger = CUInt(s_therapyAlgorithmStateValue(NameOf(TherapyAlgorithmState.SafeBasalDuration)))
                    If safeBasalDuration > 0 Then
                        title &= $", {TimeSpan.FromMinutes(safeBasalDuration):h\:mm} left."
                    End If
            End Select
        Else
            Return $"{s_basalList.ActiveBasalPattern} rate = {s_basalList.GetBasalPerHour} U Per Hour".CleanSpaces
        End If
        Return title
    End Function

    ''' <summary>
    '''  Updates the Active Insulin value display on the main form and mini display.
    ''' </summary>
    ''' <remarks>
    '''  If the active insulin value is available and non-negative, it is formatted and displayed.
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
            If PatientData.ActiveInsulin IsNot Nothing AndAlso PatientData.ActiveInsulin.Amount >= 0 Then
                Dim activeInsulinStr As String = $"Active Insulin {$"{PatientData.ActiveInsulin.Amount:N3}"} U"
                Me.ActiveInsulinValue.Text = activeInsulinStr
                _sgMiniDisplay.ActiveInsulinTextBox.Text = activeInsulinStr
            Else
                Me.ActiveInsulinValue.Text = $"Active Insulin Unknown"
                _sgMiniDisplay.ActiveInsulinTextBox.Text = $"Active Insulin --- U"
            End If
        Catch ex As ArithmeticException
            Stop
            Throw New ArithmeticException(message:=$"{ex.DecodeException()} exception in {NameOf(UpdateActiveInsulin)}")
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
    '''  Clears and repopulates the chart series with new data points based on the current markers and user settings.
    '''  Calculates active insulin on board (IOB) for each 5-minute interval, using all relevant markers.
    '''  Handles auto basal, manual basal, insulin, and low glucose suspended markers.
    '''  Updates the chart's Y axis maximum, plots suspend areas, markers, sensor glucose series, and high/low/target limits.
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

                ' Order all markers by time
                Dim timeOrderedMarkers As New SortedDictionary(Of OADate, Single)

                Dim lastTimeChangeRecord As TimeChange = Nothing
                If s_markers.Count = 0 Then
                    Exit Sub
                End If

                For Each markerWithIndex As IndexClass(Of Marker) In s_markers.WithIndex()
                    Dim item As Marker = markerWithIndex.Value
                    Dim markerOADateTime As New OADate(asDate:=item.GetMarkerTimestamp)

                    Dim key As String
                    Select Case item.Type
                        Case "AUTO_BASAL_DELIVERY"
                            key = NameOf(AutoBasalDelivery.BolusAmount)
                            Dim bolusAmount As Single = item.GetSingleFromJson(key)
                            If timeOrderedMarkers.ContainsKey(key:=markerOADateTime) Then
                                timeOrderedMarkers(key:=markerOADateTime) += bolusAmount
                            Else
                                timeOrderedMarkers.Add(key:=markerOADateTime, value:=bolusAmount)
                            End If
                        Case "MANUAL_BASAL_DELIVERY"
                            key = NameOf(AutoBasalDelivery.BolusAmount)
                            Dim bolusAmount As Single = item.GetSingleFromJson(key)
                            If timeOrderedMarkers.ContainsKey(key:=markerOADateTime) Then
                                timeOrderedMarkers(key:=markerOADateTime) += bolusAmount
                            Else
                                timeOrderedMarkers.Add(key:=markerOADateTime, value:=bolusAmount)
                            End If
                        Case "INSULIN"
                            key = NameOf(Insulin.DeliveredFastAmount)
                            Dim bolusAmount As Single = item.GetSingleFromJson(key)
                            If timeOrderedMarkers.ContainsKey(key:=markerOADateTime) Then
                                timeOrderedMarkers(key:=markerOADateTime) += bolusAmount
                            Else
                                timeOrderedMarkers.Add(key:=markerOADateTime, value:=bolusAmount)
                            End If
                        Case "LOW_GLUCOSE_SUSPENDED"
                            If PatientData.ConduitSensorInRange AndAlso CurrentPdf?.IsValid AndAlso Not InAutoMode Then
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
                    Dim initialInsulinLevel As Single = 0
                    Dim firstNotSkippedOaTime As _
                        New OADate(asDate:=(s_sgRecords(index:=0).Timestamp + (FiveMinuteSpan * i)).RoundDownToMinute())
                    While currentMarker < timeOrderedMarkers.Count AndAlso
                        timeOrderedMarkers.Keys(index:=currentMarker) <= firstNotSkippedOaTime

                        initialInsulinLevel += timeOrderedMarkers.Values(index:=currentMarker)
                        currentMarker += 1
                    End While
                    Dim item As New RunningActiveInsulin(firstNotSkippedOaTime, initialInsulinLevel, CurrentUser)
                    remainingInsulinList.Add(item)
                Next

                .ChartAreas(name:=NameOf(ChartArea)).AxisY2.Maximum = GetYMaxValueFromNativeMmolL()
                ' walk all markers, adjust active insulin and then add new markerWithIndex
                Dim maxActiveInsulin As Double = 0
                For i As Integer = 0 To remainingInsulinList.Count - 1
                    If i < CurrentUser.GetActiveInsulinIncrements Then
                        With Me.ActiveInsulinActiveInsulinSeries
                            .Points.AddXY(xValue:=remainingInsulinList(index:=i).OaDateTime, yValue:=Double.NaN)
                            .Points.Last.IsEmpty = True
                        End With
                        If i > 0 Then
                            remainingInsulinList.AdjustList(start:=0, count:=i)
                        End If
                        Continue For
                    End If
                    Dim start As Integer = i - CurrentUser.GetActiveInsulinIncrements + 1
                    Dim sum As Double = remainingInsulinList.ConditionalSum(start, count:=CurrentUser.GetActiveInsulinIncrements)
                    maxActiveInsulin = Math.Max(sum, maxActiveInsulin)
                    Me.ActiveInsulinActiveInsulinSeries.Points.AddXY(xValue:=remainingInsulinList(i).OaDateTime, yValue:=sum)
                    remainingInsulinList.AdjustList(start, CurrentUser.GetActiveInsulinIncrements)
                Next

                .ChartAreas(NameOf(ChartArea)).AxisY.Maximum = Math.Ceiling(maxActiveInsulin) + 1
                .PlotSuspendArea(SuspendSeries:=Me.ActiveInsulinSuspendSeries)
                .PlotMarkers(
                    timeChangeSeries:=Me.ActiveInsulinTimeChangeSeries,
                    markerInsulinDictionary:=s_activeInsulinMarkerInsulinDictionary,
                    markerMealDictionary:=Nothing)
                .PlotSgSeries(GetYMinValueFromNativeMmolL())
                .PlotHighLowLimitsAndTargetSg(targetSsOnly:=True)
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
                    markerInsulinDictionary:=s_summaryMarkerInsulinDictionary,
                    markerMealDictionary:=s_summaryMarkerMealDictionary)
                .PlotSgSeries(HomePageMealRow:=GetYMinValueFromNativeMmolL())
                .PlotHighLowLimitsAndTargetSg(targetSsOnly:=False)
                Application.DoEvents()
            End With
        Catch innerException As Exception
            Stop
            Throw New ApplicationException(
                message:=$"{innerException.DecodeException()} exception while plotting Markers in {NameOf(UpdateAllSummarySeries)}",
                innerException)
        End Try

    End Sub

    ''' <summary>
    '''  Updates the Auto Mode shield display on the home tab.
    '''  This method updates the shield image, last sensor glucose time, and shield units label based on the current sensor state.
    ''' </summary>
    ''' <remarks>
    '''  The shield image is set based on the sensor state, and the last sensor glucose time and shield units are displayed accordingly.
    ''' </remarks>
    Private Sub UpdateAutoModeShield()
        Try
            Me.LastSgOrExitTimeLabel.Text = s_lastSg.Timestamp.ToString(format:=s_timeWithMinuteFormat)
            Me.ShieldUnitsLabel.Text = GetBgUnitsString()

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
                        Stop
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
                _sgMiniDisplay.SetCurrentSgString(sgString, sgValue:=s_lastSg.sg)
                message = SG.TranslateAndTruncateSensorMessage(key:=PatientData.SensorState, truncate:=True)
            Else
                _sgMiniDisplay.SetCurrentSgString(sgString:="---", sgValue:=s_lastSg.sg)
                Me.CurrentSgLabel.Visible = False
                Me.LastSgOrExitTimeLabel.Visible = False
                Me.SensorMessageLabel.Visible = True
                Me.SensorMessageLabel.BackColor = Color.Transparent

                message = SG.TranslateAndTruncateSensorMessage(key:=PatientData.SensorState, truncate:=True)
                Select Case PatientData.SensorState
                    Case "UNKNOWN"
                        Me.SensorMessageLabel.Text = message
                        Me.SensorMessageLabel.CenterXYOnParent(verticalOffset:=-5)
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
                            Me.SensorMessageLabel.Text = $"{message.Remove(oldValue:="...")}{vbCrLf}{timeRemaining}"
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
    '''  This method updates the calibration due image based on the current sensor state and time to next calibration.
    ''' </summary>
    ''' <remarks>
    '''  The calibration due image is set based on the time remaining for calibration and the sensor state.
    '''  If the sensor is in range, the image is updated to reflect the calibration status:
    '''  - If the time to next calibration is unknown (>= Byte.MaxValue), a default arc is shown.
    '''  - If calibration is due now (0 hours), the image reflects whether calibration is not ready or required.
    '''  - If the time to next calibration is -1, the image is cleared.
    '''  - Otherwise, the image shows a progress arc for the remaining minutes.
    '''  The image is only visible if the sensor is in range.
    ''' </remarks>
    Private Sub UpdateCalibrationTimeRemaining()
        Try
            If PatientData.ConduitInRange Then
                If PatientData.TimeToNextCalibHours >= Byte.MaxValue Then
                    Me.CalibrationDueImage.Image = My.Resources.CalibrationDot.DrawCenteredArc(minutesToNextCalibration:=720)
                ElseIf PatientData.TimeToNextCalibHours = 0 Then
                    Me.CalibrationDueImage.Image = If(PatientData.SystemStatusMessage = "WAIT_TO_CALIBRATE" OrElse PatientData.SensorState = "WARM_UP" OrElse PatientData.SensorState = "CHANGE_SENSOR",
                    My.Resources.CalibrationNotReady,
                    My.Resources.CalibrationDotRed.DrawCenteredArc(minutesToNextCalibration:=s_timeToNextCalibrationMinutes))
                ElseIf s_timeToNextCalibrationMinutes = -1 Then
                    Me.CalibrationDueImage.Image = Nothing
                Else
                    Me.CalibrationDueImage.Image = My.Resources.CalibrationDot.DrawCenteredArc(minutesToNextCalibration:=s_timeToNextCalibrationMinutes)
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
    '''  This method calculates and displays the total insulin doses, basal rates, auto corrections, manual boluses, and carbs for the last 24 hours.
    ''' </summary>
    ''' <remarks>
    '''  The method iterates through markers to calculate total doses and updates the corresponding labels on the form.
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
                    For Each e As IndexClass(Of BasalRateRecord) In activeBasalRecords.WithIndex
                        Dim basalRate As BasalRateRecord = e.Value
                        startTime = basalRate.Time
                        endTime = If(e.IsLast,
                                     New TimeOnly(hour:=23, minute:=59, second:=59, millisecond:=999),
                                     activeBasalRecords(index:=e.Index + 1).Time
                                    )
                        Dim theTimeSpan As TimeSpan = endTime - startTime
                        Dim periodInHours As Double = theTimeSpan.Hours + (theTimeSpan.Minutes / 60) + (theTimeSpan.Seconds / 3600)
                        s_totalBasal += CSng(periodInHours * basalRate.UnitsPerHr)
                    Next
                    s_totalDailyDose += s_totalBasal
                End If
            End If
        End If

        Dim totalPercent As String = If(s_totalDailyDose = 0, "???", $"{CInt(s_totalBasal / s_totalDailyDose * 100)}")
        Me.Last24HrBasalUnitsLabel.Text = String.Format(Provider, format:=$"{s_totalBasal:F1} U")
        Me.Last24HrBasalPercentLabel.Text = $"{totalPercent}%"

        Me.Last24HrTotalInsulinUnitsLabel.Text = String.Format(Provider, format:=$"{s_totalDailyDose:F1} U")

        If s_totalAutoCorrection > 0 Then
            If s_totalDailyDose > 0 Then
                totalPercent = CInt(s_totalAutoCorrection / s_totalDailyDose * 100).ToString
            End If
            Me.Last24HrAutoCorrectionLabel.Visible = True
            Me.Last24HrAutoCorrectionUnitsLabel.Text = String.Format(Provider, format:=$"{s_totalAutoCorrection:F1} U")
            Me.Last24HrAutoCorrectionUnitsLabel.Visible = True
            Me.Last24HrAutoCorrectionPercentLabel.Text = $"{totalPercent}%"
            Me.Last24HrAutoCorrectionPercentLabel.Visible = True
            If s_totalDailyDose > 0 Then
                totalPercent = CInt(s_totalManualBolus / s_totalDailyDose * 100).ToString
            End If
            Me.Last24HrMealBolusUnitsLabel.Text = String.Format(Provider, format:=$"{s_totalManualBolus:F1} U")
            Me.Last24HrMealBolusPercentLabel.Text = $"{totalPercent}%"
        Else
            Me.Last24HrAutoCorrectionLabel.Visible = False
            Me.Last24HrAutoCorrectionUnitsLabel.Visible = False
            Me.Last24HrAutoCorrectionPercentLabel.Visible = False
            If s_totalDailyDose > 0 Then
                totalPercent = CInt(s_totalManualBolus / s_totalDailyDose * 100).ToString
            End If
            Me.Last24HrMealBolusUnitsLabel.Text = String.Format(Provider, format:=$"{s_totalManualBolus:F1} U")
            Me.Last24HrMealBolusPercentLabel.Text = $"{totalPercent}%"
        End If
        Me.Last24HrCarbsValueLabel.Text = $"{s_totalCarbs} {GetCarbDefaultUnit()}{Superscript3}"
    End Sub

    ''' <summary>
    '''  Updates the insulin level display on the home tab.
    '''  This method updates the insulin level picture box and remaining insulin units label based on the current reservoir level.
    ''' </summary>
    ''' <remarks>
    '''  The insulin level picture box is set based on the reservoir level percentage, and the remaining insulin units are displayed accordingly.
    ''' </remarks>
    Private Sub UpdateInsulinLevel()

        Me.InsulinLevelPictureBox.SizeMode = PictureBoxSizeMode.StretchImage
        If Not PatientData.ConduitInRange Then
            Me.InsulinLevelPictureBox.Image = Me.ImageList1.Images(index:=8)
            Me.RemainingInsulinUnits.Text = "???U"
        Else
            Dim key As String = NameOf(ServerDataIndexes.reservoirRemainingUnits)
            Me.RemainingInsulinUnits.Text = $"{s_listOfSummaryRecords.GetValue(Of String)(key).ParseSingle(digits:=1):N1} U"
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
        Application.DoEvents()
    End Sub

    ''' <summary>
    '''  Updates the pump battery status display on the home tab.
    '''  This method updates the pump battery picture box and remaining battery percentage label based on the current pump battery level.
    ''' </summary>
    ''' <remarks>
    '''  The pump battery picture box is set based on the battery level percentage, and the remaining battery percentage is displayed accordingly.
    ''' </remarks>
    Private Sub UpdatePumpBattery()
        If Not PatientData.ConduitInRange Then
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

    ''' <summary>
    '''  Updates the sensor life display on the home tab.
    '''  This method updates the sensor days left label and sensor time left picture box based on the current sensor duration hours.
    ''' </summary>
    ''' <remarks>
    '''  The sensor days left label is set based on the remaining sensor duration, and the sensor time left picture box is updated accordingly.
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
                    Dim sensorDurationMinutes As Integer = s_listOfSummaryRecords.GetValue(
                        key:=NameOf(ServerDataIndexes.sensorDurationMinutes),
                        throwError:=False,
                        defaultValue:=-1)

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
        Me.SensorTimeLeftPanel.Visible = PatientData.ConduitInRange
    End Sub

    ''' <summary>
    '''  Updates the Time in Range chart with the latest data and settings.
    ''' </summary>
    ''' <remarks>
    '''  Clears and repopulates the chart series with new data points based on the current patient data and user settings.
    ''' </remarks>
    Private Sub UpdateTimeInRange()
        If Me.TimeInRangeChart Is Nothing Then
            Stop
            Exit Sub
        End If

        _timeInTightRange = GetTIR(tight:=True)
        Me.TimeInRangeChartLabel.Text = GetTIR.asString
        With Me.TimeInRangeChart
            With .Series(name:=NameOf(TimeInRangeSeries)).Points
                .Clear()
                .AddXY(
                    $"{GetBelowHypoLimit.Str}% Below {GetTirLowLimitWithUnits()}",
                    PatientData.BelowHypoLimit.GetRoundedValue(digits:=1) / 100)
                .Last().Color = Color.Red
                .Last().BorderColor = Color.Black
                .Last().BorderWidth = 2
                .AddXY(
                    $"{GetAboveHyperLimit.Str}% Above {GetTirHighLimitWithUnits()}",
                    PatientData.AboveHyperLimit.GetRoundedValue(digits:=1) / 100)
                .Last().Color = Color.Yellow
                .Last().BorderColor = Color.Black
                .Last().BorderWidth = 2
                Dim tir As UInteger = GetTIR.percent
                If _timeInTightRange.Uint = tir Then
                    .AddXY($"{_timeInTightRange.Str}% In Tight Range = TIR", _timeInTightRange.Uint / 100)
                    .Last().Color = Color.LimeGreen
                    .Last().BorderColor = Color.Black
                    .Last().BorderWidth = 2
                ElseIf _timeInTightRange.Uint < tir Then
                    .AddXY($"{GetTIR.asString}% In Range", (tir - _timeInTightRange.Uint) / 100)
                    .Last().Color = Color.Green
                    .Last().BorderColor = Color.Black
                    .Last().BorderWidth = 2

                    .AddXY($"{_timeInTightRange.Str}% In Tight Range", _timeInTightRange.Uint / 100)
                    .Last().Color = Color.LimeGreen
                    .Last().BorderColor = Color.Black
                    .Last().BorderWidth = 2
                Else
                    .AddXY($"{_timeInTightRange.Str}% In Tight Range", _timeInTightRange.Uint / 100)
                    .Last().Color = Color.LimeGreen
                    .Last().BorderColor = Color.Black
                    .Last().BorderWidth = 2

                    .AddXY($"{GetTIR.asString}% In Range", (_timeInTightRange.Uint - tir) / 100)
                    .Last().Color = Color.Green
                    .Last().BorderColor = Color.Black
                    .Last().BorderWidth = 2

                End If
            End With
            .Series(NameOf(TimeInRangeSeries))("PieLabelStyle") = "Disabled"
            .Series(NameOf(TimeInRangeSeries))("PieStartAngle") = "270"
        End With

        Me.AboveHighLimitValueLabel.Text = $"{GetAboveHyperLimit.Str}%"
        Me.AboveHighLimitMessageLabel.Text = $"Above {GetTirHighLimitWithUnits()} {GetBgUnitsString()}"

        Me.TimeInRangeValueLabel.Text = $"{GetTIR.asString}%"
        If GetTIR.percent >= 70 Then
            Me.TimeInRangeMessageLabel.ForeColor = Color.DarkGreen
            Me.TimeInRangeValueLabel.ForeColor = Color.DarkGreen
        Else
            Me.TimeInRangeMessageLabel.ForeColor = Color.Red
            Me.TimeInRangeValueLabel.ForeColor = Color.Red
        End If

        Me.TimeInTightRangeValueLabel.Text = $"{_timeInTightRange.Str}%"
        Me.TiTRMgsLabel2.Text = My.Forms.OptionsConfigureTiTR.GetTiTrMsg()
        If _timeInTightRange.Uint >= OptionsConfigureTiTR.TreatmentTargetPercent Then
            Me.TiTRMgsLabel.ForeColor = Color.LimeGreen
            Me.TiTRMgsLabel2.ForeColor = Color.LimeGreen
            Me.TimeInTightRangeValueLabel.ForeColor = Color.LimeGreen
        Else
            Me.TiTRMgsLabel.ForeColor = Color.Red
            Me.TiTRMgsLabel2.ForeColor = Color.Red
            Me.TimeInTightRangeValueLabel.ForeColor = Color.Red
        End If

        Me.BelowLowLimitValueLabel.Text = $"{GetBelowHypoLimit.Str}%"
        Me.BelowLowLimitMessageLabel.Text = $"Below {GetTirLowLimitWithUnits()} {GetBgUnitsString()}"
        Dim averageSgStr As String = RecentData.GetStringValueOrEmpty(NameOf(ServerDataIndexes.averageSG))
        Me.AverageSGValueLabel.Text = If(NativeMmolL,
                                         averageSgStr.TruncateSingleString(digits:=2),
                                         averageSgStr)
        Me.AverageSGMessageLabel.Text = $"Average SG in {GetBgUnitsString()}"

        ' Calculate Time in AutoMode
        If s_autoModeStatusMarkers.Count = 0 Then
            Me.SmartGuardLabel.Text = "SmartGuard 0%"
        ElseIf s_autoModeStatusMarkers.Count = 1 AndAlso s_autoModeStatusMarkers.First.AutoModeOn Then
            Me.SmartGuardLabel.Text = "SmartGuard 100%"
        Else
            Try
                ' need to figure out %
                Dim autoModeStartTime As New Date
                Dim timeInAutoMode As TimeSpan = ZeroTickSpan
                For Each r As IndexClass(Of AutoModeStatus) In s_autoModeStatusMarkers.WithIndex
                    If r.IsFirst Then
                        If r.Value.AutoModeOn OrElse s_autoModeStatusMarkers.Count = 1 Then
                            autoModeStartTime = r.Value.Timestamp
                            timeInAutoMode += s_autoModeStatusMarkers.First.Timestamp.AddDays(value:=1) - autoModeStartTime
                        Else

                        End If
                    Else
                        If r.Value.AutoModeOn Then
                            If r.IsLast Then
                                timeInAutoMode += s_autoModeStatusMarkers.First.Timestamp.AddDays(value:=1) - r.Value.Timestamp
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
        Dim highScale As Single = (GetYMaxValueFromNativeMmolL() - GetTirHighLimit()) / (GetTirLowLimit() - GetYMinValueFromNativeMmolL())
        For Each sg As SG In s_sgRecords.Where(predicate:=Function(entry As SG)
                                                              Return entry.sg.IsSgValid
                                                          End Function)
            elements += 1
            If sg.sgMgdL < 70 Then
                lowCount += 1
                If NativeMmolL Then
                    lowDeviations += ((GetTirLowLimit() - sg.sgMmolL) * MmolLUnitsDivisor) ^ 2
                Else
                    lowDeviations += (GetTirLowLimit() - sg.sgMgdL) ^ 2
                End If
            ElseIf sg.sgMgdL > 180 Then
                highCount += 1
                If NativeMmolL Then
                    highDeviations += ((sg.sgMmolL - GetTirHighLimit()) * MmolLUnitsDivisor) ^ 2
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
            Dim lowDeviation As Single = Math.Sqrt(lowDeviations / (elements - highCount)).RoundToSingle(digits:=1, considerValue:=False)
            Select Case True
                Case lowDeviation <= 2
                    Me.LowTirComplianceLabel.Text = $"Low{vbCrLf}Excellent{Superscript2}"
                    Me.LowTirComplianceLabel.ForeColor = Color.LimeGreen
                Case lowDeviation <= 4
                    Me.LowTirComplianceLabel.Text = $"Low{vbCrLf}({lowDeviation}) OK{Superscript2}"
                    Me.LowTirComplianceLabel.ForeColor = Color.Yellow
                Case Else
                    Me.LowTirComplianceLabel.Text = $"Low{vbCrLf}({lowDeviation}) Needs{vbCrLf}Improvement{Superscript2}"
                    Me.LowTirComplianceLabel.ForeColor = Color.Red
            End Select

            Dim highDeviation As Single = Math.Sqrt(highDeviations / (elements - lowCount)).RoundToSingle(digits:=1, considerValue:=False)
            Select Case True
                Case highDeviation <= 2
                    Me.HighTirComplianceLabel.Text = $"High{vbCrLf}Excellent{Superscript2}"
                    Me.HighTirComplianceLabel.ForeColor = Color.LimeGreen
                Case highDeviation <= 4
                    Me.HighTirComplianceLabel.Text = $"High{vbCrLf}({highDeviation}) OK{Superscript2}"
                    Me.HighTirComplianceLabel.ForeColor = Color.Yellow
                Case Else
                    Me.HighTirComplianceLabel.Text =
                        $"High{vbCrLf}({highDeviation}) Needs{vbCrLf}Improvement{Superscript2}"
                    Me.HighTirComplianceLabel.ForeColor = Color.Red
            End Select
        End If
        Me.PositionControlsInPanel()
    End Sub

    ''' <summary>
    '''  Positions the controls in the panel of the home tab.
    ''' </summary>
    ''' <remarks>
    '''  This method centers the labels in the panel based on their names and adjusts their positions accordingly.
    ''' </remarks>
    Private Sub PositionControlsInPanel()
        For Each ctrl As Control In Me.SplitContainer3.Panel2.Controls
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
                            .Left = ctrl.Parent.Width - .Width - .Margin.Right
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
    '''  This method reinitializes the Treatment Markers chart, updates its title, axes, and series.
    '''  It plots suspend areas, treatment markers, sensor glucose series, and high/low/target limits.
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
                .PlotSuspendArea(SuspendSeries:=Me.TreatmentMarkerSuspendSeries)
                .PlotTreatmentMarkers(Me.TreatmentMarkerTimeChangeSeries)
                .PlotSgSeries(HomePageMealRow:=GetYMinValueFromNativeMmolL())
                .PlotHighLowLimitsAndTargetSg(targetSsOnly:=True)
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
    '''  This method updates the trend arrows label based on the current sensor glucose trend value.
    ''' </summary>
    ''' <remarks>
    '''  The trend arrows label is set based on the last sensor glucose trend value and is styled accordingly.
    ''' </remarks>
    Private Sub UpdateTrendArrows()
        Dim rowValue As String = RecentData.GetStringValueOrEmpty(NameOf(ServerDataIndexes.lastSGTrend))
        If PatientData.ConduitInRange Then
            Dim arrows As String = Nothing
            If s_trends.TryGetValue(rowValue, arrows) Then
                Me.TrendArrowsLabel.Font = If(rowValue = "NONE",
                    New Font(FamilyName, emSize:=18.0F, style:=FontStyle.Bold),
                    New Font(FamilyName, emSize:=14.25F, style:=FontStyle.Bold))
                Me.TrendArrowsLabel.Text = s_trends(rowValue)
            Else
                Me.TrendArrowsLabel.Font = New Font(FamilyName, emSize:=14.25F, style:=FontStyle.Bold)
                Me.TrendArrowsLabel.Text = rowValue
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
    '''  This method updates various components of the main form, including data tables, charts, and labels,
    '''  based on the current patient data and system status.
    ''' </remarks>
    Friend Sub UpdateAllTabPages(fromFile As Boolean)
        If RecentDataEmpty() Then
            DebugPrint($"exiting, {NameOf(RecentData)} has no data!")
            Exit Sub
        End If
        Dim lastMedicalDeviceDataUpdateServerTimeEpoch As String = ""
        Dim key As String = NameOf(ServerDataIndexes.lastMedicalDeviceDataUpdateServerTime)
        If RecentData.TryGetValue(key, value:=lastMedicalDeviceDataUpdateServerTimeEpoch) Then
            If CLng(lastMedicalDeviceDataUpdateServerTimeEpoch) = s_lastMedicalDeviceDataUpdateServerEpoch Then
                Exit Sub
            Else
                s_lastMedicalDeviceDataUpdateServerEpoch = CLng(lastMedicalDeviceDataUpdateServerTimeEpoch)
            End If
        End If

        If RecentData.Count > ServerDataIndexes.sensorLifeIcon + 1 Then
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
                    msg:=$"Last Update Time: {PumpNow.ToShortDateTimeString}",
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

        FinishInitialization(Me)
        Me.UpdateTrendArrows()
        UpdateSummaryTab(dgv:=Me.DgvSummary, classCollection:=s_listOfSummaryRecords, sort:=True)
        Me.UpdateActiveInsulin()
        Me.UpdateAutoModeShield()
        Me.UpdateCalibrationTimeRemaining()
        Me.UpdateInsulinLevel()
        Me.UpdatePumpBattery()
        Me.UpdateSensorLife()
        UpdateTransmitterBattery()
        Me.UpdateTimeInRange()
        Me.UpdateAllSummarySeries()
        Me.UpdateDosingAndCarbs()

        key = NameOf(ServerDataIndexes.lastName)
        Me.FullNameLabel.Text = $"{PatientData.FirstName} {RecentData.GetStringValueOrEmpty(key)}"

        Dim mdi As MedicalDeviceInformation = PatientData.MedicalDeviceInformation
        Me.ModelLabel.Text = $"{mdi.ModelNumber} HW Version = {mdi.HardwareRevision}"
        Me.PumpNameLabel.Text = GetPumpName(mdi.ModelNumber)

        Dim nonZeroRecords As IEnumerable(Of SG) = s_sgRecords.Where(predicate:=Function(entry As SG)
                                                                                    Return Not Single.IsNaN(entry.sg)
                                                                                End Function)
        Me.ReadingsLabel.Text = $"{nonZeroRecords.Count()}/{288} SG Readings"

        Me.TableLayoutPanelLastSG.DisplayDataTableInDGV(
            table:=ClassCollectionToDataTable(classCollection:={s_lastSg}.ToList),
            className:=NameOf(LastSG), rowIndex:=ServerDataIndexes.lastSG,
            hideRecordNumberColumn:=True)

        UpdateSummaryTab(
            dgv:=Me.DgvLastAlarm,
            classCollection:=GetSummaryRecords(jsonDictionary:=s_lastAlarmValue),
            sort:=True)
        Me.DgvLastAlarm.Columns(index:=0).Visible = False

        Me.TableLayoutPanelActiveInsulin.DisplayDataTableInDGV(
            table:=ClassCollectionToDataTable(classCollection:={s_activeInsulin}.ToList),
            className:=NameOf(ActiveInsulin), rowIndex:=ServerDataIndexes.activeInsulin,
            hideRecordNumberColumn:=True)

        Dim keySelector As Func(Of SG, Integer) = Function(x)
                                                      Return x.RecordNumber
                                                  End Function
        Me.TableLayoutPanelSgs.DisplayDataTableInDGV(
            table:=ClassCollectionToDataTable(classCollection:=s_sgRecords.OrderByDescending(keySelector).ToList()),
            dgv:=Me.DgvSGs,
            rowIndex:=ServerDataIndexes.sgs)
        Me.DgvSGs.AutoSize = True
        Me.DgvSGs.Columns(index:=0).HeaderCell.SortGlyphDirection = SortOrder.Descending

        Me.TableLayoutPanelLimits.DisplayDataTableInDGV(
            table:=ClassCollectionToDataTable(classCollection:=s_limitRecords),
            className:=NameOf(Limit), rowIndex:=ServerDataIndexes.limits)

        UpdateSummaryTab(
            dgv:=Me.DgvTherapyAlgorithmState,
            classCollection:=GetSummaryRecords(jsonDictionary:=s_therapyAlgorithmStateValue),
            sort:=False)
        Me.DgvTherapyAlgorithmState.Columns(index:=0).Visible = False

        Me.DgvLastAlarm.Columns(index:=0).Visible = False
        Me.TableLayoutPanelBasal.DisplayDataTableInDGV(
            table:=ClassCollectionToDataTable(s_basalList.ClassCollection),
            className:=NameOf(Basal), rowIndex:=ServerDataIndexes.basal,
            hideRecordNumberColumn:=True)

        UpdateMarkerTabs(mainForm:=Me)

        UpdateNotificationTabs(mainForm:=Me)

        UpdatePumpBannerStateTab(mainForm:=Me)

        Me.MenuStartHere.Enabled = True
        ProgramInitialized = True
        Me.UpdateTreatmentChart()
        Me.UpdateActiveInsulinChart()

        Dim showLegend As Boolean = s_totalAutoCorrection > 0
        ShowHideLegendItem(
            showLegend,
            legendString:="Auto Correction",
            activeInsulinChartLegend:=_activeInsulinChartLegend,
            homeChartLegend:=_summaryChartLegend,
            treatmentMarkersChartLegend:=_treatmentMarkersChartLegend)

        showLegend = s_lowGlucoseSuspendedMarkers.Any(predicate:=Function(s)
                                                                     Return s.deliverySuspended
                                                                 End Function)
        ShowHideLegendItem(
            showLegend,
            legendString:="Suspend",
            activeInsulinChartLegend:=_activeInsulinChartLegend,
            homeChartLegend:=_summaryChartLegend,
            treatmentMarkersChartLegend:=_treatmentMarkersChartLegend)

        If My.Settings.SystemAudioAlertsEnabled AndAlso My.Settings.SystemSpeechRecognitionThreshold <> 1 Then
            InitializeSpeechRecognition()
        Else
            CancelSpeechRecognition()
        End If
        Application.DoEvents()
    End Sub

#End Region ' Update Home Tab

End Class
