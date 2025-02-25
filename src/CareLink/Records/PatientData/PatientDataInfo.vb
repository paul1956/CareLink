' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class PatientDataInfo
    <JsonPropertyName("clientTimeZoneName")>
    Public Property ClientTimeZoneName As String

    <JsonPropertyName("lastName")>
    Public Property LastName As String

    <JsonPropertyName("firstName")>
    Public Property FirstName As String

    <JsonPropertyName("appModelType")>
    Public Property AppModelType As String

    <JsonPropertyName("appModelNumber")>
    Public Property AppModelNumber As String

    <JsonPropertyName("currentServerTime")>
    Public Property CurrentServerTime As Long

    <JsonPropertyName("conduitSerialNumber")>
    Public Property ConduitSerialNumber As String

    <JsonPropertyName("conduitBatteryLevel")>
    Public Property ConduitBatteryLevel As Integer

    <JsonPropertyName("conduitBatteryStatus")>
    Public Property ConduitBatteryStatus As String

    <JsonPropertyName("lastConduitDateTime")>
    Public Property LastConduitDateTime As String

    <JsonPropertyName("lastConduitUpdateServerDateTime")>
    Public Property LastConduitUpdateServerDateTime As Long

    <JsonPropertyName("medicalDeviceFamily")>
    Public Property MedicalDeviceFamily As String

    <JsonPropertyName("medicalDeviceInformation")>
    Public Property MedicalDeviceInformation As MedicalDeviceInformation

    <JsonPropertyName("medicalDeviceTime")>
    Public Property MedicalDeviceTime As Long

    <JsonPropertyName("lastMedicalDeviceDataUpdateServerTime")>
    Public Property LastMedicalDeviceDataUpdateServerTime As Long

    <JsonPropertyName("cgmInfo")>
    Public Property CgmInfo As CgmInfo

    <JsonPropertyName("calFreeSensor")>
    Public Property CalFreeSensor As Boolean

    <JsonPropertyName("calibStatus")>
    Public Property CalibStatus As String

    <JsonPropertyName("calibrationIconId")>
    Public Property CalibrationIconId As String

    <JsonPropertyName("timeToNextEarlyCalibrationMinutes")>
    Public Property TimeToNextEarlyCalibrationMinutes As Integer

    <JsonPropertyName("timeToNextCalibrationMinutes")>
    Public Property TimeToNextCalibrationMinutes As Integer

    <JsonPropertyName("timeToNextCalibrationRecommendedMinutes")>
    Public Property TimeToNextCalibrationRecommendedMinutes As Integer

    <JsonPropertyName("timeToNextCalibHours")>
    Public Property TimeToNextCalibHours As Integer

    <JsonPropertyName("finalCalibration")>
    Public Property FinalCalibration As Boolean

    <JsonPropertyName("sensorDurationMinutes")>
    Public Property SensorDurationMinutes As Integer

    <JsonPropertyName("sensorDurationHours")>
    Public Property SensorDurationHours As Integer

    <JsonPropertyName("transmitterPairedTime")>
    Public Property TransmitterPairedTime As String

    <JsonPropertyName("systemStatusTimeRemaining")>
    Public Property SystemStatusTimeRemaining As Integer

    <JsonPropertyName("gstBatteryLevel")>
    Public Property GstBatteryLevel As Integer

    <JsonPropertyName("pumpBannerState")>
    Public Property PumpBannerState As List(Of BannerState)

    <JsonPropertyName("therapyAlgorithmState")>
    Public Property TherapyAlgorithmState As TherapyAlgorithmState

    <JsonPropertyName("reservoirLevelPercent")>
    Public Property ReservoirLevelPercent As Integer

    <JsonPropertyName("reservoirAmount")>
    Public Property ReservoirAmount As Integer

    <JsonPropertyName("pumpSuspended")>
    Public Property PumpSuspended As Boolean

    <JsonPropertyName("pumpBatteryLevelPercent")>
    Public Property PumpBatteryLevelPercent As Integer

    <JsonPropertyName("reservoirRemainingUnits")>
    Public Property ReservoirRemainingUnits As Double

    <JsonPropertyName("conduitInRange")>
    Public Property ConduitInRange As Boolean

    <JsonPropertyName("conduitMedicalDeviceInRange")>
    Public Property ConduitMedicalDeviceInRange As Boolean

    <JsonPropertyName("conduitSensorInRange")>
    Public Property ConduitSensorInRange As Boolean

    <JsonPropertyName("systemStatusMessage")>
    Public Property SystemStatusMessage As String

    <JsonPropertyName("sensorState")>
    Public Property SensorState As String

    <JsonPropertyName("gstCommunicationState")>
    Public Property GstCommunicationState As Boolean

    <JsonPropertyName("pumpCommunicationState")>
    Public Property PumpCommunicationState As Boolean

    <JsonPropertyName("timeFormat")>
    Public Property TimeFormat As String

    <JsonPropertyName("bgUnits")>
    Public Property BgUnits As String

    <JsonPropertyName("maxAutoBasalRate")>
    Public Property MaxAutoBasalRate As Double

    <JsonPropertyName("maxBolusAmount")>
    Public Property MaxBolusAmount As Double

    <JsonPropertyName("sgBelowLimit")>
    Public Property SgBelowLimit As Integer

    <JsonPropertyName("approvedForTreatment")>
    Public Property ApprovedForTreatment As Boolean

    <JsonPropertyName("lastAlarm")>
    Public Property LastAlarm As LastAlarm

    <JsonPropertyName("activeInsulin")>
    Public Property ActiveInsulin As ActiveInsulin

    <JsonPropertyName("basal")>
    Public Property Basal As List(Of Basal)

    <JsonPropertyName("lastSensorTime")>
    Public Property LastSensorTime As Integer

    <JsonPropertyName("lastSG")>
    Public Property LastSG As LastSG

    <JsonPropertyName("lastSGTrend")>
    Public Property LastSGTrend As String

    <JsonPropertyName("limits")>
    Public Property Limits As List(Of Limit)

    <JsonPropertyName("belowHypoLimit")>
    Public Property BelowHypoLimit As Integer

    <JsonPropertyName("aboveHyperLimit")>
    Public Property AboveHyperLimit As Integer

    <JsonPropertyName("timeInRange")>
    Public Property TimeInRange As Integer

    <JsonPropertyName("averageSGFloat")>
    Public Property AverageSGFloat As Double

    <JsonPropertyName("averageSG")>
    Public Property AverageSG As Integer

    <JsonPropertyName("markers")>
    Public Property Markers As List(Of Marker)

    <JsonPropertyName("sgs")>
    Public Property Sgs As List(Of SG)

    <JsonPropertyName("notificationHistory")>
    Public Property NotificationHistory As Object '  As NotificationHistory

    <JsonPropertyName("sensorLifeText")>
    Public Property SensorLifeText As String

    <JsonPropertyName("sensorLifeIcon")>
    Public Property SensorLifeIcon As String
End Class
