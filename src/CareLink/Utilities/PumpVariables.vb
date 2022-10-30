' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Module PumpVariables

    ' Manually computed

    Friend s_totalAutoCorrection As Single
    Friend s_totalBasal As Single
    Friend s_totalCarbs As Single
    Friend s_totalDailyDose As Single
    Friend s_totalManualBolus As Single

#Region "Global variables to hold pump values"

#Region "Used for painting"

    Public ReadOnly s_homeTabMarkerInsulinDictionary As New Dictionary(Of OADate, Single)
    Public ReadOnly s_homeTabMarkerMealDictionary As New Dictionary(Of OADate, Single)
    Public ReadOnly s_treatmentMarkerInsulinDictionary As New Dictionary(Of OADate, Single)
    Public ReadOnly s_treatmentMarkerMealDictionary As New Dictionary(Of OADate, Single)

#End Region ' Used for painting

    Friend Const MmolLUnitsDivisor As Single = 18
    Friend ReadOnly s_insulinImage As Bitmap = My.Resources.InsulinVial_Tiny
    Friend ReadOnly s_listOfAutoBasalDeliveryMarkers As New List(Of AutoBasalDeliveryRecord)
    Friend ReadOnly s_listOfAutoModeStatusMarkers As New List(Of AutoModeStatusRecord)
    Friend ReadOnly s_listOfBgReadingMarkers As New List(Of BGReadingRecord)
    Friend ReadOnly s_listOfCalibrationMarkers As New List(Of CalibrationRecord)
    Friend ReadOnly s_listOfInsulinMarkers As New List(Of InsulinRecord)
    Friend ReadOnly s_listOflimitRecords As New List(Of LimitsRecord)
    Friend ReadOnly s_listOfLowGlucoseSuspendedMarkers As New List(Of LowGlusoceSuspendRecord)
    Friend ReadOnly s_listOfMealMarkers As New List(Of MealRecord)
    Friend ReadOnly s_listOfSummaryRecords As New List(Of SummaryRecord)
    Friend ReadOnly s_mealImage As Bitmap = My.Resources.MealImage
    Friend s_aboveHyperLimit As Double
    Friend s_activeInsulin As ActiveInsulinRecord
    Friend s_activeInsulinIncrements As Integer
    Friend s_basalValue As Dictionary(Of String, String)
    Friend s_belowHypoLimit As Single
    Friend s_clientTimeZone As TimeZoneInfo
    Friend s_clientTimeZoneName As String
    Friend s_criticalLow As Single
    Friend s_filterJsonData As Boolean = True
    Friend s_firstName As String = ""
    Friend s_lastAlarmValue As Dictionary(Of String, String)
    Friend s_lastBGDiff As Double = 0
    Friend s_lastBGTime As Date
    Friend s_lastBGValue As Single = 0
    Friend s_lastMedicalDeviceDataUpdateServerEpoch As Long
    Friend s_lastSgRecord As New SgRecord
    Friend s_limitHigh As Single
    Friend s_limitLow As Single
    Friend s_listOfSGs As New List(Of SgRecord)
    Friend s_listOfTimeChangeMarkers As New List(Of TimeChangeRecord)
    Friend s_markers As New List(Of Dictionary(Of String, String))
    Friend s_notificationHistoryValue As Dictionary(Of String, String)
    Friend s_pumpBannerStateValue As New List(Of Dictionary(Of String, String))
    Friend s_recentDatalast As Dictionary(Of String, String)
    Friend s_sensorState As String
    Friend s_sessionCountrySettings As New CountrySettingsRecord
    Friend s_systemStatusMessage As String
    Friend s_theraphyAlgorthmStateValue As Dictionary(Of String, String)
    Friend s_timeInRange As Integer
    Friend s_timeWithMinuteFormat As String
    Friend s_timeWithoutMinuteFormat As String
    Friend Property BgUnits As String
    Friend Property BgUnitsString As String

#End Region

End Module
