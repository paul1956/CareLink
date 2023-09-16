' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class InsulinSensitivityRecord

    Public Sub New(r As StringTable.Row)
        Dim cleaned As String = r.Columns(0).CleanSpaces
        If cleaned.Length = 0 Then
            Exit Sub
        End If
        Dim s() As String = cleaned.Split(" ", StringSplitOptions.RemoveEmptyEntries)
        If s.Length <> 2 Then
            Stop
            Exit Sub
        End If
        If TimeOnly.TryParse(s(0), Me.Time) Then
            Me.Sensitivity = ParseSingle(s(1))
            Me.IsValid = True
        Else
            Stop
        End If
    End Sub

    Public Property IsValid As Boolean = False
    Public Property [Time] As TimeOnly
    Public Property Sensitivity As Single

End Class
