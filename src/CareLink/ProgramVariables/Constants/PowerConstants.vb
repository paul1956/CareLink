' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Module PowerConstants
    Public Const PBT_APMBATTERYLOW As Integer = &H0000_0009
    Public Const PBT_APMOEMEVENT As Integer = &H0000_000B
    Public Const PBT_APMPOWERSTATUSCHANGE As Integer = &H0000_000A
    Public Const PBT_APMQUERYSTANDBY As Integer = &H0000_0001
    Public Const PBT_APMQUERYSTANDBYFAILED As Integer = &H0000_0003
    Public Const PBT_APMQUERYSUSPEND As Integer = &H0000_0000
    Public Const PBT_APMQUERYSUSPENDFAILED As Integer = &H0000_0002
    Public Const PBT_APMRESUMEAUTOMATIC As Integer = &H0000_0012
    Public Const PBT_APMRESUMECRITICAL As Integer = &H0000_0006
    Public Const PBT_APMRESUMESTANDBY As Integer = &H0000_0008
    Public Const PBT_APMRESUMESUSPEND As Integer = &H0000_0007
    Public Const PBT_APMSTANDBY As Integer = &H0000_0005
    Public Const PBT_APMSUSPEND As Integer = &H0000_0004
    Public Const PBTF_APMRESUMEFROMFAILURE As Integer = &H0000_0001
    Public Const WM_POWERBROADCAST As Integer = &H0000_0021
End Module
