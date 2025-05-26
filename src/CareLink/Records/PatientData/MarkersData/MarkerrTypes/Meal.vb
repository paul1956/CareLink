' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class Meal

    Public Sub New(markerEntry As Marker, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.Type = markerEntry.Type
        Me.Kind = "Marker"
        Me.TimestampAsString = markerEntry.TimestampAsString
        Me.DisplayTimeAsString = markerEntry.DisplayTimeAsString
        Me.Amount = CInt(markerEntry.GetSingleValueFromJson("amount", decimalDigits:=0))
    End Sub

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName("Type")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    <JsonPropertyName("type")>
    Public Property Type As String

    <DisplayName("Kind")>
    <Column(Order:=2, TypeName:=NameOf([String]))>
    Public Property Kind As String

    <DisplayName("Timestamp From Pump")>
    <Column(Order:=3, TypeName:="String")>
    <JsonPropertyName("timestamp")>
    Public Property TimestampAsString As String

    <DisplayName("Timestamp As Date")>
    <Column(Order:=4, TypeName:="Date")>
    <JsonPropertyName("timestampAsDate")>
    Public ReadOnly Property Timestamp As Date
        Get
            Return TryParseDateStr(Me.TimestampAsString)
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
            Return TryParseDateStr(Me.DisplayTimeAsString)
        End Get
    End Property

    <DisplayName("Carbs (amount)")>
    <Column(Order:=8, TypeName:=NameOf([Int32]))>
    <JsonPropertyName("amount")>
    Public Property Amount As Integer

    Public Shared Function TryGetMealRecord(timestamp As Date, ByRef meal As Meal) As Boolean
        For Each m As Meal In s_listOfMealMarkers
            If timestamp = m.Timestamp Then
                meal = m
                Return True
            End If
        Next
        Stop
        Return False
    End Function

End Class
