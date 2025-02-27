' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class CalibrationRecord

    Public Sub New(marker As Marker, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.Type = marker.Type
        Me.Timestamp = marker.Timestamp
        Me.TimestampAsString = marker.TimestampAsString
        Me.UnitValue = marker.GetSingleValueFromJson(NameOf(UnitValue), decimalDigits:=0, considerValue:=True)
        Me.DisplayTime = marker.DisplayTime
        Me.DisplayTimeAsString = marker.DisplayTimeAsString
        Me.calibrationSuccess = marker.GetBooleanValueFromJson(NameOf(calibrationSuccess))
    End Sub

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName("Type")>
    <Column(Order:=1, TypeName:=NameOf([Int32]))>
    Public Property Type As String

    <DisplayName("UnitValue")>
    <Column(Order:=2, TypeName:=NameOf([Single]))>
    <JsonPropertyName("unitValue")>
    Public Property UnitValue As Single

    <DisplayName("UnitValue (mg/dL)")>
    <Column(Order:=3, TypeName:=NameOf([Single]))>
    Public ReadOnly Property UnitValueMmDl As Single
        Get
            If Single.IsNaN(Me.UnitValue) Then Return Me.UnitValue
            Return If(NativeMmolL,
                      CSng(Math.Round(Me.UnitValue * MmolLUnitsDivisor)),
                      Me.UnitValue
                     )
        End Get
    End Property

    <DisplayName("UnitValueMmolL (mmol/L)")>
    <Column(Order:=4, TypeName:=NameOf([Single]))>
    Public ReadOnly Property UnitValueMmolL As Single
        Get
            If Single.IsNaN(Me.UnitValue) Then Return Me.UnitValue
            Return If(NativeMmolL,
                      Me.UnitValue,
                      RoundSingle(Me.UnitValue / MmolLUnitsDivisor, 2, False)
                     )
        End Get
    End Property

    <DisplayName(NameOf(Timestamp))>
    <Column(Order:=5, TypeName:="Date")>
    <JsonPropertyName("timestamp")>
    Public Property Timestamp As Date

    <DisplayName(NameOf(TimestampAsString))>
    <Column(Order:=6, TypeName:="String")>
    Public Property TimestampAsString As String

    <DisplayName(NameOf(DisplayTime))>
    <Column(Order:=7, TypeName:="Date")>
    <JsonPropertyName("displayTime")>
    Public Property DisplayTime As Date

    <DisplayName(NameOf(DisplayTimeAsString))>
    <Column(Order:=8, TypeName:="String")>
    Public Property DisplayTimeAsString As String

    <DisplayName("Calibration Success")>
    <Column(Order:=8, TypeName:=NameOf([Boolean]))>
    <JsonPropertyName("calibrationSuccess")>
    Public Property CalibrationSuccess As Boolean

End Class
