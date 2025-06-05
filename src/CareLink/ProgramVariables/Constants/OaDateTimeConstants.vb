' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module OaDateTimeConstants

    Public ReadOnly Property TwoMinutes30SecondsOADate As New OADate(Date.MinValue + New TimeSpan(0, 2, 30))
    Public ReadOnly Property FiveMinuteOADate As New OADate(Date.MinValue + FiveMinuteSpan)
    Public ReadOnly Property SixMinuteOADate As New OADate(Date.MinValue + New TimeSpan(0, 6, 0))
    Public ReadOnly Property OneHourAsOADate As New OADate(Date.MinValue + New TimeSpan(1, 0, 0))
End Module
