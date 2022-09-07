' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class TimeChangeRecord

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
                    Me.currentOADate = New OADate(Me.dateTime)
                Case NameOf(relativeOffset)
                    Me.relativeOffset = CInt(kvp.Value)
                Case NameOf(previousDateTime)
                    Me.previousDateTime = kvp.Value.ParseDate(NameOf(previousDateTime))
                    Me.previousDateTimeAsString = kvp.Value
                    Me.previousOADate = New OADate(Me.previousDateTime)
                Case Else
                    Stop
            End Select
        Next
        Me.deltaOADate = Me.previousDateTime - Me.dateTime
    End Sub

#If True Then ' Prevent reordering

    Public Property type As String
    Public Property index As String
    Public Property kind As String
    Public Property version As Integer
    Public Property [dateTime] As Date
    Public Property dateTimeAsString As String
    Public Property currentOADate As OADate
    Public Property relativeOffset As Integer
    Public Property previousDateTime As Date
    Public Property previousDateTimeAsString As String
    Public Property previousOADate As OADate
    Public Property deltaOADate As TimeSpan
#End If  ' Prevent reordering
End Class
