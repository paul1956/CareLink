' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text

''' <summary>
'''  Provides extension methods for <see cref="DataGridViewColumnEventArgs"/>
'''  to configure DataGridView columns and for string prefix matching in lists.
''' </summary>
Friend Module DgvColumnEventArgsExtensions

    ''' <summary>
    '''  Configures a <see cref="DataGridViewColumn"/> when it is added to a
    '''  <see cref="DataGridView"/>.
    ''' </summary>
    ''' <param name="e">
    '''  The <see cref="DataGridViewColumnEventArgs"/> containing the
    '''  column to configure.
    ''' </param>
    ''' <param name="cellStyle">
    '''  The <see cref="DataGridViewCellStyle"/> to apply to the
    '''  column's default cell style.
    ''' </param>
    ''' <param name="forceReadOnly">
    '''  If set to <see langword="True"/>, the column will be set as read-only.
    ''' </param>
    ''' <param name="caption">
    '''  The caption to use for the column header. If empty or whitespace,
    '''  the default header text is used.
    ''' </param>
    <Extension>
    Public Sub DgvColumnAdded(
        e As DataGridViewColumnEventArgs,
        cellStyle As DataGridViewCellStyle,
        forceReadOnly As Boolean,
        caption As String)

        With e.Column
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            .ReadOnly = forceReadOnly OrElse .DataPropertyName = "ID"
            .Resizable = DataGridViewTriState.False
            Dim title As New StringBuilder
            Dim value As String = .Name
            If .Name.Contains(value:="DeleteRow") Then
                value = ""
            ElseIf Not .Name.ContainsNoCase(value:="OADateTime") Then
                value = If(.DataPropertyName.Length < 4,
                           .Name,
                           .Name.ToTitleCase())
            End If

            If value.Contains(value:="™"c) Then
                title.Append(value)
            Else
                value = value.Replace(oldValue:="Care Link", newValue:=$"CareLink™")
                title.Append(value)
            End If

            .HeaderText = title.TrimEnd(value:=vbCrLf).ToString
            .DefaultCellStyle = cellStyle
            If String.IsNullOrWhiteSpace(value:=caption) Then Return
            .HeaderText = caption.Remove(s:="_")
            If .DataPropertyName.ContainsNoCase(value:="message") Then
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            Else
                .SortMode = DataGridViewColumnSortMode.NotSortable
            End If
        End With

    End Sub

End Module
