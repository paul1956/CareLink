' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports CareLink
Imports CareLink.DateTimeExtensions
Imports Xunit

Public Class DateTimeExtensionsTests

    <Theory>
    <InlineData(Nothing)>
    <InlineData("")>
    <InlineData("   ")>
    Public Sub FromUnixTime_BlankValue_ReturnsUtcEpoch(value As String)
        Dim result As Date = value.FromUnixTime()

        Assert.Equal(expected:=New Date(year:=1970,
                                        month:=1,
                                        day:=1,
                                        hour:=0,
                                        minute:=0,
                                        second:=0, kind:=DateTimeKind.Utc),
                     actual:=result)
        Assert.Equal(expected:=DateTimeKind.Utc,
                     actual:=result.Kind)
    End Sub

    <Theory>
    <InlineData("1000", 1)>
    <InlineData("-1000", -1)>
    <InlineData("1500", 1.5)>
    Public Sub FromUnixTime_ConvertsMilliseconds(value As String, seconds As Double)
        Dim expected As Date = New Date(year:=1970,
                                        month:=1,
                                        day:=1,
                                        hour:=0,
                                        minute:=0,
                                        second:=0,
                                        kind:=DateTimeKind.Utc).AddSeconds(value:=seconds)
        Dim result As Date = value.FromUnixTime()

        Assert.Equal(expected,
                     actual:=result)
        Assert.Equal(expected:=DateTimeKind.Utc,
                     actual:=result.Kind)
    End Sub

    <Fact>
    Public Sub FromUnixTime_InvalidValue_ThrowsFormatException()
        Assert.Throws(Of FormatException)(testCode:=Sub() FromUnixTime(value:="not-a-number"))
    End Sub

    <Theory>
    <InlineData(0L)>
    <InlineData(1609459200123L)>
    <InlineData(-1000L)>
    Public Sub Epoch2PumpDateTime_StringAndLong_ConvertFromUtc(milliseconds As Long)
        Dim utc As Date = New Date(year:=1970,
                                   month:=1,
                                   day:=1,
                                   hour:=0,
                                   minute:=0,
                                   second:=0,
                                   kind:=DateTimeKind.Utc).AddMilliseconds(value:=milliseconds)
        Dim expected As Date =
            TimeZoneInfo.ConvertTimeFromUtc(dateTime:=utc, destinationTimeZone:=PumpTimeZoneInfo)
        Dim text As String = milliseconds.ToString(provider:=CultureInfo.InvariantCulture)

        Assert.Equal(expected,
                     actual:=milliseconds.Epoch2PumpDateTime())
        Assert.Equal(expected,
                     actual:=text.Epoch2PumpDateTime())
    End Sub

    <Fact>
    Public Sub Epoch2DateTimeString_Zero_ReturnsEmptyForBothModes()
        Assert.Equal(expected:=String.Empty,
                     actual:="0".Epoch2DateTimeString(isLocalTime:=False))
        Assert.Equal(expected:=String.Empty,
                     actual:="0".Epoch2DateTimeString(isLocalTime:=True))
    End Sub

    <Fact>
    Public Sub Epoch2DateTimeString_UsesUtcAndLocalAndPumpTimes()
        Const milliseconds As String = "1609459200000"
        Dim utc As Date = New Date(year:=2021,
                                   month:=1,
                                   day:=1,
                                   hour:=0,
                                   minute:=0,
                                   second:=0,
                                   kind:=DateTimeKind.Utc)
        Dim local As Date = utc.ToLocalTime()
        Dim pump As Date = TimeZoneInfo.ConvertTimeFromUtc(dateTime:=utc,
                                                           destinationTimeZone:=PumpTimeZoneInfo)
        Dim description As String = If(pump.ToString() = local.ToString(),
                                       $"    Local and Pump Time are {local}",
                                       $"    Local time is {local}, Pump Time is {pump}")

        Assert.Equal(expected:=$"{utc} UTC, {description}",
                     actual:=milliseconds.Epoch2DateTimeString())
    End Sub

    <Fact>
    Public Sub Epoch2DateTimeString_LocalMode_TreatsEpochAsLocalWallClock()
        Const milliseconds As String = "1609502400000"
        Dim local As New Date(year:=2021,
                              month:=1,
                              day:=1,
                              hour:=12,
                              minute:=0,
                              second:=0,
                              kind:=DateTimeKind.Local)
        Dim utc As Date = local.ToUniversalTime()

        Assert.Equal(expected:=$"{utc} UTC,     Local and Pump Time are {local}",
                     actual:=milliseconds.Epoch2DateTimeString(isLocalTime:=True))
    End Sub

    <Theory>
    <InlineData("US", "en-US")>
    <InlineData("GB", "en-GB")>
    <InlineData("not-a-country", "en-US")>
    Public Sub GetCurrentDateCulture_ReturnsMatchingCultureOrFallback(code As String, expected As String)
        Assert.Equal(expected,
                     actual:=code.GetCurrentDateCulture().Name)
    End Sub

    <Fact>
    Public Sub RoundDownToMinute_RemovesSecondsAndFractionWithoutChangingMinute()
        Dim input As New Date(year:=2024,
                              month:=2,
                              day:=29,
                              hour:=23,
                              minute:=59,
                              second:=59,
                              kind:=DateTimeKind.Utc)
        input = input.AddMilliseconds(value:=999)
        Dim result As Date = input.RoundDownToMinute()

        Assert.Equal(expected:=New Date(year:=2024,
                                        month:=2,
                                        day:=29,
                                        hour:=23,
                                        minute:=59,
                                        second:=0),
                     actual:=result)
        Assert.Equal(expected:=DateTimeKind.Unspecified,
                     actual:=result.Kind)
    End Sub

    <Fact>
    Public Sub GetMarkerTimestamp_UsesParsedTimestampRoundedDown()
        Dim marker As New Marker With {.TimestampAsString = "2024-02-29T23:59:58"}

        Assert.Equal(expected:=New Date(year:=2024,
                                        month:=2,
                                        day:=29,
                                        hour:=23,
                                        minute:=59,
                                        second:=0),
                     actual:=marker.GetMarkerTimestamp())
    End Sub

    <Theory>
    <InlineData(0, " 0:00")>
    <InlineData(5, " 0:05")>
    <InlineData(60, " 1:00")>
    <InlineData(599, " 9:59")>
    <InlineData(600, "10:00")>
    <InlineData(-5, "-0:05")>
    <InlineData(-600, "-10:00")>
    Public Sub ToHoursMinutes_Integer_FormatsSignedMinutes(minutes As Integer, expected As String)
        Assert.Equal(expected,
                     actual:=minutes.ToHoursMinutes())
    End Sub

    <Theory>
    <InlineData(0.0F, "0:00")>
    <InlineData(1.5F, "1:30")>
    <InlineData(-1.5F, "-1:30")>
    <InlineData(1.999F, "2:00")>
    <InlineData(10.0F, "10:00")>
    Public Sub ToHoursMinutes_Single_RoundsAndFormatsHours(hours As Single, expected As String)
        Assert.Equal(expected,
                     actual:=hours.ToHoursMinutes())
    End Sub

    <Fact>
    Public Sub ToHoursMinutes_TimeOnly_UsesConfiguredDateCulture()
        Dim value As New TimeOnly(hour:=9, minute:=5)
        Dim raw As String = value.ToString(provider:=CurrentDateCulture)
        Dim expected As String = If(raw.Split(separator:=":"c)(0).Length = 1, $" {raw}", raw)

        Assert.Equal(expected,
                     actual:=value.ToHoursMinutes())
    End Sub

    <Fact>
    Public Sub ToNotificationString_UsesConfiguredTimeFormat()
        Dim value As New Date(year:=2024,
                              month:=2,
                              day:=29,
                              hour:=9,
                              minute:=5,
                              second:=6)
        Dim expected As String = value.ToString(format:=$"MMM dd, yyyy {s_timeWithMinuteFormat}")

        Assert.Equal(expected,
                     actual:=value.ToNotificationString())
    End Sub

    <Fact>
    Public Sub ToStringExact_UsesInvariantIsoFormat()
        Dim value As New Date(year:=2024,
                              month:=2,
                              day:=29,
                              hour:=9,
                              minute:=5,
                              second:=6)

        Assert.Equal(expected:="2024-02-29T09:05:06",
                     actual:=value.ToStringExact())
    End Sub

    <Fact>
    Public Sub ToShortDateTime_RespectsSecondsOption()
        Dim value As New Date(year:=2024,
                              month:=2,
                              day:=29,
                              hour:=9,
                              minute:=5,
                              second:=6)
        Dim dateText As String = value.ToString(format:="d", provider:=CultureInfo.CurrentCulture)
        Dim withSeconds As String = value.ToString(format:="T", provider:=CultureInfo.CurrentCulture)
        Dim withoutSeconds As String = value.ToString(format:="t", provider:=CultureInfo.CurrentCulture)

        Assert.Equal(expected:=$"{dateText} {withSeconds}",
                     actual:=value.ToShortDateTime())
        Assert.Equal(expected:=$"{dateText} {withoutSeconds}",
                     actual:=value.ToShortDateTime(showSeconds:=False))
    End Sub

    <Theory>
    <InlineData(Nothing)>
    <InlineData("")>
    <InlineData("   ")>
    Public Sub TryParseDateStr_Blank_ReturnsDefaultDate(value As String)
        Assert.Equal(expected:=Date.MinValue,
                     actual:=value.TryParseDateStr())
    End Sub

    <Fact>
    Public Sub TryParseDateStr_ExactFormat_ReturnsUnspecifiedDate()
        Dim result As Date = "2024-02-29T09:05:06".TryParseDateStr()

        Assert.Equal(expected:=New Date(year:=2024, month:=2, day:=29, hour:=9, minute:=5, second:=6),
                     actual:=result)
        Assert.Equal(expected:=DateTimeKind.Unspecified,
                     actual:=result.Kind)
    End Sub

    <Fact>
    Public Sub TryParseDateStr_OffsetAndFraction_ConvertsToLocalTime()
        Dim expected As Date = New DateTimeOffset(year:=2024,
                                                  month:=2,
                                                  day:=29,
                                                  hour:=9,
                                                  minute:=5,
                                                  second:=6,
                                                  millisecond:=125,
                                                  offset:=TimeSpan.FromHours(2)).LocalDateTime
        Dim result As Date = "2024-02-29T09:05:06.125+02:00".TryParseDateStr()

        Assert.Equal(expected,
                     actual:=result)
        Assert.Equal(expected:=DateTimeKind.Local,
                     actual:=result.Kind)
    End Sub

    <Fact>
    Public Sub TryParseDateStr_InvalidValue_ThrowsFormatException()
        Dim exception As FormatException = Assert.Throws(Of FormatException)(
            testCode:=Sub() TryParseDateStr(s:="not-a-date"))

        Assert.Contains(expectedSubstring:="not-a-date",
                        actualString:=exception.Message)
    End Sub

    <Theory>
    <InlineData("")>
    <InlineData("lastConduitDateTime")>
    <InlineData("medicalDeviceTime")>
    Public Sub TryParseDate_LocalKeys_AssumeLocalTime(key As String)
        Dim result As Date
        Dim success As Boolean = "2024-02-29T09:05:06".TryParseDate(key, result)

        Assert.True(condition:=success)
        Assert.Equal(expected:=New Date(year:=2024,
                                        month:=2,
                                        day:=29,
                                        hour:=9,
                                        minute:=5,
                                        second:=6),
                     actual:=result)
        Assert.Equal(expected:=DateTimeKind.Local,
                     actual:=result.Kind)
    End Sub

    <Theory>
    <InlineData("Timestamp")>
    <InlineData("dateTime")>
    <InlineData("TriggeredDateTime")>
    Public Sub TryParseDate_UtcKeys_AdjustExplicitOffset(key As String)
        Dim result As Date
        Dim success As Boolean = "2024-02-29T09:05:06+02:00".TryParseDate(key, result)

        Assert.True(condition:=success)
        Assert.Equal(expected:=New Date(year:=2024,
                                        month:=2,
                                        day:=29,
                                        hour:=7,
                                        minute:=5,
                                        second:=6,
                                        kind:=DateTimeKind.Utc),
                     actual:=result)
        Assert.Equal(expected:=DateTimeKind.Utc,
                     actual:=result.Kind)
    End Sub

    <Fact>
    Public Sub TryParseDate_LoginDateUtc_AssumesUtcThenReturnsLocalTime()
        Dim result As Date
        Dim success As Boolean = "2024-02-29T09:05:06".TryParseDate("loginDateUTC", result)
        Dim expected As Date = New Date(year:=2024,
                                        month:=2,
                                        day:=29,
                                        hour:=9,
                                        minute:=5,
                                        second:=6,
                                        kind:=DateTimeKind.Utc).ToLocalTime()

        Assert.True(condition:=success)
        Assert.Equal(expected,
                     actual:=result)
        Assert.Equal(expected:=DateTimeKind.Local,
                     actual:=result.Kind)
    End Sub

    <Fact>
    Public Sub TryParseDate_SecondaryTime_DoesNotUseCurrentDate()
        Dim result As Date
        Dim success As Boolean = "09:05:06".TryParseDate(key:="SecondaryTime", result)

        Assert.True(condition:=success)
        Assert.Equal(expected:=New Date(year:=1,
                                        month:=1,
                                        day:=1,
                                        hour:=9,
                                        minute:=5,
                                        second:=6),
                     actual:=result)
    End Sub

    <Fact>
    Public Sub TryParseDate_InvalidValue_ReturnsFalseAndClearsResult()
        Dim result As New Date(year:=2024,
                               month:=1,
                               day:=1)
        Dim success As Boolean = "not-a-date".TryParseDate(key:=String.Empty, result)

        Assert.False(condition:=success)
        Assert.Equal(expected:=Date.MinValue,
                     actual:=result)
    End Sub

    <Fact>
    Public Sub TryParseDate_UnknownKey_ReturnsFalseWithoutChangingResult()
        Dim expected As New Date(year:=2024,
                                 month:=1,
                                 day:=1)
        Dim result As Date = expected
        Dim success As Boolean = "2024-02-29T09:05:06".TryParseDate(key:="unknown", result)

        Assert.False(condition:=success)
        Assert.Equal(expected,
                     actual:=result)
    End Sub

    <Fact>
    Public Sub ParseDate_ValidValue_ReturnsParsedDate()
        Dim result As Date = "2024-02-29T09:05:06".ParseDate(key:=String.Empty)

        Assert.Equal(expected:=New Date(year:=2024,
                                        month:=2,
                                        day:=29,
                                        hour:=9,
                                        minute:=5,
                                        second:=6),
                     actual:=result)
        Assert.Equal(expected:=DateTimeKind.Local,
                     actual:=result.Kind)
    End Sub

End Class
