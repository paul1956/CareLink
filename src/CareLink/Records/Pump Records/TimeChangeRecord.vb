' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class TimeChangeRecord

    Public Sub New(timeChangeItem As Marker)
        Me.type = timeChangeItem.Type
#If False Then ' TODO
        Me.index = timeChangeItem.index
        Me.kind = timeChangeItem.kind
        Me.version = timeChangeItem.version
        Me.timestamp = timeChangeItem.Timestamp
        Me.relativeOffset = timeChangeItem.relativeOffset
        Me.previousTimeStamp = timeChangeItem.previousTimeStamp
#End If
    End Sub

    <DisplayName("Type")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    Public Property type As String

    <DisplayName(NameOf(index))>
    <Column(Order:=2, TypeName:=NameOf([Int32]))>
    Public Property index As Integer

    <DisplayName("Kind")>
    <Column(Order:=3, TypeName:=NameOf([Int32]))>
    Public Property kind As String

    <DisplayName("Version")>
    <Column(Order:=4, TypeName:=NameOf([Int32]))>
    Public Property version As Integer

    <DisplayName(NameOf(timestamp))>
    <Column(Order:=5, TypeName:="Date")>
    Public Property timestamp As Date

    <DisplayName("OAdateTime")>
    <Column(Order:=6, TypeName:=NameOf(OADate))>
    Public ReadOnly Property OaDateTime As OADate
        Get
            Return New OADate(Me.timestamp)
        End Get
    End Property

    <DisplayName(NameOf(relativeOffset))>
    <Column(Order:=7, TypeName:=NameOf([Int32]))>
    Public Property relativeOffset As Integer

    <DisplayName("Previous DateTime")>
    <Column(Order:=8, TypeName:="Date")>
    Public Property previousTimeStamp As Date

    <DisplayName("Previous OADateTime")>
    <Column(Order:=9, TypeName:=NameOf(OADate))>
    Public ReadOnly Property previousOADateTime As OADate
        Get
            Return New OADate(Me.previousTimeStamp)
        End Get
    End Property

    <DisplayName("Delta TimeSpan")>
    <Column(Order:=10, TypeName:="TimeSpan")>
    Public ReadOnly Property deltaTimeSpan As TimeSpan
        Get
            Return Me.previousTimeStamp - Me.timestamp
        End Get
    End Property

    Public Function GetLatestTime() As Date
        Return If(DateDiff(DateInterval.Second, Me.previousTimeStamp, Me.timestamp) < 0, Me.previousTimeStamp, Me.timestamp)
    End Function

End Class
