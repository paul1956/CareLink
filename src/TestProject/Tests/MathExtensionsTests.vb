' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

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
    Public Sub RoundToSingle_WithDigits3_Uses025Increment()
        ' Arrange
        Dim value As Single = 1.0375F
        Dim expected As Single = 1.05F

        ' Act
        Dim actual As Single = value.RoundToStep(isFlex:=False)

        ' Assert
        actual.Should().BeApproximately(expectedValue:=expected, precision:=0.0001F)
    End Sub

    <Fact>
    Public Sub RoundToSingle_WithDigits_RoundsNormally()
        ' Arrange
        Dim value As Single = 1.2345F
        Dim expected As Single = 1.23F

        ' Act
        Dim actual As Single = value.RoundSg()

        ' Assert
        actual.Should().Be(expected)
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
    Public Sub FractionalPart_ReturnsFractionOfDecimal()
        ' Arrange
        Dim d As Decimal = 3.75D

        ' Act
        Dim actual As Single = d.FractionalPart()

        ' Assert
        actual.Should().BeApproximately(0.75F, 0.00001F)
    End Sub

    <Fact>
    Public Sub IsSgInvalid_And_IsSgValid_Work()
        ' NaN and non-positive are invalid
        Dim nan As Single = Single.NaN
        Dim zero As Single = 0.0F
        Dim negative As Single = -1.0F
        Dim positive As Single = 1.0F

        nan.IsSgInvalid().Should().BeTrue()
        zero.IsSgInvalid().Should().BeTrue()
        negative.IsSgInvalid().Should().BeTrue()
        positive.IsSgInvalid().Should().BeFalse()

        positive.IsSgValid().Should().BeTrue()
        zero.IsSgValid().Should().BeFalse()
    End Sub

    <Fact>
    Public Sub IsSingleEqualToInteger_BasicCases()
        Dim exact As Single = 100.0F
        Dim nearlyExact As Single = 100.0F
        Dim tooFar As Single = 100.0002F

        exact.IsSingleEqualToInteger(100).Should().BeTrue()
        nearlyExact.IsSingleEqualToInteger(100).Should().BeTrue()
        tooFar.IsSingleEqualToInteger(100).Should().BeFalse()
    End Sub

    <Fact>
    Public Sub ParseSingle_StringAndObject_ParsesAndRounds()
        ' Simple string
        Dim s As String = "100"
        s.ParseSingle().RoundSg().Should().Be(expected:=100.0F)

        ' String with rounding to 3 -> uses 0.025 increments
        Dim s2 As String = "1.0375"
        Dim parsed2 As Single = s2.ParseSingle().RoundToStep(isFlex:=False)
        parsed2.Should().BeApproximately(expectedValue:=1.05F, precision:=0.0001F)

        ' Object overloads
        Dim fromSingle As Object = 2.345F
        Dim fromDouble As Object = 2.345
        Dim fromDecimal As Object = CDec(2.345)

        ParseAsSingle(value:=fromSingle).RoundSg().Should().BeApproximately(expectedValue:=2.34F, precision:=0.01)
        ParseAsSingle(value:=fromDouble).RoundSg().Should().BeApproximately(expectedValue:=2.34F, precision:=0.01)
        ParseAsSingle(value:=fromDecimal).RoundSg().Should().BeApproximately(expectedValue:=2.34F, precision:=0.01)
    End Sub

    <Fact>
    Public Sub RoundTo025_SingleAndDouble_And_NaN()
        Dim s As Single = 1.0375F
        Dim expectedValue As Single = 1.05F
        s.RoundToStep(isFlex:=False).Should().BeApproximately(expectedValue, precision:=0.0001F)

        Dim d As Double = 1.0375
        d.RoundToStep(isFlex:=False).Should().BeApproximately(expectedValue, precision:=0.0001F)

        Dim nanS As Single = Single.NaN
        Assert.True(condition:=Single.IsNaN(nanS.RoundToStep(isFlex:=False)))

        Dim nanD As Double = Double.NaN
        Assert.True(condition:=Double.IsNaN(nanD.RoundToStep(isFlex:=False)))
    End Sub

End Class
