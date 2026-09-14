' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Class PresetAmountRecord

    Public Sub New()
    End Sub

    Public Sub New(s As String)
        If s.Contains(value:="%"c) Then
            Me.TypeIsRate = False
            Me.PercentValue = Integer.Parse(s:=s.Trim(trimChar:="%"c).Trim)
        Else
            Me.TypeIsRate = True
            Dim sSplit As String() = s.Split(separator:=" ", Options)
            Me.RateValue = sSplit(0).ParseSingleInvariant
            Me.UnitsValue = sSplit(1)
        End If
    End Sub

    Private Shared ReadOnly Property Options As StringSplitOptions =
        StringSplitOptions.RemoveEmptyEntries

    Friend Property PercentValue As Single
    Friend Property RateValue As Single
    Friend Property TypeIsRate As Boolean
    Friend Property UnitsValue As String

    Public Overrides Function ToString() As String
        Return If(Me.TypeIsRate,
            $"Rate:  {vbTab}{Me.RateValue:F1} {Me.UnitsValue}",
            $"Percent:{vbTab}{Me.PercentValue}%")
    End Function

End Class
