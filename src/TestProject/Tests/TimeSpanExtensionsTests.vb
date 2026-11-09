' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports CareLink
Imports FluentAssertions
Imports Xunit

Public Class TimeSpanExtensionsTests

    <Theory>
    <InlineData("01:00", "1 Hour")>
    <InlineData("1:00", "1 Hour")>
    <InlineData("02:00", "2 Hours")>
    <InlineData("00:15", "15 Minutes")>
    <InlineData("0:5", "5 Minutes")>
    <InlineData("00:01", "1 Minute")>
    <InlineData("01:01", "1 Hour 1 Minute")>
    <InlineData("02:01", "2 Hours 1 Minute")>
    <InlineData("01:30", "1 Hour 30 Minutes")>
    <InlineData("2:05", "2 Hours 5 Minutes")>
    <InlineData("00:00", "0 Minutes")>
    Public Sub FormatTimeText_ReturnsExpectedString(timeStr As String, expected As String)
        Dim result As String = FormatTimeText(timeStr)
        result.Should().Be(expected)
    End Sub

    <Theory>
    <InlineData(0, "0 hrs")>
    <InlineData(1, "1 hr")>
    <InlineData(23, "23 hrs")>
    <InlineData(24, "1 day")>
    <InlineData(25, "1 day, 1 hr")>
    <InlineData(48, "2 days")>
    <InlineData(49, "2 days, 1 hr")>
    Public Sub HoursToDaysAndHours_ReturnsExpectedString(hours As Integer, expected As String)
        Dim result As String = HoursToDaysAndHours(hours, shortHr:=True)
        result.Should().Be(expected)
    End Sub

    <Theory>
    <InlineData(0, "0 hours")>
    <InlineData(1, "1 hour")>
    <InlineData(25, "1 day, 1 hour")>
    <InlineData(50, "2 days, 2 hours")>
    Public Sub HoursToDaysAndHours_LongUnits_ReturnsExpectedString(hours As Integer, expected As String)
        Assert.Equal(expected, hours.HoursToDaysAndHours(shortHr:=False))
    End Sub

    <Theory>
    <InlineData(0, "0 hrs")>
    <InlineData(1, "1 hr")>
    <InlineData(24, "1 day")>
    <InlineData(25, "1 day and 1 hr")>
    <InlineData(50, "2 days and 2 hrs")>
    Public Sub ToDaysHours_ReturnsExpectedString(hours As Integer, expected As String)
        Assert.Equal(expected, hours.ToDaysHours())
    End Sub

    <Theory>
    <InlineData(0, "0 mins")>
    <InlineData(1, "1 min")>
    <InlineData(60, "1 hr")>
    <InlineData(61, "1 hr and 1 min")>
    <InlineData(122, "2 hrs and 2 mins")>
    <InlineData(60000, "1000 hrs")>
    Public Sub ToHrMinText_ReturnsExpectedString(minutes As Long, expected As String)
        Assert.Equal(expected, minutes.ToHrMinText())
    End Sub

    <Fact>
    Public Sub DurationText_RejectsNegativeValues()
        Assert.Throws(Of ArgumentOutOfRangeException)(Sub() ToDaysHours(-1))
        Assert.Throws(Of ArgumentOutOfRangeException)(Sub() ToHrMinText(-1L))
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
    <InlineData(1441, "1 day, 0 hours, 1 minute")>
    <InlineData(1500, "1 day, 1 hour")>
    <InlineData(1501, "1 day, 1 hour, 1 minute")>
    Public Sub MinutesToDaysHoursMinutes_ReturnsExpectedString(totalMinutes As Integer, expected As String)
        Dim result As String = MinutesToDaysHoursMinutes(totalMinutes)
        result.Should().Be(expected)
    End Sub

    <Theory>
    <InlineData(0, "0 minutes")>
    <InlineData(1, "1 minute")>
    <InlineData(61, "1 hour")>
    <InlineData(1441, "1 day, 0 hours")>
    Public Sub MinutesToDaysHoursMinutes_WithoutMinutes_PreservesExistingLayout(totalMinutes As Integer, expected As String)
        Assert.Equal(expected, totalMinutes.MinutesToDaysHoursMinutes(showMinutes:=False))
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
    <InlineData(1, 0, 0, "hr", "1:00 hr")>
    <InlineData(1, 30, 0, "hr", "1:30 hr")>
    <InlineData(2, 0, 0, "hr", "2:00 hrs")>
    <InlineData(0, 45, 0, "min", "45 mins")>
    <InlineData(0, 1, 0, "hr", "01 min")>
    <InlineData(0, 0, 1, "min", "01 Sec")>
    <InlineData(0, 0, 2, "min", "02 Secs")>
    <InlineData(0, 0, 0, "hr", "0 hrs")>
    <InlineData(0, 0, 0, "min", "0 mins")>
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
