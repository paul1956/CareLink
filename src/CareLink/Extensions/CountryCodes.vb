' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices

Public Module RegionCountryLists
    Private ReadOnly s_countryCodeToCountry As New Dictionary(Of String, String) _
        (comparer:=StringComparer.OrdinalIgnoreCase)

    ''' <summary>
    '''  A dictionary mapping country names to their ISO 2-letter country codes.
    ''' </summary>
    Public ReadOnly s_countryCodeList As New Dictionary(Of String, String) From {
        {"United States", "US"},
        {"Albania", "AL"},
        {"Algeria", "DZ"},
        {"Argentina", "AR"},
        {"Armenia", "AM"},
        {"Aruba", "AW"},
        {"Australia", "AU"},
        {"Austria", "AT"},
        {"Azerbaijan", "AZ"},
        {"Bahamas", "BS"},
        {"Bahrain", "BH"},
        {"Bangladesh", "BD"},
        {"Barbados", "BB"},
        {"Belarus", "BY"},
        {"Belgium", "BE"},
        {"Bermuda", "BM"},
        {"Bolivia", "BO"},
        {"Bosnia-Herzegovina", "BA"},
        {"Botswana", "BW"},
        {"Brazil", "BR"},
        {"Bulgaria", "BG"},
        {"Canada", "CA"},
        {"Chile", "CL"},
        {"Colombia", "CO"},
        {"Costa Rica", "CR"},
        {"Croatia", "HR"},
        {"Curacao", "CW"},
        {"Cyprus", "CY"},
        {"Czech Republic", "CZ"},
        {"Denmark", "DK"},
        {"Dominican Republic", "DO"},
        {"Ecuador", "EC"},
        {"Egypt", "EG"},
        {"El Salvador", "SV"},
        {"Estonia", "EE"},
        {"Fiji", "FJ"},
        {"Finland", "FI"},
        {"France", "FR"},
        {"Georgia", "GE"},
        {"Germany", "DE"},
        {"Great Britain", "GB"},
        {"Greece", "GR"},
        {"Guatemala", "GT"},
        {"Honduras", "HN"},
        {"Hong Kong", "HK"},
        {"Hungary", "HU"},
        {"Iceland", "IS"},
        {"India", "IN"},
        {"Indonesia", "ID"},
        {"Iraq", "IQ"},
        {"Ireland", "IE"},
        {"Israel", "IL"},
        {"Italy", "IT"},
        {"Jamaica", "JM"},
        {"Japan", "JP"},
        {"Jordan", "JO"},
        {"Kazakhstan", "KZ"},
        {"Kenya", "KE"},
        {"Kosovo", "XK"},
        {"Kuwait", "KW"},
        {"Latvia", "LV"},
        {"Lebanon", "LB"},
        {"Libya", "LY"},
        {"Liechtenstein", "LI"},
        {"Lithuania", "LT"},
        {"Luxembourg", "LU"},
        {"Macau", "MO"},
        {"Macedonia", "MK"},
        {"Malaysia", "MY"},
        {"Maldives", "MV"},
        {"Malta", "MT"},
        {"Mauritius", "MU"},
        {"Mayotte", "YT"},
        {"Mexico", "MX"},
        {"Moldova", "MD"},
        {"Montenegro", "ME"},
        {"Morocco", "MA"},
        {"Namibia", "NA"},
        {"Netherlands", "NL"},
        {"New Caledonia", "NC"},
        {"New Zealand", "NZ"},
        {"Nicaragua", "NI"},
        {"Norway", "NO"},
        {"Oman", "OM"},
        {"Pakistan", "PK"},
        {"Panama", "PA"},
        {"Paraguay", "PY"},
        {"Peru", "PE"},
        {"Philippines", "PH"},
        {"Poland", "PL"},
        {"Portugal", "PT"},
        {"Qatar", "QA"},
        {"Romania", "RO"},
        {"Russia", "RU"},
        {"Saudi Arabia", "SA"},
        {"Serbia", "RS"},
        {"Singapore", "SG"},
        {"Slovakia", "SK"},
        {"Slovenia", "SI"},
        {"South Africa", "ZA"},
        {"South Korea", "KR"},
        {"Spain", "ES"},
        {"Sudan", "SD"},
        {"Sweden", "SE"},
        {"Switzerland", "CH"},
        {"Taiwan", "TW"},
        {"Thailand", "TH"},
        {"Trinidad & Tobago", "TT"},
        {"Tunisia", "TN"},
        {"Turkey", "TR"},
        {"Ukraine", "UA"},
        {"United Arab Emirates", "AE"},
        {"United Kingdom", "GB"},
        {"Uruguay", "UY"},
        {"Uzbekistan", "UZ"},
        {"Venezuela", "VE"},
        {"Vietnam", "VN"}}

    ''' <summary>
    '''  A dictionary mapping country names to their corresponding regions.
    ''' </summary>
    ''' <remarks>
    '''  The regions are defined as per the ISO 3166-1 standard,
    '''  grouping countries into continents or major geographic areas.
    ''' </remarks>
    Public ReadOnly s_regionCountryList As New Dictionary(Of String, String) From {
        {"United States", "North America"},
        {"Albania", "Europe"},
        {"Algeria", "Africa"},
        {"Argentina", "South America"},
        {"Armenia", "Asia"},
        {"Aruba", "South America"},
        {"Australia", "Oceania"},
        {"Austria", "Europe"},
        {"Azerbaijan", "Asia"},
        {"Bahamas", "North America"},
        {"Bahrain", "Asia"},
        {"Bangladesh", "Asia"},
        {"Barbados", "North America"},
        {"Belarus", "Europe"},
        {"Belgium", "Europe"},
        {"Bermuda", "North America"},
        {"Bolivia", "South America"},
        {"Bosnia-Herzegovina", "Europe"},
        {"Botswana", "Africa"},
        {"Brazil", "South America"},
        {"Bulgaria", "Europe"},
        {"Canada", "North America"},
        {"Chile", "South America"},
        {"Colombia", "South America"},
        {"Costa Rica", "North America"},
        {"Croatia", "Europe"},
        {"Curacao", "South America"},
        {"Cyprus", "Europe"},
        {"Czech Republic", "Europe"},
        {"Denmark", "Europe"},
        {"Dominican Republic", "North America"},
        {"Ecuador", "South America"},
        {"Egypt", "Africa"},
        {"El Salvador", "North America"},
        {"Estonia", "Europe"},
        {"Fiji", "Oceania"},
        {"Finland", "Europe"},
        {"France", "Europe"},
        {"Georgia", "Europe"},
        {"Germany", "Europe"},
        {"Great Britain", "Europe"},
        {"Greece", "Europe"},
        {"Guatemala", "North America"},
        {"Honduras", "North America"},
        {"Hong Kong", "Asia"},
        {"Hungary", "Europe"},
        {"Iceland", "Europe"},
        {"India", "Asia"},
        {"Indonesia", "Asia"},
        {"Iraq", "Asia"},
        {"Ireland", "Europe"},
        {"Israel", "Asia"},
        {"Italy", "Europe"},
        {"Jamaica", "North America"},
        {"Japan", "Asia"},
        {"Jordan", "Asia"},
        {"Kazakhstan", "Asia"},
        {"Kenya", "Africa"},
        {"Kosovo", "Europe"},
        {"Kuwait", "Asia"},
        {"Latvia", "Europe"},
        {"Lebanon", "Asia"},
        {"Libya", "Africa"},
        {"Liechtenstein", "Europe"},
        {"Lithuania", "Europe"},
        {"Luxembourg", "Europe"},
        {"Macau", "Asia"},
        {"Malaysia", "Asia"},
        {"Maldives", "Asia"},
        {"Malta", "Europe"},
        {"Macedonia", "Europe"},
        {"Mauritius", "Africa"},
        {"Mayotte", "Africa"},
        {"Mexico", "North America"},
        {"Moldova", "Europe"},
        {"Montenegro", "Europe"},
        {"Morocco", "Africa"},
        {"Namibia", "Africa"},
        {"Netherlands", "Europe"},
        {"New Caledonia", "Oceania"},
        {"New Zealand", "Oceania"},
        {"Nicaragua", "North America"},
        {"Norway", "Europe"},
        {"Oman", "Asia"},
        {"Pakistan", "Asia"},
        {"Panama", "North America"},
        {"Paraguay", "South America"},
        {"Peru", "South America"},
        {"Philippines", "Asia"},
        {"Poland", "Europe"},
        {"Portugal", "Europe"},
        {"Qatar", "Asia"},
        {"Romania", "Europe"},
        {"Russia", "Europe"},
        {"Saudi Arabia", "Asia"},
        {"Serbia", "Europe"},
        {"Singapore", "Asia"},
        {"Slovakia", "Europe"},
        {"Slovenia", "Europe"},
        {"South Africa", "Africa"},
        {"South Korea", "Asia"},
        {"Spain", "Europe"},
        {"Sudan", "Africa"},
        {"Sweden", "Europe"},
        {"Switzerland", "Europe"},
        {"Taiwan", "Asia"},
        {"Thailand", "Asia"},
        {"Trinidad & Tobago", "South America"},
        {"Tunisia", "Africa"},
        {"Turkey", "Asia"},
        {"Ukraine", "Europe"},
        {"United Arab Emirates", "Asia"},
        {"United Kingdom", "Europe"},
        {"Uruguay", "South America"},
        {"Uzbekistan", "Asia"},
        {"Venezuela", "South America"},
        {"Vietnam", "Asia"}}

    ''' <summary>
    '''  A dictionary mapping region names to themselves for region validation and lookup.
    ''' </summary>
    Public ReadOnly s_regionList As New Dictionary(Of String, String) From {
        {"North America", "North America"},
        {"Africa", "Africa"},
        {"Asia", "Asia"},
        {"Europe", "Europe"},
        {"Oceania", "Oceania"},
        {"South America", "South America"}}

    ''' <summary>
    '''  Extracts the <see cref="CultureInfo"/> from a report file name.
    ''' </summary>
    ''' <param name="ReportFileNameWithPath">The full path or name of the report file.</param>
    ''' <param name="FixedPart">
    '''  The fixed prefix part of the file name before the culture info.
    ''' </param>
    ''' <param name="fuzzy">
    '''  If <see langword="True"/>, allows the '(' to appear after the fixed part,
    '''  not necessarily immediately after.
    ''' </param>
    ''' <returns>
    '''  The <see cref="CultureInfo"/> extracted from the file name,
    '''  or <see langword="Nothing"/> if extraction fails.
    '''  If the culture name is invalid, returns <see cref="CultureInfo.CurrentCulture"/>.
    ''' </returns>
    <Extension>
    Public Function ExtractCulture(
        ReportFileNameWithPath As String,
        FixedPart As String,
        Optional fuzzy As Boolean = False) As CultureInfo

        Dim filename As String =
            Path.GetFileNameWithoutExtension(ReportFileNameWithPath)
        Dim prompt As String

        If filename.Count(c:="("c) = 0 Then
            prompt = $"'{filename}' malformed,{vbCrLf}it must contain at least one '('."
            MsgBox(
                heading:="Invalid Filename",
                prompt,
                buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                title:="Malformed Error Report Filename")
            Return Nothing
        End If

        If filename.Count(")"c) = 0 Then
            prompt =
                $"Filename '{filename}' malformed,{vbCrLf}it must contain at least one ')'."
            MsgBox(
                heading:="Invalid Filename",
                prompt,
                buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                title:="Malformed Error Report Filename")
            Return Nothing
        End If

        If Not filename.StartsWith(value:=FixedPart) Then
            prompt =
                $"Filename '{filename}' malformed,{vbCrLf}it must start with '{FixedPart}'."
            MsgBox(
                heading:="Invalid Filename",
                prompt,
                buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                title:="Malformed Error Report Filename")
            Return Nothing
        End If

        Dim indexOfOpenParenthesis As Integer = filename.IndexOf(value:="("c)
        prompt =
            $"Filename '{filename}' malformed,{vbCrLf}" &
            $"it must contain '(' after '{FixedPart}'."
        If fuzzy Then
            If indexOfOpenParenthesis < FixedPart.Length Then
                MsgBox(
                    heading:="Invalid Filename",
                    prompt,
                    buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                    title:="Malformed Error Report Filename")
                Return Nothing
            End If
        Else
            prompt =
                $"Filename '{filename}' malformed," &
                $"{vbCrLf}it must contain '(' immediately after '{FixedPart}'."
            If indexOfOpenParenthesis <> FixedPart.Length Then
                MsgBox(
                    heading:="Invalid Filename",
                    prompt,
                    buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                    title:="Malformed Error Report Filename")
                Return Nothing
            End If
        End If

        Dim indexOfClosedParenthesis As Integer = filename.IndexOf(")"c)
        If indexOfClosedParenthesis < 0 Then
            MsgBox(
                heading:="Invalid Filename",
                prompt:=$"Filename '{filename}' malformed,{vbCrLf}it must contain ')'.",
                buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                title:="Malformed Error Report Filename")
            Return Nothing
        End If

        Dim startIndex As Integer = indexOfOpenParenthesis + 1
        Dim length As Integer = indexOfClosedParenthesis - indexOfOpenParenthesis - 1
        Dim cultureName As String = filename.Substring(startIndex, length)

        Dim fileNameInvalid As Boolean = Not CultureInfoList.Any(
            predicate:=Function(c As CultureInfo) c.Name = cultureName)

        If fileNameInvalid Then
            MsgBox(
                heading:="Invalid Filename",
                prompt:=$"Culture name '{cultureName}' is not a valid culture name.",
                buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                title:="Invalid Culture Name")
            Return CultureInfo.CurrentCulture
        End If
        Return CultureInfo.GetCultureInfo(cultureName)
    End Function

    ''' <summary>
    '''  Gets the country name corresponding to a given ISO 2-letter country code.
    ''' </summary>
    ''' <param name="countryCode">The ISO 2-letter country code.</param>
    ''' <returns>The country name if found; otherwise, <see langword="Nothing"/></returns>
    <Extension>
    Public Function GetCountryFromCode(countryCode As String) As String
        Debug.Assert(condition:=countryCode.Length = 2)
        If s_countryCodeToCountry.Count = 0 Then
            ' Create the reverse lookup Dictionary only once
            For Each kvp As KeyValuePair(Of String, String) In s_countryCodeList
                s_countryCodeToCountry(key:=kvp.Value) = kvp.Key
            Next
        End If

        Dim value As String = Nothing
        Return If(s_countryCodeToCountry.TryGetValue(key:=countryCode, value),
                  value,
                  "US")
    End Function

    ''' <summary>
    '''  Gets the region name for a given ISO 2-letter country code.
    ''' </summary>
    ''' <param name="countryCode">The ISO 2-letter country code.</param>
    ''' <returns>
    '''  The region name if found; otherwise, defaults to "North America" (for "US").
    ''' </returns>
    <Extension>
    Public Function GetRegionFromCode(countryCode As String) As String
        If String.IsNullOrWhiteSpace(countryCode) Then
            countryCode = "US"
        End If
        Return s_regionCountryList(GetCountryFromCode(countryCode))
    End Function

End Module
