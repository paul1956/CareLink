' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module KnownColors
    Private ReadOnly _allKnownColors As New Dictionary(Of String, KnownColor)

    Public Function GetAllKnownColors() As Dictionary(Of String, KnownColor)
        If _allKnownColors.Count = 0 Then
            Dim kcol As Color
            For Each known As KnownColor In [Enum].GetValues(GetType(KnownColor))
                If known = KnownColor.Transparent Then Continue For
                kcol = Color.FromKnownColor(known)
                If Not kcol.IsSystemColor Then
                    _allKnownColors.Add(kcol.Name, known)
                End If
            Next
        End If
        Return _allKnownColors
    End Function

    Public Function GetIndexOfKnownColor(value As KnownColor) As Integer
        If _allKnownColors.Count = 0 Then Return -1
        Return _allKnownColors.IndexOfValue(value)
    End Function

    Public Function GetKnownColorFromName(Name As String) As KnownColor
        Dim known As KnownColor = Nothing
        If _allKnownColors.TryGetValue(Name, known) Then
            Return known
        End If
        Stop
        Return KnownColor.Red
    End Function

    Public Function GetKnownColorsBindingSource() As BindingSource
        Return New BindingSource(_allKnownColors, Nothing)
    End Function

End Module
