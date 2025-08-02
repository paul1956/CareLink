' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class Limit

    <DisplayName("Index")>
    <Column(Order:=0, TypeName:="RecordNumber")>
    Public Property Index As Integer

    <DisplayName("High Limit")>
    <Column(Order:=1, TypeName:=NameOf([Single]))>
    <JsonPropertyName("highLimit")>
    Public Property HighLimit As Single

    <DisplayName("High Limit (mg/dL)")>
    <Column(Order:=2, TypeName:=NameOf([Single]))>
    Public ReadOnly Property HighLimitMgdL As Single
        Get
            Return If(NativeMmolL,
                      CSng(Math.Round(Me.HighLimit * MmolLUnitsDivisor)),
                      Me.HighLimit)
        End Get
    End Property

    <DisplayName("High Limit (mmol/L)")>
    <Column(Order:=3, TypeName:=NameOf([Single]))>
    Public ReadOnly Property HighLimitMmolL As Single
        Get
            Return If(NativeMmolL,
                      Me.HighLimit,
                      (Me.HighLimit / MmolLUnitsDivisor).RoundSingle(digits:=2))
        End Get
    End Property

    <DisplayName("Low Limit")>
    <Column(Order:=4, TypeName:=NameOf([Single]))>
    <JsonPropertyName("lowLimit")>
    Public Property LowLimit As Single

    <DisplayName("Low Limit (mg/dL)")>
    <Column(Order:=5, TypeName:=NameOf([Single]))>
    Public ReadOnly Property lowLimitMgdL As Single
        Get
            Return If(NativeMmolL,
                      CSng(Math.Round(Me.LowLimit * MmolLUnitsDivisor)),
                      Me.LowLimit)
        End Get
    End Property

    <DisplayName("Low Limit (mmol/L)")>
    <Column(Order:=6, TypeName:=NameOf([Single]))>
    Public ReadOnly Property lowLimitMmolL As Single
        Get
            Return If(NativeMmolL,
                      Me.LowLimit,
                      (Me.LowLimit / MmolLUnitsDivisor).RoundSingle(digits:=2))
        End Get
    End Property

    <DisplayName("Kind")>
    <Column(Order:=7, TypeName:=NameOf([String]))>
    <JsonPropertyName("kind")>
    Public Property Kind As String

    <DisplayName("Version")>
    <Column(Order:=8, TypeName:=NameOf([Int32]))>
    <JsonPropertyName("version")>
    Public Property Version As Integer

    <DisplayName("Timestamp From Pump")>
    <Column(Order:=9, TypeName:="String")>
    <JsonPropertyName("timestamp")>
    Public Property TimestampAsString As String

    <DisplayName("Timestamp As Date")>
    <Column(Order:=10, TypeName:="Date")>
    <JsonPropertyName("timestampAsDate")>
    Public ReadOnly Property Timestamp As Date
        Get
            Return Me.TimestampAsString.TryParseDateStr()
        End Get
    End Property

End Class
