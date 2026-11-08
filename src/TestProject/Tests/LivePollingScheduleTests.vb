' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Diagnostics
Imports CareLink
Imports Xunit

Public Class LivePollingScheduleTests
    Private Const DeviceBase As Long = 1000000000
    Private Const ServerBase As Long = 2000000000

    Private Shared Function Sample(deviceTime As Long, updateTime As Long, serverTime As Long,
                                   Optional sg As Single = 100) As PatientDataInfo
        Return New PatientDataInfo With {
            .MedicalDeviceTime = deviceTime,
            .LastMedicalDeviceDataUpdateServerTime = updateTime,
            .CurrentServerTime = serverTime,
            .Sgs = New List(Of SG) From {New SG With {.Sg = sg}}}
    End Function

    Private Shared Function ConfirmedSchedule(serverAge As Long) As LivePollingSchedule
        Dim schedule As New LivePollingSchedule()
        schedule.Observe(Sample(DeviceBase, ServerBase, ServerBase + 45000), 0)
        schedule.Observe(Sample(DeviceBase + 300000, ServerBase + 300000,
                                ServerBase + 300000 + serverAge), 0)
        Return schedule
    End Function

    <Fact>
    Public Sub RequiresTwoRegularUploads()
        Dim schedule As New LivePollingSchedule()
        Assert.Equal(60000, schedule.NextInterval(0))
        schedule.Observe(Sample(DeviceBase, ServerBase, ServerBase + 45000), 0)
        Assert.Equal(60000, schedule.NextInterval(0))
        schedule.Observe(Sample(DeviceBase + 300000, ServerBase + 300000, ServerBase + 345000), 0)
        Assert.Equal(50000, schedule.NextInterval(0))
    End Sub

    <Fact>
    Public Sub AlignsWithoutPollingFasterThanFiftySeconds()
        Dim schedule As LivePollingSchedule = ConfirmedSchedule(45000)
        Dim elapsedSeconds As Long = 0
        For i As Integer = 1 To 4
            Dim interval As Integer = schedule.NextInterval(elapsedSeconds * Stopwatch.Frequency)
            Assert.Equal(50000, interval)
            elapsedSeconds += interval \ 1000
        Next
        Assert.Equal(60000, schedule.NextInterval(elapsedSeconds * Stopwatch.Frequency))
    End Sub

    <Fact>
    Public Sub EverySecondOfPhaseConvergesWithinSixAdjustments()
        For age As Long = 0 To 59000 Step 1000
            Dim schedule As LivePollingSchedule = ConfirmedSchedule(age)
            Dim elapsedSeconds As Long = 0
            Dim aligned As Boolean = False
            For i As Integer = 0 To 6
                Dim interval As Integer = schedule.NextInterval(elapsedSeconds * Stopwatch.Frequency)
                Assert.InRange(interval, 50000, 60000)
                If interval = 60000 Then
                    aligned = True
                    Exit For
                End If
                elapsedSeconds += interval \ 1000
            Next
            Assert.True(aligned, $"Did not align for server age {age}")
        Next
    End Sub

    <Fact>
    Public Sub InvalidEventAndMissingSensorDoNotMoveThePhase()
        Dim schedule As LivePollingSchedule = ConfirmedSchedule(45000)
        schedule.Observe(Sample(DeviceBase + 360000, ServerBase + 360000, ServerBase + 360000, 0), 0)
        Assert.Equal(50000, schedule.NextInterval(0))
        schedule.Observe(Sample(DeviceBase + 900000, ServerBase + 900000, ServerBase + 945000), 0)
        Assert.Equal(50000, schedule.NextInterval(0))
    End Sub

    <Fact>
    Public Sub ChangedCadenceMustBeConfirmedAgain()
        Dim schedule As LivePollingSchedule = ConfirmedSchedule(45000)
        schedule.Observe(Sample(DeviceBase + 640000, ServerBase + 640000, ServerBase + 685000), 0)
        Assert.Equal(60000, schedule.NextInterval(0))
        schedule.Observe(Sample(DeviceBase + 940000, ServerBase + 940000, ServerBase + 985000), 0)
        Assert.Equal(50000, schedule.NextInterval(0))
    End Sub

    <Fact>
    Public Sub StaleDataAndResetFallBackToOneMinute()
        Dim schedule As LivePollingSchedule = ConfirmedSchedule(45000)
        Assert.Equal(60000, schedule.NextInterval(856 * Stopwatch.Frequency))
        schedule.Reset()
        Assert.Equal(60000, schedule.NextInterval(0))
    End Sub

    <Theory>
    <InlineData(5000, 60000)>
    <InlineData(10000, 60000)>
    <InlineData(12000, 53000)>
    <InlineData(15000, 50000)>
    <InlineData(45000, 50000)>
    Public Sub ChoosesExpectedIntervalForServerPhase(age As Long, expected As Integer)
        Dim schedule As LivePollingSchedule = ConfirmedSchedule(age)
        Assert.Equal(expected, schedule.NextInterval(0))
    End Sub
End Class
