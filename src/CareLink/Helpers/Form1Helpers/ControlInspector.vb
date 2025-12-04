' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text

Friend Module ControlInspector

    Public Function GetAllControls(root As Control) As List(Of ControlInfo)
        'Include the root control itself (e.g., the Form).
        Dim result As New List(Of ControlInfo) From {
            New ControlInfo With {
                .ControlName = root.Name,
                .ParentName = String.Empty,
                .X = root.Location.X,
                .Y = root.Location.Y,
                .Width = root.Width,
                .Height = root.Height,
                .AnchorStyle = ControlInfo.FormatAnchor(root.Anchor)}}

        'Start with Nothing so the first call returns the first control in tab order.
        Dim current As Control = root.GetNextControl(Nothing, True)
        Dim seen As New HashSet(Of Control)()

        While current IsNot Nothing
            If seen.Add(current) Then
                If IsNotNullOrWhiteSpace(current.Name) Then
                    result.Add(New ControlInfo With {
                        .ControlName = current.Name,
                        .ParentName = (current.Parent?.Name),
                        .X = current.Location.X,
                        .Y = current.Location.Y,
                        .Width = current.Width,
                        .Height = current.Height,
                        .AnchorStyle = ControlInfo.FormatAnchor(current.Anchor)})
                Else
                    ' Ignore native scrollbar windows created by other controls (e.g., DataGridView).
                    ' If the unnamed control is not a native ScrollBar, break into debugger.
                    If Not IsNativeScrollBar(current) Then
                        Stop
                    End If
                End If
            Else
                Stop
            End If

            current = root.GetNextControl(current, True)
        End While

        Return result
    End Function

    Private Function IsNativeScrollBar(ctrl As Control) As Boolean
        If ctrl Is Nothing Then
            Return False
        End If

        Try
            ' Accessing Handle may create it; acceptable for diagnostic/inspection helpers.
            Dim h As IntPtr = ctrl.Handle
            Dim sb As New StringBuilder(256)
            If GetClassName(h, sb, sb.Capacity) > 0 Then
                Return sb.ToString().ContainsNoCase("ScrollBar")
            End If
        Catch
            ' If we can't get a class name, don't treat as scrollbar.
        End Try

        Return False
    End Function

End Module
