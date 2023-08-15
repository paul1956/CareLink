' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module ItemIndexesModule

    <Extension>
    Friend Function GetItemIndex(key As String) As ItemIndexes
        Dim result As Object = Nothing
        If [Enum].TryParse(GetType(ItemIndexes), key, result) Then
            Return CType(result, ItemIndexes)
        End If
        Stop
        Throw New ArgumentException($"{key} was not found in {NameOf(ItemIndexes)}")
    End Function

End Module
