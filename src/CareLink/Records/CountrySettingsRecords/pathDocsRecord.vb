' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PathDocsRecord
    Public pathDoc As New Dictionary(Of String, String)

    Public Sub New(jsonData As String)
        If Not String.IsNullOrWhiteSpace(jsonData) Then
            pathDoc = Loads(jsonData)
        End If
    End Sub

End Class
