' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class LimitsRecord
    Private _highLimit As Single
    Private _lowLimit As Single

    <DisplayName("High Limit")>
    <Column(Order:=2, TypeName:=NameOf([Single]))>
    Public Property highLimit As Single
        Get
            Return _highLimit
        End Get
        Set
            _highLimit = Value
        End Set
    End Property

    <DisplayName("High Limit (mm/Dl)")>
    <Column(Order:=3, TypeName:=NameOf([Single]))>
    Public ReadOnly Property HighLimitMmDl As Single
        Get
            Return If(ScalingNeeded, CSng(Math.Round(_highLimit * MmolLUnitsDivisor)), _highLimit)
        End Get
    End Property

    <DisplayName("High Limit (mmol/L)")>
    <Column(Order:=4, TypeName:=NameOf([Single]))>
    Public ReadOnly Property HighLimitMmolL As Single
        Get
            Return If(ScalingNeeded, _highLimit, (_highLimit / MmolLUnitsDivisor).RoundSingle(2))
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
        Get
            Return _lowLimit
        End Get
        Set
            _lowLimit = Value
        End Set
    End Property

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName("Low Limit (mm/Dl)")>
    <Column(Order:=6, TypeName:=NameOf([Single]))>
    Public ReadOnly Property sgMmDl As Single
        Get
            Return If(ScalingNeeded, CSng(Math.Round(_lowLimit * MmolLUnitsDivisor)), _lowLimit)
        End Get
    End Property

    <DisplayName("Low Limit (mmol/L)")>
    <Column(Order:=7, TypeName:=NameOf([Single]))>
    Public ReadOnly Property sgMmolL As Single
        Get
            Return If(ScalingNeeded, _lowLimit, (_lowLimit / MmolLUnitsDivisor).RoundSingle(2))
        End Get
    End Property

    <DisplayName("Version")>
    <Column(Order:=9, TypeName:=NameOf([Int32]))>
    Public Property version As Integer

End Class
