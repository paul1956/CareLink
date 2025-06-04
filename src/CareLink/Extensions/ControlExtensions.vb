' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module ControlExtensions

    ''' <summary>
    '''  Calculates the horizontal midpoint of a control, relative to its parent.
    ''' </summary>
    ''' <param name="ctrl">The control for which to find the horizontal midpoint.</param>
    ''' <returns>
    '''  <see langword="Integer"/> representing the X coordinate of the midpoint, relative to the parent control.
    ''' </returns>
    <Extension>
    Friend Function FindHorizontalMidpoint(ctrl As Control) As Integer
        Return ctrl.Left + (ctrl.Width \ 2)
    End Function

    ''' <summary>
    '''  Calculates the vertical midpoint of a control, relative to its parent.
    ''' </summary>
    ''' <param name="ctrl">The control for which to find the vertical midpoint.</param>
    ''' <returns>
    '''  <see langword="Integer"/> representing the Y coordinate of the midpoint, relative to the parent control.
    ''' </returns>
    <Extension>
    Friend Function FindVerticalMidpoint(ctrl As Control) As Integer
        Return ctrl.Top + (ctrl.Height \ 2)
    End Function

End Module
