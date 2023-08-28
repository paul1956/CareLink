' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PresetBolusRecord

    Public Sub New()
    End Sub

    Public Sub New(line As String)
        If String.IsNullOrWhiteSpace(line) Then
            Exit Sub
        End If
        Stop
        ' TBD
        ' Me.Normal = Normal
        ' Me.Square = Square
        Me.IsValid = True
    End Sub

    Public Property IsValid As Boolean = False
    Public Property Bolus As Single
    Public Property Duration As TimeSpan
    Public Property BolusType As BolusTypeRecord

End Class
