' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class BgReading

    Public Sub New(markerEntry As Marker, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.Type = markerEntry.Type
        Me.Kind = "Marker"
        Me.Timestamp = markerEntry.Timestamp
        Me.TimestampAsString = markerEntry.TimestampAsString
        Me.DisplayTime = markerEntry.DisplayTime
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
    <Column(Order:=21, TypeName:=NameOf([String]))>
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

    <DisplayName("Unit Value")>
    <Column(Order:=7, TypeName:=NameOf([Single]))>
    Public Property UnitValue As Single

    <DisplayName("Units")>
    <Column(Order:=8, TypeName:=NameOf([String]))>
    Public ReadOnly Property bgUnits As String

    <DisplayName("Value (mg/dL)")>
    <Column(Order:=9, TypeName:=NameOf([Single]))>
    Public ReadOnly Property valueMmDl As Single
        Get
            If Single.IsNaN(Me.unitValue) Then Return Me.unitValue
            Return If(Me.bgUnits = "MGDL",
                      Me.unitValue,
                      CSng(Math.Round(Me.unitValue * MmolLUnitsDivisor))
                     )
        End Get
    End Property

    <DisplayName("Value (mmol/L)")>
    <Column(Order:=10, TypeName:=NameOf([Single]))>
    Public ReadOnly Property valueMmolL As Single
        Get
            If Single.IsNaN(Me.unitValue) Then Return Me.unitValue
            Return If(Me.bgUnits <> "MMDL",
                      Me.unitValue,
                      (Me.unitValue / MmolLUnitsDivisor).RoundSingle(2, False)
                     )
        End Get
    End Property

End Class
