' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class CurrentUserRecord

    Sub New(userName As String)
        Me.UserName = userName
        Me.UseAdvancedAitDecay = CheckState.Indeterminate
    End Sub

    Public Property UserName As String

    Public Property Ait As TimeSpan?

    Public Property InsulinTypeName As String

    Public Property InsulinRealAit As TimeSpan

    Public Property UseAdvancedAitDecay As CheckState

    Public Property CarbRatios As New List(Of CarbRatioRecord)

End Class
