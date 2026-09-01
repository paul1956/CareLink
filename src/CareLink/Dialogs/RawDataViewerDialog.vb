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

    Private Sub FindNext(sender As Object, e As EventArgs) Handles btnFind.Click
        Dim searchText As String = Me.txtFind.Text

        ' Validate input
        If String.IsNullOrWhiteSpace(searchText) Then
            MessageBox.Show("Please enter text to search.", "No Search Text", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Search for the text starting from the last found position
        Dim index As Integer = Me.RawDataRTB.Find(searchText, _lastSearchIndex, RichTextBoxFinds.None)

        ' If not found, wrap around and search from the beginning
        If index = -1 AndAlso _lastSearchIndex > 0 Then
            index = Me.RawDataRTB.Find(searchText, 0, RichTextBoxFinds.None)
        End If

        ' Highlight if found
        If index <> -1 Then
            Me.RawDataRTB.Select(index, searchText.Length)
            Me.RawDataRTB.ScrollToCaret()
            _lastSearchIndex = index + searchText.Length
        Else
            MessageBox.Show(text:="No more occurrences found.",
                            caption:="Search Complete",
                            buttons:=MessageBoxButtons.OK,
                            icon:=MessageBoxIcon.Information)
            _lastSearchIndex = 0
        End If
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
        Me.btnFind.Text = "Find Next"
        Me.btnFind.Top = Me.RawDataRTB.Bottom + 8
        Me.btnFind.Left = Me.txtFind.Right + 10
    End Sub

End Class
