' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel

Public Module PumpVariables

    ' Manually computed

    Friend s_totalAutoCorrection As Single
    Friend s_totalBasal As Single
    Friend s_totalCarbs As Single
    Friend s_totalDailyDose As Single
    Friend s_totalManualBolus As Single

#Region "Global variables to hold pump values"

#Region "Used for painting"

    Public ReadOnly s_homeTabMarkerInsulinDictionary As New Dictionary(Of OADate, Single)
    Public ReadOnly s_homeTabMarkerMealDictionary As New Dictionary(Of OADate, Single)
    Public ReadOnly s_treatmentMarkerInsulinDictionary As New Dictionary(Of OADate, Single)
    Public ReadOnly s_treatmentMarkerMealDictionary As New Dictionary(Of OADate, Single)

#End Region ' Used for painting

    Friend Const MmolLUnitsDivisor As Single = 18
    Friend ReadOnly s_listOfAutoBasalDeliveryMarkers As New List(Of AutoBasalDeliveryRecord)
    Friend ReadOnly s_listOfInsulinMarkers As New List(Of InsulinRecord)
    Friend ReadOnly s_listOfSummaryRecords As New List(Of SummaryRecord)
    Friend ReadOnly s_insulinImage As Bitmap = My.Resources.InsulinVial_Tiny
    Friend ReadOnly s_mealImage As Bitmap = My.Resources.MealImage
    Friend s_aboveHyperLimit As Double
    Friend s_activeInsulin As ActiveInsulinRecord
    Friend s_activeInsulinIncrements As Integer
    Friend s_averageSG As String
    Friend s_belowHypoLimit As Single
    Friend s_bindingSourceSGs As New List(Of SgRecord)
    Friend s_clientTimeZone As TimeZoneInfo
    Friend s_clientTimeZoneName As String
    Friend s_conduitSensorInRange As Boolean
    Friend s_criticalLow As Single
    Friend s_filterJsonData As Boolean = True
    Friend s_firstName As String = ""
    Friend s_gstBatteryLevel As Integer
    Friend s_lastBGDiff As Double = 0
    Friend s_lastBGTime As Date
    Friend s_lastBGValue As Single = 0
    Friend s_lastMedicalDeviceDataUpdateServerTime As Long
    Friend s_lastSG As New SgRecord
    Friend s_limitHigh As Single
    Friend s_limitLow As Single
    Friend s_limits As New List(Of Dictionary(Of String, String))
    Friend s_markers As New List(Of Dictionary(Of String, String))
    Friend s_markersTimeChange As New List(Of TimeChangeRecord)
    Friend s_medicalDeviceBatteryLevelPercent As Integer
    Friend s_recentDatalast As Dictionary(Of String, String)
    Friend s_reservoirLevelPercent As Integer
    Friend s_reservoirRemainingUnits As Double
    Friend s_sensorDurationHours As Integer
    Friend s_sensorDurationMinutes As Integer
    Friend s_sensorState As String
    Friend s_sessionCountrySettings As New CountrySettingsRecord
    Friend s_systemStatusMessage As String
    Friend s_timeInRange As Integer
    Friend s_timeToNextCalibHours As UShort = UShort.MaxValue
    Friend s_timeToNextCalibrationMinutes As Integer
    Friend s_timeWithMinuteFormat As String
    Friend s_timeWithoutMinuteFormat As String
    Friend Property BgUnits As String
    Friend Property BgUnitsString As String

#End Region

    ' Do not rename these name are matched used in case sensitive matching
    Public Enum ItemIndexs As Integer
        lastSensorTS = 0
        medicalDeviceTimeAsString = 1
        lastSensorTSAsString = 2
        kind = 3
        version = 4
        pumpModelNumber = 5
        currentServerTime = 6
        lastConduitTime = 7
        lastConduitUpdateServerTime = 8
        lastMedicalDeviceDataUpdateServerTime = 9
        firstName = 10
        lastName = 11
        conduitSerialNumber = 12
        conduitBatteryLevel = 13
        conduitBatteryStatus = 14
        conduitInRange = 15
        conduitMedicalDeviceInRange = 16
        conduitSensorInRange = 17
        medicalDeviceFamily = 18
        sensorState = 19
        medicalDeviceSerialNumber = 20
        medicalDeviceTime = 21
        sMedicalDeviceTime = 22
        reservoirLevelPercent = 23
        reservoirAmount = 24
        reservoirRemainingUnits = 25
        medicalDeviceBatteryLevelPercent = 26
        sensorDurationHours = 27
        timeToNextCalibHours = 28
        calibStatus = 29
        bgUnits = 30
        timeFormat = 31
        lastSensorTime = 32
        sLastSensorTime = 33
        medicalDeviceSuspended = 34
        lastSGTrend = 35
        lastSG = 36
        lastAlarm = 37
        activeInsulin = 38
        sgs = 39
        limits = 40
        markers = 41
        notificationHistory = 42
        therapyAlgorithmState = 43
        pumpBannerState = 44
        basal = 45
        systemStatusMessage = 46
        averageSG = 47
        belowHypoLimit = 48
        aboveHyperLimit = 49
        timeInRange = 50
        pumpCommunicationState = 51
        gstCommunicationState = 52
        gstBatteryLevel = 53
        lastConduitDateTime = 54
        maxAutoBasalRate = 55
        maxBolusAmount = 56
        sensorDurationMinutes = 57
        timeToNextCalibrationMinutes = 58
        clientTimeZoneName = 59
        sgBelowLimit = 60
        averageSGFloat = 61
        timeToNextCalibrationRecommendedMinutes = 62
        calFreeSensor = 63
        finalCalibration = 64
    End Enum

End Module
