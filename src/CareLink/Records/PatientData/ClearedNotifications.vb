' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class ClearedNotifications

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf([Int32]))>
    Public Property RecordNumber As Integer

    <DisplayName("Fault Id")>
    <Column(Order:=1, TypeName:=NameOf([Int32]))>
    <JsonPropertyName("faultId")>
    Public Property FaultId As String

    <DisplayName("Version")>
    <Column(Order:=2, TypeName:=NameOf([Int32]))>
    <JsonPropertyName("version")>
    Public Property Version As Integer

    <DisplayName("Reference GUID")>
    <Column(Order:=3, TypeName:=NameOf([String]))>
    <JsonPropertyName("referenceGUID")>
    Public Property ReferenceGUID As String

    <DisplayName("Date Time")>
    <Column(Order:=4, TypeName:="Date")>
    <JsonPropertyName("dateTime")>
    Public Property [dateTime] As String

    <DisplayName("Type")>
    <Column(Order:=5, TypeName:=NameOf([String]))>
    <JsonPropertyName("type")>
    Public Property Type As String

    <DisplayName("Triggered Date Time")>
    <Column(Order:=6, TypeName:="Date")>
    <JsonPropertyName("triggeredDateTime")>
    Public Property triggeredDateTime As String

    <DisplayName("Additional Info")>
    <Column(Order:=7, TypeName:="AdditionalInfo")>
    <JsonPropertyName("additionalInfo")>
    Public Property AdditionalInfo As AdditionalInfo

End Class
