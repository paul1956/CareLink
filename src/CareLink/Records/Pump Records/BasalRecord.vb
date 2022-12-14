' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class BasalRecord

    <DisplayName("Active Basal Pattern")>
    <Column(Order:=0, TypeName:=NameOf([String]))>
    Public Property activeBasalPattern As String

    <DisplayName("Basal Rate")>
    <Column(Order:=1, TypeName:=NameOf([Single]))>
    Public Property basalRate As Single

    <DisplayName("Temp Basal Rate")>
    <Column(Order:=2, TypeName:=NameOf([Single]))>
    Public Property tempBasalRate As Single

    <DisplayName("Temp Basal Percentage")>
    <Column(Order:=3, TypeName:=NameOf([Int32]))>
    Public Property tempBasalPercentage As Integer

    <DisplayName("Temp Basal Type")>
    <Column(Order:=4, TypeName:=NameOf([String]))>
    Public Property tempBasalType As String

    <DisplayName("Temp Basal Duration Remaining")>
    <Column(Order:=5, TypeName:=NameOf([Int32]))>
    Public Property tempBasalDurationRemaining As Integer


End Class
