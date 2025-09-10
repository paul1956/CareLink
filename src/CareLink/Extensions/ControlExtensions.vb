' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module ControlExtensions

    ''' <summary>
    '''  Centers a <see cref="Label"/> parent container.
    ''' </summary>
    ''' <param name="lbl">The <see cref="Label"/> to be centered.</param>
    <Extension>
    Friend Sub CenterLabelXOnParent(ByRef lbl As Label)
        Dim controlWidth As Integer = lbl.Width
        If lbl.AutoSize Then
            ' If the control is a Label with AutoSize, adjust the width to fit the text
            controlWidth = lbl.PreferredWidth
        End If
        ' Center in the middle of the parent control
        lbl.Left = (lbl.Parent.Width - controlWidth) \ 2
    End Sub

    ''' <summary>
    '''  Centers a <see cref="Label"/> on the left or right half or
    '''  center of its parent container.
    ''' </summary>
    ''' <param name="ctrl">The <see cref="Label"/> to be centered.</param>
    ''' <param name="onLeftHalf">
    '''  If <see langword="True"/>, center on the left half;
    '''  if <see langword="False"/>, center on the right half;
    '''  if <see langword="Nothing"/>, center in the middle of the parent control.
    ''' </param>
    <Extension>
    Friend Sub CenterXOnParent(
        ByRef ctrl As Control,
        Optional onLeftHalf As Boolean? = Nothing)

        Dim controlWidth As Integer
        Dim parent As Control = ctrl.Parent
        If parent Is Nothing Then
            If Not Debugger.IsAttached Then
                Exit Sub
            End If
            Const message As String = "The control must have a parent to center it."
            Throw New InvalidOperationException(message)
        End If

        If TypeOf ctrl Is Label AndAlso ctrl.AutoSize Then
            Dim lbl As Label = DirectCast(ctrl, Label)
            ' If the control is a Label with AutoSize, adjust the width to fit the text
            If lbl.PreferredWidth > 0 Then
                ' Ensure PreferredWidth is valid
                controlWidth = lbl.PreferredWidth
            Else
                ' Fallback to Width if PreferredWidth is not set
                controlWidth = ctrl.Width
            End If
        Else
            controlWidth = ctrl.Width
        End If

        If onLeftHalf.HasValue Then
            Dim halfWidth As Integer = parent.Width \ 2
            If onLeftHalf.Value Then
                ' Center on the left half
                ctrl.Left = (halfWidth - controlWidth) \ 2
            Else
                ' Center on the right half
                ctrl.Left = halfWidth + ((halfWidth - controlWidth) \ 2)
            End If
        Else
            ' Center in the middle of the parent control
            ctrl.Left = (parent.Width - controlWidth) \ 2
        End If
    End Sub

    ''' <summary>
    '''  Centers a <see cref="Label"/> on its parent container.
    ''' </summary>
    ''' <param name="ctrl">The <see cref="Label"/> to be centered.</param>
    ''' <param name="verticalOffset">Vertical offset to apply when centering.</param>
    <Extension>
    Friend Sub CenterXYOnParent(ByRef ctrl As Label, verticalOffset As Integer)
        Dim parent As Control = ctrl.Parent
        If parent Is Nothing Then
            If Not Debugger.IsAttached Then
                Exit Sub
            End If
            Dim message As String = "The control must have a parent to center it."
            Throw New InvalidOperationException(message)
        End If
        Dim ctrlWidth As Integer = ctrl.Width
        Dim ctrlHeight As Integer = ctrl.Height
        If ctrl.AutoSize Then
            ' If the control is a Label with AutoSize, adjust the width to fit the text
            ctrlWidth = ctrl.PreferredWidth
            ctrlHeight = ctrl.PreferredHeight
        End If

        ' Center in the middle of the parent control
        ctrl.Left = (parent.Width - ctrlWidth) \ 2
        Dim totalHeight As Integer = ctrlHeight + ctrl.Margin.Top + ctrl.Margin.Bottom
        ctrl.Top = ((parent.Height - totalHeight) \ 2) + verticalOffset
    End Sub

    ''' <summary>
    '''  Centers a <see cref="Label"/> on the left or right half or
    '''  center of its parent container.
    ''' </summary>
    ''' <param name="controls">The collection of controls to search.</param>
    ''' <param name="controlName">The name of the control to find.</param>
    ''' <returns>
    '''  The control with the specified name, or <see langword="Nothing"/> if not found.
    ''' </returns>
    <Extension>
    Friend Function FindControlByName(
        controls As Control.ControlCollection,
        controlName As String) As Control

        For Each ctrl As Control In controls
            If ctrl.Name = controlName Then
                Return ctrl
            End If
        Next
        Return Nothing
    End Function

    ''' <summary>
    '''  Calculates the horizontal midpoint of a control, relative to its parent.
    ''' </summary>
    ''' <param name="ctrl">The control for which to find the horizontal midpoint.</param>
    ''' <returns>
    '''  <see langword="Integer"/> representing the X coordinate of the midpoint,
    '''  relative to the parent control.
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
    '''  <see langword="Integer"/> representing the Y coordinate of the midpoint,
    '''  relative to the parent control.
    ''' </returns>
    <Extension>
    Friend Function FindVerticalMidpoint(ctrl As Control) As Integer
        Return ctrl.Top + (ctrl.Height \ 2)
    End Function

    ''' <summary>
    '''  Sets the <see cref="DataGridView.EnableHeadersVisualStyles"/> property to
    '''  <see langword="False"/> for all <see cref="DataGridView"/> controls
    '''  within the specified control.
    '''  This is used to ensure consistent header styles across all DataGridViews.
    ''' </summary>
    ''' <param name="ctrl">The parent control containing the DataGridViews.</param>
    <Extension>
    Friend Sub SetDgvCustomHeadersVisualStyles(ctrl As Control)
        For Each c As Control In ctrl.Controls
            If TypeOf c Is DataGridView Then
                Dim dgv As DataGridView = CType(c, DataGridView)
                dgv.EnableHeadersVisualStyles = False
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.Black
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            End If
            ' Recursively search child controls
            If c.HasChildren Then
                SetDgvCustomHeadersVisualStyles(ctrl:=c)
            End If
        Next
    End Sub

End Module
