' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module UserMessageHandler

    Friend ReadOnly s_autoModeShieldMessages As New Dictionary(Of String, String) From {
                        {"AUTO_BASAL", "Auto Basal"},
                        {"FEATURE_OFF", "Feature Off"},
                        {"SAFE_BASAL", "Safe Basal"}
                    }

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
                        {"UNDEFINED", ""}
                   }

    Friend ReadOnly s_calibrationMessages As New Dictionary(Of String, String) From {
                        {"DUENOW", "Due now"},
                        {"LESS_THAN_THREE_HRS", "Less then 3 hours"},
                        {"LESS_THAN_SIX_HRS", "Less then 6 hours"},
                        {"LESS_THAN_NINE_HRS", "Less then 9 hours"},
                        {"LESS_THAN_TWELVE_HRS", "Less than twelve hours"},
                        {"UNKNOWN", "Unknown"}
                    }

    ''' <summary>
    ''' These message have parameters
    ''' The string is split by : character which is not allowed in the message.
    ''' Everything before the : is the message, the text after the : is the key in the dictionary that
    ''' will replace (0). (units) will be replace by localized units.
    ''' </summary>
    Friend ReadOnly s_notificationMessages As New Dictionary(Of String, String) From {
                        {"BC_MESSAGE_BASAL_STARTED", "Auto Mode exit(triggeredDateTime). (0) started. Would you Like to review Auto Mode Readiness Screen?:basalName"},
                        {"BC_MESSAGE_CONFIRM_SENSOR_SIGNAL_CALIBRATE", "No calibration occurred(triggeredDateTime). Confirm sensor signal. Calibrate by (secondaryTime)."},
                        {"BC_MESSAGE_CONFIRM_SENSOR_SIGNAL_CHECK_BG", "No calibration occurred(triggeredDateTime). Confirm sensor signal. Check BG again to calibrate sensor."},
                        {"BC_MESSAGE_CORRECTION_BOLUS_RECOMMENDATION", "Bolus recommended(triggeredDateTime). For (0) (units) entered, a correction bolus is recommended. Select Bolus to program a bolus.:bgValue"}, ' DO NOT RENAME
                        {"BC_MESSAGE_DELIVERY_STOPPED_SG_APPROACHILG_LOW_LIMIT_CHECK_BG", "Suspend before low. Delivery stopped. Sensor glucose approaching Low Limit. Check BG."},
                        {"BC_MESSAGE_DELIVERY_STOPPED_SG_X_CHECK_BG", "Suspend on low(triggeredDateTime). Delivery stopped. Sensor glucose (0) (units). Check BG.:sg"},
                        {"BC_MESSAGE_MONITOR_BG", "BG Required(triggeredDateTime). Enter a new BG for AutoMode."},
                        {"BC_MESSAGE_PLACE_PUMP_CLOSER_TO_TRANSMITTER", "BG not received(triggeredDateTime). PLace pump closer to transmitter. Select OK to resend BG to transmitter."},
                        {"BC_MESSAGE_SG_UNDER_50_MG_DL", "Low SG(triggeredDateTime). Sensor Glucose is under (CriticalLow) (units). Check BG and treat."},
                        {"BC_MESSAGE_SMART_GUARD_SETTINGS_TURNED_OFF", "Auto mode started. The following SmartGuard settings are turned off Suspend before low And Suspend on low. "},
                        {"BC_MESSAGE_TIME_REMAINING_CHANGE_RESERVOIR", "Low Reservoir(triggeredDateTime). (0) units remaining. Change reservoir.:unitsRemaining"},
                        {"BC_MESSAGE_TIME_SINCE_LAST_BOLUS_CHECK_BG", "Need correct message for 'BC_MESSAGE_TIME_SINCE_LAST_BOLUS_CHECK_BG'"},
                        {"BC_REMINDER_TIME", "Reminder time to take (0).:reminderName"},
                        {"BC_SID_AUTO_MODE_HIGH_SG_CONTENT", "High SG(triggeredDateTime). BG has been high over 1 hour. Change infusion set. Check Ketones. Monitor BG."},
                        {"BC_SID_BASAL_DELIVERY_RESUMED_AT_X_AFTER_LOW_SUSPEND", "Basal delivery resumed at (secondaryTime) after suspend by sensor, Check BG."},
                        {"BC_SID_BASAL_STARTED_SMART_GUARD", "SmartGuard started(triggeredDateTime)."},
                        {"BC_SID_BATTERY_LIFE_LESS_30_MINUTES", "Battery life less than 30 minutes(triggeredDateTime), replace battery now."},
                        {"BC_SID_BATTERY_REMOVED_RE_ENTER_TIME_AND_DATE", "Insert battery(triggeredDateTime). Delivery stopped. Insert a new battery now."},
                        {"BC_SID_BG_REQUIRED_CONTENT", "BG required(triggeredDateTime). Enter a New BG for Auto Mode."},
                        {"BC_SID_BOLUS_ENTRY_TIMED_OUT", "Bolus Not delivered. Bolus entry timed out before delivery. If bolus was intended, enter values again."},
                        {"BC_SID_BUTTON_PRESSED_FOR_MOR_THAN_3_MIN", "Stuck button(triggeredDateTime). Button pressed for more then 3 minutes."},
                        {"BC_SID_CALL_FOR_EMERGENCY", "Call for emergency(triggeredDateTime)."},
                        {"BC_SID_CANNOT_RESUME_BOLUS_FILL_CANNULA", "Bolus stopped(triggeredDateTime). Cannot resume bolus for cannula fill, (deliveredAmount) of (programmedAmount) U delivered. (notDeliveredAmount) U not delivered. If needed, enter values again."},
                        {"BC_SID_CHECK_BG_AND_CALIBRATE_SENSOR", "Calibrate now(triggeredDateTime). Check BG And calibrate sensor."},
                        {"BC_SID_CHECK_BG_AND_CALIBRATE_SENSOR_TO_RECEIVE", "Calibrate by (secondaryTime). Check BG and calibrate to continuing receiving sensor information."},
                        {"BC_SID_CHECK_BG_CONSIDER_TESTING_KETONES_0U", "Check BG and consider testing ketones and changing reservoir."},
                        {"BC_SID_CHECK_BG_CONSIDER_TESTING_KETONES_CHANGE_RESERVOIR", "Check BG and consider testing ketones and changing reservoir."},
                        {"BC_SID_DELIVERY_STOPPED_AFTER_REWIND_COMPLETES", "Rewind Required(triggeredDateTime). Delivery stopped. Rewind was required due to pump error. Select OK to continue."},
                        {"BC_SID_DELIVERY_STOPPED_BATTERY_REPLACE", "Replace battery(triggeredDateTime). Delivery stopped."},
                        {"BC_SID_DELIVERY_STOPPED_INSERT_NEW_BATTERY", "Insert battery(triggeredDateTime). Delivery stopped. Insert a New battery now."},
                        {"BC_SID_DO_NOT_CALIBRATE_UNLESS_NOTIFIED", "Sensor updating, Do Not calibrate unless notified. This could take up to 3 hours"},
                        {"BC_SID_ENSURE_CONNECTION_SECURE", "Check connection. Ensure transmitter and sensor is secure, then select OK."},
                        {"BC_SID_ENSURE_DELIVERY_CHANGE_RESERVOIR", "Reservoir empty reminder, change reservoir and infusion set."},
                        {"BC_SID_ENTER_BG_TO_CALIBRATE_SENSOR", "Enter BG to calibrate sensor(triggeredDateTime)."},
                        {"BC_SID_ENTER_BG_TO_CALIBRATE_SENSOR_SENSOR_INFO_NO_AVAILABLE", "Enter BG to calibrate sensor(triggeredDateTime). Sensor info not available."},
                        {"BC_SID_ENTER_BG_TO_CONTINUE_IN_SMART_GUARD", "Enter BG to continue in SmartGuard(triggeredDateTime)."},
                        {"BC_SID_FILL_TUBING_STOPPED_DISCONNECT", "Fill tubing, delivery stopped."},
                        {"BC_SID_GLUCOSE_WAS_HIGHER_THREE_HOURS", "Glucose was higher(triggeredDateTime). Three hours."},
                        {"BC_SID_HIGH_SG_CHECK_BG", "Alert on high (0) (units)(triggeredDateTime). High sensor glucose. Check BG.:sg"},
                        {"BC_SID_IF_NEW_SENSR_SELCT_START_NEW_ELSE_REWIND", "Sensor connected. If New sensor, select Start New. If Not, select Reconnect."},
                        {"BC_SID_INSERT_AND_START_NEW_SENSOR", "Sensor expired(triggeredDateTime). Insert New sensor and Start New'."},
                        {"BC_SID_INSERT_NEW_AA_BATTERY", "Battery failed(triggeredDateTime). Insert a new AA battery."},
                        {"BC_SID_INSERT_NEW_SENSOR", "Sensor expired(triggeredDateTime). Insert New sensor."},
                        {"BC_SID_INSULIN_DELIVERY_SUSPENDED", "Auto suspend(triggeredDateTime). Insulin delivery suspended. No buttons pressed within time set in Auto Suspend"},
                        {"BC_SID_LOW_SD_CHECK_BG", "Alert on low (0) (units)(triggeredDateTime). Check BG.:sg"},
                        {"BC_SID_LOW_SG_CHECK_BG", "Alert on low (0) (units)(triggeredDateTime). Insulin delivery suspended since (secondaryTime). Check BG.:sg"},
                        {"BC_SID_LOW_SG_INSULIN_DELIVERY_SUSPENDED_SINCE_X_CHECK_BG", "Alert on low (0) (units)(triggeredDateTime). Insulin delivery suspended since (secondaryTime). Check BG.:sg"},
                        {"BC_SID_MAX_FILL_DROPS_QUESITION", "Max fill reached(triggeredDateTime) (deliveredAmount)U. "},
                        {"BC_SID_MAXIMUM_2_HOUR_SUSPEND_TIME_REACHED_CHECK_BG", "Maximum 2 hour suspend time reached(triggeredDateTime). Check BG"},
                        {"BC_SID_MAXIMUM_2_HOUR_SUSPEND_TIME_REACHED_SG_STILL_UNDER_LOW", "Maximum 2 hour suspend time reached, Sensor Glucose is still under (CriticalLow) (units), basal restarted. Check BG"},
                        {"BC_SID_MOVE_AWAY_FROM_ELECTR_DEVICES", "Possible signal interface(triggeredDateTime). Move away from electronic devices. May take 15 minutes to find signal."},
                        {"BC_SID_MOVE_PUMP_CLOSER_TO_MINILINK", "Lost sensor signal(triggeredDateTime). Move pump closer to transmitter. May take 15 minutes to find signal."},
                        {"BC_SID_RECHARGE_MINILINK_WITHIN_24_HR", "Recharge transmitter within 24 hours(triggeredDateTime)."},
                        {"BC_SID_REMOVE_RESERVOIR_SELECT_REWIND", "Loading incomplete(triggeredDateTime). Remove reservoir and select Rewind to restart loading."},
                        {"BC_SID_REPLACE_BATTERY_SOON", "Battery low pump(triggeredDateTime). Replace battery soon."},
                        {"BC_SID_REWIND_BEFORE_LOADING", "No reservoir detected(triggeredDateTime). Rewind before loading reservoir."},
                        {"BC_SID_SECOND_CALIBRATION_NOT_ACCEPT", "Change Sensor(triggeredDateTime). Second calibration not accepted. Insert new sensor."},
                        {"BC_SID_SENSOR_INFO_UNAVAILABLE_FOR_UP_TO_TWO_HOURS", "Sensor info unavailable for up to two hours(triggeredDateTime)."},
                        {"BC_SID_SENSOR_RELATED_ISSUE_INSERT_NEW", "Sensor failure at(triggeredDateTime), Insert new sensor."},
                        {"BC_SID_SG_APPROACH_HIGH_LIMIT_CHECK_BG", "Alert before high(triggeredDateTime). Sensor glucose approaching High Limit. Check BG."},
                        {"BC_SID_SG_APPROACH_LOW_LIMIT_CHECK_BG", "Alert before low(triggeredDateTime). Sensor glucose approaching Low Limit. Check BG."},
                        {"BC_SID_SG_RISE_RAPID", "Raise Alert(triggeredDateTime). Sensor glucose raising rapidly."},
                        {"BC_SID_SMART_GUARD_MINIMUM_DELIVERY", "SmartGuard minimum delivery."},
                        {"BC_SID_START_NEW_SENSOR", "Start New sensor."},
                        {"BC_SID_THREE_DAYS_SINCE_LAST_SET_CHANGE", "Set Change reminder(triggeredDateTime). (0) days since last set change. Time to change reservoir and infusion set.:lastSetChange"},
                        {"BC_SID_THREE_DAYS_SINCE_LAST_SET_CHANGE1", "Set change reminder. changer reservoir and infusion set."},
                        {"BC_SID_TWO_DAYS_SINCE_LAST_SET_CHANGE", "Set Change reminder(triggeredDateTime). (0) days since last set change. Time to change reservoir and infusion set.:lastSetChange"},
                        {"BC_SID_UMAX_ALERT_INFO", "Auto Mode max delivery. Auto Mode has been at maximum delivery for 4 hours. Enter BG to continue in Auto Mode."},
                        {"BC_SID_UMIN_ALERT_INFO", "Auto Mode min delivery. Auto Mode has been at minimum delivery for 2 hours. Enter BG to continue in Auto Mode."},
                        {"BC_SID_UPDATING_CAN_TAKE_UP_TO_NINETY_MINUTES", "Sensor updating(triggeredDateTime), it can take up to 90 minutes."},
                        {"BC_SID_UPDATING_CAN_TAKE_UP_TO_THREE_HOURS", "Sensor updating(triggeredDateTime), it can take up to 3 hours."},
                        {"BC_SID_WAIT_AT_LEAST_15_MINUTES", "Calibration Not accepted.Wait at least 15 minutes. Wash hands, test BG again And calibrate."},
                        {"BC_TITLE_NEW_UNKNOWN_NOTIFICATION", "Enter BG Now (triggeredDateTime). Enter BG to calibrate sensor. Sensor information is no longer available."}
                    }

    Friend ReadOnly s_plgmLgsMessages As New Dictionary(Of String, String) From {
                        {"FEATURE_OFF", "Feature Off"},
                        {"MONITORING", "Monitoring"},
                        {"REFRACTORY_PERIOD", "Refractory Period"},
                        {"SUSPEND_FIRED_LGS", "Suspend Fired due to low-glucose management"},
                        {"SUSPEND_FIRED_PLGM", "Suspend Fired due to predictive low-glucose management"}
                    }

    Friend ReadOnly s_sensorMessages As New Dictionary(Of String, String) From {
                        {"BG_RECOMMENDED", $"BG{vbCrLf}Recommended"},
                        {"BG_REQUIRED", $"BG{vbCrLf}Required"},
                        {"CALIBRATING", "Calibrating..."},
                        {"CALIBRATION_REQUIRED", $"Calibration{vbCrLf}Required"},
                        {"CHANGE_SENSOR", $"Change{vbCrLf}Sensor"},
                        {"DELIVERY_SUSPEND", $"Delivery{vbCrLf}Suspended"},
                        {"DO_NOT_CALIBRATE", $"Do Not{vbCrLf}Calibrate"},
                        {"LOAD_RESERVOIR", $"Load{vbCrLf}Reservoir"},
                        {"NO_ACTION_REQUIRED", $"No Action{vbCrLf}Required"},
                        {"NO_DATA_FROM_PUMP", $"No Data{vbCrLf}From Pump"},
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
                        {"SUSPENDED_BEFORE_LOW", $"Suspended{vbCrLf}Before Low"},
                        {"TEMP_BASAL", $"Temp{vbCrLf}Basal"},
                        {"TEMP_TARGET", $"Temp{vbCrLf}Target"},
                        {"UNKNOWN", "Unknown"},
                        {"UPDATING", $"Sensor{vbCrLf}Updating"},
                        {"WAIT_TO_CALIBRATE", $"Wait To{vbCrLf}Calibrate..."},
                        {"WAIT_TO_ENTER_BG", $"Wait To{vbCrLf}Enter BG..."},
                        {"WARM_UP", $"Sensor{vbCrLf}Warm Up... Warm-up takes up to 2 hours. You will be notifies when calibration Is needed."}
                    }

End Module
