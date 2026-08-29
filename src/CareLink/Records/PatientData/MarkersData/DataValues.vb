' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json
Imports System.Text.Json.Serialization

Public Class DataValues

    <JsonPropertyName("ActivationType")>
    Public Property ActivationType As String

    <JsonPropertyName("amount")>
    Public Property Amount As Single

    <JsonPropertyName("bgUnits")>
    Public Property bgUnits As String

    <JsonPropertyName("bolusAmount")>
    Public Property BolusAmount As Single

    <JsonPropertyName("bolusType")>
    Public Property BolusType As String

    <JsonPropertyName("calibrationSuccess")>
    Public Property CalibrationSuccess As Boolean

    <JsonPropertyName("completed")>
    Public Property Completed As Boolean

    <JsonPropertyName("deliveredExtendedAmount")>
    Public Property DeliveredExtendedAmount As Single

    <JsonPropertyName("deliveredFastAmount")>
    Public Property DeliveredFastAmount As Single

    <JsonPropertyName("deliverySuspended")>
    Public Property DeliverySuspended As Boolean

    <JsonPropertyName("effectiveDuration")>
    Public Property EffectiveDuration As Integer

    <JsonPropertyName("insulinType")>
    Public Property InsulinType As String

    <JsonPropertyName("maxAutoBasalRate")>
    Public Property MaxAutoBasalRate As Single

    <JsonPropertyName("programmedDuration")>
    Public Property ProgrammedDuration As Integer

    <JsonPropertyName("programmedExtendedAmount")>
    Public Property ProgrammedExtendedAmount As Single

    <JsonPropertyName("programmedFastAmount")>
    Public Property ProgrammedFastAmount As Single

    <JsonPropertyName("unitValue")>
    Public Property UnitValue As Single

    <JsonExtensionData>
    Public Property AdditionalProperties As Dictionary(Of String, JsonElement)

End Class
