' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class AutoBasalDeliveryRecordHelpers

    Private Shared ReadOnly columnsToHide As New List(Of String) From {
            NameOf(AutoBasalDeliveryRecord.id),
            NameOf(AutoBasalDeliveryRecord.index),
            NameOf(AutoBasalDeliveryRecord.kind),
            NameOf(AutoBasalDeliveryRecord.relativeOffset),
            NameOf(AutoBasalDeliveryRecord.version)
        }

    Private Shared s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Friend Shared Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToCoumnAlignment(Of AutoBasalDeliveryRecord)(s_alignmentTable, columnName)
    End Function

    Friend Shared Function HideColumn(dataPropertyName As String) As Boolean
        Return s_filterJsonData AndAlso columnsToHide.Contains(dataPropertyName)
    End Function

End Class
