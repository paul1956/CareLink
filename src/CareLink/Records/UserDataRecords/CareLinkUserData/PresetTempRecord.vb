' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PresetTempRecord

    Public Sub New()
    End Sub

    Public Sub New(line As String)
        If String.IsNullOrWhiteSpace(line) Then
            Exit Sub
        End If
        Stop
        Me.IsValid = True
    End Sub

    Public Property IsValid As Boolean = False
    Public Property Duration As TimeSpan
    Public Property Type As PresetTypeRecord

End Class
