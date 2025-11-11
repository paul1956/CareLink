' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module ControlExtensions

    ''' <summary>
    '''  Centers a <see cref="Label"/> parent container.
    ''' </summary>
    ''' <param name="lbl">
    '''  The <see cref="Label"/> to be centered. May be <see langword="Nothing"/>; in that case the method
    '''  will throw a <see cref="NullReferenceException"/> when accessed.
    ''' </param>
    ''' <remarks>
    '''  This extension computes an effective width for the label and positions its <see cref="Control.Left"/>
    '''  so the label is horizontally centered relative to its immediate parent control.
    '''  If <see cref="Label.AutoSize"/> is <see langword="True"/>, <see cref="Label.PreferredWidth"/>
    '''  is used to determine the drawn width. The method does not change parent layout or anchors.
    ''' </remarks>
    <Extension>
    Friend Sub CenterLabelXOnParent(lbl As Label)
        If lbl Is Nothing OrElse lbl.IsDisposed Then
            Exit Sub
        End If
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
    ''' <param name="ctrl">
    '''  The <see cref="Control"/> to be centered. If the control is a <see cref="Label"/> with
    '''  <see cref="Label.AutoSize"/> set, the preferred size will be respected.
    ''' </param>
    ''' <param name="onLeftHalf">
    '''  If <see langword="True"/>, center on the left half;
    '''  if <see langword="False"/>, center on the right half;
    '''  if <see langword="Nothing"/>, center in the middle of the parent control.
    ''' </param>
    ''' <remarks>
    '''  - If <paramref name="ctrl"/> has no parent (for example during form disposal), the routine exits early.
    '''  - When centering on halves, the method divides the parent's width into two equal halves and
    '''    centers the control within the chosen half, preserving integer arithmetic via integer division.
    '''  - This method mutates <see cref="Control.Left"/> only; it does not modify <see cref="Control.Width"/>,
    '''    anchors, or layout settings.
    ''' </remarks>
    <Extension>
    Friend Sub CenterXOnParent(ctrl As Control, Optional onLeftHalf As Boolean? = Nothing)
        If ctrl Is Nothing OrElse ctrl.IsDisposed Then
            Exit Sub
        End If
        Dim parent As Control = ctrl.Parent
        If parent Is Nothing OrElse parent.IsDisposed Then
            Exit Sub
        End If
        Dim controlWidth As Integer
        Dim lbl As Label = TryCast(ctrl, Label)
        If lbl IsNot Nothing AndAlso ctrl.AutoSize Then
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

        ' Defensive fixes: ensure controlWidth is sane and cache parent width
        If controlWidth < 0 Then
            controlWidth = ctrl.Width
        End If
        Dim parentWidth As Integer = parent.Width

        If onLeftHalf.HasValue Then
            Dim halfWidth As Integer = parentWidth \ 2
            If onLeftHalf.Value Then
                ' Center on the left half
                ctrl.Left = Math.Max(0, (halfWidth - controlWidth) \ 2)
            Else
                ' Center on the right half
                ctrl.Left = Math.Max(0, halfWidth + ((halfWidth - controlWidth) \ 2))
            End If
        Else
            ' Center in the middle of the parent control
            ctrl.Left = Math.Max(0, (parentWidth - controlWidth) \ 2)
        End If
    End Sub

    ''' <summary>
    '''  Centers a <see cref="Label"/> on its parent container, applying an optional vertical offset.
    ''' </summary>
    ''' <param name="ctrl">
    '''  The <see cref="Label"/> to be centered. If <see langword="Nothing"/> or disposed, the method exits without action.
    ''' </param>
    ''' <param name="verticalOffset">
    '''  Vertical offset to apply when centering. Positive values move the control down; negative values move it up.
    ''' </param>
    ''' <remarks>
    '''  The method takes label margins into account when computing the vertical centering position.
    '''  When <see cref="Label.AutoSize"/> is <see langword="True"/>, preferred dimensions are used
    '''  for both width and height.
    '''  Only <see cref="Control.Left"/> and <see cref="Control.Top"/> are modified.
    '''  The routine is defensive against null or disposed parents to improve reliability when called during form teardown.
    ''' </remarks>
    <Extension>
    Friend Sub CenterXYOnParent(ctrl As Label, verticalOffset As Integer)
        If ctrl Is Nothing OrElse ctrl.IsDisposed Then
            Exit Sub
        End If
        Dim parent As Control = ctrl.Parent
        If parent Is Nothing OrElse parent.IsDisposed Then
            Exit Sub
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
    '''  Finds a child <see cref="Control"/> by name within a control collection.
    ''' </summary>
    ''' <param name="controls">
    '''  The collection of controls to search. May be empty but not <see langword="Nothing"/>.
    ''' </param>
    ''' <param name="controlName">
    '''  The name of the control to find. Comparison is ordinal and case-sensitive.
    ''' </param>
    ''' <returns>
    '''  The control with the specified name, or <see langword="Nothing"/> if not found.
    ''' </returns>
    ''' <remarks>
    '''  The routine performs a linear scan over the provided collection. If any element in the
    '''  collection is <see langword="Nothing"/> or disposed the method returns <see langword="Nothing"/>
    '''  immediately. For large collections or performance-sensitive paths consider maintaining an
    '''  index (for example, a dictionary keyed by control name) to avoid O(n) scans.
    ''' </remarks>
    <Extension>
    Friend Function FindControlByName(controls As Control.ControlCollection, controlName As String) As Control
        If controls Is Nothing Then
            Return Nothing
        End If

        For Each ctrl As Control In controls
            If ctrl Is Nothing OrElse ctrl.IsDisposed Then
                Return Nothing
            End If
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
    ''' <remarks>
    '''  Uses integer arithmetic; results are truncated toward zero. This returns the midpoint of the control bounds:
    '''  Left plus half of the control's width.
    ''' </remarks>
    <Extension>
    Friend Function FindHorizontalMidpoint(ctrl As Control) As Integer
        Return If(ctrl Is Nothing OrElse ctrl.IsDisposed,
                  0,
                  ctrl.Left + (ctrl.Width \ 2))
    End Function

    ''' <summary>
    '''  Calculates the vertical midpoint of a control, relative to its parent.
    ''' </summary>
    ''' <param name="ctrl">The control for which to find the vertical midpoint.</param>
    ''' <returns>
    '''  <see langword="Integer"/> representing the Y coordinate of the midpoint,
    '''  relative to the parent control.
    ''' </returns>
    ''' <remarks>
    '''  Uses integer arithmetic; results are truncated toward zero. This returns the midpoint of the control bounds:
    '''  Top plus half of the control's height.
    ''' </remarks>
    <Extension>
    Friend Function FindVerticalMidpoint(ctrl As Control) As Integer
        Return If(ctrl Is Nothing OrElse ctrl.IsDisposed,
                  0,
                  ctrl.Top + (ctrl.Height \ 2))
    End Function

    ''' <summary>
    '''  Sets the <see cref="DataGridView.EnableHeadersVisualStyles"/> property to <see langword="False"/>
    '''  for all <see cref="DataGridView"/> controls within the specified control and applies a dark header
    '''  color scheme (black background, white foreground).
    ''' </summary>
    ''' <param name="ctrl">
    '''  The parent control containing the DataGridViews. If <see langword="Nothing"/> or disposed,
    '''  the method exits without performing any action.
    ''' </param>
    ''' <remarks>
    '''  The method traverses the immediate child controls recursively and sets header visual styles
    '''  and colors directly on each <see cref="DataGridView"/> instance; it does not replace or clone existing styles.
    '''  This routine is defensive against null/disposed parents to improve reliability when called during form teardown.
    ''' </remarks>
    <Extension>
    Friend Sub SetDgvCustomHeadersVisualStyles(ctrl As Control)
        If ctrl Is Nothing OrElse ctrl.IsDisposed Then
            Exit Sub
        End If

        For Each c As Control In ctrl.Controls
            Dim dgv As DataGridView = TryCast(c, DataGridView)
            If dgv IsNot Nothing AndAlso Not dgv.IsDisposed Then
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
