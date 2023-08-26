' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class RemindersRecord

    Public Sub New()
    End Sub

    Public Sub New(lines As List(Of String))
        Me.LowReserviorWarning = lines.GetSingleLineValue(Of String)("Low Reservoir Warning ")
        Me.Amount = lines.GetSingleLineValue(Of Single)("Amount ")
        Me.BolusBgCheck = lines.GetSingleLineValue(Of String)("Bolus BG Check ")
        Me.SetChange = lines.GetSingleLineValue(Of String)("Set Change ")
    End Sub

    Public Property LowReserviorWarning As String
    Public Property Amount As Single
    Public Property BolusBgCheck As String
    Public Property SetChange As String

    Public Property MissedMealBolus As New Dictionary(Of String, MealStartEndRecord) From {
                {"Meal 1", New MealStartEndRecord()},
                {"Meal 2", New MealStartEndRecord()},
                {"Meal 3", New MealStartEndRecord()},
                {"Meal 4", New MealStartEndRecord()},
                {"Meal 5", New MealStartEndRecord()},
                {"Meal 6", New MealStartEndRecord()},
                {"Meal 7", New MealStartEndRecord()},
                {"Meal 8", New MealStartEndRecord()}
            }

    Public Property PersonalReminders As New Dictionary(Of String, PersonalRemindersRecord) From {
                {"Reminder 1", New PersonalRemindersRecord()},
                {"Reminder 2", New PersonalRemindersRecord()},
                {"Reminder 3", New PersonalRemindersRecord()},
                {"Reminder 4", New PersonalRemindersRecord()},
                {"Reminder 5", New PersonalRemindersRecord()},
                {"Reminder 6", New PersonalRemindersRecord()},
                {"BG Check", New PersonalRemindersRecord()},
                {"Medication", New PersonalRemindersRecord()}
            }

End Class
