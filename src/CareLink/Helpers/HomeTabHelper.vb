' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Module HomeTabHelper
    <Extension>
    Friend Sub UpdateTransmitterBatttery(MainForm As Form1)
        Dim gstBatteryLevel As Integer = s_listOfSummaryRecords.GetValue(Of Integer)(NameOf(ItemIndexs.gstBatteryLevel), False)
        MainForm.TransmatterBatterPercentLabel.Text = $"{gstBatteryLevel}%"
        If s_listOfSummaryRecords.GetValue(Of Boolean)(NameOf(ItemIndexs.conduitSensorInRange)) Then
            Select Case gstBatteryLevel
                Case 100
                    MainForm.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryFull
                Case > 50
                    MainForm.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryOK
                Case > 20
                    MainForm.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryMedium
                Case > 0
                    MainForm.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryLow
            End Select
        Else
            MainForm.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryUnknown
            MainForm.TransmatterBatterPercentLabel.Text = $"???"
        End If

    End Sub

End Module
