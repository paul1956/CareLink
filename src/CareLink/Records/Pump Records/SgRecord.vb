' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class SgRecord

    Public Sub New(dic As Dictionary(Of String, String))
        Dim allSgs As New List(Of Dictionary(Of String, String)) From {
            dic
        }
        Dim lastValidTime As Date = Nothing
        Me.processOneSg(allSgs, 0, lastValidTime, dic)
    End Sub

    Public Sub New(allSgs As List(Of Dictionary(Of String, String)), index As Integer, ByRef lastValidTime As Date)
        Dim dic As Dictionary(Of String, String) = allSgs(index)
        Me.RecordNumber = index + 1
        lastValidTime = Me.processOneSg(allSgs, index, lastValidTime, dic)
    End Sub

    Private Function processOneSg(allSgs As List(Of Dictionary(Of String, String)), index As Integer, lastValidTime As Date, dic As Dictionary(Of String, String)) As Date
        For Each kvp As KeyValuePair(Of String, String) In dic
            Select Case kvp.Key
                Case NameOf(sg)
                    Me.sg = kvp.Value.ParseSingle
                Case NameOf(Me.datetime)
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
        If dic.TryGetValue(NameOf(Me.datetime), value) Then
            Me.datetime = allSgs.SafeGetSgDateTime(index)
            lastValidTime = Me.datetime + s_fiveMinuteSpan
        Else
            Me.datetime = lastValidTime
            lastValidTime += s_fiveMinuteSpan
        End If
        Me.datetimeAsString = value
        Me.OADate = New OADate(_datetime)
        Return lastValidTime
    End Function

#If True Then ' Prevent reordering

    Public Property RecordNumber As Integer
    Public Property sg As Single
    Public Property [datetime] As Date
    Public Property datetimeAsString As String
    Public Property OADate As OADate
    Public Property timeChange As String
    Public Property sensorState As String
    Public Property kind As String
    Public Property version As Integer
    Public Property relativeOffset As Integer
#End If  ' Prevent reordering
End Class
