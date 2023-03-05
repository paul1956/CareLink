' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module ChartingHelpers

    Public Function GetGraphLineColor(lineName As String) As Color
        Return GraphColorDictionary(lineName).ToColor
    End Function

    <Extension>
    Friend Function IsMinBasal(amount As Single) As Boolean
        Return Math.Abs(amount - 0.025) < 0.001
    End Function

    <Extension>
    Friend Function IsMinBasal(amount As String) As Boolean
        Return amount = "0.025"
    End Function

End Module
