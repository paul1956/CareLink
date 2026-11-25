' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports DataGridViewColumnControls

''' <summary>
'''  A DataGridView cell that behaves like a button, but can be disabled.
''' </summary>
Friend Class ScheduleButtonCell
    Inherits DataGridViewDisableButtonCell

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs, rowIndex As Integer)
        If Me.Enabled Then MyBase.OnKeyDown(e, rowIndex)
    End Sub

    Protected Overrides Sub OnKeyUp(e As KeyEventArgs, rowIndex As Integer)
        If Me.Enabled Then MyBase.OnKeyUp(e, rowIndex)
    End Sub

    Protected Overrides Sub OnMouseDown(e As DataGridViewCellMouseEventArgs)
        If Me.Enabled Then MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseMove(e As DataGridViewCellMouseEventArgs)
        If Me.Enabled Then MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnMouseUp(e As DataGridViewCellMouseEventArgs)
        If Me.Enabled Then MyBase.OnMouseUp(e)
    End Sub

End Class
