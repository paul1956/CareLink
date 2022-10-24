' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module ItemIndexesModule

    Public ReadOnly s_singleEntries As New List(Of String) From {
                        NameOf(ItemIndexs.kind), NameOf(ItemIndexs.version),
                        NameOf(ItemIndexs.pumpModelNumber), NameOf(ItemIndexs.medicalDeviceTimeAsString),
                        NameOf(ItemIndexs.firstName), NameOf(ItemIndexs.lastName),
                        NameOf(ItemIndexs.conduitSerialNumber), NameOf(ItemIndexs.conduitBatteryLevel),
                        NameOf(ItemIndexs.conduitBatteryStatus), NameOf(ItemIndexs.conduitInRange),
                        NameOf(ItemIndexs.conduitMedicalDeviceInRange), NameOf(ItemIndexs.conduitSensorInRange),
                        NameOf(ItemIndexs.medicalDeviceFamily), NameOf(ItemIndexs.medicalDeviceSerialNumber),
                        NameOf(ItemIndexs.reservoirLevelPercent), NameOf(ItemIndexs.reservoirAmount),
                        NameOf(ItemIndexs.reservoirRemainingUnits), NameOf(ItemIndexs.medicalDeviceBatteryLevelPercent),
                        NameOf(ItemIndexs.sensorDurationHours), NameOf(ItemIndexs.timeToNextCalibHours),
                        NameOf(ItemIndexs.bgUnits), NameOf(ItemIndexs.timeFormat),
                        NameOf(ItemIndexs.medicalDeviceSuspended), NameOf(ItemIndexs.lastSGTrend),
                        NameOf(ItemIndexs.averageSG), NameOf(ItemIndexs.belowHypoLimit),
                        NameOf(ItemIndexs.aboveHyperLimit), NameOf(ItemIndexs.timeInRange),
                        NameOf(ItemIndexs.pumpCommunicationState), NameOf(ItemIndexs.gstCommunicationState),
                        NameOf(ItemIndexs.gstBatteryLevel), NameOf(ItemIndexs.maxAutoBasalRate),
                        NameOf(ItemIndexs.maxBolusAmount), NameOf(ItemIndexs.sensorDurationMinutes),
                        NameOf(ItemIndexs.timeToNextCalibrationMinutes), NameOf(ItemIndexs.clientTimeZoneName),
                        NameOf(ItemIndexs.sgBelowLimit), NameOf(ItemIndexs.averageSGFloat),
                        NameOf(ItemIndexs.timeToNextCalibrationRecommendedMinutes),
                        NameOf(ItemIndexs.calFreeSensor), NameOf(ItemIndexs.finalCalibration)
                    }

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

    <Extension>
    Friend Function GetItemIndex(key As String) As ItemIndexs
        Return CType([Enum].Parse(GetType(ItemIndexs), key), ItemIndexs)
    End Function

End Module
