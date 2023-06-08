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

    Public Sub New(innerJson As Dictionary(Of String, String), index As Integer)
        Try
            If innerJson(NameOf(sg)) <> "0" OrElse innerJson.Count = 7 Then
                Me.datetimeAsString = innerJson(NameOf(Me.datetime))
                Me.datetime = Me.datetimeAsString.ParseDate(NameOf(SgRecord.datetime))
                Me.sensorState = innerJson(NameOf(sensorState))
                Me.sg = innerJson(NameOf(sg)).ParseSingle(2)
                Me.timeChange = Boolean.Parse(innerJson(NameOf(timeChange)))
            Else
                Me.datetimeAsString = ""
                Me.sg = Single.NaN
            End If
            Me.kind = innerJson(NameOf(kind))
            Me.RecordNumber = index + 1
            Dim offset As String = Nothing
            If innerJson.TryGetValue(NameOf(relativeOffset), offset) Then
                Me.relativeOffset = CInt(offset)
            Else
                Me.relativeOffset = -1
            End If
            Me.version = CInt(innerJson(NameOf(version)))
        Catch ex As Exception
            Stop
        End Try
    End Sub

    <DisplayName(NameOf(SgRecord.datetime))>
    <Column(Order:=4, TypeName:="Date")>
    Public Property [datetime] As Date

    <DisplayName("datetime As String")>
    <Column(Order:=5, TypeName:=NameOf([String]))>
    Public Property datetimeAsString As String

    <DisplayName("Kind")>
    <Column(Order:=10, TypeName:=NameOf([String]))>
    Public Property kind As String

    <DisplayName("Sensor Message")>
    <Column(Order:=12, TypeName:=NameOf([String]))>
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
    <Column(Order:=6, TypeName:=NameOf([Double]))>
    Public ReadOnly Property OaDateTime As OADate
        Get
            Return New OADate(_datetime)
        End Get
    End Property

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName(NameOf(relativeOffset))>
    <Column(Order:=9, TypeName:=NameOf([Int32]))>
    Public Property relativeOffset As Integer

    <DisplayName("Sensor State")>
    <Column(Order:=11, TypeName:=NameOf([String]))>
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

    <DisplayName("Sensor Glucose (mg/dL)")>
    <Column(Order:=2, TypeName:=NameOf([Single]))>
    Public ReadOnly Property sgMmDl As Single
        Get
            If Single.IsNaN(_sg) Then Return _sg
            Return If(ScalingNeeded, CSng(Math.Round(_sg * MmolLUnitsDivisor)), _sg)
        End Get
    End Property

    <DisplayName("Sensor Glucose (mmol/L)")>
    <Column(Order:=3, TypeName:=NameOf([Single]))>
    Public ReadOnly Property sgMmolL As Single
        Get
            If Single.IsNaN(_sg) Then Return _sg
            Return If(ScalingNeeded, _sg, (_sg / MmolLUnitsDivisor).RoundSingle(2, False))
        End Get
    End Property

    <DisplayName("Time Change")>
    <Column(Order:=7, TypeName:=NameOf([Boolean]))>
    Public Property timeChange As Boolean

    <DisplayName("Version")>
    <Column(Order:=8, TypeName:=NameOf([Int32]))>
    Public Property version As Integer

End Class
