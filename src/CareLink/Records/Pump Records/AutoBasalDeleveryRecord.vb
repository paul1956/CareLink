' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class AutoBasalDeliveryRecord

    Public Sub New(marker As Dictionary(Of String, String), recordNumber As Integer)
        For Each kvp As KeyValuePair(Of String, String) In marker

            Select Case kvp.Key
                Case NameOf(type)
                    Me.type = kvp.Value

                Case NameOf(index)
                    Me.index = Integer.Parse(kvp.Value)

                Case NameOf(kind)
                    Me.kind = kvp.Value

                Case NameOf(version)
                    Me.version = Integer.Parse(kvp.Value)

                Case NameOf([dateTime])
                    Dim value As String = ""
                    If marker.TryGetValue(NameOf([dateTime]), value) Then
                        Me.dateTime = value.ParseDate(NameOf([dateTime]))
                    End If
                    Me.dateTimeAsString = value
                    Me.OADate = _dateTime.ToOADate

                Case NameOf(relativeOffset)
                    Me.relativeOffset = Integer.Parse(kvp.Value)

                Case NameOf(id)
                    Me.id = Integer.Parse(kvp.Value)

                Case NameOf(bolusAmount)
                    Me.bolusAmount = kvp.Value.ParseSingle

                Case Else
                    Stop
            End Select
        Next
        Me.RecordNumber = recordNumber
    End Sub

#If True Then ' Prevent reordering

    Public Property RecordNumber As Integer
    Public Property type As String
    Public Property index As Integer
    Public Property kind As String
    Public Property version As Integer
    Public Property [dateTime] As Date
    Public Property dateTimeAsString As String
    Public Property OADate As Double
    Public Property relativeOffset As Integer
    Public Property id As Integer
    Public Property bolusAmount As Single
#End If  ' Prevent reordering
End Class
