' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class GetReportsSettingsRecord
    Public Property clientTime As String = $"{Now:O}"
    Public Property dailyDetailReportDays As New List(Of String)
    Public Property endDate As String = $"{Now.Year}-{Now.Month:D2}-{Now.Day:D2}"
    Public Property patientId As String = EmptyString
    Public Property reportFileFormat As String = "PDF"
    Public Property reportShowAdherence As Boolean = False
    Public Property reportShowAssessmentAndProgress As Boolean = False
    Public Property reportShowBolusWizardFoodBolus As Boolean = False
    Public Property reportShowDashBoard As Boolean = False
    Public Property reportShowDataTable As Boolean = False
    Public Property reportShowDeviceSettings As Boolean = True
    Public Property reportShowEpisodeSummary As Boolean = False
    Public Property reportShowLogbook As Boolean = False
    Public Property reportShowOverview As Boolean = False
    Public Property reportShowWeeklyReview As Boolean = False
    Public Property startDate As String = $"{Now.Year}-{Now.Month:D2}-{Now.Day:D2}"

End Class
