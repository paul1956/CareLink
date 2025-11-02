' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module TimeInRangeHelpers

    ''' <summary>
    '''  Gets the Time In Range (TIR) as a tuple of unsigned integer and string.
    ''' </summary>
    ''' <param name="tight">
    '''  Optional. If <see langword="True"/>, calculates TIR based on tight range;
    '''  otherwise, uses the standard TIR.
    ''' </param>
    ''' <returns>
    '''  A <see cref="tuple"/> containing the TIR as an unsigned integer
    '''  and its string representation.
    ''' </returns>
    Friend Function GetTIR(Optional tight As Boolean = False) As (Percent As UInteger, AsString As String)
        If tight Then
            If s_sgRecords Is Nothing Then
                Return (0, "  ???")
            End If

            Dim validSgCount As Integer = GetValidSgRecords().Count()
            If validSgCount = 0 Then
                Return (0, "  ???")
            End If
            Dim inTightRangeCount As Integer = s_sgRecords.CountSgInTightRange()
            Dim percentInTightRange As UInteger =
                CUInt(inTightRangeCount / validSgCount * 100)
            Return (percentInTightRange, percentInTightRange.ToString())
        End If

        Dim timeInRange As Integer = PatientData.TimeInRange
        Return If(timeInRange > 0,
                  (CUInt(timeInRange), timeInRange.ToString()),
                  (0UI, "  ???"))
    End Function

    ''' <summary>
    '''  Gets the high limit for Time In Range (TIR), based on <see cref="NativeMmolL"/>,
    '''  optionally user can control units by specifying <paramref name="asMmolL"/>
    ''' </summary>
    ''' <param name="asMmolL">
    '''  Optional. If <see langword="True"/>, returns value as mmol/L;
    '''  otherwise, as mg/dL.
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
        Return $"{GetTirHighLimit()} {BgUnits}".Replace(oldValue:=CareLinkDecimalSeparator, newValue:=DecimalSeparator)
    End Function

    ''' <summary>
    '''  Gets the low limit for Time In Range (TIR), optionally as mmol/L.
    ''' </summary>
    ''' <param name="asMmolL">
    '''  Optional. If <see langword="True"/>, returns value as mmol/L;
    '''  otherwise, as mg/dL.
    '''  If not specified, uses <see cref="NativeMmolL"/>.
    ''' </param>
    ''' <returns>
    '''  The low limit for TIR.
    ''' </returns>
    Friend Function GetTirLowLimit(Optional asMmolL As Boolean = Nothing) As Single
        If asMmolL = Nothing Then
            asMmolL = NativeMmolL
        End If
        Return If(asMmolL,
                  TirLowMmDl3_9,
                  TirLowMmol70)
    End Function

    ''' <summary>
    '''  Gets the low limit for TIR with units as a formatted string.
    ''' </summary>
    ''' <returns>
    '''  The low limit for TIR with units.
    ''' </returns>
    Friend Function GetTirLowLimitWithUnits() As String
        Return $"{GetTirLowLimit()} {BgUnits}".Replace(oldValue:=CareLinkDecimalSeparator, newValue:=DecimalSeparator)
    End Function

End Module
