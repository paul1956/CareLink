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

    Friend Property MaxBasalPerDose As Single

    Friend ReadOnly Property MgDlItems As New Dictionary(Of String, Single) From {
                            {$"100 mg/dL", 100.0},
                            {$"110 mg/dL", 110.0},
                            {$"120 mg/dL", 120.0}
                        }

    Friend ReadOnly Property MmolLItems As New Dictionary(Of String, Single) From {
                            {$"5.6 mmol/L", 5.6},
                            {$"6.1 mmol/L", 6.1},
                            {$"6.7 mmol/L", 6.7}
                        }

    Friend Property nativeMmolL As Boolean = False

    Friend Property TreatmentInsulinRow As Single

    Friend Function GetInsulinYValue() As Single
        Dim maxYScaled As Single = s_listOfSgRecords.Max(Of Single)(Function(sgR As SgRecord) sgR.sg) + 2
        Return If(Single.IsNaN(maxYScaled),
            If(nativeMmolL, 330 / MmolLUnitsDivisor, 330),
            If(nativeMmolL,
                If(s_listOfSgRecords.Count = 0 OrElse maxYScaled > (330 / MmolLUnitsDivisor),
                    342 / MmolLUnitsDivisor,
                    Math.Max(maxYScaled, 260 / MmolLUnitsDivisor)),
                If(s_listOfSgRecords.Count = 0 OrElse maxYScaled > 330, 342, Math.Max(maxYScaled, 260))))
    End Function

    Friend Function GetSgTarget() As Single
        Return If(CurrentUser.CurrentTarget <> 0,
                  CurrentUser.CurrentTarget,
                  If(nativeMmolL,
                     MmolLItems.Last.Value,
                     MgDlItems.Last.Value)
                    )
    End Function

    Friend Function GetTIR() As UInteger
        Return If(s_timeInRange > 0,
                  CUInt(s_timeInRange),
                  CUInt(100 - (s_aboveHyperLimit + s_belowHypoLimit))
                 )
    End Function

    Friend Function GetYMaxValue(asMmolL As Boolean) As Single
        Return If(asMmolL, CSng(22.2), 400)
    End Function

    Friend Function GetYMinValue(asMmolL As Boolean) As Single
        Return If(asMmolL, CSng(2.8), 50)
    End Function

    Friend Function TirHighLimit(asMmolL As Boolean) As Single
        Return If(asMmolL, 10, 180)
    End Function

    Friend Function TirLowLimit(asMmolL As Boolean) As Single
        Return If(asMmolL, CSng(3.9), 70)
    End Function

End Module
