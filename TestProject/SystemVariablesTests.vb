' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports CareLink
Imports FluentAssertions
Imports Xunit
Imports System.Globalization

Public Class SystemVariablesTests
    <Fact>
    Public Sub GetWebViewCacheDirectory_ReturnsSetValue()
        ' Arrange
        Dim expected As String = "C:\\TestCache"
        s_webView2CacheDirectory = expected
        ' Act
        Dim result As String = GetWebViewCacheDirectory()
        ' Assert
        result.Should().Be(expected)
    End Sub

    <Fact>
    Public Sub GetTirHighLimit_ReturnsCorrectValue_ForUnits()
        NativeMmolL = True
        Dim highLimitMmol As Single = GetTirHighLimit()
        highLimitMmol.Should().Be(expected:=TirHighMmol10)
        NativeMmolL = False
        Dim highLimitMgdl As Single = GetTirHighLimit()
        highLimitMgdl.Should().Be(expected:=TirHighMmDl180)
        Dim convertedHighLimit As Single = highLimitMmol * 18.0F
        convertedHighLimit.Should().BeApproximately(expectedValue:=highLimitMgdl, precision:=0.01F)
    End Sub

    <Fact>
    Public Sub GetTirLowLimit_ReturnsCorrectValue_ForUnits()
        NativeMmolL = True
        Dim lowLimitMmol As Single = GetTirLowLimit()
        lowLimitMmol.Should().Be(expected:=TirLowMmDl3_9)
        NativeMmolL = False
        Dim lowLimitMgdl As Single = GetTirLowLimit()
        lowLimitMgdl.Should().Be(expected:=TirLowMmol70)
        Dim convertedLowLimit As Single = lowLimitMmol * 18.0F
        convertedLowLimit.Should().BeApproximately(expectedValue:=lowLimitMgdl, precision:=0.5F)
    End Sub

    <Fact>
    Public Sub GetTirHighLimitWithUnits_UsesDecimalSeparator()
        NativeMmolL = True
        Dim expected As String = "10 Mmol/l"
        GetTirHighLimitWithUnits().Should().Be(expected)
        NativeMmolL = False
    End Sub

    <Fact>
    Public Sub GetTirLowLimitWithUnits_UsesDecimalSeparator()
        NativeMmolL = True
        DecimalSeparator = ","
        Dim expected As String = "3,9 Mmol/l"
        GetTirLowLimitWithUnits().Should().Be(expected)
        RestoreDefaults()
    End Sub

End Class
