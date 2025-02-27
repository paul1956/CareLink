' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

' Do not rename these name are matched used in case sensitive matching
Friend Enum ServerDataIndexes
    clientTimeZoneName
    lastName
    firstName
    appModelType
    appModelNumber
    currentServerTime
    conduitSerialNumber
    conduitBatteryLevel
    conduitBatteryStatus
    lastConduitDateTime
    lastConduitUpdateServerDateTime
    medicalDeviceFamily
    medicalDeviceInformation
    medicalDeviceTime
    lastMedicalDeviceDataUpdateServerTime
    cgmInfo
    calFreeSensor
    calibStatus
    calibrationIconId
    timeToNextEarlyCalibrationMinutes
    timeToNextCalibrationMinutes
    timeToNextCalibrationRecommendedMinutes
    timeToNextCalibHours
    finalCalibration
    sensorDurationMinutes
    sensorDurationHours
    transmitterPairedTime
    systemStatusTimeRemaining
    gstBatteryLevel
    pumpBannerState
    therapyAlgorithmState
    reservoirLevelPercent
    reservoirAmount
    pumpSuspended
    pumpBatteryLevelPercent
    reservoirRemainingUnits
    conduitInRange
    conduitMedicalDeviceInRange
    conduitSensorInRange
    systemStatusMessage
    sensorState
    gstCommunicationState
    pumpCommunicationState
    timeFormat
    bgUnits
    maxAutoBasalRate
    maxBolusAmount
    sgBelowLimit
    approvedForTreatment
    lastAlarm
    activeInsulin
    basal
    lastSensorTime
    lastSG
    lastSGTrend
    limits
    belowHypoLimit
    aboveHyperLimit
    timeInRange
    averageSGFloat
    averageSG
    markers
    sgs
    notificationHistory
    sensorLifeText
    sensorLifeIcon

End Enum
