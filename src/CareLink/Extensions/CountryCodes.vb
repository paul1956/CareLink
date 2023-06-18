' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports System.IO
Imports System.Runtime.CompilerServices

Public Module RegionCountryLists

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
                    {"Puerto Rico", "US"},
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
                    {"Vietnam", "VN"}
                }

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
                    {"Puerto Rico", "North America"},
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
                    {"Vietnam", "Asia"}
                }

    Public ReadOnly s_regionList As New Dictionary(Of String, String) From {
                    {"North America", "North America"},
                    {"Africa", "Africa"},
                    {"Asia", "Asia"},
                    {"Europe", "Europe"},
                    {"Oceania", "Oceania"},
                    {"South America", "South America"}
                }

    <Extension>
    Public Function ExtractCultureFromFileName(ReportFileNameWithPath As String, FixedPart As String, Optional fuzzy As Boolean = False) As CultureInfo
        Dim filenameWithoutExtension As String = Path.GetFileNameWithoutExtension(ReportFileNameWithPath)

        If filenameWithoutExtension.Count("("c) <> 1 Then
            MsgBox($"Error Report Filename '{filenameWithoutExtension}' malformed,{vbCrLf}it must contain exactly one '('.", MsgBoxStyle.OkOnly, "Malformed Error Report Filename")
            Return Nothing
        End If

        If filenameWithoutExtension.Count(")"c) <> 1 Then
            MsgBox($"Error Report Filename '{filenameWithoutExtension}' malformed,{vbCrLf}it must contain exactly one ')'.", MsgBoxStyle.OkOnly, "Malformed Error Report Filename")
            Return Nothing
        End If

        If Not filenameWithoutExtension.StartsWith(FixedPart) Then
            MsgBox($"Error Report Filename '{filenameWithoutExtension}' malformed,{vbCrLf}it must start with '{FixedPart}'.", MsgBoxStyle.OkOnly, "Malformed Error Report Filename")
            Return Nothing
        End If

        Dim indexOfOpenParenthesis As Integer = filenameWithoutExtension.IndexOf("("c)
        If fuzzy Then
            If indexOfOpenParenthesis < FixedPart.Length Then
                MsgBox($"Error Report Filename '{filenameWithoutExtension}' malformed,{vbCrLf}it must contain '(' after '{FixedPart}.", MsgBoxStyle.OkOnly, "Malformed Error Report Filename")
                Return Nothing
            End If
        Else
            If indexOfOpenParenthesis <> FixedPart.Length Then
                MsgBox($"Error Report Filename '{filenameWithoutExtension}' malformed,{vbCrLf}it must contain '(' immediately after '{FixedPart}.", MsgBoxStyle.OkOnly, "Malformed Error Report Filename")
                Return Nothing
            End If
        End If

        Dim indexOfClosedParenthesis As Integer = filenameWithoutExtension.IndexOf(")"c)
        If indexOfClosedParenthesis < 0 Then
            MsgBox($"Error Report Filename '{filenameWithoutExtension}' malformed,{vbCrLf}it must contain ')'.", MsgBoxStyle.OkOnly, "Malformed Error Report Filename")
            Return Nothing
        End If

        Dim cultureName As String = filenameWithoutExtension.Substring(indexOfOpenParenthesis + 1, indexOfClosedParenthesis - indexOfOpenParenthesis - 1)
        If Not CultureInfoList.Where(Function(c As CultureInfo) c.Name = cultureName).Any Then
            MsgBox($"Culture name '{cultureName}' is not a valid culture name.", MsgBoxStyle.OkOnly, "Invalid Culture Name")
            Return CultureInfo.CurrentCulture
        End If
        Return CultureInfo.GetCultureInfo(cultureName)
    End Function

    <Extension>
    Public Function GetCountryFromCode(countryCode As String) As String
        Debug.Assert(countryCode.Length = 2)
        Return s_countryCodeList.Where(Function(kvp As KeyValuePair(Of String, String)) kvp.Value.Equals(countryCode)).Select(Function(kvp As KeyValuePair(Of String, String)) kvp.Key).FirstOrDefault
    End Function

    <Extension>
    Public Function GetRegionFromCode(countryCode As String) As String
        If String.IsNullOrWhiteSpace(countryCode) Then
            countryCode = "US"
        End If
        Return s_regionCountryList(GetCountryFromCode(countryCode))
    End Function

End Module
