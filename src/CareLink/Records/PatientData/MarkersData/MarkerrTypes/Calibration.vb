' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class Calibration

    Public Sub New(markerEntry As Marker, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.Type = markerEntry.Type
        Me.Kind = "Marker"
        Me.Timestamp = markerEntry.Timestamp
        Me.TimestampAsString = markerEntry.TimestampAsString
        Me.DisplayTime = markerEntry.DisplayTime
        Me.DisplayTimeAsString = markerEntry.DisplayTimeAsString
        Me.CalibrationSuccess = markerEntry.GetBooleanValueFromJson(NameOf(CalibrationSuccess))
        Me.UnitValue = markerEntry.GetSingleValueFromJson(NameOf(UnitValue), decimalDigits:=0, considerValue:=True)
        Me.bgUnits = markerEntry.GetStringValueFromJson(NameOf(bgUnits))
    End Sub

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName("Type")>
    <Column(Order:=1, TypeName:=NameOf([Int32]))>
    Public Property Type As String

    <DisplayName("Kind")>
    <Column(Order:=2, TypeName:=NameOf([String]))>
    Public ReadOnly Property Kind As String

    <DisplayName(NameOf(Timestamp))>
    <Column(Order:=3, TypeName:="Date")>
    Public Property Timestamp As Date

    <DisplayName(NameOf(TimestampAsString))>
    <Column(Order:=4, TypeName:="String")>
    Public Property TimestampAsString As String

    <DisplayName(NameOf(DisplayTime))>
    <Column(Order:=5, TypeName:="Date")>
    Public Property DisplayTime As Date

    <DisplayName(NameOf(DisplayTimeAsString))>
    <Column(Order:=6, TypeName:="String")>
    Public Property DisplayTimeAsString As String

    <DisplayName("UnitValue")>
    <Column(Order:=7, TypeName:=NameOf([Single]))>
    Public Property UnitValue As Single

    <DisplayName("UnitValue (mg/dL)")>
    <Column(Order:=8, TypeName:=NameOf([Single]))>
    Public ReadOnly Property UnitValueMgdL As Single
        Get
            If Single.IsNaN(Me.UnitValue) Then Return Me.UnitValue
            Return If(NativeMmolL,
                      CSng(Math.Round(Me.UnitValue * MmolLUnitsDivisor)),
                      Me.UnitValue
                     )
        End Get
    End Property

    <DisplayName("UnitValueMmolL (mmol/L)")>
    <Column(Order:=9, TypeName:=NameOf([Single]))>
    Public ReadOnly Property UnitValueMmolL As Single
        Get
            If Single.IsNaN(Me.UnitValue) Then Return Me.UnitValue
            Return If(NativeMmolL,
                      Me.UnitValue,
                      RoundSingle(Me.UnitValue / MmolLUnitsDivisor, 2, False)
                     )
        End Get
    End Property

    <DisplayName("BG Units")>
    <Column(Order:=10, TypeName:=NameOf([String]))>
    Public Property bgUnits As String

    <DisplayName("Calibration Success")>
    <Column(Order:=11, TypeName:=NameOf([Boolean]))>
    Public Property CalibrationSuccess As Boolean

End Class
