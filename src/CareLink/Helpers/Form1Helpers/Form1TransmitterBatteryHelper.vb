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

    Friend Sub UpdateTransmitterBattery()
        If PatientData.ConduitSensorInRange Then
            Dim gstBatteryLevel As Integer = s_listOfSummaryRecords.GetValue(Of Integer)(
                key:=NameOf(ServerDataIndexes.gstBatteryLevel),
                throwError:=False)
            Form1.TransmitterBatteryPictureBox.Image = GetBatteryImage(gstBatteryLevel)
            Form1.TransmitterBatteryPercentLabel.Text = $"{gstBatteryLevel}%"
        Else
            Form1.TransmitterBatteryPictureBox.Image = My.Resources.PumpConnectivityToTransmitterNotOK
            Form1.TransmitterBatteryPercentLabel.Text = "N/A"
        End If

    End Sub

End Module
