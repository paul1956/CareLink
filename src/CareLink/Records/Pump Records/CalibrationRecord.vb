' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class CalibrationRecord

    Public Sub New(marker As Marker, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.type = marker.Type
        Me.timestamp = marker.Timestamp
#If False Then ' TODO
        Me.index = marker index
        Me.value = marker.value
        Me.kind = marker.kind
        Me.version = marker.version
        Me.relativeOffset = marker relativeOffset
        Me.calibrationSuccess = marker calibrationSuccess
#End If
    End Sub


    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName("Type")>
    <Column(Order:=1, TypeName:=NameOf([Int32]))>
    Public Property type As String

    <DisplayName(NameOf(index))>
    <Column(Order:=2, TypeName:=NameOf([Int32]))>
    Public Property index As Integer

    <DisplayName("Value")>
    <Column(Order:=3, TypeName:=NameOf([Single]))>
    Public Property value As Single

    <DisplayName("Value (mg/dL)")>
    <Column(Order:=4, TypeName:=NameOf([Single]))>
    Public ReadOnly Property valueMmDl As Single
        Get
            If Single.IsNaN(Me.value) Then Return Me.value
            Return If(NativeMmolL,
                      CSng(Math.Round(Me.value * MmolLUnitsDivisor)),
                      Me.value
                     )
        End Get
    End Property

    <DisplayName("Value (mmol/L)")>
    <Column(Order:=5, TypeName:=NameOf([Single]))>
    Public ReadOnly Property valueMmolL As Single
        Get
            If Single.IsNaN(Me.value) Then Return Me.value
            Return If(NativeMmolL,
                      Me.value,
                      RoundSingle(Me.value / MmolLUnitsDivisor, 2, False)
                     )
        End Get
    End Property

    <DisplayName("Kind")>
    <Column(Order:=6, TypeName:=NameOf([String]))>
    Public Property kind As String

    <DisplayName("Version")>
    <Column(Order:=7, TypeName:=NameOf([Int32]))>
    Public Property version As Integer

    <DisplayName(NameOf(timestamp))>
    <Column(Order:=8, TypeName:="Date")>
    Public Property timestamp As Date

    <DisplayName(NameOf(relativeOffset))>
    <Column(Order:=10, TypeName:=NameOf([Int32]))>
    Public Property relativeOffset As Integer

    <DisplayName("Calibration Success")>
    <Column(Order:=11, TypeName:=NameOf([Boolean]))>
    Public Property calibrationSuccess As Boolean

End Class
