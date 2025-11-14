' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Friend Module TirLimitHelpers
    ''' <summary>
    '''  Gets the above hyperglycemia limit as a tuple of unsigned integer and string.
    ''' </summary>
    ''' <returns>
    '''  A tuple containing the above hyper limit as an unsigned integer and
    '''  its string representation.
    ''' </returns>
    Friend Function GetAboveHyperLimit() As (int As Integer, Str As String)
        Dim aboveHyperLimit As Single = PatientData.AboveHyperLimit.RoundToSingle(digits:=1, considerValue:=True)
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
        Dim belowHyperLimit As Single = PatientData.BelowHypoLimit.RoundToSingle(digits:=1, considerValue:=True)
        Return If(belowHyperLimit >= 0,
                  (CUInt(belowHyperLimit), belowHyperLimit.ToString),
                  (CUInt(0), "??? "))
    End Function

End Module
