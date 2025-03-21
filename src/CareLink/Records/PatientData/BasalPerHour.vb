' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Friend Class BasalPerHour

    Public Sub New(hour As Integer)
        Me.Hour = hour
        Me.BasalRate = 0
        Me.Hour2 = hour + 1
        Me.BasalRate2 = 0

    End Sub

    <DisplayName("Hour")>
    <Column(Order:=0, TypeName:=NameOf([Int32]))>
    Public Property Hour As Integer

    <DisplayName("Basal Rate")>
    <Column(Order:=1, TypeName:="Double")>
    Public Property BasalRate As Double

    <DisplayName(" Hour")>
    <Column(Order:=2, TypeName:=NameOf([Int32]))>
    Public Property Hour2 As Integer

    <DisplayName(" Basal Rate")>
    <Column(Order:=3, TypeName:="Double")>
    Public Property BasalRate2 As Double

End Class
