' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class InsulinActivationRecord
    Public Property UpCount As Integer
    Public Property AitHours As Single

    Public Sub New(upCount As Integer, aitHours As Integer)
        Me.UpCount = upCount
        Me.AitHours = aitHours
    End Sub

End Class
