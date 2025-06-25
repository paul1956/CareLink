' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text

''' <summary>
'''  Provides extension methods for <see cref="DataGridViewColumnEventArgs"/> to configure DataGridView columns
'''  and for string prefix matching in lists.
''' </summary>
Friend Module DataGridViewColumnEventArgsExtensions

    ''' <summary>
    '''  Tries to find a string in the list that <paramref name="headerText"/> starts with, ignoring case.
    ''' </summary>
    ''' <param name="list">The list of strings to search.</param>
    ''' <param name="headerText">The header string to check against the list.</param>
    ''' <param name="result">
    '''  When this method returns, contains the matching string if found;
    '''  otherwise, result is unmodified.
    ''' </param>
    ''' <returns>
    '''  <see langword="True"/> if a matching string is found; otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Friend Function TryGetPrefixMatch(list As List(Of String), headerText As String, ByRef result As String) As Boolean
        For Each value As String In list
            If headerText.StartsWith(value, comparisonType:=StringComparison.OrdinalIgnoreCase) Then
                result = value
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    '''  Configures a <see cref="DataGridViewColumn"/> when it is added to a <see cref="DataGridView"/>.
    ''' </summary>
    ''' <param name="e">
    '''  The <see cref="DataGridViewColumnEventArgs"/> containing the column to configure.
    ''' </param>
    ''' <param name="cellStyle">
    '''  The <see cref="DataGridViewCellStyle"/> to apply to the column's default cell style.
    ''' </param>
    ''' <param name="forceReadOnly">
    '''  If set to <see langword="True"/>, the column will be set as read-only.
    ''' </param>
    ''' <param name="caption">
    '''  The caption to use for the column header. If empty or whitespace, the default header text is used.
    ''' </param>
    <Extension>
    Public Sub DgvColumnAdded(
        ByRef e As DataGridViewColumnEventArgs,
        cellStyle As DataGridViewCellStyle,
        forceReadOnly As Boolean,
        caption As String)

        With e.Column
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            If e.Column.Index = 0 AndAlso e.Column.DataPropertyName <> "Hour" Then
                e.Column.MinimumWidth = 150
            End If
            Dim idHeaderName As Boolean = .DataPropertyName = "ID"
            .ReadOnly = forceReadOnly OrElse idHeaderName
            .Resizable = DataGridViewTriState.False
            Dim title As New StringBuilder
            Dim titleInTitleCase As String = .Name
            If .Name.Contains("DeleteRow") Then
                titleInTitleCase = ""
            ElseIf Not .Name.ContainsIgnoreCase("OADateTime") Then
                titleInTitleCase = If(.DataPropertyName.Length < 4, .Name, .Name.ToTitleCase())
            End If

            If titleInTitleCase.Contains("™"c) Then
                title.Append(titleInTitleCase)
            Else
                title.Append(titleInTitleCase.Replace("Care Link", $"CareLink™"))
            End If

            .HeaderText = title.TrimEnd(vbCrLf).ToString
            .DefaultCellStyle = cellStyle
            If .HeaderText <> "Record Number" Then
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End If
            If String.IsNullOrWhiteSpace(caption) Then Return
            .HeaderText = caption.Replace("_", "")
            If .DataPropertyName.ContainsIgnoreCase("message") Then
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            ElseIf .DataPropertyName.EqualsIgnoreCase("RecordNumber") Then
                .SortMode = DataGridViewColumnSortMode.Automatic
                .HeaderCell.SortGlyphDirection = SortOrder.Ascending
            End If
        End With

    End Sub

End Module
