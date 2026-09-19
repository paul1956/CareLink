' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices

Public Module RegionCountryLists

#If True Then ' Keep on top

    Private ReadOnly Property Comparer As StringComparer =
            StringComparer.OrdinalIgnoreCase

#End If

    Private ReadOnly s_countryCodeToCountry As New Dictionary(Of String, String)(Comparer)

    ''' <summary>
    '''  A dictionary mapping country names to their corresponding regions.
    ''' </summary>
    ''' <remarks>
    '''  The regions are defined as per the ISO 3166-1 standard,
    '''  grouping countries into continents or major geographic areas.
    ''' </remarks>
    Public ReadOnly s_countryNameToRegionList As New Dictionary(Of String, String) From {
        {"United States", "United States"},
        {"Albania", "Europe"},
        {"Algeria", "Africa"},
        {"Andorra", "Europe"},
        {"Argentina", "South America"},
        {"Armenia", "Transcontinental"},
        {"Aruba", "North America"},
        {"Australia", "Oceania"},
        {"Austria", "Europe"},
        {"Azerbaijan", "Transcontinental"},
        {"Bahamas", "North America"},
        {"Bahrain", "Asia"},
        {"Bangladesh", "Asia"},
        {"Barbados", "North America"},
        {"Belarus", "Europe"},
        {"Belgium", "Europe"},
        {"Belize", "North America"},
        {"Benin", "Africa"},
        {"Bermuda", "North America"},
        {"Bhutan", "Asia"},
        {"Bolivia", "South America"},
        {"Bonaire, Sint Eustatius & Saba", "North America"},
        {"Bosnia and Herzegovina", "Europe"},
        {"Botswana", "Africa"},
        {"Brazil", "South America"},
        {"British Virgin Islands", "North America"},
        {"Brunei Darussalam", "Asia"},
        {"Bulgaria", "Europe"},
        {"Burkina Faso", "Africa"},
        {"Burundi", "Africa"},
        {"Cabo Verde", "Africa"},
        {"Cambodia", "Asia"},
        {"Cameroon", "Africa"},
        {"Canada", "North America"},
        {"Cayman Islands", "North America"},
        {"Central African Republic", "Africa"},
        {"Chad", "Africa"},
        {"Chile", "South America"},
        {"China", "Asia"},
        {"Christmas Island", "Oceania"},
        {"Clinical", "Clinical"},
        {"Colombia", "South America"},
        {"Comoros (the)", "Africa"},
        {"Cook Islands (the)", "Oceania"},
        {"Costa Rica", "North America"},
        {"Côte d'Ivoire", "Africa"},
        {"Croatia", "Europe"},
        {"Cuba", "North America"},
        {"Curaçao", "North America"},
        {"Cyprus", "Transcontinental"},
        {"Czechia", "Europe"},
        {"Denmark", "Europe"},
        {"Djibouti", "Africa"},
        {"Dominica", "North America"},
        {"Dominican Republic", "North America"},
        {"Ecuador", "South America"},
        {"Egypt", "Africa"},
        {"El Salvador", "North America"},
        {"Equatorial Guinea", "Africa"},
        {"Eritrea", "Africa"},
        {"Estonia", "Europe"},
        {"Eswatini", "Africa"},
        {"Ethiopia", "Africa"},
        {"Falkland Islands (the) [Malvinas]", "South America"},
        {"Faroe Islands (the)", "Europe"},
        {"Fiji", "Oceania"},
        {"Finland", "Europe"},
        {"France", "Europe"},
        {"French Guiana", "South America"},
        {"French Polynesia", "Oceania"},
        {"French Southern Territories (the)", "Oceania"},
        {"Gabon", "Africa"},
        {"Gambia (the)", "Africa"},
        {"Georgia", "Transcontinental"},
        {"Germany", "Europe"},
        {"Ghana", "Africa"},
        {"Gibraltar", "Europe"},
        {"Greece", "Europe"},
        {"Greenland", "North America"},
        {"Grenada", "North America"},
        {"Guadeloupe", "North America"},
        {"Guam", "Oceania"},
        {"Guatemala", "North America"},
        {"Guernsey", "Europe"},
        {"Guinea (the)", "Africa"},
        {"Guinea-Bissau", "Africa"},
        {"Guyana", "South America"},
        {"Haiti", "North America"},
        {"Heard Island & McDonald Islands", "Oceania"},
        {"Honduras", "North America"},
        {"Hong Kong (SAR China)", "Asia"},
        {"Hungary", "Europe"},
        {"Iceland", "Europe"},
        {"India", "Asia"},
        {"Indonesia", "Asia"},
        {"Iraq", "Asia"},
        {"Ireland", "Europe"},
        {"Isle of Man", "Europe"},
        {"Israel", "Asia"},
        {"Italy", "Europe"},
        {"Jamaica", "North America"},
        {"Japan", "Asia"},
        {"Jersey", "Europe"},
        {"Jordan", "Asia"},
        {"Kazakhstan", "Asia"},
        {"Kenya", "Africa"},
        {"Kiribati", "Oceania"},
        {"Kosovo", "Europe"},
        {"Kuwait", "Asia"},
        {"Kyrgyzstan", "Asia"},
        {"Laos", "Asia"},
        {"Latvia", "Europe"},
        {"Lebanon", "Asia"},
        {"Lesotho", "Africa"},
        {"Liberia", "Africa"},
        {"Libya", "Africa"},
        {"Liechtenstein", "Europe"},
        {"Lithuania", "Europe"},
        {"Luxembourg", "Europe"},
        {"Macao", "Asia"},
        {"Madagascar", "Africa"},
        {"Malawi", "Africa"},
        {"Malaysia", "Asia"},
        {"Maldives", "Asia"},
        {"Malta", "Europe"},
        {"Marshall Islands", "Oceania"},
        {"Martinique", "North America"},
        {"Mauritania", "Africa"},
        {"Mauritius", "Africa"},
        {"Mayotte", "Africa"},
        {"Mexico", "North America"},
        {"Micronesia", "Oceania"},
        {"Moldova", "Europe"},
        {"Monaco", "Europe"},
        {"Mongolia", "Asia"},
        {"Montenegro", "Europe"},
        {"Montserrat", "Europe"},
        {"Morocco", "Africa"},
        {"Mozambique", "Africa"},
        {"Myanmar", "Asia"},
        {"Namibia", "Africa"},
        {"Nauru", "Oceania"},
        {"Nepal", "Asia"},
        {"Netherlands", "Europe"},
        {"New Caledonia", "Oceania"},
        {"New Zealand", "Oceania"},
        {"Nicaragua", "North America"},
        {"Niger", "Africa"},
        {"Nigeria", "Africa"},
        {"Niue", "Oceania"},
        {"Norfolk Island", "Oceania"},
        {"North Macedonia", "Europe"},
        {"Northern Mariana Islands", "Oceania"},
        {"Norway", "Europe"},
        {"Oman", "Asia"},
        {"Pakistan", "Asia"},
        {"Palau", "Oceania"},
        {"Panama", "North America"},
        {"Papua New Guinea", "Oceania"},
        {"Paraguay", "South America"},
        {"Peru", "South America"},
        {"Philippines", "Asia"},
        {"Pitcairn", "Oceania"},
        {"Poland", "Europe"},
        {"Portugal", "Europe"},
        {"Puerto Rico", "United States"},
        {"Qatar", "Asia"},
        {"Republic of the Congo", "Africa"},
        {"Réunion", "Africa"},
        {"Romania", "Europe"},
        {"Russia", "Transcontinental"},
        {"Rwanda", "Africa"},
        {"Saint Barthélemy", "North America"},
        {"Saint Helena", "Africa"},
        {"Saint Kitts & Nevis", "North America"},
        {"Saint Lucia", "North America"},
        {"Saint Martin", "North America"},
        {"Saint Pierre & Miquelon", "North America"},
        {"Saint Vincent & the Grenadines", "North America"},
        {"Samoa", "Oceania"},
        {"San Marino", "Europe"},
        {"São Tomé & Príncipe", "Africa"},
        {"Saudi Arabia", "Asia"},
        {"Senegal", "Africa"},
        {"Serbia", "Europe"},
        {"Seychelles", "Africa"},
        {"Sierra Leone", "Africa"},
        {"Singapore", "Asia"},
        {"Sint Maarten", "North America"},
        {"Slovakia", "Europe"},
        {"Slovenia", "Europe"},
        {"Solomon Islands", "Oceania"},
        {"Somalia", "Africa"},
        {"South Africa", "Africa"},
        {"South Georgia & the South Sandwich Islands", "Oceania"},
        {"South Korea", "Asia"},
        {"Spain", "Europe"},
        {"Sudan", "Africa"},
        {"Suriname", "South America"},
        {"Svalbard & Jan Mayen", "Europe"},
        {"Sweden", "Europe"},
        {"Switzerland", "Europe"},
        {"Syrian Arab Republic", "Asia"},
        {"Taiwan", "Asia"},
        {"Tajikistan", "Asia"},
        {"Tanzania", "Africa"},
        {"Thailand", "Asia"},
        {"Timor-Leste", "Asia"},
        {"Togo", "Africa"},
        {"Tokelau", "Oceania"},
        {"Tonga", "Oceania"},
        {"Trinidad & Tobago", "North America"},
        {"Tunisia", "Africa"},
        {"Turkey", "Transcontinental"},
        {"Turkmenistan", "Asia"},
        {"Turks & Caicos Islands", "North America"},
        {"Tuvalu", "Oceania"},
        {"Uganda", "Africa"},
        {"Ukraine", "Europe"},
        {"United Arab Emirates", "Asia"},
        {"United Kingdom", "Europe"},
        {"Uruguay", "South America"},
        {"Uzbekistan", "Asia"},
        {"Vanuatu", "Oceania"},
        {"Venezuela", "South America"},
        {"Vietnam", "Asia"},
        {"Virgin Islands (British)", "North America"},
        {"Virgin Islands (U.S.)", "North America"},
        {"Wallis & Futuna", "Oceania"},
        {"Western Sahara", "Africa"},
        {"Yemen", "Asia"},
        {"Zambia", "Africa"},
        {"Zimbabwe", "Africa"}}

    ''' <summary>
    '''  A dictionary mapping country names to their ISO 2-letter country codes.
    ''' </summary>
    Public ReadOnly s_countryToCodeList As New Dictionary(Of String, String) From {
        {"Albania", "AL"},
        {"Algeria", "DZ"},
        {"Andorra", "AD"},
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
        {"Bosnia and Herzegovina", "BA"},
        {"Brazil", "BR"},
        {"Bulgaria", "BG"},
        {"Canada", "CA"},
        {"Chile", "CL"},
        {"Clinical", "CLINICAL"},
        {"Colombia", "CO"},
        {"Costa Rica", "CR"},
        {"Croatia", "HR"},
        {"Curaçao", "CW"},
        {"Cyprus", "CY"},
        {"Czechia", "CZ"},
        {"Denmark", "DK"},
        {"Dominican Republic", "DO"},
        {"Ecuador", "EC"},
        {"Egypt", "EG"},
        {"El Salvador", "SV"},
        {"Estonia", "EE"},
        {"Finland", "FI"},
        {"France", "FR"},
        {"Georgia", "GE"},
        {"Germany", "DE"},
        {"Greece", "GR"},
        {"Guatemala", "GT"},
        {"Honduras", "HN"},
        {"Hong Kong (SAR China)", "HK"},
        {"Hungary", "HU"},
        {"Iceland", "IS"},
        {"India", "IN"},
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
        {"Malaysia", "MY"},
        {"Malta", "MT"},
        {"Mauritius", "MU"},
        {"Mayotte", "YT"},
        {"Mexico", "MX"},
        {"Moldova", "MD"},
        {"Montenegro", "ME"},
        {"Morocco", "MA"},
        {"Netherlands", "NL"},
        {"New Zealand", "NZ"},
        {"Nicaragua", "NI"},
        {"North Macedonia", "MK"},
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
        {"United States", "US"},
        {"Uruguay", "UY"},
        {"Uzbekistan", "UZ"},
        {"Vietnam", "VN"}}

    ' Ensure s_countryNameToRegionList contains only entries that are present in s_countryToCodeList
    ' This removes any display-name entries that the application does not have an ISO code for.
    Private ReadOnly s_countryNameToRegionListInitializer As Boolean =
        InitializeCountryNameToRegionList()

    Private Function InitializeCountryNameToRegionList() As Boolean
        Dim toRemove As New List(Of String)()
        For Each kvp As KeyValuePair(Of String, String) In s_countryNameToRegionList
            ' Never remove the special "Clinical" entry — it is handled specially by the app
            If EqualsNoCase(a:=kvp.Key, b:="Clinical") Then
                Continue For
            End If

            If Not s_countryToCodeList.ContainsKey(kvp.Key) Then
                toRemove.Add(item:=kvp.Key)
            End If
        Next
        For Each k As String In toRemove
            s_countryNameToRegionList.Remove(key:=k)
        Next
        Return True
    End Function

    Public ReadOnly s_regionList As New List(Of String) From {
        {"United States"},
        {"Clinical"},
        {"Africa"},
        {"Asia"},
        {"Europe"},
        {"North America"},
        {"Oceania"},
        {"South America"},
        {"Transcontinental"}}

    Public ReadOnly s_regionToServerMapping As New Dictionary(Of String, String) From {
        {"United States", "US"},
        {"Clinical", "CLINICAL"},
        {"Antarctica", "EU"},
        {"Africa", "EU"},
        {"Asia", "EU"},
        {"Transcontinental", "EU"},
        {"Europe", "EU"},
        {"North America", "EU"},
        {"Oceania", "EU"},
        {"South America", "EU"}}

    ''' <summary>
    ''' Get server mapping code for a region name (display string).
    ''' </summary>
    Public Function GetServerMapping(regionName As String) As String
        Dim value As String = Nothing
        If s_regionToServerMapping.TryGetValue(key:=regionName, value) Then
            Return value
        End If
        ' default to US if unknown
        Return "US"
    End Function

    ' Legacy overload removed: WorldRegion enum no longer used.
    ' Use GetServerMapping(regionName As String) which accepts the region display name.

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
    Public Function ExtractCulture(ReportFileNameWithPath As String,
                                   FixedPart As String,
                                   Optional fuzzy As Boolean = False) As CultureInfo

        Dim filename As String =
            Path.GetFileNameWithoutExtension(path:=ReportFileNameWithPath)
        Dim prompt As String
        Const buttonStyle As MsgBoxStyle =
            MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation

        If filename.Count(c:="("c) = 0 Then
            prompt = $"'{filename}' malformed,{vbCrLf}it must contain at least one '('."
            MsgBox(heading:="Invalid Filename",
                   prompt,
                   buttonStyle,
                   title:="Malformed Error Report Filename")
            Return Nothing
        End If

        If filename.Count(c:=")"c) = 0 Then
            prompt = $"Filename '{filename}' malformed,{vbCrLf}it must contain at least one ')'."
            MsgBox(
                heading:="Invalid Filename",
                prompt,
                buttonStyle,
                title:="Malformed Error Report Filename")
            Return Nothing
        End If

        If Not filename.StartsWith(value:=FixedPart) Then
            prompt = $"Filename '{filename}' malformed,{vbCrLf}it must start with '{FixedPart}'."
            MsgBox(
                heading:="Invalid Filename",
                prompt,
                buttonStyle,
                title:="Malformed Error Report Filename")
            Return Nothing
        End If

        Dim indexOfOpenParenthesis As Integer = filename.IndexOf(value:="("c)
        prompt = $"Filename '{filename}' malformed,{vbCrLf}it must contain '(' after '{FixedPart}'."
        If fuzzy Then
            If indexOfOpenParenthesis < FixedPart.Length Then
                MsgBox(heading:="Invalid Filename",
                       prompt,
                       buttonStyle,
                       title:="Malformed Error Report Filename")
                Return Nothing
            End If
        Else
            prompt = $"Filename '{filename}' malformed,{vbCrLf}it must contain '(' immediately after '{FixedPart}'."
            If indexOfOpenParenthesis <> FixedPart.Length Then
                MsgBox(heading:="Invalid Filename",
                       prompt,
                       buttonStyle,
                       title:="Malformed Error Report Filename")
                Return Nothing
            End If
        End If

        Dim indexOfClosedParenthesis As Integer = filename.IndexOf(")"c)
        If indexOfClosedParenthesis < 0 Then
            MsgBox(heading:="Invalid Filename",
                   prompt:=$"Filename '{filename}' malformed,{vbCrLf}it must contain ')'.",
                   buttonStyle,
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
                buttonStyle,
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
            For Each kvp As KeyValuePair(Of String, String) In s_countryToCodeList
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
    '''  The region name if found;
    '''  otherwise, defaults to "United States" (for "US").
    ''' </returns>
    <Extension>
    Public Function GetRegionFromCode(countryCode As String) As String
        If IsNullOrWhiteSpace(value:=countryCode) Then
            countryCode = "US"
        End If
        Return s_countryNameToRegionList(key:=GetCountryFromCode(countryCode))
    End Function

End Module
