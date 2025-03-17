' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Module PumpVariables

    ' Manually computed

    Friend s_totalAutoCorrection As Single
    Friend s_totalBasal As Single
    Friend s_totalCarbs As Single
    Friend s_totalDailyDose As Single
    Friend s_totalManualBolus As Single

    Friend s_aboveHyperLimit As Single
    Friend s_activeInsulin As ActiveInsulin
    Friend s_autoModeReadinessState As SummaryRecord
    Friend s_basal As Basal
    Friend s_belowHypoLimit As Single
    Friend s_filterJsonData As Boolean = True
    Friend s_firstName As String = ""
    Friend s_gstCommunicationState As Boolean
    Friend s_lastAlarmValue As Dictionary(Of String, String)
    Friend s_lastMedicalDeviceDataUpdateServerEpoch As Long
    Friend s_lastSg As New LastSG
    Friend s_lastSgValue As Single = 0
    Friend s_listOfSgRecords As New List(Of SG)
    Friend s_listOfTimeChangeMarkers As New List(Of TimeChange)
    Friend s_markers As New List(Of Marker)
    Friend s_notificationHistoryValue As Dictionary(Of String, String)
    Friend s_pumpBannerStateValue As New List(Of Dictionary(Of String, String))
    Friend s_pumpHardwareRevision As String
    Friend s_pumpInRangeOfPhone As Boolean
    Friend s_pumpInRangeOfTransmitter As Boolean
    Friend s_modelNumber As String
    Friend s_reservoirLevelPercent As Integer
    Friend s_sensorDurationHours As Integer
    Friend s_sensorState As String
    Friend s_systemStatusMessage As String
    Friend s_systemStatusTimeRemaining As TimeSpan
    Friend s_therapyAlgorithmStateValue As Dictionary(Of String, String)
    Friend s_timeFormat As String
    Friend s_timeInRange As Integer
    Friend s_timeToNextCalibrationHours As Short
    Friend s_timeToNextCalibrationMinutes As Short
    Friend s_timeWithMinuteFormat As String
    Friend s_timeWithoutMinuteFormat As String

    Friend Property BgUnitsNativeString As String
        Get
            Return If(s_bgUnitsNativeString = "MGDL", "mg/dL", "Mmol/l")
        End Get
        Set
            s_bgUnitsNativeString = Value
        End Set
    End Property

    Friend Property InAutoMode As Boolean
    Public Property PatientData As PatientDataInfo
    Public Property ProgramInitialized As Boolean = False
    Public Property RecentData As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)

    Private s_bgUnitsNativeString As String
    Friend ReadOnly s_listOfAutoBasalDeliveryMarkers As New List(Of AutoBasalDelivery)
    Friend ReadOnly s_listOfAutoModeStatusMarkers As New List(Of AutoModeStatus)
    Friend ReadOnly s_listOfCalibrationMarkers As New List(Of Calibration)
    Friend ReadOnly s_listOfInsulinMarkers As New List(Of Insulin)
    Friend ReadOnly s_listOfLowGlucoseSuspendedMarkers As New List(Of LowGlucoseSuspended)
    Friend ReadOnly s_listOfMealMarkers As New List(Of Meal)
    Friend ReadOnly s_listOfBgReadingMarkers As New List(Of BgReading)
    Friend ReadOnly s_listOfSummaryRecords As New List(Of SummaryRecord)
    Friend ReadOnly s_listOfUserSummaryRecord As New List(Of SummaryRecord)
    Friend s_listOfLimitRecords As New List(Of Limit)

    Public Function GetCarbDefaultUnit() As String
        Return "Grams"
    End Function

End Module
