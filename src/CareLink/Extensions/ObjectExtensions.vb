' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text.Json

Friend Module ObjectExtensions

    ' Helper method to convert object to Dictionary(Of String, String)
    <Extension>
    Public Function InstanceToDictionary(Of T)(instance As T, Optional prefix As String = "") As Dictionary(Of String, String)
        Dim result As New Dictionary(Of String, String)()
        Dim second As New Dictionary(Of String, String)
        Try
            Dim elementSelector As Func(Of KeyValuePair(Of String, String), String) =
                Function(v As KeyValuePair(Of String, String))
                    Return v.Value
                End Function
            Dim keySelector As Func(Of KeyValuePair(Of String, String), String) =
                Function(k As KeyValuePair(Of String, String)) As String
                    Return k.Key
                End Function
            For Each pi As PropertyInfo In GetType(T).GetProperties
                Dim obj As Object = pi.GetValue(obj:=instance)
                If pi.PropertyType.IsGenericType AndAlso
                   pi.PropertyType.GetGenericTypeDefinition() = GetType(Nullable(Of )) AndAlso
                   obj Is Nothing Then
                    Continue For
                ElseIf obj Is Nothing Then
                    Continue For
                ElseIf TypeOf obj Is Color AndAlso
                    CType(obj, Color).IsEmpty Then
                    Continue For
                End If
                Dim value As String = obj.ToString()
                Select Case pi.Name
                    Case NameOf(AdditionalInfo)
                        second =
                            CType(obj, AdditionalInfo).
                                InstanceToDictionary(prefix:="AdditionalInfo:")
                        result = result.Concat(second).ToDictionary(keySelector, elementSelector)
                        second.Clear()
                    Case NameOf(AdditionalInfo.AdditionalProperties)
                        Dim source As Dictionary(Of String, JsonElement) =
                            CType(obj, Dictionary(Of String, JsonElement))
                        second = ConvertJsonElementDict(source)
                        result = result.Concat(second).ToDictionary(keySelector, elementSelector)
                        second.Clear()
                    Case "Acknowledged"
                        second =
                            CType(obj, AcknowledgedRecord).
                                InstanceToDictionary(prefix:="Acknowledged:")
                        result = result.Concat(second).ToDictionary(keySelector, elementSelector)
                        second.Clear()
                    Case Else
                        If s_rowsToHide.Contains(item:=pi.Name) Then
                            Continue For
                        End If
                        result.Add(key:=$"{prefix}{pi.Name}", value)
                End Select
            Next
            For Each fi As FieldInfo In GetType(T).GetFields
                result.Add(key:=fi.Name,
                           value:=$"{fi.GetValue(obj:=instance)?.ToString}")
            Next
        Catch ex As Exception
            Stop
        End Try
        If second.Count > 0 Then
            For Each kvp As KeyValuePair(Of String, String) In second
                result.Add(kvp.Key, kvp.Value)
            Next
        End If
        Return result
    End Function

End Module
