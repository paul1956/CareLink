' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module TimeConstants

    Friend ReadOnly s_midnight As String = New TimeOnly(0, 0).ToString()

#Region "TimeSpan Constants"

    Public ReadOnly s_0TicksSpan As New TimeSpan(0)
    Public ReadOnly s_minus1TickSpan As New TimeSpan(-1)

#Region "Minute Spans"

    Public ReadOnly s_01MinuteSpan As New TimeSpan(0, 1, 0)
    Public ReadOnly s_05MinuteSpan As New TimeSpan(0, 5, 0)
    Public ReadOnly s_30MinuteSpan As New TimeSpan(0, 30, 0)

#End Region

#Region "Day Spans"

    Public ReadOnly s_01DaySpan As New TimeSpan(1, 0, 0, 0)
    Public ReadOnly s_30DaysSpan As New TimeSpan(30, 0, 0, 0)

#End Region

#End Region ' TimeSpan Constants

#Region "Millisecond Constants"

    Public ReadOnly s_30SecondInMilliseconds As Integer = CInt(s_01MinuteSpan.TotalMilliseconds / 2)
    Public ReadOnly s_1MinutesInMilliseconds As Integer = CInt(s_01MinuteSpan.TotalMilliseconds)
    Public ReadOnly s_5MinutesInMilliseconds As Integer = CInt(s_05MinuteSpan.TotalMilliseconds)

#End Region ' Millisecond Constants

#Region "OaDateTime Constants"

    Public ReadOnly s_02Minutes30SecondsOADate As New OADate(Date.MinValue + New TimeSpan(0, 2, 30))
    Public ReadOnly s_05MinuteOADate As New OADate(Date.MinValue + s_05MinuteSpan)
    Public ReadOnly s_06MinuteOADate As New OADate(Date.MinValue + New TimeSpan(0, 6, 0))
    Public ReadOnly s_1HourAsOADate As New OADate(Date.MinValue + New TimeSpan(1, 0, 0))

#End Region ' OaDateTime Constants

End Module
