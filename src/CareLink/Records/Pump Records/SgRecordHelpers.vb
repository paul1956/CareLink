Imports System.Runtime.CompilerServices
Imports CareLink

' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class SgRecordHelpers

    Private Shared ReadOnly s_columnsToHide As New List(Of String) From {
                        NameOf(SgRecord.OAdatetime),
                        NameOf(SgRecord.kind),
                        NameOf(SgRecord.relativeOffset),
                        NameOf(SgRecord.version)
                    }

    Friend Shared Function HideColumn(dataPropertyName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(dataPropertyName)
    End Function

    Public Shared Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf(SgRecord.sg),
                    NameOf(SgRecord.relativeOffset),
                    NameOf(SgRecord.OAdatetime)
                cellStyle = cellStyle.CellStyleMiddleRight(10)
            Case NameOf(SgRecord.RecordNumber),
                    NameOf(SgRecord.version)
                cellStyle = cellStyle.CellStyleMiddleCenter()
            Case NameOf(SgRecord.datetime),
                    NameOf(SgRecord.datetimeAsString),
                    NameOf(SgRecord.sensorState)
                cellStyle = cellStyle.CellStyleMiddleLeft
            Case NameOf(SgRecord.timeChange),
                    NameOf(SgRecord.kind)
                cellStyle = cellStyle.CellStyleMiddleCenter
            Case Else
                Throw UnreachableException()
        End Select
        Return cellStyle
    End Function

End Class
