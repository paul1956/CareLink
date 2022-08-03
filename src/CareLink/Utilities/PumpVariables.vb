' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module PumpVariables

    Friend ReadOnly s_listOfSingleItems As New List(Of Integer) From {
                        ItemIndexs.lastSG,
                        ItemIndexs.lastAlarm,
                        ItemIndexs.activeInsulin,
                        ItemIndexs.limits,
                        ItemIndexs.markers,
                        ItemIndexs.notificationHistory,
                        ItemIndexs.basal}

    Friend ReadOnly s_ListOfTimeItems As New List(Of Integer) From {
                        ItemIndexs.lastSensorTS,
                        ItemIndexs.lastConduitTime,
                        ItemIndexs.medicalDeviceTime,
                        ItemIndexs.sMedicalDeviceTime,
                        ItemIndexs.lastSensorTime,
                        ItemIndexs.sLastSensorTime,
                        ItemIndexs.lastConduitDateTime}

    ' Manually computed
    Friend s_totalAutoCorrection As Single

    Friend s_totalBasal As Single
    Friend s_totalCarbs As Double
    Friend s_totalDailyDose As Single
    Friend s_totalManualBolus As Single

#Region "Global variables to hold pump values"

    Public s_aboveHyperLimit As Integer
    Public s_activeInsulin As Dictionary(Of String, String)
    Public s_averageSG As Double
    Public s_belowHypoLimit As Integer
    Public s_clientTimeZone As TimeZoneInfo
    Public s_clientTimeZoneName As String
    Public s_conduitSensorInRange As Boolean
    Public S_criticalLow As Single
    Public s_gstBatteryLevel As Integer
    Public s_lastSG As Dictionary(Of String, String)
    Public s_limits As List(Of Dictionary(Of String, String))
    Public s_markers As List(Of Dictionary(Of String, String))
    Public s_sensorState As String
    Public s_systemStatusMessage As String

#End Region

    Public s_timeZoneList As List(Of TimeZoneInfo)

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
