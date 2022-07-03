' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module Messages

    Friend ReadOnly _messages As New Dictionary(Of String, String) From {
                        {"BC_MESSAGE_BASAL_STARTED", "Auto Mode exit. Basal 1 started. Would you like to review Auto Mode Readiness Screen?"},
                        {"BC_MESSAGE_SG_UNDER_50_MG_DL", "Sensor Glucose under..."}, ' The actual message is generated at runtime
                        {"BC_SID_BASAL_STARTED_SMART_GUARD", "Basal started SmartGuard."},
                        {"BC_SID_BG_REQUIRED_CONTENT", "BG required Content."},
                        {"BC_SID_BOLUS_ENTRY_TIMED_OUT", "Bolus not delivered. Bolus entry timed out before delivery. If bolus was intended, enter values again."},
                        {"BC_SID_CHECK_BG_AND_CALIBRATE_SENSOR", "Calibrate now. Check BG and calibrate sensor."},
                        {"BC_SID_DELIVERY_STOPPED_INSERT_NEW_BATTERY", "nsert battery. Delivery stopped. Insert a new batterynow."},
                        {"BC_SID_DO_NOT_CALIBRATE_UNLESS_NOTIFIED", "Sensor updating, Do not calibrate unless notified. This could take up to 3 hours"},
                        {"BC_SID_ENSURE_CONNECTION_SECURE", "Check connection. Ensure transmitter and sensor is secure, then select OK."},
                        {"BC_SID_ENTER_BG_TO_CONTINUE_IN_SMART_GUARD", "Enter BG to continue in SmartGuard."},
                        {"BC_SID_IF_NEW_SENSR_SELCT_START_NEW_ELSE_REWIND", "If new sensor select start new else rewind."},
                        {"BC_SID_LOW_SD_CHECK_BG", "Alert on low. Low sensor glucose. Check BG."},
                        {"BC_SID_MOVE_AWAY_FROM_ELECTR_DEVICES", "Possible signal interface. Move away from electronic devices."},
                        {"BC_SID_MOVE_PUMP_CLOSER_TO_MINILINK", "Move pump closer to transmitter."},
                        {"BC_SID_SG_APPROACH_HIGH_LIMIT_CHECK_BG", "Alert before high. Sensor glucose approaching high limit. Check BG."},
                        {"BC_SID_SG_APPROACH_LOW_LIMIT_CHECK_BG", "Alert before low. Sensor Glucose approaching low limit. Check BG."},
                        {"BC_SID_SG_RISE_RAPID", "Raise Alert. Sensor glusose raising rapidly."},
                        {"BC_SID_START_NEW_SENSOR", "Start new sensor."},
                        {"BC_SID_UPDATING_CAN_TAKE_UP_TO_THREE_HOURS", "Sensor updating, it can take up to 3 hours."},
                        {"BC_SID_WAIT_AT_LEAST_15_MINUTES", "Calibration not accepted. Wait at least 15 mintues. Wash hands, test BGagain and calibrate."},
                        {"CALIBRATING", "Calibrating ..."},
                        {"CALIBRATION_REQUIRED", "Calibration required"},
                        {"NO_DATA_FROM_PUMP", "No data from pump"},
                        {"NO_ERROR_MESSAGE", "---"},
                        {"NO_SENSOR_SIGNAL", "Lost sensor signal, move pump closer to transmitter."},
                        {"SEARCHING_FOR_SENSOR_SIGNAL", "Searching for sensor signal"},
                        {"SENSOR_DISCONNECTED", "Sensor disconnected"},
                        {"UNKNOWN", "Unknown"},
                        {"WAIT_TO_CALIBRATE", "Wait To Calibrate..."},
                        {"WARM_UP", "Sensor warm up..."}
                    }
    Friend ReadOnly _messagesSpecialHandling As New Dictionary(Of String, String) From {
                        {"BC_MESSAGE_CORRECTION_BOLUS_RECOMMENDATION", "Blood Glucore (0). Correction bolus reccomended.:bgValue"},
                        {"BC_MESSAGE_TIME_REMAINING_CHANGE_RESERVOIR", "(0) units remaining. Change reservoir.:unitsRemaining"},
                        {"BC_SID_CHECK_BG_AND_CALIBRATE_SENSOR_TO_RECEIVE", "Check BG by (0) and calibrate to continuing receiving sensor information:secondaryTime"},
                        {"BC_SID_THREE_DAYS_SINCE_LAST_SET_CHANGE", "(0) days since last set change:lastSetChange"}
                    }

    ' Add additional units here, default
    Friend ReadOnly _unitsStrings As New Dictionary(Of String, String) From {
                {"MGDL", "mg/dl"},
                {"MMOLL", "mmol/L"}
            }

End Module
