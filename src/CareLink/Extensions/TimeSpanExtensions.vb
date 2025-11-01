' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module TimeSpanExtensions
    ''' <summary>
    '''  Formats a time string in "HH:MM" format into a human-readable string.
    ''' </summary>
    ''' <param name="timeStr">The time string in "HH:MM" format.</param>
    ''' <returns>
    '''  A formatted string representing the time in hours and minutes.
    ''' </returns>
    <Extension>
    Public Function FormatTimeText(timeStr As String) As String
        Dim parts() As String = timeStr.Split(":"c)
        Dim hours As Integer = Integer.Parse(parts(0))
        Dim minutes As Integer = Integer.Parse(parts(1))

        If hours > 0 And minutes = 0 Then
            Return If(hours = 1, "1 Hour", $"{hours} Hours")
        ElseIf hours = 0 And minutes > 0 Then
            Return $"{minutes} Minutes"
        ElseIf hours > 0 And minutes > 0 Then
            Dim hourLabel As String = If(hours = 1, "Hour", "Hours")
            Return $"{hours} {hourLabel} {minutes} Minutes"
        Else
            Return "0 Minutes"
        End If
    End Function

    '''' <summary>
    '''  Converts a number of hours into a human-readable string representing days and hours.
    ''' </summary>
    ''' <param name="hours">The total number of hours to convert.</param>
    ''' <returns>
    '''  A string formatted as "X days, Y hours" or "X days" or "Y hours",
    '''  depending on the values.
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
    '''  Converts a number of minutes into a human-readable string representing
    '''  days, hours, and minutes.
    ''' </summary>
    ''' <param name="minutes">The total number of minutes to convert.</param>
    ''' <returns>
    '''  A string formatted as "X days, Y hours, Z minutes" or "X days" or
    '''  "Y hours" or "Z minutes", depending on the values.
    ''' </returns>
    <Extension>
    Public Function MinutesToDaysHoursMinutes(minutes As Integer) As String
        Dim parts As New List(Of String)
        If minutes < 0 Then
            Return "Unknown"
        End If
        Dim days As UInteger = CUInt(minutes \ 1440) ' 1440 minutes in a day
        Dim hours As UInteger = CUInt((minutes Mod 1440) \ 60)
        Dim mins As UInteger = CUInt(minutes Mod 60)

        If days > 0 Then parts.Add(item:=days.ToUnits(unit:="day"))
        If hours > 0 Then parts.Add(item:=hours.ToUnits(unit:="hour"))
        If mins > 0 OrElse parts.Count = 0 Then parts.Add(item:=mins.ToUnits(unit:="minute"))

        Return String.Join(separator:=", ", values:=parts)
    End Function

    ''' <summary>
    '''  Formats a <see cref="TimeSpan"/> into a human-readable string.
    ''' </summary>
    ''' <param name="tSpan">The <see cref="TimeSpan"/> to format.</param>
    ''' <param name="unit">
    '''  The <paramref name="unit"/> to use in the formatted string (e.g., "hr", "min").
    ''' </param>
    ''' <returns>
    '''  A formatted string representing the <see cref="TimeSpan"/>,
    '''  including appropriate units.
    ''' </returns>
    <Extension>
    Public Function ToFormattedTimeSpan(tSpan As TimeSpan, unit As String) As String
        Dim r As String
        Dim unitOut As String = unit
        Select Case True
            Case unit = "hr"
                If tSpan.Hours > 0 Then
                    r = $"{tSpan.Hours,2}:{tSpan.Minutes:D2}"
                    unitOut = If(tSpan.Minutes > 0,
                                 "hrs",
                                 tSpan.Hours.ToUnits(unit:="hr", includeValue:=False))
                ElseIf tSpan.Minutes > 0 Then
                    r = $"{tSpan.Minutes:D2}"
                    unitOut = tSpan.Minutes.ToUnits(unit:="min", includeValue:=False)
                Else
                    r = " 0"
                    unitOut = tSpan.Hours.ToUnits(unit, includeValue:=False)
                End If
            Case unit.Contains(value:="hr")
                r = $"{tSpan.Hours}"
            Case unit = "min"
                If tSpan.Minutes > 0 AndAlso tSpan.Seconds > 0 Then
                    r = $"{0:D2}:{tSpan.Seconds:D2}"
                    unitOut = tSpan.Minutes.ToUnits(unit, includeValue:=False)
                ElseIf tSpan.Minutes > 0 Then
                    r = $"{tSpan.Minutes,2}"
                    unitOut = tSpan.Minutes.ToUnits(unit, includeValue:=False)
                ElseIf tSpan.Seconds > 0 Then
                    r = $"{tSpan.Seconds:D2}"
                    unitOut = tSpan.Seconds.ToUnits(unit:="Sec", includeValue:=False)
                Else
                    r = $"{0,2}"
                    unitOut = ToUnits(totalUnits:=0, unit, includeValue:=False)
                End If
            Case Else
                unitOut = unit
                r = $"{tSpan.Hours,2}:{tSpan.Minutes:D2}:{tSpan.Seconds:D2}"
        End Select
        Return $"{r} {unitOut}".TrimEnd
    End Function

End Module
