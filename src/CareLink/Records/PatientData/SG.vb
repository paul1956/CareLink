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

    Public Sub New()
    End Sub

    Public Sub New(lastSg As LastSG)
        Me.Kind = lastSg.Kind
        Me.RecordNumber = 0
        _sensorState = lastSg.SensorState
        _sg = If(lastSg.Sg.IsSgValid,
                 If(NativeMmolL,
                    lastSg.Sg / MmolLUnitsDivisor,
                    lastSg.Sg),
                 Single.NaN)
        Me.timeChange = False
        Me.TimestampAsString = lastSg.TimestampAsString
        Me.Version = lastSg.Version
    End Sub

    Public Sub New(json As Dictionary(Of String, String), index As Integer)
        Try
            If json(key:=NameOf(sg)) <> "0" OrElse json.Count = 5 Then
                Me.TimestampAsString = json(key:=NameOf(Me.Timestamp))
                Me.SensorState = json(key:=NameOf(SensorState))
                Me.sg = json(key:=NameOf(sg)).ParseSingle(digits:=2)
                Dim value As String = "False"
                Me.timeChange = json.TryGetValue(key:=NameOf(timeChange), value) AndAlso Boolean.Parse(value)
            Else
                Me.sg = Single.NaN
            End If
            Me.Kind = json(key:=NameOf(Kind))
            Me.RecordNumber = index + 1
            Me.Version = CInt(json(key:=NameOf(Version)))
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
                     Value)
        End Set
    End Property

    <DisplayName("Sensor Glucose (mg/dL)")>
    <Column(Order:=4, TypeName:=NameOf([Single]))>
    Public ReadOnly Property sgMgdL As Single
        Get
            If _sg.IsSgInvalid Then Return _sg
            Return If(NativeMmolL,
                      CSng(Math.Round(_sg * MmolLUnitsDivisor)),
                      _sg)
        End Get
    End Property

    <DisplayName("Sensor Glucose (mmol/L)")>
    <Column(Order:=5, TypeName:=NameOf([Single]))>
    Public ReadOnly Property sgMmolL As Single
        Get
            If Single.IsNaN(_sg) Then Return _sg
            Return If(NativeMmolL, _sg, (_sg / MmolLUnitsDivisor).RoundSingle(digits:=2, considerValue:=False))
        End Get
    End Property

    <DisplayName("Timestamp From Pump")>
    <Column(Order:=6, TypeName:="Date")>
    <JsonPropertyName("timestamp")>
    Public Property TimestampAsString As String

    <DisplayName("Timestamp As Date")>
    <Column(Order:=7, TypeName:="Date")>
    <JsonPropertyName("timestampAsDate")>
    Public ReadOnly Property Timestamp As Date
        Get
            Return Me.TimestampAsString.TryParseDateStr()
        End Get
    End Property

    <DisplayName("OA Date Time")>
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
    Public Property SensorState As String
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
            Return TranslateAndTruncateSensorMessage(key:=PatientData.SensorState, truncate:=False)
        End Get
    End Property

    Public Overrides Function ToString() As String
        Dim provider As CultureInfo = CultureInfo.CurrentUICulture
        Return Me.sg.ToString(format:=GetFormatForBg(), provider)
    End Function

    ''' <summary>
    '''  Translates the sensor state message based on the <see langword="key"/>.
    '''  If the sensor state is not recognized, it will return the state as a title-cased string.
    '''  If debugging is enabled, it will show a message box with the unknown sensor state.
    '''  If the sensor state message contains an ellipsis ("..."), it will truncate the message to the first line.
    ''' </summary>
    ''' <param name="key">The message key</param>
    ''' <returns>
    '''  A string representing the translated and truncated sensor state message.
    ''' </returns>
    ''' <param name="truncate">If <see langword="True"/> the message will be truncated to the first line.</param>
    Public Shared Function TranslateAndTruncateSensorMessage(key As String, Optional truncate As Boolean = False) As String
        Dim value As String = ""
        If s_sensorMessages.TryGetValue(key, value) Then
            If Not truncate Then
                Return value
            End If
            Dim line1 As String = value.Split(separator:=SentenceSeparator)(0)
            Return If(value.Contains(value:="..."),
                      $"{line1}...",
                      line1)
        Else
            If Debugger.IsAttached Then
                Stop
                Dim stackFrame As New StackFrame(skipFrames:=0, needFileInfo:=True)
                MsgBox(
                    heading:=$"{PatientData.SensorState} is unknown sensor message",
                    text:="",
                    buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                    title:=GetTitleFromStack(stackFrame))
            End If
            Return value.ToTitle
        End If

    End Function

End Class
