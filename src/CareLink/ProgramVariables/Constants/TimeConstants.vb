' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module TimeConstants

    Friend ReadOnly Property Eleven59 As TimeOnly =
        TimeOnly.FromTimeSpan(New TimeSpan(hours:=23,
                                           minutes:=59,
                                           seconds:=59))
    Friend ReadOnly Property Eleven59Str As String = Eleven59.ToHoursMinutes
    Friend ReadOnly Property Midnight As New TimeOnly(hour:=0,
                                                      minute:=0)
    Friend ReadOnly Property MidnightStr As String = Midnight.ToHoursMinutes()

#Region "TimeSpan Constants"

    Friend ReadOnly Property ZeroTickSpan As New TimeSpan(ticks:=0)

#Region "Minute Spans"

    Public ReadOnly Property OneMinuteSpan As New TimeSpan(hours:=0,
                                                           minutes:=1,
                                                           seconds:=0)
    Public ReadOnly Property FiveMinuteSpan As New TimeSpan(hours:=0,
                                                            minutes:=5,
                                                            seconds:=0)
    Public ReadOnly Property ThirtyMinuteSpan As New TimeSpan(hours:=0,
                                                              minutes:=30,
                                                              seconds:=0)

#End Region

    Public ReadOnly Property OneHourSpan As New TimeSpan(hours:=1,
                                                         minutes:=0,
                                                         seconds:=0)

#Region "Day Spans"

    Friend ReadOnly Property Eleven30Span As TimeSpan = New TimeSpan(hours:=23,
                                                                     minutes:=30,
                                                                     seconds:=0)
    Friend ReadOnly Property Eleven55Span As TimeSpan = New TimeSpan(hours:=23,
                                                                     minutes:=55,
                                                                     seconds:=0)
    Public ReadOnly Property OneDaySpan As New TimeSpan(days:=1,
                                                        hours:=0,
                                                        minutes:=0,
                                                        seconds:=0)
    Public ReadOnly Property ThirtyDaysSpan As New TimeSpan(days:=30,
                                                            hours:=0,
                                                            minutes:=0,
                                                            seconds:=0)

#End Region

#End Region ' TimeSpan Constants

#Region "Millisecond Constants"

    Public ReadOnly Property OneMinutesInMilliseconds As Integer =
        CInt(OneMinuteSpan.TotalMilliseconds)
    Public ReadOnly Property ThirtySecondInMilliseconds As Integer =
        CInt(OneMinuteSpan.TotalMilliseconds / 2)
    Public ReadOnly Property FiveMinutesInMilliseconds As Integer =
        CInt(FiveMinuteSpan.TotalMilliseconds)

#End Region ' Millisecond Constants

End Module
