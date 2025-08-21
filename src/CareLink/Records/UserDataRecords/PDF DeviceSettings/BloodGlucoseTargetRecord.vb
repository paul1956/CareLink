' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class BloodGlucoseTargetRecord

    Public Sub New(r As StringTable.Row)
        Const options As StringSplitOptions = StringSplitOptions.RemoveEmptyEntries
        Dim s() As String = r.Columns(index:=0).Split(separator:=" ", options)
        If s.Length = 2 Then
            If TimeOnly.TryParse(s:=s(0), result:=Me.Time) Then
                Me.Low = ParseSingle(s:=s(1))
                Me.High = ParseSingle(s:=r.Columns(index:=1))
                Me.IsValid = True
            Else
                Stop
            End If
        End If
    End Sub

    Public Property [Time] As New TimeOnly
    Public Property High As Single = 180
    Public Property IsValid As Boolean = False
    Public Property Low As Single = 70
End Class
