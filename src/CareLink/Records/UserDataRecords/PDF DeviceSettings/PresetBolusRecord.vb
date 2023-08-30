' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PresetBolusRecord

    Public Sub New()
    End Sub

    Public Sub New(r As StringTable.Row)
        If r.Columns(1) = "" Then
            Me.IsValid = True
            Exit Sub
        End If
        Stop
    End Sub

    Public Property IsValid As Boolean = False
    Public Property Bolus As Single
    Public Property Duration As TimeSpan
    Public Property BolusType As BolusTypeRecord

End Class
