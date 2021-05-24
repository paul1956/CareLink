' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module DateExtensions
    <Extension()>
    Public Function RoundToHour(sgDateTime As Date) As Date
        Return New DateTime(sgDateTime.Year, sgDateTime.Month, sgDateTime.Day, sgDateTime.Hour, 0, 0)
    End Function

End Module
