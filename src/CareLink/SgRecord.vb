' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class SgRecord
    Public Property RecordNumber As Integer
    Public Property sg As Single
    Public Property [datetime] As Date
    Public Property timeChange As String
    Public Property sensorState As String
    Public Property kind As String
    Public Property version As Integer
    Public Property relativeOffset As Integer

    Sub New(allSgs As List(Of Dictionary(Of String, String)), index As Integer)
        Dim dic As Dictionary(Of String, String) = allSgs(index)
        Me.RecordNumber = index + 1
        If dic.Count > 7 Then Stop
        Dim value As String = ""
        If dic.TryGetValue(NameOf(sg), value) Then
            Me.sg = Single.Parse(value)
        End If
        If dic.TryGetValue(NameOf(datetime), value) Then
            Me.datetime = allSgs.SafeGetSgDateTime(index)
        End If

        If dic.TryGetValue(NameOf(timeChange), value) Then
            Me.timeChange = Boolean.Parse(value).ToString()
        End If

        If dic.TryGetValue(NameOf(sensorState), Me.sensorState) Then
        End If

        If dic.TryGetValue(NameOf(kind), Me.kind) Then
        End If
        If dic.TryGetValue(NameOf(version), value) Then
            Me.version = Integer.Parse(value)
        End If

        If dic.TryGetValue(NameOf(relativeOffset), value) Then
            Me.relativeOffset = Integer.Parse(value)
        End If

    End Sub

    Public Shared Function GetCellStyle(memberName As String) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case memberName
            Case NameOf(sg)
                cellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                cellStyle.Padding = New Padding(10, 0, 0, 0)
            Case NameOf(RecordNumber), NameOf(version), NameOf(relativeOffset)
                cellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                cellStyle.Padding = New Padding(0, 0, 0, 0)
            Case NameOf(datetime), NameOf(sensorState)
                cellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                cellStyle.Padding = New Padding(0, 0, 3, 0)
            Case NameOf(timeChange), NameOf(kind)
                cellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                cellStyle.Padding = New Padding(0, 0, 0, 0)
            Case Else
                Throw New Exception("Location thought to be unreachable")
        End Select
        Return cellStyle
    End Function

End Class
