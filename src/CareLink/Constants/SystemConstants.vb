' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Text.Json

Public Module SystemConstants

    Friend Const ClickToShowDetails As String = "Click To Show Details"
    Friend Const ExceptionStartingString As String = "--- Start of Exception ---"
    Friend Const ExceptionTerminatingString As String = "--- End of Exception ---"
    Friend Const GitOwnerName As String = "Paul1956"
    Friend Const MmolLUnitsDivisor As Single = 18
    Friend Const ProjectName As String = "CareLink"
    Friend Const RegisteredTrademark As String = ChrW(&HAE)
    Friend Const StackTraceStartingStr As String = "--- Start of stack trace ---"
    Friend Const StackTraceTerminatingStr As String = "--- End of stack trace from previous location ---"
    Friend Const TimeFormatMilitaryWithMinutes As String = "HH:mm"
    Friend Const TimeFormatMilitaryWithoutMinutes As String = "HH"
    Friend Const TimeFormatTwelveHourWithMinutes As String = "h:mm tt"
    Friend Const TimeFormatTwelveHourWithoutMinutes As String = "h tt"

    Friend ReadOnly s_insulinTypes As New Dictionary(Of String, InsulinActivationProperties) From {
                        {$"Humalog{RegisteredTrademark}", New InsulinActivationProperties(8, 4)},
                        {$"Novolog{RegisteredTrademark}", New InsulinActivationProperties(8, 4)},
                        {$"Generic (Insulin Lispro)", New InsulinActivationProperties(9, 4)},
                        {$"NovoRapid", New InsulinActivationProperties(7, 4)},
                        {$"FIASP{RegisteredTrademark}", New InsulinActivationProperties(4, 3)},
                        {$"Lyumjev{RegisteredTrademark}", New InsulinActivationProperties(3, 3)}
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

    Public ReadOnly s_oneToNineteen As New List(Of String) From {
                        "zero", "one", "two", "three", "four", "five",
                        "six", "seven", "eight", "nine", "ten", "eleven",
                        "twelve", "thirteen", "fourteen", "fifteen",
                        "sixteen", "seventeen", "eighteen", "nineteen"
                    }

    Public ReadOnly s_unitsStrings As New Dictionary(Of String, String) From {
                        {"MG_DL", "mg/dl"},
                        {"MGDL", "mg/dl"},
                        {"MMOL_L", "mmol/L"},
                        {"MMOLL", "mmol/L"}
                    }

    Public ReadOnly Trends As New Dictionary(Of String, String) From {
                        {"DOWN", "↓"},
                        {"DOWN_DOUBLE", "↓↓"},
                        {"DOWN_TRIPLE", "↓↓↓"},
                        {"UP", "↑"},
                        {"UP_DOUBLE", "↑↑"},
                        {"UP_TRIPLE", "↑↑↑"},
                        {"NONE", "↔"}
                    }

    Public Enum FileToLoadOptions As Integer
        LastSaved = 0
        TestData = 1
        Login = 2
    End Enum

    Public ReadOnly Property GitHubCareLinkUrl As String = $"https://GitHub.com/{GitOwnerName}/{ProjectName}/"
    Public ReadOnly Property JsonFormattingOptions As New JsonSerializerOptions With {.WriteIndented = True}
    Public ReadOnly Property SavedTitle As String = $"{ProjectName}™ For Windows"

End Module
