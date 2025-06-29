' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Friend Class InsulinPerHour

    Public Sub New(hour As Integer)
        Me.Hour = hour
        Me.BasalRate = 0
        Me.SmartGuard = 0
        Me.Hour2 = hour + 1
        Me.BasalRate2 = 0
        Me.SmartGuard2 = 0
    End Sub

    <DisplayName("Hour")>
    <Column(Order:=0, TypeName:=NameOf([Int32]))>
    Public Property Hour As Integer

    <DisplayName("Basal Rate")>
    <Column(Order:=1, TypeName:="Double")>
    Public Property BasalRate As Double

    <DisplayName("SmartGuard Rate")>
    <Column(Order:=2, TypeName:="Double")>
    Public Property SmartGuard As Integer

    <DisplayName(" Hour")>
    <Column(Order:=3, TypeName:=NameOf([Int32]))>
    Public Property Hour2 As Integer

    <DisplayName("Basal Rate")>
    <Column(Order:=4, TypeName:="Double")>
    Public Property BasalRate2 As Double

    <DisplayName("SmartGuard Rate")>
    <Column(Order:=5, TypeName:="Double")>
    Public Property SmartGuard2 As Integer

    Friend Shared Sub AddBasalAmountToInsulinPerHour(basalDeliveryMarker As AutoBasalDelivery)
        Dim hour As Integer = basalDeliveryMarker.DisplayTime.Hour
        Dim index As Integer = hour \ 2
        If (hour Mod 2) = 0 Then
            s_listOfBasalPerHour(index).BasalRate += basalDeliveryMarker.BolusAmount
        Else
            s_listOfBasalPerHour(index).BasalRate2 += basalDeliveryMarker.BolusAmount
        End If
    End Sub

End Class
