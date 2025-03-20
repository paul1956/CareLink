' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization
Imports DocumentFormat.OpenXml.InkML

Friend Class BasalPerHour

    Public Sub New(hour As Integer)
        Me.Hour = hour
        Me.BasalRate = 0
    End Sub

    <DisplayName("Hour")>
    <Column(Order:=0, TypeName:=NameOf([Int32]))>
    Public Property Hour As Integer

    <DisplayName(NameOf(BasalRate))>
    <Column(Order:=1, TypeName:="Double")>
    Public Property BasalRate As Double

End Class
