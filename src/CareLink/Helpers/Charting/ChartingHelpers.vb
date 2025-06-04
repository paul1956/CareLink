' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module ChartingHelpers
    ''' <summary>
    '''  Check if the amount is the minimum basal rate
    ''' </summary>
    ''' <param name="amount">The amount to check.</param>
    ''' <returns>
    '''  <see langword="True"/> if the amount is the minimum basal rate; otherwise,
    '''  <see langword="False"/>.
    ''' </returns>
    <Extension>
    Friend Function IsMinBasal(amount As Single) As Boolean
        Return Math.Abs(amount - 0.025!) < 0.005
    End Function

End Module
