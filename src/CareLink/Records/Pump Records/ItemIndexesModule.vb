' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module ItemIndexesModule

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
