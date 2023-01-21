' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module TransmitterBatteryHelper

    Private Function GetBatteryImage(gstBatteryLevel As Integer) As Image
        Select Case gstBatteryLevel
            Case 100
                Return My.Resources.TransmitterBatteryFull
            Case > 50
                Return My.Resources.TransmitterBatteryOK
            Case > 20
                Return My.Resources.TransmitterBatteryMedium
            Case > 0
                Return My.Resources.TransmitterBatteryLow
            Case Else
                Return My.Resources.TransmitterBatteryUnknown
        End Select
    End Function

    <Extension>
    Friend Sub UpdateTransmitterBattery(MainForm As Form1)
        If s_listOfSummaryRecords.GetValue(Of Boolean)(NameOf(ItemIndexes.conduitSensorInRange)) Then
            Dim gstBatteryLevel As Integer = s_listOfSummaryRecords.GetValue(Of Integer)(NameOf(ItemIndexes.gstBatteryLevel), False)
            MainForm.TransmitterBatteryPictureBox.Image = GetBatteryImage(gstBatteryLevel)
            MainForm.TransmitterBatteryPercentLabel.Text = $"{gstBatteryLevel}%"
        Else
            MainForm.TransmitterBatteryPictureBox.Image = My.Resources.TransmitterBatteryUnknown
            MainForm.TransmitterBatteryPercentLabel.Text = $"???"
        End If

    End Sub

End Module
