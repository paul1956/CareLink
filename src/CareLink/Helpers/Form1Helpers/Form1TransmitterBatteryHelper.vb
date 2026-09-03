' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module Form1TransmitterBatteryHelper

    <Extension>
    Private Sub GetBatteryImage(pictureBox As PictureBox, gstBatteryLevel As Integer)
        Select Case gstBatteryLevel
            Case 100
                pictureBox.GetBitmapFromCache(imageId:=ImageEnum.TransmitterBatteryFull)
            Case > 50
                pictureBox.GetBitmapFromCache(imageId:=ImageEnum.TransmitterBatteryOK)
            Case > 20
                pictureBox.GetBitmapFromCache(imageId:=ImageEnum.TransmitterBatteryMedium)
            Case > 0
                pictureBox.GetBitmapFromCache(imageId:=ImageEnum.TransmitterBatteryLow)
            Case Else
                pictureBox.GetBitmapFromCache(imageId:=ImageEnum.TransmitterBatteryUnknown)
        End Select
    End Sub

    Private Sub Form1SensorDataUpdate()
        If PatientData.ConduitSensorInRange Xor PatientData.AppModelType = "INSTINCT_10" Then
            If PatientData.CgmInfo.SensorType = "DURABLE" Then
                Form1.TransmitterBatteryPictureBox.GetBatteryImage(PatientData.GstBatteryLevel)
                Form1.TransmitterBatteryPercentLabel.Text = $"{PatientData.GstBatteryLevel}%"
            Else
                Select Case $"{PatientData.CgmInfo?.SensorProductModel}".TrimEnd
                    Case "MMT-5120"
                        Form1.TransmitterBatteryPictureBox.GetBitmapFromCache(imageId:=ImageEnum.PumpConnectivityToSimpleraOK)
                        Form1.TransmitterBatteryPercentLabel.Text =
                            $"Simplera{vbCrLf}Connected"

                    Case "MMT-1894"
                        Form1.TransmitterBatteryPictureBox.GetBitmapFromCache(imageId:=ImageEnum.PumpConnectivityToInstinctOK)
                        Form1.TransmitterBatteryPercentLabel.Text =
                            $"Instinct{vbCrLf}Connected"

                    Case Else
                        ' default for Disposible sensor
                        Form1.TransmitterBatteryPictureBox.GetBitmapFromCache(imageId:=ImageEnum.PumpConnectivityToSimpleraOK)
                        Form1.TransmitterBatteryPercentLabel.Text =
                            $"Simplera{vbCrLf}Connected"
                End Select
            End If
        Else
            Form1.TransmitterBatteryPictureBox.GetBitmapFromCache(imageId:=ImageEnum.PumpConnectivityToTransmitterNotOK)
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
