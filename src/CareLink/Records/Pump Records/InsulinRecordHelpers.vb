' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class InsulinRecordHelpers

    Private Shared ReadOnly columnsToHide As New List(Of String) From {
            NameOf(InsulinRecord.id),
            NameOf(InsulinRecord.index),
            NameOf(InsulinRecord.kind),
            NameOf(InsulinRecord.OAdateTime),
            NameOf(InsulinRecord.relativeOffset),
            NameOf(InsulinRecord.type),
            NameOf(InsulinRecord.version)
        }

    Private Shared s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Friend Shared Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToCoumnAlignment(Of InsulinRecord)(s_alignmentTable, columnName)
    End Function

    Friend Shared Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso columnsToHide.Contains(columnName)
    End Function

End Class
