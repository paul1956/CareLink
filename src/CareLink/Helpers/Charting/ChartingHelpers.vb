' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module ChartingHelpers

    ''' <summary>
    '''  Check if the amount is the minimum basal rate or 0
    '''  (0.025 units per hour) considering floating-point precision.
    '''  This is used to determine if the basal rate is effectively zero for charting purposes.
    ''' </summary>
    ''' <param name="value">The amount to check.</param>
    ''' <returns>
    '''  <see langword="True"/> if the amount is the minimum basal rate; otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Friend Function IsMinBasal(value As Single) As Boolean
        If value < 0.025 OrElse Math.Abs(value:=value - 0.025) < 0.0005 Then
            Return True  ' amount is <= 0.025, considering floating-point precision
        End If
        Return False
    End Function

End Module
