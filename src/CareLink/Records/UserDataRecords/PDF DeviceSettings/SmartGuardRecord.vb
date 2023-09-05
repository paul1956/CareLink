' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class SmartGuardRecord

    Public Sub New()
    End Sub

    Public Sub New(sTable As StringTable, smartGuard As String)
        Me.SmartGuard = smartGuard
        Me.Target = sTable.GetSingleLineValue(Of Single)("Target ")
        Me.AutoCorrection = sTable.GetSingleLineValue(Of String)("Auto Correction ")
    End Sub

    Public Property SmartGuard As String = "Off"
    Public Property Target As Single = 120
    Public Property AutoCorrection As String = "Off"

End Class
