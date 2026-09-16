' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization

Friend Module FlagHelpers

    Public Function GetFlag(country As String) As String
        If country = "Clinical" Then
            Return "⚕️"
        End If
        Dim cultures As CultureInfo() =
            CultureInfo.GetCultures(types:=CultureTypes.SpecificCultures)
        Dim selector As Func(Of CultureInfo, RegionInfo) =
            Function(x As CultureInfo) As RegionInfo
                ' Use the culture name (string) to avoid LCID-to-string/overload issues under Option Strict
                Return New RegionInfo(x.Name)
            End Function
        Dim regions As IEnumerable(Of RegionInfo) = cultures.Select(selector)
        Dim predicate As Func(Of RegionInfo, Boolean) =
            Function(region As RegionInfo) As Boolean
                Return region.EnglishName.Contains(value:=country)
            End Function
        Dim englishRegion As RegionInfo = regions.FirstOrDefault(predicate)
        If englishRegion Is Nothing Then
            Return "" ' To avoid null exceptions
        End If
        Dim countryCode As String = englishRegion.TwoLetterISORegionName
        Return IsoCountryCodeToFlagEmoji(countryCode)
    End Function

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
