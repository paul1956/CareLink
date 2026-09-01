' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Reflection
Imports System.Runtime.CompilerServices

Friend Module ObjectExtensions
    ' Helper method to convert object to Dictionary(Of String, String)
    <Extension>
    Public Function InstanceToDictionary(Of T)(instance As T) As Dictionary(Of String, String)
        Dim result As New Dictionary(Of String, String)()
        For Each pi As PropertyInfo In GetType(T).GetProperties
            result.Add(key:=pi.Name, value:=pi.GetValue(instance).ToString)
        Next
        For Each fi As FieldInfo In GetType(T).GetFields
            result.Add(key:=fi.Name, value:=fi.GetValue(instance).ToString)
        Next
        Return result
    End Function
End Module
