' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module PumpVariables

    Friend ReadOnly _listOfSingleItems As New List(Of Integer) From {
                        ItemIndexs.lastSG,
                        ItemIndexs.lastAlarm,
                        ItemIndexs.activeInsulin,
                        ItemIndexs.limits,
                        ItemIndexs.markers,
                        ItemIndexs.notificationHistory,
                        ItemIndexs.basal}

    ' Manually computed
    Friend s_totalAutoCorrection As Single
    Friend s_totalBasal As Single
    Friend s_totalDailyDose As Single
    Friend s_totalManualBolus As Single

    ' From Pump
    Public s_aboveHyperLimit As Integer

    Public s_activeInsulin As Dictionary(Of String, String)
    Public s_averageSG As Double
    Public s_averageSGFloat As Double
    Public s_basal As Dictionary(Of String, String)
    Public s_belowHypoLimit As Integer
    Public s_bgUnits As String
    Public s_calFreeSensor As Boolean
    Public s_calibStatus As String
    Public s_clientTimeZoneName As String
    Public s_conduitBatteryLevel As Integer
    Public s_conduitBatteryStatus As String
    Public s_conduitInRange As Boolean
    Public s_conduitMedicalDeviceInRange As Boolean
    Public s_conduitSensorInRange As Boolean
    Public s_conduitSerialNumber As String
    Public s_currentServerTime As String
    Public s_finalCalibration As Boolean
    Public s_firstName As String
    Public s_gstBatteryLevel As Integer
    Public s_gstCommunicationState As Boolean
    Public s_kind As String
    Public s_lastAlarm As Dictionary(Of String, String)
    Public s_lastConduitDateTime As String
    Public s_lastConduitTime As String
    Public s_lastConduitUpdateServerTime As String
    Public s_lastMedicalDeviceDataUpdateServerTime As String
    Public s_lastName As String
    Public s_lastSensorTime As String
    Public s_lastSensorTS As String
    Public s_lastSensorTSAsString As String
    Public s_lastSG As Dictionary(Of String, String)
    Public s_lastSGTrend As String
    Public s_limits As List(Of Dictionary(Of String, String))
    Public s_markers As List(Of Dictionary(Of String, String))
    Public s_maxAutoBasalRate As Double
    Public s_maxBolusAmount As Double
    Public s_medicalDeviceBatteryLevelPercent As Integer
    Public s_medicalDeviceFamily As String
    Public s_medicalDeviceSerialNumber As String
    Public s_medicalDeviceSuspended As Boolean
    Public s_medicalDeviceTime As String
    Public s_medicalDeviceTimeAsString As String
    Public s_pumpBannerState As List(Of Dictionary(Of String, String))
    Public s_pumpCommunicationState As Boolean
    Public s_pumpModelNumber As String
    Public s_reservoirAmount As Double
    Public s_reservoirLevelPercent As Integer
    Public s_reservoirRemainingUnits As Double
    Public s_sensorDurationHours As Integer
    Public s_sensorDurationMinutes As Integer
    Public s_sensorState As String
    Public s_sgBelowLimit As Integer
    Public s_sGs As New List(Of SgRecord)
    Public s_sLastSensorTime As Date
    Public s_sMedicalDeviceTime As Date
    Public s_systemStatusMessage As String
    Public s_therapyAlgorithmState As Dictionary(Of String, String)
    Public s_timeFormat As String
    Public s_timeInRange As Integer
    Public s_timeToNextCalibHours As UShort = UShort.MaxValue
    Public s_timeToNextCalibrationMinutes As Integer
    Public s_timeToNextCalibrationRecommendedMinutes As UShort
    Public s_version As String

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
        finalCalibration = 65
    End Enum

End Module
