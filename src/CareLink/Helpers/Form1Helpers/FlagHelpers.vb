' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module FlagHelpers

    Public Function IsoCountryCodeToFlagEmoji(countryCode As String) As String
        If countryCode = "Clinical" Then
            Return "⚕️"
        End If
        Dim selector As Func(Of Char, String) =
            Function(x As Char) As String
                ' Use AscW to get the Unicode code point of the Char explicitly
                Dim codePoint As Integer = AscW(x)
                Dim utf32 As Integer = codePoint + 127397
                Return Char.ConvertFromUtf32(utf32)
            End Function
        Return If(String.IsNullOrEmpty(value:=countryCode),
                  String.Empty,
                  String.Concat(values:=countryCode.ToUpper().Select(selector)))
    End Function

End Module
