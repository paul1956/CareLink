' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Text.Json
Imports System.Text.Json.Nodes

Public Class RawDataViewerDialog
    Private _contextMenu As New ContextMenuStrip
    Private _copyMenuItem As ToolStripMenuItem
    Private _lastSearchIndex As Integer = 0

    ''' <summary>
    '''  Initializes a new instance of the <see cref="RawDataViewerDialog"/> class
    '''  with the specified JSON data.
    ''' </summary>
    ''' <param name="json">
    '''  The JSON data to display.
    ''' </param>
    Public Sub New(json As JsonElement)
        Me.InitializeComponent()
        Dim rootNode As JsonNode = JsonNode.Parse(json:=json.ToString())
        Me.RawDataRTB.Text = rootNode.ToJson
    End Sub

    ''' <summary>
    '''  Handles the Opening event of the context menu, enabling or disabling
    '''  the "Copy Selected Text" menu item based on whether any text is selected
    '''  in the RichTextBox.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">A <see cref="CancelEventArgs"/> that contains the event data.</param>
    Private Sub _contextMenu_Opening(sender As Object, e As CancelEventArgs)
        _copyMenuItem.Enabled = Me.RawDataRTB.SelectedText.Length > 0
    End Sub

    ''' <summary>
    '''  Handles the Click event of the Cancel button, closing the dialog
    '''  and setting the DialogResult to Cancel.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    ''' <summary>
    '''  Handles the Click event of the "Copy Selected Text" menu item, copying the selected text to the clipboard.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub CopySelectedText(sender As Object, e As EventArgs)
        Try
            If Me.RawDataRTB.SelectionLength > 0 Then
                Clipboard.SetText(Me.RawDataRTB.SelectedText)
            End If
        Catch ex As Exception
            MessageBox.Show(text:=$"Error copying text: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    '''  Handles the Click event of the "Find All" button, finding all occurrences of the search text in the RichTextBox.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub FindAll(sender As Object, e As EventArgs) Handles btnFindAll.Click
        Dim rtb As RichTextBox = Me.RawDataRTB
        rtb.FindAll(Me.txtFind.Text)
    End Sub

    ''' <summary>
    '''  Handles the Click event of the "Find Next" button, finding the next occurrence of the search text in the RichTextBox.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub FindNext(sender As Object, e As EventArgs) Handles btnFindNext.Click
        Dim rtb As RichTextBox = Me.RawDataRTB
        rtb.FindNext(Me.txtFind.Text, reportNotFound:=True, lastSearchIndex:=_lastSearchIndex)
    End Sub

    ''' <summary>
    '''  Handles the Click event of the OK button, closing the dialog and setting the DialogResult to OK.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    ''' <summary>
    '''  Handles the FormClosing event of the RawDataViewerDialog,
    '''  removing event handlers for the context menu and copy menu item.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">A <see cref="FormClosingEventArgs"/> that contains the event data.</param>
    Private Sub RawDataViewerDialog_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        RemoveHandler _copyMenuItem.Click, AddressOf Me.CopySelectedText
        RemoveHandler _contextMenu.Opening, AddressOf Me._contextMenu_Opening
    End Sub

    ''' <summary>
    '''  Handles the Load event of the RawDataViewerDialog, initializing the positions and sizes of controls.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub RawDataViewerDialog_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.txtFind.Top = Me.RawDataRTB.Bottom + 10
        Me.txtFind.Left = 10
        Me.txtFind.Width = 200

        ' Find Button setup
        Me.btnFindNext.Text = "Find Next"
        Me.btnFindNext.Top = Me.RawDataRTB.Bottom + 8
        Me.btnFindNext.Left = Me.txtFind.Right + 10

        _contextMenu = New ContextMenuStrip()
        _copyMenuItem = New ToolStripMenuItem(text:="Copy Selected Text")
        AddHandler _copyMenuItem.Click, AddressOf Me.CopySelectedText
        _contextMenu.Items.Add(value:=_copyMenuItem)
        Me.RawDataRTB.ContextMenuStrip = _contextMenu
        AddHandler _contextMenu.Opening, AddressOf Me._contextMenu_Opening
    End Sub

    ''' <summary>
    '''  Handles the KeyDown event of the RichTextBox, allowing navigation keys but blocking editing keys.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">A <see cref="KeyEventArgs"/> that contains the event data.</param>
    Private Sub rtb_KeyDown(sender As Object, e As KeyEventArgs) Handles RawDataRTB.KeyDown
        ' Allow navigation keys but block editing keys
        If e.KeyCode = Keys.Back OrElse e.KeyCode = Keys.Delete OrElse
           (e.Control AndAlso (e.KeyCode = Keys.V OrElse e.KeyCode = Keys.X)) Then
            e.SuppressKeyPress = True
        End If
    End Sub

    ''' <summary>
    '''  Handles the KeyPress event of the RichTextBox, blocking all key presses.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">A <see cref="KeyPressEventArgs"/> that contains the event data.</param>
    Private Sub rtb_KeyPress(sender As Object, e As KeyPressEventArgs) Handles RawDataRTB.KeyPress
        e.Handled = True ' Block all key presses
    End Sub

    ''' <summary>
    ''' Validates TextBox input against RichTextBox content and highlights matches.
    ''' </summary>
    Private Sub ValidateAndHighlight(sender As Object, e As EventArgs) Handles txtFind.TextChanged
        Dim rtb As RichTextBox = Me.RawDataRTB
        Dim inputBox As TextBox = CType(sender, TextBox)
        Dim typedText As String = inputBox.Text

        ' Clear previous highlights
        rtb.SelectAll()
        rtb.SelectionColor = rtb.ForeColor

        If String.IsNullOrEmpty(value:=typedText) Then
            Exit Sub
        End If

        ' Search for typed text in RichTextBox
        Dim matchIndex As Integer =
            rtb.Find(str:=typedText,
                     options:=RichTextBoxFinds.None)

        If matchIndex >= 0 Then
            ' Highlight the match
            rtb.Select(start:=matchIndex, length:=typedText.Length)
            rtb.SelectionColor = Color.Green
            rtb.FindNext(Me.txtFind.Text,
                         reportNotFound:=False,
                         lastSearchIndex:=_lastSearchIndex)
        Else
            ' Remove last character if no match
            If typedText.Length > 0 Then
                inputBox.Text =
                    typedText.Substring(startIndex:=0, length:=typedText.Length - 1)
                inputBox.SelectionStart = inputBox.Text.Length
            End If
        End If
    End Sub

End Class
