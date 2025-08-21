' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Reflection
Imports System.Runtime.CompilerServices

''' <summary>
'''  DataTable/Class Mapping Class
''' </summary>
Friend Module DataTableHelpers

    ''' <summary>
    '''  Adds a <see cref="DataRow"/> to a <see cref="DataTable"/> from
    '''  the public properties of a class.
    ''' </summary>
    ''' <typeparam name="T">The type of the class to add as a row.</typeparam>
    ''' <param name="table">A reference to the DataTable to insert the DataRow into.</param>
    ''' <param name="obj">The class containing the data to fill the DataRow from.</param>
    <Extension>
    Private Sub Add(Of T As Class)(ByRef table As DataTable, obj As T)
        If obj Is Nothing Then
            table = Nothing
            Return
        End If
        Dim row As DataRow = table.NewRow()
        For Each [property] As PropertyInfo In GetType(T).GetProperties()
            Dim name As String = [property].Name
            If table.Columns.Contains(name) Then
                If table.Columns(name) IsNot Nothing Then
                    row(columnName:=name) = [property].GetValue(obj, index:=Nothing)
                End If
            End If
        Next [property]
        table.Rows.Add(row)
    End Sub

    ''' <summary>
    '''  Creates a <see cref="DataTable"/> from a class type's public properties.
    '''  The DataColumns of the table will match the name and type of the public properties.
    ''' </summary>
    ''' <typeparam name="T">The type of the class to create a DataTable from.</typeparam>
    ''' <returns>
    '''  A DataTable who's DataColumns match the name and type of each
    '''  class T's public properties.
    ''' </returns>
    Private Function ClassToDataTable(Of T As Class)() As DataTable
        Dim classType As Type = GetType(T)
        Dim result As New DataTable(classType.UnderlyingSystemType.Name)
        Dim propertyOrder As New SortedDictionary(Of Integer, PropertyInfo)
        Dim fallbackOrder As Integer = 1000

        For Each [property] As PropertyInfo In classType.GetProperties()
            Dim colAttribute As ColumnAttribute = [property].GetCustomAttributes(
                    attributeType:=GetType(ColumnAttribute),
                    inherit:=True).Cast(Of ColumnAttribute)().SingleOrDefault()
            Dim key As Integer = If(colAttribute Is Nothing,
                                    fallbackOrder,
                                    colAttribute.Order)
            While propertyOrder.ContainsKey(key)
                key += 1 ' Avoid duplicate keys
            End While
            propertyOrder.Add(key, value:=[property])
            If colAttribute Is Nothing Then
                fallbackOrder += 1
            End If
        Next

        For Each [property] As PropertyInfo In propertyOrder.Values
            Dim propertyType As Type = [property].PropertyType
            If propertyType = GetType(Boolean) Then
                propertyType = GetType(String)
            ElseIf propertyType.IsEnum Then
                propertyType = GetType(String) ' Or propertyType = [property].PropertyType
            End If
            Dim column As New DataColumn With {
                .ColumnName = [property].Name,
                .Caption = GetColumnDisplayName([property]),
                .DataType =
                If(IsNullableType(nullableType:=propertyType) AndAlso
                        propertyType.IsGenericType,
                   propertyType.GenericTypeArguments.FirstOrDefault(),
                   propertyType)
            }
            result.Columns.Add(column)
        Next

        Return result
    End Function

    ''' <summary>
    '''  Gets the display name for a property, using the
    '''  <see cref="DisplayNameAttribute"/> if present.
    '''  Replaces spaces with non-breaking spaces for certain display names.
    ''' </summary>
    ''' <param name="property">The property to get the display name for.</param>
    ''' <returns>The display name for the property.</returns>
    Private Function GetColumnDisplayName([property] As PropertyInfo) As String
        Dim displayNameAttribute As DisplayNameAttribute = [property].GetCustomAttributes(
            attributeType:=GetType(DisplayNameAttribute),
            inherit:=True).Cast(Of DisplayNameAttribute)().SingleOrDefault()
        If displayNameAttribute Is Nothing Then
            Return [property].Name
        Else

            ' Non-breaking space for better display
            Dim displayName As String = displayNameAttribute.DisplayName
            If displayName.Contains(value:="From Pump") OrElse
                    displayName.Contains(value:="As Date") Then
                displayName = displayName.Replace(oldValue:=" ", newValue:=NonBreakingSpace)
            End If
            Return displayName
        End If
    End Function

    ''' <summary>
    '''  Indicates whether a specified Type can be assigned null.
    ''' </summary>
    ''' <param name="nullableType">The Type to check for nullable property.</param>
    ''' <returns>True if the specified Type can be assigned null, otherwise false.</returns>
    Private Function IsNullableType(nullableType As Type) As Boolean
        If Not nullableType.IsValueType Then
            Return True ' Reference Type
        End If
        If Nullable.GetUnderlyingType(nullableType) IsNot Nothing Then
            Return True ' Nullable<T>
        End If
        Return False ' Value Type
    End Function

    ''' <summary>
    '''  Creates a <see cref="DataTable"/> from a class type's public properties and
    '''  adds a new <see cref="DataRow"/> to the table for each class passed as a parameter.
    '''  The DataColumns of the table will match the name and type of the public properties.
    ''' </summary>
    ''' <param name="classCollection">A List(Of class) to fill the DataTable with.</param>
    ''' <returns>
    '''  A DataTable who's DataColumns match the name and type of
    '''  each class T's public properties.
    ''' </returns>
    Public Function ClassCollectionToDataTable(Of T As Class)(classCollection As List(Of T)) _
        As DataTable

        Dim result As DataTable = ClassToDataTable(Of T)()

        If Not IsValidDataTable(result, IgnoreRows:=True) Then
            Return New DataTable()
        End If
        If classCollection?.Count > 0 Then
            For Each obj As T In classCollection
                result.Add(obj)
            Next
        End If

        Return result
    End Function

    ''' <summary>
    '''  Indicates whether a specified DataTable is null, has zero columns,
    '''  or (optionally) zero rows.
    ''' </summary>
    ''' <param name="Table">DataTable to check.</param>
    ''' <param name="IgnoreRows">
    '''  When set to true, the function will return true even if the table's row count
    '''  is equal to zero.
    ''' </param>
    ''' <returns>
    '''  False if the specified DataTable null, has zero columns,
    '''  or zero rows, otherwise true.
    ''' </returns>
    <Extension>
    Public Function IsValidDataTable(
        Table As DataTable,
        Optional IgnoreRows As Boolean = False) As Boolean

        Return Table IsNot Nothing AndAlso
               Table.Columns.Count <> 0 AndAlso
               (IgnoreRows OrElse Table.Rows.Count <> 0)
    End Function

End Module
