' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Reflection
Imports System.Runtime.CompilerServices

''' <summary>
''' DataTable/Class Mapping Class
''' </summary>
Friend Module DataTableHelpers

    ''' <summary>
    ''' Adds a DataRow to a DataTable from the public properties of a class.
    ''' </summary>
    ''' <param propertyName="Table">A reference to the DataTable to insert the DataRow into.</param>
    ''' <param propertyName="ClassObject">The class containing the data to fill the DataRow from.</param>
    <Extension>
    Private Sub Add(Of T As Class)(ByRef Table As DataTable, ClassObject As T)
        Dim row As DataRow = Table.NewRow()
        For Each [property] As PropertyInfo In GetType(T).GetProperties()
            If Table.Columns.Contains([property].Name) Then
                If Table.Columns([property].Name) IsNot Nothing Then
                    row([property].Name) = [property].GetValue(ClassObject, Nothing)
                End If
            End If
        Next [property]
        Table.Rows.Add(row)
    End Sub

    ''' <summary>
    ''' Creates a DataTable from a class type's public properties. The DataColumns of the table will match the propertyName and type of the public properties.
    ''' </summary>
    ''' <typeparam propertyName="T">The type of the class to create a DataTable from.</typeparam>
    ''' <returns>A DataTable who's DataColumns match the propertyName and type of each class T's public properties.</returns>
    Private Function ClassToDataTable(Of T As Class)() As DataTable
        Dim classType As Type = GetType(T)
        Dim result As New DataTable(classType.UnderlyingSystemType.Name)
        Dim propertyOrder As New SortedDictionary(Of Integer, PropertyInfo)
        For Each [property] As PropertyInfo In classType.GetProperties()
            Dim colAttribute As ColumnAttribute = [property].GetCustomAttributes(GetType(ColumnAttribute), True).Cast(Of ColumnAttribute)().SingleOrDefault()
            propertyOrder.Add(colAttribute.Order, [property])
        Next
        For Each [property] As PropertyInfo In propertyOrder.Values
            Dim propertyType As Type = [property].PropertyType
            If propertyType = GetType(Boolean) Then
                propertyType = GetType(String)
            End If
            Dim column As New DataColumn With {
                                .ColumnName = [property].Name,
                                .Caption = GetColumnDisplayName([property]),
                                .DataType = propertyType
                            }
            If IsNullableType(column.DataType) AndAlso column.DataType.IsGenericType Then
                ' If Nullable<>, this is how we get the underlying Type...
                column.DataType = column.DataType.GenericTypeArguments.FirstOrDefault()
            Else ' True by default, so set it false
                'column.AllowDBNull = False
            End If

            ' Add column
            result.Columns.Add(column)
        Next [property]
        Return result
    End Function

    Private Function GetColumnDisplayName([property] As PropertyInfo) As String
        Dim displayNameAttribute As DisplayNameAttribute = [property].GetCustomAttributes(GetType(DisplayNameAttribute), True).Cast(Of DisplayNameAttribute)().SingleOrDefault()
        Return If(displayNameAttribute Is Nothing, [property].Name, displayNameAttribute.DisplayName)
    End Function

    ''' <summary>
    '''  Indicates whether a specified Type can be assigned null.
    ''' </summary>
    ''' <param name="Input">The Type to check for nullable property.</param>
    ''' <returns>True if the specified Type can be assigned null, otherwise false.</returns>
    Private Function IsNullableType(Input As Type) As Boolean
        If Not Input.IsValueType Then
            Return True ' Reference Type
        End If
        If Nullable.GetUnderlyingType(Input) IsNot Nothing Then
            Return True ' Nullable<T>
        End If
        Return False ' Value Type
    End Function

    ''' <summary>
    ''' Creates a DataTable from a class type's public properties and adds a new DataRow to the table for each class passed as a parameter.
    ''' The DataColumns of the table will match the name and type of the public properties.
    ''' </summary>
    ''' <param name="ClassCollection">A class or array of class to fill the DataTable with.</param>
    ''' <returns>A DataTable who's DataColumns match the name and type of each class T's public properties.</returns>
    Public Function ClassCollectionToDataTable(Of T As Class)(classCollection As List(Of T)) As DataTable
        Dim result As DataTable = ClassToDataTable(Of T)()

        If Not IsValidDataTable(result, IgnoreRows:=True) Then
            Return New DataTable()
        End If
        If classCollection?.Count > 0 Then
            For Each classObject As T In classCollection
                result.Add(classObject)
            Next classObject
        End If

        Return result
    End Function

    ''' <summary>
    ''' Created a Dictionary that maps Class Property Name to Column Alignment
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <returns>Dictionary</returns>
    Public Function ClassPropertiesToColumnAlignment(Of T As Class)(ByRef alignmentTable As Dictionary(Of String, DataGridViewCellStyle), columnName As String) As DataGridViewCellStyle
        Dim classType As Type = GetType(T)
        Dim cellStyle As New DataGridViewCellStyle
        If alignmentTable.Count = 0 Then
            For Each [property] As PropertyInfo In classType.GetProperties()
                cellStyle = New DataGridViewCellStyle
                Select Case [property].GetCustomAttributes(GetType(ColumnAttribute), True).Cast(Of ColumnAttribute)().SingleOrDefault().TypeName
                    Case "Date", NameOf(OADate), NameOf([String]), NameOf(SummaryRecord.RecordNumber)
                        cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1))
                    Case NameOf([Decimal]), NameOf([Double]), NameOf([Int32]), NameOf([Single]), NameOf([TimeSpan])
                        cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleRight, New Padding(0, 1, 1, 1))
                    Case NameOf([Boolean])
                        cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleCenter, New Padding(0))
                    Case Else
                        Throw UnreachableException($"{NameOf(DataTableHelpers)}.{NameOf(ClassPropertiesToColumnAlignment)} [property].PropertyType.Name = {[property].PropertyType.Name}")
                End Select
                alignmentTable.Add([property].Name, cellStyle)
            Next
        End If
        If Not alignmentTable.TryGetValue(columnName, cellStyle) Then
            cellStyle = If(columnName = NameOf(SummaryRecord.RecordNumber),
                           (New DataGridViewCellStyle).SetCellStyle(DataGridViewContentAlignment.MiddleCenter, New Padding(0)),
                           (New DataGridViewCellStyle).SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1))
                          )
        End If
        Return cellStyle
    End Function

    ''' <summary>
    ''' Indicates whether a specified DataTable is null, has zero columns, or (optionally) zero rows.
    ''' </summary>
    ''' <param name="Table">DataTable to check.</param>
    ''' <param name="IgnoreRows">When set to true, the function will return true even if the table's row count is equal to zero.</param>
    ''' <returns>False if the specified DataTable null, has zero columns, or zero rows, otherwise true.</returns>
    Public Function IsValidDataTable(Table As DataTable, Optional IgnoreRows As Boolean = False) As Boolean
        Return Table IsNot Nothing AndAlso
               Table.Columns.Count <> 0 AndAlso
               (IgnoreRows OrElse Table.Rows.Count <> 0)
    End Function

End Module
