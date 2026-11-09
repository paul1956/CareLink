' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

''' <summary>
'''  Formats counts with regular English singular or plural unit names.
''' </summary>
Public Module UnitFormattingExtensions

    ''' <summary>
    '''  Formats a count and unit, using the singular unit only for a count of one.
    ''' </summary>
    ''' <param name="totalUnits">The count to format.</param>
    ''' <param name="unit">
    '''  The singular unit name, including any desired capitalization.
    ''' </param>
    ''' <param name="prefix">Text to prepend to the result.</param>
    ''' <param name="suffix">Text to append to the result.</param>
    ''' <param name="includeValue">Whether to include the count.</param>
    ''' <param name="separator">
    '''  Text between the count and unit when the count is included.
    ''' </param>
    ''' <param name="numberFormat">
    '''  The count's numeric format; use "N0" to include thousands separators.
    ''' </param>
    <Extension>
    Public Function ToUnits(totalUnits As Long,
                            unit As String,
                            Optional prefix As String = "",
                            Optional suffix As String = "",
                            Optional includeValue As Boolean = True,
                            Optional separator As String = " ",
                            Optional numberFormat As String = "") As String
        Dim unitOnly As String = If(totalUnits = 1, unit, $"{unit}s")
        Dim value As String = If(includeValue, $"{totalUnits.ToString(format:=numberFormat)}{separator}", "")
        Return $"{prefix}{value}{unitOnly}{suffix}"
    End Function

    ''' <inheritdoc cref="ToUnits(Long, String, String, String, Boolean, String, String)"/>
    <Extension>
    Public Function ToUnits(totalUnits As Integer,
                            unit As String,
                            Optional prefix As String = "",
                            Optional suffix As String = "",
                            Optional includeValue As Boolean = True,
                            Optional separator As String = " ",
                            Optional numberFormat As String = "") As String
        Return ToUnits(totalUnits:=CLng(totalUnits),
                       unit,
                       prefix,
                       suffix,
                       includeValue,
                       separator,
                       numberFormat)
    End Function

    ''' <inheritdoc cref="ToUnits(Long, String, String, String, Boolean, String, String)"/>
    <Extension>
    Public Function ToUnits(totalUnits As UInteger,
                            unit As String,
                            Optional prefix As String = "",
                            Optional suffix As String = "",
                            Optional includeValue As Boolean = True,
                            Optional separator As String = " ",
                            Optional numberFormat As String = "") As String
        Return ToUnits(totalUnits:=CLng(totalUnits),
                       unit,
                       prefix,
                       suffix,
                       includeValue,
                       separator,
                       numberFormat)
    End Function

End Module
