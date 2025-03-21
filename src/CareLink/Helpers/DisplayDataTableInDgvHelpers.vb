' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module DisplayDataTableInDgvHelpers

    Friend Delegate Sub attachHandlers(dgv As DataGridView)

    <Extension>
    Friend Sub DisplayDataTableInDGV(
        realPanel As TableLayoutPanel,
        table As DataTable,
        dGV As DataGridView,
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
        If table?.Rows.Count > 0 Then
            Dim dGVIndex As Integer = realPanel.Controls.Count - 1
            Dim dGV As DataGridView = TryCast(realPanel.Controls(dGVIndex), DataGridView)

            If dGV Is Nothing Then
                dGV = New DataGridView With {.Name = $"DataGridView{className}"}
                dGV.InitializeDgv()
                dGV.AutoSize = True
                dGV.Dock = DockStyle.Fill
                realPanel.Controls.Add(dGV, column:=0, row:=1)
            Else
                dGV.InitializeDgv()
                dGV.Dock = DockStyle.Fill
            End If
            attachHandlers?(dGV)
            dGV.DataSource = Nothing
            dGV.DataSource = table
            dGV.RowHeadersVisible = False
            If hideRecordNumberColumn AndAlso dGV.Columns(0).Name = "RecordNumber" Then
                dGV.Columns("RecordNumber").Visible = False
            End If
        Else
            DisplayEmptyDGV(realPanel, className)
        End If
        Form1.Refresh()
    End Sub

    <Extension>
    Friend Sub DisplayDataTableInDGV(
        realPanel As TableLayoutPanel,
        table As DataTable,
        className As String,
        rowIndex As ServerDataIndexes,
        Optional hideRecordNumberColumn As Boolean = False)

        realPanel.SetTabName(rowIndex, isClearedNotifications:=False)
        If table?.Rows.Count > 0 Then
            Dim dGVIndex As Integer = realPanel.Controls.Count - 1
            Dim dGV As DataGridView = TryCast(realPanel.Controls(dGVIndex), DataGridView)

            If dGV Is Nothing Then
                dGV = New DataGridView With {.Name = $"DataGridView{className}"}
                dGV.InitializeDgv()
                dGV.AutoSize = True
                dGV.Dock = DockStyle.Fill
                realPanel.Controls.Add(dGV, column:=0, row:=1)
            Else
                dGV.InitializeDgv()
                dGV.Dock = DockStyle.Fill
            End If
            dGV.DataSource = Nothing
            dGV.DataSource = table
            dGV.RowHeadersVisible = False
            If hideRecordNumberColumn AndAlso dGV.Columns(0).Name = "RecordNumber" Then
                dGV.Columns("RecordNumber").Visible = False
            End If
        Else
            DisplayEmptyDGV(realPanel, className)
        End If
        Form1.Refresh()
    End Sub

End Module
