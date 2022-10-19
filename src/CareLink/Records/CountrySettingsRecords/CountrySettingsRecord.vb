' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class CountrySettingsRecord

    Public Sub New()
        _hasValue = False
    End Sub

    Public Sub New(jsonData As Dictionary(Of String, String))
        If jsonData Is Nothing OrElse jsonData.Count = 0 Then
            _hasValue = False
            Exit Sub
        End If

        Dim dgvCountryItems() As DataGridView = {My.Forms.Form1.DataGridViewCountryItemsPage1, My.Forms.Form1.DataGridViewCountryItemsPage2, My.Forms.Form1.DataGridViewCountryItemsPage3}
        dgvCountryItems(0).Rows.Clear()
        dgvCountryItems(0).RowHeadersVisible = False
        dgvCountryItems(1).Rows.Clear()
        dgvCountryItems(1).RowHeadersVisible = False
        dgvCountryItems(2).Rows.Clear()
        dgvCountryItems(2).RowHeadersVisible = False
        Dim currentLeftRow As Integer = 0
        Dim currentRightRow As Integer = 0
        For Each row As IndexClass(Of KeyValuePair(Of String, String)) In jsonData.WithIndex
            Dim rowValue As KeyValuePair(Of String, String) = row.Value

            Dim itemIndex As String = $"{row.Index + 1}"
            Select Case rowValue.Key
                Case NameOf(name)
                    Me.name = rowValue.Value
                    dgvCountryItems(0).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(languages)
                    languages.Clear()
                    For Each dic As IndexClass(Of Dictionary(Of String, String)) In LoadList(rowValue.Value).WithIndex
                        languages.Add(New LanguageRecord(dic.Value))
                        dgvCountryItems(0).Rows.Add($"{row.Index + 1}.{dic.Index + 1}", rowValue.Key, languages.Last.GetCsvKeys, languages.Last.GetCsvValues)
                    Next

                Case NameOf(defaultLanguage)
                    Me.defaultLanguage = rowValue.Value
                    dgvCountryItems(0).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(defaultCountryName)
                    Me.defaultCountryName = rowValue.Value
                    dgvCountryItems(0).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(defaultDevice)
                    Me.defaultDevice = rowValue.Value
                    dgvCountryItems(0).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(dialCode)
                    Me.dialCode = rowValue.Value
                    dgvCountryItems(0).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(cpMobileAppAvailable)
                    Me.cpMobileAppAvailable = rowValue.Value
                    dgvCountryItems(0).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(uploaderAllowed)
                    Me.uploaderAllowed = rowValue.Value
                    dgvCountryItems(0).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(techSupport)
                    Me.techSupport = rowValue.Value
                    dgvCountryItems(0).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(techDays)
                    Me.techDays = rowValue.Value
                    dgvCountryItems(0).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(firstDayOfWeek)
                    Me.firstDayOfWeek = rowValue.Value
                    dgvCountryItems(0).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(techHours)
                    Me.techHours = rowValue.Value
                    dgvCountryItems(0).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(legalAge)
                    Me.legalAge = rowValue.Value
                    dgvCountryItems(0).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(shortDateFormat)
                    Me.shortDateFormat = rowValue.Value
                    dgvCountryItems(0).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(shortTimeFormat)
                    Me.shortTimeFormat = rowValue.Value
                    dgvCountryItems(0).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(mediaHost)
                    Me.mediaHost = rowValue.Value
                    dgvCountryItems(0).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(blePereodicDataEndpoint)
                    Me.blePereodicDataEndpoint = rowValue.Value
                    dgvCountryItems(0).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(region)
                    Me.region = rowValue.Value
                    dgvCountryItems(0).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(pathDocs)
                    Me.pathDocs = New pathDocsRecord(rowValue.Value)
                    For Each member As IndexClass(Of KeyValuePair(Of String, String)) In Me.pathDocs.pathDoc.WithIndex
                        Dim itemIndex1 As String = $"{row.Index + 1}.{member.Index + 1}"
                        dgvCountryItems(1).Rows.Add(itemIndex1, rowValue.Key, member.Value.Key, member.Value.Value)
                    Next
                Case NameOf(carbDefaultUnit)
                    Me.carbDefaultUnit = rowValue.Value
                    dgvCountryItems(1).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(bgUnits)
                    Me.bgUnits = rowValue.Value
                    dgvCountryItems(1).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(timeFormat)
                    Me.timeFormat = rowValue.Value
                    dgvCountryItems(1).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(timeUnitsDefault)
                    Me.timeUnitsDefault = rowValue.Value
                    dgvCountryItems(1).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(recordSeparator)
                    Me.recordSeparator = rowValue.Value
                    dgvCountryItems(1).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(glucoseUnitsDefault)
                    Me.glucoseUnitsDefault = rowValue.Value
                    dgvCountryItems(1).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(carbohydrateUnitsDefault)
                    Me.carbohydrateUnitsDefault = rowValue.Value
                    dgvCountryItems(1).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(carbExchangeRatioDefault)
                    Me.carbExchangeRatioDefault = rowValue.Value
                    dgvCountryItems(1).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(reportDateFormat)
                    Me.reportDateFormat = New reportDateFormatRecord(rowValue.Value)
                    For Each member As IndexClass(Of KeyValuePair(Of String, String)) In Me.reportDateFormat.ToList.WithIndex
                        dgvCountryItems(1).Rows.Add($"{row.Index + 1}.{member.Index + 1}", rowValue.Key, member.Value.Key, member.Value.Value)
                    Next

                Case NameOf(mfa)
                    Me.mfa = New mfaRecord(rowValue.Value)
                    For Each member As IndexClass(Of KeyValuePair(Of String, String)) In Me.mfa.ToList.WithIndex
                        dgvCountryItems(2).Rows.Add($"{row.Index + 1}.{member.Index + 1}", rowValue.Key, member.Value.Key, member.Value.Value)
                    Next

                Case NameOf(supportedReports)
                    supportedReports.Clear()
                    For Each dic As IndexClass(Of Dictionary(Of String, String)) In LoadList(rowValue.Value).WithIndex
                        supportedReports.Add(New supportedReportRecord(dic.Value, dic.Index + 1))
                        With supportedReports.Last
                            dgvCountryItems(2).Rows.Add($"{row.Index + 1}.{dic.Index + 1}", rowValue.Key, dic.Value.Keys(0), .report, .onlyFor, .notFor)
                        End With
                    Next
                Case NameOf(smsSendingAllowed)
                    Me.smsSendingAllowed = rowValue.Value
                    dgvCountryItems(2).Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                Case NameOf(postal)
                    Me.postal = New postalRecord(rowValue.Value)
                    For Each member As IndexClass(Of KeyValuePair(Of String, String)) In Me.postal.ToList.WithIndex
                        dgvCountryItems(2).Rows.Add($"{row.Index + 1}.{member.Index + 1}", rowValue.Key, member.Value.Key, member.Value.Value)
                    Next

                Case NameOf(numberFormat)
                    Me.numberFormat = New numberFormatRecord(rowValue.Value)
                    For Each member As IndexClass(Of KeyValuePair(Of String, String)) In Me.numberFormat.ToList.WithIndex
                        dgvCountryItems(2).Rows.Add(itemIndex, rowValue.Key, member.Value.Key, member.Value.Value)
                    Next
                Case Else
                    Stop
            End Select
            Application.DoEvents()
        Next
        _hasValue = True
    End Sub

    Public Property bgUnits As String
    Public Property blePereodicDataEndpoint As String
    Public Property carbDefaultUnit As String
    Public Property carbExchangeRatioDefault As String
    Public Property carbohydrateUnitsDefault As String
    Public Property cpMobileAppAvailable As String
    Public Property defaultCountryName As String
    Public Property defaultDevice As String
    Public Property defaultLanguage As String
    Public Property dialCode As String
    Public Property firstDayOfWeek As String
    Public Property glucoseUnitsDefault As String
    Public Property legalAge As String
    Public Property mediaHost As String
    Public Property mfa As mfaRecord
    Public Property name As String
    Public Property numberFormat As numberFormatRecord

    ' "{""ddms.termsOfUse"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/terms_of_use.html"",""ddms.privacyStatementPdf"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/privacy_policy.pdf"",""ddms.termsOfUsePdf"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/terms_of_use.pdf"",""ddms.privacyStatement"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/privacy_policy.pdf"",""ddms.faqPdf"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/faq.pdf"",""ddms.privacyPractices"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/notices.pdf""}"
    Public Property pathDocs As pathDocsRecord

    Public Property postal As postalRecord

    Public Property recordSeparator As String
    Public Property region As String

    ' "{""longTimePattern12"":""hh:mm:ss tt"",""longTimePattern24"":""HH:mm:ss"",""shortTimePattern12"":""h:mm tt"",""shortTimePattern24"":""HH:mm"",""shortDatePattern"":""MM-dd-yyyy"",""dateSeparator"":""-"",""timeSeparator"":"":""}"
    Public Property reportDateFormat As reportDateFormatRecord

    Public Property shortDateFormat As String
    Public Property shortTimeFormat As String
    Public Property smsSendingAllowed As String
    Public Property techDays As String
    Public Property techHours As String
    Public Property techSupport As String
    Public Property timeFormat As String
    Public Property timeUnitsDefault As String
    Public Property uploaderAllowed As String

#Region "Lists"

    Private _hasValue As Boolean
    Public languages As New List(Of LanguageRecord)                 ' "[{""name"":""English"",""code"":""EN""}]"
    Public supportedReports As New List(Of supportedReportRecord)   ' "[{""report"":""ADHERENCE"",""onlyFor"":[],""notFor"":[]},{""report"":""ASSESSMENT_AND_PROGRESS"",""onlyFor"":[],""notFor"":[]},{""report"":""BOLUS_WIZARD_FOOD_BOLUS"",""onlyFor"":[],""notFor"":[]},{""report"":""DAILY_DETAILS"",""onlyFor"":[],""notFor"":[]},{""report"":""DASHBOARD"",""onlyFor"":[],""notFor"":[]},{""report"":""DEVICE_SETTINGS"",""onlyFor"":[],""notFor"":[]},{""report"":""EPISODE_SUMMARY"",""onlyFor"":[],""notFor"":[]},{""report"":""LOGBOOK"",""onlyFor"":[],""notFor"":[]},{""report"":""OVERVIEW"",""onlyFor"":[],""notFor"":[]},{""report"":""WEEKLY_REVIEW"",""onlyFor"":[],""notFor"":[]}]"

#End Region

    Public Sub Clear()
        _hasValue = False
    End Sub

    Public Function HasValue() As Boolean
        Return _hasValue
    End Function

End Class
