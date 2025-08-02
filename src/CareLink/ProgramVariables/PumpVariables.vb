' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json

Public Module PumpVariables

#Region "Lists"

    Friend ReadOnly s_basalPerHour As New List(Of BasalPerHour)
    Friend s_basalList As New List(Of Basal) From {New Basal}
    Friend s_limitRecords As New List(Of Limit)
    Friend s_pumpBannerStateValue As New List(Of Dictionary(Of String, String))
    Friend s_sgRecords As New List(Of SG)

#Region "Markers"

    Friend ReadOnly s_autoBasalDeliveryMarkers As New List(Of AutoBasalDelivery)
    Friend ReadOnly s_autoModeStatusMarkers As New List(Of AutoModeStatus)
    Friend ReadOnly s_bgReadingMarkers As New List(Of BgReading)
    Friend ReadOnly s_calibrationMarkers As New List(Of Calibration)
    Friend ReadOnly s_insulinMarkers As New List(Of Insulin)
    Friend ReadOnly s_listOfSummaryRecords As New List(Of SummaryRecord)
    Friend ReadOnly s_mealMarkers As New List(Of Meal)
    Friend s_lowGlucoseSuspendedMarkers As New List(Of LowGlucoseSuspended)
    Friend s_markers As New List(Of Marker)
    Friend s_timeChangeMarkers As New List(Of TimeChange)

#End Region ' Markers

#End Region ' Lists

    Friend s_activeInsulin As ActiveInsulin
    Friend s_autoModeReadinessState As SummaryRecord
    Friend s_filterJsonData As Boolean = True
    Friend s_lastAlarmValue As Dictionary(Of String, String)
    Friend s_lastMedicalDeviceDataUpdateServerEpoch As Long
    Friend s_lastSg As SG  ' Do not replace this, it is used in the UI
    Friend s_lastSgValue As Single = 0 ' Do not replace this, it is used in the UI
    Friend s_notificationHistoryValue As Dictionary(Of String, String)
    Friend s_suspendedSince As String = "???"
    Friend s_systemStatusTimeRemaining As TimeSpan
    Friend s_therapyAlgorithmStateValue As Dictionary(Of String, String)
    Friend s_timeToNextCalibrationMinutes As Short ' Do not replace this, it is used in the UI
    Friend s_timeWithMinuteFormat As String
    Friend s_timeWithoutMinuteFormat As String

#Region "Manually Computed"

    Friend s_totalAutoCorrection As Single
    Friend s_totalBasal As Single
    Friend s_totalCarbs As Single
    Friend s_totalDailyDose As Single
    Friend s_totalManualBolus As Single

#End Region ' Manually computed

    Public Property InAutoMode As Boolean

    Public Property PatientData As PatientDataInfo

    Public Property PatientDataElement As JsonElement

    Public Property ProgramInitialized As Boolean = False

    Public Property RecentData As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)

    Public Function GetCarbDefaultUnit() As String
        Return "Grams"
    End Function

End Module
