' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module DataTableExtensions

    ''' <summary>
    ''' Returns an enumerator, which supports a simple iteration over a collection of all the DataColumns in a specified DataTable.
    ''' </summary>
    <Extension>
    Public Function GetColumns(Input As DataTable) As IEnumerable(Of DataColumn)
        If IsValidDataTable(Input) Then
            Return New List(Of DataColumn)()
        End If
        Return Input.Columns.OfType(Of DataColumn)().ToList()
    End Function

    ''' <summary>
    ''' Returns an list, which supports a simple iteration over a collection of all the DataRows in a specified DataTable.
    ''' </summary>
    <Extension>
    Public Function GetRows(Input As DataTable) As IEnumerable(Of DataRow)
        If Not IsValidDataTable(Input) Then
            Return New List(Of DataRow)()
        End If
        Return Input.Rows.OfType(Of DataRow)().ToList()
    End Function

End Module
