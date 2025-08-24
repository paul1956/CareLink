' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

''' <summary>
'''  Provides helper methods for working with known colors in the application.
''' </summary>
Friend Module KnownColors

    ''' <summary>
    '''  Stores all known colors in a sorted dictionary, keyed by color name.
    ''' </summary>
    Private ReadOnly s_allKnownColors As New SortedDictionary(Of String, KnownColor)

    ''' <summary>
    '''  Gets a sorted dictionary of all known colors,
    '''  excluding system and transparent colors.
    ''' </summary>
    ''' <returns>
    '''  A <see cref="SortedDictionary(Of String, KnownColor)"/> containing
    '''  all known colors.
    ''' </returns>
    Public Function GetAllKnownColors() As SortedDictionary(Of String, KnownColor)
        If s_allKnownColors.Count = 0 Then
            Dim kColor As Color
            For Each value As KnownColor In [Enum].GetValues(Of KnownColor)
                If value = KnownColor.Transparent Then Continue For
                kColor = Color.FromKnownColor(color:=value)
                If kColor.IsSystemColor OrElse s_allKnownColors.ContainsValue(value) Then
                    Continue For
                End If
                s_allKnownColors.Add(key:=kColor.Name, value)
            Next
        End If
        Return s_allKnownColors
    End Function

    ''' <summary>
    '''  Gets the index of the specified known color in the sorted dictionary.
    ''' </summary>
    ''' <param name="item">The <see cref="KnownColor"/> to find.</param>
    ''' <returns>
    '''  The zero-based index of the known color, or -1 if not found.
    ''' </returns>
    Public Function GetIndexOfKnownColor(item As KnownColor) As Integer
        Return GetAllKnownColors.IndexOfValue(item)
    End Function

    ''' <summary>
    '''  Gets the <see cref="KnownColor"/> value from its name.
    ''' </summary>
    ''' <param name="key">The name of the known color.</param>
    ''' <returns>
    '''  The corresponding <see cref="KnownColor"/> if found;
    '''  otherwise, <see cref="KnownColor.Red"/>.
    ''' </returns>
    Public Function GetKnownColorFromName(key As String) As KnownColor
        Dim value As KnownColor = Nothing
        If GetAllKnownColors.TryGetValue(key, value) Then
            Return value
        End If
        Stop
        Return KnownColor.Red
    End Function

    ''' <summary>
    '''  Gets the name of the specified <see cref="KnownColor"/>.
    ''' </summary>
    ''' <param name="item">The known color.</param>
    ''' <returns>
    '''  The name of the known color, or "Unknown" if not found.
    ''' </returns>
    Public Function GetNameFromKnownColor(item As KnownColor) As String
        Dim index As Integer = GetAllKnownColors.IndexOfValue(item)
        If index = -1 Then
            Return "Unknown"
        End If
        Stop
        Return GetAllKnownColors.Keys(index)
    End Function

    ''' <summary>
    '''  Converts a <see cref="KnownColor"/> to a <see cref="Color"/> instance.
    ''' </summary>
    ''' <param name="c">The known color to convert.</param>
    ''' <returns>
    '''  A <see cref="Color"/> instance representing the known color.
    ''' </returns>
    <Extension>
    Public Function ToColor(c As KnownColor) As Color
        Return Color.FromKnownColor(c)
    End Function

End Module
