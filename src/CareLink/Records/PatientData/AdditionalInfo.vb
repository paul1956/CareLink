' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class AdditionalInfo

    <DisplayName("Sensor Glucose")>
    <Column(Order:=1, TypeName:=NameOf([Single]))>
    <JsonPropertyName("sg")>
    Public Property sg As Single

    <DisplayName("Pump Delivery Suspend State")>
    <Column(Order:=2, TypeName:=NameOf([Boolean]))>
    <JsonPropertyName("pumpDeliverySuspendState")>
    Public Property PumpDeliverySuspendState As Boolean

    <DisplayName("pnpId")>
    <Column(Order:=3, TypeName:=NameOf([Single]))>
    <JsonPropertyName("pnpId")>
    Public Property PnpId As String

    <DisplayName("Units Remaining")>
    <Column(Order:=4, TypeName:=NameOf([Single]))>
    <JsonPropertyName("unitsRemaining")>
    Public Property UnitsRemaining As Single

    <DisplayName("Alert Silenced")>
    <Column(Order:=4, TypeName:=NameOf([Boolean]))>
    <JsonPropertyName("alertSilenced")>
    Public Property AlertSilenced As Boolean

    <DisplayName("Last Set Change")>
    <Column(Order:=5, TypeName:=NameOf([Int32]))>
    <JsonPropertyName("lastSetChange")>
    Public Property LastSetChange As Integer

    <DisplayName("Delivered Amount")>
    <Column(Order:=6, TypeName:=NameOf([Double]))>
    <JsonPropertyName("deliveredAmount")>
    Public Property DeliveredAmount As String

    <DisplayName("Not Delivered Amount")>
    <Column(Order:=7, TypeName:=NameOf([Double]))>
    <JsonPropertyName("notDeliveredAmount")>
    Public Property NotDeliveredAmount As String

End Class
