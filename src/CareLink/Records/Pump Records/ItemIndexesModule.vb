' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

''' <summary>
'''  Provides extension methods for working with item indexes related
'''  to <see cref="ServerDataEnum"/>.
''' </summary>
Friend Module ItemIndexesModule

    ''' <summary>
    '''  Attempts to resolve a string key to a <see cref="ServerDataEnum"/> value.
    '''  Handles special cases for notification history and keys containing a colon.
    ''' </summary>
    ''' <param name="key">The string key to resolve.</param>
    ''' <returns>The corresponding <see cref="ServerDataEnum"/> value.</returns>
    ''' <exception cref="ArgumentException">
    '''  Thrown if the key cannot be resolved to a <see cref="ServerDataEnum"/> value.
    ''' </exception>
    <Extension>
    Friend Function GetItemIndex(key As String) As ServerDataEnum
        Dim result As Object = Nothing

        If key.Contains(value:="Notification") Then
            Return ServerDataEnum.notificationHistory
        End If
        If key.Contains(value:=":"c) Then
            key = key.Split(separator:=":")(0)
        End If

        If [Enum].TryParse(enumType:=GetType(ServerDataEnum), value:=key, result) Then
            Return CType(result, ServerDataEnum)
        End If
        Stop
        Dim message As String = $"{key} was not found in {NameOf(ServerDataEnum)}"
        Throw New ArgumentException(message)
    End Function

End Module
