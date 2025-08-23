' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports CareLink
Imports FluentAssertions
Imports Xunit

Public Class StringExtensionsTests

    <Fact>
    Public Sub CleanSpaces_RemovesExtraSpacesAndTrims()
        Dim input As String = "  This   is   a   test   "

        Dim result As String = input.CleanSpaces()
        result.Should().Be(expected:="This is a test")

        input = ""
        result = input.CleanSpaces()
        result.Should().Be(expected:="")
    End Sub

    <Fact>
    Public Sub ContainsIgnoreCase_NothingOrStringEmptyAsInput()
        Dim input As String = Nothing

        Dim result As Boolean = ContainsNoCase(input, value:="world")
        result.Should().BeFalse()

        input = "Hello World"
        result = ContainsNoCase(input, value:="")
        result.Should().BeTrue()
    End Sub

    <Fact>
    Public Sub ContainsIgnoreCase_ReturnsTrueIfContains()
        Dim input As String = "Hello World"

        Dim result As Boolean = input.ContainsNoCase(value:="world")

        result.Should().BeTrue()
    End Sub

    <Fact>
    Public Sub Count_ReturnsCorrectCount()
        Dim input As String = "banana"

        Dim c As Char = "a"c
        Dim result As Integer = input.Count(c)
        result.Should().Be(expected:=3)

        c = "x"c
        result = input.Count(c)
        result.Should().Be(expected:=0)
    End Sub

    <Fact>
    Public Sub EndsWithIgnoreCase_ReturnsTrueIfEndsWith()
        Dim input As String = "Hello World"
        Dim value As String = "WORLD"

        Dim result As Boolean = input.EndsWithNoCase(value)

        result.Should().BeTrue()
    End Sub

    <Fact>
    Public Sub EqualsIgnoreCase_ReturnsTrueIfEqual()
        Dim a As String = "test"
        Dim b As String = "TEST"

        Dim result As Boolean = a.EqualsNoCase(b)

        result.Should().BeTrue()
    End Sub

    <Fact>
    Public Sub FindIndexOfAnyChar_FindsFirstOccurrence()
        Dim input As String = "abcdefg"
        Dim chars As New List(Of Char) From {"d"c, "e"c}
        Dim expected As Integer = 3

        Dim result As Integer = input.FindIndexOfAnyChar(chars, startIndex:=0)

        result.Should().Be(expected)
    End Sub

    <Fact>
    Public Sub FindIndexOfAnyChar_ShouldReturnExpectedIndex_ForNothingFound()
        ' Arrange
        Dim inputString As String = "abc"
        Dim chars As New List(Of Char) From {"y"c, "z"c}
        Dim startIndex As Integer = 0

        ' Act
        Dim result As Integer = FindIndexOfAnyChar(inputString, chars, startIndex)

        ' Assert
        result.Should().Be(expected:=-1)
    End Sub

    <Fact>
    Public Sub FindIndexOfAnyChar_ShouldReturnExpectedIndex_ForValidInputs()
        ' Arrange
        Dim inputString As String = "abc"
        Dim chars As New List(Of Char) From {"b"c, "c"c}
        Dim startIndex As Integer = 0

        ' Act
        Dim result As Integer = FindIndexOfAnyChar(inputString, chars, startIndex)

        ' Assert
        result.Should().Be(expected:=1)
    End Sub

    <Fact>
    Public Sub FindIndexOfAnyChar_ShouldReturnMinus1_WhenInputStringIsNothing()
        ' Arrange
        Dim chars As New List(Of Char) From {"a"c, "b"c}

        ' Act
        Dim result As Integer = FindIndexOfAnyChar(inputString:=Nothing, chars, startIndex:=0)

        ' Assert
        result.Should().Be(expected:=-1)
    End Sub

    <Fact>
    Public Sub FindIndexOfAnyChar_ShouldThrow_WhenCharsIsNothing()
        ' Act
        Dim act As Action = Sub()
                                FindIndexOfAnyChar(
                                    inputString:="abc",
                                    chars:=Nothing,
                                    startIndex:=0)
                            End Sub

        ' Assert
        act.Should() _
           .Throw(Of ArgumentException)() _
           .WithMessage(expectedWildcardPattern:="Invalid input parameters.")
    End Sub

    <Fact>
    Public Sub FindIndexOfAnyChar_ShouldThrow_WhenStartIndexIsGreaterThanOrEqualLength()
        ' Arrange
        Dim inputString As String = "abc"
        Dim chars As New List(Of Char) From {"a"c, "b"c}
        Dim startIndex As Integer = inputString.Length

        ' Act
        Dim act As Action = Sub()
                                FindIndexOfAnyChar(inputString, chars, startIndex)
                            End Sub

        ' Assert
        act.Should() _
           .Throw(Of ArgumentException)() _
           .WithMessage(expectedWildcardPattern:="Invalid input parameters.")
    End Sub

    <Fact>
    Public Sub FindIndexOfAnyChar_ShouldThrow_WhenStartIndexIsNegative()
        ' Arrange
        Dim chars As New List(Of Char) From {"a"c, "b"c}

        ' Act
        Dim act As Action = Sub()
                                FindIndexOfAnyChar(inputString:="abc", chars, startIndex:=-1)
                            End Sub

        ' Assert
        act.Should() _
           .Throw(Of ArgumentException)() _
           .WithMessage(expectedWildcardPattern:="Invalid input parameters.")
    End Sub

    <Fact>
    Public Sub IndexOfIgnoreCase_ReturnsCorrectIndex()
        Dim input As String = "Hello World"
        Dim value As String = "world"
        Dim expected As Integer = 6

        Dim result As Integer = input.IndexOfNoCase(value)

        result.Should().Be(expected)
    End Sub

    <Fact>
    Public Sub ParseDoubleInvariant_ParsesCorrectly()
        Dim input As String = "123.45"
        Dim expectedValue As Double = 123.45

        Dim result As Double = input.ParseDoubleInvariant()

        result.Should().BeApproximately(expectedValue, precision:=0.0001)
    End Sub

    <Fact>
    Public Sub ParseSingleInvariant_ParsesCorrectly()
        Dim input As String = "123.45"
        Dim expectedValue As Single = 123.45F

        Dim result As Single = input.ParseSingleInvariant()

        result.Should().BeApproximately(expectedValue, precision:=0.0001F)
    End Sub

    <Fact>
    Public Sub Remove_RemovesSubstringIgnoreCase()
        Dim input As String = "Hello World"
        Dim s As String = "world"
        Dim expected As String = "Hello "

        Dim result As String = input.Remove(s)

        result.Should().Be(expected)
    End Sub

    <Fact>
    Public Sub ReplaceIgnoreCase_ReplacesSubstringIgnoreCase()
        Dim input As String = "Hello World"
        Dim oldValue As String = "world"
        Dim newValue As String = "Universe"
        Dim expected As String = "Hello Universe"

        Dim result As String = input.ReplaceNoCase(oldValue, newValue)

        result.Should().Be(expected)
    End Sub

    <Fact>
    Public Sub StartsWithIgnoreCase_ReturnsTrueIfStartsWith()
        Dim input As String = "Hello World"
        Dim value As String = "hello"

        Dim result As Boolean = input.StartsWithNoCase(value)

        result.Should().BeTrue()

        result = input.StartsWithNoCase(value:="world")
        result.Should().BeFalse()

    End Sub

    <Fact>
    Public Sub ToLowerCamelCase_ConvertsFirstLetterToLower()
        Dim input As String = "HelloWorld"
        Dim result As String = input.ToLowerCamelCase()
        result.Should().Be(expected:="helloWorld")

        input = ""
        result = input.ToLowerCamelCase()
        result.Should().Be(expected:="")
    End Sub

    <Fact>
    Public Sub ToTimeUnits_With_1_Unit_Returns_Singular()
        ' Act
        Dim result As String = 1UI.ToUnits(Unit:="hour")

        ' Assert
        result.Should().Be(expected:="1 hour")
    End Sub

    <Fact>
    Public Sub ToTimeUnits_With_2_Units_Returns_Plural()
        ' Act
        Dim result As String = 2UI.ToUnits(Unit:="hour")

        ' Assert
        result.Should().Be(expected:="2 hours")
    End Sub

    <Fact>
    Public Sub ToTitle_ConvertsFirstLetterToUpperAndOthersToLower()
        Dim input As String = "hello"
        Dim result As String = input.ToTitle()
        result.Should().Be(expected:="Hello")
    End Sub

    <Fact>
    Public Sub ToTitle_HandlesSpacesAndCapitalizesNextLetter()
        Dim input As String = "hello world"
        Dim result As String = input.ToTitle()
        ' "hello world" -> "Hello World"
        result.Should().Be(expected:="Hello World")
    End Sub

    <Fact>
    Public Sub ToTitle_HandlesUnderscoresAndCapitalizesNextLetter()
        Dim input As String = "hello_world"
        Dim result As String = input.ToTitle()
        ' underscore replaced by space and next letter capitalized
        result.Should().Be(expected:="Hello World")
    End Sub

    <Fact>
    Public Sub ToTitle_ReplacesBgAndSgAsUppercase()
        Dim input As String = "hello bg and sg test"
        Dim result As String = input.ToTitle()
        ' "bg " should become "BG ", "sg " should become "SG "
        result.Should().Be(expected:="Hello BG And SG Test")
    End Sub

    <Fact>
    Public Sub ToTitle_ReturnsEmptyString_WhenInputIsNullOrWhitespace()
        Dim input As String = "   "
        Dim result As String = input.ToTitle()
        result.Should().BeEmpty()
    End Sub

    <Fact>
    Public Sub ToTitle_SeparateNumbersDoesNotSeparate_WhenSeparateNumbersIsFalse()
        Dim input As String = "test1test"
        Dim result As String = input.ToTitle(separateDigits:=False)
        ' Number is treated like a normal char, no space inserted
        result.Should().Be(expected:="Test1test")
    End Sub

    <Fact>
    Public Sub ToTitle_SeparateNumbersInsertsSpaceAndLowercasesDigit_WhenSeparateNumbersIsTrue()
        Dim input As String = "test1test"
        Dim result As String = input.ToTitle(separateDigits:=True)
        ' Expects a space before and after the number
        result.Should().Be(expected:="Test 1 Test")
    End Sub

    <Fact>
    Public Sub ToTitle_SeparateNumberInsertsSpaceLowerDigit_WhenSeparateNumbersFirstCharDigit()
        Dim input As String = "1test"
        Dim result As String = input.ToTitle(separateDigits:=True)
        result.Should().Be(expected:="1 Test")
    End Sub

    <Fact>
    Public Sub ToTitleCase_AppendsTime()
        Dim input As String = "myTime"
        input.ToTitleCase().Should().Be(expected:="My Time")
    End Sub

    <Fact>
    Public Sub ToTitleCase_DoesNotDuplicateTrademark()
        Dim input As String = "carelink™Test"
        input.ToTitleCase().Should().Be(expected:="CareLink™ Test")
    End Sub

    <Fact>
    Public Sub ToTitleCase_ElseIfAndElse_Branches_AreHit()
        ' Test for the core logic inside the For Each loop

        ' Case 1: Next char is lower OR lastWasNumeric --> branch 1
        Dim input As String = "ab"

        ' a (upper), b (lower) triggers first If
        input.ToTitleCase().Should().Be(expected:="Ab")

        ' Case 2: Next char is number AND separateNumbers = False --> ElseIf
        ' a (upper), 1 (ElseIf, since it's a digit and separateNumbers=False)
        input = "a1b"
        input.ToTitleCase(separateNumbers:=False).Should().Be(expected:="A1B")

        ' Case 3: Else branch (uppercase letter after previous char not number)
        ' a (upper), B triggers Else since not lower/not lastWasNumeric/not number
        input = "aB"
        input.ToTitleCase().Should().Be(expected:="A B")

        ' Case 4: Else branch (number with separateNumbers=True)
        ' A (upper), 1 (Else, since separateNumbers=True), B triggers another Else
        input = "A1B"
        input.ToTitleCase(separateNumbers:=True).Should().Be(expected:="A 1 B")

        ' Multiple transitions: covers else and elseif
        input = "A1bC"
        input.ToTitleCase(separateNumbers:=False).Should().Be(expected:="A1B C")
    End Sub

    <Fact>
    Public Sub ToTitleCase_ReturnsEmpty_ForNullOrWhitespace()
        Dim expected As String = String.Empty
        DirectCast(Nothing, String).ToTitleCase().Should().Be(expected)

        Dim input As String = "   "
        input.ToTitleCase().Should().Be(expected)
    End Sub

    <Fact>
    Public Sub ToTitleCase_ReturnsInput_WhenContainsMmolL()
        Dim input As String = "abcMmolLxyz"
        input.ToTitleCase().Should().Be(expected:="abcMmolLxyz")

        input = "MMOLl"
        input.ToTitleCase().Should().Be(expected:="MMOLl")
    End Sub

    <Fact>
    Public Sub TruncateSingle_WithDecimal_ShortensToDigits()
        Dim expression As String = "123.45678"
        Dim expected As String = "123.45"
        Dim digits As Integer = 2

        Dim result As String = TruncateSingle(expression, digits)

        result.Should().Be(expected)
    End Sub

    <Fact>
    Public Sub TruncateSingle_WithNonNumeric_ReturnsOriginal()
        Dim expression As String = "abc"
        Dim expected As String = "abc"
        Dim digits As Integer = 2

        Dim result As String = TruncateSingle(expression, digits)

        result.Should().Be(expected)
    End Sub

    <Fact>
    Public Sub TruncateSingle_WithoutDecimal_AppendsDecimalAndZeros()
        Dim expression As String = "10"
        Dim expected As String = "10.00"
        Dim digits As Integer = 2
        Dim result As String = TruncateSingle(expression, digits)

        result.Should().Be(expected)
    End Sub

End Class
