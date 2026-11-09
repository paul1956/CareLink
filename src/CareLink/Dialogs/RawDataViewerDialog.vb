' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Text.Json
Imports System.Text.Json.Nodes
Imports System.Text.RegularExpressions

Public Class RawDataViewerDialog
    Private Const EM_GETSCROLLPOS As Integer = &H4DD
    Private Const EM_SETSCROLLPOS As Integer = &H4DE

    Private Shared ReadOnly s_timestampDifference As New Regex(
        pattern:="^\$.*\[""[^""\]]*Time[^""\]]*""\] = (?<value>\d{10}|\d{13})$",
        options:=RegexOptions.IgnoreCase)

    Private Shared ReadOnly s_timestampLine As New Regex(
        pattern:="^\s*""[^""]*Time[^""]*""\s*:\s*(?<value>\d{10}|\d{13}),?$",
        options:=RegexOptions.IgnoreCase)

    Private Shared ReadOnly s_durationDifference As New Regex(
        pattern:="^\$.*\[""(?<name>[^""\]]*Duration[^""\]]*)""\] = (?<value>\d+)$",
        options:=RegexOptions.IgnoreCase)

    Private Shared ReadOnly s_durationLine As New Regex(
        pattern:="^\s*""(?<name>[^""]*Duration[^""]*)""\s*:\s*(?<value>\d+),?$",
        options:=RegexOptions.IgnoreCase)

    Private Const MaxDurationMinutes As Long = 14L * 24L * 60L

    Private ReadOnly _currentWordWrap As Boolean
    Private ReadOnly _previousScrollBars As RichTextBoxScrollBars
    Private _contextMenu As New ContextMenuStrip
    Private _copyMenuItem As ToolStripMenuItem
    Private _currentBounds As Rectangle
    Private _currentDifferences As String
    Private _currentText As String
    Private _hasDifferences As Boolean
    Private _lastSearchIndex As Integer = 0
    Private _previousDifferences As String
    Private _previousText As String
    Private _synchronizingScroll As Boolean
    Private _updatingDataView As Boolean = True

    ''' <summary>
    '''  Initializes a new instance of the <see cref="RawDataViewerDialog"/> class
    '''  with the specified JSON data.
    ''' </summary>
    ''' <param name="json">
    '''  The JSON data to display.
    ''' </param>
    Public Sub New(prevJson As JsonElement, json As JsonElement)
        Me.InitializeComponent()
        Me.PreviousRawDataRTB.WordWrap = False
        Me.RawDataRTB.WordWrap = False
        _currentWordWrap = False
        _previousScrollBars = Me.PreviousRawDataRTB.ScrollBars
        _currentBounds = Me.RawDataRTB.Bounds
        _updatingDataView = False
        Me.LoadData(prevJson, json)
    End Sub

    Private Shared ReadOnly Property Provider As CultureInfo = CultureInfo.InvariantCulture

    ''' <summary>
    '''  Appends the local time after Unix timestamp values (seconds or milliseconds)
    '''  of elements whose name contains "Time".
    ''' </summary>
    Private Shared Function AnnotateTimestamps(text As String) As String
        If String.IsNullOrEmpty(value:=text) Then
            Return text
        End If

        Dim lines() As String = text.Split(separator:=vbLf)
        For index As Integer = 0 To lines.Length - 1
            Dim line As String = lines(index)
            Dim trailingReturn As String = If(line.EndsWith(value:=vbCr), vbCr, "")
            Dim body As String = line.Substring(startIndex:=0, length:=line.Length - trailingReturn.Length)
            Dim m As Match = s_timestampLine.Match(input:=body)
            If Not m.Success Then
                m = s_timestampDifference.Match(input:=body)
            End If

            If Not m.Success Then
                Dim duration As String = FormatDuration(body)
                If duration.Length > 0 Then
                    lines(index) = $"{body}  // {duration}{trailingReturn}"
                End If
                Continue For
            End If

            If m.Success Then
                Dim digits As String = m.Groups(groupname:="value").Value
                Dim raw As Long = Long.Parse(s:=digits, Provider)
                Dim local As DateTimeOffset
                Try
                    local = If(digits.Length = 13,
                               DateTimeOffset.FromUnixTimeMilliseconds(milliseconds:=raw),
                               DateTimeOffset.FromUnixTimeSeconds(seconds:=raw)).ToLocalTime()
                Catch ex As ArgumentOutOfRangeException
                    Continue For
                End Try
                Dim timeFormat As String = If(PatientData?.TimeFormat = "HR_12", "yyyy-MM-dd h:mm:ss tt", "yyyy-MM-dd HH:mm:ss")
                lines(index) = $"{body}  // {local.ToString(format:=timeFormat, formatProvider:=CultureInfo.CurrentCulture)}{trailingReturn}"
            End If
        Next
        Return String.Join(separator:=vbLf, value:=lines)
    End Function

    ''' <summary>
    '''  Returns a readable duration for a line holding a numeric "Duration" element,
    '''  or an empty string. Names containing "Hour" are hours, otherwise minutes.
    '''  Zero values and values over 14 days are not annotated.
    ''' </summary>
    Private Shared Function FormatDuration(body As String) As String
        Dim m As Match = s_durationLine.Match(input:=body)
        If Not m.Success Then
            m = s_durationDifference.Match(input:=body)
        End If
        If Not m.Success Then
            Return ""
        End If

        Dim raw As Long
        If Not Long.TryParse(s:=m.Groups(groupname:="value").Value, style:=NumberStyles.None, Provider, result:=raw) Then
            Return ""
        End If

        Dim name As String = m.Groups(groupname:="name").Value
        Dim isHours As Boolean = name.Contains(value:="Hour", comparisonType:=StringComparison.OrdinalIgnoreCase) AndAlso
                                 Not name.Contains(value:="Minute", comparisonType:=StringComparison.OrdinalIgnoreCase)
        Dim totalMinutes As Long = If(isHours, raw * 60L, raw)
        If totalMinutes <= 0 OrElse totalMinutes > MaxDurationMinutes OrElse raw > MaxDurationMinutes Then
            Return ""
        End If

        Dim days As Long = totalMinutes \ 1440L
        Dim remainder As Long = totalMinutes Mod 1440L
        Dim dayText As String = If(days > 0, $"{days} {If(days = 1, "day", "days")}", "")
        If remainder = 0 Then
            Return dayText
        End If
        Dim rest As String = remainder.ToHoursMinutes()
        Return If(days > 0, $"{dayText}, {rest}", rest)
    End Function

    Private Shared Sub CollectValues(node As JsonNode, path As String, values As Dictionary(Of String, JsonNode))
        If TypeOf node Is JsonObject AndAlso DirectCast(node, JsonObject).Count > 0 Then
            For Each entry As KeyValuePair(Of String, JsonNode) In DirectCast(node, JsonObject)
                CollectValues(node:=entry.Value, path:=$"{path}[{entry.Key.ToJson}]", values)
            Next
        ElseIf TypeOf node Is JsonArray AndAlso DirectCast(node, JsonArray).Count > 0 Then
            Dim array As JsonArray = DirectCast(node, JsonArray)
            For index As Integer = 0 To array.Count - 1
                CollectValues(node:=array(index), path:=$"{path}[{index.ToString(Provider)}]", values)
            Next
        Else
            values.Add(key:=path, value:=node)
        End If
    End Sub

    Private Shared Function FormatDifference(path As String, value As JsonNode) As String
        Return $"{path} = {If(value Is Nothing, "null", value.ToJsonString())}"
    End Function

    Private Shared Sub RemoveMatchingArrayValues(previous As JsonNode, current As JsonNode, path As String,
                                                 previousValues As Dictionary(Of String, JsonNode),
                                                 currentValues As Dictionary(Of String, JsonNode))
        If TypeOf previous Is JsonObject AndAlso TypeOf current Is JsonObject Then
            Dim currentObject As JsonObject = DirectCast(current, JsonObject)
            For Each entry As KeyValuePair(Of String, JsonNode) In DirectCast(previous, JsonObject)
                Dim currentValue As JsonNode = Nothing
                If currentObject.TryGetPropertyValue(propertyName:=entry.Key, jsonNode:=currentValue) Then
                    RemoveMatchingArrayValues(previous:=entry.Value,
                                              current:=currentValue,
                                              path:=$"{path}[{entry.Key.ToJson}]",
                                              previousValues, currentValues)
                End If
            Next
        ElseIf TypeOf previous Is JsonArray AndAlso TypeOf current Is JsonArray Then
            Dim previousArray As JsonArray = DirectCast(previous, JsonArray)
            Dim currentArray As JsonArray = DirectCast(current, JsonArray)
            Dim previousMatched As New HashSet(Of Integer)
            Dim currentMatched As New HashSet(Of Integer)
            Dim nextCurrentIndex As Integer = 0

            For previousIndex As Integer = 0 To previousArray.Count - 1
                ' Continue after the last match first, since array entries usually shift together.
                For offset As Integer = 0 To currentArray.Count - 1
                    Dim currentIndex As Integer = CInt((CLng(nextCurrentIndex) + offset) Mod currentArray.Count)
                    If Not currentMatched.Contains(item:=currentIndex) AndAlso
                       JsonNode.DeepEquals(previousArray(index:=previousIndex), currentArray(index:=currentIndex)) Then
                        previousMatched.Add(item:=previousIndex)
                        currentMatched.Add(item:=currentIndex)
                        nextCurrentIndex = (currentIndex + 1) Mod currentArray.Count
                        Dim previousPath As String = $"{path}[{previousIndex.ToString(Provider)}]"
                        Dim currentPath As String = $"{path}[{currentIndex.ToString(Provider)}]"
                        Dim matchedPrevious As New Dictionary(Of String, JsonNode)(Comparer)
                        Dim matchedCurrent As New Dictionary(Of String, JsonNode)(Comparer)
                        CollectValues(node:=previousArray(index:=previousIndex), path:=previousPath, values:=matchedPrevious)
                        CollectValues(node:=currentArray(index:=currentIndex), path:=currentPath, values:=matchedCurrent)
                        For Each key As String In matchedPrevious.Keys
                            previousValues.Remove(key)
                        Next
                        For Each key As String In matchedCurrent.Keys
                            currentValues.Remove(key)
                        Next
                        Exit For
                    End If
                Next
            Next

            For index As Integer = 0 To Math.Min(previousArray.Count, currentArray.Count) - 1
                If Not previousMatched.Contains(item:=index) AndAlso Not currentMatched.Contains(item:=index) Then
                    RemoveMatchingArrayValues(previous:=previousArray(index), current:=currentArray(index),
                                              path:=$"{path}[{index.ToString(Provider)}]",
                                              previousValues:=previousValues, currentValues:=currentValues)
                End If
            Next
        End If
    End Sub

    <DllImport("user32.dll", EntryPoint:="SendMessageW")>
    Private Shared Function SendScrollMessage(handle As IntPtr, message As Integer, wParam As IntPtr, ByRef position As Point) As IntPtr
    End Function

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

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Dim current As JsonElement = PatientDataElement
        If current.ValueKind = JsonValueKind.Undefined OrElse current.ValueKind = JsonValueKind.Null Then
            MessageBox.Show(owner:=Me,
                            text:="No current patient data is available to refresh.",
                            caption:="Raw Data Viewer",
                            buttons:=MessageBoxButtons.OK,
                            icon:=MessageBoxIcon.Information)
            Return
        End If

        Me.LoadData(prevJson:=PreviousPatientDataElement, json:=current)
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

    Private Sub CheckBoxDifferenceOnly_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxDifferenceOnly.CheckedChanged
        If Not _updatingDataView Then
            Me.UpdateDataView()
        End If
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

    Private Sub Data_VScroll(sender As Object, e As EventArgs) Handles PreviousRawDataRTB.VScroll, RawDataRTB.VScroll
        Dim source As RichTextBox = DirectCast(sender, RichTextBox)
        Dim target As RichTextBox = If(source Is Me.RawDataRTB, Me.PreviousRawDataRTB, Me.RawDataRTB)
        Me.SynchronizeScroll(source, target)
    End Sub

    ''' <summary>
    '''  Handles the Click event of the "Find All" button, finding all occurrences of the search text in the RichTextBox.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub FindAll(sender As Object, e As EventArgs) Handles btnFindAll.Click
        Dim rtb As RichTextBox = Me.RawDataRTB
        rtb.FindAll(Me.txtFind.Text)
        Me.SynchronizeScroll(source:=Me.RawDataRTB, target:=Me.PreviousRawDataRTB)
    End Sub

    ''' <summary>
    '''  Handles the Click event of the "Find Next" button, finding the next occurrence of the search text in the RichTextBox.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    Private Sub FindNext(sender As Object, e As EventArgs) Handles btnFindNext.Click
        Dim rtb As RichTextBox = Me.RawDataRTB
        rtb.FindNext(Me.txtFind.Text, reportNotFound:=True, lastSearchIndex:=_lastSearchIndex)
        Me.SynchronizeScroll(source:=Me.RawDataRTB, target:=Me.PreviousRawDataRTB)
    End Sub

    Private Sub LoadData(prevJson As JsonElement, json As JsonElement)
        Dim rootNode As JsonNode = JsonNode.Parse(json:=json.ToString())
        _currentText = AnnotateTimestamps(text:=rootNode.ToJson)
        _previousText = ""
        _previousDifferences = ""
        _currentDifferences = ""
        _hasDifferences = False
        If Not prevJson.IsEmpty Then
            Dim prevRootNode As JsonNode = JsonNode.Parse(json:=prevJson.ToString())
            _previousText = AnnotateTimestamps(text:=prevRootNode.ToJson)

            Dim previousValues As New Dictionary(Of String, JsonNode)(Comparer)
            Dim currentValues As New Dictionary(Of String, JsonNode)(Comparer)
            CollectValues(node:=prevRootNode, path:="$", values:=previousValues)
            CollectValues(node:=rootNode, path:="$", values:=currentValues)
            RemoveMatchingArrayValues(previous:=prevRootNode,
                                      current:=rootNode,
                                      path:="$",
                                      previousValues,
                                      currentValues)

            Dim lastPreviousSgsPrefix As String = Nothing
            Dim previousRoot As JsonObject = TryCast(prevRootNode, JsonObject)
            Dim previousSgs As JsonArray = Nothing
            If previousRoot IsNot Nothing Then
                previousSgs = TryCast(previousRoot(propertyName:="sgs"), JsonArray)
            End If
            If previousSgs IsNot Nothing AndAlso previousSgs.Count > 0 Then
                lastPreviousSgsPrefix = $"$[""sgs""][{(previousSgs.Count - 1).ToString(Provider)}]"
            End If

            Dim rows As New List(Of DifferenceRow)
            For Each entry As KeyValuePair(Of String, JsonNode) In previousValues
                Dim currentValue As JsonNode = Nothing
                Dim existsInCurrent As Boolean = currentValues.TryGetValue(entry.Key, value:=currentValue)
                ' The last previous sgs element with no counterpart in the current data is not shown.
                If Not existsInCurrent AndAlso lastPreviousSgsPrefix IsNot Nothing AndAlso
                   (entry.Key = lastPreviousSgsPrefix OrElse
                    entry.Key.StartsWith(value:=$"{lastPreviousSgsPrefix}[", ComparisonType)) Then
                    Continue For
                End If
                If Not existsInCurrent OrElse Not JsonNode.DeepEquals(entry.Value, currentValue) Then
                    Dim currentLine As String =
                        If(existsInCurrent,
                           FormatDifference(path:=entry.Key, value:=currentValue),
                           "")
                    rows.Add(item:=New DifferenceRow(path:=entry.Key,
                                                     sequence:=rows.Count,
                                                     previousLine:=FormatDifference(path:=entry.Key, entry.Value),
                                                     currentLine))
                End If
            Next
            For Each entry As KeyValuePair(Of String, JsonNode) In currentValues
                If Not previousValues.ContainsKey(entry.Key) Then
                    rows.Add(item:=New DifferenceRow(path:=entry.Key,
                                                     sequence:=rows.Count,
                                                     previousLine:="",
                                                     currentLine:=FormatDifference(path:=entry.Key, entry.Value)))
                End If
            Next

            ' Keep groups in first-appearance order, but list lower array indexes above higher ones.
            Dim groupOrder As New Dictionary(Of String, Integer)(StringComparer.Ordinal)
            For Each row As DifferenceRow In rows
                If Not groupOrder.ContainsKey(key:=row.Group) Then
                    groupOrder.Add(key:=row.Group, value:=groupOrder.Count)
                End If
            Next
            Dim comparison As Comparison(Of DifferenceRow) =
                Function(x, y)
                    Dim result As Integer =
                        groupOrder(key:=x.Group).CompareTo(value:=groupOrder(key:=y.Group))
                    If result <> 0 Then
                        Return result
                    End If
                    For i As Integer = 0 To Math.Min(x.Indexes.Length, y.Indexes.Length) - 1
                        result = x.Indexes(i).CompareTo(value:=y.Indexes(i))
                        If result <> 0 Then
                            Return result
                        End If
                    Next
                    Return x.Sequence.CompareTo(value:=y.Sequence)
                End Function

            rows.Sort(comparison)

            Dim previousLines As List(Of String) =
                rows.Select(selector:=Function(r)
                                          Return r.PreviousLine
                                      End Function).ToList()
            Dim currentLines As List(Of String) =
                rows.Select(selector:=Function(r)
                                          Return r.CurrentLine
                                      End Function).ToList()

            Dim separator As String = Environment.NewLine
            _previousDifferences = AnnotateTimestamps(text:=String.Join(separator, values:=previousLines))
            _currentDifferences = AnnotateTimestamps(text:=String.Join(separator, values:=currentLines))
            _hasDifferences = previousLines.Count > 0
        End If
        Me.UpdateDataView()
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

    Private Sub SynchronizeScroll(source As RichTextBox, target As RichTextBox)
        If _synchronizingScroll OrElse _updatingDataView OrElse Not _hasDifferences OrElse
           Not source.IsHandleCreated OrElse Not target.IsHandleCreated Then
            Return
        End If

        _synchronizingScroll = True
        Try
            Dim sourcePosition As Point
            Dim targetPosition As Point
            SendScrollMessage(source.Handle, message:=EM_GETSCROLLPOS, wParam:=IntPtr.Zero, position:=sourcePosition)
            SendScrollMessage(target.Handle, message:=EM_GETSCROLLPOS, wParam:=IntPtr.Zero, position:=targetPosition)
            If targetPosition.Y <> sourcePosition.Y Then
                targetPosition.Y = sourcePosition.Y
                SendScrollMessage(target.Handle, message:=EM_SETSCROLLPOS, wParam:=IntPtr.Zero, position:=targetPosition)
            End If
        Finally
            _synchronizingScroll = False
        End Try
    End Sub

    Private Sub UpdateDataView()
        _updatingDataView = True
        Me.SuspendLayout()
        Try
            If _hasDifferences Then
                If Me.RawDataRTB.Dock <> DockStyle.None Then
                    Me.RawDataRTB.Dock = DockStyle.None
                    Me.RawDataRTB.Bounds = _currentBounds
                End If
                Me.PreviousRawDataRTB.Visible = True
                Me.PreviousRawDataRTB.WordWrap = False
                Me.RawDataRTB.WordWrap = Not Me.CheckBoxDifferenceOnly.Checked AndAlso _currentWordWrap
                Me.PreviousRawDataRTB.ScrollBars =
                    If(Me.CheckBoxDifferenceOnly.Checked,
                       RichTextBoxScrollBars.Horizontal,
                       _previousScrollBars)
                Me.PreviousRawDataRTB.Text =
                    If(Me.CheckBoxDifferenceOnly.Checked,
                       _previousDifferences,
                       _previousText)
                Me.RawDataRTB.Text =
                    If(Me.CheckBoxDifferenceOnly.Checked,
                       _currentDifferences,
                       _currentText)
            Else
                Me.PreviousRawDataRTB.Visible = False
                Me.PreviousRawDataRTB.WordWrap = False
                Me.PreviousRawDataRTB.ScrollBars = _previousScrollBars
                Me.RawDataRTB.WordWrap = _currentWordWrap
                Me.RawDataRTB.Text = _currentText
                Me.RawDataRTB.Dock = DockStyle.Top
            End If
            _lastSearchIndex = 0
            Me.txtFind.Clear()
        Finally
            Me.ResumeLayout()
            _updatingDataView = False
        End Try
        Me.SynchronizeScroll(source:=Me.RawDataRTB, target:=Me.PreviousRawDataRTB)
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
            Me.SynchronizeScroll(source:=Me.RawDataRTB, target:=Me.PreviousRawDataRTB)
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
        Me.SynchronizeScroll(source:=Me.RawDataRTB, target:=Me.PreviousRawDataRTB)
    End Sub

    Private NotInheritable Class DifferenceRow
        Private Shared ReadOnly s_arrayIndex As New Regex("\[(\d+)\]", RegexOptions.CultureInvariant)

        Public Sub New(path As String, sequence As Integer, previousLine As String, currentLine As String)
            Me.Sequence = sequence
            Me.PreviousLine = previousLine
            Me.CurrentLine = currentLine
            Dim matches As MatchCollection = s_arrayIndex.Matches(path)
            Me.Indexes = matches.Select(
                selector:=Function(m)
                              Dim index As Long = Long.MaxValue
                              Long.TryParse(s:=m.Groups(groupnum:=1).Value, style:=NumberStyles.None, Provider, result:=index)
                              Return index
                          End Function).ToArray()
            Me.Group = If(matches.Count > 0, path.Substring(0, matches(0).Index), path)
        End Sub

        Public ReadOnly Property CurrentLine As String
        Public ReadOnly Property Group As String
        Public ReadOnly Property Indexes As Long()
        Public ReadOnly Property PreviousLine As String
        Public ReadOnly Property Sequence As Integer
    End Class
End Class
