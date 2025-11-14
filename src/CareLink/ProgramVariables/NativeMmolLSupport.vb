' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Module NativeMmolLSupport

    ''' <summary>
    '''  Gets the string representation of the current blood glucose units.
    ''' </summary>
    ''' <returns>
    '''  "Mmol/l" if <see cref="NativeMmolL"/> is <see langword="True"/>;
    '''  otherwise returns "mg/dL".
    ''' </returns>
    Public ReadOnly Property BgUnits As String
        Get
            Return If(NativeMmolL, "Mmol/l", "mg/dL")
        End Get
    End Property

    Public Property NativeMmolL As Boolean = False

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
                     MgDlItems.Last.Value))
    End Function

    ''' <summary>
    '''  Gets the maximum Y value for plotting, based on <see cref="NativeMmolL"/> setting.
    ''' </summary>
    ''' <returns>
    '''  The maximum Y value for plotting.
    ''' </returns>
    Friend Function GetYMaxNativeMmolL() As Single
        Return If(NativeMmolL, MaxMmolL22_2, MaxMmDl400)
    End Function

    ''' <summary>
    '''  Gets the minimum Y value for plotting, based on <see cref="NativeMmolL"/> setting.
    ''' </summary>
    ''' <returns>
    '''  The minimum Y value for plotting in the selected units.
    ''' </returns>
    Friend Function GetYMinNativeMmolL() As Single
        Return If(NativeMmolL, MinMmolL2_8, MinMmDl50)
    End Function

    ''' <summary>
    '''  Gets the number of Precision Digits for String conversion based
    '''  on the current mmol/L setting.
    ''' </summary>
    ''' <returns>if MmolL 2 or 0 if mg/dL </returns>
    Public Function GetPrecisionDigits() As Integer
        Return If(NativeMmolL, 2, 0)
    End Function

    ''' <summary>
    '''  Gets the standard format string for blood glucose values
    '''  based on the current mmol/L setting.
    ''' </summary>
    ''' <param name="withSign">
    '''  If <see langword="True"/>, includes a sign (+ or -) in the format;
    '''  if <see langword="False"/>, no sign is included;
    '''  if <see langword="Nothing"/>, uses "F1" for mmol/L and "F0" for mg/dL.
    ''' </param>
    ''' <returns>
    '''  The standard format string for blood glucose values.
    ''' </returns>
    Public Function GetSgFormat(Optional withSign As Boolean? = Nothing) As String
        Return If(withSign Is Nothing,
                  If(NativeMmolL, "F1", "F0"),
                  If(withSign,
                     If(NativeMmolL, $"+0{DecimalSeparator}0;-#{DecimalSeparator}0", "+0;-#"),
                     If(NativeMmolL, $"0{DecimalSeparator}0", "0")))
    End Function

End Module
