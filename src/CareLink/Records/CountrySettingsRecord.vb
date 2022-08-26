' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Reflection

Public Class CountrySettingsRecord

#Region "Single Items"

    Public Property name As String                       ' "United States"
    Public Property defaultLanguage As String            ' "EN"
    Public Property defaultCountryName As String         ' "United States"
    Public Property defaultDevice As String              ' "GM"
    Public Property dialCode As String                   ' "+1"
    Public Property cpMobileAppAvailable As String       ' "False"
    Public Property uploaderAllowed As String            ' "True"
    Public Property techSupport As String                ' "1-800-646-4633"
    Public Property techDays As String                   ' "Mo-Fr"
    Public Property firstDayOfWeek As String             ' "Monday"
    Public Property techHours As String                  ' "7:00 am - 7:00 pm CST"
    Public Property legalAge As String                   ' "18"
    Public Property shortDateFormat As String            ' "MM-DD-YYYY"
    Public Property shortTimeFormat As String            ' "h:mm A"
    Public Property mediaHost As String                  ' "https://carelink.minimed.com"
    Public Property blePereodicDataEndpoint As String    ' "https://clcloud.minimed.com/connect/v2/display/message"
    Public Property region As String                     ' "US"
    Public Property carbDefaultUnit As String            ' "GRAMS"
    Public Property bgUnits As String                    ' "MG_DL"
    Public Property timeFormat As String                 ' "HR_12"
    Public Property timeUnitsDefault As String           ' "12h"
    Public Property recordSeparator As String            ' ","
    Public Property glucoseUnitsDefault As String        ' "MG_DL"
    Public Property carbohydrateUnitsDefault As String   ' "GRAMS"
    Public Property carbExchangeRatioDefault As String   ' "15.0"
    Public Property mfa As mfaRecord                        ' "{""status"":""OPTIONAL"",""fromDate"":""05/10/2019"",""gracePeriod"":5,""codeValidityDuration"":15,""maxAttempts"":3,""rememberPeriod"":3}"
    Public Property smsSendingAllowed As String          ' "True"

#End Region 'Single Items

#Region "Flag"

    Public DataValid As Boolean

#End Region ' Flag

#Region "Flat Records"

    Public pathDocs As pathDocsRecord                   ' "{""ddms.termsOfUse"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/terms_of_use.html"",""ddms.privacyStatementPdf"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/privacy_policy.pdf"",""ddms.termsOfUsePdf"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/terms_of_use.pdf"",""ddms.privacyStatement"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/privacy_policy.pdf"",""ddms.faqPdf"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/faq.pdf"",""ddms.privacyPractices"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/notices.pdf""}"
    Public reportDateFormat As reportDateFormatRecord   ' "{""longTimePattern12"":""hh:mm:ss tt"",""longTimePattern24"":""HH:mm:ss"",""shortTimePattern12"":""h:mm tt"",""shortTimePattern24"":""HH:mm"",""shortDatePattern"":""MM-dd-yyyy"",""dateSeparator"":""-"",""timeSeparator"":"":""}"
    Public postal As postalRecord                       ' "{""postalFormat"":[""99999-9999"",""99999""],""regExpStr"":""^(\\d{5}-\\d{4})$|^(\\d{5})$""}"
    Public numberFormat As numberFormatRecord           ' "{""decimalSeparator"":""."",""groupsSeparator"":"",""}"

#End Region

#Region "Lists"

    Public languages As New List(Of LanguageRecord)                 ' "[{""name"":""English"",""code"":""EN""}]"
    Public supportedReports As New List(Of supportedReportRecord)   ' "[{""report"":""ADHERENCE"",""onlyFor"":[],""notFor"":[]},{""report"":""ASSESSMENT_AND_PROGRESS"",""onlyFor"":[],""notFor"":[]},{""report"":""BOLUS_WIZARD_FOOD_BOLUS"",""onlyFor"":[],""notFor"":[]},{""report"":""DAILY_DETAILS"",""onlyFor"":[],""notFor"":[]},{""report"":""DASHBOARD"",""onlyFor"":[],""notFor"":[]},{""report"":""DEVICE_SETTINGS"",""onlyFor"":[],""notFor"":[]},{""report"":""EPISODE_SUMMARY"",""onlyFor"":[],""notFor"":[]},{""report"":""LOGBOOK"",""onlyFor"":[],""notFor"":[]},{""report"":""OVERVIEW"",""onlyFor"":[],""notFor"":[]},{""report"":""WEEKLY_REVIEW"",""onlyFor"":[],""notFor"":[]}]"

#End Region

    Public Sub New()
        DataValid = False
    End Sub

    Public Sub New(sessionCountrySettings As Dictionary(Of String, String))
        If sessionCountrySettings Is Nothing Then
            DataValid = False
            Exit Sub
        End If

        Dim dataGridViewSingleCountryItems As DataGridView = My.Forms.Form1.DataGridViewSingleCountryItems
        dataGridViewSingleCountryItems.Rows.Clear()
        Dim dataGridViewLanguages As DataGridView = My.Forms.Form1.DataGridViewLanguages
        dataGridViewLanguages.Rows.Clear()
        Dim dataGridViewSupportedReports As DataGridView = My.Forms.Form1.DataGridViewSuportedReports
        For Each row As IndexClass(Of KeyValuePair(Of String, String)) In sessionCountrySettings.WithIndex
            Dim rowValue As KeyValuePair(Of String, String) = row.Value

            Select Case rowValue.Key
                Case NameOf(name)
                    Me.name = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(languages)
                    languages.Clear()
                    For Each lang As IndexClass(Of Dictionary(Of String, String)) In LoadList(rowValue.Value).WithIndex
                        languages.Add(New LanguageRecord(lang.Value))
                        dataGridViewLanguages.Rows.Add((New String() _
                            {$"{row.Index + 1}", languages.Last.name, languages.Last.code}))
                    Next

                Case NameOf(defaultLanguage)
                    Me.defaultLanguage = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(defaultCountryName)
                    Me.defaultCountryName = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(defaultDevice)
                    Me.defaultDevice = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(dialCode)
                    Me.dialCode = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(cpMobileAppAvailable)
                    Me.cpMobileAppAvailable = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(uploaderAllowed)
                    Me.uploaderAllowed = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(techSupport)
                    Me.techSupport = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(techDays)
                    Me.techDays = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(firstDayOfWeek)
                    Me.firstDayOfWeek = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(techHours)
                    Me.techHours = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(legalAge)
                    Me.legalAge = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(shortDateFormat)
                    Me.shortDateFormat = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(shortTimeFormat)
                    Me.shortTimeFormat = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(mediaHost)
                    Me.mediaHost = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(blePereodicDataEndpoint)
                    Me.blePereodicDataEndpoint = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(region)
                    Me.region = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(pathDocs)
                    pathDocs = New pathDocsRecord(rowValue.Value)
                    For Each member As IndexClass(Of KeyValuePair(Of String, String)) In pathDocs.pathDoc.WithIndex
                        dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {$"{row.Index + 1},{member.Index}", "pathDocsRecord", member.Value.Key, member.Value.Value}))
                    Next
                Case NameOf(carbDefaultUnit)
                    Me.carbDefaultUnit = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(bgUnits)
                    Me.bgUnits = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(timeFormat)
                    Me.timeFormat = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(timeUnitsDefault)
                    Me.timeUnitsDefault = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(recordSeparator)
                    Me.recordSeparator = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(glucoseUnitsDefault)
                    Me.glucoseUnitsDefault = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(carbohydrateUnitsDefault)
                    Me.carbohydrateUnitsDefault = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(carbExchangeRatioDefault)
                    Me.carbExchangeRatioDefault = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(reportDateFormat)
                    reportDateFormat = New reportDateFormatRecord(rowValue.Value)
                    For Each member As IndexClass(Of KeyValuePair(Of String, String)) In reportDateFormat.ToList.WithIndex
                        dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {$"{row.Index + 1},{member.Index}", "reportDateFormat", member.Value.Key, member.Value.Value}))
                    Next

                Case NameOf(mfa)
                    Me.mfa = New mfaRecord(rowValue.Value)
                    For Each member As IndexClass(Of KeyValuePair(Of String, String)) In Me.mfa.ToList.WithIndex
                        dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {$"{row.Index + 1},{member.Index}", "mfa", member.Value.Key, member.Value.Value}))
                    Next

                Case NameOf(supportedReports)
                    supportedReports.Clear()
                    For Each dic As IndexClass(Of Dictionary(Of String, String)) In LoadList(rowValue.Value).WithIndex
                        supportedReports.Add(New supportedReportRecord(dic.Value, dic.Index + 1))
                    Next
                    dataGridViewSupportedReports.DataSource = supportedReports
                Case NameOf(smsSendingAllowed)
                    Me.smsSendingAllowed = rowValue.Value
                    dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {(row.Index + 1).ToString, "", rowValue.Key, rowValue.Value}))
                Case NameOf(postal)
                    postal = New postalRecord(rowValue.Value)
                    For Each member As IndexClass(Of KeyValuePair(Of String, String)) In postal.ToList.WithIndex
                        dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {$"{row.Index + 1},{member.Index}", "postal", member.Value.Key, member.Value.Value}))
                    Next

                Case NameOf(numberFormat)
                    numberFormat = New numberFormatRecord(rowValue.Value)
                    For Each member As IndexClass(Of KeyValuePair(Of String, String)) In numberFormat.ToList.WithIndex
                        dataGridViewSingleCountryItems.Rows.Add((New String() _
                        {$"{row.Index + 1},{member.Index}", "postal", member.Value.Key, member.Value.Value}))
                    Next
                Case Else
                    Stop
            End Select
        Next
        DataValid = True
    End Sub

End Class
