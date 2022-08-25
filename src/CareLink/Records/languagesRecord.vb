' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class languagesRecord
    Public languages As New List(Of LanguageRecord)

    Public Sub New(jsonList As String)
        Dim l As List(Of Dictionary(Of String, String)) = LoadList(jsonList)
        For Each dic As Dictionary(Of String, String) In l
            languages.Add(New LanguageRecord(dic))
        Next
    End Sub
End Class
