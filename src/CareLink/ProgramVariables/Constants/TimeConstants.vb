' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module TimeConstants

    Friend ReadOnly Property Eleven59 As TimeOnly =
        TimeOnly.FromTimeSpan(New TimeSpan(hours:=23, minutes:=59, seconds:=59))

    Friend ReadOnly Property Eleven59PM As String = Eleven59.ToHoursMinutes

    Friend ReadOnly Property Midnight As New TimeOnly(hour:=0, minute:=0)

    Friend ReadOnly Property MidnightStr As String = Midnight.ToHoursMinutes()

#Region "TimeSpan Constants"

    Public ReadOnly Property FiveMinutesInSeconds As Integer = CInt(FiveMinuteSpan.TotalSeconds)
    Public ReadOnly Property OneHourSpan As New TimeSpan(hours:=1, minutes:=0, seconds:=0)
    Public ReadOnly Property ZeroTickSpan As New TimeSpan(ticks:=0)

#Region "Minute Spans"

    Public ReadOnly Property FiveMinuteSpan As New TimeSpan(hours:=0, minutes:=5, seconds:=0)
    Public ReadOnly Property OneMinuteSpan As New TimeSpan(hours:=0, minutes:=1, seconds:=0)
    Public ReadOnly Property SixMinuteSpan As New TimeSpan(hours:=0, minutes:=6, seconds:=0)
    Public ReadOnly Property ThirtyMinuteSpan As New TimeSpan(hours:=0, minutes:=30, seconds:=0)
    Public ReadOnly Property ThirtySecondSpan As New TimeSpan(hours:=0, minutes:=0, seconds:=30)

#End Region

#Region "Day Spans"

    Friend ReadOnly Property Eleven30Span As TimeSpan = New TimeSpan(hours:=23, minutes:=30, seconds:=0)

    Friend ReadOnly Property Eleven55PmSpan As TimeSpan = New TimeSpan(hours:=23, minutes:=55, seconds:=0)

    Public ReadOnly Property OneDaySpan As New TimeSpan(days:=1, hours:=0, minutes:=0, seconds:=0)

    Public ReadOnly Property ThirtyDaysSpan As New TimeSpan(days:=30, hours:=0, minutes:=0, seconds:=0)

#End Region

#End Region ' TimeSpan Constants

#Region "Millisecond Constants"

    ''' <summary>
    '''  Milliseconds in 1 minute. Used for refresh rate of the main form.
    ''' </summary>
    ''' <returns>Milliseconds in 1 minute.</returns>
    Public ReadOnly Property RefreshRate1Min As Integer = CInt(OneMinuteSpan.TotalMilliseconds)

    ''' <summary>
    '''  Milliseconds in 30 seconds. Used for refresh rate of the main form
    '''  after suspend.
    ''' </summary>
    ''' <returns>Milliseconds in 30 seconds.</returns>
    Public ReadOnly Property ThirtySecondsInMilliseconds As Integer = CInt(OneMinuteSpan.TotalMilliseconds / 2)

#End Region ' Millisecond Constants

End Module
