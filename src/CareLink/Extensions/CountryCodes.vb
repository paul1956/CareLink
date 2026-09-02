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
    '''  A dictionary mapping country names to their corresponding regions.
    ''' </summary>
    ''' <remarks>
    '''  The regions are defined as per the ISO 3166-1 standard,
    '''  grouping countries into continents or major geographic areas.
    ''' </remarks>
    Public ReadOnly CountryNameToRegionList As New Dictionary(Of String, WorldRegion) From {
        {"United States", WorldRegion.UnitedStates},
        {"Afghanistan", WorldRegion.Asia},
        {"Åland Islands", WorldRegion.Europe},
        {"Albania", WorldRegion.Europe},
        {"Algeria", WorldRegion.Africa},
        {"American Samoa", WorldRegion.Oceania},
        {"Andorra", WorldRegion.Europe},
        {"Angola", WorldRegion.Africa},
        {"Anguilla", WorldRegion.NorthAmerica},
        {"Antigua & Barbuda", WorldRegion.NorthAmerica},
        {"Argentina", WorldRegion.SouthAmerica},
        {"Armenia", WorldRegion.Transcontinental},
        {"Aruba", WorldRegion.NorthAmerica},
        {"Australia", WorldRegion.Oceania},
        {"Austria", WorldRegion.Europe},
        {"Azerbaijan", WorldRegion.Transcontinental},
        {"Bahamas", WorldRegion.NorthAmerica},
        {"Bahrain", WorldRegion.Asia},
        {"Bangladesh", WorldRegion.Asia},
        {"Barbados", WorldRegion.NorthAmerica},
        {"Belarus", WorldRegion.Europe},
        {"Belgium", WorldRegion.Europe},
        {"Belize", WorldRegion.NorthAmerica},
        {"Benin", WorldRegion.Africa},
        {"Bermuda", WorldRegion.NorthAmerica},
        {"Bhutan", WorldRegion.Asia},
        {"Bolivia", WorldRegion.SouthAmerica},
        {"Bonaire, Sint Eustatius & Saba", WorldRegion.NorthAmerica},
        {"Bosnia & Herzegovina", WorldRegion.Europe},
        {"Botswana", WorldRegion.Africa},
        {"Brazil", WorldRegion.SouthAmerica},
        {"British Virgin Islands", WorldRegion.NorthAmerica},
        {"Brunei Darussalam", WorldRegion.Asia},
        {"Bulgaria", WorldRegion.Europe},
        {"Burkina Faso", WorldRegion.Africa},
        {"Burundi", WorldRegion.Africa},
        {"Cabo Verde", WorldRegion.Africa},
        {"Cambodia", WorldRegion.Asia},
        {"Cameroon", WorldRegion.Africa},
        {"Canada", WorldRegion.NorthAmerica},
        {"Cayman Islands", WorldRegion.NorthAmerica},
        {"Central African Republic", WorldRegion.Africa},
        {"Chad", WorldRegion.Africa},
        {"Chile", WorldRegion.SouthAmerica},
        {"China", WorldRegion.Asia},
        {"Christmas Island", WorldRegion.Oceania},
        {"Colombia", WorldRegion.SouthAmerica},
        {"Comoros (the)", WorldRegion.Africa},
        {"Cook Islands (the)", WorldRegion.Oceania},
        {"Costa Rica", WorldRegion.NorthAmerica},
        {"Côte d'Ivoire", WorldRegion.Africa},
        {"Croatia", WorldRegion.Europe},
        {"Cuba", WorldRegion.NorthAmerica},
        {"Curaçao", WorldRegion.NorthAmerica},
        {"Cyprus", WorldRegion.Transcontinental},
        {"Czechia", WorldRegion.Europe},
        {"Denmark", WorldRegion.Europe},
        {"Djibouti", WorldRegion.Africa},
        {"Dominica", WorldRegion.NorthAmerica},
        {"Dominican Republic", WorldRegion.NorthAmerica},
        {"Ecuador", WorldRegion.SouthAmerica},
        {"Egypt", WorldRegion.Africa},
        {"El Salvador", WorldRegion.NorthAmerica},
        {"Equatorial Guinea", WorldRegion.Africa},
        {"Eritrea", WorldRegion.Africa},
        {"Estonia", WorldRegion.Europe},
        {"Eswatini", WorldRegion.Africa},
        {"Ethiopia", WorldRegion.Africa},
        {"Falkland Islands (the) [Malvinas]", WorldRegion.SouthAmerica},
        {"Faroe Islands (the)", WorldRegion.Europe},
        {"Fiji", WorldRegion.Oceania},
        {"Finland", WorldRegion.Europe},
        {"France", WorldRegion.Europe},
        {"French Guiana", WorldRegion.SouthAmerica},
        {"French Polynesia", WorldRegion.Oceania},
        {"French Southern Territories (the)", WorldRegion.Oceania},
        {"Gabon", WorldRegion.Africa},
        {"Gambia (the)", WorldRegion.Africa},
        {"Georgia", WorldRegion.Transcontinental},
        {"Germany", WorldRegion.Europe},
        {"Ghana", WorldRegion.Africa},
        {"Gibraltar", WorldRegion.Europe},
        {"Greece", WorldRegion.Europe},
        {"Greenland", WorldRegion.NorthAmerica},
        {"Grenada", WorldRegion.NorthAmerica},
        {"Guadeloupe", WorldRegion.NorthAmerica},
        {"Guam", WorldRegion.Oceania},
        {"Guatemala", WorldRegion.NorthAmerica},
        {"Guernsey", WorldRegion.Europe},
        {"Guinea (the)", WorldRegion.Africa},
        {"Guinea-Bissau", WorldRegion.Africa},
        {"Guyana", WorldRegion.SouthAmerica},
        {"Haiti", WorldRegion.NorthAmerica},
        {"Heard Island & McDonald Islands", WorldRegion.Oceania},
        {"Honduras", WorldRegion.NorthAmerica},
        {"Hong Kong (SAR China)", WorldRegion.Asia},
        {"Hungary", WorldRegion.Europe},
        {"Iceland", WorldRegion.Europe},
        {"India", WorldRegion.Asia},
        {"Indonesia", WorldRegion.Asia},
        {"Iraq", WorldRegion.Asia},
        {"Ireland", WorldRegion.Europe},
        {"Isle of Man", WorldRegion.Europe},
        {"Israel", WorldRegion.Asia},
        {"Italy", WorldRegion.Europe},
        {"Jamaica", WorldRegion.NorthAmerica},
        {"Japan", WorldRegion.Asia},
        {"Jersey", WorldRegion.Europe},
        {"Jordan", WorldRegion.Asia},
        {"Kazakhstan", WorldRegion.Transcontinental},
        {"Kenya", WorldRegion.Africa},
        {"Kiribati", WorldRegion.Oceania},
        {"Kosovo", WorldRegion.Europe},
        {"Kuwait", WorldRegion.Asia},
        {"Kyrgyzstan", WorldRegion.Asia},
        {"Laos", WorldRegion.Asia},
        {"Latvia", WorldRegion.Europe},
        {"Lebanon", WorldRegion.Asia},
        {"Lesotho", WorldRegion.Africa},
        {"Liberia", WorldRegion.Africa},
        {"Libya", WorldRegion.Africa},
        {"Liechtenstein", WorldRegion.Europe},
        {"Lithuania", WorldRegion.Europe},
        {"Luxembourg", WorldRegion.Europe},
        {"Macao", WorldRegion.Asia},
        {"Madagascar", WorldRegion.Africa},
        {"Malawi", WorldRegion.Africa},
        {"Malaysia", WorldRegion.Asia},
        {"Maldives", WorldRegion.Asia},
        {"Malta", WorldRegion.Europe},
        {"Marshall Islands", WorldRegion.Oceania},
        {"Martinique", WorldRegion.NorthAmerica},
        {"Mauritania", WorldRegion.Africa},
        {"Mauritius", WorldRegion.Africa},
        {"Mayotte", WorldRegion.Africa},
        {"Mexico", WorldRegion.NorthAmerica},
        {"Micronesia", WorldRegion.Oceania},
        {"Moldova", WorldRegion.Europe},
        {"Monaco", WorldRegion.Europe},
        {"Mongolia", WorldRegion.Asia},
        {"Montenegro", WorldRegion.Europe},
        {"Montserrat", WorldRegion.Europe},
        {"Morocco", WorldRegion.Africa},
        {"Mozambique", WorldRegion.Africa},
        {"Myanmar", WorldRegion.Asia},
        {"Namibia", WorldRegion.Africa},
        {"Nauru", WorldRegion.Oceania},
        {"Nepal", WorldRegion.Asia},
        {"Netherlands", WorldRegion.Europe},
        {"New Caledonia", WorldRegion.Oceania},
        {"New Zealand", WorldRegion.Oceania},
        {"Nicaragua", WorldRegion.NorthAmerica},
        {"Niger", WorldRegion.Africa},
        {"Nigeria", WorldRegion.Africa},
        {"Niue", WorldRegion.Oceania},
        {"Norfolk Island", WorldRegion.Oceania},
        {"North Macedonia", WorldRegion.Europe},
        {"Northern Mariana Islands", WorldRegion.Oceania},
        {"Norway", WorldRegion.Europe},
        {"Oman", WorldRegion.Asia},
        {"Pakistan", WorldRegion.Asia},
        {"Palau", WorldRegion.Oceania},
        {"Panama", WorldRegion.NorthAmerica},
        {"Papua New Guinea", WorldRegion.Oceania},
        {"Paraguay", WorldRegion.SouthAmerica},
        {"Peru", WorldRegion.SouthAmerica},
        {"Philippines", WorldRegion.Asia},
        {"Pitcairn", WorldRegion.Oceania},
        {"Poland", WorldRegion.Europe},
        {"Portugal", WorldRegion.Europe},
        {"Puerto Rico", WorldRegion.UnitedStates},
        {"Qatar", WorldRegion.Asia},
        {"Republic of the Congo", WorldRegion.Africa},
        {"Réunion", WorldRegion.Africa},
        {"Romania", WorldRegion.Europe},
        {"Russia", WorldRegion.Transcontinental},
        {"Rwanda", WorldRegion.Africa},
        {"Saint Barthélemy", WorldRegion.NorthAmerica},
        {"Saint Helena", WorldRegion.Africa},
        {"Saint Kitts & Nevis", WorldRegion.NorthAmerica},
        {"Saint Lucia", WorldRegion.NorthAmerica},
        {"Saint Martin", WorldRegion.NorthAmerica},
        {"Saint Pierre & Miquelon", WorldRegion.NorthAmerica},
        {"Saint Vincent & the Grenadines", WorldRegion.NorthAmerica},
        {"Samoa", WorldRegion.Oceania},
        {"San Marino", WorldRegion.Europe},
        {"São Tomé & Príncipe", WorldRegion.Africa},
        {"Saudi Arabia", WorldRegion.Asia},
        {"Senegal", WorldRegion.Africa},
        {"Serbia", WorldRegion.Europe},
        {"Seychelles", WorldRegion.Africa},
        {"Sierra Leone", WorldRegion.Africa},
        {"Singapore", WorldRegion.Asia},
        {"Sint Maarten", WorldRegion.NorthAmerica},
        {"Slovakia", WorldRegion.Europe},
        {"Slovenia", WorldRegion.Europe},
        {"Solomon Islands", WorldRegion.Oceania},
        {"Somalia", WorldRegion.Africa},
        {"South Africa", WorldRegion.Africa},
        {"South Georgia & the South Sandwich Islands", WorldRegion.Oceania},
        {"South Korea", WorldRegion.Asia},
        {"Spain", WorldRegion.Europe},
        {"Sudan", WorldRegion.Africa},
        {"Suriname", WorldRegion.SouthAmerica},
        {"Svalbard & Jan Mayen", WorldRegion.Europe},
        {"Sweden", WorldRegion.Europe},
        {"Switzerland", WorldRegion.Europe},
        {"Syrian Arab Republic", WorldRegion.Asia},
        {"Taiwan", WorldRegion.Asia},
        {"Tajikistan", WorldRegion.Asia},
        {"Tanzania", WorldRegion.Africa},
        {"Thailand", WorldRegion.Asia},
        {"Timor-Leste", WorldRegion.Asia},
        {"Togo", WorldRegion.Africa},
        {"Tokelau", WorldRegion.Oceania},
        {"Tonga", WorldRegion.Oceania},
        {"Trial", WorldRegion.Trial},
        {"Trinidad & Tobago", WorldRegion.NorthAmerica},
        {"Tunisia", WorldRegion.Africa},
        {"Turkey", WorldRegion.Transcontinental},
        {"Turkmenistan", WorldRegion.Asia},
        {"Turks & Caicos Islands", WorldRegion.NorthAmerica},
        {"Tuvalu", WorldRegion.Oceania},
        {"Uganda", WorldRegion.Africa},
        {"Ukraine", WorldRegion.Europe},
        {"United Arab Emirates", WorldRegion.Asia},
        {"United Kingdom", WorldRegion.Europe},
        {"Uruguay", WorldRegion.SouthAmerica},
        {"Uzbekistan", WorldRegion.Asia},
        {"Vanuatu", WorldRegion.Oceania},
        {"Venezuela", WorldRegion.SouthAmerica},
        {"Vietnam", WorldRegion.Asia},
        {"Virgin Islands (British)", WorldRegion.NorthAmerica},
        {"Virgin Islands (U.S.)", WorldRegion.NorthAmerica},
        {"Wallis & Futuna", WorldRegion.Oceania},
        {"Western Sahara", WorldRegion.Africa},
        {"Yemen", WorldRegion.Asia},
        {"Zambia", WorldRegion.Africa},
        {"Zimbabwe", WorldRegion.Africa}}

    ''' <summary>
    '''  A dictionary mapping country names to their ISO 2-letter country codes.
    ''' </summary>
    Public ReadOnly CountryToCodeList As New Dictionary(Of String, String) From {
        {"Afghanistan", "AF"},
        {"Åland Islands", "AX"},
        {"Albania", "AL"},
        {"Algeria", "DZ"},
        {"American Samoa", "AS"},
        {"Andorra", "AD"},
        {"Angola", "AO"},
        {"Anguilla", "AI"},
        {"Antigua & Barbuda", "AG"},
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
        {"Belize", "BZ"},
        {"Benin", "BJ"},
        {"Bermuda", "BM"},
        {"Bhutan", "BT"},
        {"Bolivia", "BO"},
        {"Bonaire, Sint Eustatius & Saba", "BQ"},
        {"Bosnia & Herzegovina", "BA"},
        {"Botswana", "BW"},
        {"Brazil", "BR"},
        {"British Indian Ocean Territory", "IO"},
        {"British Virgin Islands", "VG"},
        {"Brunei Darussalam", "BN"},
        {"Bulgaria", "BG"},
        {"Burkina Faso", "BF"},
        {"Burundi", "BI"},
        {"Cabo Verde", "CV"},
        {"Cambodia", "KH"},
        {"Cameroon", "CM"},
        {"Canada", "CA"},
        {"Cayman Islands", "KY"},
        {"Central African Republic", "CF"},
        {"Chad", "TD"},
        {"Chile", "CL"},
        {"China", "CN"},
        {"Christmas Island", "CX"},
        {"Colombia", "CO"},
        {"Comoros (the)", "KM"},
        {"Cook Islands (the)", "CK"},
        {"Costa Rica", "CR"},
        {"Côte d'Ivoire", "CI"},
        {"Croatia", "HR"},
        {"Cuba", "CU"},
        {"Curaçao", "CW"},
        {"Cyprus", "CY"},
        {"Czechia", "CZ"},
        {"Denmark", "DK"},
        {"Djibouti", "DJ"},
        {"Dominica", "DM"},
        {"Dominican Republic", "DO"},
        {"Ecuador", "EC"},
        {"Egypt", "EG"},
        {"El Salvador", "SV"},
        {"Equatorial Guinea", "GQ"},
        {"Eritrea", "ER"},
        {"Estonia", "EE"},
        {"Eswatini", "SZ"},
        {"Ethiopia", "ET"},
        {"Falkland Islands (the) [Malvinas]", "FK"},
        {"Faroe Islands (the)", "FO"},
        {"Fiji", "FJ"},
        {"Finland", "FI"},
        {"France", "FR"},
        {"French Guiana", "GF"},
        {"French Polynesia", "PF"},
        {"French Southern Territories (the)", "TF"},
        {"Gabon", "GA"},
        {"Gambia (the)", "GM"},
        {"Georgia", "GE"},
        {"Germany", "DE"},
        {"Ghana", "GH"},
        {"Gibraltar", "GI"},
        {"Greece", "GR"},
        {"Greenland", "GL"},
        {"Grenada", "GD"},
        {"Guadeloupe", "GP"},
        {"Guam", "GU"},
        {"Guatemala", "GT"},
        {"Guernsey", "GG"},
        {"Guinea (the)", "GN"},
        {"Guinea-Bissau", "GW"},
        {"Guyana", "GY"},
        {"Haiti", "HT"},
        {"Heard Island & McDonald Islands", "HM"},
        {"Honduras", "HN"},
        {"Hong Kong (SAR China)", "HK"},
        {"Hungary", "HU"},
        {"Iceland", "IS"},
        {"India", "IN"},
        {"Indonesia", "ID"},
        {"Iraq", "IQ"},
        {"Ireland", "IE"},
        {"Isle of Man", "IM"},
        {"Israel", "IL"},
        {"Italy", "IT"},
        {"Jamaica", "JM"},
        {"Japan", "JP"},
        {"Jersey", "JE"},
        {"Jordan", "JO"},
        {"Kazakhstan", "KZ"},
        {"Kenya", "KE"},
        {"Kiribati", "KI"},
        {"Kosovo", "XK"},
        {"Kuwait", "KW"},
        {"Kyrgyzstan", "KG"},
        {"Laos", "LA"},
        {"Latvia", "LV"},
        {"Lebanon", "LB"},
        {"Lesotho", "LS"},
        {"Liberia", "LR"},
        {"Libya", "LY"},
        {"Liechtenstein", "LI"},
        {"Lithuania", "LT"},
        {"Luxembourg", "LU"},
        {"Macao", "MO"},
        {"Madagascar", "MG"},
        {"Malawi", "MW"},
        {"Malaysia", "MY"},
        {"Maldives", "MV"},
        {"Malta", "MT"},
        {"Marshall Islands", "MH"},
        {"Martinique", "MQ"},
        {"Mauritania", "MR"},
        {"Mauritius", "MU"},
        {"Mayotte", "YT"},
        {"Mexico", "MX"},
        {"Micronesia", "FM"},
        {"Moldova", "MD"},
        {"Monaco", "MC"},
        {"Mongolia", "MN"},
        {"Montenegro", "ME"},
        {"Montserrat", "MS"},
        {"Morocco", "MA"},
        {"Mozambique", "MZ"},
        {"Myanmar", "MM"},
        {"Namibia", "NA"},
        {"Nauru", "NR"},
        {"Nepal", "NP"},
        {"Netherlands", "NL"},
        {"New Caledonia", "NC"},
        {"New Zealand", "NZ"},
        {"Nicaragua", "NI"},
        {"Niger", "NE"},
        {"Nigeria", "NG"},
        {"Niue", "NU"},
        {"Norfolk Island", "NF"},
        {"North Macedonia", "MK"},
        {"Northern Mariana Islands", "MP"},
        {"Norway", "NO"},
        {"Oman", "OM"},
        {"Pakistan", "PK"},
        {"Palau", "PW"},
        {"Panama", "PA"},
        {"Papua New Guinea", "PG"},
        {"Paraguay", "PY"},
        {"Peru", "PE"},
        {"Philippines", "PH"},
        {"Pitcairn", "PN"},
        {"Poland", "PL"},
        {"Portugal", "PT"},
        {"Puerto Rico", "PR"},
        {"Qatar", "QA"},
        {"Republic of the Congo", "CG"},
        {"Réunion", "RE"},
        {"Romania", "RO"},
        {"Russia", "RU"},
        {"Rwanda", "RW"},
        {"Saint Barthélemy", "BL"},
        {"Saint Helena", "SH"},
        {"Saint Kitts & Nevis", "KN"},
        {"Saint Lucia", "LC"},
        {"Saint Martin", "MF"},
        {"Saint Pierre & Miquelon", "PM"},
        {"Saint Vincent & the Grenadines", "VC"},
        {"Samoa", "WS"},
        {"San Marino", "SM"},
        {"São Tomé & Príncipe", "ST"},
        {"Saudi Arabia", "SA"},
        {"Senegal", "SN"},
        {"Serbia", "RS"},
        {"Seychelles", "SC"},
        {"Sierra Leone", "SL"},
        {"Singapore", "SG"},
        {"Sint Maarten", "SX"},
        {"Slovakia", "SK"},
        {"Slovenia", "SI"},
        {"Solomon Islands", "SB"},
        {"Somalia", "SO"},
        {"South Africa", "ZA"},
        {"South Georgia & the South Sandwich Islands", "GS"},
        {"South Korea", "KR"},
        {"Spain", "ES"},
        {"Sudan", "SD"},
        {"Suriname", "SR"},
        {"Svalbard & Jan Mayen", "SJ"},
        {"Sweden", "SE"},
        {"Switzerland", "CH"},
        {"Syrian Arab Republic", "SY"},
        {"Taiwan", "TW"},
        {"Tajikistan", "TJ"},
        {"Tanzania", "TZ"},
        {"Thailand", "TH"},
        {"Timor-Leste", "TL"},
        {"Togo", "TG"},
        {"Tokelau", "TK"},
        {"Tonga", "TO"},
        {"Trial", "Trial"},
        {"Trinidad & Tobago", "TT"},
        {"Tunisia", "TN"},
        {"Turkey", "TR"},
        {"Turkmenistan", "TM"},
        {"Turks & Caicos Islands", "TC"},
        {"Tuvalu", "TV"},
        {"Uganda", "UG"},
        {"Ukraine", "UA"},
        {"United Arab Emirates", "AE"},
        {"United Kingdom", "GB"},
        {"United States", "US"},
        {"Uruguay", "UY"},
        {"Uzbekistan", "UZ"},
        {"Vanuatu", "VU"},
        {"Venezuela", "VE"},
        {"Vietnam", "VN"},
        {"Virgin Islands (British)", "VG"},
        {"Virgin Islands (U.S.)", "VI"},
        {"Wallis & Futuna", "WF"},
        {"Western Sahara", "EH"},
        {"Yemen", "YE"},
        {"Zambia", "ZM"},
        {"Zimbabwe", "ZW"}}

    Public ReadOnly RegionDictionary As New Dictionary(Of WorldRegion, String) From {
        {WorldRegion.UnitedStates, "United States"},
        {WorldRegion.Trial, "Trial"},
        {WorldRegion.Africa, "Africa"},
        {WorldRegion.Asia, "Asia"},
        {WorldRegion.Europe, "Europe"},
        {WorldRegion.NorthAmerica, "North America"},
        {WorldRegion.Oceania, "Oceania"},
        {WorldRegion.SouthAmerica, "South America"}}

    Public ReadOnly RegionToServerMapping As New Dictionary(Of WorldRegion, String) From {
        {WorldRegion.UnitedStates, WorldRegion.UnitedStates.ToString},
        {WorldRegion.Trial, WorldRegion.Trial.ToString},
        {WorldRegion.Africa, WorldRegion.Europe.ToString},
        {WorldRegion.Asia, WorldRegion.Europe.ToString},
        {WorldRegion.Europe, WorldRegion.Europe.ToString},
        {WorldRegion.NorthAmerica, WorldRegion.Europe.ToString},
        {WorldRegion.Oceania, WorldRegion.Europe.ToString},
        {WorldRegion.SouthAmerica, WorldRegion.Europe.ToString}}

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

        Dim filename As String = Path.GetFileNameWithoutExtension(ReportFileNameWithPath)
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
            prompt = $"Filename '{filename}' malformed,{vbCrLf}it must contain at least one ')'."
            MsgBox(
                heading:="Invalid Filename",
                prompt,
                buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                title:="Malformed Error Report Filename")
            Return Nothing
        End If

        If Not filename.StartsWith(value:=FixedPart) Then
            prompt = $"Filename '{filename}' malformed,{vbCrLf}it must start with '{FixedPart}'."
            MsgBox(
                heading:="Invalid Filename",
                prompt,
                buttonStyle:=MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation,
                title:="Malformed Error Report Filename")
            Return Nothing
        End If

        Dim indexOfOpenParenthesis As Integer = filename.IndexOf(value:="("c)
        prompt = $"Filename '{filename}' malformed,{vbCrLf}it must contain '(' after '{FixedPart}'."
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
            prompt = $"Filename '{filename}' malformed,{vbCrLf}it must contain '(' immediately after '{FixedPart}'."
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

        Dim predicate As Func(Of CultureInfo, Boolean) = Function(c As CultureInfo) As Boolean
                                                             Return c.Name = cultureName
                                                         End Function
        Dim fileNameInvalid As Boolean = Not CultureInfoList.Any(predicate)

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
        If s_countryCodeToCountry.Count = 0 Then
            ' Create the reverse lookup Dictionary only once
            For Each kvp As KeyValuePair(Of String, String) In CountryToCodeList
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
    '''  The <see cref="WorldRegion"/> if found;
    '''  otherwise, defaults to <see cref="WorldRegion.UnitedStates"/> (for "US").
    ''' </returns>
    <Extension>
    Public Function GetRegionFromCode(countryCode As String) As WorldRegion
        If IsNullOrWhiteSpace(value:=countryCode) Then
            countryCode = "US"
        End If
        Return CountryNameToRegionList(key:=GetCountryFromCode(countryCode))
    End Function

End Module
