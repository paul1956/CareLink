' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class OptionsDialog

    Private Property SaveGraphColorDictionary As Dictionary(Of String, KnownColor)

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ItemNameComboBox_DrawItem(sender As Object, e As DrawItemEventArgs) Handles ItemNameComboBox.DrawItem
        If e.Index >= 0 Then
            Dim eBounds As Rectangle = e.Bounds
            Dim comboBox1 As ComboBox = DirectCast(sender, ComboBox)
            Dim keyValuePair As KeyValuePair(Of String, KnownColor) = CKvp(comboBox1.Items(e.Index))
            Using b As Brush = New SolidBrush(SystemColors.Control)
                Dim pt As New Point(eBounds.X, eBounds.Top)
                e.Graphics.FillRectangle(b, eBounds.X, eBounds.Y, eBounds.Width - 200, eBounds.Height)
                TextRenderer.DrawText(e.Graphics, keyValuePair.Key, Me.Font, pt, SystemColors.ControlText, SystemColors.Control)
            End Using

            Dim paintColor As Color = keyValuePair.Value.ToColor
            If keyValuePair.Key = "Min Basal" Then
                paintColor = Color.FromArgb(150, paintColor)
            End If

            Using b As Brush = New SolidBrush(paintColor)
                e.Graphics.FillRectangle(b, eBounds.X + 150, eBounds.Y, eBounds.Width - 150, eBounds.Height)
            End Using
        End If
        Me.UpdateForeground_Button.Enabled = False
    End Sub

    Private Sub ItemNameComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ItemNameComboBox.SelectedIndexChanged
        If GetAllKnownColors().Count = 0 Then Exit Sub
        Me.KnownColorsComboBox.SelectedIndex = GetIndexOfKnownColor(CKvp(Me.ItemNameComboBox.SelectedItem).Value)
        Application.DoEvents()
    End Sub

    Private Sub KnownColorsComboBox_DrawItem(sender As Object, e As DrawItemEventArgs) Handles KnownColorsComboBox.DrawItem
        If e.Index >= 0 Then
            ' get the colors to use for this KnownColorsValue for this
            Dim comboBox1 As ComboBox = DirectCast(sender, ComboBox)
            Dim item As KeyValuePair(Of String, KnownColor) = CKvp(comboBox1.Items(e.Index))
            Dim key As String = item.Key
            Dim itemColor As Color = item.Value.ToColor
            Dim fClr As Color = itemColor.GetContrastingColor()
            Dim eBounds As Rectangle = e.Bounds
            Using b As Brush = New SolidBrush(itemColor)
                Dim pt As New Point(eBounds.X, eBounds.Top)
                e.Graphics.FillRectangle(b, eBounds.X, eBounds.Y, eBounds.Width, eBounds.Height)
                TextRenderer.DrawText(e.Graphics, key, Me.Font, pt, fClr, itemColor)
            End Using
        End If
        e.DrawFocusRectangle()
    End Sub

    Private Sub KnownColorsComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles KnownColorsComboBox.SelectedIndexChanged
        If Me.ItemNameComboBox.SelectedIndex < 0 OrElse Me.KnownColorsComboBox.SelectedIndex < 0 Then
            Exit Sub
        End If

        Dim knownColor As KnownColor = GetKnownColorFromName(CKvp(Me.KnownColorsComboBox.SelectedItem).Key)
        Dim itemColor As KnownColor = CKvp(Me.ItemNameComboBox.SelectedItem).Value
        Dim colorChanged As Boolean = knownColor <> itemColor
        Me.UpdateForeground_Button.Enabled = colorChanged
        Me.OK_Button.Enabled = Not colorChanged
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Me.DialogResult = DialogResult.OK
        Me.Cursor = Cursors.WaitCursor
        Application.DoEvents()
        If MsgBox("If you continue, changes will be saved and application will restart, if you select ""No"" changes will be lost.", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            ColorDictionaryToFile(RepoName)
            Application.Restart()
        Else
            ColorDictionaryFromBackup(Me.SaveGraphColorDictionary)
        End If

        Form1.ServerUpdateTimer.Start()
        Me.Cursor = Cursors.Default
        Application.DoEvents()
        Me.Close()
    End Sub

    Private Sub OptionsDialog_Load(sender As Object, e As EventArgs) Handles Me.Load
        Form1.ServerUpdateTimer.Stop()
        ColorDictionaryBackup(Me.SaveGraphColorDictionary)
        Me.KnownColorsComboBox.DataSource = GetKnownColorsBindingSource()
        Me.KnownColorsComboBox.DisplayMember = "Key"
        Me.KnownColorsComboBox.ValueMember = "Value"

        Me.ItemNameComboBox.DataSource = GetGraphColorsBindingSource()
        Me.ItemNameComboBox.DisplayMember = "Key"
        Me.ItemNameComboBox.ValueMember = "Value"

        Me.ItemNameComboBox.SelectedIndex = 0
        Me.KnownColorsComboBox.SelectedIndex = GetIndexOfKnownColor(CKvp(Me.ItemNameComboBox.SelectedItem).Value)
    End Sub

    Private Sub UpdateForeground_Button_Click(sender As Object, e As EventArgs) Handles UpdateForeground_Button.Click
        Dim item As KnownColor = CKvp(Me.KnownColorsComboBox.SelectedItem).Value
        Dim key As String = CKvp(Me.ItemNameComboBox.SelectedItem).Key
        UpdateColorDictionary(key, item)
        Me.ItemNameComboBox.DataSource = GetGraphColorsBindingSource()
        Me.OK_Button.Enabled = True
        Application.DoEvents()
    End Sub

End Class
