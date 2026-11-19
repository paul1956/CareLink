' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Globalization
Imports CareLink
Imports FluentAssertions
Imports Xunit

<Collection("Sequential")>
Public Class MathExtensionsTests

    Public Sub New()
        ' Ensure default state
        NativeMmolL = False
    End Sub

    <Fact>
    Public Sub AlmostZero_DoubleAndSingle_BehaveAsExpected()
        ' Arrange
        Dim smallDouble As Double = 0.0000005
        Dim largeDouble As Double = 0.000002
        Dim smallSingle As Single = 0.0000005F
        Dim largeSingle As Single = 0.000002F

        ' Act / Assert
        smallDouble.AlmostZero().Should().BeTrue()
        largeDouble.AlmostZero().Should().BeFalse()
        smallSingle.AlmostZero().Should().BeTrue()
        largeSingle.AlmostZero().Should().BeFalse()
    End Sub

    <Fact>
    Public Sub AlmostZero_IncludesToleranceBoundaryAndRejectsSpecialValues()
        Dim tolerance As Double = MathExtensions.Tolerance
        Dim negativeTolerance As Double = -tolerance
        Dim beyondTolerance As Double = tolerance + 0.0000001

        tolerance.AlmostZero().Should().BeTrue()
        negativeTolerance.AlmostZero().Should().BeTrue()
        beyondTolerance.AlmostZero().Should().BeFalse()
        Double.NaN.AlmostZero().Should().BeFalse()
        Double.PositiveInfinity.AlmostZero().Should().BeFalse()

        Dim singleTolerance As Single = MathExtensions.Tolerance
        Dim negativeSingleTolerance As Single = -singleTolerance
        singleTolerance.AlmostZero().Should().BeTrue()
        negativeSingleTolerance.AlmostZero().Should().BeTrue()
        Single.PositiveInfinity.AlmostZero().Should().BeFalse()
    End Sub

    <Fact>
    Public Sub FormatWithTrailingZero_RoundsToThreePlacesAndFormatsIntegers()
        FormatWithTrailingZero(f:=1.2346F).Should().Be(expected:="1.235")
        FormatWithTrailingZero(f:=2.5F).Should().Be(expected:="2.5")
        FormatWithTrailingZero(f:=42.0F).Should().Be(expected:="42.0")
    End Sub

    <Fact>
    Public Sub FractionalPart_ReturnsFractionOfDecimal()
        ' Arrange
        Dim d As Decimal = 3.75D

        ' Act
        Dim actual As Single = d.FractionalPart()

        ' Assert
        actual.Should().BeApproximately(expectedValue:=0.75F, precision:=0.00001F)
    End Sub

    <Fact>
    Public Sub FractionalPart_ReturnsPositiveFractionForNegativeDecimal()
        Dim value As Decimal = -3.25D

        value.FractionalPart().Should().Be(expected:=0.75F)
    End Sub

    <Fact>
    Public Sub IsEqual_RejectsIntegersOutsideExactSingleRange()
        Dim maxExactInteger As Integer = 16777216
        Dim negativeMaxExactInteger As Integer = -maxExactInteger
        Dim aboveMaxExactInteger As Integer = maxExactInteger + 1
        Dim belowMaxExactInteger As Integer = -maxExactInteger - 1
        Dim singleAtMaximum As Single = CSng(maxExactInteger)
        Dim negativeSingleAtMaximum As Single = -singleAtMaximum

        singleAtMaximum.IsEqual(integerValue:=maxExactInteger).Should().BeTrue()
        negativeSingleAtMaximum.IsEqual(integerValue:=negativeMaxExactInteger).Should().BeTrue()
        singleAtMaximum.IsEqual(integerValue:=aboveMaxExactInteger).Should().BeFalse()
        negativeSingleAtMaximum.IsEqual(integerValue:=belowMaxExactInteger).Should().BeFalse()
    End Sub

    <Fact>
    Public Sub IsSgInvalid_And_IsSgValid_Work()
        ' NaN and non-positive are invalid
        Dim nan As Single = Single.NaN
        Dim infinity As Single = Single.PositiveInfinity
        Dim zero As Single = 0.0F
        Dim negative As Single = -1.0F
        Dim positive As Single = 1.0F

        nan.IsSgInvalid().Should().BeTrue()
        infinity.IsSgInvalid().Should().BeTrue()
        zero.IsSgInvalid().Should().BeTrue()
        negative.IsSgInvalid().Should().BeTrue()
        positive.IsSgInvalid().Should().BeFalse()

        positive.IsSgValid().Should().BeTrue()
        zero.IsSgValid().Should().BeFalse()
    End Sub

    <Fact>
    Public Sub IsSgRecordValid_RequiresValidGlucoseOrTimestamp()
        Dim sg As New SG

        sg.IsSgRecordValid().Should().BeFalse()

        sg.Sg = 120.0F
        sg.IsSgRecordValid().Should().BeTrue()

        sg.Sg = 0.0F
        sg.TimestampAsString = "2024-01-02T03:04:05"
        sg.IsSgRecordValid().Should().BeTrue()
    End Sub

    <Fact>
    Public Sub IsSingleEqualToInteger_BasicCases()
        Dim exact As Single = 100.0F
        Dim nearlyExact As Single = 100.0F
        Dim tooFar As Single = 100.0002F

        exact.IsEqual(integerValue:=100).Should().BeTrue()
        nearlyExact.IsEqual(integerValue:=100).Should().BeTrue()
        tooFar.IsEqual(integerValue:=100).Should().BeFalse()
    End Sub

    <Fact>
    Public Sub ParseAsSingle_ReturnsNaNForNullOrWhitespaceAndRejectsUnsupportedTypes()
        Dim missing As Object = Nothing
        Dim blank As Object = "  "
        Dim numericString As Object = "12.5"
        Dim unsupported As Object = Date.MinValue

        Assert.True(condition:=Single.IsNaN(f:=ParseAsSingle(obj:=missing)))
        Assert.True(condition:=Single.IsNaN(f:=ParseAsSingle(obj:=blank)))
        ParseAsSingle(obj:=numericString).Should().Be(expected:=12.5F)
        Assert.Throws(Of ArgumentOutOfRangeException)(
            testCode:=Sub() ParseAsSingle(obj:=unsupported))
    End Sub

    <Fact>
    Public Sub ParseSingle_NormalizesDecimalCommaAndReturnsNaNForInvalidInput()
        Dim whitespace As String = "  "
        Dim decimalComma As String = "1,25"
        Dim invalid As String = "not a number"
        Dim ambiguous As String = "1,2.3"

        Assert.True(condition:=Single.IsNaN(f:=ParseSingle(s:=Nothing)))
        Assert.True(condition:=Single.IsNaN(f:=ParseSingle(s:=whitespace)))
        ParseSingle(s:=decimalComma).Should().Be(expected:=1.25F)
        Assert.True(condition:=Single.IsNaN(f:=ParseSingle(s:=invalid)))
        Assert.Throws(Of ArgumentException)(testCode:=Sub()
                                                          ParseSingle(s:=ambiguous)
                                                      End Sub)
    End Sub

    <Fact>
    Public Sub ParseSingle_StringAndObject_ParsesAndRounds()
        ' Simple string
        Dim s As String = "100"
        s.ParseSingle().RoundSg().Should().Be(expected:=100.0F)

        ' String with rounding to 3 -> uses 0.025 increments
        Dim s2 As String = "1.0375"
        Dim parsed2 As Single = s2.ParseSingle().RoundToStep(flex:=False)
        parsed2.Should().BeApproximately(expectedValue:=1.05F, precision:=0.0001F)

        ' Object overloads
        Dim fromSingle As Object = 2.345F
        Dim fromDouble As Object = 2.345
        Dim fromDecimal As Object = CDec(2.345)

        ParseAsSingle(obj:=fromSingle).RoundSg().Should().BeApproximately(expectedValue:=2.34F, precision:=0.01)
        ParseAsSingle(obj:=fromDouble).RoundSg().Should().BeApproximately(expectedValue:=2.34F, precision:=0.01)
        ParseAsSingle(obj:=fromDecimal).RoundSg().Should().BeApproximately(expectedValue:=2.34F, precision:=0.01)
    End Sub

    <Fact>
    Public Sub RoundSg_UsesPrecisionBasedOnMagnitude()
        Dim smallValue As Single = 9.876F
        Dim largeValue As Single = 10.06F
        Dim negativeValue As Single = -2.346F

        smallValue.RoundSg().Should().Be(expected:=9.88F)
        largeValue.RoundSg().Should().Be(expected:=10.1F)
        negativeValue.RoundSg().Should().Be(expected:=-2.35F)
    End Sub

    <Fact>
    Public Sub RoundTo025_SingleAndDouble_And_NaN()
        Dim s As Single = 1.0375F
        Dim expectedValue As Single = 1.05F
        s.RoundToStep(flex:=False).Should().BeApproximately(expectedValue, precision:=0.0001F)

        Dim d As Double = 1.0375
        d.RoundToStep(flex:=False).Should().BeApproximately(expectedValue, precision:=0.0001F)

        Dim nanS As Single = Single.NaN
        Assert.True(condition:=Single.IsNaN(nanS.RoundToStep(flex:=False)))

        Dim nanD As Double = Double.NaN
        Assert.True(condition:=Double.IsNaN(nanD.RoundToStep(flex:=False)))
    End Sub

    <Fact>
    Public Sub RoundToSingle_WithDigits_RoundsToDecimalPlaces()
        ' Arrange
        Dim value As Single = 1.2345F
        Dim expected As Single = 1.23F

        ' Act
        Dim actual As Single = value.RoundToSingle(digits:=2)

        ' Assert
        actual.Should().Be(expected)
        Dim largerValue As Single = 12.345F
        Dim negativeValue As Single = -1.2345F
        Dim nan As Single = Single.NaN
        largerValue.RoundToSingle(digits:=2).Should().Be(expected:=12.35F)
        negativeValue.RoundToSingle(digits:=2).Should().Be(expected:=-1.23F)
        Assert.True(condition:=Single.IsNaN(f:=nan.RoundToSingle(digits:=2)))
    End Sub

    <Fact>
    Public Sub RoundToSingle_WithDigits3_Uses025Increment()
        ' Arrange
        Dim value As Single = 1.0375F
        Dim expected As Single = 1.05F

        ' Act
        Dim actual As Single = value.RoundToStep(flex:=False)

        ' Assert
        actual.Should().BeApproximately(expectedValue:=expected, precision:=0.0001F)
    End Sub

    <Fact>
    Public Sub ToCommaAndPeriodDelimited_UseExpectedSeparators()
        Dim originalCulture As CultureInfo = CultureInfo.CurrentCulture

        Try
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture
            ToCommaDelimited(f:=12.5F).Should().Be(expected:="12,5")
            ToPeriodDelimited(f:=12.5F).Should().Be(expected:="12.5")
        Finally
            CultureInfo.CurrentCulture = originalCulture
        End Try
    End Sub

End Class
