' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting

Public Module SystemConstants

    Friend Const RsaKeySize As Integer = 2048
    Friend Const BaseNameSavedErrorReport As String = "CareLinkErrorReport"
    Friend Const BaseNameSavedLastDownload As String = "CareLinkLastDownload"
    Friend Const BaseNameSavedSnapshot As String = "CareLinkSnapshot"
    Friend Const ClickToShowDetails As String = "Click To Show Details"
    Friend Const ExceptionStartingString As String = "--- Start of Exception ---"
    Friend Const ExceptionTerminatingString As String = "--- End of Exception ---"
    Friend Const GitOwnerName As String = "Paul1956"
    Friend Const MmolLUnitsDivisor As Single = 18
    Friend Const StackTraceStartingStr As String = "--- Start of stack trace ---"
    Friend Const StackTraceTerminatingStr As String = "--- End of stack trace from previous location ---"
    Friend Const TimeFormatMilitaryWithMinutes As String = "HH:mm"
    Friend Const TimeFormatMilitaryWithoutMinutes As String = "HH"
    Friend Const TimeFormatTwelveHourWithMinutes As String = " h:mm tt"
    Friend Const TimeFormatTwelveHourWithoutMinutes As String = " h tt"

    Friend ReadOnly s_calloutAnnotations As New Dictionary(Of String, CalloutAnnotation) From {
        {"SummaryChart", New CalloutAnnotation},
        {"ActiveInsulinChart", New CalloutAnnotation},
        {"TreatmentMarkersChart", New CalloutAnnotation}}

    Friend ReadOnly s_common_Headers As New Dictionary(Of String, String) From {
        {"Accept", "application/json,text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;deviceFamily=b3;q=0.9"},
        {"Accept-Language", "en-US,en;q=0.9"},
        {"Connection", "keep-alive"},
        {"sec-ch-ua", """Not/A)Brand"";v=""99"", ""Microsoft Edge"";v=""115"", ""Chromium"";v=""115"""},
        {"User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36 Edg/120.0.0.0"}}

    Friend ReadOnly s_discoverUrl As New Dictionary(Of String, String) From {
        {"EU", "https://clcloud.minimed.eu/connect/carepartner/v11/discover/android/3.2"},
        {"US", "https://clcloud.minimed.com/connect/carepartner/v11/discover/android/3.2"}}

    Friend ReadOnly s_insulinTypes As New Dictionary(Of String, InsulinActivationRecord) From {
        {$"Humalog{RegisteredTrademark}", New InsulinActivationRecord(8, 4)},
        {$"Novolog{RegisteredTrademark}", New InsulinActivationRecord(8, 4)},
        {$"Generic (Insulin Lispro)", New InsulinActivationRecord(9, 4)},
        {$"NovoRapid", New InsulinActivationRecord(7, 4)},
        {$"FIASP{RegisteredTrademark}", New InsulinActivationRecord(4, 3)},
        {$"Lyumjev{RegisteredTrademark}", New InsulinActivationRecord(3, 3)}}

    Public ReadOnly s_oneToNineteen As New List(Of String) From {
        "zero",
        "one", "two", "three", "four", "five",
        "six", "seven", "eight", "nine", "ten",
        "eleven", "twelve", "thirteen", "fourteen", "fifteen",
        "sixteen", "seventeen", "eighteen", "nineteen"}

    Public ReadOnly s_trends As New Dictionary(Of String, String) From {
        {"DOWN", "↓"},
        {"DOWN_DOUBLE", "↓↓"},
        {"DOWN_TRIPLE", "↓↓↓"},
        {"UP", "↑"},
        {"UP_DOUBLE", "↑↑"},
        {"UP_TRIPLE", "↑↑↑"},
        {"NONE", "↔"}}

    Friend ReadOnly Property GitHubCareLinkUrl As String = $"https://GitHub.com/{GitOwnerName}/CareLink/"
    Friend ReadOnly Property SavedTitle As String = "CareLink™ For Windows"

#Region "AIT Constants"

    Friend ReadOnly s_aitLengths As New Dictionary(Of String, Single) From {
            {"2:00", 2}, {"2:15", 2.25}, {"2:30", 2.5}, {"2:45", 2.75},
            {"3:00", 3}, {"3:15", 3.25}, {"3:30", 3.5}, {"3:45", 3.75},
            {"4:00", 4}, {"4:15", 4.25}, {"4:30", 4.5}, {"4:45", 4.75},
            {"5:00", 5}, {"5:15", 5.25}, {"5:30", 5.5}, {"5:45", 5.45},
            {"6:00", 6}
        }

    Public ReadOnly s_aitValues As New Dictionary(Of String, String) From {
            {"AIT 2:00", "2:00"}, {"AIT 2:15", "2:15"},
            {"AIT 2:30", "2:30"}, {"AIT 2:45", "2:45"},
            {"AIT 3:00", "3:00"}, {"AIT 3:15", "3:15"},
            {"AIT 3:30", "3:30"}, {"AIT 3:45", "3:45"},
            {"AIT 4:00", "4:00"}, {"AIT 4:15", "4:15"},
            {"AIT 4:30", "4:30"}, {"AIT 4:45", "4:45"},
            {"AIT 5:00", "5:00"}, {"AIT 5:15", "5:15"},
            {"AIT 5:30", "5:30"}, {"AIT 5:45", "5:45"},
            {"AIT 6:00", "6:00"}
        }

#End Region

#Region "BG Constant Lists"

    Friend Const MaxMmDl400 As Integer = 400
    Friend Const MaxMmolL22_2 As Single = 22.2
    Friend Const MinMmDl50 As Integer = 50
    Friend Const MinMmolL2_8 As Single = 2.8
    Friend Const Target100mgDl As Single = 100.0
    Friend Const Target110mgDl As Single = 110.0
    Friend Const Target120mgDl As Single = 120.0
    Friend Const Target5_6mmol As Single = 5.6
    Friend Const Target6_1mmol As Single = 6.1
    Friend Const Target6_7mmol As Single = 6.7
    Friend Const TirHighMmol10 As Single = 10.0
    Friend Const TirHighMmDl180 As Single = 180.0
    Friend Const TirLowMmol70 As Single = 70.0
    Friend Const TirLowMmDl3_9 As Single = 3.9

    Friend ReadOnly Property MgDlItems As New Dictionary(Of String, Single) From {
        {$"{Target100mgDl} mg/dL", Target100mgDl},
        {$"{Target110mgDl} mg/dL", Target110mgDl},
        {$"{Target120mgDl} mg/dL", Target120mgDl}}

    Friend ReadOnly Property MmolLItemsComma As New Dictionary(Of String, Single) From {
        {$"{Target5_6mmol.ToCommaDelimited} mmol/L", Target5_6mmol},
        {$"{Target6_1mmol.ToCommaDelimited} mmol/L", Target6_1mmol},
        {$"{Target6_7mmol.ToCommaDelimited} mmol/L", Target6_7mmol}}

    Friend ReadOnly Property MmolLItemsPeriod As New Dictionary(Of String, Single) From {
        {$"{Target5_6mmol.ToPeriodDelimited} mmol/L", Target5_6mmol},
        {$"{Target6_1mmol.ToPeriodDelimited} mmol/L", Target6_1mmol},
        {$"{Target6_7mmol.ToPeriodDelimited} mmol/L", Target6_7mmol}}

    Friend ReadOnly Property UnitsStrings As New Dictionary(Of String, String) From {
        {"MG_DL", "mg/dL"},
        {"MGDL", "mg/dL"},
        {"MMOL_L", "mmol/L"},
        {"MMOLL", "mmol/L"}}

    <Extension>
    Private Function ToCommaDelimited(s As Single) As String
        Return s.ToString.Replace(".", ",")
    End Function

    <Extension>
    Private Function ToPeriodDelimited(s As Single) As String
        Return s.ToString.Replace(",", ".")
    End Function

#End Region

#Region "Images"

    Friend ReadOnly s_insulinImage As Bitmap = My.Resources.InsulinVial_Tiny
    Friend ReadOnly s_mealImage As Bitmap = My.Resources.MealImage

#End Region

End Module
