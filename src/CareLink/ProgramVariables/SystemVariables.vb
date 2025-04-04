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
    Friend s_currentSummaryRow As Integer = 0
    Friend s_formLoaded As Boolean = False
    Friend s_useLocalTimeZone As Boolean
    Friend s_countryCode As String = ""
    Friend s_password As String = ""
    Friend s_userName As String = My.Settings.CareLinkUserName
    Friend Property CurrentUser As CurrentUserRecord
    Friend Property DecimalSeparator As String = "."

    Friend Property MaxBasalPerDose As Double

    Friend Property NativeMmolL As Boolean = False
    Friend Property TreatmentInsulinRow As Single

    Friend Function GetInsulinYValue() As Single
        Dim maxYScaled As Single = s_listOfSgRecords.Max(Of Single)(Function(sgR As SG) sgR.sg) + 2
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

    Friend Function GetTIR() As (Uint As UInteger, Str As String)
        Return If(s_timeInRange >= 0,
                  (CUInt(s_timeInRange), s_timeInRange.ToString),
                  (CUInt(0), "??? "))
    End Function

    Friend Function GetYMaxValue(asMmolL As Boolean) As Single
        Return If(asMmolL, ParseSingle(22.2, decimalDigits:=1), 400)
    End Function

    Friend Function GetYMinValue(asMmolL As Boolean) As Single
        Return If(asMmolL, ParseSingle(2.8, decimalDigits:=1), 50)
    End Function

    Friend Function TirHighLimit(asMmolL As Boolean) As Single
        Return If(asMmolL, 10, 180)
    End Function

    Friend Function TirHighLimitAsString(asMmolL As Boolean) As String
        Return If(asMmolL, "10", "180")
    End Function

    Friend Function TirLowLimit(asMmolL As Boolean) As Single
        Return If(asMmolL, ParseSingle(3.9, decimalDigits:=1), 70)
    End Function

    Friend Function TirLowLimitAsString(asMmolL As Boolean) As String
        Return If(asMmolL, "3.9", "70").Replace(".", Provider.NumberFormat.NumberDecimalSeparator)
    End Function

End Module
