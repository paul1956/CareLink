' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text
''' <summary>
''' Helper Functions. Conversion, Validation
''' </summary>
Public Module Helper
    ''' <summary>
    ''' Indicates whether a specified DataTable is null, has zero columns, or (optionally) zero rows.
    ''' </summary>
    ''' <param name="Table">DataTable to check.</param>
    ''' <param name="IgnoreRows">When set to true, the function will return true even if the table's row count is equal to zero.</param>
    ''' <returns>False if the specified DataTable null, has zero columns, or zero rows, otherwise true.</returns>
    Public Function IsValidDatatable(Table As DataTable, Optional IgnoreRows As Boolean = False) As Boolean
        If Table Is Nothing Then
            Return False
        End If
        If Table.Columns.Count = 0 Then
            Return False
        End If
        If Not IgnoreRows AndAlso Table.Rows.Count = 0 Then
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Indicates whether a specified Enumerable collection is null or an empty collection.
    ''' </summary>
    ''' <typeparam name="T">The specified type contained in the collection.</typeparam>
    ''' <param name="Input">An Enumerator to the collection to check.</param>
    ''' <returns>True if the specified Enumerable collection is null or empty, otherwise false.</returns>
    Public Function IsCollectionEmpty(Of T)(Input As IEnumerable(Of T)) As Boolean
        Return Input Is Nothing OrElse Not Input.Any()
    End Function

    ''' <summary>
    '''  Indicates whether a specified Type can be assigned null.
    ''' </summary>
    ''' <param name="Input">The Type to check for nullable property.</param>
    ''' <returns>True if the specified Type can be assigned null, otherwise false.</returns>
    Public Function IsNullableType(Input As Type) As Boolean
        If Not Input.IsValueType Then
            Return True ' Reference Type
        End If
        If Nullable.GetUnderlyingType(Input) IsNot Nothing Then
            Return True ' Nullable<T>
        End If
        Return False ' Value Type
    End Function

    ''' <summary>
    ''' Returns all the column names of the specified DataRow in a string delimited like and SQL INSERT INTO statement.
    ''' Example: ([FullName], [Gender], [BirthDate])
    ''' </summary>
    ''' <returns>A string formatted like the columns specified in an SQL 'INSERT INTO' statement.</returns>
    Public Function RowToColumnString(Row As DataRow) As String
        Dim collection As IEnumerable(Of String) = Row.ItemArray.Select(Function(item) TryCast(item, String))
        Return ListToDelimitedString(collection, "([", "], [", "])")
    End Function

    ''' <summary>
    ''' Returns all the values the specified DataRow in as a string delimited like and SQL INSERT INTO statement.
    ''' Example: ('John Doe', 'M', '10/3/1981'')
    ''' </summary>
    ''' <returns>A string formatted like the values specified in an SQL 'INSERT INTO' statement.</returns>
    Public Function RowToValueString(Row As DataRow) As String
        Dim collection As IEnumerable(Of String) = GetDatatableColumns(Row.Table).Select(Function(c) c.ColumnName)
        Return ListToDelimitedString(collection, "('", "', '", "')")
    End Function

    ''' <summary>
    ''' Enumerates a collection as delimited collection of strings.
    ''' </summary>
    ''' <typeparam name="T">The Type of the collection.</typeparam>
    ''' <param name="Collection">An Enumerator to a collection to populate the string.</param>
    ''' <param name="Prefix">The string to prefix the result.</param>
    ''' <param name="Delimiter">The string that will appear between each item in the specified collection.</param>
    ''' <param name="Postfix">The string to postfix the result.</param>         
    Public Function ListToDelimitedString(Of T)(Collection As IEnumerable(Of T), Prefix As String, Delimiter As String, Postfix As String) As String
        If IsCollectionEmpty(Of T)(Collection) Then
            Return String.Empty
        End If

        Dim result As New StringBuilder()
        For Each item As T In Collection
            If result.Length <> 0 Then
                result.Append(Delimiter) ' Add comma
            End If

            result.Append(EscapeSingleQuotes(TryCast(item, String)))
        Next item
        If result.Length < 1 Then
            Return String.Empty
        End If

        result.Insert(0, Prefix)
        result.Append(Postfix)

        Return result.ToString()
    End Function

    ''' <summary>
    ''' Returns an enumerator, which supports a simple iteration over a collection of all the DataColumns in a specified DataTable.
    ''' </summary>
    Public Function GetDatatableColumns(Input As DataTable) As IEnumerable(Of DataColumn)
        If Input Is Nothing OrElse Input.Columns.Count < 1 Then
            Return New List(Of DataColumn)()
        End If
        Return Input.Columns.OfType(Of DataColumn)().ToList()
    End Function

    ''' <summary>
    ''' Returns an enumerator, which supports a simple iteration over a collection of all the DataRows in a specified DataTable.
    ''' </summary>
    Public Function GetDatatableRows(Input As DataTable) As IEnumerable(Of DataRow)
        If Not IsValidDatatable(Input) Then
            Return New List(Of DataRow)()
        End If
        Return Input.Rows.OfType(Of DataRow)().ToList()
    End Function

    ''' <summary>
    ''' Returns a new string in which all occurrences of the single quote character in the current instance are replaced with a back-tick character.
    ''' </summary>
    Public Function EscapeSingleQuotes(Input As String) As String
        Return Input.Replace("'"c, "`"c) ' Replace with back-tick
    End Function
End Module
