' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class TimeChangeRecord
    Private _previousDateTime As Date
    Private _dateTime As Date

    Public Sub New(timeChangeItem As Dictionary(Of String, String))
        For Each kvp As KeyValuePair(Of String, String) In timeChangeItem
            Select Case kvp.Key
                Case NameOf(type)
                    Me.type = kvp.Value
                Case NameOf(index)
                    Me.index = kvp.Value
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

#If True Then ' Prevent reordering

    Public Property type As String
    Public Property index As String
    Public Property kind As String
    Public Property version As Integer
    Public Property [dateTime] As Date
        Get
            Return _dateTime
        End Get
        Set
            _dateTime = Value
        End Set
    End Property

    Public Property dateTimeAsString As String

    Public ReadOnly Property OADateTime As OADate
        Get
            Return New OADate(_dateTime)
        End Get
    End Property

    Public Property relativeOffset As Integer

    Public Property previousDateTime As Date
        Get
            Return _previousDateTime
        End Get
        Set
            _previousDateTime = Value
        End Set
    End Property

    Public Property previousDateTimeAsString As String

    Public ReadOnly Property previousOADateTime As OADate
        Get
            Return New OADate(_previousDateTime)
        End Get
    End Property

    Public ReadOnly Property deltaOADateTime As TimeSpan
        Get
            Return _previousDateTime - _dateTime
        End Get
    End Property

#End If  ' Prevent reordering
End Class
