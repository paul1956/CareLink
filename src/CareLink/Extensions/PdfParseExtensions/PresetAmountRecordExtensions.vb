' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module PresetAmountRecordExtensions

    Private ReadOnly Property Options As StringSplitOptions =
        StringSplitOptions.RemoveEmptyEntries

    <Extension>
    Public Sub InitializeFromString(this As PresetAmountRecord, s As String)
        If s Is Nothing Then Return
        Try
            If s.Contains(value:="%"c) Then
                ' percent type
                Dim percentVal As Integer = Integer.Parse(s:=s.Trim(trimChar:="%"c).Trim)
                ' use reflection to set readonly backing fields if necessary
                this.TypeIsRate = False
                this.PercentValue = percentVal
            Else
                Dim sSplit As String() = s.Split(separator:=" "c, Options)
                Dim rateVal As Single = sSplit(0).ParseSingleInvariant
                Dim unitsVal As String = sSplit(1)
                this.TypeIsRate = True
                this.RateValue = rateVal
                this.UnitsValue = unitsVal
            End If
        Catch
        End Try
    End Sub

End Module
