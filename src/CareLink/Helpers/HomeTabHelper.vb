' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Module HomeTabHelper
    <Extension>
    Friend Sub UpdateTransmitterBatttery(MeForm As Form1)
        Dim gstBatteryLevel As Integer = CInt(s_listOfSummaryRecords.GetValue(NameOf(ItemIndexs.gstBatteryLevel), False))
        MeForm.TransmatterBatterPercentLabel.Text = $"{gstBatteryLevel}%"
        If CBool(s_listOfSummaryRecords.GetValue(NameOf(ItemIndexs.conduitSensorInRange))) Then
            Select Case gstBatteryLevel
                Case 100
                    MeForm.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryFull
                Case > 50
                    MeForm.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryOK
                Case > 20
                    MeForm.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryMedium
                Case > 0
                    MeForm.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryLow
            End Select
        Else
            MeForm.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryUnknown
            MeForm.TransmatterBatterPercentLabel.Text = $"???"
        End If

    End Sub

End Module
