' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Reflection

''' <summary>
''' DataTable/Class Mapping Class
''' </summary>
Friend Module DataTableMap

    ''' <summary>
    ''' Adds a DataRow to a DataTable from the public properties of a class.
    ''' </summary>
    ''' <param propertyName="Table">A reference to the DataTable to insert the DataRow into.</param>
    ''' <param propertyName="ClassObject">The class containing the data to fill the DataRow from.</param>
    Private Sub ClassToDataRow(Of T As Class)(ByRef Table As DataTable, ClassObject As T)
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
    Private Function ClassToDatatable(Of T As Class)() As DataTable
        Dim classType As Type = GetType(T)
        Dim result As New DataTable(classType.UnderlyingSystemType.Name)
        Dim propertyOrder As New SortedDictionary(Of Integer, PropertyInfo)
        For Each [property] As PropertyInfo In classType.GetProperties()
            Dim columnAttrib As ColumnAttribute = [property].GetCustomAttributes(GetType(ColumnAttribute), True).Cast(Of ColumnAttribute)().SingleOrDefault()
            propertyOrder.Add(columnAttrib.Order, [property])
        Next
        For Each [property] As PropertyInfo In propertyOrder.Values
            Dim displayName As String = GetColumnDisplayName([property])
            Dim column As New DataColumn With {
                                .ColumnName = [property].Name,
                                .Caption = displayName,
                                .DataType = [property].PropertyType
                            }
            If IsNullableType(column.DataType) AndAlso column.DataType.IsGenericType Then ' If Nullable<>, this is how we get the underlying Type...
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
        Dim displayNameAttrib As DisplayNameAttribute = [property].GetCustomAttributes(GetType(DisplayNameAttribute), True).Cast(Of DisplayNameAttribute)().SingleOrDefault()
        Return If(displayNameAttrib IsNot Nothing, displayNameAttrib.DisplayName, [property].Name)
    End Function

    ''' <summary>
    ''' Created a Dictionary that maps Class Property Name to Column Alignment
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <returns>Dictionary</returns>
    Public Function ClassPropertiesToCoumnAlignment(Of T As Class)(ByRef alignmentTable As Dictionary(Of String, DataGridViewCellStyle), columnName As String) As DataGridViewCellStyle
        Dim classType As Type = GetType(T)
        Dim cellStyle As New DataGridViewCellStyle
        If Not alignmentTable.Any Then
            For Each [property] As PropertyInfo In classType.GetProperties()
                Dim columnAttrib As ColumnAttribute = [property].GetCustomAttributes(GetType(ColumnAttribute), True).Cast(Of ColumnAttribute)().SingleOrDefault()
                cellStyle = New DataGridViewCellStyle
                Select Case columnAttrib.TypeName
                    Case "Date", NameOf(OADate), NameOf([String])
                        cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1))
                    Case NameOf([Double]), NameOf([Int32]), NameOf([Single]), NameOf([TimeSpan])
                        cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleRight, New Padding(0, 1, 1, 1))
                    Case NameOf([Boolean]), NameOf(Summaryrecord.RecordNumber)
                        cellStyle = cellStyle.SetCellStyle(DataGridViewContentAlignment.MiddleCenter, New Padding(0))
                    Case Else
                        Throw UnreachableException($"{NameOf(DataTableMap)}.{NameOf(ClassPropertiesToCoumnAlignment)} [property].PropertyType.Name = {[property].PropertyType.Name}")
                End Select
                alignmentTable.Add([property].Name, cellStyle)
            Next
        End If
        If Not alignmentTable.TryGetValue(columnName, cellStyle) Then
            If columnName = NameOf(SummaryRecord.RecordNumber) Then
                cellStyle = (New DataGridViewCellStyle).SetCellStyle(DataGridViewContentAlignment.MiddleCenter, New Padding(0))
            Else
                cellStyle = (New DataGridViewCellStyle).SetCellStyle(DataGridViewContentAlignment.MiddleLeft, New Padding(1))
            End If
        End If
        Return cellStyle
    End Function

    ''' <summary>
    ''' Creates a DataTable from a class type's public properties and adds a new DataRow to the table for each class passed as a parameter.
    ''' The DataColumns of the table will match the name and type of the public properties.
    ''' </summary>
    ''' <param name="ClassCollection">A class or array of class to fill the DataTable with.</param>
    ''' <returns>A DataTable who's DataColumns match the name and type of each class T's public properties.</returns>
    Public Function ClassToDatatable(Of T As Class)(ParamArray ClassCollection() As T) As DataTable
        Dim result As DataTable = ClassToDatatable(Of T)()

        If Not IsValidDatatable(result, IgnoreRows:=True) Then
            Return New DataTable()
        End If
        If IsCollectionEmpty(ClassCollection) Then
            Return result ' Returns and empty DataTable with columns defined (table schema)
        End If

        For Each classObject As T In ClassCollection
            ClassToDataRow(result, classObject)
        Next classObject

        Return result
    End Function

    ''' <summary>
    ''' Fills properties of a class from a row of a DataTable where the propertyName of the property matches the column propertyName from that DataTable.
    ''' It does this for each row in the DataTable, returning a List of classes.
    ''' </summary>
    ''' <typeparam propertyName="T">The class type that is to be returned.</typeparam>
    ''' <param propertyName="Table">DataTable to fill from.</param>
    ''' <returns>A list of ClassType with its properties set to the data from the matching columns from the DataTable.</returns>
    Public Function DatatableToClass(Of T As {Class, New})(Table As DataTable) As IList(Of T)
        If Not IsValidDatatable(Table) Then
            Return New List(Of T)()
        End If

        Dim classType As Type = GetType(T)
        Dim propertyList As IList(Of PropertyInfo) = classType.GetProperties()

        ' Parameter class has no public properties.
        If propertyList.Count = 0 Then
            Return New List(Of T)()
        End If

        Dim columnNames As List(Of String) = Table.Columns.Cast(Of DataColumn)().Select(Function(column) column.ColumnName).ToList()

        Dim result As New List(Of T)()
        Try
            For Each row As DataRow In Table.Rows
                Dim classObject As New T()
                For Each [property] As PropertyInfo In propertyList
                    If [property] IsNot Nothing AndAlso [property].CanWrite Then ' Make sure property isn't read only
                        If columnNames.Contains([property].Name) Then ' If property is a column propertyName
                            If row([property].Name) IsNot DBNull.Value Then ' Don't copy over DBNull
                                Dim propertyValue As Object = Convert.ChangeType(row([property].Name), [property].PropertyType)
                                [property].SetValue(classObject, propertyValue, Nothing)
                            Else
                                Stop
                            End If
                        End If
                    End If
                Next [property]
                result.Add(classObject)
            Next row
            Return result
        Catch
            Return New List(Of T)()
        End Try
    End Function

End Module
