' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json.Serialization

Public Class TherapyAlgorithmState

    <JsonPropertyName("autoModeShieldState")>
    Public Property AutoModeShieldState As String

    <JsonPropertyName("autoModeReadinessState")>
    Public Property AutoModeReadinessState As String

    <JsonPropertyName("plgmLgsState")>
    Public Property PlgmLgsState As String

    <JsonPropertyName("waitToCalibrateDuration")>
    Public Property WaitToCalibrateDuration As Integer

    <JsonPropertyName("safeBasalDuration")>
    Public Property SafeBasalDuration As Integer

End Class
