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

    <DisplayName("Date Time")>
    <Column(Order:=4, TypeName:="Date")>
    <JsonPropertyName("dateTime")>
    Public Property [Datetime] As Date

    <DisplayName("Type")>
    <Column(Order:=5, TypeName:=NameOf([String]))>
    <JsonPropertyName("type")>
    Public Property Type As String

    <DisplayName("Code")>
    <Column(Order:=6, TypeName:=NameOf([String]))>
    Public Property code As String = Nothing

    <DisplayName("Flash")>
    <Column(Order:=7, TypeName:=NameOf([Boolean]))>
    Public Property flash As Boolean = Nothing

    <DisplayName("Message Id")>
    <Column(Order:=8, TypeName:=NameOf([String]))>
    Public Property messageId As String

    <DisplayName("last Set Change")>
    <Column(Order:=9, TypeName:=NameOf([Int32]))>
    Public Property lastSetChange As Integer = Nothing

    <DisplayName("Units Remaining")>
    <Column(Order:=10, TypeName:=NameOf([Single]))>
    Public Property unitsRemaining As Single = Nothing

    <DisplayName("Pump Delivery Suspend State")>
    <Column(Order:=11, TypeName:=NameOf([Boolean]))>
    Public Property pumpDeliverySuspendState As Boolean = Nothing

    <DisplayName("Reference GUID")>
    <Column(Order:=12, TypeName:=NameOf([String]))>
    Public Property referenceGUID As String

    <DisplayName("Kind")>
    <Column(Order:=13, TypeName:=NameOf([String]))>
    Public Property kind As String = Nothing

    <DisplayName("Basal Name")>
    <Column(Order:=14, TypeName:=NameOf([String]))>
    Public Property basalName As String = Nothing

    <DisplayName("Triggered DateTime")>
    <Column(Order:=15, TypeName:="Date")>
    Public Property triggeredDateTime As Date = Nothing

    ' DO NOT RENAME
    <DisplayName("bgValue")>
    <Column(Order:=16, TypeName:=NameOf([Single]))>
    Public Property bgValue As Single = Nothing

    <DisplayName("Reminder Name")>
    <Column(Order:=17, TypeName:=NameOf([String]))>
    Public Property reminderName As String = Nothing

    <DisplayName("Secondary Time")>
    <Column(Order:=18, TypeName:="Date")>
    Public Property secondaryTime As Date = Nothing

    <DisplayName("Sensor Glucose")>
    <Column(Order:=19, TypeName:=NameOf([Single]))>
    Public Property sg As Single

    <DisplayName("Additional Info")>
    <Column(Order:=20, TypeName:=NameOf(AdditionalInfo))>
    <JsonPropertyName("additionalInfo")>
    Public Property AdditionalInfo As Dictionary(Of String, Object)

End Class
