' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class ClearedNotificationsRecord
    <DisplayName("Record Number")>
    <Column(Order:=0)>
    Public Property RecordNumber As Integer

    <DisplayName(NameOf(referenceGUID))>
    <Column(Order:=1)>
    Public Property referenceGUID As String

    Private _dateTime As Date


    <DisplayName(NameOf([dateTime]))>
    <Column(Order:=2)>
    Public Property [dateTime] As Date
        Get
            Return _dateTime
        End Get
        Set
            _dateTime = Value
        End Set
    End Property

    <DisplayName("dateTime As String")>
    <Column(Order:=3)>
    Public Property dateTimeAsString As String

    <DisplayName(NameOf(type))>
    <Column(Order:=4)>
    Public Property type As String

    <DisplayName(NameOf(faultId))>
    <Column(Order:=5)>
    Public Property faultId As Integer

    <DisplayName(NameOf(instanceId))>
    <Column(Order:=6)>
    Public Property instanceId As Integer


    <DisplayName("Message Id")>
    <Column(Order:=7)>
    Public Property messageId As String

    <DisplayName("Message")>
    <Column(Order:=8)>
    Public Property message As String

    <DisplayName(NameOf(basalName))>
    <Column(Order:=11)>
    Public Property basalName As String = Nothing

    <DisplayName("Sensor Glucose")>
    <Column(Order:=12)>
    Public Property sg As Single = Nothing

    <DisplayName(NameOf(secondaryTime))>
    <Column(Order:=13)>
    Public Property secondaryTime As Date = Nothing

    <DisplayName("Pump Delivery Suspend State")>
    <Column(Order:=14)>
    Public Property pumpDeliverySuspendState As Boolean = Nothing

    <DisplayName("pnpId")>
    <Column(Order:=15)>
    Public Property pnpId As Single = Nothing

    <DisplayName(NameOf(relativeOffset))>
    <Column(Order:=16)>
    Public Property relativeOffset As Integer = Nothing

    <DisplayName("Triggered DateTime")>
    <Column(Order:=17)>
    Public Property triggeredDateTime As Date = Nothing

End Class
