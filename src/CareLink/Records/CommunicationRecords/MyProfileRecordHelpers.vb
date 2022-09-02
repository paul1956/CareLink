Imports CareLink

' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Class MyProfileRecordHelpers
    Private Shared ReadOnly s_columnsToHide As New List(Of String)

    Friend Shared Function Epoch2String(epoch As String) As String
        If String.IsNullOrWhiteSpace(epoch) Then
            Return ""
        End If
        Dim days As Integer = CInt(Long.Parse(epoch) / 86400000)
        Return New Date(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddDays(days).ToShortDateString
    End Function

    Friend Shared Function HideColumn(dataPropertyName As String) As Boolean
        Return s_filterJsonData AndAlso MyProfileRecordHelpers.s_columnsToHide.Contains(dataPropertyName)
    End Function

    Public Shared Function GetCellStyle() As DataGridViewCellStyle
        Return New DataGridViewCellStyle().CellStyleMiddleLeft
    End Function

End Class
