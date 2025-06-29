' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

''' <summary>
'''  Provides extension methods for working with item indexes related to <see cref="ServerDataIndexes"/>.
''' </summary>
Friend Module ItemIndexesModule

    ''' <summary>
    '''  Attempts to resolve a string key to a <see cref="ServerDataIndexes"/> value.
    '''  Handles special cases for notification history and keys containing a colon.
    ''' </summary>
    ''' <param name="key">The string key to resolve.</param>
    ''' <returns>The corresponding <see cref="ServerDataIndexes"/> value.</returns>
    ''' <exception cref="ArgumentException">
    '''  Thrown if the key cannot be resolved to a <see cref="ServerDataIndexes"/> value.
    ''' </exception>
    <Extension>
    Friend Function GetItemIndex(key As String) As ServerDataIndexes
        Dim result As Object = Nothing

        If key.Contains("Notification") Then
            Return ServerDataIndexes.notificationHistory
        End If
        If key.Contains(":"c) Then
            key = key.Split(":")(0)
        End If

        If [Enum].TryParse(GetType(ServerDataIndexes), key, result) Then
            Return CType(result, ServerDataIndexes)
        End If
        Stop
        Throw New ArgumentException($"{key} was not found in {NameOf(ServerDataIndexes)}")
    End Function

End Module
