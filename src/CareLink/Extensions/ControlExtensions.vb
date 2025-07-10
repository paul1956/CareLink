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

    ''' <summary>
    '''  Centers a <see cref="Label"/> on the left or right half or center of its parent container.
    ''' </summary>
    ''' <param name="ctrl">The <see cref="Label"/> to be centered.</param>
    ''' <param name="onLeftHalf">
    '''  If <see langword="True"/>, center on the left half;
    '''  if <see langword="False"/>, center on the right half;
    '''  if <see langword="Nothing"/>, center in the middle of the parent control.
    ''' </param>
    <Extension>
    Public Sub CenterLeft(ByRef ctrl As Control, Optional onLeftHalf As Boolean? = Nothing)
        Dim parentWidth As Integer = ctrl.Parent.Width
        Dim controlWidth As Integer = ctrl.Width
        If TypeOf ctrl Is Label AndAlso ctrl.AutoSize Then
            ' If the control is a Label with AutoSize, adjust the width to fit the text
            controlWidth = DirectCast(ctrl, Label).PreferredWidth
        End If
        If onLeftHalf.HasValue Then
            Dim halfWidth As Integer = parentWidth \ 2
            If onLeftHalf.Value Then
                ' Center on the left half
                ctrl.Left = (halfWidth - (controlWidth + ctrl.Margin.Left)) \ 2
            Else
                ' Center on the right half
                ctrl.Left = halfWidth + ((halfWidth - (controlWidth + ctrl.Margin.Right)) \ 2)
            End If
        Else
            ' Center in the middle of the parent control
            ctrl.Left = (parentWidth - (controlWidth + ctrl.Margin.Left + +ctrl.Margin.Right)) \ 2
        End If
    End Sub

End Module
