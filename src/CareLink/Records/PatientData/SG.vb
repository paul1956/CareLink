' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Globalization
Imports System.Text.Json.Serialization

Public Class SG

    Private _sensorState As String
    Private _sg As Single
    Private _timestampAsString As String

    Public Sub New()
    End Sub

    Public Sub New(lastSg As LastSG, index As Integer)
        If lastSg.Sg <> 0 Then
            _timestampAsString = lastSg.Timestamp.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
            Me.sensorState = lastSg.SensorState
            Me.sg = lastSg.Sg
            Me.timeChange = False
        Else
            Me.sg = Single.NaN
        End If
        Me.Kind = lastSg.Kind
        Me.RecordNumber = index
        Me.Version = lastSg.Version
    End Sub

    Public Sub New(innerJson As Dictionary(Of String, String), index As Integer)
        Try
            If innerJson(NameOf(sg)) <> "0" OrElse innerJson.Count = 5 Then
                _timestampAsString = innerJson(NameOf(Me.Timestamp))
                Me.sensorState = innerJson(NameOf(sensorState))
                Me.sg = innerJson(NameOf(sg)).ParseSingle(decimalDigits:=2)
                Dim value As String = "False"
                Me.timeChange = innerJson.TryGetValue(NameOf(timeChange), value) AndAlso Boolean.Parse(innerJson(NameOf(timeChange)))
            Else
                Me.sg = Single.NaN
            End If
            Me.Kind = innerJson(NameOf(Kind))
            Me.RecordNumber = index + 1
            Me.Version = CInt(innerJson(NameOf(Version)))
        Catch ex As Exception
            Stop
        End Try
    End Sub

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer

    <DisplayName("Kind")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    <JsonPropertyName("kind")>
    Public Property Kind As String

    <DisplayName("Version")>
    <Column(Order:=2, TypeName:=NameOf([Int32]))>
    <JsonPropertyName("version")>
    Public Property Version As Integer

    <DisplayName("Sensor Glucose (sg)")>
    <Column(Order:=3, TypeName:=NameOf([Single]))>
    <JsonPropertyName("sg")>
    Public Property sg As Single
        Get
            Return _sg
        End Get
        Set
            _sg = If(Value = 0,
                     Single.NaN,
                     Value
                    )
        End Set
    End Property

    <DisplayName("Sensor Glucose (mg/dL)")>
    <Column(Order:=4, TypeName:=NameOf([Single]))>
    Public ReadOnly Property sgMgdL As Single
        Get
            If Single.IsNaN(_sg) Then Return _sg
            Return If(NativeMmolL, CSng(Math.Round(_sg * MmolLUnitsDivisor)), _sg)
        End Get
    End Property

    <DisplayName("Sensor Glucose (mmol/L)")>
    <Column(Order:=5, TypeName:=NameOf([Single]))>
    Public ReadOnly Property sgMmolL As Single
        Get
            If Single.IsNaN(_sg) Then Return _sg
            Return If(NativeMmolL, _sg, (_sg / MmolLUnitsDivisor).RoundSingle(2, False))
        End Get
    End Property

    <DisplayName(NameOf(Timestamp))>
    <Column(Order:=6, TypeName:="Date")>
    <JsonPropertyName("timestampAsDate")>
    Public ReadOnly Property Timestamp As Date
        Get
            Return If(String.IsNullOrWhiteSpace(_timestampAsString),
                Nothing,
                Date.ParseExact(_timestampAsString, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture))
        End Get
    End Property

    <DisplayName(NameOf(Timestamp))>
    <Column(Order:=7, TypeName:="Date")>
    <JsonPropertyName("timestamp")>
    Public Property TimestampAsString As String
        Get
            Return _timestampAsString
        End Get
        Set
            _timestampAsString = Value
        End Set
    End Property

    <DisplayName(NameOf(OaDateTime))>
    <Column(Order:=8, TypeName:=NameOf([Double]))>
    Public ReadOnly Property OaDateTime As OADate
        Get
            Return New OADate(Me.Timestamp)
        End Get
    End Property

    <DisplayName("Time Change")>
    <Column(Order:=9, TypeName:=NameOf([Boolean]))>
    Public Property timeChange As Boolean

    <DisplayName("Sensor State")>
    <Column(Order:=10, TypeName:=NameOf([String]))>
    <JsonPropertyName("sensorState")>
    Public Property sensorState As String
        Get
            Return _sensorState
        End Get
        Set
            _sensorState = If(Value, "")
        End Set
    End Property

    <DisplayName("Sensor Message")>
    <Column(Order:=11, TypeName:=NameOf([String]))>
    Public ReadOnly Property Message As String
        Get
            _sensorState = If(_sensorState, "")
            Dim resultMessage As String = Nothing
            Return If(s_sensorMessages.TryGetValue(_sensorState, resultMessage),
                      resultMessage,
                      _sensorState?.ToTitle
                    )
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return If(NativeMmolL, Me.sg.ToString("F1", Provider), Me.sg.ToString("F0"))
    End Function

End Class
