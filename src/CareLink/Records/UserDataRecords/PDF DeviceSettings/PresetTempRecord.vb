' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PresetTempRecord

    Public Sub New()
    End Sub

    Public Sub New(r As StringTable.Row, key As String)
        If r.Columns.Count <> 3 OrElse r.Columns(0).Length = 0 Then
            Exit Sub
        End If
        Dim column0Trim As String = r.Columns(0).Replace(key, "").Trim
        If column0Trim.Length = 0 Then
            If r.Columns(1).Length = 0 AndAlso r.Columns(1).Length = 0 Then
                Exit Sub
            End If
            Me.Type = New PresetTypeRecord(r.Columns(1))
            Me.Duration = TimeSpan.Parse(r.Columns(2))
            Me.IsValid = True
        Else
            Me.Type = New PresetTypeRecord(column0Trim)
            Me.Duration = TimeSpan.Parse(r.Columns(1))
            Me.IsValid = True
        End If

        Stop
    End Sub

    Public Property IsValid As Boolean = False
    Public Property Duration As TimeSpan
    Public Property Type As PresetTypeRecord

End Class
