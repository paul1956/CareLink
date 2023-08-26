' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class BloodGlucoseTargetRecord

    Public Sub New(line As String)
        If String.IsNullOrWhiteSpace(line) Then
            Exit Sub
        End If
        Dim lineSplit As String() = line.Split(" ")
        If lineSplit.Length = 3 Then
            Me.Time = TimeOnly.Parse(lineSplit(0))
            Me.Low = ParseSingle(lineSplit(1))
            Me.High = ParseSingle(lineSplit(2))
            Me.IsValid = True
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

    Friend Shared Function GetColumnTitle() As String
        Return ColumnTitles.ToArray.JoinLines(" ")
    End Function

End Class
