' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports Xunit
Imports FluentAssertions
Imports CareLink

Public Class TrendHelperTests

    <Fact>
    Public Sub GetTrendText_Should_Handle_SingleArrowCorrectly()
        ' Arrange
        Dim arrows As String = "↑" ' single up arrow

        ' Act
        Dim result As String = GetTrendText(arrows)

        ' Assert
        result.Should().Be(expected:=" and is trending up with 1 arrow")
    End Sub

    <Fact>
    Public Sub GetTrendText_Should_Return_NoTrendArrows_When_ArrowsContainNone()
        ' Arrange
        Dim arrows As String = "--" ' no arrows

        ' Act
        Dim result As String = GetTrendText(arrows)

        ' Assert
        result.Should().Be(expected:=" with no trend arrows")
    End Sub

    <Fact>
    Public Sub GetTrendText_Should_Return_TrendingDown_When_ArrowsContainDown()
        ' Arrange
        Dim arrows As String = "↓↓↓" ' 3 down arrows

        ' Act
        Dim result As String = GetTrendText(arrows)

        ' Assert
        result.Should().Be(expected:=" and is trending down with 3 arrows")
    End Sub

    <Fact>
    Public Sub GetTrendText_Should_Return_TrendingUp_When_ArrowsContainUp()
        ' Arrange
        Dim arrows As String = "↑↑" ' 2 up arrows

        ' Act
        Dim result As String = GetTrendText(arrows)

        ' Assert
        result.Should().Be(expected:=" and is trending up with 2 arrows")
    End Sub

End Class
