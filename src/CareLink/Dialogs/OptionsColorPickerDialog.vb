' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel

Public Class OptionsColorPickerDialog
    Private Property SaveGraphColorDictionary As Dictionary(Of String, KnownColor)

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) _
        Handles Cancel_Button.Click

        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ItemNameComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles ItemNameComboBox.SelectedIndexChanged

        Me.KnownColorsComboBox1.SelectedIndex =
            GetIndexOfKnownColor(item:=Me.ItemNameComboBox.SelectedValue)
        Me.UpdateForegroundButton.Enabled = False
        Application.DoEvents()
    End Sub

    Private Sub KnownColorsComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles KnownColorsComboBox1.SelectedIndexChanged

        If Me.ItemNameComboBox.SelectedIndex < 0 OrElse Me.KnownColorsComboBox1.SelectedIndex < 0 Then
            Exit Sub
        End If

        Dim knownColor As KnownColor = Me.KnownColorsComboBox1.SelectedValue
        Dim itemColor As KnownColor = Me.ItemNameComboBox.SelectedValue
        Dim colorChanged As Boolean = knownColor <> itemColor
        Me.UpdateForegroundButton.Enabled = colorChanged
        Me.OK_Button.Enabled = Not colorChanged
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Me.DialogResult = DialogResult.OK
        Me.Cursor = Cursors.WaitCursor
        Application.DoEvents()
        If MsgBox(
                heading:="Are you sure you want to continue?",
                prompt:=$"Yes will save changes and application will restart{vbCrLf}" &
                    "If you select ""No"" changes will be lost.",
                buttonStyle:=MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation,
                title:="Color Options") = MsgBoxResult.Yes Then
            WriteColorDictionaryToFile()
            Application.Restart()
        Else
            GraphColorDictionary = Me.SaveGraphColorDictionary.Clone
        End If

        SetServerUpdateTimer(Start:=True)
        Me.Cursor = Cursors.Default
        Application.DoEvents()
        Me.Close()
    End Sub

    Private Sub OptionsDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetServerUpdateTimer(Start:=False)
        Me.ItemNameComboBox.DrawMode = DrawMode.OwnerDrawFixed
        Dim resources As New ComponentResourceManager(GetType(OptionsColorPickerDialog))
        Me.KnownColorsComboBox1.SelectedItem = CType(
            resources.GetObject("KnownColorsComboBox1.SelectedItem"),
            KeyValuePair(Of String, KnownColor))
        Me.KnownColorsComboBox1.DrawMode = DrawMode.OwnerDrawFixed

        Me.SaveGraphColorDictionary = GraphColorDictionary.Clone
        Me.ItemNameComboBox.DataSource = GetColorDictionaryBindingSource()
        Me.ItemNameComboBox.DisplayMember = "Key"
        Me.ItemNameComboBox.ValueMember = "Value"

        Me.ItemNameComboBox.SelectedIndex = 0
        Me.KnownColorsComboBox1.SelectedIndex =
            GetIndexOfKnownColor(Me.ItemNameComboBox.SelectedValue)
    End Sub

    Private Sub UpdateForegroundButton_Click(sender As Object, e As EventArgs) _
        Handles UpdateForegroundButton.Click

        Dim item As KnownColor = Me.KnownColorsComboBox1.SelectedValue
        Dim key As String = Me.ItemNameComboBox.SelectedItem.Key
        Dim saveIndex As Integer = Me.ItemNameComboBox.SelectedIndex
        UpdateColorDictionary(key, item)
        Me.ItemNameComboBox.DataSource = Nothing
        Me.ItemNameComboBox.Items.Clear()
        Me.ItemNameComboBox.DataSource = GetColorDictionaryBindingSource()
        Me.ItemNameComboBox.SelectedIndex = saveIndex
        Me.OK_Button.Enabled = True
        Application.DoEvents()
    End Sub

End Class
