' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module SummaryRecordListHelpers

    <Extension>
    Friend Function GetValue(l As List(Of SummaryRecord), Key As String) As String
        For Each s As SummaryRecord In l
            If s.Key = Key Then
                Return s.Value
            End If
        Next
        Throw New ArgumentException("Key not found", NameOf(Key))
    End Function

    Friend Sub ProcessSummaryEntry(row As KeyValuePair(Of String, String), rowIndex As ItemIndexs, ByRef firstName As String)
        If row.Value Is Nothing Then
            row = KeyValuePair.Create(row.Key, "")
        End If
        Select Case rowIndex
            Case ItemIndexs.lastSensorTS
                If row.Value = "0" Then
                    ' Handled by ItemIndexs.lastSensorTSAsString
                Else
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))
                End If

            Case ItemIndexs.medicalDeviceTimeAsString
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.lastSensorTSAsString
                If s_listOfSummaryRecords.Count < ItemIndexs.lastSensorTSAsString Then
                    s_listOfSummaryRecords.Insert(ItemIndexs.lastSensorTS, New SummaryRecord(ItemIndexs.lastSensorTS, New KeyValuePair(Of String, String)(NameOf(ItemIndexs.lastSensorTS), row.Value.CDateOrDefault(NameOf(ItemIndexs.lastSensorTS), CurrentUICulture))))
                End If
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.kind
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.version
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.pumpModelNumber
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.currentServerTime,
               ItemIndexs.lastConduitTime,
               ItemIndexs.lastConduitUpdateServerTime
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row.Key, row.Value.Epoch2DateTimeString))

            Case ItemIndexs.lastMedicalDeviceDataUpdateServerTime
                s_lastMedicalDeviceDataUpdateServerTime = CLng(row.Value)
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row.Key, row.Value.Epoch2DateTimeString))

            Case ItemIndexs.firstName
                firstName = row.Value
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.lastName
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.conduitSerialNumber,
                 ItemIndexs.conduitBatteryLevel,
                 ItemIndexs.conduitBatteryStatus
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.conduitInRange
                s_conduitSensorInRange = CBool(row.Value)
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.conduitMedicalDeviceInRange,
                 ItemIndexs.conduitSensorInRange,
                 ItemIndexs.medicalDeviceFamily
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.sensorState
                s_sensorState = row.Value
                Dim message As String = ""
                If s_sensorMessages.TryGetValue(row.Value, message) Then
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row.Key, row.Value, message))
                Else
                    If Debugger.IsAttached Then
                        MsgBox($"{row.Value} is unknown sensor state message", MsgBoxStyle.OkOnly, $"Form 1 line:{New StackFrame(0, True).GetFileLineNumber()}")
                    End If
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row.Key, row.Value.ToTitleCase))
                End If

            Case ItemIndexs.medicalDeviceSerialNumber
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.medicalDeviceTime
                If row.Value = "0" Then
                    ' Handled by ItemIndexs.lastSensorTSAsString
                Else
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))
                End If

            Case ItemIndexs.sMedicalDeviceTime
                If s_listOfSummaryRecords.Count < ItemIndexs.sMedicalDeviceTime Then
                    s_listOfSummaryRecords.Add(New SummaryRecord(ItemIndexs.medicalDeviceTime, New KeyValuePair(Of String, String)(NameOf(ItemIndexs.medicalDeviceTime), row.Value.CDateOrDefault(NameOf(ItemIndexs.medicalDeviceTime), CurrentUICulture))))
                End If
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.reservoirLevelPercent
                s_reservoirLevelPercent = CInt(row.Value)
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.reservoirAmount
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.reservoirRemainingUnits
                s_reservoirRemainingUnits = row.Value.ParseSingle(0)
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.medicalDeviceBatteryLevelPercent
                s_medicalDeviceBatteryLevelPercent = CInt(row.Value)
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.sensorDurationHours
                s_sensorDurationHours = CInt(row.Value)
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.timeToNextCalibHours
                s_timeToNextCalibHours = CUShort(row.Value)
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.calibStatus
                Dim message As String = ""
                If s_calibrationMessages.TryGetValue(row.Value, message) Then
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row.Key, row.Value, message))
                Else
                    If Debugger.IsAttached Then
                        MsgBox($"{row.Value} is unknown calibration message", MsgBoxStyle.OkOnly, $"Form 1 line:{New StackFrame(0, True).GetFileLineNumber()}")
                    End If
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row.Key, row.Value.ToTitleCase))
                End If

            Case ItemIndexs.bgUnits
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.timeFormat
                s_timeWithMinuteFormat = If(row.Value = "HR_12", TwelveHourTimeWithMinuteFormat, MilitaryTimeWithMinuteFormat)
                s_timeWithoutMinuteFormat = If(row.Value = "HR_12", TwelveHourTimeWithoutMinuteFormat, MilitaryTimeWithoutMinuteFormat)
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.lastSensorTime
                If row.Value = "0" Then
                    ' Handled by ItemIndexs.lastSensorTSAsString
                Else
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))
                End If

            Case ItemIndexs.sLastSensorTime
                If s_listOfSummaryRecords.Count < ItemIndexs.sLastSensorTime Then
                    s_listOfSummaryRecords.Add(New SummaryRecord(ItemIndexs.lastSensorTime, New KeyValuePair(Of String, String)(NameOf(ItemIndexs.lastSensorTime), row.Value.CDateOrDefault(NameOf(ItemIndexs.medicalDeviceTime), CurrentUICulture))))
                End If
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.medicalDeviceSuspended
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.lastSGTrend
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.systemStatusMessage
                s_systemStatusMessage = row.Value
                Dim message As String = ""
                If s_sensorMessages.TryGetValue(row.Value, message) Then
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row.Key, row.Value, message))
                Else
                    If Not String.IsNullOrWhiteSpace(row.Value) AndAlso Debugger.IsAttached Then
                        MsgBox($"{row.Value} is unknown system status message", MsgBoxStyle.OkOnly, $"Form 1 line:{New StackFrame(0, True).GetFileLineNumber()}")
                    End If
                    s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row.Key, row.Value.ToTitleCase))
                End If

            Case ItemIndexs.averageSG
                s_averageSG = row.Value
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.belowHypoLimit
                s_belowHypoLimit = row.Value.ParseSingle(1)
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.aboveHyperLimit
                s_aboveHyperLimit = row.Value.ParseSingle(1)
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.timeInRange
                s_timeInRange = CInt(row.Value)
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.pumpCommunicationState,
             ItemIndexs.gstCommunicationState
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.gstBatteryLevel
                s_gstBatteryLevel = CInt(row.Value)
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.lastConduitDateTime
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, New KeyValuePair(Of String, String)(NameOf(ItemIndexs.lastConduitDateTime), row.Value.CDateOrDefault(NameOf(ItemIndexs.lastConduitDateTime), CurrentUICulture))))

            Case ItemIndexs.maxAutoBasalRate,
             ItemIndexs.maxBolusAmount
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.sensorDurationMinutes
                s_sensorDurationMinutes = CInt(row.Value)
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.timeToNextCalibrationMinutes
                s_timeToNextCalibrationMinutes = CInt(row.Value)
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.clientTimeZoneName
                s_clientTimeZoneName = row.Value
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))

            Case ItemIndexs.sgBelowLimit,
             ItemIndexs.averageSGFloat,
             ItemIndexs.timeToNextCalibrationRecommendedMinutes,
             ItemIndexs.calFreeSensor,
             ItemIndexs.finalCalibration
                s_listOfSummaryRecords.Add(New SummaryRecord(rowIndex, row))
            Case Else
                Stop
        End Select
    End Sub

End Module
