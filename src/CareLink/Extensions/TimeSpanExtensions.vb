' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module TimeSpanExtensions

    '''' <summary>
    '''  Converts a number of hours into a human-readable string representing days and hours.
    ''' </summary>
    ''' <param name="hours">The total number of hours to convert.</param>
    ''' <returns>
    '''  A string formatted as "X days, Y hours" or "X days" or "Y hours", depending on the values.
    ''' </returns>
    <Extension>
    Public Function HoursToDaysAndHours(hours As Integer) As String
        Dim days As Integer = hours \ 24
        Dim remHours As Integer = hours Mod 24
        Dim dayPart As String = If(days = 1, "1 day", $"{days} days")
        Dim hourPart As String = If(remHours = 1, "1 hour", $"{remHours} hours")
        If days > 0 And remHours > 0 Then
            Return $"{dayPart}, {hourPart}"
        ElseIf days > 0 Then
            Return dayPart
        Else
            Return hourPart
        End If
    End Function

    ''' <summary>
    '''  Converts a number of minutes into a human-readable string representing days, hours, and minutes.
    ''' </summary>
    ''' <param name="minutes">The total number of minutes to convert.</param>
    ''' <returns>
    '''  A string formatted as "X days, Y hours, Z minutes" or "X days" or "Y hours" or "Z minutes",
    '''  depending on the values.
    ''' </returns>
    <Extension>
    Public Function MinutesToDaysHoursMinutes(minutes As Integer) As String
        Dim days As Integer = minutes \ 1440 ' 1440 minutes in a day
        Dim hours As Integer = (minutes Mod 1440) \ 60
        Dim mins As Integer = minutes Mod 60

        Dim parts As New List(Of String)
        If days > 0 Then parts.Add(days.ToTimeUnits(Unit:="day"))
        If hours > 0 Then parts.Add(hours.ToTimeUnits(Unit:="hour"))
        If mins > 0 OrElse parts.Count = 0 Then parts.Add(mins.ToTimeUnits(Unit:="minute"))

        Return String.Join(separator:=", ", values:=parts)
    End Function

    ''' <summary>
    '''  Formats a <see cref="TimeSpan"/> into a human-readable string, optionally using specified units.
    ''' </summary>
    ''' <param name="tSpan">The <see cref="TimeSpan"/> to format.</param>
    ''' <param name="units">
    '''  Optional. The <paramref name="units"/> to use in the formatted string (e.g., "hr", "min", "sec").
    '''  If not specified, the method will infer appropriate units based on the <see cref="TimeSpan"/> value.
    ''' </param>
    ''' <returns>
    '''  A formatted string representing the <see cref="TimeSpan"/>, including appropriate units.
    ''' </returns>
    <Extension>
    Public Function ToFormattedTimeSpan(tSpan As TimeSpan, Optional units As String = "") As String
        Dim r As String = ""
        If units.Contains("hr") Then
            units = If(tSpan.Hours = 0,
                       units,
                       units.Replace("hr", "hrs")
                      )
            r = $"{tSpan.Hours,2}:"
        End If
        If tSpan.Seconds > 0 AndAlso tSpan.Minutes > 0 Then
            r &= $"{tSpan.Minutes}:{tSpan.Seconds:D2}"
            units = If(tSpan.Minutes = 0,
                       "min",
                       "mins"
                      )
        ElseIf tSpan.Seconds > 0 Then
            r &= $"{tSpan.Seconds:D2}"
            units = If(tSpan.Minutes = 0,
                       "sec",
                       "sec"
                      )
        Else
            r &= $"{tSpan.Minutes:D2}"
            If Not units.Contains("hr") Then
                units = If(tSpan.Minutes = 0,
                           "min",
                           "mins"
                          )
            End If
        End If
        Return $"{r} {units}".TrimEnd
    End Function

End Module
