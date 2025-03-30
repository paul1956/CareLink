' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text

Friend Module UserMessageConstants

    Public Function CreateDictionarySortedByValue(myDictionary As Dictionary(Of String, String)) As String
        Dim strBuilder As New StringBuilder
        strBuilder.AppendLine("Dim sortedDict As New Dictionary(Of String, String) With {")
        For Each kvp As KeyValuePair(Of String, String) In myDictionary.OrderBy(Function(x) x.Value)
            strBuilder.AppendLine($"    {{""{kvp.Key}"", ""{kvp.Value}""}},")
        Next
        strBuilder.AppendLine("}")
        Return strBuilder.ToString
    End Function

    Friend ReadOnly s_autoModeShieldMessages As New Dictionary(Of String, String) From {
        {"AUTO_BASAL", "Auto Basal"},
        {"FEATURE_OFF", "Feature Off"},
        {"SAFE_BASAL", "Safe Basal"}}

    Friend ReadOnly s_calibrationIconMessages As New Dictionary(Of String, String) From {
        {"HCL_REQUIRES_BG_LEGACY", "BG Required"},
        {"HOURS_12", "Calibration due 12 hours"},
        {"HOURS_11", "Calibration due 11 hours"},
        {"HOURS_10", "Calibration due 10 hours"},
        {"HOURS_9", "Calibration due 9 hours"},
        {"HOURS_8", "Calibration due 8 hours"},
        {"HOURS_7", "Calibration due 7 hours"},
        {"HOURS_6", "Calibration due 6 hours"},
        {"HOURS_5", "Calibration due 5 hours"},
        {"HOURS_4", "Calibration due 4 hours"},
        {"HOURS_3", "Calibration due 3 hours"},
        {"HOURS_2", "Calibration due 2 hours"},
        {"HOURS_1", "Calibration due 1 hour"},
        {"HOURS_0", "Calibration due <1 Hour"},
        {"INIT", "Initializing"},
        {"NO_ICON", ""},
        {"UNDEFINED", ""}}

    Friend ReadOnly s_calibrationMessages As New Dictionary(Of String, String) From {
        {"DUENOW", "Due now"},
        {"LESS_THAN_THREE_HRS", "Less then 3 hours"},
        {"LESS_THAN_SIX_HRS", "Less then 6 hours"},
        {"LESS_THAN_NINE_HRS", "Less then 9 hours"},
        {"LESS_THAN_TWELVE_HRS", "Less than twelve hours"},
        {"UNKNOWN", "Unknown"}}

    ''' <summary>
    ''' These message have parameters in (), (units) will be replace by localized units.
    ''' </summary>
    Friend ReadOnly s_notificationMessages As New Dictionary(Of String, String) From {
        {"2", "Pump Error. Delivery Stopped"},
        {"6", "Insert battery(triggeredDateTime). Delivery stopped. Insert a new battery now."},
        {"7", "Delivery Stopped. Check BG"}, _ ' From Java
        {"11", "Battery failed(triggeredDateTime). Replace Pump Battery Now."},
        {"12", "Auto suspend Limit reached(triggeredDateTime). Insulin delivery suspended. No buttons pressed within time set in Auto Suspend."},
        {"19", "Loading Incomplete During Infusion Set Change(triggeredDateTime)"},
        {"24", "Critical Pump Error. Stop Pump Use. Use Other Treatment"},
        {"25", "Pump Power Error. Record Settings"},
        {"29", "Pump Restarted. Delivery Stopped"},
        {"37", "Pump Motor Error. Delivery Stopped"},
        {"51", "Bolus stopped(triggeredDateTime). Cannot resume bolus for cannula fill, (deliveredAmount) of (programmedAmount) U delivered. {vbCrLf}(notDeliveredAmount) U not delivered. If needed, enter values again."},
        {"52", "Delivery Limit Exceeded. Check BG"}, _ ' From Java
        {"57", "Pump Battery Not Compatible"},
        {"58", "Insert A New AA Battery"},
        {"61", "Stuck button(triggeredDateTime). Button pressed for more then 3 minutes."},
        {"62", "New Notification Received From Pump"},
        {"66", "No Reservoir Detected During Infusion Set Change"},
        {"69", "Loading incomplete(triggeredDateTime). Restart the Reservoir & Set procedure"},
        {"73", "Replace Pump Battery Now"},
        {"77", "Pump Settings Error. Delivery Stopped"},
        {"84", "Pump Battery Removed. Replace Battery"},
        {"100", "Bolus Not delivered. Bolus entry timed out before delivery. If bolus was intended, enter values again."},
        {"103", "BG Check Reminder"},
        {"104", "Battery low pump(triggeredDateTime). Replace battery soon."},
        {"105", "Low Reservoir(triggeredDateTime). (unitsRemaining) units remaining. Change reservoir."},
        {"107", "Missed Meal Bolus Reminder"}, _ ' From Java
        {"109", "Set change reminder(triggeredDateTime). (lastSetChange) days since last set change. Time to change the infusion set."},
        {"110", "Silenced Sensor Alert. Check Alarm History"},
        {"113", "Reservoir empty reminder, change reservoir."},
        {"117", "Active Insulin Cleared"},
        {"130", "Rewind Required(triggeredDateTime). Delivery stopped. Rewind was required due to pump error. Select OK to continue."},
        {"140", "Delivery Suspended. Connect Infusion Set"}, _ ' From Java
        {"775", "Enter BG now(triggeredDateTime). Enter BG to calibrate sensor."},
        {"776", "Calibration Not accepted(triggeredDateTime). Wait at least 15 minutes. Wash hands, test BG again and calibrate."},
        {"777", "Change Sensor(triggeredDateTime). Sensor not working properly. Insert new sensor."},
        {"779", "Recharge Transmitter Now"},
        {"780", "Lost sensor signal(triggeredDateTime). Move pump closer to sensor. May take 15 minutes to find signal."},
        {"781", "Possible signal interface(triggeredDateTime). Move away from electronic devices. May take 15 minutes to find signal."},
        {"784", "Raise Alert(triggeredDateTime). Sensor glucose raising rapidly."},
        {"794", "Sensor expired(triggeredDateTime). Insert New sensor."},
        {"795", "Lost Sensor Signal. Check Transmitter"}, _ ' From Java
        {"796", "No Sensor Signal"}, _ ' From Java
        {"797", "Sensor connected(triggeredDateTime). Start new sensor."},
        {"798", "Sensor connected(triggeredDateTime). If new sensor, select Start New. If not, select Reconnect."},
        {"801", "Sensor updating(triggeredDateTime), Updating can take up it can take up to (sensorUpdateTime). Monitor BG.(vbCrLf) Entered BGs will not calibrate the sensor, but can still be used for therapy."},
        {"802", "Alert on low (sg) (units)(triggeredDateTime). Low sensor glucose. Check BG."},
        {"803", "Alert on low (sg) (units)(triggeredDateTime). Low sensor glucose. Insulin delivery suspended since (suspendedSince). Check BG."},
        {"805", "Alert before low(triggeredDateTime). Sensor glucose approaching Low Limit. Check BG."},
        {"807", "Basal delivery resumed at (secondaryTime) after suspend by sensor, Check BG."},
        {"809", "Suspend on low(triggeredDateTime). Delivery stopped. Sensor glucose (sg) (units). Check BG."},
        {"811", "Suspend before low. Delivery stopped. Sensor glucose approaching Low Limit. Check BG."},
        {"812", "Call for emergency(triggeredDateTime)."},
        {"814", "Basal Resumed. SG Still Under Low Limit. Check BG"}, _ ' From Java
        {"815", "Low Limit Changed. Basal Manually Resumed. Check BG"}, _ ' From Java
        {"816", "Alert on high (sg) (units)(triggeredDateTime). High sensor glucose. Check BG."},
        {"817", "Alert before high(triggeredDateTime). Sensor glucose approaching High Limit. Check BG."},
        {"819", "Auto Mode exit(triggeredDateTime). (basalName) started. Would you Like to review Auto Mode Readiness Screen?"},
        {"820", "SmartGuard exit(triggeredDateTime). (basalName) started. Would you Like to review SmartGuard Checklist?"},
        {"821", "Minimum Delivery Timeout. BG Required"}, _ ' From Java
        {"822", "SmartGuard maximum delivery. Auto Mode has been at maximum delivery for 4 hours. Enter BG to continue in SmartGuard."},
        {"823", "High Sensor Glucose(triggeredDateTime). BG has been high over 1 hour. Change infusion set. Check Ketones. Monitor BG."},
        {"827", "Low Sg (sg) (units)(triggeredDateTime) SG is under (lowLimit). Check BG and treat"},
        {"829", "BG required(triggeredDateTime). Enter a New BG for SmartGuard."},
        {"831", "Enter BG Now(triggeredDateTime). Enter a BG to continue in SmartGuard."},
        {"832", "Calibration Required"}, _ ' From Java
        {"833", "Bolus recommended(triggeredDateTime). For (bgValue) (units) entered, a correction bolus is recommended. Select Bolus to program a bolus."},
        {"838", "Calibration not accepted(triggeredDateTime).Sensor information is unavailable for up to 2 hours. Entered BGs may not calibrate the sensor but can be used for therapy."},
        {"869", "Reminder time to take (reminderName)."},
        {"870", "Recharge transmitter within 24 hours(triggeredDateTime)."}}

    Friend ReadOnly s_plgmLgsMessages As New Dictionary(Of String, String) From {
            {"FEATURE_OFF", "Feature Off"},
            {"MONITORING", "Monitoring"},
            {"REFRACTORY_PERIOD", "Refractory Period"},
            {"SUSPEND_FIRED_LGS", "Suspend Fired due to low-glucose management"},
            {"SUSPEND_FIRED_PLGM", "Suspend Fired due to predictive low-glucose management"}}

    Friend ReadOnly s_sensorMessages As New Dictionary(Of String, String) From {
            {"BG_RECOMMENDED", $"BG{vbCrLf}Recommended"},
            {"BG_REQUIRED", $"BG{vbCrLf}Required"},
            {"CALIBRATING", "Calibrating..."},
            {"CALIBRATION_REQUIRED", $"Calibration{vbCrLf}Required"},
            {"CHANGE_SENSOR", $"Change{vbCrLf}Sensor"},
            {"DELIVERY_SUSPEND", $"Delivery{vbCrLf}Suspended"},
            {"DO_NOT_CALIBRATE", $"Do Not{vbCrLf}Calibrate"},
            {"DUAL_BOLUS", $"Dual{vbCrLf}Bolus"},
            {"LOAD_RESERVOIR", $"Load{vbCrLf}Reservoir"},
            {"NO_ACTION_REQUIRED", $"No Action{vbCrLf}Required"},
            {"NO_DATA_FROM_PUMP", $"No Data{vbCrLf}From Pump"},
            {"NO_DELIVERY", $"No{vbCrLf}Delivery"},
            {"NO_ERROR_MESSAGE", "---"},
            {"NO_SENSOR_SIGNAL", "Lost Sensor Signal... Move pump closer to transmitter. May take 15 minutes to find signal"},
            {"PROCESSING_BG", $"Processing{vbCrLf}BG"},
            {"PUMP_PAIRING_LOST", "Pump Pairing Lost"},
            {"RECONNECTING_TO_PUMP", $"Reconnecting{vbCrLf}To Pump"},
            {"SEARCHING_FOR_SENSOR_SIGNAL", $"Searching For{vbCrLf}Sensor Signal"},
            {"SENSOR_DISCONNECTED", $"Sensor{vbCrLf}Disconnected"},
            {"SENSOR_OFF", $"Sensor{vbCrLf}Off"},
            {"SG_ABOVE_400_MGDL", $"SG Above{vbCrLf}400 mg/dL"},
            {"SG_BELOW_40_MGDL", $"SG Below{vbCrLf}50 mg/dL"},
            {"SQUARE_BOLUS", $"Square{vbCrLf}Bolus"},
            {"SUSPENDED_BEFORE_LOW", $"Suspended{vbCrLf}Before Low"},
            {"SUSPENDED_ON_LOW", $"Suspended{vbCrLf}On Low"},
            {"TEMP_BASAL", $"Temp{vbCrLf}Basal"},
            {"TEMP_TARGET", $"Temp{vbCrLf}Target"},
            {"UNKNOWN", "Unknown"},
            {"UPDATING", $"Sensor{vbCrLf}Updating"},
            {"WAIT_TO_CALIBRATE", $"Wait To{vbCrLf}Calibrate..."},
            {"WAIT_TO_ENTER_BG", $"Wait To{vbCrLf}Enter BG..."},
            {"WARM_UP", $"Sensor{vbCrLf}Warm Up... Warm-up takes up to 2 hours. You will be notifies when calibration Is needed."}}

End Module
