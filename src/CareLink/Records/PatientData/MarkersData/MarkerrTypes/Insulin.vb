' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

''' <summary>
'''  Represents an insulin marker record, containing details about insulin delivery events such as bolus, extended bolus, and related metadata.
''' </summary>
Public Class Insulin

    ''' <summary>
    '''  Initializes a new instance of the <see cref="Insulin"/> class using a marker entry and record number.
    ''' </summary>
    ''' <param name="markerEntry">The marker entry containing insulin event data.</param>
    ''' <param name="recordNumber">The record number for this insulin entry.</param>
    Public Sub New(markerEntry As Marker, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.Type = markerEntry.Type
        Me.Kind = "Marker"
        Me.ActivationType = markerEntry.GetStringValueFromJson(NameOf(ActivationType))
        Me.TimestampAsString = markerEntry.TimestampAsString
        Me.DisplayTimeAsString = markerEntry.DisplayTimeAsString
        Me.ProgrammedFastAmount = markerEntry.GetSingleValueFromJson(NameOf(ProgrammedFastAmount), digits:=3)
        Me.DeliveredFastAmount = markerEntry.GetSingleValueFromJson(NameOf(DeliveredFastAmount), digits:=3)
        Me.Completed = markerEntry.GetBooleanValueFromJson(NameOf(Completed))
        Me.BolusType = markerEntry.GetStringValueFromJson(NameOf(BolusType))
        Me.ProgrammedExtendedAmount = markerEntry.GetSingleValueFromJson(NameOf(ProgrammedExtendedAmount), digits:=3)
        Me.DeliveredExtendedAmount = markerEntry.GetSingleValueFromJson(NameOf(DeliveredExtendedAmount), digits:=3)
        Me.ProgrammedDuration = markerEntry.GetIntegerValueFromJson(NameOf(ProgrammedDuration))
        Me.EffectiveDuration = markerEntry.GetIntegerValueFromJson(NameOf(EffectiveDuration))
        Me.InsulinType = markerEntry.GetStringValueFromJson(NameOf(InsulinType))
        If Me.InsulinType.EqualsIgnoreCase("Unknown") Then
            Me.InsulinType = CurrentUser.InsulinTypeName
        End If
    End Sub

    ''' <summary>
    '''  Gets or sets the record number for this insulin entry.
    ''' </summary>
    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    ''' <summary>
    '''  Gets or sets the type of the marker.
    ''' </summary>
    <DisplayName("Type")>
    <Column(Order:=1, TypeName:=NameOf([Int32]))>
    Public Property Type As String

    ''' <summary>
    '''  Gets the kind of the record, always "Marker".
    ''' </summary>
    <DisplayName("Kind")>
    <Column(Order:=2, TypeName:=NameOf([String]))>
    Public ReadOnly Property Kind As String

    ''' <summary>
    '''  Gets or sets the timestamp as a string from the pump.
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
    '''  Gets or sets the display time as a string from the pump.
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
    '''  Gets the OA date/time representation of the timestamp.
    ''' </summary>
    <DisplayName("OA Timestamp")>
    <Column(Order:=7, TypeName:=NameOf([Double]))>
    Public ReadOnly Property OAdateTime As OADate
        Get
            Return New OADate(Me.Timestamp)
        End Get
    End Property

    ''' <summary>
    '''  Gets or sets the activation type for the insulin event.
    ''' </summary>
    <DisplayName("Activation Type")>
    <Column(Order:=8, TypeName:="String", TypeName:=NameOf([String]))>
    Public Property ActivationType As String

    ''' <summary>
    '''  Gets or sets the bolus type for the insulin event.
    ''' </summary>
    <DisplayName("Bolus Type")>
    <Column(Order:=9, TypeName:=NameOf([String]))>
    Public Property BolusType As String

    ''' <summary>
    '''  Gets or sets the programmed extended amount of insulin (units).
    ''' </summary>
    <DisplayName("Programmed Extended Amount")>
    <Column(Order:=10, TypeName:=NameOf([Single]))>
    Public Property ProgrammedExtendedAmount As Single

    ''' <summary>
    '''  Gets or sets the delivered extended amount of insulin (units).
    ''' </summary>
    <DisplayName("Delivered Extended Amount")>
    <Column(Order:=12, TypeName:=NameOf([Single]))>
    Public Property DeliveredExtendedAmount As Single

    ''' <summary>
    '''  Gets or sets the programmed duration for the extended bolus (minutes).
    ''' </summary>
    <DisplayName("Programmed Duration")>
    <Column(Order:=13, TypeName:=NameOf([Int32]))>
    Public Property ProgrammedDuration As Integer

    ''' <summary>
    '''  Gets or sets the effective duration for the extended bolus (minutes).
    ''' </summary>
    <DisplayName("Effective Duration")>
    <Column(Order:=14, TypeName:=NameOf([Int32]))>
    Public Property EffectiveDuration As Integer

    ''' <summary>
    '''  Gets or sets the programmed fast (immediate) amount of insulin (units).
    ''' </summary>
    <DisplayName("Programmed Fast Amount")>
    <Column(Order:=15, TypeName:=NameOf([Single]))>
    Public Property ProgrammedFastAmount As Single

    ''' <summary>
    '''  Gets or sets the delivered fast (immediate) amount of insulin (units).
    ''' </summary>
    <DisplayName("Delivered Fast Amount")>
    <Column(Order:=16, TypeName:=NameOf([Single]))>
    Public Property DeliveredFastAmount As Single

    ''' <summary>
    '''  Gets the safe meal reduction value, representing the difference between expected and programmed bolus if applicable.
    ''' </summary>
    <DisplayName("Safe Meal Reduction")>
    <Column(Order:=17, TypeName:=NameOf([Single]))>
    Public ReadOnly Property SafeMealReduction As Single
        Get
            If {"RECOMMENDED", "UNDETERMINED"}.Contains(Me.ActivationType) Then
                Dim meal As Meal = Nothing
                If Meal.TryGetMealRecord(Me.Timestamp, meal) Then
                    Dim cRatio As Single = CurrentUser.GetCarbRatio(TimeOnly.FromDateTime(meal.Timestamp))
                    Dim expectedBolus As Single = meal.Amount / cRatio
                    If expectedBolus - 0.025 > Me.ProgrammedFastAmount Then
                        Return (expectedBolus - Me.ProgrammedFastAmount).RoundTo025
                    End If
                End If
            End If
            Return 0
        End Get
    End Property

    ''' <summary>
    '''  Gets or sets a value indicating whether the insulin delivery was completed.
    ''' </summary>
    <DisplayName("Completed")>
    <Column(Order:=18, TypeName:=NameOf([Boolean]))>
    Public Property Completed As Boolean

    ''' <summary>
    '''  Gets or sets the type of insulin used for this event.
    ''' </summary>
    <DisplayName("Insulin Type")>
    <Column(Order:=19, TypeName:=NameOf([String]))>
    Public Property InsulinType As String

End Class
