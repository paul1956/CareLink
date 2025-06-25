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
    '''  Adds a <see cref="DataRow"/> to a <see cref="DataTable"/> from the public properties of a class.
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
            If table.Columns.Contains([property].Name) Then
                If table.Columns([property].Name) IsNot Nothing Then
                    row(columnName:=[property].Name) = [property].GetValue(obj, index:=Nothing)
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
    '''  A DataTable who's DataColumns match the name and type of each class T's public properties.
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
            Dim order As Integer = If(colAttribute IsNot Nothing, colAttribute.Order, fallbackOrder)
            While propertyOrder.ContainsKey(order)
                order += 1 ' Avoid duplicate keys
            End While
            propertyOrder.Add(order, [property])
            If colAttribute Is Nothing Then fallbackOrder += 1
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
                .DataType = If(IsNullableType(propertyType) AndAlso propertyType.IsGenericType,
                               propertyType.GenericTypeArguments.FirstOrDefault(),
                               propertyType)
            }
            result.Columns.Add(column)
        Next

        Return result
    End Function

    ''' <summary>
    '''  Gets the display name for a property, using the <see cref="DisplayNameAttribute"/> if present.
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
            If displayName.Contains("From Pump") OrElse displayName.Contains("As Date") Then
                displayName = displayName.Replace(" ", NonBreakingSpace)
            End If
            Return displayName
        End If
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
    '''  Creates a <see cref="DataTable"/> from a class type's public properties and
    '''  adds a new <see cref="DataRow"/> to the table for each class passed as a parameter.
    '''  The DataColumns of the table will match the name and type of the public properties.
    ''' </summary>
    ''' <param name="listOfClass">A List(Of class) to fill the DataTable with.</param>
    ''' <returns>A DataTable who's DataColumns match the name and type of each class T's public properties.</returns>
    Public Function ClassCollectionToDataTable(Of T As Class)(listOfClass As List(Of T)) As DataTable
        Dim result As DataTable = ClassToDataTable(Of T)()

        If Not IsValidDataTable(result, IgnoreRows:=True) Then
            Return New DataTable()
        End If
        If listOfClass?.Count > 0 Then
            For Each classObject As T In listOfClass
                result.Add(classObject)
            Next classObject
        End If

        Return result
    End Function

    ''' <summary>
    '''  Creates a <see cref="Dictionary"/> that maps class property names to <see cref="DataGridViewCellStyle"/> for column alignment.
    '''  Determines alignment and padding based on the <see cref="ColumnAttribute"/> type name.
    ''' </summary>
    ''' <typeparam name="T">The type of the class whose properties are mapped.</typeparam>
    ''' <param name="alignmentTable">A dictionary to populate with property name to cell style mappings.</param>
    ''' <param name="columnName">The column name to retrieve or add alignment for.</param>
    ''' <returns>The <see cref="DataGridViewCellStyle"/> for the specified column.</returns>
    Public Function ClassPropertiesToColumnAlignment(Of T As Class)(ByRef alignmentTable As Dictionary(Of String, DataGridViewCellStyle), columnName As String) As DataGridViewCellStyle
        Dim classType As Type = GetType(T)
        Dim cellStyle As New DataGridViewCellStyle
        If alignmentTable.Count = 0 Then
            For Each [property] As PropertyInfo In classType.GetProperties()
                cellStyle = New DataGridViewCellStyle
                Dim typeName As String = [property].GetCustomAttributes(GetType(ColumnAttribute), inherit:=True).Cast(Of ColumnAttribute)().SingleOrDefault()?.TypeName
                Select Case typeName
                    Case "additionalInfo", "Date", "DateTime", NameOf(OADate), "RecordNumber", NameOf([String]), "Version"
                        cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(all:=1))
                    Case NameOf([Decimal]), NameOf([Double]), NameOf([Int32]), NameOf([Single]), NameOf([TimeSpan])
                        cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleRight, New Padding(left:=0, top:=1, right:=1, bottom:=1))
                    Case NameOf([Boolean]), "DeleteRow"
                        cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleCenter, New Padding(all:=0))
                    Case "CustomProperty"
                        cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleRight, New Padding(left:=0, top:=2, right:=2, bottom:=2))
                    Case Else
                        Throw UnreachableException([property].PropertyType.Name)
                End Select
                alignmentTable.Add([property].Name, cellStyle)
            Next
        End If
        If Not alignmentTable.TryGetValue(columnName, cellStyle) Then
            cellStyle = If(columnName = NameOf(SummaryRecord.RecordNumber) OrElse columnName = NameOf(Limit.Index),
                           (New DataGridViewCellStyle).SetCellStyle(alignment:=DataGridViewContentAlignment.MiddleCenter, padding:=New Padding(all:=0)),
                           (New DataGridViewCellStyle).SetCellStyle(alignment:=DataGridViewContentAlignment.MiddleLeft, padding:=New Padding(all:=1))
                          )
            alignmentTable.Add(columnName, cellStyle)
        End If
        Return cellStyle
    End Function

    ''' <summary>
    '''  Indicates whether a specified DataTable is null, has zero columns, or (optionally) zero rows.
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
