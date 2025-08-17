' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class CountrySettingsRecord

    Public Sub New()
        _hasValue = False
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
    Public Property stateCodes As Object
    Public Property smsSendingAllowed As String
    Public Property techDays As String
    Public Property techHours As String
    Public Property techSupport As String
    Public Property timeFormat As String
    Public Property timeUnitsDefault As String
    Public Property uploaderAllowed As String

#Region "Lists"

    Private _hasValue As Boolean

    Private Property languages As New List(Of LanguageRecord)
    ' "[{""name"":""English"",""code"":""EN""}]"

    Private Property supportedReports As New List(Of SupportedReportRecord)
    '"[{""report"":""ADHERENCE"",""onlyFor"":[],""notFor"":[]},
    '{""report"":""ASSESSMENT_AND_PROGRESS"",""onlyFor"":[],""notFor"":[]},
    '{""report"":""BOLUS_WIZARD_FOOD_BOLUS"",""onlyFor"":[],""notFor"":[]},
    '{""report"":""DAILY_DETAILS"",""onlyFor"":[],""notFor"":[]},
    '{""report"":""DASHBOARD"",""onlyFor"":[],""notFor"":[]},
    '{""report"":""DEVICE_SETTINGS"",""onlyFor"":[],""notFor"":[]},
    '{""report"":""EPISODE_SUMMARY"",""onlyFor"":[],""notFor"":[]},
    '{""report"":""LOGBOOK"",""onlyFor"":[],""notFor"":[]},
    '{""report"":""OVERVIEW"",""onlyFor"":[],""notFor"":[]},
    '{""report"":""WEEKLY_REVIEW"",""onlyFor"":[],""notFor"":[]}]"


#End Region

    Public Sub Clear()
        _hasValue = False
    End Sub

    Public Function HasValue() As Boolean
        Return _hasValue
    End Function

End Class
