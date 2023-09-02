' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text

Public Module DictionaryExtensions

    <Extension>
    Friend Function GetSingleValue(item As Dictionary(Of String, String), key As String) As Single
        Dim ret As String = ""
        Return If(item.TryGetValue(key, ret),
                  ret.ParseSingle(3),
                  Single.NaN
                 )
    End Function

    <Extension>
    Friend Function GetStringValueOrEmpty(item As Dictionary(Of String, String), Key As String) As String
        If item Is Nothing Then
            Return ""
        End If
        Dim returnString As String = ""
        Return If(item.TryGetValue(Key, returnString),
                  returnString,
                  ""
                 )
    End Function

    <Extension>
    Public Function Clone(Of T)(dic As Dictionary(Of String, T)) As Dictionary(Of String, T)
        Return (From x In dic Select x).ToDictionary(Function(p) p.Key, Function(p) p.Value)
    End Function

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
                                propertyValue = row.Value.ParseSingle(10)
                            Case NameOf([Double])
                                propertyValue = CDbl(row.Value.ParseSingle(10))
                            Case NameOf([Decimal])
                                propertyValue = CDec(row.Value.ParseSingle(3))
                            Case NameOf([Boolean]),
                                 NameOf([Int32]),
                                 NameOf([String])
                                propertyValue = Convert.ChangeType(row.Value, [property].PropertyType)
                            Case Else
                                Throw UnreachableException([property].PropertyType.Name)
                        End Select

                        classObject.GetType.GetProperty([property].Name).SetValue(classObject, propertyValue, Nothing)
                    Catch ex As Exception
                        Return New T
                    End Try
                End If
            Else
                Stop
                MsgBox($"'{row.Key}' is unknown Property", $"Please open a GitHub issue at {GitHubCareLinkUrl}issues", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, GetTitleFromStack(New StackFrame(0, True)))
            End If
        Next row

        classObject.GetType.GetProperty("RecordNumber")?.SetValue(classObject, recordNumber, Nothing)

        Return classObject
    End Function

    <Extension>
    Public Function IndexOfValue(dic As SortedDictionary(Of String, KnownColor), item As KnownColor) As Integer
        Return dic.Values.ToList.IndexOf(item)
    End Function

    Public Function Is770G() As Boolean
        If RecentDataEmpty() Then Return False
        Return RecentData.GetStringValueOrEmpty(NameOf(ItemIndexes.pumpModelNumber)) = "MMT-1880"
    End Function

    <Extension>
    Public Function Sort(Of T)(dic As Dictionary(Of String, T)) As Dictionary(Of String, T)
        Dim sortDic As New SortedDictionary(Of String, T)
        For Each kvp As KeyValuePair(Of String, T) In dic
            sortDic.Add(kvp.Key, kvp.Value)
        Next
        Return (From x In sortDic Select x).ToDictionary(Function(p) p.Key, Function(p) p.Value)
    End Function

    <Extension>
    Public Function ToCsv(Of T)(dic As Dictionary(Of String, T)) As String
        If dic Is Nothing Then
            Return "{}"
        End If

        Dim result As New StringBuilder
        For Each kvp As KeyValuePair(Of String, T) In dic
            result.Append($"{kvp.Key} = {kvp.Value}, ")
        Next
        result.TrimEnd(", ")
        Return $"{{{result}}}"
    End Function

    <Extension>
    Public Function ToDataSource(Of T)(dic As Dictionary(Of String, T)) As List(Of KeyValuePair(Of String, T))
        Dim dataSource As New List(Of KeyValuePair(Of String, T))
        For Each kvp As KeyValuePair(Of String, T) In dic
            dataSource.Add(KeyValuePair.Create(kvp.Key, kvp.Value))
        Next
        Return dataSource
    End Function

End Module
