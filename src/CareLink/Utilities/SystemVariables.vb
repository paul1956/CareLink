' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module SystemVariables
    Public ReadOnly s_settingsCsvFile As String = IO.Path.Combine(MyDocumentsPath, SavedCsvFileName)
    Public s_allUserSettingsData As New CareLinkUserDataList
    Friend Property HomePageBasalRow As Single = 400
    Friend Property HomePageInsulinRow As Single = 342
    Friend Property HomePageMealRow As Single = 50
    Friend Property MaxBasalPerDose As Single
    Friend Property MaxBasalPerHour As Single
    Friend Property scalingNeeded As Boolean = Nothing
    Friend Property LastServerUpdateTime As Date
    Friend Property TreatmentInsulinRow As Single

End Module
