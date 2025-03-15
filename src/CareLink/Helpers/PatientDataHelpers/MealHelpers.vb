' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module MealHelpers

    Private ReadOnly s_alignmentTable As New Dictionary(Of String, DataGridViewCellStyle)

    Private ReadOnly s_columnsToHide As New List(Of String) From {
        NameOf(Meal.OAdateTime)}

    Friend Function GetCellStyle(columnName As String) As DataGridViewCellStyle
        Return ClassPropertiesToColumnAlignment(Of Meal)(s_alignmentTable, columnName)
    End Function

    Friend Function HideColumn(dataPropertyName As String) As Boolean
        Return s_filterJsonData AndAlso s_columnsToHide.Contains(dataPropertyName)
    End Function

    Public Function TryGetMealRecord(recordNumber As Integer, ByRef meal As Meal) As Boolean
        For Each m As Meal In s_listOfMealMarkers
            If m.RecordNumber = recordNumber Then
                meal = m
                Return True
            End If
        Next
        Return False
    End Function

End Module
