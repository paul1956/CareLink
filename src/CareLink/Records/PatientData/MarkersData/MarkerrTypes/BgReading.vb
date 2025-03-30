' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class BgReading

    Public Sub New(markerEntry As Marker, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.Type = markerEntry.Type
        Me.Kind = "Marker"
        Me.TimestampAsString = markerEntry.TimestampAsString
        Me.DisplayTimeAsString = markerEntry.DisplayTimeAsString
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
    <Column(Order:=3, TypeName:=NameOf([String]))>
    Public ReadOnly Property Kind As String

    <DisplayName("Timestamp")>
    <Column(Order:=4, TypeName:="String")>
    <JsonPropertyName("timestamp")>
    Public Property TimestampAsString As String

    <DisplayName("Timestamp As Date")>
    <Column(Order:=5, TypeName:="Date")>
    <JsonPropertyName("timestampAsDate")>
    Public ReadOnly Property Timestamp As Date
        Get
            Return TryParseDateStr(Me.TimestampAsString)
        End Get
    End Property

    <DisplayName("Display Time")>
    <Column(Order:=6, TypeName:="String")>
    <JsonPropertyName("displayTime")>
    Public Property DisplayTimeAsString As String

    <DisplayName("Display Time As Date")>
    <Column(Order:=7, TypeName:="Date")>
    <JsonPropertyName("displayTimeAsDate")>
    Public ReadOnly Property DisplayTime As Date
        Get
            Return TryParseDateStr(Me.DisplayTimeAsString)
        End Get
    End Property

    <DisplayName("Unit Value")>
    <Column(Order:=8, TypeName:=NameOf([Single]))>
    Public Property UnitValue As Single

    <DisplayName("Units")>
    <Column(Order:=9, TypeName:=NameOf([String]))>
    Public ReadOnly Property bgUnits As String

    <DisplayName("UnitValue (mg/dL)")>
    <Column(Order:=10, TypeName:=NameOf([Single]))>
    Public ReadOnly Property UnitValueMgdL As Single
        Get
            If Me.UnitValue.IsSgInvalid Then Return Me.UnitValue
            Return If(Me.bgUnits = "MGDL",
                      Me.UnitValue,
                      CSng(Math.Round(Me.UnitValue * MmolLUnitsDivisor))
                     )
        End Get
    End Property

    <DisplayName("UnitValue (mmol/L)")>
    <Column(Order:=11, TypeName:=NameOf([Single]))>
    Public ReadOnly Property UnitValueMmolL As Single
        Get
            If Me.UnitValue.IsSgInvalid Then Return Me.UnitValue
            Return If(Me.bgUnits <> "MGDL",
                      Me.UnitValue,
                      (Me.UnitValue / MmolLUnitsDivisor).RoundSingle(2, False)
                     )
        End Get
    End Property

End Class
