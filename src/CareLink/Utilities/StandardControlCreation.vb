' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Module StandardControlCreation

    Friend Sub initializeTableLayoutPanel(realPanel As TableLayoutPanel, rowIndex As ItemIndexs)
        realPanel.RowCount = 1
        If realPanel.Controls.Count > 1 AndAlso TypeOf realPanel.Controls(1) IsNot DataGridView Then
            For i As Integer = 1 To realPanel.Controls.Count - 1
                realPanel.Controls.RemoveAt(1)
            Next
            For i As Integer = 1 To realPanel.RowStyles.Count - 1
                realPanel.RowStyles.RemoveAt(1)
            Next
        End If
        realPanel.Controls(0).Text = $"{CInt(rowIndex)} {rowIndex}"
    End Sub

    Friend Function InitializeWorkingPanel(realPanel As TableLayoutPanel, rowIndex As ItemIndexs) As TableLayoutPanel
        initializeTableLayoutPanel(realPanel, rowIndex)
        Return realPanel
    End Function

End Module
