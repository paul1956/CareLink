Imports System.Runtime.CompilerServices
Imports CareLink
' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class SummaryRecordHelpers

    Public Shared Function GetCellStyle(columnName As String, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf(SummaryRecord.RecordNumber)
                cellStyle = cellStyle.CellStyleMiddleCenter
            Case NameOf(SummaryRecord.Key)
                cellStyle = cellStyle.CellStyleMiddleLeft
            Case NameOf(SummaryRecord.Value)
                cellStyle = cellStyle.CellStyleMiddleLeft
            Case Else
                Throw UnreachableException(memberName, sourceLineNumber)
        End Select
        Return cellStyle
    End Function
End Class
