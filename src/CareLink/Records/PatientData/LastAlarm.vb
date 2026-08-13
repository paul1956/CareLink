' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class LastAlarm

    <DisplayName("Fault Id")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    <JsonPropertyName("faultId")>
    Public Property FaultId As String

    <DisplayName("Version")>
    <Column(Order:=2, TypeName:=NameOf([String]))>
    <JsonPropertyName("version")>
    Public Property Version As String

    <DisplayName("GUID")>
    <Column(Order:=3, TypeName:=NameOf([String]))>
    <JsonPropertyName("GUID")>
    Public Property GUID As String = Nothing

    <JsonPropertyName("acknowledged")>
    Public Property Acknowledged As AcknowledgedRecord

    <JsonPropertyName("snoozed")>
    Public Property Snoozed As SnoozedRecord

    <DisplayName("Date Time")>
    <Column(Order:=4, TypeName:="Date")>
    <JsonPropertyName("dateTime")>
    Public Property [Datetime] As Date

    <DisplayName("Type")>
    <Column(Order:=5, TypeName:=NameOf([String]))>
    <JsonPropertyName("type")>
    Public Property Type As String

    <DisplayName("Color")>
    <Column(Order:=6, TypeName:=NameOf(Color))>
    <JsonPropertyName("color")>
    Public Property Color As Color

    <DisplayName("Code")>
    <Column(Order:=7, TypeName:=NameOf([String]))>
    <JsonPropertyName("code")>
    Public Property Code As String = Nothing

    <DisplayName("Flash")>
    <Column(Order:=8, TypeName:=NameOf([Boolean]))>
    <JsonPropertyName("flash")>
    Public Property Flash As Boolean = Nothing

    <DisplayName("Message Id")>
    <Column(Order:=9, TypeName:=NameOf([String]))>
    <JsonPropertyName("messageId")>
    Public Property MessageId As String

    <DisplayName("Last Set Change")>
    <Column(Order:=10, TypeName:=NameOf([Int32]))>
    <JsonPropertyName("lastSetChange")>
    Public Property LastSetChange As Integer = Nothing

    <DisplayName("Units Remaining")>
    <Column(Order:=11, TypeName:=NameOf([Single]))>
    <JsonPropertyName("unitsRemaining")>
    Public Property UnitsRemaining As Single = Nothing

    <DisplayName("Pump Delivery Suspend State")>
    <Column(Order:=12, TypeName:=NameOf([Boolean]))>
    <JsonPropertyName("pumpDeliverySuspendState")>
    Public Property PumpDeliverySuspendState As Boolean = Nothing

    <DisplayName("Reference GUID")>
    <Column(Order:=13, TypeName:=NameOf([String]))>
    <JsonPropertyName("referenceGUID")>
    Public Property ReferenceGUID As String

    <DisplayName("Kind")>
    <Column(Order:=14, TypeName:=NameOf([String]))>
    <JsonPropertyName("kind")>
    Public Property Kind As String = Nothing

    <DisplayName("Basal Name")>
    <Column(Order:=15, TypeName:=NameOf([String]))>
    <JsonPropertyName("basalName")>
    Public Property BasalName As String = Nothing

    <DisplayName("Triggered DateTime")>
    <Column(Order:=16, TypeName:="Date")>
    <JsonPropertyName("triggeredDateTime")>
    Public Property TriggeredDateTime As Date = Nothing

    ' DO NOT RENAME
    <DisplayName("bgValue")>
    <Column(Order:=17, TypeName:=NameOf([Single]))>
    <JsonPropertyName("bgValue")>
    Public Property BgValue As Single = Nothing

    <DisplayName("Reminder Name")>
    <Column(Order:=18, TypeName:=NameOf([String]))>
    <JsonPropertyName("reminderName")>
    Public Property ReminderName As String = Nothing

    <DisplayName("Secondary Time")>
    <Column(Order:=19, TypeName:="Date")>
    <JsonPropertyName("secondaryTime")>
    Public Property SecondaryTime As Date = Nothing

    <DisplayName("Sensor Glucose")>
    <Column(Order:=20, TypeName:=NameOf([Single]))>
    <JsonPropertyName("sg")>
    Public Property Sg As Single

    <DisplayName("Background Color")>
    <Column(Order:=21, TypeName:=NameOf([String]))>
    <JsonPropertyName("backgroundColor")>
    Public Property BackgroundColor As String

    <DisplayName("Additional Info")>
    <Column(Order:=22, TypeName:=NameOf(AdditionalInfo))>
    <JsonPropertyName("additionalInfo")>
    Public Property AdditionalInfo As Dictionary(Of String, Object)

End Class
