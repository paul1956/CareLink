' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module BasalHelpers

    ''' <summary>
    '''  Get the minimum basal rate based on the pump type.
    ''' </summary>
    ''' <returns>The minimum basal rate.</returns>
    Public Function GetMinBasal() As Single
        Return If(IsFlex(),
                  0.05F,
                  0.025F)
    End Function

    ''' <summary>
    '''  Check if the amount is the minimum basal rate or 0
    '''  (minBasal units per hour) considering floating-point precision.
    '''  This is used to determine if the basal rate is effectively zero
    '''  for charting purposes.
    ''' </summary>
    ''' <param name="value">The amount to check.</param>
    ''' <returns>
    '''  <see langword="True"/> if the amount is the minimum basal rate;
    '''  otherwise, <see langword="False"/>.
    ''' </returns>
    <Extension>
    Public Function IsMinBasal(value As Single) As Boolean
        Const tolerance As Single = 0.0005
        Dim minBasal As Single = GetMinBasal()
        Return value <= minBasal + tolerance
    End Function

    ''' <summary>
    '''  Rounds a Single value to the nearest MinBasal increment.
    ''' </summary>
    ''' <param name="f">The Single value to round.</param>
    ''' <returns>
    '''  The rounded Single value, or <see cref="Single.NaN"/> if the input is NaN.
    ''' </returns>
    ''' <param name="isFlex"></param>
    <Extension>
    Public Function RoundToStep(f As Single, isFlex As Boolean) As Single
        Return If(Single.IsNaN(f),
                  Single.NaN,
                  CDbl(f).RoundToStep(isFlex))
    End Function

    ''' <summary>
    '''  Rounds a Double value to the nearest MinBasal increment and returns as Single.
    ''' </summary>
    ''' <param name="d">The Double value to round.</param>
    ''' <returns>
    '''  The rounded value as Single, or <see cref="Single.NaN"/> if the input is NaN.
    ''' </returns>
    ''' <remarks>
    '''  Uses an inverse multiplier of 1/MinBasal to scale the value, perform integer
    '''  rounding, then rescale back to the original magnitude.
    ''' </remarks>
    ''' <summary>
    '''  Rounds a number to the nearest step (0.05 or 0.025) based on a flag.
    ''' </summary>
    ''' <param name="value">The number to round.</param>
    ''' <param name="isFlex">
    '''  If <see langword="True"/>, rounds to the nearest 0.05;
    '''  otherwise, rounds to the nearest 0.025.
    ''' </param>
    ''' <returns>The rounded number.</returns>
    <Extension>
    Public Function RoundToStep(value As Double, isFlex As Boolean) As Single
        ' Determine step size using a conditional expression
        ' not function IsFlex() to support testing
        Dim stepSize As Double = If(isFlex, 0.05, 0.025)

        ' Handle NaN or Infinity
        If Double.IsNaN(value) OrElse Double.IsInfinity(value) Then
            Return Double.NaN
        End If

        ' Perform rounding
        Return CSng(Math.Round(value:=value / stepSize,
                               mode:=MidpointRounding.AwayFromZero) * stepSize)
    End Function

End Module
