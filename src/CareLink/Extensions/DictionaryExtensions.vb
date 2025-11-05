' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text

Public Module DictionaryExtensions

    ''' <summary>
    '''  Sets a <see cref="Date"/> property on an object by assigning the
    '''   string value to a corresponding property with an "AsString" suffix.
    ''' </summary>
    ''' <typeparam name="T">
    '''  The type of the object, which must be a class with a parameterless constructor.
    ''' </typeparam>
    ''' <param name="obj">The object whose property will be set.</param>
    ''' <param name="row">
    '''  The key-value pair containing the property name and value.
    ''' </param>
    ''' <param name="[property]">
    '''  The <see cref="PropertyInfo"/> of the property to set.
    ''' </param>
    Private Sub SetDateProperty(Of T As {Class, New})(
            obj As T,
            row As KeyValuePair(Of String, String),
            [property] As PropertyInfo)
        Try
            Dim propertyInfo As PropertyInfo = obj.GetType.GetProperty(name:=$"{[property].Name}AsString")
            If propertyInfo Is Nothing Then
                Stop
            End If
            propertyInfo.SetValue(obj, row.Value, index:=Nothing)
        Catch ex As Exception
            Stop
        End Try
    End Sub

    ''' <summary>
    '''  Converts a <see cref="Dictionary(Of String, T)"/> to a
    '''  CSV string representation.
    ''' </summary>
    ''' <typeparam name="T">The type of the values in the dictionary.</typeparam>
    ''' <param name="dic">The dictionary to convert.</param>
    ''' <returns>A CSV string representation of the dictionary.</returns>
    <Extension>
    Private Function ToCsv(Of T)(dic As Dictionary(Of String, T)) As String
        If dic Is Nothing Then
            Return "{}"
        End If

        Dim result As New StringBuilder
        For Each kvp As KeyValuePair(Of String, T) In dic
            result.Append(value:=$"{kvp.Key} = {kvp.Value}, ")
        Next
        result.TrimEnd(value:=", ")
        Return $"{{{result}}}"
    End Function

    ''' <summary>
    '''  Gets the value for a key from a <see cref="Dictionary(Of String, String)"/>,
    '''  or returns an empty string if not found.
    ''' </summary>
    ''' <param name="item">The dictionary to search.</param>
    ''' <param name="Key">The key to look up.</param>
    ''' <returns>
    '''  The value for the key, or a <see cref="String.Empty"/> if not found.
    ''' </returns>
    <Extension>
    Friend Function GetStringValueOrEmpty(item As Dictionary(Of String, String), Key As String) As String
        If item Is Nothing Then
            Return ""
        End If
        Dim value As String = String.Empty
        Return If(item.TryGetValue(Key, value),
                  value,
                  String.Empty)
    End Function

    ''' <summary>
    '''  Clones a <see cref="Dictionary(Of String, T)"/> to a new instance.
    '''  This is useful when you want to create a copy of the dictionary
    '''  without modifying the original.
    ''' </summary>
    ''' <typeparam name="T">The type of the values in the dictionary.</typeparam>
    ''' <param name="dictionary">The dictionary to clone.</param>
    ''' <returns>
    '''  A new instance of <see cref="Dictionary(Of String, T)"/> with
    '''  the same key-value pairs.
    ''' </returns>
    <Extension>
    Public Function Clone(Of T)(dictionary As Dictionary(Of String, T)) As Dictionary(Of String, T)
        Return New Dictionary(Of String, T)(dictionary)
    End Function

    ''' <summary>
    '''  Fills properties of a class from a row of a
    '''  <see cref="Dictionary(Of String, String)"/> where the propertyName
    '''  of the property matches the Key from that dictionary.
    '''  It does this for each row in the Dictionary, returning a class.
    ''' </summary>
    ''' <typeparam propertyName="T">The class type that is to be returned.</typeparam>
    ''' <param propertyName="Dictionary">Dictionary to fill from.</param>
    ''' <returns>
    '''  A list of <see langword="Class"/> types with its properties set to the data
    '''  from the matching columns from the <see cref="DataTable"/>.
    ''' </returns>
    Public Function DictionaryToClass(Of T As {Class, New})(
            dic As Dictionary(Of String, String),
            recordNumber As Integer) As T

        If dic Is Nothing OrElse dic.Count = 0 Then
            Return New T
        End If

        Dim classType As Type = GetType(T)
        Dim propertyList As IList(Of PropertyInfo) = classType.GetProperties()

        ' Parameter class has no public properties.
        If propertyList.Count = 0 Then
            Return New T
        End If

        Dim columnNames As List(Of String) = dic.Keys.ToList()
        Dim obj As New T
        For Each row As KeyValuePair(Of String, String) In dic
            Dim [property] As PropertyInfo =
                classType.GetProperty(
                    name:=row.Key,
                    bindingAttr:=BindingFlags.Public Or BindingFlags.Instance Or BindingFlags.IgnoreCase)

            If [property] IsNot Nothing Then
                If [property].CanWrite Then ' Make sure property isn't read only
                    Try
                        Dim value As Object = Nothing
                        Select Case [property].PropertyType.Name
                            Case "DateTime"
                                SetDateProperty(obj, row, [property])
                                Continue For
                            Case NameOf([Single])
                                value = row.Value.ParseSingle(digits:=10)
                            Case NameOf([Double])
                                value = CDbl(row.Value.ParseSingle(digits:=10))
                            Case NameOf([Decimal])
                                value = CDec(row.Value.ParseSingle(digits:=3))
                            Case NameOf([Boolean]),
                                 NameOf([Int32]),
                                 NameOf([String])
                                value = Convert.ChangeType(
                                    row.Value,
                                    conversionType:=[property].PropertyType)
                            Case "MarkerData"
                                Stop
                            Case Else
                                Throw UnreachableException(
                                    paramName:=[property].PropertyType.Name)
                        End Select

                        obj.GetType.GetProperty([property].Name) _
                                   .SetValue(obj, value, index:=Nothing)
                    Catch ex As Exception
                        Return New T
                    End Try
                End If
            Else
                Dim stackFrame As New StackFrame(skipFrames:=0, needFileInfo:=True)
                MsgBox(
                    heading:=$"'{row.Key}' is unknown Property",
                    prompt:=$"Please open a GitHub issue at {GitHubCareLinkUrl}issues",
                    buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                    title:=GetTitleFromStack(stackFrame))
            End If
        Next row

        obj.GetType.GetProperty(name:="RecordNumber")?.SetValue(obj, value:=recordNumber, index:=Nothing)
        Return obj
    End Function

    ''' <summary>
    '''  Converts a JSON string to a <see cref="Dictionary(Of String, String)"/>.
    ''' </summary>
    ''' <param name="json">The JSON string to convert.</param>
    ''' <returns>A Dictionary with the key-value pairs from the JSON string.</returns>
    Public Function GetAdditionalInformation(json As String) As Dictionary(Of String, String)
        Dim valueList() As String = GetValueList(json)
        Dim dic As New Dictionary(Of String, String)
        For Each row As String In valueList
            Dim value() As String = row.Split(separator:=" = ")
            dic.Add(key:=value(0).Trim, value:=value(1).Trim)
        Next
        Return dic
    End Function

    ''' <summary>
    '''  Converts a JSON string to a Dictionary.
    ''' </summary>
    ''' <param name="json">The JSON string to convert.</param>
    ''' <returns>A Dictionary with the key-value pairs from the JSON string.
    Public Function GetValueList(json As String) As String()
        Dim values As String() =
            JsonToDictionary(json).ToCsv.Remove(s:="{").Trim.Remove(s:="}").Trim.Split(separator:=",")

        Dim selector As Func(Of String, String) = Function(s As String) As String
                                                      Return s.Trim()
                                                  End Function
        Return values.Select(selector).ToArray
    End Function

    ''' <summary>
    '''  Returns the index of the value in the <see cref="SortedDictionary"/>.
    ''' </summary>
    ''' <param name="dic">The SortedDictionary to search.</param>
    ''' <param name="item">The <see cref="KnownColor"/> to find.</param>
    ''' <returns>
    '''  The index of the item in the <see cref="SortedDictionary"/>;
    '''  otherwise -1 if not found.
    ''' </returns>
    <Extension>
    Public Function IndexOfValue(dic As SortedDictionary(Of String, KnownColor), item As KnownColor) As Integer
        Return dic.Values.ToList.IndexOf(item)
    End Function

    ''' <summary>
    '''  Sorts a <see cref="Dictionary(Of String, T)"/> by its keys.
    '''  This is useful when you want to ensure the keys are in sorted order.
    ''' </summary>
    ''' <typeparam name="T">The type of the values in the dictionary.</typeparam>
    ''' <param name="dic">The dictionary to sort.</param>
    ''' <returns>A <see cref="Dictionary(Of String, T)"/> sorted by keys.</returns>
    <Extension>
    Public Function Sort(Of T)(dic As Dictionary(Of String, T)) As Dictionary(Of String, T)
        Dim sortDic As New SortedDictionary(Of String, T)
        For Each kvp As KeyValuePair(Of String, T) In dic
            sortDic.Add(kvp.Key, kvp.Value)
        Next

        Dim keySelector As Func(Of KeyValuePair(Of String, T), String) =
            Function(p As KeyValuePair(Of String, T)) As String
                Return p.Key
            End Function
        Dim elementSelector As Func(Of KeyValuePair(Of String, T), T) = Function(p As KeyValuePair(Of String, T)) As T
                                                                            Return p.Value
                                                                        End Function
        Return (From x In sortDic Select x).ToDictionary(keySelector, elementSelector)
    End Function

End Module
