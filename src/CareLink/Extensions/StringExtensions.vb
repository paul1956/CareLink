' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module StringExtensions

    <Extension()>
    Friend Function ToTitleCase(inStr As String) As String
        Dim result As New Text.StringBuilder(Char.ToUpperInvariant(inStr(0)))

        For Each c As Char In inStr.Substring(1)
            If Char.IsLower(c) Then
                result.Append(c)
            Else
                result.Append($" {Char.ToUpperInvariant(c)}")
            End If
        Next
        Return result.ToString().Replace("time", " Time", False, Globalization.CultureInfo.CurrentUICulture)
    End Function

End Module
