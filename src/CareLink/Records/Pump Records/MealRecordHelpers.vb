' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module MealRecordHelpers

    Private ReadOnly columnsToHide As New List(Of String) From {
                                NameOf(MealRecord.kind),
                                NameOf(MealRecord.relativeOffset),
                                NameOf(AutoBasalDeliveryRecord.OA_dateTime),
                                NameOf(MealRecord.version)
                            }

    Private s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Friend Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of MealRecord)(s_alignmentTable, columnName)
    End Function

    Friend Function HideColumn(dataPropertyName As String) As Boolean
        Return s_filterJsonData AndAlso columnsToHide.Contains(dataPropertyName)
    End Function

    Public Function TryGetMealRecord(index As Integer, ByRef meal As MealRecord) As Boolean
        For Each m As MealRecord In s_listOfMealMarkers
            If m.index = index Then
                meal = m
                Return True
            End If
        Next
        Return False
    End Function

End Module
