' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class TherapyAlgorithmState

    <DisplayName(NameOf(AutoModeShieldState))>
    <Column(Order:=0, TypeName:=NameOf([String]))>
    <JsonPropertyName("autoModeShieldState")>
    Public Property AutoModeShieldState As String

    <DisplayName(NameOf(AutoModeReadinessState))>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    <JsonPropertyName("autoModeReadinessState")>
    Public Property AutoModeReadinessState As String

    <DisplayName(NameOf(plgmLgsState))>
    <Column(Order:=2, TypeName:=NameOf([String]))>
    <JsonPropertyName("plgmLgsState")>
    Public Property PlgmLgsState As String

    <DisplayName(NameOf(safeBasalDuration))>
    <Column(Order:=3, TypeName:=NameOf([Int32]))>
    <JsonPropertyName("safeBasalDuration")>
    Public Property SafeBasalDuration As Integer

    <DisplayName(NameOf(waitToCalibrateDuration))>
    <Column(Order:=4, TypeName:=NameOf([Int32]))>
    <JsonPropertyName("waitToCalibrateDuration")>
    Public Property WaitToCalibrateDuration As Integer

End Class
