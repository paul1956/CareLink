' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Reflection
Imports System.Runtime.CompilerServices

Friend Module ObjectExtensions

    ' Helper method to convert object to Dictionary(Of String, String)
    <Extension>
    Public Function InstanceToDictionary(Of T)(instance As T, Optional prefix As String = "") As Dictionary(Of String, String)
        Dim result As New Dictionary(Of String, String)()
        Dim tmpDictionary As New Dictionary(Of String, String)
        Try
            For Each pi As PropertyInfo In GetType(T).GetProperties
                Dim value As String = $"{pi.GetValue(obj:=instance)?.ToString}"
                Select Case pi.Name
                    Case NameOf(AdditionalInfo)
                        tmpDictionary =
                            CType(pi.GetValue(obj:=instance), Json.AdditionalInfo).
                                InstanceToDictionary(prefix:="AdditionalInfo:")
                        tmpDictionary =
                            CType(pi.GetValue(obj:=instance), Json.AdditionalInfo).
                                InstanceToDictionary(prefix:="AdditionalInfo:")

                    Case "Acknowledged"
                        tmpDictionary =
                            CType(pi.GetValue(obj:=instance), AcknowledgedRecord).
                                InstanceToDictionary(prefix:="Acknowledged:")
                        tmpDictionary =
                        CType(pi.GetValue(obj:=instance), AcknowledgedRecord).
                            InstanceToDictionary(prefix:="Acknowledged:")
                        Continue For
                    Case Else
                        If s_rowsToHide.Contains(item:=pi.Name) OrElse
                            IsNullOrEmpty(value) OrElse
                            value = "0" OrElse
                            value = "Color [Empty]" OrElse
                            value = "1/1/0001 12:00:00 AM" Then
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
        If tmpDictionary.Count > 0 Then
            For Each kvp As KeyValuePair(Of String, String) In tmpDictionary
                result.Add(kvp.Key, kvp.Value)
            Next
        End If
        Return result
    End Function

End Module
