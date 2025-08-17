' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module DataTableExtensions

    ''' <summary>
    '''  Returns an enumerable collection of all <see cref="DataColumn"/>
    '''  objects in the specified <see cref="DataTable"/>.
    ''' </summary>
    ''' <param name="table">
    '''  The <see cref="DataTable"/> whose columns are to be enumerated.
    ''' </param>
    ''' <returns>
    '''  An <see cref="IEnumerable(Of DataColumn)"/> containing all columns
    '''  in the <paramref name="table"/> table, or an empty list if the table is not valid.
    ''' </returns>
    <Extension>
    Public Function GetColumns(table As DataTable) As IEnumerable(Of DataColumn)
        Return If(IsValidDataTable(table),
                  New List(Of DataColumn)(),
                  table.Columns.OfType(Of DataColumn)().ToList())
    End Function

    ''' <summary>
    '''  Returns an enumerable collection of all <see cref="DataRow"/>
    '''  objects in the specified <see cref="DataTable"/>.
    ''' </summary>
    ''' <param name="table">
    '''  The <see cref="DataTable"/> whose rows are to be enumerated.
    ''' </param>
    ''' <returns>
    '''  An <see cref="IEnumerable(Of DataRow)"/> containing all rows in
    '''  the <paramref name="table"/>, or an empty list if the table is not valid.
    ''' </returns>
    <Extension>
    Public Function GetRows(table As DataTable) As IEnumerable(Of DataRow)
        Return If(Not IsValidDataTable(table),
                  New List(Of DataRow)(),
                  table.Rows.OfType(Of DataRow)().ToList())
    End Function

End Module
