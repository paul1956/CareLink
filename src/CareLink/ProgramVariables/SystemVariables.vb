' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module SystemVariables

#Region "Used for painting"

    Friend ReadOnly s_activeInsulinMarkerInsulinDictionary As New Dictionary(Of OADate, Single)
    Friend ReadOnly s_summaryMarkerInsulinDictionary As New Dictionary(Of OADate, Single)
    Friend ReadOnly s_summaryMarkerMealDictionary As New Dictionary(Of OADate, Single)
    Friend ReadOnly s_treatmentMarkerInsulinDictionary As New Dictionary(Of OADate, Single)
    Friend ReadOnly s_treatmentMarkerMealDictionary As New Dictionary(Of OADate, Single)

#End Region ' Used for painting

    Friend s_allUserSettingsData As New CareLinkUserDataList
    Friend s_countryCode As String = ""
    Friend s_currentSummaryRow As Integer = 0
    Friend s_formLoaded As Boolean = False
    Friend s_password As String = ""
    Friend s_useLocalTimeZone As Boolean
    Friend s_userName As String = My.Settings.CareLinkUserName
    Friend s_webViewCacheDirectory As String
    Friend Property CareLinkDecimalSeparator As Char = "."c
    Friend Property CurrentUser As CurrentUserRecord
    Friend Property MaxBasalPerDose As Double
    Friend Property NativeMmolL As Boolean = False
    Friend Property SentenceSeparator As Char = "."c
    Friend Property TreatmentInsulinRow As Single

    Friend Function GetAboveHyperLimit() As (Uint As UInteger, Str As String)
        Dim aboveHyperLimit As Single = PatientData.AboveHyperLimit.GetRoundedValue(decimalDigits:=1)
        Return If(aboveHyperLimit >= 0,
                  (CUInt(aboveHyperLimit), aboveHyperLimit.ToString),
                  (CUInt(0), "??? "))
    End Function

    Friend Function GetBelowHypoLimit() As (Uint As UInteger, Str As String)
        Dim belowHyperLimit As Single = PatientData.BelowHypoLimit.GetRoundedValue(decimalDigits:=1)
        Return If(belowHyperLimit >= 0,
                  (CUInt(belowHyperLimit), belowHyperLimit.ToString),
                  (CUInt(0), "??? "))
    End Function

    Friend Function GetInsulinYValue() As Single
        Dim maxYScaled As Single = s_listOfSgRecords.Max(Of Single)(Function(sgR As SG) sgR.sg) + 2
        Const mmDlInsulinYValue As Integer = 330
        Const mmoLInsulinYValue As Single = mmDlInsulinYValue / MmolLUnitsDivisor
        Return If(Single.IsNaN(maxYScaled),
            If(NativeMmolL, mmoLInsulinYValue, mmDlInsulinYValue),
            If(NativeMmolL,
                If(s_listOfSgRecords.Count = 0 OrElse maxYScaled > mmoLInsulinYValue,
                    342 / MmolLUnitsDivisor,
                    Math.Max(maxYScaled, 260 / MmolLUnitsDivisor)),
                If(s_listOfSgRecords.Count = 0 OrElse maxYScaled > mmDlInsulinYValue, 342, Math.Max(maxYScaled, 260))))
    End Function

    Friend Function GetSgFormat(withSign As Boolean) As String
        Return If(withSign,
            If(NativeMmolL, $"+0{Provider.NumberFormat.NumberDecimalSeparator}0;-#{Provider.NumberFormat.NumberDecimalSeparator}0", "+0;-#"),
            If(NativeMmolL, $"0{Provider.NumberFormat.NumberDecimalSeparator}0", "0"))
    End Function

    Friend Function GetSgTarget() As Single
        Return If(CurrentUser.CurrentTarget <> 0,
                  CurrentUser.CurrentTarget,
                  If(NativeMmolL,
                     MmolLItemsPeriod.Last.Value,
                     MgDlItems.Last.Value)
                    )
    End Function

    Friend Function GetTIR() As (Uint As UInteger, Str As String)
        Dim timeInRange As Integer = PatientData.TimeInRange
        Return If(timeInRange > 0,
                  (CUInt(timeInRange), timeInRange.ToString),
                  (CUInt(0), "??? "))
    End Function

    Friend Function GetTirHighLimit(Optional asMmolL As Boolean = Nothing) As Single
        If asMmolL = Nothing Then
            asMmolL = NativeMmolL
        End If
        Return If(asMmolL, TirHighMmol10, TirHighMmDl180)
    End Function

    Friend Function GetTirHighLimitWithUnits() As String
        Return $"{GetTirHighLimit()} {GetBgUnitsString()}".
            Replace(CareLinkDecimalSeparator, Provider.NumberFormat.NumberDecimalSeparator)
    End Function

    Friend Function GetTirLowLimit(Optional asMmolL As Boolean = Nothing) As Single
        If asMmolL = Nothing Then
            asMmolL = NativeMmolL
        End If
        Return If(asMmolL, TirLowMmDl3_9, TirLowMmol70)
    End Function

    Friend Function GetTirLowLimitWithUnits() As String
        Return $"{GetTirLowLimit()} {GetBgUnitsString()}".
            Replace(CareLinkDecimalSeparator, Provider.NumberFormat.NumberDecimalSeparator)
    End Function

    Friend Function GetYMaxValueFromNativeMmolL() As Single
        Return If(NativeMmolL, MaxMmolL22_2, MaxMmDl400)
    End Function

    Friend Function GetYMinValueFromNativeMmolL() As Single
        Return If(NativeMmolL, MinMmolL2_8, MinMmDl50)
    End Function

    Public Function GetWebViewCacheDirectory() As String
        Return s_webViewCacheDirectory
    End Function

End Module
