' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Reflection
Imports System.Text

Friend Module ControlInspector

    Public Function GetAllControlsAndNative(root As Control) As List(Of ControlInfo)
        'Include the root control itself (e.g., the Form).
        Dim result As New List(Of ControlInfo) From {
            New ControlInfo With {
                .ControlName = root.Name,
                .ParentName = String.Empty,
                .X = root.Location.X,
                .Y = root.Location.Y,
                .Width = root.Width,
                .Height = root.Height,
                .AnchorStyle = ControlInfo.FormatAnchor(root.Anchor),
                .BorderStyle = GetBorderStyle(root)}}

        'Traverse the control tree so nested controls (e.g., inside Panels) are included.
        Dim seen As New HashSet(Of Control)()
        Dim stack As New Stack(Of Control)()

        ' Seed with direct children of the root
        For Each c As Control In root.Controls
            stack.Push(c)
        Next

        While stack.Count > 0
            Dim current As Control = stack.Pop()
            If current Is Nothing Then
                Continue While
            End If

            If seen.Add(current) Then
                If IsNotNullOrWhiteSpace(current.Name) Then
                    result.Add(New ControlInfo With {
                        .ControlName = current.Name,
                        .ParentName = GetParentDisplayName(current.Parent),
                        .X = current.Location.X,
                        .Y = current.Location.Y,
                        .Width = current.Width,
                        .Height = current.Height,
                        .AnchorStyle = ControlInfo.FormatAnchor(current.Anchor),
                        .BorderStyle = GetBorderStyle(current)})
                Else
                    ' Handle unnamed controls specially: include SplitterPanel children so we can
                    ' show which panel (Panel1/Panel2) they are instead of stopping in the debugger.
                    Dim sc As System.Windows.Forms.SplitContainer = TryCast(current.Parent, System.Windows.Forms.SplitContainer)
                    If sc IsNot Nothing Then
                        Dim panelLabel As String = current.GetType().Name
                        If Object.ReferenceEquals(sc.Panel1, current) Then
                            panelLabel = "Panel1"
                        ElseIf Object.ReferenceEquals(sc.Panel2, current) Then
                            panelLabel = "Panel2"
                        End If

                        Dim displayName As String = If(IsNotNullOrWhiteSpace(sc.Name), $"{sc.Name}.{panelLabel}", panelLabel)
                        result.Add(New ControlInfo With {
                            .ControlName = displayName,
                            .ParentName = If(IsNotNullOrWhiteSpace(sc.Name), sc.Name, sc.GetType().Name),
                            .X = current.Location.X,
                            .Y = current.Location.Y,
                            .Width = current.Width,
                            .Height = current.Height,
                            .AnchorStyle = ControlInfo.FormatAnchor(current.Anchor),
                            .BorderStyle = GetBorderStyle(current)})
                    Else
                        ' Ignore native scrollbar windows created by other controls (e.g., DataGridView).
                        ' If the unnamed control is not a native ScrollBar or other native window, break into debugger.
                        If Not IsNativeControl(current) Then
                            Stop
                        End If
                    End If
                End If

                ' Push children to stack to visit nested controls
                For Each child As Control In current.Controls
                    stack.Push(child)
                Next
            Else
                Stop
            End If
        End While

        Return result
    End Function

    Private Function IsNativeControl(ctrl As Control) As Boolean
        If ctrl Is Nothing Then
            Return False
        End If

        Try
            ' Accessing Handle may create it; acceptable for diagnostic/inspection helpers.
            Dim h As IntPtr = ctrl.Handle
            Dim sb As New StringBuilder(256)
            If GetClassName(h, sb, sb.Capacity) > 0 Then
                Dim cls As String = sb.ToString()
                If cls.ContainsNoCase("ScrollBar") Then
                    Return True
                End If

                ' Other native windowed child controls may be present; treat them as native if they
                ' don't expose a managed Name and their class indicates a native window.
                If String.IsNullOrWhiteSpace(ctrl.Name) Then
                    ' Common native control class prefixes
                    If cls.StartsWith("MS", StringComparison.OrdinalIgnoreCase) OrElse cls.Contains("Sys") Then
                        Return True
                    End If
                End If
                Return False
            End If
        Catch
            ' If we can't get a class name, don't treat as scrollbar.
        End Try

        Return False
    End Function

    Private Function GetNearestNamedParentName(parent As Control) As String
        While parent IsNot Nothing
            If IsNotNullOrWhiteSpace(parent.Name) Then
                Return parent.Name
            End If
            parent = parent.Parent
        End While
        Return String.Empty
    End Function

    Private Function GetParentDisplayName(parent As Control) As String
        If parent Is Nothing Then
            Return String.Empty
        End If

        ' If the parent is a panel belonging to a SplitContainer, show the split container name and panel
        Dim sc As System.Windows.Forms.SplitContainer = TryCast(parent.Parent, System.Windows.Forms.SplitContainer)
        If sc IsNot Nothing Then
            Dim panelLabel As String = "Panel"
            If Object.ReferenceEquals(sc.Panel1, parent) Then
                panelLabel = "Panel1"
            ElseIf Object.ReferenceEquals(sc.Panel2, parent) Then
                panelLabel = "Panel2"
            End If

            If IsNotNullOrWhiteSpace(sc.Name) Then
                Return $"{sc.Name}.{panelLabel}"
            End If

            ' If split container has no name, include its type
            Return $"{sc.GetType().Name}.{panelLabel}"
        End If

        If IsNotNullOrWhiteSpace(parent.Name) Then
            Return parent.Name
        End If

        ' If parent has no name and is not a SplitContainer panel, return its type to help identify container
        Return parent.GetType().Name
    End Function

    Private Function GetBorderStyle(ctrl As Control) As String
        If ctrl Is Nothing Then
            Return String.Empty
        End If

        Try
            ' Many controls have a BorderStyle property (e.g., TextBox, Panel derived), access via reflection
            Dim pi As PropertyInfo = ctrl.GetType().GetProperty("BorderStyle")
            If pi IsNot Nothing Then
                Dim val As Object = pi.GetValue(ctrl)
                If val IsNot Nothing Then
                    Return val.ToString()
                End If
            End If
        Catch
            ' ignore
        End Try

        Return String.Empty
    End Function

End Module
