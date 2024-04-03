' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PresetBolusRecord

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
            If r.Columns(1).Contains("-"c) Then
                Me.BolusTypeNormal = True
                Me.Bolus = column0Trim
            Else
                Me.BolusTypeNormal = False
                Dim squareSplit() As String = r.Columns(1).Split("-")
                Me.Bolus = squareSplit(0)
                Me.Duration = squareSplit(1)
            End If
        Else
            Me.BolusTypeNormal = True
            Me.Bolus = column0Trim
            Me.Square = r.Columns(1)
        End If
        Me.IsValid = True

    End Sub

    Public Property Bolus As String
    Public Property BolusTypeNormal As Boolean
    Public Property Duration As String
    Public Property IsValid As Boolean = False
    Public Property Square As String

End Class
