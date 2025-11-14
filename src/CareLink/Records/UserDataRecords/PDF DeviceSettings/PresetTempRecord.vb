' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PresetTempRecord

    Public Sub New()
    End Sub

    Public Sub New(r As StringTable.Row, key As String)
        If r.Columns.Count <> 3 OrElse r.Columns(index:=0).Length = 0 Then
            Exit Sub
        End If
        Dim column0Trim As String = r.Columns(index:=0).Replace(oldValue:=key, newValue:="").Trim
        If column0Trim.Length = 0 Then
            If r.Columns(index:=1).Length = 0 AndAlso r.Columns(index:=1).Length = 0 Then
                Exit Sub
            End If
            Me.PresetAmount = New PresetAmountRecord(s:=r.Columns(index:=1))
            Me.Duration = TimeSpan.Parse(s:=r.Columns(index:=2))
            Me.IsValid = True
        Else
            Me.PresetAmount = New PresetAmountRecord(column0Trim)
            Me.Duration = TimeSpan.Parse(s:=r.Columns(index:=1))
            Me.IsValid = True
        End If
    End Sub

    Public Property Duration As TimeSpan
    Public Property DurationUnits As String
    Public Property IsValid As Boolean = False
    Public Property PresetAmount As PresetAmountRecord

End Class
