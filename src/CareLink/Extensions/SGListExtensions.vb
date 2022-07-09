' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module SGListExtensions
    <Extension()>
    Friend Function ToSgList(innerJson As List(Of Dictionary(Of String, String)), ignoreZero As Boolean) As List(Of SgRecord)
        Dim sGs As New List(Of SgRecord)
        For i As Integer = 0 To innerJson.Count - 1

            Dim sgItem As New SgRecord(innerJson, i)
            If ignoreZero AndAlso sgItem.sg = 0 Then
                Continue For
            End If
            sGs.Add(sgItem)
        Next
        Return sGs
    End Function

End Module
