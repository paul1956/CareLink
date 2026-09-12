' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class BasalRateRecord

    ' Parsing is done by BasalRateRecordExtensions.InitializeFromString
    Public Sub New()
    End Sub

    Public Property [Time] As TimeOnly
    Public Property UnitsPerHr As Single
    Public Property IsValid As Boolean = False

    Public Overrides Function ToString() As String
        Return If(Me.IsValid,
                  $"{Me.Time} {Me.UnitsPerHr}",
                  " ")
    End Function

End Class
