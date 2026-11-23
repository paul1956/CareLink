' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module UserMessageConstants

    Friend Const CheckComplianceValues As String =
        "Ensure to check the compliance values for accuracy" &
        vbCrLf &
        "Check TIR compliance regularly for optimal management."

    Friend Const TirToolTip As String = "Estimated TIR Compliance shown in parentheses" &
        vbCrLf &
        "Values < 2 is Excellent and not shown" & vbCrLf &
        "Values between 2 and 4 are considered acceptable, but it is possible to improve" &
        vbCrLf &
        "Values > 4 suggests improvement is needed"

    Friend ReadOnly s_autoModeShieldMessages As New Dictionary(Of String, String)(Comparer) From {
        {"AUTO_BASAL", "Auto Basal"},
        {"FEATURE_OFF", "Feature Off"},
        {"SAFE_BASAL", "Safe Basal"}}

    Friend ReadOnly s_calibrationIconMessages As New Dictionary(Of String, String)(Comparer) From {
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

    Friend ReadOnly s_calibrationMessages As New Dictionary(Of String, String)(Comparer) From {
        {"DUENOW", "Due now"},
        {"LESS_THAN_THREE_HRS", "Less then 3 hours"},
        {"LESS_THAN_SIX_HRS", "Less then 6 hours"},
        {"LESS_THAN_NINE_HRS", "Less than 9 hours"},
        {"LESS_THAN_TWELVE_HRS", "Less than twelve hours"},
        {"UNKNOWN", "Unknown"}}

    Friend ReadOnly s_plgmLgsMessages As New Dictionary(Of String, String)(Comparer) From {
        {"FEATURE_OFF", "Feature Off"},
        {"MONITORING", "Monitoring"},
        {"REFRACTORY_PERIOD", "Refractory Period"},
        {"SUSPEND_FIRED_LGS", "Suspend Fired due to low-glucose management"},
        {"SUSPEND_FIRED_PLGM", "Suspend Fired due to predictive low-glucose management"}}

    Friend ReadOnly s_sensorMessages As New Dictionary(Of String, String)(Comparer) From {
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
        {"NO_SENSOR_SIGNAL", "Lost Sensor Signal... Move pump closer to transmitter. " &
                             "May take 15 minutes to find signal"},
        {"PROCESSING_BG", $"Processing{vbCrLf}BG"},
        {"PUMP_PAIRING_LOST", "Pump Pairing Lost"},
        {"RECONNECTING_TO_PUMP", $"Reconnecting{vbCrLf}To Pump"},
        {"SEARCHING_FOR_SENSOR_SIGNAL", $"Searching For{vbCrLf}Sensor Signal"},
        {"SENSOR_DISCONNECTED", $"Sensor{vbCrLf}Disconnected"},
        {"SENSOR_OFF", $"Sensor{vbCrLf}Off"},
        {"SG_BELOW_50_MGDL", $"Sensor{vbCrLf}Below 50 mg/dL"},
        {"SQUARE_BOLUS", $"Square{vbCrLf}Bolus"},
        {"SUSPENDED_BEFORE_LOW", $"Suspended{vbCrLf}Before Low"},
        {"SUSPENDED_ON_LOW", $"Suspended{vbCrLf}On Low"},
        {"TEMP_BASAL", $"Temp{vbCrLf}Basal"},
        {"TEMP_TARGET", $"Temp{vbCrLf}Target"},
        {"UNKNOWN", "Unknown"},
        {"UPDATING", $"Sensor{vbCrLf}Updating"},
        {"WAIT_TO_CALIBRATE", $"Wait To{vbCrLf}Calibrate..."},
        {"WAIT_TO_ENTER_BG", $"Wait To{vbCrLf}Enter BG..."},
        {"WAITING_WARM_UP", $"Waiting For warm up{vbCrLf}to start..."},
        {"WARM_UP", $"Sensor{vbCrLf}warm up"}}

End Module
