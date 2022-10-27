' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class TherapyAlgorithmStateRecord

    <DisplayName(NameOf(autoModeShieldState))>
    <Column(Order:=0, TypeName:=NameOf([String]))>
    Public Property autoModeShieldState As String

    <DisplayName(NameOf(autoModeReadinessState))>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    Public Property autoModeReadinessState As String

    <DisplayName(NameOf(plgmLgsState))>
    <Column(Order:=2, TypeName:=NameOf([String]))>
    Public ReadOnly Property plgmLgsState As String

    <DisplayName(NameOf(safeBasalDuration))>
    <Column(Order:=3, TypeName:=NameOf([Int32]))>
    Public Property safeBasalDuration As Integer

    <DisplayName(NameOf(waitToCalibrateDuration))>
    <Column(Order:=4, TypeName:=NameOf([Int32]))>
    Public Property waitToCalibrateDuration As Integer

End Class
