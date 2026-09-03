' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json
Imports System.Text.Json.Nodes

Public Class RawDataViewerDialog
    Private _lastSearchIndex As Integer = 0

    Public Sub New(json As JsonElement)
        Me.InitializeComponent()
        Dim rootNode As JsonNode = JsonNode.Parse(json:=json.ToString())
        Me.RawDataRTB.Text =
            rootNode.ToJsonString(options:=Me.SerializerOptions)
    End Sub

    Private ReadOnly Property SerializerOptions As New JsonSerializerOptions With
         {.WriteIndented = True}

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub FindAll(sender As Object, e As EventArgs) Handles btnFindAll.Click
        Dim rtb As RichTextBox = Me.RawDataRTB
        rtb.FindAll(Me.txtFind.Text)
    End Sub

    Private Sub FindNext(sender As Object, e As EventArgs) Handles btnFindNext.Click
        Dim rtb As RichTextBox = Me.RawDataRTB
        rtb.FindNext(Me.txtFind.Text, lastSearchIndex:=_lastSearchIndex)
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub RawDataViewerDialog_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.txtFind.Top = Me.RawDataRTB.Bottom + 10
        Me.txtFind.Left = 10
        Me.txtFind.Width = 200

        ' Find Button setup
        Me.btnFindNext.Text = "Find Next"
        Me.btnFindNext.Top = Me.RawDataRTB.Bottom + 8
        Me.btnFindNext.Left = Me.txtFind.Right + 10
    End Sub

    Private Sub txtFind_TextChanged(sender As Object, e As EventArgs) Handles txtFind.TextChanged
        Dim rtb As RichTextBox = Me.RawDataRTB
        rtb.SelectAll()
        rtb.SelectionColor = rtb.ForeColor
        rtb.Select(start:=0, length:=0) ' Move caret to start
        rtb.FindNext(Me.txtFind.Text, lastSearchIndex:=_lastSearchIndex)
    End Sub

End Class
