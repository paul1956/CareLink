' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class Calibration

    Public Sub New(item As Marker, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.Type = item.Type
        Me.Kind = "Marker"
        Me.TimestampAsString = item.TimestampAsString
        Me.DisplayTimeAsString = item.DisplayTimeAsString
        Me.CalibrationSuccess = item.GetBooleanFromJson(key:=NameOf(CalibrationSuccess))
        Me.UnitValue =
            item.GetSingleFromJson(key:=NameOf(UnitValue), digits:=0, considerValue:=True)
        Me.bgUnits = item.GetStringFromJson(NameOf(bgUnits))
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

    <DisplayName("Timestamp From Pump")>
    <Column(Order:=3, TypeName:="String")>
    <JsonPropertyName("timestamp")>
    Public Property TimestampAsString As String

    <DisplayName("Timestamp As Date")>
    <Column(Order:=4, TypeName:="Date")>
    <JsonPropertyName("timestampAsDate")>
    Public ReadOnly Property Timestamp As Date
        Get
            Return Me.TimestampAsString.TryParseDateStr()
        End Get
    End Property

    <DisplayName("Display Time From Pump")>
    <Column(Order:=5, TypeName:="String")>
    <JsonPropertyName("displayTime")>
    Public Property DisplayTimeAsString As String

    <DisplayName("Display Time As Date")>
    <Column(Order:=6, TypeName:="Date")>
    <JsonPropertyName("displayTimeAsDate")>
    Public ReadOnly Property DisplayTime As Date
        Get
            Return Me.DisplayTimeAsString.TryParseDateStr()
        End Get
    End Property

    <DisplayName("Unit Value")>
    <Column(Order:=7, TypeName:=NameOf([Single]))>
    Public Property UnitValue As Single

    <DisplayName("Unit Value (mg/dL)")>
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

    <DisplayName("Unit Value (mmol/L)")>
    <Column(Order:=9, TypeName:=NameOf([Single]))>
    Public ReadOnly Property UnitValueMmolL As Single
        Get
            If Me.UnitValue.IsSgInvalid Then Return Me.UnitValue
            Return If(NativeMmolL,
                      Me.UnitValue,
                      (Me.UnitValue / MmolLUnitsDivisor).RoundToSingle(digits:=2))
        End Get
    End Property

    <DisplayName("Blood Glucose Units")>
    <Column(Order:=10, TypeName:=NameOf([String]))>
    Public Property bgUnits As String

    <DisplayName("Calibration Success")>
    <Column(Order:=11, TypeName:=NameOf([Boolean]))>
    Public Property CalibrationSuccess As Boolean

End Class
