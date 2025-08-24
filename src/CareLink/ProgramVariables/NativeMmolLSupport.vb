' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Module NativeMmolLSupport
    Public Property NativeMmolL As Boolean = False

    ''' <summary>
    '''  Gets the string representation of the current blood glucose units.
    ''' </summary>
    ''' <returns>
    '''  Returns "Mmol/l" if NativeMmolL is True, otherwise returns "mg/dL".
    ''' </returns>
    Public ReadOnly Property BgUnits As String
        Get
            Return If(NativeMmolL, "Mmol/l", "mg/dL")
        End Get
    End Property

    ''' <summary>
    '''  Gets the number of Precision Digits for String conversion based
    '''  on the current mmol/L setting.
    ''' </summary>
    ''' <returns>if MmolL 2 or 0 if mg/dL </returns>
    Public Function GetPrecisionDigits() As Integer
        Return If(NativeMmolL, 2, 0)
    End Function

    ''' <summary>
    '''  Gets the Format for String conversion based on the current mmol/L setting.
    ''' </summary>
    ''' <returns>if MmolL "F1" or "F0" if mg/dL </returns>
    Public Function GetSgFormat() As String
        Return If(NativeMmolL, "F1", "F0")
    End Function

    ''' <summary>
    '''  Gets the format string for SG values, optionally with sign.
    ''' </summary>
    ''' <param name="withSign">Whether to include a sign in the format.</param>
    ''' <returns>
    '''  The format string for SG values.
    ''' </returns>
    ''' <remarks>
    '''  The format string is based on the current UI culture's decimal separator
    '''  and whether the values are in mmol/L or mg/dL.
    ''' </remarks>
    Public Function GetSgFormat(withSign As Boolean) As String
        Return If(withSign,
                  If(NativeMmolL,
                     $"+0{DecimalSeparator}0;-#{DecimalSeparator}0",
                     "+0;-#"),
                  If(NativeMmolL,
                     $"0{DecimalSeparator}0",
                     "0"))
    End Function

End Module
