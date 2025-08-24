' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports CareLink
Imports FluentAssertions
Imports Xunit
Imports System.Globalization

<Collection("Sequential")>
<UISettings(MaxAttempts:=10)>
Public Class NativeMmolLSupportTests

    Public Sub New()
        ' Reset NativeMmolL to default before each test
        NativeMmolL = False
    End Sub

    <Fact>
    Public Sub GetBgUnits_ReturnsCorrectUnits()
        NativeMmolL = True
        BgUnits.Should().Be(expected:="Mmol/l")
        NativeMmolL = False
        BgUnits.Should().Be(expected:="mg/dL")
    End Sub

    <Fact>
    Public Sub GetPrecisionDigits_ReturnsCorrectDigits()
        NativeMmolL = True
        GetPrecisionDigits().Should().Be(expected:=2)
        NativeMmolL = False
        GetPrecisionDigits().Should().Be(expected:=0)
    End Sub

    <Fact>
    Public Sub GetSgFormat_WithoutSign_ReturnsCorrectFormat()
        NativeMmolL = True
        GetSgFormat().Should().Be(expected:="F1")
        NativeMmolL = False
        GetSgFormat().Should().Be(expected:="F0")
    End Sub

    <Fact>
    Public Sub GetSgFormat_WithoutSign_ReturnsCorrectFormat_WithCulture()
        Dim separator As String =
            CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
        NativeMmolL = True
        GetSgFormat(withSign:=False).Should().Be(expected:=$"0{separator}0")
        NativeMmolL = False
        GetSgFormat(withSign:=False).Should().Be(expected:="0")
        RestoreDefaults()
    End Sub

    <Fact>
    Public Sub GetSgFormat_WithSign_ReturnsCorrectFormat()
        Dim separator As String =
            CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
        NativeMmolL = True
        GetSgFormat(withSign:=True).Should().Be(expected:=$"+0{separator}0;-#{separator}0")
        NativeMmolL = False
        GetSgFormat(withSign:=True).Should().Be(expected:="+0;-#")
    End Sub

End Class
