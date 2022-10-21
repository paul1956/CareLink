' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Class supportedReportRecordHelpers

    Public Shared Function GetCellStyle() As DataGridViewCellStyle
        Return New DataGridViewCellStyle().SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1))
    End Function
End Class
