' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PresetBolusRecord

    Public Sub New()
    End Sub

    Public Sub New(row As StringTable.Row, key As String)
        If row.Columns.Count <> 3 OrElse row.Columns(index:=0).Length = 0 Then
            Exit Sub
        End If
        Dim column0Trim As String = row.Columns(index:=0).Replace(oldValue:=key, newValue:="").Trim
        If column0Trim.Length = 0 Then
            If row.Columns(index:=1).Length = 0 AndAlso row.Columns(index:=1).Length = 0 Then
                Exit Sub
            End If
            If row.Columns(index:=1).Contains(value:="-"c) Then
                Me.BolusTypeNormal = True
                Me.Bolus = column0Trim
            Else
                Me.BolusTypeNormal = False
                Dim squareSplit() As String = row.Columns(index:=1).Split(separator:="-"c)
                Me.Bolus = squareSplit(0)
                Me.Duration = squareSplit(1)
            End If
        Else
            Me.BolusTypeNormal = True
            Me.Bolus = column0Trim
            Me.Square = row.Columns(index:=1)
        End If
        Me.IsValid = True

    End Sub

    Public Property Bolus As String
    Public Property BolusTypeNormal As Boolean
    Public Property Duration As String
    Public Property IsValid As Boolean = False
    Public Property Square As String

End Class
