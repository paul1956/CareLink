' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module Form1TransmitterBatteryHelper

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

    ''' <summary>
    ''' Updates the sensor status on Form1.
    ''' </summary>
    Friend Sub UpdateSensorData()
        If PatientData.ConduitSensorInRange Then
            If PatientData.CgmInfo.SensorType = "DURABLE" Then
                Form1.TransmitterBatteryPictureBox.Image = GetBatteryImage(PatientData.GstBatteryLevel)
                Form1.TransmitterBatteryPercentLabel.Text = $"{PatientData.GstBatteryLevel}%"
            ElseIf PatientData.CgmInfo.SensorProductModel.TrimEnd = "MMT-5120" Then
                Form1.TransmitterBatteryPictureBox.Image = My.Resources.PumpConnectivityToSimpleraOK
                Form1.TransmitterBatteryPercentLabel.Text = $"Simplera{vbCrLf}Connected"
            ElseIf PatientData.CgmInfo.SensorProductModel.TrimEnd = "MMT-1894" Then
                Form1.TransmitterBatteryPictureBox.Image = My.Resources.PumpConnectivityToInstinctOK
                Form1.TransmitterBatteryPercentLabel.Text = $"Instinct{vbCrLf}Connected"
            Else
                Stop
            End If
        Else
            Form1.TransmitterBatteryPictureBox.Image = My.Resources.PumpConnectivityToTransmitterNotOK
            Form1.TransmitterBatteryPercentLabel.Text = "N/A"
        End If

    End Sub

End Module
