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

    Public Shared Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf(InsulinRecord.[dateTime]),
                 NameOf(InsulinRecord.dateTimeAsString),
                 NameOf(InsulinRecord.OAdateTime),
                 NameOf(InsulinRecord.type),
                 NameOf(InsulinRecord.activationType)
                cellStyle = cellStyle.CellStyleMiddleLeft
            Case NameOf(InsulinRecord.RecordNumber),
                 NameOf(InsulinRecord.kind),
                 NameOf(InsulinRecord.index),
                 NameOf(InsulinRecord.id),
                 NameOf(InsulinRecord.completed),
                 NameOf(InsulinRecord.bolusType)
                cellStyle = cellStyle.CellStyleMiddleCenter
            Case NameOf(InsulinRecord.version),
                 NameOf(InsulinRecord.programmedExtendedAmount),
                 NameOf(InsulinRecord.relativeOffset),
                 NameOf(InsulinRecord.deliveredExtendedAmount),
                 NameOf(InsulinRecord.programmedFastAmount),
                 NameOf(InsulinRecord.programmedDuration),
                 NameOf(InsulinRecord.deliveredFastAmount),
                 NameOf(InsulinRecord.effectiveDuration)
                cellStyle = cellStyle.CellStyleMiddleRight(0)
            Case Else
                Stop
                Throw UnreachableException()
        End Select
        Return cellStyle
    End Function

    Friend Shared Function HideColumn(columnName As String) As Boolean
        Return s_filterJsonData AndAlso columnsToHide.Contains(columnName)
    End Function
End Class
