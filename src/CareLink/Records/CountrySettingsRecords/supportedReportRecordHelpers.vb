' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class supportedReportRecordHelpers
    Private Shared s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Friend Shared Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToCoumnAlignment(Of supportedReportRecord)(s_alignmentTable, columnName)
    End Function

End Class
