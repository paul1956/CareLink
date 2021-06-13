' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class SgRecord
    ' ReSharper disable InconsistentNaming
    Public Property RecordNumber As Integer
    Public Property sg As Single
    Public Property [datetime] As Date
    Public Property timeChange As Boolean
    Public Property sensorState As String
    Public Property kind As String
    Public Property version As Integer
    Public Property relativeOffset As Integer
    ' ReSharper restore InconsistentNaming

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
            Me.timeChange = Boolean.Parse(value)
        End If

        If dic.TryGetValue(NameOf(sensorState), Me.sensorState) Then
        End If

        If dic.TryGetValue(NameOf(kind), Me.kind) Then
        End If
        If dic.TryGetValue(NameOf(version), value) Then
            Me.relativeOffset = Integer.Parse(value)
        End If

        If dic.TryGetValue(NameOf(relativeOffset), value) Then
            Me.relativeOffset = Integer.Parse(value)
        End If

    End Sub

End Class
