' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module RunningActiveInsulinHelpers

    ''' <summary>
    '''  Adjusts the insulin levels in a list of RunningActiveInsulin objects.
    '''  This method iterates through the specified range of indices in the list and adjusts each
    '''  RunningActiveInsulin object by calling its Adjust method.
    ''' </summary>
    ''' <param name="myList">The list of RunningActiveInsulin objects to adjust.</param>
    ''' <param name="start">The starting index in the list from which to begin adjustments.</param>
    ''' <param name="count">The number of RunningActiveInsulin objects to adjust starting from startIndex.</param>
    ''' <remarks>
    '''  This method modifies the RunningActiveInsulin objects in place.
    ''' </remarks>
    <Extension>
    Friend Sub AdjustList(myList As List(Of RunningActiveInsulin), start As Integer, count As Integer)
        ArgumentNullException.ThrowIfNull(argument:=myList)

        If start < 0 OrElse count < 0 Then
            Dim message As String = $"{NameOf(start)} and {NameOf(count)} must be non-negative."
            Throw New ArgumentOutOfRangeException(paramName:=NameOf(start), message)
        End If

        If start >= myList.Count Then Exit Sub

        ' Ensure we do not go out of bounds
        Dim endIndex As Integer = Math.Min(start + count, myList.Count)
        For i As Integer = start To endIndex - 1
            myList(index:=i) = myList(index:=i).Adjust()
        Next
    End Sub

    ''' <summary>
    '''  Calculates the conditional sum of the CurrentInsulinLevel property for a specified range in a list
    '''  of RunningActiveInsulin objects. This method sums the CurrentInsulinLevel values from
    '''  the specified start index to the specified length, ensuring that it does not exceed the bounds of the list.
    '''  If the sum is negative, it returns 0 instead. This is useful for calculating the total
    '''  active insulin over a specific period without exceeding the list boundaries.
    ''' </summary>
    ''' <param name="myList">The list of RunningActiveInsulin objects to sum.</param>
    ''' <param name="index">The starting index from which to sum.</param>
    ''' <param name="count">The number of elements to sum.</param>
    ''' <returns>The conditional sum of CurrentInsulinLevel.</returns>
    <Extension>
    Friend Function ConditionalSum(myList As List(Of RunningActiveInsulin), index As Integer, count As Integer) As Double
        If index + count > myList.Count Then
            count = myList.Count - index
        End If

        Dim selector As Func(Of RunningActiveInsulin, Single) =
            Function(i As RunningActiveInsulin) As Single
                Return i.CurrentInsulinLevel
            End Function
        Dim sum As Single = myList.GetRange(index, count).Sum(selector)
        If sum < 0 Then
            sum = 0
        End If
        Return sum
    End Function

End Module
