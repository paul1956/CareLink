' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class LastAlarmRecord
    Private _datetime As Date

    <DisplayName(NameOf([datetime]))>
    <Column(Order:=2, TypeName:="Date")>
    Public Property [datetime] As Date
        Get
            Return _datetime
        End Get
        Set
            _datetime = Value
        End Set
    End Property

    <DisplayName(NameOf(basalName))>
    <Column(Order:=15,TypeName:=NameOf([String]))>
    Public Property basalName As String = Nothing

    <DisplayName(NameOf(bgValue))>
    <Column(Order:=16,TypeName:=NameOf([Single]))>
    Public Property bgValue As Single = Nothing

    <DisplayName(NameOf(code))>
    <Column(Order:=1,TypeName:=NameOf([String]))>
    Public Property code As String = Nothing

    <DisplayName("datetime As String")>
    <Column(Order:=3,TypeName:=NameOf([String]))>
    Public Property datetimeAsString As String

    <DisplayName("Flash")>
    <Column(Order:=5,TypeName:=NameOf([Boolean]))>
    Public Property flash As Boolean = Nothing

    <DisplayName(NameOf(GUID))>
    <Column(Order:=14,TypeName:=NameOf([String]))>
    Public Property GUID As String = Nothing

    <DisplayName(NameOf(instanceId))>
    <Column(Order:=6,TypeName:=NameOf([Integer]))>
    Public Property instanceId As Integer

    <DisplayName("Kind")>
    <Column(Order:=13,TypeName:=NameOf([String]))>
    Public Property kind As String = Nothing

    <DisplayName(NameOf(lastSetChange))>
    <Column(Order:=9,TypeName:=NameOf([Integer]))>
    Public Property lastSetChange As Integer = Nothing

    <DisplayName("Message Id")>
    <Column(Order:=7,TypeName:=NameOf([String]))>
    Public Property messageId As String

    <DisplayName("Pump Delivery Suspend State")>
    <Column(Order:=11,TypeName:=NameOf([Boolean]))>
    Public Property pumpDeliverySuspendState As Boolean = Nothing

    <DisplayName(NameOf(referenceGUID))>
    <Column(Order:=12,TypeName:=NameOf([String]))>
    Public Property referenceGUID As String

    <DisplayName(NameOf(reminderName))>
    <Column(Order:=16,TypeName:=NameOf([String]))>
    Public Property reminderName As String = Nothing

    <DisplayName(NameOf(secondaryTime))>
    <Column(Order:=17, TypeName:="Date")>
    Public Property secondaryTime As Date = Nothing

    <DisplayName("Sensor Glucose")>
    <Column(Order:=19,TypeName:=NameOf([Single]))>
    Public Property sg As Single

    <DisplayName("Triggered DateTime")>
    <Column(Order:=15, TypeName:="Date")>
    Public Property triggeredDateTime As Date = Nothing

    <DisplayName("Type")>
    <Column(Order:=4,TypeName:=NameOf([String]))>
    Public Property type As String

    <DisplayName("Units Remaining")>
    <Column(Order:=10,TypeName:=NameOf([Single]))>
    Public Property unitsRemaining As Single = Nothing

    <DisplayName("Version")>
    <Column(Order:=14,TypeName:=NameOf([Integer]))>
    Public Property version As Integer

End Class
