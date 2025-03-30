' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Text.Json
Imports WebView2.DevTools.Dom

Public Module DictionaryExtensions

    Private ReadOnly s_700Models As New List(Of String) From {
        "MMT-1812",
        "MMT-1880"}

    Private Sub SetDateProperty(Of T As {Class, New})(classObject As T, row As KeyValuePair(Of String, String), [property] As PropertyInfo)
        Try
            Dim propertyInfo As PropertyInfo = classObject.GetType.GetProperty($"{[property].Name}AsString")
            If propertyInfo Is Nothing Then
                Stop
            End If
            propertyInfo.SetValue(classObject, row.Value, Nothing)
        Catch ex As Exception
            Stop
        End Try
    End Sub

    <Extension>
    Friend Function GetBooleanValue(item As Dictionary(Of String, String), key As String) As Boolean
        Dim ret As String = ""
        Return item.TryGetValue(key, ret) AndAlso Boolean.Parse(ret)
    End Function

    <Extension>
    Friend Function GetSingleValue(item As Dictionary(Of String, String), key As String) As Single
        Dim value As String = ""
        Return If(item.TryGetValue(key, value),
                  value.ParseSingle(decimalDigits:=3),
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
        Dim classObject As New T
        For Each row As KeyValuePair(Of String, String) In dic
            Dim [property] As PropertyInfo = classType.GetProperty(row.Key, BindingFlags.Public Or BindingFlags.Instance Or BindingFlags.IgnoreCase)
            If [property] IsNot Nothing Then
                If [property].CanWrite Then ' Make sure property isn't read only

                    Try
                        Dim propertyValue As Object = Nothing
                        Select Case [property].PropertyType.Name
                            Case "DateTime"
                                SetDateProperty(classObject, row, [property])
                                Continue For
                            Case NameOf([Single])
                                propertyValue = row.Value.ParseSingle(decimalDigits:=10)
                            Case NameOf([Double])
                                propertyValue = CDbl(row.Value.ParseSingle(decimalDigits:=10))
                            Case NameOf([Decimal])
                                propertyValue = CDec(row.Value.ParseSingle(decimalDigits:=3))
                            Case NameOf([Boolean]),
                                 NameOf([Int32]),
                                 NameOf([String])
                                propertyValue = Convert.ChangeType(row.Value, [property].PropertyType)
                            Case "MarkerData"
                                Stop
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

    Public Function GetAdditionalInformation(jsonString As String) As Dictionary(Of String, String)
        Dim valueList() As String = GetValueList(jsonString)
        Dim dic As New Dictionary(Of String, String)
        For Each row As String In valueList
            Dim value() As String = row.Split(" = ")
            dic.Add(value(0).Trim, value(1).Trim)
        Next
        Return dic
    End Function

    Public Function GetValueList(jsonString As String) As String()
        Dim valueList As String() = JsonToDictionary(jsonString).ToCsv _
            .Replace("{", "").Trim _
            .Replace("}", "").Trim _
            .Split(",")
        Return valueList.Select(Function(s) s.Trim()).ToArray
    End Function

    <Extension>
    Public Function IndexOfValue(dic As SortedDictionary(Of String, KnownColor), item As KnownColor) As Integer
        Return dic.Values.ToList.IndexOf(item)
    End Function

    Public Function Is700Series() As Boolean
        If RecentDataEmpty() Then Return False
        Return s_700Models.Contains(PatientData.MedicalDeviceInformation.ModelNumber)
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
    Public Function ToDataSource(dic As Dictionary(Of String, Object)) As List(Of KeyValuePair(Of String, String))
        Dim dataSource As New List(Of KeyValuePair(Of String, String))
        For Each kvp As KeyValuePair(Of String, Object) In dic
            dataSource.Add(KeyValuePair.Create(kvp.Key, CType(kvp.Value, String)))
        Next
        Return dataSource
    End Function

End Module
