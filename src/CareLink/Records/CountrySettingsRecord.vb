' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class CountrySettingsRecord
    Public name As String                       ' "United States"
    Public languages As languagesRecord         ' "[{""name"":""English"",""code"":""EN""}]"
    Public defaultLanguage As String            ' "EN"
    Public defaultCountryName As String         ' "United States"
    Public defaultDevice As String              ' "GM"
    Public dialCode As String                   ' "+1"
    Public cpMobileAppAvailable As String       ' "False"
    Public uploaderAllowed As String            ' "True"
    Public techSupport As String                ' "1-800-646-4633"
    Public techDays As String                   ' "Mo-Fr"
    Public firstDayOfWeek As String             ' "Monday"
    Public techHours As String                  ' "7:00 am - 7:00 pm CST"
    Public legalAge As String                   ' "18"
    Public shortDateFormat As String            ' "MM-DD-YYYY"
    Public shortTimeFormat As String            ' "h:mm A"
    Public mediaHost As String                  ' "https://carelink.minimed.com"
    Public blePereodicDataEndpoint As String    ' "https://clcloud.minimed.com/connect/v2/display/message"
    Public region As String                     ' "US"
    Public pathDocs As pathDocsRecord           ' "{""ddms.termsOfUse"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/terms_of_use.html"",""ddms.privacyStatementPdf"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/privacy_policy.pdf"",""ddms.termsOfUsePdf"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/terms_of_use.pdf"",""ddms.privacyStatement"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/privacy_policy.pdf"",""ddms.faqPdf"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/faq.pdf"",""ddms.privacyPractices"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/notices.pdf""}"
    Public carbDefaultUnit As String            ' "GRAMS"
    Public bgUnits As String                    ' "MG_DL"
    Public timeFormat As String                 ' "HR_12"
    Public timeUnitsDefault As String           ' "12h"
    Public recordSeparator As String            ' ","
    Public glucoseUnitsDefault As String        ' "MG_DL"
    Public carbohydrateUnitsDefault As String   ' "GRAMS"
    Public carbExchangeRatioDefault As String   ' "15.0"
    Public reportDateFormat As reportDateFormatRecord   ' "{""longTimePattern12"":""hh:mm:ss tt"",""longTimePattern24"":""HH:mm:ss"",""shortTimePattern12"":""h:mm tt"",""shortTimePattern24"":""HH:mm"",""shortDatePattern"":""MM-dd-yyyy"",""dateSeparator"":""-"",""timeSeparator"":"":""}"
    Public mfa As String                        ' "{""status"":""OPTIONAL"",""fromDate"":""05/10/2019"",""gracePeriod"":5,""codeValidityDuration"":15,""maxAttempts"":3,""rememberPeriod"":3}"
    Public supportedReports As supportedReportsRecord           ' "[{""report"":""ADHERENCE"",""onlyFor"":[],""notFor"":[]},{""report"":""ASSESSMENT_AND_PROGRESS"",""onlyFor"":[],""notFor"":[]},{""report"":""BOLUS_WIZARD_FOOD_BOLUS"",""onlyFor"":[],""notFor"":[]},{""report"":""DAILY_DETAILS"",""onlyFor"":[],""notFor"":[]},{""report"":""DASHBOARD"",""onlyFor"":[],""notFor"":[]},{""report"":""DEVICE_SETTINGS"",""onlyFor"":[],""notFor"":[]},{""report"":""EPISODE_SUMMARY"",""onlyFor"":[],""notFor"":[]},{""report"":""LOGBOOK"",""onlyFor"":[],""notFor"":[]},{""report"":""OVERVIEW"",""onlyFor"":[],""notFor"":[]},{""report"":""WEEKLY_REVIEW"",""onlyFor"":[],""notFor"":[]}]"
    Public smsSendingAllowed As String          ' "True"
    Public postal As postalRecord               ' "{""postalFormat"":[""99999-9999"",""99999""],""regExpStr"":""^(\\d{5}-\\d{4})$|^(\\d{5})$""}"
    Public numberFormat As numberFormatRecord   ' "{""decimalSeparator"":""."",""groupsSeparator"":"",""}"
    Public DataValid As Boolean

    Public Sub New()
        DataValid = False
    End Sub

    Public Sub New(sessionCountrySettings As Dictionary(Of String, String))
        If sessionCountrySettings Is Nothing Then
            DataValid = False
            Exit Sub
        End If

        For Each row As KeyValuePair(Of String, String) In sessionCountrySettings
            Select Case row.Key
                Case NameOf(name)
                    name = row.Value
                Case NameOf(languages)
                    languages = New languagesRecord(row.Value)
                Case NameOf(defaultLanguage)
                    defaultLanguage = row.Value
                Case NameOf(defaultCountryName)
                    defaultCountryName = row.Value
                Case NameOf(defaultDevice)
                    defaultDevice = row.Value
                Case NameOf(dialCode)
                    dialCode = row.Value
                Case NameOf(cpMobileAppAvailable)
                    cpMobileAppAvailable = row.Value
                Case NameOf(uploaderAllowed)
                    uploaderAllowed = row.Value
                Case NameOf(techSupport)
                    techSupport = row.Value
                Case NameOf(techDays)
                    techDays = row.Value
                Case NameOf(firstDayOfWeek)
                    firstDayOfWeek = row.Value
                Case NameOf(techHours)
                    techHours = row.Value
                Case NameOf(legalAge)
                    legalAge = row.Value
                Case NameOf(shortDateFormat)
                    shortDateFormat = row.Value
                Case NameOf(shortTimeFormat)
                    shortTimeFormat = row.Value
                Case NameOf(mediaHost)
                    mediaHost = row.Value
                Case NameOf(blePereodicDataEndpoint)
                    blePereodicDataEndpoint = row.Value
                Case NameOf(region)
                    region = row.Value
                Case NameOf(pathDocs)
                    pathDocs = New pathDocsRecord(row.Value)
                Case NameOf(carbDefaultUnit)
                    carbDefaultUnit = row.Value
                Case NameOf(bgUnits)
                    bgUnits = row.Value
                Case NameOf(timeFormat)
                    timeFormat = row.Value
                Case NameOf(timeUnitsDefault)
                    timeUnitsDefault = row.Value
                Case NameOf(recordSeparator)
                    recordSeparator = row.Value
                Case NameOf(glucoseUnitsDefault)
                    glucoseUnitsDefault = row.Value
                Case NameOf(carbohydrateUnitsDefault)
                    carbohydrateUnitsDefault = row.Value
                Case NameOf(carbExchangeRatioDefault)
                    carbExchangeRatioDefault = row.Value
                Case NameOf(reportDateFormat)
                    reportDateFormat = New reportDateFormatRecord(row.Value)
                Case NameOf(mfa)
                    mfa = row.Value
                Case NameOf(supportedReports)
                    supportedReports = New supportedReportsRecord(row.Value)
                Case NameOf(smsSendingAllowed)
                    smsSendingAllowed = row.Value
                Case NameOf(postal)
                    postal = New postalRecord(row.Value)
                Case NameOf(numberFormat)
                    numberFormat = New numberFormatRecord(row.Value)
                Case Else
                    Stop
            End Select
        Next
        DataValid = True
    End Sub
End Class
