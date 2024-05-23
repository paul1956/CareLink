' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module KnownColors
    Private ReadOnly s_allKnownColors As New SortedDictionary(Of String, KnownColor)

    Public Function GetAllKnownColors() As SortedDictionary(Of String, KnownColor)

        If s_allKnownColors.Count = 0 Then
            Dim kColor As Color
            For Each known As KnownColor In [Enum].GetValues(Of KnownColor)
                If known = KnownColor.Transparent Then Continue For
                kColor = Color.FromKnownColor(known)
                If kColor.IsSystemColor OrElse s_allKnownColors.ContainsValue(known) Then
                    Continue For
                End If
                s_allKnownColors.Add(kColor.Name, known)
            Next
        End If
        Return s_allKnownColors

    End Function

    Public Function GetIndexOfKnownColor(value As KnownColor) As Integer
        Return GetAllKnownColors.IndexOfValue(value)
    End Function

    Public Function GetKnownColorFromName(Name As String) As KnownColor
        Dim known As KnownColor = Nothing
        If GetAllKnownColors.TryGetValue(Name, known) Then
            Return known
        End If
        Stop
        Return KnownColor.Red
    End Function

    Public Function GetNameFromKnownColor(known As KnownColor) As String
        Dim index As Integer = GetAllKnownColors.IndexOfValue(known)
        If index = -1 Then
            Return "Unknown"
        End If
        Stop
        Return GetAllKnownColors.Keys(index)
    End Function

    <Extension>
    Public Function ToColor(c As KnownColor) As Color
        Return Color.FromKnownColor(c)
    End Function

End Module
