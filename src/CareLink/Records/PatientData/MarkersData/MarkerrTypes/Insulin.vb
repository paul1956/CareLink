' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class Insulin

    Public Sub New(markerEntry As Marker, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.Type = markerEntry.Type
        Me.Kind = "Marker"
        Me.ActivationType = markerEntry.GetStringValueFromJson(NameOf(ActivationType))
        Me.TimestampAsString = markerEntry.TimestampAsString
        Me.DisplayTimeAsString = markerEntry.DisplayTimeAsString
        Me.ProgrammedFastAmount = markerEntry.GetSingleValueFromJson(NameOf(ProgrammedFastAmount), decimalDigits:=3)
        Me.DeliveredFastAmount = markerEntry.GetSingleValueFromJson(NameOf(DeliveredFastAmount), decimalDigits:=3)
        Me.Completed = markerEntry.GetBooleanValueFromJson(NameOf(Completed))
        Me.BolusType = markerEntry.GetStringValueFromJson(NameOf(BolusType))
        Me.ProgrammedExtendedAmount = markerEntry.GetSingleValueFromJson(NameOf(ProgrammedExtendedAmount), decimalDigits:=3)
        Me.DeliveredExtendedAmount = markerEntry.GetSingleValueFromJson(NameOf(DeliveredExtendedAmount), decimalDigits:=3)
        Me.ProgrammedDuration = markerEntry.GetIntegerValueFromJson(NameOf(ProgrammedDuration))
        Me.EffectiveDuration = markerEntry.GetIntegerValueFromJson(NameOf(EffectiveDuration))
        Me.InsulinType = markerEntry.GetStringValueFromJson(NameOf(InsulinType))
        If Me.InsulinType.Equals("Unknown", StringComparison.InvariantCultureIgnoreCase) Then
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

    <DisplayName("OA Date Time")>
    <Column(Order:=7, TypeName:=NameOf([Double]))>
    Public ReadOnly Property OAdateTime As OADate
        Get
            Return New OADate(Me.Timestamp)
        End Get
    End Property

    <DisplayName("Activation Type")>
    <Column(Order:=8, TypeName:="String", TypeName:=NameOf([String]))>
    Public Property ActivationType As String

    <DisplayName("Bolus Type")>
    <Column(Order:=9, TypeName:=NameOf([String]))>
    Public Property BolusType As String

    <DisplayName("Programmed Extended Amount")>
    <Column(Order:=10, TypeName:=NameOf([Single]))>
    Public Property ProgrammedExtendedAmount As Single

    <DisplayName("Delivered Extended Amount")>
    <Column(Order:=12, TypeName:=NameOf([Single]))>
    Public Property DeliveredExtendedAmount As Single

    <DisplayName("Programmed Duration")>
    <Column(Order:=13, TypeName:=NameOf([Int32]))>
    Public Property ProgrammedDuration As Integer

    <DisplayName("Effective Duration")>
    <Column(Order:=14, TypeName:=NameOf([Int32]))>
    Public Property EffectiveDuration As Integer

    <DisplayName("Programmed Fast Amount")>
    <Column(Order:=15, TypeName:=NameOf([Single]))>
    Public Property ProgrammedFastAmount As Single

    <DisplayName("Delivered Fast Amount")>
    <Column(Order:=16, TypeName:=NameOf([Single]))>
    Public Property DeliveredFastAmount As Single

    <DisplayName("Safe Meal Reduction")>
    <Column(Order:=17, TypeName:=NameOf([Single]))>
    Public ReadOnly Property SafeMealReduction As Single
        Get
            If {"RECOMMENDED", "UNDETERMINED"}.Contains(Me.ActivationType) Then
                Dim meal As Meal = Nothing
                If TryGetMealRecord(Me.Timestamp, meal) Then
                    Dim cRatio As Single = CurrentUser.GetCarbRatio(TimeOnly.FromDateTime(meal.Timestamp))
                    Dim expectedBolus As Single = meal.amount / cRatio
                    If expectedBolus - 0.025 > Me.ProgrammedFastAmount Then
                        Return (expectedBolus - Me.ProgrammedFastAmount).RoundTo025
                    End If
                End If
            End If
            Return 0
        End Get
    End Property

    <DisplayName("Completed")>
    <Column(Order:=18, TypeName:=NameOf([Boolean]))>
    Public Property Completed As Boolean

    <DisplayName("Insulin Type")>
    <Column(Order:=19, TypeName:=NameOf([String]))>
    Public Property InsulinType As String

End Class
