' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class SgRecord
    Private _sensorState As String
    Private _sg As Single

    Public Sub New()

    End Sub

    Public Sub New(dic As Dictionary(Of String, String))
        Dim allSgs As New List(Of Dictionary(Of String, String)) From {
            dic
        }
        Dim lastValidTime As Date = Nothing
        Me.processOneSg(allSgs, 0, lastValidTime, dic)
    End Sub

    Public Sub New(allSgs As List(Of Dictionary(Of String, String)), index As Integer, ByRef lastValidTime As Date)
        Dim dic As Dictionary(Of String, String) = allSgs(index)
        Me.RecordNumber = index + 1
        lastValidTime = Me.processOneSg(allSgs, index, lastValidTime, dic)
    End Sub

    <DisplayName(NameOf(SgRecord.datetime))>
    <Column(Order:=2, TypeName:="Date")>
    Public Property [datetime] As Date

    <DisplayName("datetime As String")>
    <Column(Order:=3, TypeName:=NameOf([String]))>
    Public Property datetimeAsString As String

    <DisplayName("Kind")>
    <Column(Order:=8, TypeName:=NameOf([String]))>
    Public Property kind As String

    <DisplayName("Sensor Message")>
    <Column(Order:=7, TypeName:=NameOf([String]))>
    Public ReadOnly Property Message As String
        Get
            _sensorState = If(_sensorState, "")
            Dim resultMessage As String = Nothing
            If s_sensorMessages.TryGetValue(_sensorState, resultMessage) Then
                Return resultMessage
            End If
            Return _sensorState?.ToTitle
        End Get
    End Property

    <DisplayName(NameOf(OaDateTime))>
    <Column(Order:=4, TypeName:=NameOf([Double]))>
    Public ReadOnly Property OaDateTime As OADate
        Get
            Return New OADate(_datetime)
        End Get
    End Property

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName(NameOf(relativeOffset))>
    <Column(Order:=10, TypeName:=NameOf([Int32]))>
    Public Property relativeOffset As Integer

    <DisplayName("Sensor State")>
    <Column(Order:=6, TypeName:=NameOf([String]))>
    Public Property sensorState As String
        Get
            Return _sensorState
        End Get
        Set
            _sensorState = If(Value, "")
        End Set
    End Property

    <DisplayName("Sensor Glucose (sg)")>
    <Column(Order:=1, TypeName:=NameOf([Single]))>
    Public Property sg As Single
        Get
            Return _sg
        End Get
        Set
            If Value = 0 Then
                _sg = Single.NaN
            Else
                _sg = Value
            End If
        End Set
    End Property

    <DisplayName("Time Change")>
    <Column(Order:=5, TypeName:=NameOf([Boolean]))>
    Public Property timeChange As Boolean

    <DisplayName("Version")>
    <Column(Order:=9, TypeName:=NameOf([Int32]))>
    Public Property version As Integer

    Private Function processOneSg(allSgs As List(Of Dictionary(Of String, String)), Index As Integer, lastValidTime As Date, dic As Dictionary(Of String, String)) As Date
        Me.sg = dic(NameOf(sg)).ParseSingle
        Dim value As String = ""
        If dic.TryGetValue(NameOf(Me.datetime), value) Then
            Me.datetime = allSgs.SafeGetSgDateTime(Index)
            lastValidTime = Me.datetime + s_fiveMinuteSpan
        Else
            Me.datetime = lastValidTime
            lastValidTime += s_fiveMinuteSpan
        End If
        Me.datetimeAsString = value
        If dic.TryGetValue(NameOf(timeChange), value) Then
            Me.timeChange = Boolean.Parse(value)
        End If
        If dic.TryGetValue(NameOf(sensorState), value) Then
            Me.sensorState = value
        End If
        If dic.TryGetValue(NameOf(kind), value) Then
            Me.kind = value
        End If
        If dic.TryGetValue(NameOf(version), value) Then
            Me.version = CInt(value)
        End If
        If dic.TryGetValue(NameOf(relativeOffset), value) Then
            Me.relativeOffset = CInt(value)
        End If
        Return lastValidTime
    End Function

End Class
