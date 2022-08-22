' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Class AutoBasalDeliveryRecord

    Private Shared ReadOnly s_columnsToHide As New List(Of String) From {
            NameOf(AutoBasalDeliveryRecord.id),
            NameOf(AutoBasalDeliveryRecord.index),
            NameOf(AutoBasalDeliveryRecord.kind),
            NameOf(AutoBasalDeliveryRecord.OADate),
            NameOf(AutoBasalDeliveryRecord.relativeOffset),
            NameOf(AutoBasalDeliveryRecord.type),
            NameOf(AutoBasalDeliveryRecord.version)
        }

    Public Sub New(oneRow As Dictionary(Of String, String), recordNumber As Integer)
        Dim dic As Dictionary(Of String, String) = oneRow
        For Each kvp As KeyValuePair(Of String, String) In oneRow

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
                    If dic.TryGetValue(NameOf([dateTime]), value) Then
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

    Public Property RecordNumber As Integer
    Public Property type As String
    Public Property index As Integer
    Public Property kind As String
    Public Property version As Integer
    Public Property [dateTime] As Date
    Public Property dateTimeAsString As String
    Public Property relativeOffset As Integer
    Public Property id As Integer
    Public Property bolusAmount As Single
    Public Property OADate As Double

    Friend Shared Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(columnName)
    End Function

    Public Shared Function GetCellStyle(columnName As String, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf([dateTime]),
                    NameOf(dateTimeAsString),
                    NameOf(type)
                cellStyle.CellStyleMiddleLeft
            Case NameOf(RecordNumber),
                    NameOf(kind),
                    NameOf(index),
                    NameOf(id)
                cellStyle = cellStyle.CellStyleMiddleCenter
                cellStyle.Padding = New Padding(0, 0, 0, 0)
            Case NameOf(bolusAmount),
                    NameOf(version),
                    NameOf(OADate),
                    NameOf(relativeOffset)
                cellStyle = cellStyle.CellStyleMiddleRight(0)
            Case Else
                Stop
                Throw New Exception($"Line {sourceLineNumber} in {memberName} thought to be unreachable for column '{columnName}'")
        End Select
        Return cellStyle
    End Function

End Class
