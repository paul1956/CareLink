' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class OptionsDialog
    Private Property SaveGraphColorDictionary As Dictionary(Of String, KnownColor)

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ItemNameComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ItemNameComboBox.SelectedIndexChanged
        Me.KnownColorsComboBox1.SelectedIndex = GetIndexOfKnownColor(Me.ItemNameComboBox.SelectedValue)
        Me.UpdateForeground_Button.Enabled = False
        Application.DoEvents()
    End Sub

    Private Sub KnownColorsComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles KnownColorsComboBox1.SelectedIndexChanged
        If Me.ItemNameComboBox.SelectedIndex < 0 OrElse Me.KnownColorsComboBox1.SelectedIndex < 0 Then
            Exit Sub
        End If

        Dim knownColor As KnownColor = Me.KnownColorsComboBox1.SelectedValue
        Dim itemColor As KnownColor = Me.ItemNameComboBox.SelectedValue
        Dim colorChanged As Boolean = knownColor <> itemColor
        Me.UpdateForeground_Button.Enabled = colorChanged
        Me.OK_Button.Enabled = Not colorChanged
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Me.DialogResult = DialogResult.OK
        Me.Cursor = Cursors.WaitCursor
        Application.DoEvents()
        If MsgBox(
                heading:="Are you sure you want to continue?",
                text:=$"Yes will save changes and application will restart{vbCrLf}If you select ""No"" changes will be lost.",
                buttonStyle:=MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation,
                title:="Color Options") = MsgBoxResult.Yes Then
            WriteColorDictionaryToFile()
            Application.Restart()
        Else
            GraphColorDictionary = Me.SaveGraphColorDictionary.Clone
        End If

        StartOrStopServerUpdateTimer(Start:=True)
        Me.Cursor = Cursors.Default
        Application.DoEvents()
        Me.Close()
    End Sub

    Private Sub OptionsDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StartOrStopServerUpdateTimer(Start:=False)
        Me.SaveGraphColorDictionary = GraphColorDictionary.Clone
        Me.ItemNameComboBox.DataSource = GetColorDictionaryBindingSource()
        Me.ItemNameComboBox.DisplayMember = "Key"
        Me.ItemNameComboBox.ValueMember = "Value"

        Me.ItemNameComboBox.SelectedIndex = 0
        Me.KnownColorsComboBox1.SelectedIndex = GetIndexOfKnownColor(Me.ItemNameComboBox.SelectedValue)
    End Sub

    Private Sub UpdateForeground_Button_Click(sender As Object, e As EventArgs) Handles UpdateForeground_Button.Click
        Dim item As KnownColor = Me.KnownColorsComboBox1.SelectedValue
        Dim key As String = Me.ItemNameComboBox.SelectedText
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
