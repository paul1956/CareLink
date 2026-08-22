' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.CompilerServices

Public Module EnumExtensions

    <Extension>
    Public Function Description(value As [Enum]) As String
        Dim field As FieldInfo =
            value.GetType().GetField(name:=value.ToString())
        Dim attribute As DescriptionAttribute =
            field?.GetCustomAttribute(Of DescriptionAttribute)()

        Return If(attribute?.Description, value.ToString())
    End Function

End Module
