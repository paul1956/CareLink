Imports System.Runtime.CompilerServices
Imports CareLink
' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class AutoBasalDeliveryRecordHelpers

    Private Shared ReadOnly s_columnsToHide As New List(Of String) From {
            NameOf(AutoBasalDeliveryRecord.id),
            NameOf(AutoBasalDeliveryRecord.index),
            NameOf(AutoBasalDeliveryRecord.kind),
            NameOf(AutoBasalDeliveryRecord.relativeOffset),
            NameOf(AutoBasalDeliveryRecord.type),
            NameOf(AutoBasalDeliveryRecord.version)
        }

    Friend Shared Function GetCellStyle(columnName As String, <CallerMemberName> Optional memberName As String = Nothing, <CallerLineNumber()> Optional sourceLineNumber As Integer = 0) As DataGridViewCellStyle
        Dim cellStyle As New DataGridViewCellStyle

        Select Case columnName
            Case NameOf(AutoBasalDeliveryRecord.[dateTime]),
                    NameOf(AutoBasalDeliveryRecord.dateTimeAsString),
                    NameOf(AutoBasalDeliveryRecord.type)
                cellStyle.CellStyleMiddleLeft
            Case NameOf(AutoBasalDeliveryRecord.RecordNumber),
                    NameOf(AutoBasalDeliveryRecord.kind),
                    NameOf(AutoBasalDeliveryRecord.index),
                    NameOf(AutoBasalDeliveryRecord.id)
                cellStyle = cellStyle.CellStyleMiddleCenter
                cellStyle.Padding = New Padding(0, 0, 0, 0)
            Case NameOf(AutoBasalDeliveryRecord.bolusAmount),
                    NameOf(AutoBasalDeliveryRecord.version),
                    NameOf(AutoBasalDeliveryRecord.OAdateTime),
                    NameOf(AutoBasalDeliveryRecord.relativeOffset)
                cellStyle = cellStyle.CellStyleMiddleRight(0)
            Case Else
                Stop
                Throw UnreachableException(memberName, sourceLineNumber)
        End Select
        Return cellStyle
    End Function

    Friend Shared Function HideColumn(dataPropertyName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(dataPropertyName)
    End Function
End Class
