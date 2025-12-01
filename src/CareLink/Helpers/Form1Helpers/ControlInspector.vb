' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module ControlInspector
    Public Function GetAllControls(root As Control) As List(Of ControlInfo)
        Dim result As New List(Of ControlInfo)()

        'Iterate all controls in tab order, including nested.[web:5][web:19]
        Dim current As Control = root.GetNextControl(root, True)
        Dim seen As New HashSet(Of Control)()
        While current IsNot Nothing
            ' Use HashSet.Add return value to avoid double lookup (Contains then Add).
            If seen.Add(current) Then
                Dim parentName As String = If(current.Parent IsNot Nothing,
                                              current.Parent.Name,
                                              String.Empty)

                result.Add(New ControlInfo With {
                    .ControlName = current.Name,
                    .ParentName = parentName,
                    .X = current.Location.X,
                    .Y = current.Location.Y, 'Location is relative to parent
                    .Width = current.Width,
                    .Height = current.Height, 'Width/Height from Size
                    .AnchorStyle = ControlInfo.FormatAnchor(current.Anchor)
                })
            End If

            current = root.GetNextControl(current, True) 'Walk all nested controls
        End While

        Return result
    End Function

End Module
