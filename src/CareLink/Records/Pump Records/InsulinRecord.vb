﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class InsulinRecord
    Private _programmedFastAmount As Single

    Public Sub New(markerEntry As Marker, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.type = markerEntry.Type
        Me.timestamp = markerEntry.Timestamp
#If alse Then ' TODO

        Me.index = markerEntry.index
        Me.kind = markerEntry.kind
        Me.version = markerEntry.version
        Me.programmedFastAmount = markerEntry programmedFastAmount
        Me.unknownIncompletedFlag = markerEntry.unknownIncompletedFlag
        Me.relativeOffset = markerEntry.relativeOffset
        Me.programmedExtendedAmount = markerEntry.programmedExtendedAmount
        Me.activationType = markerEntry.activationType
        Me.deliveredExtendedAmount = markerEntry.deliveredExtendedAmount
        Me.programmedFastAmount = markerEntry.programmedFastAmount
        Me.programmedDuration = markerEntry.programmedDuration
        Me.deliveredFastAmount = markerEntry.deliveredFastAmount
        Me.id = markerEntry.id
        Me.effectiveDuration = markerEntry.effectiveDuration
        Me.SafeMealReduction = markerEntry.SafeMealReduction
        Me.completed = markerEntry.completed
        Me.bolusType = markerEntry.bolusType
#End If
    End Sub

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName("Type")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    Public Property type As String

    <DisplayName(NameOf(index))>
    <Column(Order:=2, TypeName:=NameOf([Int32]))>
    Public Property index As Integer

    <DisplayName("Kind")>
    <Column(Order:=3, TypeName:=NameOf([String]))>
    Public Property kind As String

    <DisplayName("Version")>
    <Column(Order:=4, TypeName:=NameOf([Int32]))>
    Public Property version As Integer

    <DisplayName(NameOf(timestamp))>
    <Column(Order:=5, TypeName:="Date")>
    Public Property timestamp As Date

    <DisplayName("Unknown Incompleted Flag")>
    <Column(Order:=6, TypeName:=NameOf([Boolean]))>
    Public Property unknownIncompletedFlag As Boolean

    <DisplayName("OA Date Time")>
    <Column(Order:=7, TypeName:=NameOf([Double]))>
    Public ReadOnly Property OAdateTime As OADate
        Get
            Return New OADate(Me.timestamp)
        End Get
    End Property

    <DisplayName(NameOf(relativeOffset))>
    <Column(Order:=8, TypeName:=NameOf([Int32]))>
    Public Property relativeOffset As Integer

    <DisplayName("Programmed Extended Amount")>
    <Column(Order:=9, TypeName:=NameOf([Single]))>
    Public Property programmedExtendedAmount As Single

    <DisplayName("Activation Type")>
    <Column(Order:=10, TypeName:="String", TypeName:=NameOf([String]))>
    Public Property activationType As String

    <DisplayName("Delivered Extended Amount")>
    <Column(Order:=11, TypeName:=NameOf([Single]))>
    Public Property deliveredExtendedAmount As Single

    <DisplayName("Programmed Fast Amount")>
    <Column(Order:=12, TypeName:=NameOf([Single]))>
    Public Property programmedFastAmount As Single
        Get
            Return _programmedFastAmount
        End Get
        Set
            If {"RECOMMENDED", "UNDETERMINED"}.Contains(Me.activationType) Then
                Dim meal As MealRecord = Nothing
                If TryGetMealRecord(Me.index, meal) Then
                    Dim cRatio As Single = CurrentUser.GetCarbRatio(TimeOnly.FromDateTime(meal.timestamp))
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
    Public Property programmedDuration As Integer

    <DisplayName("Delivered Fast Amount")>
    <Column(Order:=14, TypeName:=NameOf([Single]))>
    Public Property deliveredFastAmount As Single

    <DisplayName(NameOf(id))>
    <Column(Order:=15, TypeName:=NameOf([Int32]))>
    Public Property id As Integer

    <DisplayName("Effective Duration")>
    <Column(Order:=16, TypeName:=NameOf([Int32]))>
    Public Property effectiveDuration As Integer

    <DisplayName("Safe Meal Reduction")>
    <Column(Order:=17, TypeName:=NameOf([Single]))>
    Public Property SafeMealReduction As Single

    <DisplayName("Completed")>
    <Column(Order:=18, TypeName:=NameOf([Boolean]))>
    Public Property completed As Boolean

    <DisplayName("Bolus Type")>
    <Column(Order:=19, TypeName:=NameOf([String]))>
    Public Property bolusType As String

End Class
