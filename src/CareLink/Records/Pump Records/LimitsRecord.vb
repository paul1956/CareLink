' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class LimitsRecord

    <DisplayName("High Limit")>
    <Column(Order:=2, TypeName:=NameOf([Single]))>
    Public Property highLimit As Single

    <DisplayName("High Limit (mg/dL)")>
    <Column(Order:=3, TypeName:=NameOf([Single]))>
    Public ReadOnly Property HighLimitMmDl As Single
        Get
            Return If(NativeMmolL,
                      CSng(Math.Round(Me.highLimit * MmolLUnitsDivisor)),
                      Me.highLimit
                     )
        End Get
    End Property

    <DisplayName("High Limit (mmol/L)")>
    <Column(Order:=4, TypeName:=NameOf([Single]))>
    Public ReadOnly Property HighLimitMmolL As Single
        Get
            Return If(NativeMmolL,
                      Me.highLimit,
                      RoundSingle(Me.highLimit / MmolLUnitsDivisor, 2, False)
                     )
        End Get
    End Property

    <DisplayName(NameOf(index))>
    <Column(Order:=1, TypeName:=NameOf([Int32]))>
    Public Property index As Integer

    <DisplayName("Kind")>
    <Column(Order:=8, TypeName:=NameOf([String]))>
    Public Property kind As String

    <DisplayName("Low Limit")>
    <Column(Order:=5, TypeName:=NameOf([Single]))>
    Public Property lowLimit As Single

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName("Low Limit (mg/dL)")>
    <Column(Order:=6, TypeName:=NameOf([Single]))>
    Public ReadOnly Property sgMmDl As Single
        Get
            Return If(NativeMmolL,
                      CSng(Math.Round(Me.lowLimit * MmolLUnitsDivisor)),
                      Me.lowLimit
                     )
        End Get
    End Property

    <DisplayName("Low Limit (mmol/L)")>
    <Column(Order:=7, TypeName:=NameOf([Single]))>
    Public ReadOnly Property sgMmolL As Single
        Get
            Return If(NativeMmolL,
                      Me.lowLimit,
                      RoundSingle(Me.lowLimit / MmolLUnitsDivisor, 2, False)
                     )
        End Get
    End Property

    <DisplayName("Version")>
    <Column(Order:=9, TypeName:=NameOf([Int32]))>
    Public Property version As Integer

End Class
