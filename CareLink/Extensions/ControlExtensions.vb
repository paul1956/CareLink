' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module ControlExtensions
    <Extension>
    Friend Function HorizontalCenterOn(parentControl As Control, ParamArray childControls() As Control) As Integer
        Dim totalWidth As Integer = childControls.First.Right - childControls.Last.Left
        Return parentControl.FindHorizontalMidpoint - (totalWidth \ 2)
    End Function

    <Extension>
    Friend Function PositionBelow(controlAbove As Control) As Integer
        Return controlAbove.Top + controlAbove.Height + 1
    End Function

    <Extension>
    Friend Function FindHorizontalMidpoint(ctrl As Control) As Integer
        Return ctrl.Left + (ctrl.Width \ 2)
    End Function

End Module
