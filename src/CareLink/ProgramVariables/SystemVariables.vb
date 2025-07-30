' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Linq

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
    Friend s_webView2CacheDirectory As String
    Friend Property CareLinkDecimalSeparator As Char = "."c
    Friend Property CurrentUser As CurrentUserRecord
    Friend Property MaxBasalPerDose As Double
    Friend Property NativeMmolL As Boolean = False


    ''' <summary>
    '''  Gets the character used to separate sentences, usually a period.
    ''' </summary>
    ''' <returns></returns>
    Friend Property SentenceSeparator As Char = "."c
    Friend Property TreatmentInsulinRow As Single

    ''' <summary>
    '''  Gets the above hyperglycemia limit as a tuple of unsigned integer and string.
    ''' </summary>
    ''' <returns>
    '''  A tuple containing the above hyper limit as an unsigned integer and its string representation.
    ''' </returns>
    Friend Function GetAboveHyperLimit() As (Uint As UInteger, Str As String)
        Dim aboveHyperLimit As Single = PatientData.AboveHyperLimit.GetRoundedValue(digits:=1)
        Return If(aboveHyperLimit >= 0,
                  (CUInt(aboveHyperLimit), aboveHyperLimit.ToString),
                  (CUInt(0), "??? "))
    End Function

    ''' <summary>
    '''  Gets the below hypoglycemia limit as a tuple of unsigned integer and string.
    ''' </summary>
    ''' <returns>
    '''  A tuple containing the below hypo limit as an unsigned integer and its string representation.
    ''' </returns>
    Friend Function GetBelowHypoLimit() As (Uint As UInteger, Str As String)
        Dim belowHyperLimit As Single = PatientData.BelowHypoLimit.GetRoundedValue(digits:=1)
        Return If(belowHyperLimit >= 0,
                  (CUInt(belowHyperLimit), belowHyperLimit.ToString),
                  (CUInt(0), "??? "))
    End Function

    ''' <summary>
    '''  Gets the Y value for insulin plotting based on current SG records and units.
    ''' </summary>
    ''' <returns>
    '''  The Y value for insulin plotting.
    ''' </returns>
    Friend Function GetInsulinYValue() As Single
        Const mmDlInsulinYValue As Integer = 330
        Const mmoLInsulinYValue As Single = mmDlInsulinYValue / MmolLUnitsDivisor
        Dim maxYScaled As Single = s_sgRecords.Max(Of Single)(selector:=Function(sgR As SG)
                                                                            Return sgR.sg
                                                                        End Function) + 2
        Return If(Single.IsNaN(maxYScaled),
                  If(NativeMmolL, mmoLInsulinYValue, mmDlInsulinYValue),
                  If(NativeMmolL,
                     If(s_sgRecords.Count = 0 OrElse maxYScaled > mmoLInsulinYValue,
                        342 / MmolLUnitsDivisor,
                        Math.Max(maxYScaled, 260 / MmolLUnitsDivisor)),
                     If(s_sgRecords.Count = 0 OrElse maxYScaled > mmDlInsulinYValue,
                        342,
                        Math.Max(maxYScaled, 260))))
    End Function

    ''' <summary>
    '''  Gets the format string for SG values, optionally with sign.
    ''' </summary>
    ''' <param name="withSign">Whether to include a sign in the format.</param>
    ''' <returns>
    '''  The format string for SG values.
    ''' </returns>
    Friend Function GetSgFormat(withSign As Boolean) As String
        Return If(withSign,
            If(NativeMmolL, $"+0{Provider.NumberFormat.NumberDecimalSeparator}0;-#{Provider.NumberFormat.NumberDecimalSeparator}0", "+0;-#"),
            If(NativeMmolL, $"0{Provider.NumberFormat.NumberDecimalSeparator}0", "0"))
    End Function

    ''' <summary>
    '''  Gets the current SG target value.
    ''' </summary>
    ''' <returns>
    '''  The current SG target value.
    ''' </returns>
    Friend Function GetSgTarget() As Single
        Return If(CurrentUser.CurrentTarget <> 0,
                  CurrentUser.CurrentTarget,
                  If(NativeMmolL,
                     MmolLItemsPeriod.Last.Value,
                     MgDlItems.Last.Value)
                    )
    End Function

    ''' <summary>
    '''  Counts the number of SG.sg values in the specified range [OptionsConfigureTiTR.LowThreshold, 140], excluding Single.NaN.
    ''' </summary>
    ''' <param name="sgList">The list of SG records to evaluate.</param>
    ''' <returns>
    '''  The count of SG.sg values within the range and not NaN.
    ''' </returns>
    Friend Function CountSgInTightRange(sgList As IEnumerable(Of SG)) As UInteger
        Dim predicate As Func(Of SG, Boolean) = Function(sg)
                                                    Return Not Single.IsNaN(sg.sg) AndAlso
                                                           sg.sgMgdL >= OptionsConfigureTiTR.LowThreshold AndAlso
                                                           sg.sgMgdL <= 140.0
                                                End Function

        Return CUInt(sgList.Count(predicate))
    End Function

    ''' <summary>
    '''  Counts the number of valid SG records in the specified list.
    '''  A valid SG record is one where sg is not NaN and sgMgdL is not zero.
    ''' </summary>
    ''' <param name="sgList">The list of SG records to evaluate.</param>
    ''' <returns>
    '''  The count of valid SG records.
    ''' </returns>
    Friend Function CountValidSg(sgList As IEnumerable(Of SG)) As UInteger
        Return CUInt(sgList.Count(
            predicate:=Function(sg)
                           Return Not Single.IsNaN(sg.sg) AndAlso sg.sgMgdL <> 0.0
                       End Function))
    End Function

    ''' <summary>
    '''  Gets the Time In Range (TIR) as a tuple of unsigned integer and string.
    ''' </summary>
    ''' <param name="tight">
    '''  Optional. If <see langword="True"/>, calculates TIR based on tight range; otherwise, uses the standard TIR.
    ''' </param>
    ''' <returns>
    '''  A <see cref="tuple"/> containing the TIR as an unsigned integer and its string representation.
    ''' </returns>
    Friend Function GetTIR(Optional tight As Boolean = False) As (percent As UInteger, asString As String)
        If tight Then
            If s_sgRecords Is Nothing Then
                Return (0, "  ???")
            End If

            Dim validSgCount As UInteger = CountValidSg(s_sgRecords)
            If validSgCount = 0 Then
                Return (0, "  ???")
            End If
            Dim inTightRangeCount As UInteger = CountSgInTightRange(s_sgRecords)
            Dim percentInTightRange As UInteger = CUInt(inTightRangeCount / validSgCount * 100)
            Return (percentInTightRange, percentInTightRange.ToString())
        End If

        Dim timeInRange As Integer = PatientData.TimeInRange
        Return If(timeInRange > 0,
                  (CUInt(timeInRange), timeInRange.ToString()),
                  (CUInt(0), "  ???"))
    End Function

    ''' <summary>
    '''  Gets the high limit for Time In Range (TIR), based on <see cref="NativeMmolL"/>,
    '''  optionally user can control units by specifying <paramref name="asMmolL"/>
    ''' </summary>
    ''' <param name="asMmolL">
    '''  Optional. If <see langword="True"/>, returns value as mmol/L; otherwise, as mg/dL.
    '''  If not specified, uses <see cref="NativeMmolL"/>.
    ''' </param>
    ''' <returns>
    '''  The high limit for TIR.
    ''' </returns>
    Friend Function GetTirHighLimit(Optional asMmolL As Boolean = Nothing) As Single
        If asMmolL = Nothing Then
            asMmolL = NativeMmolL
        End If
        Return If(asMmolL, TirHighMmol10, TirHighMmDl180)
    End Function

    ''' <summary>
    '''  Gets the high limit for TIR with units as a formatted string.
    ''' </summary>
    ''' <returns>
    '''  The high limit for TIR in the selected units.
    ''' </returns>
    Friend Function GetTirHighLimitWithUnits() As String
        Return $"{GetTirHighLimit()} {GetBgUnitsString()}".
            Replace(CareLinkDecimalSeparator, Provider.NumberFormat.NumberDecimalSeparator)
    End Function

    ''' <summary>
    '''  Gets the low limit for Time In Range (TIR), optionally as mmol/L.
    ''' </summary>
    ''' <param name="asMmolL">
    '''  Optional. If <see langword="True"/>, returns value as mmol/L; otherwise, as mg/dL.
    '''  If not specified, uses <see cref="NativeMmolL"/>.
    ''' </param>
    ''' <returns>
    '''  The low limit for TIR.
    ''' </returns>
    Friend Function GetTirLowLimit(Optional asMmolL As Boolean = Nothing) As Single
        If asMmolL = Nothing Then
            asMmolL = NativeMmolL
        End If
        Return If(asMmolL, TirLowMmDl3_9, TirLowMmol70)
    End Function

    ''' <summary>
    '''  Gets the low limit for TIR with units as a formatted string.
    ''' </summary>
    ''' <returns>
    '''  The low limit for TIR with units.
    ''' </returns>
    Friend Function GetTirLowLimitWithUnits() As String
        Return $"{GetTirLowLimit()} {GetBgUnitsString()}".
            Replace(CareLinkDecimalSeparator, Provider.NumberFormat.NumberDecimalSeparator)
    End Function

    ''' <summary>
    '''  Gets the maximum Y value for plotting, based on <see cref="NativeMmolL"/> setting.
    ''' </summary>
    ''' <returns>
    '''  The maximum Y value for plotting.
    ''' </returns>
    Friend Function GetYMaxValueFromNativeMmolL() As Single
        Return If(NativeMmolL, MaxMmolL22_2, MaxMmDl400)
    End Function

    ''' <summary>
    '''  Gets the minimum Y value for plotting, based on <see cref="NativeMmolL"/> setting.
    ''' </summary>
    ''' <returns>
    '''  The minimum Y value for plotting in the selected units.
    ''' </returns>
    Friend Function GetYMinValueFromNativeMmolL() As Single
        Return If(NativeMmolL, MinMmolL2_8, MinMmDl50)
    End Function

    ''' <summary>
    '''  Gets the directory path for the WebView2 cache.
    ''' </summary>
    ''' <returns>
    '''  The WebView2 cache directory path as a string.
    ''' </returns>
    Public Function GetWebViewCacheDirectory() As String
        Return s_webView2CacheDirectory
    End Function

End Module
