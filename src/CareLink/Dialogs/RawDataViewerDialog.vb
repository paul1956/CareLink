' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json
Imports System.Text.Json.Nodes
Imports System.Windows.Forms

Public Class RawDataViewerDialog

    Public Sub New(json As JsonElement)
        Me.InitializeComponent()
        Dim rootNode As JsonNode = JsonNode.Parse(json:=json.ToString())
        Me.RawDataRTB.Text = JsonSerializer.Serialize(value:=rootNode, options:=s_jsonSerializerOptions)
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class
