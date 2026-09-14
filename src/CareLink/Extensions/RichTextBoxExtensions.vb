' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module RichTextBoxExtensions
    Public Const LeftPanelTotalWidth As Integer = 54
    Public Const RightPanelTotalWidth As Integer = 66
    Public Const LeftColumnTotalWidth As Integer = 34
    Public Const Indent4 As String = "    "
    Public Const Indent8 As String = "        "
    Public Const Indent32 As String = "                                "

    Public ReadOnly Property FixedWidthBoldFont As New Font(familyName:="Consolas", emSize:=14, style:=FontStyle.Bold)

    Public ReadOnly Property FixedWidthFont As New Font(familyName:="Consolas", emSize:=14, style:=FontStyle.Regular)

    Public ReadOnly Property HeadingBoldFont As New Font(familyName:="Segoe UI", emSize:=16, style:=FontStyle.Bold)

    Public ReadOnly Property HeadingFont As New Font(familyName:="Segoe UI", emSize:=16, style:=FontStyle.Regular)

    ''' <summary>
    '''  Returns a string representation of the given text, centered within
    '''  a specified total width.
    ''' </summary>
    ''' <param name="text">The text to center.</param>
    ''' <param name="leftPanel">Indicates whether the text is in the left panel.</param>
    ''' <returns>A centered string representation of the text.</returns>
    <Extension>
    Friend Function AlignCenter(text As String, leftPanel As Boolean) As String
        Dim totalWidth As Integer =
            If(leftPanel,
               LeftPanelTotalWidth - LeftColumnTotalWidth,
               RightPanelTotalWidth)
        Dim pad As Integer = (totalWidth - text.Length) \ 2
        Return If(pad > 0,
                  text.PadLeft(totalWidth:=text.Length + pad).PadRight(totalWidth),
                  text.PadRight(totalWidth))
    End Function

    ''' <summary>
    '''  Centers text within an explicit fixed-width column (characters).
    ''' </summary>
    <Extension>
    Friend Function AlignCenter(text As String, totalWidth As Integer) As String
        If text.Length Mod 2 <> 0 Then
            text = " " & text
        End If
        Dim pad As Integer = (totalWidth - text.Length) \ 2
        Return If(pad > 0,
                  text.PadLeft(totalWidth:=text.Length + pad).PadRight(totalWidth),
                  text.PadRight(totalWidth))
    End Function

    ''' <summary>
    '''  Appends a text value pair to the current text in a <see cref="RichTextBox"/>,
    '''  with the text formatted in bold and the value in regular font.
    ''' </summary>
    ''' <param name="rtb">
    '''  The <see cref="RichTextBox"/> to append text to.
    ''' </param>
    ''' <param name="title">
    '''  The label or description text to append.
    ''' </param>
    ''' <param name="value">
    '''  The value associated with the label, which will be appended in regular font.
    ''' </param>
    ''' <param name="singleIndent"></param>
    <Extension>
    Friend Sub AppendKeyValue(rtb As RichTextBox,
                              leftPanel As Boolean,
                              title As String,
                              value As String,
                              Optional secondValue As String = "",
                              Optional indent As String = Indent4)
        Dim text As String = $"{indent}{title}"
        rtb.AppendTextNewFont(text, newFont:=FixedWidthBoldFont)

        ' Determine the available width on the right side (in characters)
        Dim totalRightWidth As Integer =
            If(leftPanel,
               LeftPanelTotalWidth - LeftColumnTotalWidth,
               RightPanelTotalWidth - LeftColumnTotalWidth)

        If String.IsNullOrEmpty(value:=secondValue) Then
            ' Single value: center it in the entire right area
            rtb.AppendTextNewFont(text:=value.AlignCenter(totalWidth:=totalRightWidth), newFont:=FixedWidthFont, padRight:=False, includeNewLine:=True)
        Else
            ' Two values: split the right area into two fixed-width columns
            Dim leftHalf As Integer = totalRightWidth \ 2
            Dim rightHalf As Integer = totalRightWidth - leftHalf

            Dim leftText As String = value.AlignCenter(totalWidth:=leftHalf)
            Dim rightText As String = secondValue.AlignCenter(totalWidth:=rightHalf)

            Dim combined As String = leftText & rightText
            rtb.AppendTextNewFont(text:=combined, newFont:=FixedWidthFont, padRight:=False, includeNewLine:=True)
        End If

        Application.DoEvents()
    End Sub

    ''' <summary>
    '''  Appends line break to current text of a <see cref="RichTextBox"/>.
    ''' </summary>
    ''' <param name="rtb">
    '''  The <see cref="RichTextBox"/> to append a new line to.
    ''' </param>
    <Extension>
    Friend Sub AppendNewLine(rtb As RichTextBox)
        rtb.AppendText(text:=vbCrLf)
    End Sub

    ''' <summary>
    '''  Appends <paramref name="text"/> to current text of a <see cref="RichTextBox"/>.
    ''' </summary>
    ''' <param name="rtb">
    '''  The <see cref="RichTextBox"/> to append text to.
    ''' </param>
    ''' <param name="text">
    '''  The text to append.
    ''' </param>
    ''' <param name="newFont">
    '''  The <see cref="Font"/> to use for future appended text.
    ''' </param>
    <Extension>
    Friend Sub AppendTextNewFont(rtb As RichTextBox,
                                 text As String,
                                 newFont As Font,
                                 Optional padRight As Boolean = True,
                                 Optional includeNewLine As Boolean = False)

        Dim start As Integer = rtb.TextLength
        Dim originalLength As Integer = text.Length

        If padRight Then
            Dim padLength As Integer = LeftColumnTotalWidth - originalLength
            If padLength < 0 Then
                padLength = 0
            End If

            If Debugger.IsAttached AndAlso padLength > 0 Then
                Dim nbsp As Char = Convert.ToChar(value:=160)
                text &= New String(c:=nbsp, count:=padLength)
            Else
                text = text.PadRight(totalWidth:=LeftColumnTotalWidth)
            End If
        End If

        rtb.AppendText(text)

        ' Apply the new font to the entire appended text
        rtb.Select(start, length:=text.Length)
        rtb.SelectionFont = newFont
        rtb.SelectionStart = rtb.TextLength
        rtb.SelectionLength = 0

        If includeNewLine Then
            rtb.AppendNewLine
        End If
    End Sub

    ''' <summary>
    '''  Appends text to current text in a <see cref="RichTextBox"/> with an option
    '''  to append a line break. The text is formatted with a specified <see cref="Font"/>,
    '''  and optionally highlights a specific substring with a different <see cref="Font"/>
    '''  and <see cref="Color"/>.
    ''' </summary>
    ''' <param name="rtb">The <see cref="RichTextBox"/> to append text to.</param>
    ''' <param name="text">The text to append.</param>
    ''' <param name="symbol">
    '''  The symbol to use to represent button to press with Gear, Shield, or other symbols.
    '''  Defaults to Gear symbol.
    ''' </param>
    ''' <param name="includeNewLine">
    '''  If <see langword="True"/>, appends a new line after the text.
    ''' </param>
    <Extension>
    Friend Sub AppendTextWithSymbol(rtb As RichTextBox,
                                    text As String,
                                    Optional symbol As String = Gear,
                                    Optional includeNewLine As Boolean = True)

        Dim splitText() As String = text.Split(separator:=symbol, options:=StringSplitOptions.None)
        rtb.AppendTextNewFont(text:=splitText(0), newFont:=HeadingBoldFont, padRight:=False)
        If splitText.Length > 1 Then
            Dim bufferLength As Integer = rtb.Text.Length
            rtb.AppendTextNewFont(text:=symbol, newFont:=HeadingBoldFont, padRight:=False)
            rtb.Select(start:=bufferLength, length:=symbol.Length)
            rtb.SelectionBackColor = SystemColors.Window
            Select Case symbol
                Case Gear
                    rtb.SelectionColor = Color.Yellow
                Case Shield
                    rtb.SelectionColor = Color.CadetBlue
                Case Else
                    rtb.SelectionColor = Color.HotPink
            End Select
            rtb.SelectionStart = rtb.Text.Length
            rtb.SelectionBackColor = SystemColors.Window
            rtb.SelectionColor = SystemColors.WindowText
            rtb.AppendTextNewFont(text:=splitText(1),
                                  newFont:=HeadingBoldFont,
                                  padRight:=False)
        End If
        If includeNewLine Then
            rtb.AppendNewLine
        End If
    End Sub

    ''' <summary>
    '''  Appends a time value row to the <see cref="RichTextBox"/>.
    ''' </summary>
    ''' <param name="rtb">
    '''  The <see cref="RichTextBox"/> to append the time value row to.
    ''' </param>
    ''' <param name="startTime">The start time of the row.</param>
    ''' <param name="endTime">The end time of the row.</param>
    ''' <param name="value">The value associated with the time range.</param>
    ''' <param name="indent">If true, applies a single indent to the row.</param>
    ''' <remarks>
    '''  The time values are formatted to a standard width for consistency.
    ''' </remarks>
    ''' <param name="heading"></param>
    '''
    '''
    <Extension>
    Friend Sub AppendTimeValueRow(rtb As RichTextBox,
                                  startTime As String,
                                  endTime As String,
                                  value As String,
                                  Optional indent As String = Indent8,
                                  Optional heading As Boolean = False)

        Dim timeRange As String
        Dim leftPanel As Boolean = True
        If endTime = "N/A" Then
            timeRange = $"{startTime}     {endTime}"
            leftPanel = False
        Else
            timeRange = $"{startTime} - {endTime}"
        End If
        Dim newFont As Font = If(heading, FixedWidthBoldFont, FixedWidthFont)

        rtb.AppendTextNewFont(text:=$"{indent}{timeRange}", newFont)
        Dim text As String = value.AlignCenter(leftPanel).TrimEnd
        rtb.AppendTextNewFont(text,
                              newFont:=FixedWidthFont,
                              padRight:=False,
                              includeNewLine:=True)
    End Sub

    ''' <summary>
    '''  Appends a time value row to the <see cref="RichTextBox"/> with start
    '''  and end times as <see cref="TimeOnly"/> values.
    ''' </summary>
    ''' <param name="rtb">The <see cref="RichTextBox"/> to append the time value row to.</param>
    ''' <param name="startTime">The start time of the row as a <see cref="TimeOnly"/> value.</param>
    ''' <param name="endTime">The end time of the row as a <see cref="TimeOnly"/> value.</param>
    ''' <param name="value">The value associated with the time range.</param>
    ''' <param name="indent">If true, applies a single indent to the row.</param>
    ''' <param name="heading">If true, formats the row as a heading.</param>
    <Extension>
    Friend Sub AppendTimeValueRow(rtb As RichTextBox,
                                  startTime As TimeOnly,
                                  endTime As TimeOnly,
                                  value As String,
                                  Optional indent As String = Indent8,
                                  Optional heading As Boolean = False)
        rtb.AppendTimeValueRow(startTime:=startTime.ToString(),
            endTime:=endTime.ToString(),
            value:=value,
            indent:=indent, heading:=heading)
    End Sub

    ' <summary>
    '''  Appends a time value row to the <see cref="RichTextBox"/> with start
    '''  and end times as strings.
    ''' </summary>
    ''' <param name="rtb">
    '''  The <see cref="RichTextBox"/> to append the time value row to.
    ''' </param>
    ''' <param name="key">The label for the time value row.</param>
    ''' <param name="startTime">The start time of the row as a string.</param>
    ''' <param name="endTime">The end time of the row as a string.</param>
    <Extension>
    Friend Sub AppendTimeValueRow(rtb As RichTextBox,
                                  key As String,
                                  startTime As String,
                                  Optional endTime As String = EmptyString)

        Dim text As String = $"{Indent4}{key}".PadRight(LeftColumnTotalWidth)
        rtb.AppendTextNewFont(text, newFont:=FixedWidthFont)

        Dim separator As String
        If endTime = String.Empty Then
            separator = String.Empty
        Else
            separator = "-"
            If startTime <> "Off" Then
                startTime = startTime.PadLeft(totalWidth:=8)
                endTime = endTime.PadLeft(totalWidth:=8)
            End If
        End If

        text = $"{startTime}{separator}{endTime}".AlignCenter(leftPanel:=False)
        rtb.AppendTextNewFont(text, newFont:=FixedWidthFont, includeNewLine:=True)
    End Sub

    ''' <summary>
    '''  Makes all occurrences of a specified string in a <see cref="RichTextBox"/> bold.
    ''' </summary>
    ''' <param name="rtb">
    '''  The <see cref="RichTextBox"/> to modify.
    ''' </param>
    ''' <param name="str">
    '''  The string to make bold.
    ''' </param>
    ''' <remarks>
    '''  This method searches for all occurrences of the specified string and applies bold formatting to them.
    ''' </remarks>
    <Extension>
    Public Sub BoldText(rtb As RichTextBox, str As String)
        Dim start As Integer = 0
        Dim length As Integer = str.Length
        While start < rtb.TextLength - 1
            Dim wordStartIndex As Integer =
                rtb.Find(str,
                         start,
                         [end]:=rtb.TextLength - 1,
                         options:=RichTextBoxFinds.MatchCase)

            If wordStartIndex = -1 Then
                Exit While ' No more occurrences found
            Else
                rtb.Select(start:=wordStartIndex, length)
                rtb.SelectionFont = New Font(prototype:=rtb.SelectionFont, FontStyle.Bold)
                start = wordStartIndex + length
            End If
        End While
        rtb.SelectionLength = 0
    End Sub

    ''' <summary>
    '''  Finds all occurrences of a specified string in a <see cref="RichTextBox"/>
    '''  and highlights them with bold formatting and blue color.
    ''' </summary>
    ''' <param name="rtb">
    '''  The <see cref="RichTextBox"/> to search in.
    ''' </param>
    ''' <param name="text">
    '''  The string to find and highlight.
    ''' </param>
    <Extension>
    Public Sub FindAll(rtb As RichTextBox, text As String)
        ' Validate input
        If String.IsNullOrWhiteSpace(value:=text) Then
            Return
        End If
        rtb.SelectAll()
        rtb.SelectionColor = rtb.ForeColor
        rtb.Select(start:=0, length:=0) ' Move caret to start

        Dim searchText As String = text
        Dim startIndex As Integer = 0

        While startIndex < rtb.Text.Length
            ' Find the index of the search word
            Dim wordIndex As Integer = rtb.Find(str:=searchText,
                                                start:=startIndex,
                                                options:=RichTextBoxFinds.None)
            If wordIndex = -1 Then Exit While

            ' Select the found word
            rtb.Select(start:=wordIndex, length:=searchText.Length)

            ' Apply formatting: Bold and LimeGreen
            rtb.SelectionFont =
                New Font(prototype:=rtb.SelectionFont, newStyle:=FontStyle.Bold)
            rtb.SelectionColor = Color.LimeGreen

            ' Move to the next occurrence
            startIndex = wordIndex + searchText.Length
        End While

        ' Reset selection to avoid highlighting other text
        rtb.Select(start:=rtb.Text.Length, length:=0)
    End Sub

    ''' <summary>
    '''  Searches for the next occurrence of a specified string in
    '''  a <see cref="RichTextBox"/> and highlights it.
    ''' </summary>
    ''' <param name="rtb">
    '''  The <see cref="RichTextBox"/> to search in.
    ''' </param>
    ''' <param name="text">
    '''  The string to search for.
    ''' </param>
    ''' <param name="lastSearchIndex">
    '''  The index of the last found occurrence.
    ''' </param>
    <Extension>
    Public Sub FindNext(rtb As RichTextBox, text As String, ByRef lastSearchIndex As Integer)
        ' Validate input
        If String.IsNullOrWhiteSpace(value:=text) Then
            Return
        End If

        ' Search for the text starting from the last found position
        Dim index As Integer =
            rtb.Find(str:=text,
                     start:=lastSearchIndex,
                     options:=RichTextBoxFinds.None)

        ' If not found, wrap around and search from the beginning
        If index = -1 AndAlso lastSearchIndex > 0 Then
            index = rtb.Find(str:=text,
                             start:=0,
                             options:=RichTextBoxFinds.None)
        End If

        ' Highlight if found
        If index <> -1 Then
            rtb.Select(start:=index, length:=text.Length)
            rtb.ScrollToCaret()
            lastSearchIndex = index + text.Length
            rtb.SelectionFont =
                New Font(prototype:=rtb.SelectionFont, newStyle:=FontStyle.Bold)
            rtb.SelectionColor = Color.LimeGreen
        Else
            lastSearchIndex = 0
        End If
    End Sub

End Module
