' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class Insulin
    Private _programmedFastAmount As Single

    Public Sub New(markerEntry As Marker, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.Type = markerEntry.Type
        Me.Kind = "Marker"
        Me.TimestampAsString = markerEntry.TimestampAsString
        Me.DisplayTimeAsString = markerEntry.DisplayTimeAsString
        Me.ProgrammedFastAmount = markerEntry.GetSingleValueFromJson(NameOf(ProgrammedFastAmount), decimalDigits:=3)
        Me.DeliveredFastAmount = markerEntry.GetSingleValueFromJson(NameOf(DeliveredFastAmount), decimalDigits:=3)
        Me.ActivationType = markerEntry.GetStringValueFromJson(NameOf(ActivationType))
        Me.completed = markerEntry.GetBooleanValueFromJson(NameOf(completed))
        Me.BolusType = markerEntry.GetStringValueFromJson(NameOf(BolusType))
        Me.DeliveredExtendedAmount = markerEntry.GetSingleValueFromJson(NameOf(DeliveredExtendedAmount), decimalDigits:=3)
        Me.ProgrammedExtendedAmount = markerEntry.GetSingleValueFromJson(NameOf(ProgrammedExtendedAmount), decimalDigits:=3)
        Me.ProgrammedFastAmount = markerEntry.GetSingleValueFromJson(NameOf(ProgrammedFastAmount), decimalDigits:=3)
        Me.SafeMealReduction = markerEntry.GetSingleValueFromJson(NameOf(SafeMealReduction), decimalDigits:=3)
        Me.UnknownIncompletedFlag = markerEntry.GetBooleanValueFromJson(NameOf(UnknownIncompletedFlag))
        Me.EffectiveDuration = markerEntry.GetIntegerValueFromJson(NameOf(EffectiveDuration))
        Me.ProgrammedDuration = markerEntry.GetIntegerValueFromJson(NameOf(ProgrammedDuration))
        Me.InsulinType = markerEntry.GetStringValueFromJson(NameOf(InsulinType))
        If Me.InsulinType = "Unknown" Then
            Me.InsulinType = CurrentUser.InsulinTypeName
        End If
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

    <DisplayName("Timestamp")>
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

    <DisplayName("Display Time")>
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

    <DisplayName("Unknown Incompleted Flag")>
    <Column(Order:=7, TypeName:=NameOf([Boolean]))>
    Public Property UnknownIncompletedFlag As Boolean

    <DisplayName("OA Date Time")>
    <Column(Order:=8, TypeName:=NameOf([Double]))>
    Public ReadOnly Property OAdateTime As OADate
        Get
            Return New OADate(Me.Timestamp)
        End Get
    End Property

    <DisplayName("Programmed Extended Amount")>
    <Column(Order:=9, TypeName:=NameOf([Single]))>
    Public Property ProgrammedExtendedAmount As Single

    <DisplayName("Activation Type")>
    <Column(Order:=10, TypeName:="String", TypeName:=NameOf([String]))>
    Public Property ActivationType As String

    <DisplayName("Delivered Extended Amount")>
    <Column(Order:=11, TypeName:=NameOf([Single]))>
    Public Property DeliveredExtendedAmount As Single

    <DisplayName("Programmed Fast Amount")>
    <Column(Order:=12, TypeName:=NameOf([Single]))>
    Public Property ProgrammedFastAmount As Single
        Get
            Return _programmedFastAmount
        End Get
        Set
            If {"RECOMMENDED", "UNDETERMINED"}.Contains(Me.ActivationType) Then
                Dim meal As Meal = Nothing
                If TryGetMealRecord(Me.RecordNumber, meal) Then
                    Dim cRatio As Single = CurrentUser.GetCarbRatio(TimeOnly.FromDateTime(meal.Timestamp))
                    Dim expectedBolus As Single = (meal.amount / cRatio).RoundTo025
                    If expectedBolus > Value Then
                        Me.SafeMealReduction = (expectedBolus - Value).RoundTo025
                    End If
                End If
            End If
            _programmedFastAmount = Value
        End Set
    End Property

    <DisplayName("Programmed Duration")>
    <Column(Order:=13, TypeName:=NameOf([Int32]))>
    Public Property ProgrammedDuration As Integer

    <DisplayName("Delivered Fast Amount")>
    <Column(Order:=14, TypeName:=NameOf([Single]))>
    Public Property DeliveredFastAmount As Single

    <DisplayName("Effective Duration")>
    <Column(Order:=15, TypeName:=NameOf([Int32]))>
    Public Property EffectiveDuration As Integer

    <DisplayName("Safe Meal Reduction")>
    <Column(Order:=16, TypeName:=NameOf([Single]))>
    Public Property SafeMealReduction As Single

    <DisplayName("Completed")>
    <Column(Order:=17, TypeName:=NameOf([Boolean]))>
    Public Property completed As Boolean

    <DisplayName("Bolus Type")>
    <Column(Order:=18, TypeName:=NameOf([String]))>
    Public Property BolusType As String

    <DisplayName("Insulin Type")>
    <Column(Order:=19, TypeName:=NameOf([String]))>
    Public Property InsulinType As String

End Class
