' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Reflection

Friend Module DictionaryToClassMapper

    ''' <summary>
    ''' Fills properties of a class from a row of a Dictionary where the propertyName of the property matches the Key from that dictionary.
    ''' It does this for each row in the Dictionary, returning a class.
    ''' </summary>
    ''' <typeparam propertyName="T">The class type that is to be returned.</typeparam>
    ''' <param propertyName="Dictionary">Dictionary to fill from.</param>
    ''' <returns>A list of ClassType with its properties set to the data from the matching columns from the DataTable.</returns>
    Public Function DictionaryToClass(Of T As {Class, New})(dic As Dictionary(Of String, String), recordNumber As Integer) As T
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
            If [property] IsNot Nothing Then
                If [property].CanWrite Then ' Make sure property isn't read only

                    Try
                        Dim propertyValue As Object
                        Select Case [property].PropertyType.Name
                            Case NameOf(TimeChangeRecord.dateTime)
                                propertyValue = row.Value.ParseDate([property].Name)
                                classObject.GetType.GetProperty($"{[property].Name}AsString").SetValue(classObject, row.Value, Nothing)
                            Case NameOf(TimeChangeRecord.previousDateTime)
                                propertyValue = row.Value.ParseDate($"{[property].Name}AsString")
                                classObject.GetType.GetProperty([property].Name).SetValue(classObject, row.Value, Nothing)
                            Case NameOf([Single])
                                propertyValue = row.Value.ParseSingle()
                            Case NameOf([Boolean]), NameOf([Int32]), NameOf([String])
                                propertyValue = Convert.ChangeType(row.Value, [property].PropertyType)
                            Case Else
                                Throw UnreachableException($"{NameOf(SummaryRecordHelpers)}.{NameOf(GetCellStyle)} [property].PropertyType.Name = {[property].PropertyType.Name}")
                        End Select

                        classObject.GetType.GetProperty([property].Name).SetValue(classObject, propertyValue, Nothing)
                    Catch ex As Exception
                        Return New T
                    End Try
                End If
            Else
                If Not Debugger.IsAttached Then
                    Stop
                    MsgBox($"{row.Key} is unknown Property, please open a GitHub issue", MsgBoxStyle.OkOnly, $"Form 1 line:{New StackFrame(0, True).GetFileLineNumber()}")
                End If
            End If
        Next row

        classObject.GetType.GetProperty("RecordNumber")?.SetValue(classObject, recordNumber, Nothing)

        Return classObject
    End Function

End Module
