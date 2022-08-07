' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.Runtime.CompilerServices

Public Class SgRecord

    Public Sub New(allSgs As List(Of Dictionary(Of String, String)), index As Integer, ByRef lastValidTime As Date)
        Dim dic As Dictionary(Of String, String) = allSgs(index)
        Me.RecordNumber = index + 1
        For Each kvp As KeyValuePair(Of String, String) In allSgs(index)
            Select Case kvp.Key
                Case NameOf(sg)
                    Single.TryParse(kvp.Value, NumberStyles.Number, CurrentDataCulture, Me.sg)
                Case NameOf(datetime)
                    ' Handled below
                Case NameOf(timeChange)
                    Me.timeChange = Boolean.Parse(kvp.Value).ToString()
                Case NameOf(sensorState)
                    Me.sensorState = kvp.Value
                Case NameOf(kind)
                    Me.kind = kvp.Value
                Case NameOf(version)
                    Me.version = Integer.Parse(kvp.Value)
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

    Public Shared Function GetCellStyle(memberName As String, <CallerMemberName> Optional functionName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case memberName
            Case NameOf(sg)
                cellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                cellStyle.Padding = New Padding(10, 0, 0, 0)
            Case NameOf(RecordNumber), NameOf(version), NameOf(relativeOffset)
                cellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                cellStyle.Padding = New Padding(0, 0, 0, 0)
            Case NameOf(datetime), NameOf(dateTimeAsString), NameOf(sensorState)
                cellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                cellStyle.Padding = New Padding(0, 0, 3, 0)
            Case NameOf(timeChange), NameOf(kind)
                cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                cellStyle.Padding = New Padding(0, 0, 0, 0)
            Case Else
                Throw New Exception($"Line {sourceLineNumber} in {functionName} thought to be unreachable for '{memberName}'")
        End Select
        Return cellStyle
    End Function

End Class
