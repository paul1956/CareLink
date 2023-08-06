' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO

Friend Module SystemVariables
    Friend s_allUserSettingsData As New CareLinkUserDataList
    Friend s_currentSummaryRow As Integer = 0
    Friend s_formLoaded As Boolean = False
    Friend s_useLocalTimeZone As Boolean
    Friend s_userName As String = ""
    Friend Property CurrentUser As CurrentUserRecord
    Friend Property DecimalSeparator As String = "."

    Friend Property MaxBasalPerDose As Single

    Friend ReadOnly Property MgDlItems As New Dictionary(Of String, Single) From {
                            {$"100 mg/dL", 100.0},
                            {$"110 mg/dL", 110.0},
                            {$"120 mg/dL", 120.0}
                        }

    Friend ReadOnly Property MmolLItemsComma As New Dictionary(Of String, Single) From {
                            {$"5,6 mmol/L", 5.6},
                            {$"6,1 mmol/L", 6.1},
                            {$"6,7 mmol/L", 6.7}
                        }

    Friend ReadOnly Property MmolLItemsPeriod As New Dictionary(Of String, Single) From {
                           {$"5.6 mmol/L", 5.6},
                           {$"6.1 mmol/L", 6.1},
                           {$"6.7 mmol/L", 6.7}
                        }

    Friend Property NativeMmolL As Boolean = False
    Friend Property TreatmentInsulinRow As Single

    Friend Function GetInsulinYValue() As Single
        Dim maxYScaled As Single = s_listOfSgRecords.Max(Of Single)(Function(sgR As SgRecord) sgR.sg) + 2
        Return If(Single.IsNaN(maxYScaled),
            If(NativeMmolL, 330 / MmolLUnitsDivisor, 330),
            If(NativeMmolL,
                If(s_listOfSgRecords.Count = 0 OrElse maxYScaled > (330 / MmolLUnitsDivisor),
                    342 / MmolLUnitsDivisor,
                    Math.Max(maxYScaled, 260 / MmolLUnitsDivisor)),
                If(s_listOfSgRecords.Count = 0 OrElse maxYScaled > 330, 342, Math.Max(maxYScaled, 260))))
    End Function

    Friend Function GetSgFormat(withSign As Boolean) As String
        Return If(withSign,
            If(NativeMmolL, $"+0{CurrentUICulture.NumberFormat.NumberDecimalSeparator}0;-#{CurrentUICulture.NumberFormat.NumberDecimalSeparator}0", "+0;-#"),
            If(NativeMmolL, $"0{CurrentUICulture.NumberFormat.NumberDecimalSeparator}0", "0"))
    End Function

    Friend Function GetSgTarget() As Single
        Return If(CurrentUser.CurrentTarget <> 0,
                  CurrentUser.CurrentTarget,
                  If(NativeMmolL,
                     MmolLItemsPeriod.Last.Value,
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

    Friend Function TirHighLimitAsString(asMmolL As Boolean) As String
        Return If(asMmolL, "10", "180")
    End Function

    Friend Function TirLowLimit(asMmolL As Boolean) As Single
        Return If(asMmolL, CSng(3.9), 70)
    End Function

    Friend Function TirLowLimitAsString(asMmolL As Boolean) As String
        Return If(asMmolL, "3.9", "70").Replace(".", CurrentUICulture.NumberFormat.NumberDecimalSeparator)
    End Function

End Module
