' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO

Public Class OptionsDialog

    Private Property SaveGraphColorDictionary As Dictionary(Of String, KnownColor)

    Private Shared Function GetContrastingKnownColor(knownClrBase As KnownColor) As KnownColor
        Dim clrBase As Color = knownClrBase.ToColor
        ' Y is the "brightness"
        Dim y As Double = (0.299 * clrBase.R) + (0.587 * clrBase.G) + (0.114 * clrBase.B)
        Return If(y < 140,
                  KnownColor.White,
                  KnownColor.Black
                 )
    End Function

    Public Shared Sub WriteColorDictionaryToFile()
        Using fileStream As FileStream = File.OpenWrite(GetPathToGraphColorsFile(True))
            Using sw As New StreamWriter(fileStream)
                sw.WriteLine($"Key,ForegroundColor,BackgroundColor")
                For Each kvp As KeyValuePair(Of String, KnownColor) In GraphColorDictionary
                    Dim contrastingColor As KnownColor = GetContrastingKnownColor(kvp.Value)
                    sw.WriteLine($"{kvp.Key},{kvp.Value},{contrastingColor}")
                Next
                sw.Flush()
                sw.Close()
            End Using
        End Using
    End Sub

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
        If MsgBox("Are you sure yiu want to continue?", $"Yes will save changes and application will restart{vbCrLf}If you select ""No"" changes will be lost.", MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation, "Color Options") = MsgBoxResult.Yes Then
            WriteColorDictionaryToFile()
            Application.Restart()
        Else
            GraphColorDictionary = Me.SaveGraphColorDictionary.Clone
        End If

        Form1.ServerUpdateTimer.Start()
        Me.Cursor = Cursors.Default
        Application.DoEvents()
        Me.Close()
    End Sub

    Private Sub OptionsDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Form1.ServerUpdateTimer.Stop()
        Me.SaveGraphColorDictionary = GraphColorDictionary.Clone
        Me.ItemNameComboBox.Items.Clear()
        Me.ItemNameComboBox.DataSource = New BindingSource(GraphColorDictionary, Nothing)
        Me.ItemNameComboBox.DisplayMember = "Key"
        Me.ItemNameComboBox.ValueMember = "Value"

        Me.ItemNameComboBox.SelectedIndex = 0
        Me.KnownColorsComboBox1.SelectedIndex = GetIndexOfKnownColor(Me.ItemNameComboBox.SelectedValue)
    End Sub

    Private Sub UpdateForeground_Button_Click(sender As Object, e As EventArgs) Handles UpdateForeground_Button.Click
        Dim item As KnownColor = Me.KnownColorsComboBox1.SelectedValue
        Dim key As String = Me.ItemNameComboBox.SelectedText
        Dim saveIndex As Integer = Me.ItemNameComboBox.SelectedIndex
        GraphColorDictionary(key) = item
        Me.ItemNameComboBox.DataSource = Nothing
        Me.ItemNameComboBox.Items.Clear()
        Me.ItemNameComboBox.DataSource = New BindingSource(GraphColorDictionary, Nothing)
        Me.ItemNameComboBox.SelectedIndex = saveIndex
        Me.OK_Button.Enabled = True
        Application.DoEvents()
    End Sub

    Public Shared Sub GetColorDictionaryFromFile()

        Using fileStream As FileStream = File.OpenRead(GetPathToGraphColorsFile(True))
            Using sr As New StreamReader(fileStream)
                sr.ReadLine()
                While sr.Peek() <> -1
                    Dim line As String = sr.ReadLine()
                    If Not line.Any Then
                        Continue While
                    End If
                    Dim splitLine() As String = line.Split(","c)
                    Dim key As String = splitLine(0)
                    If GraphColorDictionary.ContainsKey(key) Then
                        GraphColorDictionary(key) = GetKnownColorFromName(splitLine(1))
                    End If
                End While
                sr.Close()
            End Using

            fileStream.Close()
        End Using
    End Sub

    Public Shared Sub UpdateColorDictionary(key As String, item As KnownColor)
        GraphColorDictionary(key) = item
    End Sub

    Public Shared Sub UpdateColorDictionary(key As String, colorName As String)
        GraphColorDictionary(key) = GetKnownColorFromName(colorName)
    End Sub

End Class
