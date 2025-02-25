' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class Limit

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName(NameOf(index))>
    <Column(Order:=1, TypeName:=NameOf([Int32]))>
    Public Property index As Integer

    <DisplayName("High Limit")>
    <Column(Order:=2, TypeName:=NameOf([Single]))>
    <JsonPropertyName("highLimit")>
    Public Property HighLimit As Single

    <DisplayName("High Limit (mg/dL)")>
    <Column(Order:=3, TypeName:=NameOf([Single]))>
    Public ReadOnly Property HighLimitMmDl As Single
        Get
            Return If(NativeMmolL,
                      CSng(Math.Round(Me.HighLimit * MmolLUnitsDivisor)),
                      Me.HighLimit
                     )
        End Get
    End Property

    <DisplayName("High Limit (mmol/L)")>
    <Column(Order:=4, TypeName:=NameOf([Single]))>
    Public ReadOnly Property HighLimitMmolL As Single
        Get
            Return If(NativeMmolL,
                      Me.HighLimit,
                      RoundSingle(Me.HighLimit / MmolLUnitsDivisor, 2, False)
                     )
        End Get
    End Property

    <JsonPropertyName("lowLimit")>
    <DisplayName("Low Limit")>
    <Column(Order:=5, TypeName:=NameOf([Single]))>
    Public Property LowLimit As Single

    <DisplayName("Low Limit (mg/dL)")>
    <Column(Order:=6, TypeName:=NameOf([Single]))>
    Public ReadOnly Property sgMmDl As Single
        Get
            Return If(NativeMmolL,
                      CSng(Math.Round(Me.LowLimit * MmolLUnitsDivisor)),
                      Me.LowLimit
                     )
        End Get
    End Property

    <DisplayName("Low Limit (mmol/L)")>
    <Column(Order:=7, TypeName:=NameOf([Single]))>
    Public ReadOnly Property sgMmolL As Single
        Get
            Return If(NativeMmolL,
                      Me.LowLimit,
                      RoundSingle(Me.LowLimit / MmolLUnitsDivisor, 2, False)
                     )
        End Get
    End Property

    <DisplayName("Kind")>
    <Column(Order:=8, TypeName:=NameOf([String]))>
    <JsonPropertyName("kind")>
    Public Property Kind As String

    <DisplayName("Version")>
    <Column(Order:=9, TypeName:=NameOf([Int32]))>
    <JsonPropertyName("version")>
    Public Property Version As Integer

    <DisplayName("timestamp")>
    <Column(Order:=10, TypeName:=NameOf([DateTime]))>
    <JsonPropertyName("timestamp")>
    Public Property Timestamp As String

End Class
