' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
'''  Learns the regular device upload phase without treating event uploads as cycle changes.
''' </summary>
Friend NotInheritable Class LivePollingSchedule
    Private Const AlignedWindowMs As Long = 5000
    Private Const CycleMs As Long = 300000
    Private Const MaximumSampleAgeMs As Long = 900000
    Private Const MaximumUnixTimeMs As Long = 253402300799999
    Private Const PhaseToleranceMs As Long = 30000
    Private Const PollMs As Long = 60000
    Private Const UploadMarginMs As Long = 5000
    Private _confirmed As Boolean
    Private _deviceTime As Long
    Private _observationTicks As Long
    Private _serverTime As Long
    Private _serverUpdateTime As Long
    Friend Const MinimumPollMs As Integer = 50000

    Friend Function NextInterval(nowTicks As Long) As Integer
        If Not _confirmed OrElse nowTicks < _observationTicks Then
            Return CInt(PollMs)
        End If

        Dim elapsedMs As Double = (nowTicks - _observationTicks) * 1000.0 / Stopwatch.Frequency
        If elapsedMs + _serverTime - _serverUpdateTime > MaximumSampleAgeMs Then
            Return CInt(PollMs)
        End If

        Dim serverNow As Long = _serverTime + CLng(elapsedMs)
        ' A five-minute cycle is a multiple of the normal one-minute poll period.
        ' Shorten polls gradually until their phase is just after the server update.
        Dim phase As Long = (serverNow + PollMs - _serverUpdateTime - UploadMarginMs) Mod PollMs
        If phase < 0 Then phase += PollMs
        If phase <= AlignedWindowMs Then Return CInt(PollMs)
        Return CInt(PollMs - Math.Min(phase, PollMs - MinimumPollMs))
    End Function

    Friend Sub Observe(data As PatientDataInfo, observationTicks As Long)
        If data Is Nothing OrElse data.Sgs Is Nothing OrElse data.Sgs.Count = 0 OrElse
           data.Sgs(index:=0) Is Nothing OrElse Not data.Sgs(index:=0).Sg.IsSgValid OrElse
           data.MedicalDeviceTime <= 0 OrElse data.MedicalDeviceTime > MaximumUnixTimeMs OrElse
           data.LastMedicalDeviceDataUpdateServerTime <= 0 OrElse
           data.CurrentServerTime > MaximumUnixTimeMs OrElse
           data.CurrentServerTime < data.LastMedicalDeviceDataUpdateServerTime OrElse
           data.CurrentServerTime - data.LastMedicalDeviceDataUpdateServerTime > MaximumSampleAgeMs Then
            Return
        End If

        If data.MedicalDeviceTime = _deviceTime Then
            Return
        End If

        Dim deviceDelta As Long = data.MedicalDeviceTime - _deviceTime
        Dim serverDelta As Long = data.LastMedicalDeviceDataUpdateServerTime - _serverUpdateTime
        Dim cycles As Long = CLng(Math.Round(value:=deviceDelta / CycleMs, mode:=MidpointRounding.AwayFromZero))
        _confirmed = _deviceTime > 0 AndAlso cycles >= 1 AndAlso cycles <= 3 AndAlso
                     Math.Abs(value:=deviceDelta - (cycles * CycleMs)) <= PhaseToleranceMs AndAlso
                     Math.Abs(value:=serverDelta - (cycles * CycleMs)) <= PhaseToleranceMs
        _deviceTime = data.MedicalDeviceTime
        _serverUpdateTime = data.LastMedicalDeviceDataUpdateServerTime
        _serverTime = data.CurrentServerTime
        _observationTicks = observationTicks
    End Sub

    Friend Sub Reset()
        _deviceTime = 0
        _serverUpdateTime = 0
        _serverTime = 0
        _observationTicks = 0
        _confirmed = False
    End Sub

End Class
