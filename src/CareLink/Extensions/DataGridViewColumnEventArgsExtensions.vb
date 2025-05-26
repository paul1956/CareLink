' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text

Friend Module DataGridViewColumnEventArgsExtensions

    <Extension>
    Public Sub DgvColumnAdded(ByRef e As DataGridViewColumnEventArgs, cellStyle As DataGridViewCellStyle, forceReadOnly As Boolean, caption As String)
        With e.Column
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            If (e.Column.Index = 0 AndAlso e.Column.DataPropertyName <> "Hour") OrElse
               (e.Column.DataGridView.Name = "DgvLastSensorGlucose" AndAlso e.Column.HeaderText = "Sensor Glucose (sg)") Then
                e.Column.MinimumWidth = 150
            End If
            Dim idHeaderName As Boolean = .DataPropertyName = "ID"
            .ReadOnly = forceReadOnly OrElse idHeaderName
            .Resizable = DataGridViewTriState.False
            Dim title As New StringBuilder
            Dim titleInTitleCase As String = .Name
            If .Name.Contains("DeleteRow") Then
                titleInTitleCase = ""
            ElseIf Not .Name.Contains("OADateTime", StringComparison.InvariantCultureIgnoreCase) Then
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
            If .DataPropertyName.Contains("message", StringComparison.OrdinalIgnoreCase) Then
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            ElseIf .DataPropertyName.Equals("RecordNumber", StringComparison.OrdinalIgnoreCase) Then
                .SortMode = DataGridViewColumnSortMode.Automatic
                .HeaderCell.SortGlyphDirection = SortOrder.Ascending
            End If
        End With

    End Sub

End Module
