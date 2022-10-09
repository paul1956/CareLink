' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Friend Module DoubleExtensions

    <Extension>
    Public Function OAToDateTime(d As Double) As String
        Return Date.FromOADate(d).ToShortDateTimeString
    End Function

    <Extension>
    Public Function OAToTime(d As Double) As String
        Return Date.FromOADate(d).ToShortTimeString
    End Function

End Module
