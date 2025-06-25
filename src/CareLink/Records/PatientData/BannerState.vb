' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Text.Json.Serialization

Public Class BannerState

    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer = Nothing

    <DisplayName("Type")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    <JsonPropertyName("type")>
    Public Property Type As String

    <DisplayName("Time Remaining")>
    <Column(Order:=3, TypeName:=NameOf([Int32]))>
    <JsonPropertyName("timeRemaining")>
    Public Property TimeRemaining As Integer = Nothing

    <DisplayName("Message")>
    <Column(Order:=4, TypeName:=NameOf([String]))>
    <JsonPropertyName("message")>
    Public ReadOnly Property Message As String
        Get
            Dim formattedMessage As String = Nothing
            If s_sensorMessages.TryGetValue(Me.Type, formattedMessage) Then
                Return formattedMessage
            Else
                Stop
            End If

            Return Me.Type.ToTitle
        End Get
    End Property

End Class
