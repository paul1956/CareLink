' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Module Messages

    Friend ReadOnly _messages As New Dictionary(Of String, String) From {
                {"BC_MESSAGE_BASAL_STARTED", "Automode exit, manual basal started"},
                {"BC_MESSAGE_SG_UNDER_50_MG_DL", "BG under..."}, ' The actual message is generated at runtime
                {"BC_SID_CHECK_BG_AND_CALIBRATE_SENSOR", "Check BG and calibrate sensor"},
                {"BC_SID_ENSURE_CONNECTION_SECURE", "Check connection. Ensure transmitter and sensor is secure, then select OK"},
                {"BC_SID_IF_NEW_SENSR_SELCT_START_NEW_ELSE_REWIND", "If new sensor select start new else rewind"},
                {"BC_SID_LOW_SD_CHECK_BG", "Low sensor reading check BG"},
                {"BC_SID_MOVE_AWAY_FROM_ELECTR_DEVICES", "Possible signal interface. Move away from electronic devices."},
                {"BC_SID_MOVE_PUMP_CLOSER_TO_MINILINK", "Move pump closer to transmitter"},
                {"BC_SID_SG_APPROACH_HIGH_LIMIT_CHECK_BG", "Sensor glucose approaching high limit. Check BG."},
                {"BC_SID_SG_RISE_RAPID", "BG rapidly raising"},
                {"BC_SID_WAIT_AT_LEAST_15_MINUTES", "Wait at least 15 mintues,"},
                {"CALIBRATING", "Calibrating ..."},
                {"CALIBRATION_REQUIRED", "Calibration required"},
                {"NO_DATA_FROM_PUMP", "No data from pump"},
                {"NO_ERROR_MESSAGE", "---"},
                {"NO_SENSOR_SIGNAL", "Lost sensor signal, move pump closer to transmitter."},
                {"SEARCHING_FOR_SENSOR_SIGNAL", "Searching for sensor signal"},
                {"SENSOR_DISCONNECTED", "Sensor disconnected"},
                {"UNKNOWN", "Unknown"},
                {"WARM_UP", "Sensor warm up..."},
                {"WAIT_TO_CALIBRATE", "Wait To Calibrate..."}
            }

    Friend ReadOnly _messagesSpecialHandling As New Dictionary(Of String, String) From {
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
