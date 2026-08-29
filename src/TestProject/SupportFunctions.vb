' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports CareLink

Public Module SupportFunctions

    Public Sub RestoreDefaults()
        NativeMmolL = False
        DecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
    End Sub

End Module
