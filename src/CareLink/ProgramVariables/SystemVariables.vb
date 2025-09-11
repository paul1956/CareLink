' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization

Public Module SystemVariables

#Region "Used for painting"

    Friend ReadOnly s_activeInsulinMarkers As New Dictionary(Of OADate, Single)
    Friend ReadOnly s_summaryMarkersInsulin As New Dictionary(Of OADate, Single)
    Friend ReadOnly s_summaryMarkersMeal As New Dictionary(Of OADate, Single)
    Friend ReadOnly s_treatmentMarkersInsulin As New Dictionary(Of OADate, Single)
    Friend ReadOnly s_treatmentMarkersMeal As New Dictionary(Of OADate, Single)

#End Region ' Used for painting

    Friend s_allUserSettingsData As New CareLinkUserDataList
    Friend s_countryCode As String = ""
    Friend s_currentSummaryRow As Integer = 0
    Friend s_formLoaded As Boolean = False
    Friend s_password As String = ""
    Friend s_useLocalTimeZone As Boolean
    Friend s_userName As String = My.Settings.CareLinkUserName
    Friend s_webView2CacheDirectory As String
    Friend ReadOnly Property CareLinkDecimalSeparator As Char = "."c
    Friend Property CurrentUser As CurrentUserRecord
    Friend Property DecimalSeparator As String = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator
    Friend Property MaxBasalPerDose As Double
    Friend Property TreatmentInsulinRow As Single

    ''' <summary>
    '''  Gets the above hyperglycemia limit as a tuple of unsigned integer and string.
    ''' </summary>
    ''' <returns>
    '''  A tuple containing the above hyper limit as an unsigned integer and
    '''  its string representation.
    ''' </returns>
    Friend Function GetAboveHyperLimit() As (int As Integer, Str As String)
        Dim aboveHyperLimit As Single =
            PatientData.AboveHyperLimit.RoundToSingle(digits:=1, considerValue:=True)
        Return If(aboveHyperLimit >= 0,
                  (CInt(aboveHyperLimit), aboveHyperLimit.ToString),
                  (0, "??? "))
    End Function

    ''' <summary>
    '''  Gets the below hypoglycemia limit as a tuple of unsigned integer and string.
    ''' </summary>
    ''' <returns>
    '''  A tuple containing the below hypo limit as an unsigned integer and
    '''  its string representation.
    ''' </returns>
    Friend Function GetBelowHypoLimit() As (Uint As UInteger, Str As String)
        Dim belowHyperLimit As Single =
            PatientData.BelowHypoLimit.RoundToSingle(digits:=1, considerValue:=True)
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

        Dim selector As Func(Of SG, Single) =
            Function(sgR As SG) As Single
                Return sgR.sg
            End Function
        Dim maxYScaled As Single = s_sgRecords.Max(Of Single)(selector) + 2
        If Single.IsNaN(maxYScaled) Then
            Return If(NativeMmolL,
                      mmoLInsulinYValue,
                      mmDlInsulinYValue)
        End If

        Dim noRecords As Boolean = s_sgRecords.Count = 0
        Dim mmolValue As Single = If(noRecords OrElse maxYScaled > mmoLInsulinYValue,
                                     342 / MmolLUnitsDivisor,
                                     Math.Max(maxYScaled, 260 / MmolLUnitsDivisor))
        Dim mgdlValue As Single = If(noRecords OrElse maxYScaled > mmDlInsulinYValue,
                                     342,
                                     Math.Max(maxYScaled, 260))
        Return If(NativeMmolL,
                  mmolValue,
                  mgdlValue)
    End Function

    ''' <summary>
    '''  Gets the directory path for the WebView2 cache.
    ''' </summary>
    ''' <returns>
    '''  The WebView2 cache directory path as a string.
    ''' </returns>
    Public Function GetWebViewDirectory() As String
        Return s_webView2CacheDirectory
    End Function

End Module
