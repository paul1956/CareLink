' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module BasalListExtensions

    <Extension>
    Private Function IsEmpty(basalList As List(Of Basal)) As Boolean
        Return basalList.Count = 0 OrElse basalList(0) = New Basal
    End Function

    <Extension>
    Friend Function ActiveBasalPattern(basalList As List(Of Basal)) As String
        Return If(basalList.IsEmpty, String.Empty, basalList(0).ActiveBasalPattern)
    End Function

    <Extension>
    Friend Function GetBasalPerHour(basalList As List(Of Basal)) As Double
        Return If(basalList.IsEmpty, Double.NaN, basalList(0).GetBasalPerHour)
    End Function

    <Extension>
    Friend Function Subtitle(basalList As List(Of Basal)) As String
        Return If(basalList.IsEmpty, String.Empty, $"- {basalList(0).ActiveBasalPattern}")
    End Function


    <Extension>
    Friend Function Value(basalList As List(Of Basal)) As List(Of Basal)
        Return If(basalList.IsEmpty, New List(Of Basal), basalList)
    End Function

End Module
