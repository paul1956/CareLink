' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module UserMessageHandler

    ' Add additional units here
    Private ReadOnly _unitsStrings As New Dictionary(Of String, String) From {
                            {"MGDL", "mg/dl"},
                            {"MMOLL", "mmol/L"}
                        }

    Friend ReadOnly _messages As New Dictionary(Of String, String) From {
                            {"BC_MESSAGE_CONFIRM_SENSOR_SIGNAL_CHECK_BG", "No calibration occured.Confirm sensor signal. Check BG again to calibrate sensor."},
                            {"BC_MESSAGE_DELIVERY_STOPPED_SG_APPROACHILG_LOW_LIMIT_CHECK_BG", "Suspend before low. Delivery stopped. Sensor glucose approaching Low Limit. Check BG."},
                            {"BC_MESSAGE_SMART_GUARD_SETTINGS_TURNED_OFF", "Auto mode started. The following SmartGuard settings are turned off Suspend before low and Suspend on low. "},
                            {"BC_SID_BASAL_STARTED_SMART_GUARD", "Basal started SmartGuard."},
                            {"BC_SID_BG_REQUIRED_CONTENT", "BG required. Enter a new BG for Auto Mode."},
                            {"BC_SID_BOLUS_ENTRY_TIMED_OUT", "Bolus not delivered. Bolus entry timed out before delivery. If bolus was intended, enter values again."},
                            {"BC_SID_CHECK_BG_AND_CALIBRATE_SENSOR", "Calibrate now. Check BG and calibrate sensor."},
                            {"BC_SID_CHECK_BG_CONSIDER_TESTING_KETONES_CHANGE_RESERVOIR", "Check BG and consider testing ketones and changing reservoir."},
                            {"BC_SID_DELIVERY_STOPPED_INSERT_NEW_BATTERY", "Insert battery. Delivery stopped. Insert a new battery now."},
                            {"BC_SID_DO_NOT_CALIBRATE_UNLESS_NOTIFIED", "Sensor updating, Do not calibrate unless notified. This could take up to 3 hours"},
                            {"BC_SID_ENSURE_CONNECTION_SECURE", "Check connection. Ensure transmitter and sensor is secure, then select OK."},
                            {"BC_SID_ENSURE_DELIVERY_CHANGE_RESERVOIR", "To ensure delivery change reservoir."},
                            {"BC_SID_ENTER_BG_TO_CONTINUE_IN_SMART_GUARD", "Enter BG to continue in SmartGuard."},
                            {"BC_SID_FILL_TUBING_STOPPED_DISCONNECT", "Fill tubing, delivery stopped"},
                            {"BC_SID_IF_NEW_SENSR_SELCT_START_NEW_ELSE_REWIND", "Sensor connected. If new sensor, select Start New. If not, select Reconnect."},
                            {"BC_SID_INSERT_NEW_SENSOR", "Sensor expired, Insert new sensor."},
                            {"BC_SID_MOVE_AWAY_FROM_ELECTR_DEVICES", "Possible signal interface. Move away from electronic devices.May take 15 minutes to find signal."},
                            {"BC_SID_MOVE_PUMP_CLOSER_TO_MINILINK", "Lost sensor signal. Move pump closer to transmitter. May take 15 minutes to find signal."},
                            {"BC_SID_REPLACE_BATTERY_SOON", "Battery low. Replace battery soon."},
                            {"BC_SID_SG_APPROACH_HIGH_LIMIT_CHECK_BG", "Alert before high. Sensor glucose approaching high limit. Check BG."},
                            {"BC_SID_SG_APPROACH_LOW_LIMIT_CHECK_BG", "Alert before low. Sensor Glucose approaching low limit. Check BG."},
                            {"BC_SID_SG_RISE_RAPID", "Raise Alert. Sensor glucose raising rapidly."},
                            {"BC_SID_SMART_GUARD_MINIMUM_DELIVERY", "SmartGuard minimum delivery."},
                            {"BC_SID_START_NEW_SENSOR", "Start new sensor."},
                            {"BC_SID_UMAX_ALERT_INFO", "Auto Mode max delivery. Auto Mode has been at maximum delivery for 4 hours. Enter BG to continue in Auto Mode."},
                            {"BC_SID_UMIN_ALERT_INFO", "Auto Mode max delivery. Auto Mode has been at minimum delivery for 2 hours. Enter BG to continue in Auto Mode."},
                            {"BC_SID_UPDATING_CAN_TAKE_UP_TO_THREE_HOURS", "Sensor updating, it can take up to 3 hours."},
                            {"BC_SID_WAIT_AT_LEAST_15_MINUTES", "Calibration not accepted. Wait at least 15 minutes. Wash hands, test BGagain and calibrate."},
                            {"CALIBRATING", "Calibrating ..."},
                            {"CALIBRATION_REQUIRED", "Calibration required"},
                            {"DO_NOT_CALIBRATE", "Do not calibrate."},
                            {"NO_DATA_FROM_PUMP", "No data from pump"},
                            {"NO_ERROR_MESSAGE", "---"},
                            {"NO_SENSOR_SIGNAL", "Lost sensor signal, move pump closer to transmitter. May take 15 minutes to find signal"},
                            {"SEARCHING_FOR_SENSOR_SIGNAL", "Searching for sensor signal"},
                            {"SENSOR_DISCONNECTED", "Sensor disconnected"},
                            {"UNKNOWN", "Unknown"},
                            {"WAIT_TO_CALIBRATE", "Wait To Calibrate..."},
                            {"WARM_UP", "Sensor warm up. Warm-up takes up to 2 hours. You will be notifies when calibration is needed."}
                        }

    ' These message have parameters
    ' The string is split by : character which is not allowed in the message.
    ' Everything before the : is the message, the text after the : is the key in the dictionary that
    ' will replace (0). (units) will be replace by localized units.
    '
    Friend ReadOnly s_messagesSpecialHandling As New Dictionary(Of String, String) From {
                            {"BC_MESSAGE_BASAL_STARTED", "Auto Mode exit. (0) started. Would you like to review Auto Mode Readiness Screen?:basalName"},
                            {"BC_MESSAGE_CORRECTION_BOLUS_RECOMMENDATION", $"Blood Glucose (0) (units). Correction bolus recommended.:bgValue"},
                            {"BC_MESSAGE_DELIVERY_STOPPED_SG_X_CHECK_BG", "Suspend on low. Delivery stopped. Sensor glucose (0) (units). Check BG.:sg"},
                            {"BC_MESSAGE_SG_UNDER_50_MG_DL", "Low SG. Sensor Glucose is under (CriticalLow) (units). Check BG and treat.:sg"},
                            {"BC_MESSAGE_TIME_REMAINING_CHANGE_RESERVOIR", "Low Reservoir (0) units remaining. Change reservoir.:unitsRemaining"},
                            {"BC_SID_BASAL_DELIVERY_RESUMED_AT_X_AFTER_LOW_SUSPEND", "Basal delivery resumed at (0) after suspend by sensor, Check BG.:secondaryTime"},
                            {"BC_SID_CHECK_BG_AND_CALIBRATE_SENSOR_TO_RECEIVE", "Calibrate by (0). Check BG and calibrate to continuing receiving sensor information.:secondaryTime"},
                            {"BC_SID_HIGH_SG_CHECK_BG", "$Alert on high (0) (units). High sensor glucose. Check BG.:sg"},
                            {"BC_SID_LOW_SD_CHECK_BG", $"Alert on low (0) (units). Low sensor glucose. Check BG.:sg"},
                            {"BC_SID_LOW_SG_INSULIN_DELIVERY_SUSPENDED_SINCE_X_CHECK_BG", "Alert on low (0) (units). Insulin delivery suspended since (secondaryTime). Check BG.:sg"},
                            {"BC_SID_THREE_DAYS_SINCE_LAST_SET_CHANGE", "(0) days since last set change:lastSetChange"}
                        }

    <Extension>
    Private Function FormatTimeOnly(rawTime As String, format As String) As String
        Return New TimeOnly(CInt(rawTime.Substring(0, 2)), CInt(rawTime.Substring(3, 2))).ToString(format)
    End Function

    Friend Function GetLocalizedUnits(unitName As String) As String
        Return _unitsStrings(unitName)
    End Function

    Friend Function TranslateMessageId(dic As Dictionary(Of String, String), entryValue As String, TimeFormat As String) As String
        Dim formattedMessage As String = ""
        If _messages.TryGetValue(entryValue, formattedMessage) Then
        Else
            If s_messagesSpecialHandling.TryGetValue(entryValue, formattedMessage) Then
                Dim splitMessageValue As String() = formattedMessage.Split(":")
                Dim key As String = splitMessageValue(1)
                Dim replacementValue As String = If(key = "secondaryTime", dic(key).FormatTimeOnly(TimeFormat), dic(key))
                Dim secondaryTime As String = If(dic.ContainsKey("secondaryTime"), dic("secondaryTime").FormatTimeOnly(TimeFormat), "")
                formattedMessage = splitMessageValue(0).Replace("(0)", replacementValue) _
                                                       .Replace("(CriticalLow)", S_criticalLow.ToString(CurrentUICulture)) _
                                                       .Replace("(units)", GetLocalizedUnits(s_bgUnits)) _
                                                       .Replace("(secondaryTime)", secondaryTime)
            Else

                If Debugger.IsAttached Then
                    MsgBox($"Unknown sensor message '{entryValue}'", MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Unknown Sensor Message")
                End If
                formattedMessage = entryValue.Replace("_", " ")
            End If
        End If
        Return $"{entryValue}({formattedMessage})"
    End Function

End Module
