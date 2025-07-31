' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

''' <summary>
'''  Provides extension methods for working with lists of <see cref="Basal"/> objects.
''' </summary>
Imports System.Runtime.CompilerServices

Public Module BasalListExtensions

    ''' <summary>
    '''  Determines whether the specified list of <see cref="Basal"/> objects is empty
    '''  or contains a default <see cref="Basal"/> instance.
    ''' </summary>
    ''' <param name="basalList">The list of <see cref="Basal"/> objects to check.</param>
    ''' <returns>
    '''  <see langword="True"/> if the list is empty or the first element is a default <see cref="Basal"/>;
    '''  <see cref="Basal"/> instance, which is defined as having no active basal pattern.
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Private Function IsEmpty(basalList As List(Of Basal)) As Boolean
        Return basalList.Count = 0 OrElse basalList(0) = New Basal
    End Function

    ''' <summary>
    '''  Gets the active basal pattern name from the specified list of <see cref="Basal"/> objects.
    ''' </summary>
    ''' <param name="basalList">The list of <see cref="Basal"/> objects.</param>
    ''' <returns>
    '''  The active basal pattern name if available; otherwise, an empty string.
    ''' </returns>
    <Extension>
    Friend Function ActiveBasalPattern(basalList As List(Of Basal)) As String
        Return If(basalList.IsEmpty,
                  String.Empty,
                  basalList(index:=0).ActiveBasalPattern)
    End Function

    ''' <summary>
    '''  Gets the basal rate per hour from the specified list of <see cref="Basal"/> objects.
    ''' </summary>
    ''' <param name="basalList">The list of <see cref="Basal"/> objects.</param>
    ''' <returns>
    '''  The basal rate per hour if available; otherwise, <see cref="Double.NaN"/>.
    ''' </returns>
    <Extension>
    Friend Function GetBasalPerHour(basalList As List(Of Basal)) As Double
        Return If(basalList.IsEmpty,
                  Double.NaN,
                  basalList(index:=0).GetBasalPerHour)
    End Function

    ''' <summary>
    '''  Gets a subtitle string representing the active basal pattern from the specified
    '''  list of <see cref="Basal"/> objects.
    ''' </summary>
    ''' <param name="basalList">The list of <see cref="Basal"/> objects.</param>
    ''' <returns>
    '''  A formatted subtitle string if available; otherwise, an empty string.
    ''' </returns>
    <Extension>
    Friend Function Subtitle(basalList As List(Of Basal)) As String
        Return If(basalList.IsEmpty, String.Empty, $"- {basalList(0).ActiveBasalPattern}")
    End Function

    ''' <summary>
    '''  Returns the specified list of <see cref="Basal"/> objects, or a new empty list if the original is empty.
    ''' </summary>
    ''' <param name="basalList">The list of <see cref="Basal"/> objects.</param>
    ''' <returns>
    '''  The original list if not empty; otherwise, a new empty list.
    ''' </returns>
    <Extension>
    Friend Function ClassCollection(basalList As List(Of Basal)) As List(Of Basal)
        Return If(basalList.IsEmpty, New List(Of Basal), basalList)
    End Function

End Module
