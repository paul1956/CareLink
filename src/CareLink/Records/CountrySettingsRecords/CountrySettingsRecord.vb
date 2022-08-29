' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Diagnostics.Metrics
Imports CareLink.PumpVariables

Public Class CountrySettingsRecord

#Region "Single Items"

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
    Public Property recordSeparator As String
    Public Property region As String
    Public Property shortDateFormat As String
    Public Property shortTimeFormat As String
    Public Property smsSendingAllowed As String
    Public Property techDays As String
    Public Property techHours As String
    Public Property techSupport As String
    Public Property timeFormat As String
    Public Property timeUnitsDefault As String
    Public Property uploaderAllowed As String

#End Region 'Single Items

#Region "Flat Records"

    Public numberFormat As numberFormatRecord
    Public pathDocs As pathDocsRecord                   ' "{""ddms.termsOfUse"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/terms_of_use.html"",""ddms.privacyStatementPdf"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/privacy_policy.pdf"",""ddms.termsOfUsePdf"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/terms_of_use.pdf"",""ddms.privacyStatement"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/privacy_policy.pdf"",""ddms.faqPdf"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/faq.pdf"",""ddms.privacyPractices"":""https://carelink.minimed.com/crs/ocl/14.06/media/en/us/notices.pdf""}"
    Public postal As postalRecord
    Public reportDateFormat As reportDateFormatRecord   ' "{""longTimePattern12"":""hh:mm:ss tt"",""longTimePattern24"":""HH:mm:ss"",""shortTimePattern12"":""h:mm tt"",""shortTimePattern24"":""HH:mm"",""shortDatePattern"":""MM-dd-yyyy"",""dateSeparator"":""-"",""timeSeparator"":"":""}"

#End Region

#Region "Lists"

    Public languages As New List(Of LanguageRecord)                 ' "[{""name"":""English"",""code"":""EN""}]"
    Public supportedReports As New List(Of supportedReportRecord)   ' "[{""report"":""ADHERENCE"",""onlyFor"":[],""notFor"":[]},{""report"":""ASSESSMENT_AND_PROGRESS"",""onlyFor"":[],""notFor"":[]},{""report"":""BOLUS_WIZARD_FOOD_BOLUS"",""onlyFor"":[],""notFor"":[]},{""report"":""DAILY_DETAILS"",""onlyFor"":[],""notFor"":[]},{""report"":""DASHBOARD"",""onlyFor"":[],""notFor"":[]},{""report"":""DEVICE_SETTINGS"",""onlyFor"":[],""notFor"":[]},{""report"":""EPISODE_SUMMARY"",""onlyFor"":[],""notFor"":[]},{""report"":""LOGBOOK"",""onlyFor"":[],""notFor"":[]},{""report"":""OVERVIEW"",""onlyFor"":[],""notFor"":[]},{""report"":""WEEKLY_REVIEW"",""onlyFor"":[],""notFor"":[]}]"
    Private _hasValue As Boolean

#End Region

    Public Sub New()
        _hasValue = False
    End Sub

    Public Sub New(jsonData As Dictionary(Of String, String))
        If jsonData Is Nothing OrElse jsonData.Count = 0 Then
            _hasValue = False
            Exit Sub
        End If

        Dim dgvCountryItems As DataGridView = My.Forms.Form1.DataGridViewCountryItems
        dgvCountryItems.Rows.Clear()
        Dim currentLeftRow As Integer = 0
        Dim currentRightRow As Integer = 0
        For Each row As IndexClass(Of KeyValuePair(Of String, String)) In jsonData.WithIndex
            Dim rowValue As KeyValuePair(Of String, String) = row.Value

            Dim itemIndex As String = $"{row.Index + 1}"
            Select Case rowValue.Key
                Case NameOf(name)
                    Me.name = rowValue.Value
                    dgvCountryItems.AddLeft("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(languages)
                    languages.Clear()
                    currentRightRow = dgvCountryItems.RowCount - 1
                    For Each dic As IndexClass(Of Dictionary(Of String, String)) In LoadList(rowValue.Value).WithIndex
                        languages.Add(New LanguageRecord(dic.Value))
                        Dim itemIndex1 As String = $"{itemIndex}.{dic.Index + 1}"
                        Dim key As String = languages.Last.GetCsvKeys
                        Dim value As String = languages.Last.GetCsvValues
                        dgvCountryItems.AddRight(NameOf(languages), $"{row.Index + 1}.{dic.Index + 1}", currentLeftRow, currentRightRow, KeyValuePair.Create(key, value))
                    Next

                Case NameOf(defaultLanguage)
                    Me.defaultLanguage = rowValue.Value
                    dgvCountryItems.AddLeft("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(defaultCountryName)
                    Me.defaultCountryName = rowValue.Value
                    dgvCountryItems.AddRight("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(defaultDevice)
                    Me.defaultDevice = rowValue.Value
                    dgvCountryItems.AddLeft("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(dialCode)
                    Me.dialCode = rowValue.Value
                    dgvCountryItems.AddRight("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(cpMobileAppAvailable)
                    Me.cpMobileAppAvailable = rowValue.Value
                    dgvCountryItems.AddLeft("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(uploaderAllowed)
                    Me.uploaderAllowed = rowValue.Value
                    dgvCountryItems.AddRight("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(techSupport)
                    Me.techSupport = rowValue.Value
                    dgvCountryItems.AddLeft("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(techDays)
                    Me.techDays = rowValue.Value
                    dgvCountryItems.AddRight("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(firstDayOfWeek)
                    Me.firstDayOfWeek = rowValue.Value
                    dgvCountryItems.AddLeft("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(techHours)
                    Me.techHours = rowValue.Value
                    dgvCountryItems.AddRight("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(legalAge)
                    Me.legalAge = rowValue.Value
                    dgvCountryItems.AddLeft("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(shortDateFormat)
                    Me.shortDateFormat = rowValue.Value
                    dgvCountryItems.AddRight("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(shortTimeFormat)
                    Me.shortTimeFormat = rowValue.Value
                    dgvCountryItems.AddLeft("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(mediaHost)
                    Me.mediaHost = rowValue.Value
                    dgvCountryItems.AddRight("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(blePereodicDataEndpoint)
                    Me.blePereodicDataEndpoint = rowValue.Value
                    dgvCountryItems.AddLeft("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(region)
                    Me.region = rowValue.Value
                    dgvCountryItems.AddRight("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(pathDocs)
                    pathDocs = New pathDocsRecord(rowValue.Value)
                    For Each member As IndexClass(Of KeyValuePair(Of String, String)) In pathDocs.pathDoc.WithIndex
                        Dim itemIndex1 As String = $"{row.Index + 1}.{member.Index + 1}"
                        dgvCountryItems.AddLeft(NameOf(pathDocs), itemIndex1, currentLeftRow, currentRightRow, member.Value)
                    Next
                Case NameOf(carbDefaultUnit)
                    Me.carbDefaultUnit = rowValue.Value
                    dgvCountryItems.AddRight("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(bgUnits)
                    Me.bgUnits = rowValue.Value
                    dgvCountryItems.AddLeft("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(timeFormat)
                    Me.timeFormat = rowValue.Value
                    dgvCountryItems.AddRight("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(timeUnitsDefault)
                    Me.timeUnitsDefault = rowValue.Value
                    dgvCountryItems.AddLeft("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(recordSeparator)
                    Me.recordSeparator = rowValue.Value
                    dgvCountryItems.AddRight("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(glucoseUnitsDefault)
                    Me.glucoseUnitsDefault = rowValue.Value
                    dgvCountryItems.AddLeft("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(carbohydrateUnitsDefault)
                    Me.carbohydrateUnitsDefault = rowValue.Value
                    dgvCountryItems.AddRight("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(carbExchangeRatioDefault)
                    Me.carbExchangeRatioDefault = rowValue.Value
                    dgvCountryItems.AddLeft("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(reportDateFormat)
                    reportDateFormat = New reportDateFormatRecord(rowValue.Value)
                    For Each member As IndexClass(Of KeyValuePair(Of String, String)) In reportDateFormat.ToList.WithIndex
                        dgvCountryItems.AddRight(NameOf(reportDateFormat), $"{row.Index + 1}.{member.Index + 1}", currentLeftRow, currentRightRow, member.Value)
                    Next

                Case NameOf(mfa)
                    Me.mfa = New mfaRecord(rowValue.Value)
                    For Each member As IndexClass(Of KeyValuePair(Of String, String)) In Me.mfa.ToList.WithIndex
                        dgvCountryItems.AddRight(NameOf(mfa), $"{row.Index + 1}.{member.Index + 1}", currentLeftRow, currentRightRow, member.Value)
                    Next

                Case NameOf(supportedReports)
                    supportedReports.Clear()
                    For Each dic As IndexClass(Of Dictionary(Of String, String)) In LoadList(rowValue.Value).WithIndex
                        supportedReports.Add(New supportedReportRecord(dic.Value, dic.Index + 1))
                        Dim value As String
                        With supportedReports.Last
                            value = $"{ .report} notFor '{ .notFor}', onlyFor '{ .onlyFor}'"
                        End With
                        dgvCountryItems.AddLeft(NameOf(supportedReports), $"{row.Index + 1}.{dic.Index + 1}", currentLeftRow, currentRightRow, KeyValuePair.Create(dic.Value.Keys(0), value))
                    Next
                Case NameOf(smsSendingAllowed)
                    Me.smsSendingAllowed = rowValue.Value
                    dgvCountryItems.AddRight("Country", itemIndex, currentLeftRow, currentRightRow, row.Value)
                Case NameOf(postal)
                    postal = New postalRecord(rowValue.Value)
                    For Each member As IndexClass(Of KeyValuePair(Of String, String)) In postal.ToList.WithIndex
                        dgvCountryItems.AddRight(NameOf(postalRecord), $"{row.Index + 1}.{member.Index + 1}", currentLeftRow, currentRightRow, member.Value)
                    Next

                Case NameOf(numberFormat)
                    numberFormat = New numberFormatRecord(rowValue.Value)
                    For Each member As IndexClass(Of KeyValuePair(Of String, String)) In numberFormat.ToList.WithIndex
                        dgvCountryItems.AddLeft(NameOf(numberFormatRecord), itemIndex, currentLeftRow, currentRightRow, member.Value)
                    Next
                Case Else
                    Stop
            End Select
            Application.DoEvents()
        Next
        _hasValue = True
    End Sub

    Public Sub Clear()
        _hasValue = False
    End Sub
    Public Function HasValue() As Boolean
        Return _hasValue
    End Function

End Class
