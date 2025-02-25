' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module RunningActiveInsulinRecordHelpers

    <Extension>
    Friend Sub AdjustList(myList As List(Of RunningActiveInsulin), startIndex As Integer, count As Integer)
        For i As Integer = startIndex To startIndex + count
            If i >= myList.Count Then Exit Sub
            myList(i) = myList(i).Adjust()
        Next
    End Sub

    <Extension>
    Friend Function ConditionalSum(myList As List(Of RunningActiveInsulin), start As Integer, length As Integer) As Double
        If start + length > myList.Count Then
            length = myList.Count - start
        End If
        Dim sum As Single = myList.GetRange(start, length).Sum(Function(i As RunningActiveInsulin) i.CurrentInsulinLevel)
        If sum < 0 Then sum = 0
        Return sum
    End Function

End Module
