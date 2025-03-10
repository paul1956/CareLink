' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module DisplayDataTableInDgvHelpers

    Friend Delegate Sub attachHandlers(dgv As DataGridView)

    <Extension>
    Friend Sub DisplayDataTableInDGV(
        realPanel As TableLayoutPanel,
        dGV As DataGridView,
        table As DataTable,
        rowIndex As ServerDataIndexes)

        realPanel.SetTabName(rowIndex, isClearedNotifications:=False)
        dGV.InitializeDgv()
        dGV.DataSource = table
        dGV.RowHeadersVisible = False
    End Sub

    <Extension>
    Friend Sub DisplayDataTableInDGV(
        realPanel As TableLayoutPanel,
        table As DataTable,
        className As String,
        attachHandlers As attachHandlers,
        rowIndex As ServerDataIndexes,
        hideRecordNumberColumn As Boolean)

        realPanel.SetTabName(rowIndex, isClearedNotifications:=False)
        Dim dGVIndex As Integer = realPanel.Controls.Count - 1
        Dim dGV As DataGridView = TryCast(realPanel.Controls(dGVIndex), DataGridView)

        If dGV Is Nothing Then
            dGV = New DataGridView With {.Name = $"DataGridView{className}"}
            dGV.InitializeDgv()
            dGV.AutoSize = True
        Else
            dGV.InitializeDgv()
        End If
        realPanel.Controls.Add(dGV, 0, 1)
        attachHandlers?(dGV)
        dGV.DataSource = table
        dGV.RowHeadersVisible = False
        If hideRecordNumberColumn AndAlso dGV.Columns(0).Name = "RecordNumber" Then
            dGV.Columns("RecordNumber").Visible = False
        End If
    End Sub

End Module
