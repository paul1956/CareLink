' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Class SgRecord
    Private Shared ReadOnly s_columnsToHide As New List(Of String) From {
                        NameOf(SgRecord.OADate),
                        NameOf(SgRecord.kind),
                        NameOf(SgRecord.relativeOffset),
                        NameOf(SgRecord.version)
                    }

    Public Sub New(allSgs As List(Of Dictionary(Of String, String)), index As Integer, ByRef lastValidTime As Date)
        Dim dic As Dictionary(Of String, String) = allSgs(index)
        Me.RecordNumber = index + 1
        For Each kvp As KeyValuePair(Of String, String) In allSgs(index)
            Select Case kvp.Key
                Case NameOf(sg)
                    Me.sg = kvp.Value.ParseSingle
                Case NameOf(datetime)
                    ' Handled below
                Case NameOf(timeChange)
                    Me.timeChange = Boolean.Parse(kvp.Value).ToString()
                Case NameOf(sensorState)
                    Me.sensorState = kvp.Value
                Case NameOf(kind)
                    Me.kind = kvp.Value
                Case NameOf(version)
                    Me.version = CInt(kvp.Value)
                Case NameOf(relativeOffset)
                    Me.relativeOffset = CInt(kvp.Value)
                Case Else
                    Stop
            End Select
        Next
        Dim value As String = ""
        If dic.TryGetValue(NameOf(datetime), value) Then
            Me.datetime = allSgs.SafeGetSgDateTime(index)
            lastValidTime = Me.datetime + s_fiveMinuteSpan
        Else
            Me.datetime = lastValidTime
            lastValidTime += s_fiveMinuteSpan
        End If
        Me.dateTimeAsString = value
        Me.OADate = _datetime.ToOADate

    End Sub

    Public Property RecordNumber As Integer
    Public Property [datetime] As Date
    Public Property dateTimeAsString As String
    Public Property sg As Single
    Public Property kind As String
    Public Property OADate As Double
    Public Property relativeOffset As Integer
    Public Property sensorState As String
    Public Property timeChange As String
    Public Property version As Integer
    Friend Shared Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(columnName)
    End Function

    Public Shared Function GetCellStyle(columnName As String, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf(sg),
                 NameOf(relativeOffset)
                cellStyle = cellStyle.CellStyleMiddleRight(10)
            Case NameOf(RecordNumber),
                 NameOf(version)
                cellStyle = cellStyle.CellStyleMiddleCenter()
            Case NameOf(datetime),
                 NameOf(dateTimeAsString),
                 NameOf(sensorState)
                cellStyle = cellStyle.CellStyleMiddleLeft
            Case NameOf(timeChange),
                 NameOf(kind)
                cellStyle = cellStyle.CellStyleMiddleCenter
            Case Else
                Throw New Exception($"Line {sourceLineNumber} in {memberName} thought to be unreachable for column '{columnName}'")
        End Select
        Return cellStyle
    End Function

End Class
