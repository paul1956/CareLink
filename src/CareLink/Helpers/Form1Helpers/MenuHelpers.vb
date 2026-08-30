' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module MenuHelpers

    ''' <summary>
    '''  Hides separators that are at the start, end, or next to another separator,
    '''  or between only hidden items.
    ''' </summary>
    <Extension>
    Friend Sub UpdateSeparators(items As ToolStripItemCollection)
        Dim prevVisibleIsSeparator As Boolean = True ' Start as True to hide leading separators

        For Each item As ToolStripItem In items
            If TypeOf item Is ToolStripSeparator Then
                ' Hide if previous visible item was a separator or none
                item.Visible = Not prevVisibleIsSeparator
                prevVisibleIsSeparator = True
            Else
                ' For normal menu items
                If item.Available Then
                    prevVisibleIsSeparator = False
                End If
            End If
        Next

        ' Hide trailing separator if last visible was a separator
        If items.Count > 0 AndAlso TypeOf items(index:=items.Count - 1) Is ToolStripSeparator Then
            items(index:=items.Count - 1).Visible = False
        End If
    End Sub

End Module
