' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class TimeChangeRecord

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
                    Me.dateTime = kvp.Value.ParseDate(NameOf(Me.dateTime))
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

    <DisplayName(NameOf([dateTime]))>
    <Column(Order:=5, TypeName:="Date")>
    Public Property [dateTime] As Date

    <DisplayName("dateTime As String")>
    <Column(Order:=6, TypeName:=NameOf([String]))>
    Public Property dateTimeAsString As String

    <DisplayName("Delta TimeSpan")>
    <Column(Order:=12, TypeName:="TimeSpan")>
    Public ReadOnly Property deltaTimeSpan As TimeSpan
        Get
            Return Me.previousDateTime - Me.dateTime
        End Get
    End Property

    <DisplayName(NameOf(index))>
    <Column(Order:=2, TypeName:=NameOf([Int32]))>
    Public Property index As Integer

    <DisplayName("Kind")>
    <Column(Order:=3, TypeName:=NameOf([Int32]))>
    Public Property kind As String

    <DisplayName("OAdateTime")>
    <Column(Order:=7, TypeName:=NameOf(OADate))>
    Public ReadOnly Property OaDateTime As OADate
        Get
            Return New OADate(Me.dateTime)
        End Get
    End Property

    <DisplayName("Previous DateTime")>
    <Column(Order:=9, TypeName:="Date")>
    Public Property previousDateTime As Date

    <DisplayName("Previous DateTime As String")>
    <Column(Order:=10, TypeName:=NameOf([String]))>
    Public Property previousDateTimeAsString As String

    <DisplayName("Previous OADateTime")>
    <Column(Order:=11, TypeName:=NameOf(OADate))>
    Public ReadOnly Property previousOADateTime As OADate
        Get
            Return New OADate(Me.previousDateTime)
        End Get
    End Property

    <DisplayName(NameOf(relativeOffset))>
    <Column(Order:=8, TypeName:=NameOf([Int32]))>
    Public Property relativeOffset As Integer

    <DisplayName("Type")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    Public Property type As String

    <DisplayName("Version")>
    <Column(Order:=4, TypeName:=NameOf([Int32]))>
    Public Property version As Integer

    Public Function GetLatestTime() As Date
        Return If(DateDiff(DateInterval.Second, Me.previousDateTime, Me.dateTime) < 0, Me.previousDateTime, Me.dateTime)
    End Function

End Class
