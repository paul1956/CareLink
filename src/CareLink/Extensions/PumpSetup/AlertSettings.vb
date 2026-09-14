' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module AlertSettings

    Private Function GetEndTime(h1 As HighAlertRecord, h2 As HighAlertRecord) As String
        Dim endTime As String = h1.End
        If h1.End Is Nothing OrElse h1.End.Length = 0 Then
            endTime = h2.Start
        End If

        Return endTime
    End Function

    <Extension>
    Friend Sub AlertSettings1HighAlert(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            Dim h1 As HighAlertRecord =
                pdf.HighAlerts.HighAlert(index:=0)
            Dim h2 As HighAlertRecord =
                pdf.HighAlerts.HighAlert(index:=1)
            .AppendKeyValue(leftPanel:=False,
                            title:="",
                            value:=h1.Name,
                            secondValue:=h2.Name)
            .AppendKeyValue(leftPanel:=False,
                            title:="",
                            value:=$"{h1.Start}",
                            secondValue:=$"{h2.Start}")
            .AppendKeyValue(leftPanel:=False,
                            title:=$"{Indent8}Alert Before High:",
                            value:=$"{h1.AlertBeforeHigh}",
                            secondValue:=$"{h2.AlertBeforeHigh}")
            If Not pdf.IsFlex Then
                .AppendKeyValue(leftPanel:=False,
                                title:=$"{Indent8}Time Before High:",
                                value:=$"{h1.TimeBeforeHigh}",
                                secondValue:=$"{h2.TimeBeforeHigh}")
            End If
            .AppendKeyValue(leftPanel:=False,
                            title:=$"{Indent8}Alert on High:",
                            value:=$"{h1.AlertOnHigh}",
                            secondValue:=$"{h2.AlertOnHigh}")
            .AppendKeyValue(leftPanel:=False,
                            title:=$"{Indent8}Rise Alert:",
                            value:=h1.RiseAlert,
                            secondValue:=h2.RiseAlert)
            .AppendNewLine
        End With
    End Sub

    <Extension>
    Friend Sub AlertSettings2LowAlert(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            Dim l1 As LowAlertRecord =
                pdf.LowAlerts.LowAlert(index:=0)
            Dim l2 As LowAlertRecord =
                pdf.LowAlerts.LowAlert(index:=1)

            .AppendKeyValue(leftPanel:=False,
                            title:="",
                            value:=l1.Name,
                            secondValue:=l2.Name,
                            indent:=Indent4)

            .AppendKeyValue(leftPanel:=False,
                            title:="",
                            value:=l1.Start,
                            secondValue:=l2.Start,
                            indent:=Indent4)

            .AppendKeyValue(leftPanel:=False,
                            title:="Fall Alert on Low:",
                            value:=l1.FallAlert,
                            secondValue:=l2.FallAlert,
                            indent:=Indent4)

            .AppendKeyValue(leftPanel:=False,
                            title:="Alert on Low:",
                            value:=l1.AlertOnLow,
                            secondValue:=l2.AlertOnLow,
                            indent:=Indent4)

            .AppendKeyValue(leftPanel:=False,
                            title:="Alert Before Low:",
                            value:=l1.AlertBeforeLow,
                            secondValue:=l2.AlertBeforeLow,
                            indent:=Indent4)

            .AppendKeyValue(leftPanel:=False,
                            title:="Max Volume At Night:",
                            value:=l1.MaxVolumeAtNight,
                            secondValue:=l2.MaxVolumeAtNight,
                            indent:=Indent4)

            .AppendKeyValue(leftPanel:=False,
                            title:="Snooze Duration:",
                            value:=l1.SnoozeDuration,
                            secondValue:=l2.SnoozeDuration,
                            indent:=Indent4)

            .AppendKeyValue(leftPanel:=False,
                            title:="Suspend Limit:",
                            value:=l1.SuspendLimit,
                            secondValue:=l2.SuspendLimit,
                            indent:=Indent4)

            .AppendKeyValue(leftPanel:=False,
                            title:="Suspend Before Low:",
                            value:=l1.SuspendBeforeLow,
                            secondValue:=l2.SuspendBeforeLow,
                            indent:=Indent4)

            .AppendKeyValue(leftPanel:=False,
                            title:="Alert For Suspend Before Low:",
                            value:=l1.AlertForSuspendBeforeLow,
                            secondValue:=l2.AlertForSuspendBeforeLow,
                            indent:=Indent4)

            .AppendKeyValue(leftPanel:=False,
                            title:="Suspend on Low:",
                            value:=l1.SuspendOnLow,
                            secondValue:=l2.SuspendOnLow,
                            indent:=Indent4)

            If l1.ValueUnits Is Nothing Then ' its Flex
                .AppendKeyValue(leftPanel:=False,
                                title:=$"Carb Units:",
                                value:=$"Grams",
                                indent:=Indent4)

                .AppendKeyValue(leftPanel:=False,
                                title:=$"BG Units:",
                                value:=l1.SuspendLimit.Split(separator:=" ")(1),
                                indent:=Indent4)
                Exit Sub
            End If

            Dim valueUnits As String() = l1.ValueUnits.Split(separator:=",")
            .AppendKeyValue(leftPanel:=False,
                            title:=$"Carb Units:",
                            value:=$"{valueUnits(0)}",
                            indent:=Indent4)

            .AppendKeyValue(leftPanel:=False,
                            title:=$"BG Units:",
                            value:=$"{valueUnits(1)}",
                            indent:=Indent4)

            ' legacy property names for compatibility with existing code
            If Not pdf.IsFlex Then
                .AppendKeyValue(leftPanel:=False,
                                title:="Resume Basal Alert:",
                                value:=l1.ResumeBasalAlert,
                                secondValue:=l2.ResumeBasalAlert,
                                indent:=Indent4)

                .AppendKeyValue(leftPanel:=False,
                                title:=$"Suspend:",
                                value:=l1.Suspend,
                                secondValue:=l2.Suspend,
                                indent:=Indent4)
            End If

            .AppendNewLine
        End With
    End Sub

    <Extension>
    Friend Sub AlertSettings4Reminders(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            Dim symbol As String = Gear
            .AppendKeyValue(leftPanel:=False,
                            title:="Low Reservoir Warning:",
                            value:=$"{pdf.Reminders.LowReservoirWarning}")
            .AppendKeyValue(leftPanel:=False,
                            title:="Type:",
                            value:="Units")
            .AppendKeyValue(leftPanel:=False,
                            title:="Units:",
                            value:=$"{pdf.Reminders.Amount}")
            .AppendNewLine

            .AppendTextWithSymbol(text:=$"Menu>{Gear}>Alert Settings>Reminders > Set Change", symbol)
            .AppendKeyValue(leftPanel:=False,
                            title:="Set Change:",
                            value:=pdf.Reminders.SetChange.OnOff,
                            secondValue:=pdf.Reminders.SetChange.TimeLeft)
            .AppendNewLine

            .AppendTextWithSymbol(text:=$"Menu>{Gear}>Alert Settings>Reminders > Sensor Ending", symbol)
            .AppendKeyValue(leftPanel:=False,
                            title:="Less than 24 hours:",
                            value:=$"{pdf.Sensor.SensorEnding.LessThan24Hours}")
            .AppendKeyValue(leftPanel:=False,
                            title:="Custom Reminder:",
                            value:=pdf.Sensor.SensorEnding.LessThan2State)
            .AppendKeyValue(leftPanel:=False,
                            title:="Less than:",
                            value:=pdf.Sensor.SensorEnding.LessThan2)
            .AppendNewLine

            Dim text As String
            If Not pdf.IsFlex Then
                .AppendTextWithSymbol(text:=$"Menu>{Gear}>Alert Settings>Reminders > Bolus BG Check", symbol)
                .AppendKeyValue(leftPanel:=False,
                                title:="Reminder:",
                                value:=$"{pdf.Reminders.BolusBgCheck}")
                .AppendNewLine

                text = $"Menu>{Gear}>Alert Settings>Reminders > Sensor Info"
                .AppendTextWithSymbol(text, symbol)

                Dim sensorRecord As SensorRecord = pdf.Reminders.LowSensorLife
                .AppendKeyValue(leftPanel:=False,
                                title:="Sensor On:",
                                value:=sensorRecord.SensorOn)
                If Not pdf.IsFlex Then
                    .AppendKeyValue(leftPanel:=False,
                                    title:="AutoCalibration",
                                    value:=sensorRecord.AutoCalibration)
                    .AppendKeyValue(leftPanel:=False,
                                    title:="Calibration Reminder",
                                    value:=sensorRecord.CalibrationReminder,
                                    secondValue:=sensorRecord.CalibrationReminderTime)
                End If
            End If

            If Not pdf.IsFlex Then
                text = $"Menu>{Gear}>Alert Settings>Reminders > Missed Meal"
                .AppendTextWithSymbol(text, symbol)
                For Each item As KeyValuePair(Of String, MealStartEndRecord) In pdf.Reminders.MissedMealBolus
                    .AppendKeyValue(leftPanel:=False,
                                    title:=item.Key,
                                    value:=item.Value.Start,
                                    secondValue:=item.Value.End)
                Next
                .AppendNewLine()

                text = $"Menu>{Gear}>Alert Settings>Reminders > Personal"
                .AppendTextWithSymbol(text, symbol)
                For Each item As KeyValuePair(Of String, PersonalRemindersRecord) In pdf.Reminders.PersonalReminders
                    .AppendKeyValue(leftPanel:=False,
                                    title:=item.Key,
                                    value:=item.Value.Time)
                Next
                .AppendNewLine
            End If

        End With
    End Sub

End Module
