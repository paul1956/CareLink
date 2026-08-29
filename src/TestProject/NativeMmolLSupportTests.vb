' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports CareLink
Imports FluentAssertions
Imports Xunit

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
        GetSgFormat(nativeMmolL:=True).Should().Be(expected:="0.0")
        GetSgFormat(nativeMmolL:=False).Should().Be(expected:="0")
    End Sub

    <Fact>
    Public Sub GetSgFormat_WithoutSign_ReturnsCorrectFormat_WithCulture()
        GetSgFormat(nativeMmolL:=True, withSign:=False).Should().Be(expected:="0.0")
        GetSgFormat(nativeMmolL:=False, withSign:=False).Should().Be(expected:="0")
        RestoreDefaults()
    End Sub

    <Fact>
    Public Sub GetSgFormat_WithSign_ReturnsCorrectFormat()
        GetSgFormat(nativeMmolL:=True, withSign:=True).Should().Be(expected:="+0.0;-0.0;0.0")
        GetSgFormat(nativeMmolL:=False, withSign:=True).Should().Be(expected:="+0;-0;0")
    End Sub

    <Fact>
    Public Sub GetSgFormat_UsesCurrentUICultureDecimalSeparator()
        Dim oldUi As CultureInfo = CultureInfo.CurrentUICulture
        Try
            ' Use a culture with a different decimal separator (comma)
            Dim testUi As New CultureInfo("fr-FR")
            Threading.Thread.CurrentThread.CurrentUICulture = testUi

            GetSgFormat(nativeMmolL:=True, withSign:=False).Should().Be(expected:="0.0")
            GetSgFormat(nativeMmolL:=True, withSign:=True).Should().Be(expected:="+0.0;-0.0;0.0")

            GetSgFormat(nativeMmolL:=False, withSign:=False).Should().Be(expected:="0")
            GetSgFormat(nativeMmolL:=False, withSign:=True).Should().Be(expected:="+0;-0;0")
        Finally
            Threading.Thread.CurrentThread.CurrentUICulture = oldUi
            RestoreDefaults()
        End Try
    End Sub

End Class
