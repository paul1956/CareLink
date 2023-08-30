' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class BloodGlucoseTargetRecord

    Public Sub New(r As StringTable.Row)
        Dim s() As String = r.Columns(0).Split(" ")
        If s.Length <> 4 Then
            If r.Columns(0) = "" Then
                Exit Sub
            End If
            Stop
            Exit Sub
        End If
        If TimeOnly.TryParse(s(0), Me.Time) Then
            Me.Low = ParseSingle(s(3))
            Me.High = ParseSingle(r.Columns(1))
            Me.IsValid = True
        Else
            Stop
        End If
    End Sub

    Private Shared ReadOnly Property ColumnTitles As New List(Of String) From {
                    {NameOf(Time)},
                    {NameOf(Low)},
                    {NameOf(High)}
                }

    Public Property IsValid As Boolean = False
    Public Property [Time] As TimeOnly
    Public Property Low As Single
    Public Property High As Single

End Class
