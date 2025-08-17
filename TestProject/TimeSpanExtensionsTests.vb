' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports CareLink
Imports FluentAssertions
Imports Xunit

Public Class TimeSpanExtensionsTests

    <Theory>
    <InlineData(0, "0 hours")>
    <InlineData(1, "1 hour")>
    <InlineData(23, "23 hours")>
    <InlineData(24, "1 day")>
    <InlineData(25, "1 day, 1 hour")>
    <InlineData(48, "2 days")>
    <InlineData(49, "2 days, 1 hour")>
    Public Sub HoursToDaysAndHours_ReturnsExpectedString(hours As Integer, expected As String)
        Dim result As String = HoursToDaysAndHours(hours)
        result.Should().Be(expected)
    End Sub

    <Theory>
    <InlineData(-1, "Unknown")>
    <InlineData(0, "0 minutes")>
    <InlineData(1, "1 minute")>
    <InlineData(59, "59 minutes")>
    <InlineData(60, "1 hour")>
    <InlineData(61, "1 hour, 1 minute")>
    <InlineData(120, "2 hours")>
    <InlineData(121, "2 hours, 1 minute")>
    <InlineData(1440, "1 day")>
    <InlineData(1500, "1 day, 1 hour")>
    <InlineData(1501, "1 day, 1 hour, 1 minute")>
    Public Sub MinutesToDaysHoursMinutes_ReturnsExpectedString(minutes As Integer, expected As String)
        Dim result As String = MinutesToDaysHoursMinutes(minutes)
        result.Should().Be(expected)
    End Sub

    <Theory>
    <InlineData(1, 0, 0, "invalid")>
    <InlineData(0, 0, 0, "hourly")>
    <InlineData(0, 0, 0, "seconds")>
    Public Sub ToFormattedTimeSpan_InvalidUnits_StillReturnsString(
        hours As Integer,
        minutes As Integer,
        seconds As Integer,
        unit As String)

        Dim tSpan As New TimeSpan(hours, minutes, seconds)
        Dim result As String = tSpan.ToFormattedTimeSpan(unit)
        result.Should().NotBeNullOrEmpty()
    End Sub

    <Theory>
    <InlineData(1, 0, 0, "hr", " 1:00 hr")>
    <InlineData(1, 30, 0, "hr", " 1:30 hrs")>
    <InlineData(2, 0, 0, "hr", " 2:00 hrs")>
    <InlineData(0, 45, 0, "min", "45 mins")>
    <InlineData(0, 0, 0, "hr", " 0 hrs")>
    <InlineData(0, 0, 0, "min", " 0 mins")>
    <InlineData(0, 0, 0, "U/hr", "0 U/hr")>
    Public Sub ToFormattedTimeSpan_ValidUnits_ReturnsExpectedString(
        hours As Integer,
        minutes As Integer,
        seconds As Integer,
        units As String,
        expected As String)

        ' Only allow hr, U/hr, min as valid units for this test
        If units <> "hr" AndAlso units <> "U/hr" AndAlso units <> "min" Then
            Return
        End If
        Dim tSpan As New TimeSpan(hours, minutes, seconds)
        Dim result As String = tSpan.ToFormattedTimeSpan(units)
        result.Should().Be(expected)
    End Sub

End Class
