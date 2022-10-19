' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Reflection

Friend Module DictionaryMap

    Public Function ClassToDictionary(Of T)(ClassObject As T, ParamArray skipProperties() As String) As Dictionary(Of String, String)
        Dim sortedResult As New SortedDictionary(Of Integer, KeyValuePair(Of String, String))

        Dim result As New Dictionary(Of String, String)
        If ClassObject Is Nothing Then
            Return result
        End If

        Dim classType As Type = GetType(T)
        Dim propertyList As IList(Of PropertyInfo) = classType.GetProperties()

        ' Parameter class has no public properties.
        If propertyList.Count = 0 Then
            Return result
        End If

        For Each [property] As PropertyInfo In GetType(T).GetProperties()
            If skipProperties.Contains([property].Name) Then Continue For
            Dim v As String = [property].GetValue(ClassObject, Nothing).ToString
            Dim columnAttrib As ColumnAttribute = [property].GetCustomAttributes(GetType(ColumnAttribute), True).Cast(Of ColumnAttribute)().SingleOrDefault()
            sortedResult.Add(columnAttrib.Order, KeyValuePair.Create([property].Name, v))
        Next [property]
        For Each r As KeyValuePair(Of String, String) In sortedResult.Values
            result.Add(r.Key, r.Value)
        Next
        Return result
    End Function

    ''' <summary>
    ''' Fills properties of a class from a row of a Dictionary where the propertyName of the property matches the ket from that dictionary.
    ''' It does this for each row in the Dictionary, returning a class.
    ''' </summary>
    ''' <typeparam propertyName="T">The class type that is to be returned.</typeparam>
    ''' <param propertyName="Dictionary">Dictionary to fill from.</param>
    ''' <returns>A list of ClassType with its properties set to the data from the matching columns from the DataTable.</returns>
    Public Function DictionaryToClass(Of T As {Class, New})(dic As Dictionary(Of String, String), RecordNumber As Integer) As T
        If dic Is Nothing Then
            Return New T
        End If

        Dim classType As Type = GetType(T)
        Dim propertyList As IList(Of PropertyInfo) = classType.GetProperties()

        ' Parameter class has no public properties.
        If propertyList.Count = 0 Then
            Return New T
        End If

        Dim columnNames As List(Of String) = dic.Keys.ToList()
        Dim classObject As New T
        For Each row As KeyValuePair(Of String, String) In dic
            Dim [property] As PropertyInfo = classType.GetProperty(row.Key, BindingFlags.Public Or BindingFlags.Instance)
            If [property] IsNot Nothing AndAlso [property].CanWrite Then ' Make sure property isn't read only
                Try
                    Dim propertyValue As Object
                    Select Case [property].PropertyType.Name
                        Case "DateTime", "dateTime"
                            propertyValue = row.Value.ParseDate([property].Name)
                            classObject.GetType.GetProperty($"{[property].Name}AsString").SetValue(classObject, row.Value, Nothing)
                        Case "previousDateTime"
                            propertyValue = row.Value.ParseDate($"{[property].Name}AsString")
                            classObject.GetType.GetProperty([property].Name).SetValue(classObject, row.Value, Nothing)
                        Case "Single"
                            propertyValue = row.Value.ParseSingle
                        Case "String", "Int32", "Boolean"
                            propertyValue = Convert.ChangeType(row.Value, [property].PropertyType)
                        Case Else
                            Throw UnreachableException()
                    End Select

                    classObject.GetType.GetProperty([property].Name).SetValue(classObject, propertyValue, Nothing)
                Catch ex As Exception
                    Return New T
                End Try
            End If
        Next row
        classObject.GetType.GetProperty(NameOf(RecordNumber))?.SetValue(classObject, RecordNumber, Nothing)

        Return classObject
    End Function

End Module
