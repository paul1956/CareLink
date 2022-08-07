' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Module PumpVariables

    Friend ReadOnly s_ListOfTimeItems As New List(Of Integer) From {
                        ItemIndexs.lastSensorTS,
                        ItemIndexs.lastConduitTime,
                        ItemIndexs.medicalDeviceTime,
                        ItemIndexs.lastSensorTime}

    Friend s_timeZoneList As List(Of TimeZoneInfo)

    ' Manually computed
    Friend s_totalAutoCorrection As Single

    Friend s_totalBasal As Single
    Friend s_totalCarbs As Double
    Friend s_totalDailyDose As Single
    Friend s_totalManualBolus As Single

#Region "Global variables to hold pump values"

    Private _scaleUnitsDivisor As Double
    Friend s_aboveHyperLimit As Integer
    Friend s_activeInsulin As Dictionary(Of String, String)
    Friend s_averageSG As Double
    Friend s_belowHypoLimit As Integer
    Friend s_clientTimeZone As TimeZoneInfo
    Friend s_clientTimeZoneName As String
    Friend s_conduitSensorInRange As Boolean
    Friend s_criticalLow As Single
    Friend s_gstBatteryLevel As Integer
    Friend s_insulinRow As Single
    Friend s_lastSG As Dictionary(Of String, String)
    Friend s_limitHigh As Single
    Friend s_limitLow As Single
    Friend s_limits As New List(Of Dictionary(Of String, String))
    Friend s_markerRow As Single
    Friend s_markers As New List(Of Dictionary(Of String, String))
    Friend s_sensorState As String
    Friend s_systemStatusMessage As String
    Friend s_timeWithMinuteFormat As String
    Friend s_timeWithoutMinuteFormat As String

    Friend Property InsulinRow As Single
        Get
            If s_insulinRow = 0 Then
                Throw New ArgumentNullException(NameOf(s_insulinRow))
            End If
            Return s_insulinRow
        End Get
        Set
            s_insulinRow = Value
        End Set
    End Property

    Friend Property MarkerRow As Single
        Get
            If s_markerRow = 0 Then
                Throw New ArgumentNullException(NameOf(s_markerRow))
            End If
            Return s_markerRow
        End Get
        Set
            s_markerRow = Value
        End Set
    End Property

    Friend Property scaleUnitsDivisor As Double
        Get
            If _scaleUnitsDivisor = 0 Then
                Stop
            End If
            Return _scaleUnitsDivisor
        End Get
        Set
            _scaleUnitsDivisor = Value
        End Set
    End Property

    Friend Property RecentData As New Dictionary(Of String, String)

    Public Property BgUnitsString As String

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
