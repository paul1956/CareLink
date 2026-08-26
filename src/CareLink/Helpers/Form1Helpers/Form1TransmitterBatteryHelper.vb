' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module Form1TransmitterBatteryHelper

    Private Function GetBatteryImage(gstBatteryLevel As Integer) As Image
        Select Case gstBatteryLevel
            Case 100
                Return GetBitmapFromCache(id:=ImageEnum.TransmitterBatteryFull)
            Case > 50
                Return GetBitmapFromCache(id:=ImageEnum.TransmitterBatteryOK)
            Case > 20
                Return GetBitmapFromCache(id:=ImageEnum.TransmitterBatteryMedium)
            Case > 0
                Return GetBitmapFromCache(id:=ImageEnum.TransmitterBatteryLow)
            Case Else
                Return GetBitmapFromCache(id:=ImageEnum.TransmitterBatteryUnknown)
        End Select
    End Function

    Private Sub Form1SensorDataUpdate()
        If PatientData.ConduitSensorInRange Xor PatientData.AppModelType = "INSTINCT_10" Then
            If PatientData.CgmInfo.SensorType = "DURABLE" Then
                Form1.TransmitterBatteryPictureBox.Image = GetBatteryImage(PatientData.GstBatteryLevel)
                Form1.TransmitterBatteryPercentLabel.Text = $"{PatientData.GstBatteryLevel}%"
            Else
                Select Case $"{PatientData.CgmInfo?.SensorProductModel}".TrimEnd
                    Case "MMT-5120"
                        Form1.TransmitterBatteryPictureBox.Image =
                            GetBitmapFromCache(id:=ImageEnum.PumpConnectivityToSimpleraOK)
                        Form1.TransmitterBatteryPercentLabel.Text =
                            $"Simplera{vbCrLf}Connected"

                    Case "MMT-1894"
                        Form1.TransmitterBatteryPictureBox.Image =
                            GetBitmapFromCache(id:=ImageEnum.PumpConnectivityToInstinctOK)
                        Form1.TransmitterBatteryPercentLabel.Text =
                            $"Instinct{vbCrLf}Connected"

                    Case Else
                        ' default for Disposible sensor
                        Form1.TransmitterBatteryPictureBox.Image =
                            GetBitmapFromCache(id:=ImageEnum.PumpConnectivityToSimpleraOK)
                        Form1.TransmitterBatteryPercentLabel.Text =
                            $"Simplera{vbCrLf}Connected"
                End Select
            End If
        Else
            Form1.TransmitterBatteryPictureBox.Image =
            GetBitmapFromCache(id:=ImageEnum.PumpConnectivityToTransmitterNotOK)
            Form1.TransmitterBatteryPercentLabel.Text = "N/A"
        End If
    End Sub

    ''' <summary>
    ''' Updates the sensor status on Form1.
    ''' </summary>
    Friend Sub ThreadSafeForm1SensorDataUpdate()
        Dim method As Action =
            Sub()
                Form1SensorDataUpdate()
            End Sub
        Invoke(owner:=My.Forms.Form1, method)
    End Sub

End Module
