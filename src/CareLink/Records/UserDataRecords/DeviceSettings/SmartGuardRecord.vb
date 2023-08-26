' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class SmartGuardRecord

    Public Sub New(lines As List(Of String))
        Me.SmartGuard = lines.GetSingleLineValue(Of String)("SmartGuard ")
        Me.Target = lines.GetSingleLineValue(Of Single)("Target ")
        Me.AutoCorrection = lines.GetSingleLineValue(Of String)("Auto Correction ")
    End Sub

    Public Property SmartGuard As String
    Public Property Target As Single
    Public Property AutoCorrection As String

End Class
