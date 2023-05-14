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

    Friend Function GetInsulinYValue() As Single
        Dim maxYScaled As Single = s_listOfSGs.Max(Of Single)(Function(sgR As SgRecord) sgR.sg) + 2
        If ScalingNeeded Then
            If s_listOfSGs.Count = 0 OrElse maxYScaled > (330 / MmolLUnitsDivisor) Then
                Return 342 / MmolLUnitsDivisor
            End If
            Return Math.Max(maxYScaled, 260 / MmolLUnitsDivisor)
        Else
            If s_listOfSGs.Count = 0 Or maxYScaled > 330 Then
                Return 342
            End If
            Return Math.Max(maxYScaled, 260)
        End If
    End Function

    Friend Function GetYMaxValue() As Single
        Return If(ScalingNeeded, 22, 400)
    End Function

    Friend Function GetYMinValue() As Single
        Return If(ScalingNeeded, 2, 50)
    End Function

    Friend Function TirHighLimit(doScaling As Boolean) As Single
        Return If(doScaling, 10, 180)
    End Function

    Friend Function TirLowLimit(doScaling As Boolean) As Single
        Return If(doScaling, CSng(3.89), 70)
    End Function

End Module
