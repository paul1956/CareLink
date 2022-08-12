' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Class InsulinRecord

    Private Shared ReadOnly columnsToHide As New List(Of String) From {
            NameOf(InsulinRecord.id),
            NameOf(InsulinRecord.index),
            NameOf(InsulinRecord.kind),
            NameOf(InsulinRecord.OADate),
            NameOf(InsulinRecord.relativeOffset),
            NameOf(InsulinRecord.type),
            NameOf(InsulinRecord.version)
        }

    Public Sub New(oneRow As Dictionary(Of String, String), recordNumber As Integer)
        Dim dic As Dictionary(Of String, String) = oneRow
        For Each kvp As KeyValuePair(Of String, String) In oneRow

            Select Case kvp.Key
                Case NameOf(Me.type)
                    Me.type = kvp.Value

                Case NameOf(Me.index)
                    Me.index = Integer.Parse(kvp.Value)

                Case NameOf(Me.kind)
                    Me.kind = kvp.Value

                Case NameOf(Me.version)
                    Me.version = Integer.Parse(kvp.Value)

                Case NameOf(Me.dateTime)
                    Dim value As String = ""
                    If dic.TryGetValue(NameOf(Me.dateTime), value) Then
                        Me.dateTime = value.ParseDate(NameOf(Me.dateTime))
                    End If
                    Me.dateTimeAsString = value
                    Me.OADate = _dateTime.ToOADate
                Case NameOf(Me.relativeOffset)
                    Me.relativeOffset = Integer.Parse(kvp.Value)

                Case NameOf(Me.programmedExtendedAmount)
                    Me.programmedExtendedAmount = Single.Parse(kvp.Value)

                Case NameOf(Me.activationType)
                    Me.activationType = kvp.Value

                Case NameOf(Me.deliveredExtendedAmount)
                    Me.deliveredExtendedAmount = Single.Parse(kvp.Value)

                Case NameOf(Me.programmedFastAmount)
                    Me.programmedFastAmount = Single.Parse(kvp.Value)

                Case NameOf(Me.programmedDuration)
                    Me.programmedDuration = Integer.Parse(kvp.Value)

                Case NameOf(Me.deliveredFastAmount)
                    Me.deliveredFastAmount = Single.Parse(kvp.Value)

                Case NameOf(Me.id)
                    Me.id = Integer.Parse(kvp.Value)

                Case NameOf(Me.effectiveDuration)
                    Me.effectiveDuration = Integer.Parse(kvp.Value)

                Case NameOf(Me.completed)
                    Me.completed = Boolean.Parse(kvp.Value)

                Case NameOf(Me.bolusType)
                    Me.bolusType = kvp.Value

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
    Public Property programmedExtendedAmount As Single
    Public Property activationType As String
    Public Property deliveredExtendedAmount As Single
    Public Property programmedFastAmount As Single
    Public Property programmedDuration As Integer
    Public Property deliveredFastAmount As Single
    Public Property id As Integer
    Public Property effectiveDuration As Integer
    Public Property completed As Boolean
    Public Property bolusType As String
    Public Property OADate As Double

    Friend Shared Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso columnsToHide.Contains(columnName)
    End Function

    Public Shared Function GetCellStyle(memberName As String, <CallerMemberName> Optional functionName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case memberName
            Case NameOf([dateTime]),
                 NameOf(dateTimeAsString),
                 NameOf(type),
                 NameOf(activationType)
                cellStyle = cellStyle.CellStyleMiddleLeft
            Case NameOf(RecordNumber),
                 NameOf(kind),
                 NameOf(index),
                 NameOf(id),
                 NameOf(completed),
                 NameOf(bolusType)
                cellStyle = cellStyle.CellStyleMiddleCenter
            Case NameOf(version),
                 NameOf(programmedExtendedAmount),
                 NameOf(relativeOffset),
                 NameOf(deliveredExtendedAmount),
                 NameOf(programmedFastAmount),
                 NameOf(programmedDuration),
                 NameOf(deliveredFastAmount),
                 NameOf(effectiveDuration)
                cellStyle = cellStyle.CellStyleMiddleRight(0)
            Case Else
                Stop
                Throw New Exception($"Line {sourceLineNumber} in {functionName} thought to be unreachable for '{memberName}'")
        End Select
        Return cellStyle
    End Function

End Class
