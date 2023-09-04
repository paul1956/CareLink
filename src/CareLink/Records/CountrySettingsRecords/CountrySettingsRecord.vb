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
        With Form1
            For Each dgv As DataGridView In { .DgvCountryDataPg1, .DgvCountryDataPg2, .DgvCountryDataPg3}
                dgv.Rows.Clear()
                dgv.RowHeadersVisible = False
                dgv.SelectionMode = DataGridViewSelectionMode.CellSelect
                dgv.InitializeDgv()
                dgv.ContextMenuStrip = Form1.DgvCopyWithoutExcelMenuStrip
            Next

            Dim currentLeftRow As Integer = 0
            Dim currentRightRow As Integer = 0
            For Each row As IndexClass(Of KeyValuePair(Of String, String)) In jsonData.WithIndex
                Dim rowValue As KeyValuePair(Of String, String) = row.Value

                Dim itemIndex As String = $"{row.Index + 1}"
                Select Case rowValue.Key
                    Case NameOf(name)
                        Me.name = rowValue.Value
                        .DgvCountryDataPg1.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(languages)
                        Me.languages.Clear()
                        For Each dic As IndexClass(Of Dictionary(Of String, String)) In JsonToLisOfDictionary(rowValue.Value).WithIndex
                            Me.languages.Add(New LanguageRecord(dic.Value))
                            .DgvCountryDataPg1.Rows.Add($"{row.Index + 1}.{dic.Index + 1}", rowValue.Key, Me.languages.Last.GetCsvKeys, Me.languages.Last.GetCsvValues)
                        Next

                    Case NameOf(defaultLanguage)
                        Me.defaultLanguage = rowValue.Value
                        .DgvCountryDataPg1.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(defaultCountryName)
                        Me.defaultCountryName = rowValue.Value
                        .DgvCountryDataPg1.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(defaultDevice)
                        Me.defaultDevice = rowValue.Value
                        .DgvCountryDataPg1.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(dialCode)
                        Me.dialCode = rowValue.Value
                        .DgvCountryDataPg1.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(cpMobileAppAvailable)
                        Me.cpMobileAppAvailable = rowValue.Value
                        .DgvCountryDataPg1.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(uploaderAllowed)
                        Me.uploaderAllowed = rowValue.Value
                        .DgvCountryDataPg1.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(techSupport)
                        Me.techSupport = rowValue.Value
                        .DgvCountryDataPg1.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(techDays)
                        Me.techDays = rowValue.Value
                        .DgvCountryDataPg1.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(firstDayOfWeek)
                        Me.firstDayOfWeek = rowValue.Value
                        .DgvCountryDataPg1.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(techHours)
                        Me.techHours = rowValue.Value
                        .DgvCountryDataPg1.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(legalAge)
                        Me.legalAge = rowValue.Value
                        .DgvCountryDataPg1.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(shortDateFormat)
                        Me.shortDateFormat = rowValue.Value
                        .DgvCountryDataPg1.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(shortTimeFormat)
                        Me.shortTimeFormat = rowValue.Value
                        .DgvCountryDataPg1.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(mediaHost)
                        Me.mediaHost = rowValue.Value
                        .DgvCountryDataPg1.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(blePereodicDataEndpoint)
                        Me.blePereodicDataEndpoint = rowValue.Value
                        .DgvCountryDataPg1.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(region)
                        Me.region = rowValue.Value
                        .DgvCountryDataPg1.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(pathDocs)
                        Me.pathDocs = New PathDocsRecord(rowValue.Value)
                        For Each member As IndexClass(Of KeyValuePair(Of String, String)) In Me.pathDocs.pathDoc.WithIndex
                            Dim itemIndex1 As String = $"{row.Index + 1}.{member.Index + 1}"
                            .DgvCountryDataPg2.Rows.Add(itemIndex1, rowValue.Key, member.Value.Key, member.Value.Value)
                        Next
                    Case NameOf(carbDefaultUnit)
                        Me.carbDefaultUnit = rowValue.Value
                        .DgvCountryDataPg2.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(bgUnits)
                        Me.bgUnits = rowValue.Value
                        .DgvCountryDataPg2.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(timeFormat)
                        Me.timeFormat = rowValue.Value
                        .DgvCountryDataPg2.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(timeUnitsDefault)
                        Me.timeUnitsDefault = rowValue.Value
                        .DgvCountryDataPg2.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(recordSeparator)
                        Me.recordSeparator = rowValue.Value
                        .DgvCountryDataPg2.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(glucoseUnitsDefault)
                        Me.glucoseUnitsDefault = rowValue.Value
                        .DgvCountryDataPg2.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(carbohydrateUnitsDefault)
                        Me.carbohydrateUnitsDefault = rowValue.Value
                        .DgvCountryDataPg2.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(carbExchangeRatioDefault)
                        Me.carbExchangeRatioDefault = rowValue.Value
                        .DgvCountryDataPg2.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(reportDateFormat)
                        Me.reportDateFormat = New ReportDateFormatRecord(rowValue.Value)
                        For Each member As IndexClass(Of KeyValuePair(Of String, String)) In Me.reportDateFormat.ToList.WithIndex
                            .DgvCountryDataPg2.Rows.Add($"{row.Index + 1}.{member.Index + 1}", rowValue.Key, member.Value.Key, member.Value.Value)
                        Next

                    Case NameOf(mfa)
                        Me.mfa = New MfaRecord(rowValue.Value)
                        For Each member As IndexClass(Of KeyValuePair(Of String, String)) In Me.mfa.ToList.WithIndex
                            .DgvCountryDataPg3.Rows.Add($"{row.Index + 1}.{member.Index + 1}", rowValue.Key, member.Value.Key, member.Value.Value)
                        Next

                    Case NameOf(supportedReports)
                        Me.supportedReports.Clear()
                        For Each dic As IndexClass(Of Dictionary(Of String, String)) In JsonToLisOfDictionary(rowValue.Value).WithIndex
                            Me.supportedReports.Add(New SupportedReportRecord(dic.Value, dic.Index + 1))
                            With Me.supportedReports.Last
                                Form1.DgvCountryDataPg3.Rows.Add($"{row.Index + 1}.{dic.Index + 1}", rowValue.Key, dic.Value.Keys(0), .report, .onlyFor, .notFor)
                            End With
                        Next
                    Case NameOf(smsSendingAllowed)
                        Me.smsSendingAllowed = rowValue.Value
                        .DgvCountryDataPg3.Rows.Add(itemIndex, "", rowValue.Key, rowValue.Value)
                    Case NameOf(postal)
                        Me.postal = New PostalRecord(rowValue.Value)
                        For Each member As IndexClass(Of KeyValuePair(Of String, String)) In Me.postal.ToList.WithIndex
                            .DgvCountryDataPg3.Rows.Add($"{row.Index + 1}.{member.Index + 1}", rowValue.Key, member.Value.Key, member.Value.Value)
                        Next

                    Case NameOf(numberFormat)
                        Me.numberFormat = New NumberFormatRecord(rowValue.Value)
                        For Each member As IndexClass(Of KeyValuePair(Of String, String)) In Me.numberFormat.ToList.WithIndex
                            .DgvCountryDataPg3.Rows.Add(itemIndex, rowValue.Key, member.Value.Key, member.Value.Value)
                        Next
                    Case Else
                        Stop
                End Select
            Next
            Application.DoEvents()
        End With
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
    Public Property mfa As MfaRecord
    Public Property name As String
    Public Property numberFormat As NumberFormatRecord
    Public Property pathDocs As PathDocsRecord
    Public Property postal As PostalRecord
    Public Property recordSeparator As String
    Public Property region As String
    Public Property reportDateFormat As ReportDateFormatRecord
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
    Private Property languages As New List(Of LanguageRecord)                 ' "[{""name"":""English"",""code"":""EN""}]"
    Private Property supportedReports As New List(Of SupportedReportRecord)   ' "[{""report"":""ADHERENCE"",""onlyFor"":[],""notFor"":[]},{""report"":""ASSESSMENT_AND_PROGRESS"",""onlyFor"":[],""notFor"":[]},{""report"":""BOLUS_WIZARD_FOOD_BOLUS"",""onlyFor"":[],""notFor"":[]},{""report"":""DAILY_DETAILS"",""onlyFor"":[],""notFor"":[]},{""report"":""DASHBOARD"",""onlyFor"":[],""notFor"":[]},{""report"":""DEVICE_SETTINGS"",""onlyFor"":[],""notFor"":[]},{""report"":""EPISODE_SUMMARY"",""onlyFor"":[],""notFor"":[]},{""report"":""LOGBOOK"",""onlyFor"":[],""notFor"":[]},{""report"":""OVERVIEW"",""onlyFor"":[],""notFor"":[]},{""report"":""WEEKLY_REVIEW"",""onlyFor"":[],""notFor"":[]}]"

#End Region

    Public Sub Clear()
        _hasValue = False
    End Sub

    Public Function HasValue() As Boolean
        Return _hasValue
    End Function

End Class
