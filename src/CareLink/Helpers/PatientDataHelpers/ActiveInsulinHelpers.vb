﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module ActiveInsulinHelpers

    Private ReadOnly s_columnsToHide As New List(Of String) From {
                         NameOf(ActiveInsulin.kind),
                         NameOf(ActiveInsulin.Version)
                    }

    Private s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Friend Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of ActiveInsulin)(s_alignmentTable, columnName)
    End Function

    Friend Function HideColumn(dataPropertyName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(dataPropertyName)
    End Function

End Module
