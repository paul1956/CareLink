' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module OaDateTimeConstants

    Public ReadOnly s_02Minutes30SecondsOADate As New OADate(Date.MinValue + New TimeSpan(0, 2, 30))
    Public ReadOnly s_05MinuteOADate As New OADate(Date.MinValue + s_05MinuteSpan)
    Public ReadOnly s_06MinuteOADate As New OADate(Date.MinValue + New TimeSpan(0, 6, 0))
    Public ReadOnly s_1HourAsOADate As New OADate(Date.MinValue + New TimeSpan(1, 0, 0))

End Module
