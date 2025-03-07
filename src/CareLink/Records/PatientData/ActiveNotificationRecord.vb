﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class ActiveNotificationRecord

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName(NameOf(referenceGUID))>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    Public Property referenceGUID As String

    <DisplayName(NameOf(timestamp))>
    <Column(Order:=2, TypeName:="Date")>
    Public Property timestamp As Date

    <DisplayName("Type")>
    <Column(Order:=3, TypeName:=NameOf([String]))>
    Public Property type As String

    <DisplayName(NameOf(faultId))>
    <Column(Order:=4, TypeName:=NameOf([Int32]))>
    Public Property faultId As Integer

    <DisplayName(NameOf(instanceId))>
    <Column(Order:=5, TypeName:=NameOf([Int32]))>
    Public Property instanceId As Integer

    <DisplayName("Message Id")>
    <Column(Order:=6, TypeName:=NameOf([String]))>
    Public Property messageId As String

    <DisplayName("Message")>
    <Column(Order:=7, TypeName:=NameOf([String]))>
    Public Property message As String

    <DisplayName(NameOf(basalName))>
    <Column(Order:=8, TypeName:=NameOf([String]))>
    Public Property basalName As String = Nothing

    <DisplayName("Sensor Glucose")>
    <Column(Order:=9, TypeName:=NameOf([Single]))>
    Public Property sg As Single = Nothing

    <DisplayName(NameOf(secondaryTime))>
    <Column(Order:=10, TypeName:=NameOf(secondaryTime))>
    Public Property secondaryTime As Date = Nothing

    <DisplayName("Pump Delivery Suspend State")>
    <Column(Order:=11, TypeName:=NameOf([Boolean]))>
    Public Property pumpDeliverySuspendState As Boolean = Nothing

    <DisplayName("pnpId")>
    <Column(Order:=12, TypeName:=NameOf([Single]))>
    Public Property pnpId As Single = Nothing

    <DisplayName(NameOf(relativeOffset))>
    <Column(Order:=13, TypeName:=NameOf([Int32]))>
    Public Property relativeOffset As Integer = Nothing

    <DisplayName(NameOf(triggeredDateTime))>
    <Column(Order:=14, TypeName:="Date")>
    Public Property triggeredDateTime As Date = Nothing

End Class
