' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class LastAlarmRecord
    Private _datetime As Date

    <DisplayName(NameOf([datetime]))>
    <Column(Order:=2)>
    Public Property [datetime] As Date
        Get
            Return _datetime
        End Get
        Set
            _datetime = Value
        End Set
    End Property

    <DisplayName(NameOf(basalName))>
    <Column(Order:=15)>
    Public Property basalName As String = Nothing

    <DisplayName(NameOf(bgValue))>
    <Column(Order:=16)>
    Public Property bgValue As Single = Nothing

    <DisplayName(NameOf(code))>
    <Column(Order:=1)>
    Public Property code As String = Nothing

    <DisplayName("datetime As String")>
    <Column(Order:=3)>
    Public Property datetimeAsString As String

    <DisplayName("Flash")>
    <Column(Order:=5)>
    Public Property flash As Boolean = Nothing

    <DisplayName(NameOf(GUID))>
    <Column(Order:=14)>
    Public Property GUID As Integer = Nothing

    <DisplayName(NameOf(instanceId))>
    <Column(Order:=6)>
    Public Property instanceId As Integer

    <DisplayName(NameOf(kind))>
    <Column(Order:=13)>
    Public Property kind As String = Nothing

    <DisplayName(NameOf(lastSetChange))>
    <Column(Order:=9)>
    Public Property lastSetChange As Integer = Nothing

    <DisplayName("Message")>
    <Column(Order:=8)>
    Public Property message As String

    <DisplayName("Message Id")>
    <Column(Order:=7)>
    Public Property messageId As String

    <DisplayName("Pump Delivery Suspend State")>
    <Column(Order:=11)>
    Public Property pumpDeliverySuspendState As Boolean = Nothing

    <DisplayName("Record Number")>
    <Column(Order:=0)>
    Public Property RecordNumber As Integer

    <DisplayName(NameOf(referenceGUID))>
    <Column(Order:=12)>
    Public Property referenceGUID As String

    <DisplayName(NameOf(reminderName))>
    <Column(Order:=16)>
    Public Property reminderName As String = Nothing

    <DisplayName(NameOf(secondaryTime))>
    <Column(Order:=17)>
    Public Property secondaryTime As Date = Nothing

    <DisplayName("Sensor Glucose")>
    <Column(Order:=19)>
    Public Property sg As Single

    <DisplayName("Triggered DateTime")>
    <Column(Order:=15)>
    Public Property triggeredDateTime As Date = Nothing

    <DisplayName(NameOf(type))>
    <Column(Order:=4)>
    Public Property type As String

    <DisplayName("Units Remaining")>
    <Column(Order:=10)>
    Public Property unitsRemaining As Single = Nothing

    <DisplayName(NameOf(version))>
    <Column(Order:=14)>
    Public Property version As Integer

End Class
