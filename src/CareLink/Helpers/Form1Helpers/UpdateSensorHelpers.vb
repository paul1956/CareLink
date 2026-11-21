' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module UpdateSensorHelpers

    ''' <summary>
    '''  Updates the sensor life display for Simplera sensors on the home tab.
    '''  This method updates the sensor days left label and sensor time left
    '''  picture box based on the current sensor duration hours for Simplera sensors.
    ''' </summary>
    ''' <remarks>
    '''  The sensor days left label is set based on the remaining sensor duration,
    '''  and the sensor time left picture box is updated accordingly for Simplera sensors.
    ''' </remarks>
    <Extension>
    Private Function GetInstinctTimeLeftMessage(SensorDurationHours As Integer) As String
        If SensorDurationHours <= 24 Then
            Return $"{SensorDurationHours} hour{If(SensorDurationHours = 1, String.Empty, "s")}{vbCrLf}left"
        End If
        Dim timeLeft() As String =
            SensorDurationHours.HoursToDaysAndHours(shortHr:=True).Split(separator:=", ")
        Return If(timeLeft.Length = 2,
                  $"{timeLeft(0)}{vbCrLf}{timeLeft(1)}{vbCrLf}left",
                  $"{timeLeft(0)}{vbCrLf}left")
    End Function

    ''' <summary>
    '''  Updates the sensor life display for G4 sensors on the home tab.
    '''  This method updates the sensor days left label and sensor time left
    '''  picture box based on the current sensor duration hours for G4 sensors.
    ''' </summary>
    ''' <remarks>
    '''  The sensor days left label is set based on the remaining sensor duration,
    '''  and the sensor time left picture box is updated accordingly for G4 sensors.
    ''' </remarks>
    <Extension>
    Private Sub UpdateSensorLifeG4(f As Form1)
        Dim sensorDurationHours As Integer = PatientData.SensorDurationHours
        Dim sensorDurationDays As Integer = CInt(Math.Ceiling(a:=sensorDurationHours / 24))
        Select Case sensorDurationHours
            Case Is >= 255
                f.SensorDaysLeftLabel.Text = EmptyString
                f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorExpirationUnknown)
                f.SensorTimeLeftLabel.Text = "Unknown"
            Case Is >= 168
                f.SensorDaysLeftLabel.Text = "~7"
                f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorLifeOK)
                f.SensorTimeLeftLabel.Text = "7 Days"
            Case Is >= 24
                f.SensorDaysLeftLabel.Text = sensorDurationDays.ToString()
                f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorLifeOK)
                f.SensorTimeLeftLabel.Text = sensorDurationDays.ToUnits(unit:="Day")
            Case Is > 0
                f.SensorDaysLeftLabel.Text =
                                $"<{sensorDurationDays}"
                f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorLifeNotOK)
                f.SensorTimeLeftLabel.Text =
                                sensorDurationHours.ToUnits(unit:="Hour", separator:=vbCrLf)
            Case Is = 0
                Dim sensorDurationMinutes As Integer =
                                If(PatientData.SensorDurationMinutes Is Nothing,
                                   -1,
                                   CInt(PatientData.SensorDurationMinutes))
                Select Case sensorDurationMinutes
                    Case Is > 0
                        f.SensorDaysLeftLabel.Text = "0"
                        f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorLifeNotOK)
                        f.SensorTimeLeftLabel.Text = sensorDurationMinutes.ToUnits(unit:="minute", separator:=vbCrLf)
                    Case Is = 0
                        f.SensorDaysLeftLabel.Text = EmptyString
                        f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorExpired)
                        f.SensorTimeLeftLabel.Text = "Expired"
                    Case Else
                        f.SensorDaysLeftLabel.Text = EmptyString
                        Const imageId As ImageEnum = ImageEnum.SensorExpirationUnknown
                        f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId)
                        f.SensorTimeLeftLabel.Text = "Unknown"
                End Select
        End Select
    End Sub

    ''' <summary>
    '''  Updates the sensor life display for Instinct sensors on the home tab.
    '''  This method updates the sensor days left label and sensor time left
    '''  picture box based on the current sensor duration hours for Instinct sensors.
    ''' </summary>
    ''' <remarks>
    '''  The sensor days left label is set based on the remaining sensor duration,
    '''  and the sensor time left picture box is updated accordingly for Instinct sensors.
    ''' </remarks>
    <Extension>
    Private Sub UpdateSensorLifeInstinct(f As Form1)
        Dim sensorDurationHours As Integer = PatientData.SensorDurationHours
        Select Case sensorDurationHours
            Case Is >= 360
                f.SensorDaysLeftLabel.Text = EmptyString
                f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorExpirationUnknown)
                f.SensorTimeLeftLabel.Text = "Unknown"
            Case Is > 24
                f.SensorDaysLeftLabel.Text =
                    Math.Ceiling(a:=sensorDurationHours / 24).ToString()
                f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorLifeOK)
                f.SensorTimeLeftLabel.Text =
                    GetInstinctTimeLeftMessage(sensorDurationHours)
            Case Is = 0
                Dim sensorDurationMinutes As Integer =
                    If(PatientData.SensorDurationMinutes Is Nothing,
                       -1,
                       CInt(PatientData.SensorDurationMinutes))
                Select Case sensorDurationMinutes
                    Case Is > 0
                        f.SensorDaysLeftLabel.Text = "0"
                        f.SensorTimeLeftLabel.Text = sensorDurationMinutes.ToUnits(unit:="minute", separator:=vbCrLf)
                        f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorLifeNotOK)
                    Case Is = 0
                        f.SensorDaysLeftLabel.Text = EmptyString
                        f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorExpired)
                        f.SensorTimeLeftLabel.Text = "Expired"
                    Case Else
                        f.SensorDaysLeftLabel.Text = EmptyString
                        f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorExpirationUnknown)
                        f.SensorTimeLeftLabel.Text = "Unknown"
                End Select
            Case Else
                f.SensorDaysLeftLabel.Text = EmptyString
                f.SensorTimeLeftLabel.Text = "Unknown"
                f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorExpirationUnknown)
        End Select
        f.SensorDaysLeftLabel.AdjustFontToFitWidth(maxWidth:=70)
    End Sub

    Friend Function GetSimpleraTimeLeftMessage(SensorDurationHours As Integer) As String
        Dim hoursStr As String
        Select Case SensorDurationHours
            Case Is <= 24
                hoursStr = SensorDurationHours.HoursToDaysAndHours(shortHr:=True)
                Return $"{hoursStr} left{vbCrLf}in grace period"
            Case Is < 48
                hoursStr = (SensorDurationHours - 24).HoursToDaysAndHours(shortHr:=True)
                Return $"{hoursStr}{vbCrLf}+ 24 hour{vbCrLf}grace period"
            Case Else  ' sensorDurationHours >= 48
                hoursStr = (SensorDurationHours - 24).HoursToDaysAndHours(shortHr:=True)
                Return $"{hoursStr}{vbCrLf}+ 24 hour{vbCrLf}grace period"
        End Select
    End Function

    ''' <summary>
    '''  Updates the sensor life display on the home tab.
    '''  This method updates the sensor days left label and sensor time left
    '''  picture box based on the current sensor duration hours.
    ''' </summary>
    ''' <remarks>
    '''  The sensor days left label is set based on the remaining sensor duration,
    '''  and the sensor time left picture box is updated accordingly.
    ''' </remarks>
    <Extension>
    Friend Sub UpdateSensorLife(f As Form1)
        Dim haveCGM As Boolean = PatientData.ConduitInRange AndAlso PatientData.CgmInfo IsNot Nothing
        Dim sensorModelNumber As String
        sensorModelNumber = PatientData.CgmInfo.SensorProductModel?.Trim
        If haveCGM Then
            If sensorModelNumber Is Nothing Then
                If PatientData.CgmInfo.SensorType = "DISPOSIBLE" Then
                    f.UpdateSensorLifeSimplera()
                Else
                    ' Instinct
                    f.UpdateSensorLifeInstinct()
                End If
            ElseIf sensorModelNumber = "MMT-5120" Then
                f.UpdateSensorLifeSimplera()
            Else
                f.UpdateSensorLifeG4()
            End If
        Else
            f.SensorDaysLeftLabel.Text = EmptyString
            f.SensorTimeLeftLabel.Text = "Unknown"
            f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorExpirationUnknown)
            f.SensorTimeLeftPanel.Visible = True
        End If
        f.SensorDaysLeftLabel.Visible = f.SensorDaysLeftLabel.Text <> String.Empty
        f.SensorTimeLeftPanel.Visible = PatientData.ConduitInRange
    End Sub

    ''' <summary>
    '''  Updates the sensor life display for Simplera sensors on the home tab.
    '''  This method updates the sensor days left label and sensor time left
    '''  picture box based on the current sensor duration hours for Simplera sensors.
    ''' </summary>
    ''' <remarks>
    '''  The sensor days left label is set based on the remaining sensor duration,
    '''  and the sensor time left picture box is updated accordingly for Simplera sensors.
    ''' </remarks>
    <Extension>
    Friend Sub UpdateSensorLifeSimplera(f As Form1)
        Dim sensorDurationHours As Integer = PatientData.SensorDurationHours
        Dim durationWithoutGrace As Integer = sensorDurationHours - 24

        Select Case sensorDurationHours
            Case Is >= 255
                f.SensorDaysLeftLabel.Text = EmptyString
                f.SensorTimeLeftLabel.Text = "Unknown"
                f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorExpirationUnknown)
            Case Is >= 48
                f.SensorDaysLeftLabel.Text = CStr(Math.Ceiling(a:=durationWithoutGrace / 24))
                f.SensorTimeLeftLabel.Text = GetSimpleraTimeLeftMessage(sensorDurationHours)
                f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorLifeOK)
            Case Is > 24
                f.SensorDaysLeftLabel.Text =
                    Math.Ceiling(a:=durationWithoutGrace / 24).ToString()
                f.SensorTimeLeftLabel.Text =
                    GetSimpleraTimeLeftMessage(sensorDurationHours)
                f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorLifeOK)
            Case Is > 0 ' Grace
                f.SensorDaysLeftLabel.Text = EmptyString
                f.SensorTimeLeftLabel.Text =
                    GetSimpleraTimeLeftMessage(sensorDurationHours)
                f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorExpiringSoon)
            Case Is = 0
                Dim sensorDurationMinutes As Integer =
                    If(PatientData.SensorDurationMinutes Is Nothing,
                       -1,
                       CInt(PatientData.SensorDurationMinutes))
                Select Case sensorDurationMinutes
                    Case Is > 0
                        f.SensorDaysLeftLabel.Text = "0"
                        f.SensorTimeLeftLabel.Text = sensorDurationMinutes.ToUnits(unit:="minute", separator:=vbCrLf)
                        f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorLifeNotOK)
                    Case Is = 0
                        f.SensorDaysLeftLabel.Text = EmptyString
                        f.SensorTimeLeftLabel.Text = "Expired"
                        f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorExpired)
                    Case Else
                        f.SensorDaysLeftLabel.Text = EmptyString
                        f.SensorTimeLeftLabel.Text = "Unknown"
                        f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorExpirationUnknown)
                End Select
            Case Else
                f.SensorDaysLeftLabel.Text = EmptyString
                f.SensorTimeLeftLabel.Text = "Unknown"
                f.SensorTimeLeftPictureBox.UpdatePictureBox(imageId:=ImageEnum.SensorExpirationUnknown)
        End Select
        f.SensorDaysLeftLabel.AdjustFontToFitWidth(maxWidth:=70)
    End Sub

End Module
