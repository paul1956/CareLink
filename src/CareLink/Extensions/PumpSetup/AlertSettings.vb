' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module AlertSettings

    <Extension>
    Public Sub AlertSettings1HighAlert(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            For Each h As HighAlertRecord In pdf.HighAlerts.HighAlert
                .AppendTimeValueRow(
                    startTime:=h.Start,
                    endTime:=h.End,
                    value:=$"{h.HighLimit}",
                    timeFormat:=pdf.Utilities.TimeFormat,
                    indent:=Indent4,
                    heading:=True)
                .AppendKeyValue(
                    key:="Alert Before High:",
                    value:=h.AlertBeforeHigh.BoolToOnOff(),
                    indent:=Indent8)
                .AppendKeyValue(
                    key:="Time Before High:",
                    value:=h.TimeBeforeHigh,
                    indent:=Indent8)
                .AppendKeyValue(key:="Alert on High:",
                    value:=h.AlertOnHigh.BoolToOnOff(),
                    indent:=Indent8)
                .AppendKeyValue(key:="Rise Alert:",
                    value:=h.RiseAlert.BoolToOnOff(),
                    indent:=Indent8)
            Next
            .AppendNewLine
        End With
    End Sub

    <Extension>
    Public Sub AlertSettings2LowAlert(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            For Each l As LowAlertRecord In pdf.LowAlerts.LowAlert
                rtb.AppendTimeValueRow(
                    startTime:=l.Start,
                    endTime:=l.End,
                    value:=$"{l.LowLimit}",
                    timeFormat:=pdf.Utilities.TimeFormat,
                    indent:=Indent4, heading:=True)
                .AppendKeyValue(key:=$"Suspend:", value:=$"{l.Suspend}", indent:=Indent8)
                .AppendKeyValue(
                    key:="Alert Before Low:",
                    value:=l.AlertBeforeLow.BoolToOnOff(),
                    indent:=Indent8)
                .AppendKeyValue(key:="Alert on Low:",
                    value:=l.AlertOnLow.BoolToOnOff(),
                    indent:=Indent8)
                .AppendKeyValue(key:="Resume Basal Alert:",
                    value:=$"{l.ResumeBasalAlert}",
                    indent:=Indent8)
            Next
            .AppendNewLine
        End With
    End Sub

    <Extension>
    Public Sub AlertSettings4Reminders(rtb As RichTextBox, pdf As PdfSettingsRecord)
        With rtb
            Dim symbol As String = Gear
            .AppendKeyValue(
                    key:="Low Reservoir Warning:",
                    value:=$"{pdf.Reminders.LowReservoirWarning}")
            .AppendKeyValue(key:="Type:", value:="Units")
            .AppendKeyValue(key:="Units:", value:=$"{pdf.Reminders.Amount}")
            .AppendNewLine

            .AppendTextWithSymbol(
            text:=$"Menu>{Gear}>Alert Settings>Reminders > Set Change", symbol)
            .AppendKeyValue(key:="Set Change:", value:=$"{pdf.Reminders.SetChange}")

            .AppendNewLine
            .AppendTextWithSymbol(
            text:=$"Menu>{Gear}>Alert Settings>Reminders > Bolus BG Check", symbol)
            .AppendKeyValue(key:="Reminder:", value:=$"{pdf.Reminders.BolusBgCheck}")

            .AppendNewLine
            .AppendTextWithSymbol(
            text:=$"Menu>{Gear}>Alert Settings>Reminders > Missed Meal", symbol)

            For Each item As KeyValuePair(Of String, MealStartEndRecord) In
            pdf.Reminders.MissedMealBolus

                Dim startTime As String = item.Value.Start
                Dim endTime As String = item.Value.End
                .AppendTimeValueRow(item.Key, startTime, endTime)
            Next

            .AppendNewLine
            .AppendTextWithSymbol(
            text:=$"Menu>{Gear}>Alert Settings>Reminders > Personal", symbol)
            For Each item As KeyValuePair(Of String, PersonalRemindersRecord) In
                pdf.Reminders.PersonalReminders

                .AppendTimeValueRow(key:=item.Key, startTime:=item.Value.Time)
            Next
            .AppendNewLine
        End With
    End Sub

End Module
