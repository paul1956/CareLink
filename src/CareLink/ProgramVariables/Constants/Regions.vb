' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel

Public Module ServerLocations

    Public Enum ServerLocation As Integer

        <Description("us")>
        US

        <Description("eu")>
        Eu

        <Description("clinical")>
        Clinical

    End Enum

End Module
