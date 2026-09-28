' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json

Public Module PumpVariables

#Region "Lists"

    Friend ReadOnly Property BasalList As New List(Of Basal) From {New Basal}
    Friend ReadOnly Property BasalPerHourList As New List(Of BasalPerHour)
    Friend Property LimitRecordsList As New List(Of Limit)
    Friend Property s_pumpBannerStateValue As New List(Of Dictionary(Of String, String))
    Friend Property s_sgRecords As New List(Of SG)

#Region "Markers"

    Friend ReadOnly Property AllMarkers As New List(Of Marker)
    Friend ReadOnly Property AutoBasalDeliveryMarkers As New List(Of AutoBasalDelivery)
    Friend ReadOnly Property AutoModeStatusMarkers As New List(Of AutoModeStatus)
    Friend ReadOnly Property BgReadingMarkers As New List(Of BgReading)
    Friend ReadOnly Property CalibrationMarkers As New List(Of Calibration)
    Friend ReadOnly Property InsulinMarkers As New List(Of Insulin)
    Friend ReadOnly Property ListOfSummaryRecords As New List(Of SummaryRecord)
    Friend ReadOnly Property MealMarkers As New List(Of Meal)
    Friend ReadOnly Property OtherMarkers As New List(Of Marker)
    Friend ReadOnly Property SuspendedMarkers As New List(Of LowGlucoseSuspended)
    Friend ReadOnly Property TimeChangeMarkers As New List(Of TimeChange)

#End Region ' Markers

#End Region ' Lists

    Friend s_activeInsulin As ActiveInsulin
    Friend s_autoModeReadinessState As SummaryRecord

    Friend s_lastAlarmValue As Dictionary(Of String, String)

    Friend s_lastMedicalDeviceDataUpdateServerEpoch As Long
    Friend s_lastSgValue As Single = 0 ' Do not replace this, it is used in the UI
    Friend s_notificationHistoryValue As Dictionary(Of String, String)
    Friend s_suspendedSince As String = "???"
    Friend s_systemStatusTimeRemaining As TimeSpan
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

    Public Function GetCarbDefaultUnit() As String
        Return "Grams"
    End Function

End Module
