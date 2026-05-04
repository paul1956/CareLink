' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports CareLink.DateTimeExtensions
Imports FluentAssertions
Imports Xunit

Public Class DateHelperTests

    <Fact>
    Public Sub IsDateOlderThan_Returns_True_When_Date_Is_Older_Than_Span()
        ' Arrange
        Dim referenceDate As Date = Date.Now
        Dim dateTime As Date = referenceDate.AddMinutes(-11)
        Dim span As TimeSpan = TimeSpan.FromMinutes(10)

        ' Act
        Dim result As Boolean = dateTime.IsDateOlderThan(referenceDate, span)

        ' Assert
        result.Should().BeTrue()
    End Sub

    <Fact>
    Public Sub IsDateOlderThan_Returns_False_When_Date_Is_Exactly_Span_Old()
        ' Arrange
        Dim referenceDate As Date = Date.Now
        Dim dateTime As Date = referenceDate.AddMinutes(-10)
        Dim span As TimeSpan = TimeSpan.FromMinutes(10)

        ' Act
        Dim result As Boolean = dateTime.IsDateOlderThan(referenceDate, span)

        ' Assert
        result.Should().BeFalse()
    End Sub

    <Fact>
    Public Sub IsDateOlderThan_Returns_False_When_Date_Is_Newer_Than_Span()
        ' Arrange
        Dim referenceDate As Date = Date.Now
        Dim dateTime As Date = referenceDate.AddMinutes(-9)
        Dim span As TimeSpan = TimeSpan.FromMinutes(10)

        ' Act
        Dim result As Boolean = dateTime.IsDateOlderThan(referenceDate, span)

        ' Assert
        result.Should().BeFalse()
    End Sub

End Class
