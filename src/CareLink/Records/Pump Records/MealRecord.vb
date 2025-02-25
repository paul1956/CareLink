' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class MealRecord

    Public Sub New(markerEntry As Marker, recordNumber As Integer)
        Me.RecordNumber = recordNumber
        Me.type = markerEntry.Type
#If False Then ' TODO
        Me.index = markerEntry.index
        Me.kind = markerEntry.kind
        Me.version = markerEntry.version
        Me.timestamp = markerEntry.Timestamp
        Me.relativeOffset = markerEntry.relativeOffset
        Me.amount = markerEntry.amount
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

    <DisplayName(NameOf(OAdateTime))>
    <Column(Order:=6, TypeName:=NameOf(OADate))>
    Public ReadOnly Property OAdateTime As OADate
        Get
            Return New OADate(Me.timestamp)
        End Get
    End Property

    <DisplayName(NameOf(relativeOffset))>
    <Column(Order:=7, TypeName:=NameOf([Int32]))>
    Public Property relativeOffset As Integer

    <DisplayName("Carbs (amount)")>
    <Column(Order:=8, TypeName:=NameOf([Int32]))>
    Public Property amount As Integer

    Friend Shared Sub AttachHandlers(dgv As DataGridView)
        Throw New NotImplementedException()
    End Sub

End Class
