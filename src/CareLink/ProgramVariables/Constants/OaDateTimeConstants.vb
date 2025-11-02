' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module OaDateTimeConstants

    Public ReadOnly Property TwoMinutes30SecondsOADate As _
        New OADate(asDate:=Date.MinValue + New TimeSpan(hours:=0, minutes:=2, seconds:=30))

    Public ReadOnly Property SixMinuteOADate As _
        New OADate(asDate:=Date.MinValue + New TimeSpan(hours:=0, minutes:=6, seconds:=0))

    Public ReadOnly Property OneHourAsOADate As _
        New OADate(asDate:=Date.MinValue + New TimeSpan(hours:=1, minutes:=0, seconds:=0))

End Module
