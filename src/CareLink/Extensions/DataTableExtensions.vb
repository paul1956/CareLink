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
        Return If(IsValidDataTable(Input),
                  New List(Of DataColumn)(),
                  Input.Columns.OfType(Of DataColumn)().ToList())
    End Function

    ''' <summary>
    ''' Returns an list, which supports a simple iteration over a collection of all the DataRows in a specified DataTable.
    ''' </summary>
    <Extension>
    Public Function GetRows(table As DataTable) As IEnumerable(Of DataRow)
        Return If(Not IsValidDataTable(table),
                  New List(Of DataRow)(),
                  table.Rows.OfType(Of DataRow)().ToList())
    End Function

End Module
