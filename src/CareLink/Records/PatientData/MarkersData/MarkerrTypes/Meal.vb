' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

''' <summary>
'''  Represents a meal marker record, containing information about a meal event such as timestamp, display time,
'''  and carbohydrate amount.
''' </summary>
Public Class Meal

    ''' <summary>
    '''  Initializes a new instance of the <see cref="Meal"/> class using a marker entry and record number.
    ''' </summary>
    ''' <param name="markerEntry">
    '''  The marker entry containing meal data.
    ''' </param>
    ''' <param name="recordNumber">
    '''  The record number for this meal marker.
    ''' </param>
    Public Sub New(markerEntry As Marker, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.Type = markerEntry.Type
        Me.Kind = "Marker"
        Me.TimestampAsString = markerEntry.TimestampAsString
        Me.DisplayTimeAsString = markerEntry.DisplayTimeAsString
        Me.Amount = CInt(markerEntry.GetSingleValueFromJson("amount", digits:=0))
    End Sub

    ''' <summary>
    '''  Gets or sets the record number for this meal marker.
    ''' </summary>
    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    ''' <summary>
    '''  Gets or sets the type of the marker.
    ''' </summary>
    <DisplayName("Type")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    <JsonPropertyName("type")>
    Public Property Type As String

    ''' <summary>
    '''  Gets or sets the kind of marker. For meal markers, this is always "Marker".
    ''' </summary>
    <DisplayName("Kind")>
    <Column(Order:=2, TypeName:=NameOf([String]))>
    Public Property Kind As String

    ''' <summary>
    '''  Gets or sets the timestamp as a string, as provided by the pump.
    ''' </summary>
    <DisplayName("Timestamp From Pump")>
    <Column(Order:=3, TypeName:="String")>
    <JsonPropertyName("timestamp")>
    Public Property TimestampAsString As String

    ''' <summary>
    '''  Gets the timestamp as a <see cref="Date"/> object, parsed from <see cref="TimestampAsString"/>.
    ''' </summary>
    <DisplayName("Timestamp As Date")>
    <Column(Order:=4, TypeName:="Date")>
    <JsonPropertyName("timestampAsDate")>
    Public ReadOnly Property Timestamp As Date
        Get
            Return TryParseDateStr(Me.TimestampAsString)
        End Get
    End Property

    ''' <summary>
    '''  Gets or sets the display time as a string, as provided by the pump.
    ''' </summary>
    <DisplayName("Display Time From Pump")>
    <Column(Order:=5, TypeName:="String")>
    <JsonPropertyName("displayTime")>
    Public Property DisplayTimeAsString As String

    ''' <summary>
    '''  Gets the display time as a <see cref="Date"/> object, parsed from <see cref="DisplayTimeAsString"/>.
    ''' </summary>
    <DisplayName("Display Time As Date")>
    <Column(Order:=6, TypeName:="Date")>
    <JsonPropertyName("displayTimeAsDate")>
    Public ReadOnly Property DisplayTime As Date
        Get
            Return TryParseDateStr(Me.DisplayTimeAsString)
        End Get
    End Property

    ''' <summary>
    '''  Gets or sets the amount of carbohydrates (in grams) for the meal.
    ''' </summary>
    <DisplayName("Carbs (amount)")>
    <Column(Order:=8, TypeName:=NameOf([Int32]))>
    <JsonPropertyName("amount")>
    Public Property Amount As Integer

    ''' <summary>
    '''  Attempts to find a meal record with the specified timestamp.
    ''' </summary>
    ''' <param name="timestamp">
    '''  The timestamp to search for.
    ''' </param>
    ''' <param name="meal">
    '''  When this method returns, contains the found <see cref="Meal"/> if found; otherwise, <see langword="Nothing"/>.
    ''' </param>
    ''' <returns>
    '''  <see langword="True"/> if a meal record with the specified timestamp is found; otherwise, <see langword="False"/>.
    ''' </returns>
    Public Shared Function TryGetMealRecord(timestamp As Date, ByRef meal As Meal) As Boolean
        For Each m As Meal In s_mealMarkers
            If timestamp = m.Timestamp Then
                meal = m
                Return True
            End If
        Next
        Return False
    End Function

End Class
