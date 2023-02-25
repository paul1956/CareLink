' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module TimeConstants

#Region "TimeSpan Constants"

    Public ReadOnly s_1MinuteSpan As New TimeSpan(hours:=0, minutes:=1, seconds:=0)
    Public ReadOnly s_5MinuteSpan As New TimeSpan(hours:=0, minutes:=5, seconds:=0)
    Public ReadOnly s_30MinuteSpan As New TimeSpan(0, 30, 0)

#End Region ' TimeSpan Constants

#Region "Millisecond Constants"

    Public ReadOnly s_30SecondInMilliseconds As Double = New TimeSpan(0, 0, seconds:=30).TotalMilliseconds
    Public ReadOnly s_1MinutesInMilliseconds As Double = s_1MinuteSpan.TotalMilliseconds
    Public ReadOnly s_5MinutesInMilliseconds As Double = s_5MinuteSpan.TotalMilliseconds

#End Region ' Millisecond Constants

#Region "OaDateTime Constants"

    Public ReadOnly s_fiveMinuteOADate As New OADate(Date.MinValue + s_5MinuteSpan)
    Public ReadOnly s_hourAsOADate As New OADate(Date.MinValue + New TimeSpan(hours:=1, minutes:=0, seconds:=0))
    Public ReadOnly s_sixMinuteOADate As New OADate(Date.MinValue + New TimeSpan(hours:=0, minutes:=6, seconds:=0))
    Public ReadOnly s_twoHalfMinuteOADate As New OADate(Date.MinValue + New TimeSpan(hours:=0, minutes:=2, seconds:=30))

#End Region ' OaDateTime Constants

    Public Enum RoundTo
        Second
        Minute
        Hour
        Day
    End Enum

End Module
