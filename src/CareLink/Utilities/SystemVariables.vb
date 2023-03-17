' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module SystemVariables
    Friend s_useLocalTimeZone As Boolean
    Public s_allUserSettingsData As New CareLinkUserDataList
    Friend Property CurrentUser As CurrentUserRecord

    Friend Property GraphColorDictionary As New Dictionary(Of String, KnownColor) From {
                        {"Active Insulin", KnownColor.Lime},
                        {"Auto Correction", KnownColor.Aqua},
                        {"Basal Series", KnownColor.HotPink},
                        {"High Limit", KnownColor.Yellow},
                        {"Low Limit", KnownColor.Red},
                        {"Min Basal", KnownColor.LightYellow},
                        {"SG Series", KnownColor.White},
                        {"SG Target", KnownColor.Green},
                        {"Time Change", KnownColor.White}
                    }

    Friend Property LastServerUpdateTime As Date

    Friend Property MaxBasalPerDose As Single

    Friend Property MaxBasalPerHour As Single

    Friend Property ScalingNeeded As Boolean = False

    Friend Property TreatmentInsulinRow As Single

    Private Function ScaleValue(value As Integer) As Single
        Return If(ScalingNeeded, CSng(Math.Round(value / MmolLUnitsDivisor, 2, MidpointRounding.ToZero)), value)
    End Function

    Friend Function GetInsulinYValue() As Single
        Return ScaleValue(342)
    End Function

    Friend Function GetYMaxValue() As Single
        Return ScaleValue(400)
    End Function

    Friend Function GetYMinValue() As Single
        Return ScaleValue(50)
    End Function

    Friend Function TirHighLimit() As Single
        Return ScaleValue(180)
    End Function

    Friend Function TirLowLimit() As Single
        Return ScaleValue(70)
    End Function

End Module
