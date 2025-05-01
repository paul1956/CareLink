' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module TimeConstants

    Friend ReadOnly Property Eleven59 As TimeOnly = TimeOnly.FromTimeSpan(New TimeSpan(23, 59, 59))
    Friend ReadOnly Property Eleven59Str As String = TimeOnly.FromTimeSpan(New TimeSpan(23, 59, 59)).ToHoursMinutes
    Friend ReadOnly Property Midnight As New TimeOnly(0, 0)
    Friend ReadOnly Property MidnightStr As String = Midnight.ToHoursMinutes()

#Region "TimeSpan Constants"

    Public ReadOnly Property ZeroTickSpan As New TimeSpan(0)

#Region "Minute Spans"

    Public ReadOnly Property OneMinuteSpan As New TimeSpan(0, 1, 0)
    Public ReadOnly Property FiveMinuteSpan As New TimeSpan(0, 5, 0)
    Public ReadOnly Property ThirtyMinuteSpan As New TimeSpan(0, 30, 0)

#End Region

#Region "Day Spans"

    Public ReadOnly Property OneDaySpan As New TimeSpan(1, 0, 0, 0)
    Public ReadOnly Property ThirtyDaysSpan As New TimeSpan(30, 0, 0, 0)

#End Region

#End Region ' TimeSpan Constants

#Region "Millisecond Constants"

    Public ReadOnly Property OneMinutesInMilliseconds As Integer = CInt(OneMinuteSpan.TotalMilliseconds)
    Public ReadOnly Property ThirtySecondInMilliseconds As Integer = CInt(OneMinuteSpan.TotalMilliseconds / 2)
    Public ReadOnly Property FiveMinutesInMilliseconds As Integer = CInt(FiveMinuteSpan.TotalMilliseconds)

#End Region ' Millisecond Constants

End Module
