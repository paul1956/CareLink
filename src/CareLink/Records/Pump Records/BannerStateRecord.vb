' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class BannerStateRecord
    Private _type As String
    <DisplayName("Record Number")>
    <Column(Order:=0, TypeName:=NameOf(RecordNumber))>
    Public Property RecordNumber As Integer = Nothing

    <DisplayName("Type")>
    <Column(Order:=1, TypeName:=NameOf([String]))>
    Public Property type As String
        Get
            Return _type
        End Get
        Set
            _type = Value
        End Set
    End Property

    <DisplayName("Message")>
    <Column(Order:=2, TypeName:=NameOf([String]))>
    Public ReadOnly Property message As String
        Get
            Dim formattedMessage As String = Nothing
            If s_sensorMessages.TryGetValue(_type, formattedMessage) Then
                Return formattedMessage
            Else
                Stop
            End If

            Return _type.ToTitle
        End Get
    End Property

    <DisplayName("Time Remaining")>
    <Column(Order:=3, TypeName:=NameOf([Int32]))>
    Public Property timeRemaining As Integer = Nothing

End Class
