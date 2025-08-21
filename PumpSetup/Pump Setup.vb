' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports CareLink

Public Class PumpSetup
    Private Const CareLinkUrl As String =
        "https://www.medtronicdiabetes.com/products/carelink-personal-diabetes-software"

    Private _currentUrlUnderMouse As String = ""

    ''' <summary>
    '''  Holds the list of PDF files found in the Downloads directory.
    ''' </summary>
    ''' <remarks>
    '''  This is initialized in the Load event of the form.
    ''' </remarks>
    Private _pdfFilesInDownLoadDirectory As String()

    Private Property PdfFileNameWithPath As String = ""

    ''' <summary>
    '''  Gets the word at the specified index in the given text.
    ''' </summary>
    ''' <param name="text">The text to search.</param>
    ''' <param name="index">The index at which to find the word.</param>
    ''' <returns>
    '''  The word at the specified index, or an empty string if the index is out of bounds.
    ''' </returns>
    Private Shared Function GetWordAtIndex(text As String, index As Integer) As String
        If index < 0 OrElse index >= text.Length Then Return ""
        Dim start As Integer = index
        Dim finish As Integer = index
        While start > 0 AndAlso Not Char.IsWhiteSpace(c:=text(index:=start - 1))
            start -= 1
        End While
        While finish < text.Length - 1 AndAlso Not Char.IsWhiteSpace(c:=text(index:=finish + 1))
            finish += 1
        End While
        Return text.Substring(startIndex:=start, length:=finish - start + 1)
    End Function

    Private Sub Accept_Button_Click(sender As Object, e As EventArgs) Handles Accept_Button.Click
        Me.Cursor = Cursors.WaitCursor
        Application.DoEvents()

        Dim currentPdf As New PdfSettingsRecord(Me.PdfFileNameWithPath)
        If currentPdf Is Nothing OrElse Not currentPdf.IsValid() Then
            MessageBox.Show(text:="The selected PDF file is not a valid Settings file.",
                            caption:="Invalid PDF File",
                            buttons:=MessageBoxButtons.OK,
                            icon:=MessageBoxIcon.Error)
        Else
            Using dialog As New PumpSetupDialog
                dialog.Pdf = currentPdf
                dialog.ShowDialog(owner:=Me)
            End Using
        End If
        Me.Cursor = Cursors.Default
        Application.DoEvents()
        End
    End Sub

    ''' <summary>
    '''  Handles the SelectedIndexChanged event for the ComboBoxPDFs to enable
    '''  or disable the Accept button.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The EventArgs for the SelectedIndexChanged event.</param>
    Private Sub ComboBoxPDFs_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles ComboBoxPDFs.SelectedIndexChanged
        ' Enable the Accept button only if a valid PDF is selected
        Dim validPdf As Boolean = Me.ComboBoxPDFs.SelectedIndex > 0
        Me.Accept_Button.Enabled = validPdf  ' Index 0 is "(None)"
        Me.UserName.Enabled = Me.ComboBoxPDFs.SelectedIndex = 0
        If validPdf Then
            ' If a valid PDF is selected, set the current PDF name with path
            Me.PdfFileNameWithPath =
                IO.Path.Combine(GetDownloadsDirectory(), Me.ComboBoxPDFs.SelectedItem.ToString())
        Else
            ' If "(None)" is selected, clear the current PDF name with path
            Me.PdfFileNameWithPath = ""
        End If
    End Sub

    ''' <summary>
    '''  Handles the Click event for the Exit_Button to close the form.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The EventArgs for the Click event.</param>
    Private Sub copyUrlMenuItem_Click(sender As Object, e As EventArgs) Handles copyUrlMenuItem.Click
        If Not String.IsNullOrEmpty(_currentUrlUnderMouse) Then
            Clipboard.SetText(_currentUrlUnderMouse)
        End If
    End Sub

    Private Sub Exit_Button_Click(sender As Object, e As EventArgs) Handles Exit_Button.Click
        End
    End Sub

    Private Sub InstructionsRtb_LinkClicked(
        sender As Object,
        e As LinkClickedEventArgs) Handles InstructionsRtb.LinkClicked

        Dim startInfo As New ProcessStartInfo(fileName:=e.LinkText) With {
            .UseShellExecute = True}
        Process.Start(startInfo)
    End Sub

    ''' <summary>
    '''  Handles the MouseDown event for the InstructionsRtb to detect right-clicks on URLs.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The MouseEventArgs for the MouseDown event.</param>
    Private Sub InstructionsRtb_MouseDown(sender As Object, e As MouseEventArgs) _
        Handles InstructionsRtb.MouseDown
        If e.Button = MouseButtons.Right Then
            Dim idx As Integer = Me.InstructionsRtb.GetCharIndexFromPosition(pt:=e.Location)
            Dim word As String = GetWordAtIndex(Me.InstructionsRtb.Text, idx)
            If Uri.IsWellFormedUriString(word, UriKind.Absolute) Then
                _currentUrlUnderMouse = word
                Me.copyUrlMenuItem.Visible = True
            Else
                _currentUrlUnderMouse = ""
                Me.copyUrlMenuItem.Visible = False
            End If
        End If
    End Sub

    ''' <summary>
    '''  Handles the Load event for the PumpSetup form to initialize the instructions and focus.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The EventArgs for the Load event.</param>
    Private Sub PumpSetup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim settingExist As Boolean = IO.Directory.Exists(path:=GetSettingsDirectory)
        _pdfFilesInDownLoadDirectory =
            IO.Directory.GetFiles(path:=GetDownloadsDirectory(), searchPattern:=$"*.pdf")

        Dim downloadFilesExist As Boolean = _pdfFilesInDownLoadDirectory.Length = 0
        If downloadFilesExist AndAlso Not settingExist Then
            Dim text As String =
                $"No PDF files exist, " &
                $"Please download from {CareLinkUrl} and restart program."
            MessageBox.Show(text,
                            caption:="CareLink PDF Settings File Required",
                            buttons:=MessageBoxButtons.OK,
                            icon:=MessageBoxIcon.Warning)
            Me.Close()
        End If
        If downloadFilesExist Then
            Dim text As String =
                $"No PDF files exist in download directory," &
                $" Please download from {CareLinkUrl} and restart program."
            MessageBox.Show(
                text,
                caption:="CareLink PDF Settings File Required",
                buttons:=MessageBoxButtons.OK,
                icon:=MessageBoxIcon.Warning)
            Me.Close()
        End If

        Dim keySelector As Func(Of String, Date) =
            Function(path) IO.File.GetLastWriteTime(path)
        _pdfFilesInDownLoadDirectory =
            _pdfFilesInDownLoadDirectory.OrderByDescending(keySelector).ToArray()

        Dim step1 As String
        Dim existingUserMessageStart As String =
            "Since you are an existing CareLink™ for Windows user, you can use your existing" &
            " Settings file by entering your CareLink™ UserName in the box below then" &
            $" click Accept.{vbCrLf}Your information will not be saved" &
            $" or verified locally.{vbCrLf}If instead want to use a new PDF Settings" &
            " file that you HAVE download from"
        Dim newUserMessageStart As String =
            $"To start you will need to download your latest CareLink file from"
        step1 = If(settingExist,
                   existingUserMessageStart,
                   newUserMessageStart)
        Me.UserName.Visible = settingExist

        step1 &= $"{vbCr}{CareLinkUrl}.{vbCr}" &
            $"Select it from the dropdown then click Accept."
        Me.InstructionsRtb.Text = step1
        Me.InstructionsRtb.BoldText(text:="Accept")

        Me.ComboBoxPDFs.Items.Add(item:="(None)")
        For Each pdfFile As String In _pdfFilesInDownLoadDirectory
            Me.ComboBoxPDFs.Items.Add(item:=IO.Path.GetFileName(path:=pdfFile))
        Next

        ' Set default selection to 'None'
        Me.ComboBoxPDFs.SelectedIndex = 0
    End Sub

    ''' <summary>
    '''  Handles the KeyPress event for the UserName TextBox to restrict input
    '''  to letters and digits only.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The KeyPress event data.</param>
    Private Sub UserName_KeyPress(sender As Object, e As KeyPressEventArgs) _
        Handles UserName.KeyPress

        Dim c As Char = e.KeyChar
        ' Block anything that is not a Unicode letter or decimal digit
        If Not Char.IsLetterOrDigit(c) AndAlso Not Char.IsControl(c) Then
            e.Handled = True
        End If
    End Sub

    Private Sub UserName_TextChanged(sender As Object, e As EventArgs) _
        Handles UserName.TextChanged
        Dim path As String =
            IO.Path.Combine(GetSettingsDirectory(), $"{Me.UserName.Text}Settings.pdf")
        Dim validPdf As Boolean = IO.File.Exists(path)
        Me.Accept_Button.Enabled = validPdf
        Me.ComboBoxPDFs.Enabled = Not validPdf
        If validPdf Then
            ' If a valid PDF is selected, set the current PDF name with path
            Me.PdfFileNameWithPath = path
        Else
            ' If "(None)" is selected, clear the current PDF name with path
            Me.PdfFileNameWithPath = ""
        End If
    End Sub

End Class
