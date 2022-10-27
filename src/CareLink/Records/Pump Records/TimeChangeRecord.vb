' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class TimeChangeRecord
    Private _previousDateTime As Date
    Private _dateTime As Date

    Public Sub New(timeChangeItem As Dictionary(Of String, String))
        For Each kvp As KeyValuePair(Of String, String) In timeChangeItem
            Select Case kvp.Key
                Case NameOf(type)
                    Me.type = kvp.Value
                Case NameOf(index)
                    Me.index = CInt(kvp.Value)
                Case NameOf(kind)
                    Me.kind = kvp.Value
                Case NameOf(version)
                    Me.version = CInt(kvp.Value)
                Case NameOf(Me.dateTime)
                    Me.dateTime = kvp.Value.ParseDate(NameOf(Me.dateTime), NameOf(Me.dateTime))
                    Me.dateTimeAsString = kvp.Value
                Case NameOf(relativeOffset)
                    Me.relativeOffset = CInt(kvp.Value)
                Case NameOf(previousDateTime)
                    Me.previousDateTime = kvp.Value.ParseDate(NameOf(previousDateTime))
                    Me.previousDateTimeAsString = kvp.Value
                Case Else
                    Stop
            End Select
        Next
    End Sub

    <DisplayName("Type")>
    <Column(Order:=1, TypeName:="String")>
    Public Property type As String

    <DisplayName(NameOf(index))>
    <Column(Order:=2, TypeName:="Integer")>
    Public Property index As Integer

    <DisplayName("Kind")>
    <Column(Order:=3, TypeName:="Integer")>
    Public Property kind As String

    <DisplayName("Version")>
    <Column(Order:=4, TypeName:="Integer")>
    Public Property version As Integer

    <DisplayName(NameOf([dateTime]))>
    <Column(Order:=5, TypeName:="Date")>
    Public Property [dateTime] As Date
        Get
            Return _dateTime
        End Get
        Set
            _dateTime = Value
        End Set
    End Property

    <DisplayName("dateTime As String")>
    <Column(Order:=6, TypeName:="String")>
    Public Property dateTimeAsString As String

    <DisplayName("OA dateTime")>
    <Column(Order:=7, TypeName:="OADate")>
    Public ReadOnly Property OAdateTime As OADate
        Get
            Return New OADate(_dateTime)
        End Get
    End Property

    <DisplayName(NameOf(relativeOffset))>
    <Column(Order:=8, TypeName:="Integer")>
    Public Property relativeOffset As Integer

    <DisplayName("Previous DateTime")>
    <Column(Order:=9, TypeName:="Date")>
    Public Property previousDateTime As Date
        Get
            Return _previousDateTime
        End Get
        Set
            _previousDateTime = Value
        End Set
    End Property

    <DisplayName("Previous DateTime As String")>
    <Column(Order:=10, TypeName:="String")>
    Public Property previousDateTimeAsString As String

    <DisplayName("Previous DateTime")>
    <Column(Order:=11, TypeName:="Double")>
    Public ReadOnly Property previousOADateTime As OADate
        Get
            Return New OADate(_previousDateTime)
        End Get
    End Property

    <DisplayName("Delta OA TimeSpan")>
    <Column(Order:=12, TypeName:="TimeSpan")>
    Public ReadOnly Property deltaOATimeSpan As TimeSpan
        Get
            Return _previousDateTime - _dateTime
        End Get
    End Property

End Class
