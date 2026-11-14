' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.CompilerServices

Public Module EnumExtensions

    <Extension>
    Public Function Description(enumVal As [Enum]) As String
        Dim field As FieldInfo =
            enumVal.GetType().GetField(name:=enumVal.ToString())
        Dim attribute As DescriptionAttribute =
            field?.GetCustomAttribute(Of DescriptionAttribute)()

        Return If(attribute?.Description, enumVal.ToString())
    End Function

    ''' <summary>
    '''  Returns the description of an enum value, or the enum value's name if no description is present.
    ''' </summary>
    ''' <param name="enumVal">The enum value for which to retrieve the description.</param>
    ''' <returns>The description of the enum value, or the enum value's name if no description is present.</returns>
    Public Function GetEnumDescription(enumVal As [Enum]) As String
        Dim field As FieldInfo =
            enumVal.GetType().GetField(name:=enumVal.ToString())
        Dim attribute As DescriptionAttribute() =
            DirectCast(field.GetCustomAttributes(attributeType:=GetType(DescriptionAttribute), inherit:=False), DescriptionAttribute())

        Return If(attribute IsNot Nothing AndAlso attribute.Length > 0, attribute(0).Description, enumVal.ToString())
    End Function

End Module
