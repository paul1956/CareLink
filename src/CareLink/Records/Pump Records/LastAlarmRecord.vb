' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class LastAlarmRecord

    <DisplayName(NameOf(code))>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    Public Property code As String = Nothing

    <DisplayName(NameOf([datetime]))>
    <Column(Order:=2, TypeName:="Date")>
    Public Property [datetime] As Date

    <DisplayName("Type")>
    <Column(Order:=3, TypeName:=NameOf([String]))>
    Public Property type As String

    <DisplayName("Flash")>
    <Column(Order:=4, TypeName:=NameOf([Boolean]))>
    Public Property flash As Boolean = Nothing

    <DisplayName(NameOf(instanceId))>
    <Column(Order:=5, TypeName:=NameOf([Int32]))>
    Public Property instanceId As Integer

    <DisplayName("Message Id")>
    <Column(Order:=6, TypeName:=NameOf([String]))>
    Public Property messageId As String

    <DisplayName(NameOf(lastSetChange))>
    <Column(Order:=7, TypeName:=NameOf([Int32]))>
    Public Property lastSetChange As Integer = Nothing

    <DisplayName("Units Remaining")>
    <Column(Order:=8, TypeName:=NameOf([Single]))>
    Public Property unitsRemaining As Single = Nothing

    <DisplayName("Pump Delivery Suspend State")>
    <Column(Order:=9, TypeName:=NameOf([Boolean]))>
    Public Property pumpDeliverySuspendState As Boolean = Nothing

    <DisplayName(NameOf(referenceGUID))>
    <Column(Order:=10, TypeName:=NameOf([String]))>
    Public Property referenceGUID As String

    <DisplayName("Kind")>
    <Column(Order:=11, TypeName:=NameOf([String]))>
    Public Property kind As String = Nothing

    <DisplayName(NameOf(GUID))>
    <Column(Order:=12, TypeName:=NameOf([String]))>
    Public Property GUID As String = Nothing

    <DisplayName(NameOf(basalName))>
    <Column(Order:=13, TypeName:=NameOf([String]))>
    Public Property basalName As String = Nothing

    <DisplayName("Version")>
    <Column(Order:=14, TypeName:=NameOf([Int32]))>
    Public Property version As Integer

    <DisplayName("Triggered DateTime")>
    <Column(Order:=15, TypeName:="Date")>
    Public Property triggeredDateTime As Date = Nothing

    ' DO NOT RENAME
    <DisplayName(NameOf(bgValue))>
    <Column(Order:=16, TypeName:=NameOf([Single]))>
    Public Property bgValue As Single = Nothing

    <DisplayName(NameOf(reminderName))>
    <Column(Order:=17, TypeName:=NameOf([String]))>
    Public Property reminderName As String = Nothing

    <DisplayName(NameOf(secondaryTime))>
    <Column(Order:=18, TypeName:="Date")>
    Public Property secondaryTime As Date = Nothing

    <DisplayName("Sensor Glucose")>
    <Column(Order:=19, TypeName:=NameOf([Single]))>
    Public Property sg As Single

End Class
